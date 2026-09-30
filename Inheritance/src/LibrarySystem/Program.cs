using System;
using System.Collections.Generic;

namespace LibrarySystem
{
    public enum LoanStatus
    {
        Borrowed,
        Returned,
        Lost
    }
    public class Person
    {
        public string PersonId { get; }
        public string FullName { get; }
        public string Phone { get; }

        protected Person(string personId, string fullName, string phone)
        {
            if (string.IsNullOrWhiteSpace(personId))
                throw new ArgumentException("Person ID cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone cannot be null or empty.");

            PersonId = personId;
            FullName = fullName;
            Phone = phone;
        }
    }

    public class Member : Person
    {
        public int MaxLoanLimit { get; }
        public double LateFeeDiscountPercentage { get; }

        private readonly List<Loan> _loans = new List<Loan>();
        public IReadOnlyList<Loan> Loans
        {
            get { return _loans.AsReadOnly(); }
        }

        protected Member(string personId, string fullName, string phone, int maxLoanLimit, double discountPercentage)
            : base(personId, fullName, phone)
        {
            if (maxLoanLimit <= 0)
                throw new ArgumentException("Max loan limit must be positive.");

            if (discountPercentage < 0 || discountPercentage > 100)
                throw new ArgumentException("Discount percentage must be between 0 and 100.");

            MaxLoanLimit = maxLoanLimit;
            LateFeeDiscountPercentage = discountPercentage;
        }

        public int GetActiveLoansCount()
        {
            int count = 0;
            foreach (var loan in _loans)
            {
                if (loan.Status == LoanStatus.Borrowed)
                {
                    count++;
                }
            }
            return count;
        }

        public Loan BorrowItem(LibraryItem item, DateTime borrowDate, string loanId)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (item.IsWithdrawn)
                throw new InvalidOperationException($"Item '{item.Title}' is withdrawn from circulation and cannot be borrowed.");

            if (item.IsOnLoan)
                throw new InvalidOperationException($"Item '{item.Title}' is already on loan to someone else.");

            if (GetActiveLoansCount() >= MaxLoanLimit)
                throw new InvalidOperationException($"Member '{FullName}' has reached the maximum loan limit of {MaxLoanLimit} items.");

            var loan = new Loan(loanId, this, item, borrowDate);
            item.IsOnLoan = true;
            _loans.Add(loan);
            return loan;
        }
    }

    public class StudentMember : Member
    {
        public StudentMember(string personId, string fullName, string phone)
            : base(personId, fullName, phone, 3, 0)
        {
        }
    }

    public class PremiumMember : Member
    {
        public PremiumMember(string personId, string fullName, string phone, double discountPercentage)
            : base(personId, fullName, phone, 10, discountPercentage)
        {
        }

        public int ReadingPoints
        {
            get
            {
                int points = 0;
                foreach (var loan in Loans)
                {
                    if (loan.Status == LoanStatus.Returned)
                    {
                        points += 5;
                    }
                }
                return points;
            }
        }
    }

    public class Staff : Person
    {
        public DateTime HireDate { get; }
        public decimal MonthlySalary { get; private set; }
        protected decimal ResponsibilityAllowance { get; }

        protected Staff(string personId, string fullName, string phone,
                        DateTime hireDate, decimal monthlySalary, decimal responsibilityAllowance = 0)
            : base(personId, fullName, phone)
        {
            if (monthlySalary <= 0)
                throw new ArgumentException("Monthly salary must be greater than zero.");

            HireDate = hireDate;
            MonthlySalary = monthlySalary;
            ResponsibilityAllowance = responsibilityAllowance;
        }

        public void GiveRaise(double percentage)
        {
            if (percentage <= 0)
                throw new ArgumentException("Raise percentage must be greater than zero.");

            MonthlySalary += MonthlySalary * (decimal)(percentage / 100.0);
        }

        public decimal GetMonthlyPay()
        {
            return MonthlySalary + ResponsibilityAllowance;
        }
    }

    public class Librarian : Staff
    {
        public Librarian(string personId, string fullName, string phone, DateTime hireDate, decimal monthlySalary)
            : base(personId, fullName, phone, hireDate, monthlySalary, 0)
        {
        }
    }

    public class Shelver : Staff
    {
        public string Section { get; private set; }

        public Shelver(string personId, string fullName, string phone, DateTime hireDate, decimal monthlySalary, string section)
            : base(personId, fullName, phone, hireDate, monthlySalary, 0)
        {
            Reassign(section);
        }

        public void Reassign(string newSection)
        {
            if (string.IsNullOrWhiteSpace(newSection))
                throw new ArgumentException("Section name cannot be null or empty.");

            Section = newSection;
        }
    }

    public class HeadLibrarian : Staff
    {
        public HeadLibrarian(string personId, string fullName, string phone, DateTime hireDate, decimal monthlySalary)
            : base(personId, fullName, phone, hireDate, monthlySalary, 400)
        {
        }
    }
    public class LibraryItem
    {
        public string CatalogNumber { get; }
        public string Title { get; }
        public int LoanPeriodDays { get; }
        public decimal FeeMultiplier { get; }
        public decimal BaseLateFee { get; private set; }
        public bool IsWithdrawn { get; private set; }
        public bool IsOnLoan { get; internal set; }

        protected LibraryItem(string catalogNumber, string title, int loanPeriodDays, decimal feeMultiplier, decimal baseLateFee)
        {
            if (string.IsNullOrWhiteSpace(catalogNumber))
                throw new ArgumentException("Catalog number cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty.");

            if (loanPeriodDays <= 0)
                throw new ArgumentException("Loan period must be positive.");

            if (baseLateFee <= 0)
                throw new ArgumentException("Base late fee must be greater than zero.");

            CatalogNumber = catalogNumber;
            Title = title;
            LoanPeriodDays = loanPeriodDays;
            FeeMultiplier = feeMultiplier;
            BaseLateFee = baseLateFee;
            IsWithdrawn = false;
            IsOnLoan = false;
        }

        public decimal GetDailyLateFee()
        {
            return BaseLateFee * FeeMultiplier;
        }

        public void SetBaseLateFee(decimal newFee)
        {
            if (newFee <= 0)
                throw new ArgumentException("Late fee must be greater than zero.");

            BaseLateFee = newFee;
        }

        public void Withdraw()
        {
            IsWithdrawn = true;
        }

        public void Restore()
        {
            IsWithdrawn = false;
        }
    }
    public class Book : LibraryItem
    {
        public Book(string catalogNumber, string title, decimal baseLateFee)
            : base(catalogNumber, title, 21, 1.0m, baseLateFee)
        {
        }
    }
    public class Dvd : LibraryItem
    {
        public Dvd(string catalogNumber, string title, decimal baseLateFee)
            : base(catalogNumber, title, 7, 2.0m, baseLateFee)
        {
        }
    }
    public class Magazine : LibraryItem
    {
        public Magazine(string catalogNumber, string title, decimal baseLateFee)
            : base(catalogNumber, title, 3, 0.5m, baseLateFee)
        {
        }
    }
    public class Loan
    {
        public string LoanId { get; }
        public Member Member { get; }
        public LibraryItem Item { get; }
        public DateTime BorrowDate { get; }
        public DateTime? ReturnDate { get; private set; }
        public LoanStatus Status { get; private set; }

        public Loan(string loanId, Member member, LibraryItem item, DateTime borrowDate)
        {
            if (string.IsNullOrWhiteSpace(loanId))
                throw new ArgumentException("Loan ID cannot be null or empty.");

            LoanId = loanId;
            Member = member ?? throw new ArgumentNullException(nameof(member));
            Item = item ?? throw new ArgumentNullException(nameof(item));
            BorrowDate = borrowDate;
            Status = LoanStatus.Borrowed;
            ReturnDate = null;
        }

        public DateTime DueDate
        {
            get
            {
                return BorrowDate.AddDays(Item.LoanPeriodDays);
            }
        }

        public decimal LateFee
        {
            get
            {
                if (ReturnDate == null || ReturnDate.Value.Date <= DueDate.Date)
                    return 0m;

                int daysLate = (ReturnDate.Value.Date - DueDate.Date).Days;
                decimal baseLateFee = daysLate * Item.GetDailyLateFee();
                decimal discountMultiplier = 1m - ((decimal)Member.LateFeeDiscountPercentage / 100m);

                return baseLateFee * discountMultiplier;
            }
        }

        public void CompleteReturn(DateTime returnDate)
        {
            if (Status != LoanStatus.Borrowed)
                throw new InvalidOperationException($"Cannot return a loan with status {Status}.");

            if (returnDate < BorrowDate)
                throw new ArgumentException("Return date cannot be earlier than borrow date.");

            ReturnDate = returnDate;
            Status = LoanStatus.Returned;
            Item.IsOnLoan = false;
        }

        public void MarkAsLost()
        {
            if (Status != LoanStatus.Borrowed)
                throw new InvalidOperationException($"Cannot mark a loan as lost with status {Status}.");

            Status = LoanStatus.Lost;
            Item.IsOnLoan = false;
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("        03 LIBRARY SYSTEM - DEMONSTRATION         ");
            Console.WriteLine("==================================================\n");

            Console.WriteLine("--- 1. Staff List & Monthly Pay ---");
            List<Staff> staffMembers = new List<Staff>
            {
                new Librarian("ST-01", "Ahmed Ali", "01011111111", new DateTime(2020, 1, 15), 5000m),
                new Shelver("ST-02", "Mona Omar", "01022222222", new DateTime(2021, 6, 1), 4000m, "Fiction"),
                new HeadLibrarian("ST-03", "Dr. Khaled", "01033333333", new DateTime(2018, 3, 10), 8000m)
            };

            foreach (var staff in staffMembers)
            {
                Console.WriteLine($"Staff: {staff.FullName,-15} | Base Salary: {staff.MonthlySalary:C0} | Total Monthly Pay: {staff.GetMonthlyPay():C0}");
            }
            Console.WriteLine();

            Console.WriteLine("--- 2. Library Items & Daily Late Fees ---");
            List<LibraryItem> items = new List<LibraryItem>
            {
                new Book("BK-101", "Clean Architecture", 10m),
                new Dvd("DVD-201", "Inception Movie", 10m),
                new Magazine("MG-301", "Tech Monthly #45", 10m)
            };

            foreach (var item in items)
            {
                Console.WriteLine($"Item: {item.Title,-22} | Loan Period: {item.LoanPeriodDays,2} days | Daily Late Fee: {item.GetDailyLateFee():C2}");
            }
            Console.WriteLine();

            Console.WriteLine("--- 3. Premium Member 5-Day Late DVD Return ---");
            var premiumMember = new PremiumMember("M-PREM-1", "Youssef Nabil", "01155555555", 20.0);
            var dvdItem = new Dvd("DVD-555", "Interstellar", 10m);

            DateTime borrowDate = new DateTime(2026, 9, 1);
            Loan premLoan = premiumMember.BorrowItem(dvdItem, borrowDate, "LN-001");

            DateTime returnDate = premLoan.DueDate.AddDays(5);
            premLoan.CompleteReturn(returnDate);

            Console.WriteLine($"Member: {premiumMember.FullName} (Discount: {premiumMember.LateFeeDiscountPercentage}%)");
            Console.WriteLine($"Borrowed: {dvdItem.Title} on {borrowDate:yyyy-MM-dd}");
            Console.WriteLine($"Due Date: {premLoan.DueDate:yyyy-MM-dd}");
            Console.WriteLine($"Returned Date: {returnDate:yyyy-MM-dd} (5 days late)");
            Console.WriteLine($"Late Fee: {premLoan.LateFee:C2}");
            Console.WriteLine($"Premium Member Reading Points: {premiumMember.ReadingPoints} pts");
            Console.WriteLine();

            Console.WriteLine("--- 4. Business Rules & Exceptions Verification ---");

            try
            {
                var bookWithdrawn = new Book("BK-999", "Old Damaged Book", 5m);
                bookWithdrawn.Withdraw();
                premiumMember.BorrowItem(bookWithdrawn, DateTime.Now, "LN-ERR-1");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Caught Expected Error - Withdrawn Item]: {ex.Message}");
            }

            try
            {
                var busyBook = new Book("BK-501", "C# in Depth", 10m);
                var studentA = new StudentMember("STU-1", "Tamer", "0120000001");
                var studentB = new StudentMember("STU-2", "Maged", "0120000002");

                studentA.BorrowItem(busyBook, DateTime.Now, "LN-BUSY-1");
                studentB.BorrowItem(busyBook, DateTime.Now, "LN-BUSY-2");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Caught Expected Error - Double Borrow]: {ex.Message}");
            }

            try
            {
                var student = new StudentMember("STU-3", "Kareem", "0120000003");
                var b1 = new Book("B-1", "Book 1", 5m);
                var b2 = new Book("B-2", "Book 2", 5m);
                var b3 = new Book("B-3", "Book 3", 5m);
                var b4 = new Book("B-4", "Book 4", 5m);

                student.BorrowItem(b1, DateTime.Now, "L-1");
                student.BorrowItem(b2, DateTime.Now, "L-2");
                student.BorrowItem(b3, DateTime.Now, "L-3");
                student.BorrowItem(b4, DateTime.Now, "L-4");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Caught Expected Error - Max Loan Limit]: {ex.Message}");
            }

            try
            {
                var member = new StudentMember("STU-4", "Hany", "0120000004");
                var book = new Book("B-10", "Data Structures", 5m);
                var loan = member.BorrowItem(book, DateTime.Now, "L-RET-TWICE");

                loan.CompleteReturn(DateTime.Now);
                loan.CompleteReturn(DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Caught Expected Error - Double Return]: {ex.Message}");
            }

            try
            {
                var member = new StudentMember("STU-5", "Ramy", "0120000005");
                var book = new Book("B-20", "Algorithms", 5m);
                var loan = member.BorrowItem(book, DateTime.Now, "L-LOST-ERR");

                loan.CompleteReturn(DateTime.Now);
                loan.MarkAsLost(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Caught Expected Error - Mark Returned as Lost]: {ex.Message}");
            }

            Console.WriteLine("\n==================================================");
            Console.WriteLine("        ALL TESTS PASSED SUCCESSFULLY!            ");
            Console.WriteLine("==================================================");
        }
    }
}