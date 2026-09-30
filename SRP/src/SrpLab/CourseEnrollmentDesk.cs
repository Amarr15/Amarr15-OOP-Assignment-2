namespace SrpLab;

public sealed class CourseEnrollmentDesk
{
    private readonly string _courseCode;
    private readonly decimal _tuition;

    private readonly CourseRegistration _registration;
    private readonly CourseWaitlist _waitlist;

    public int Capacity { get; }

    public CourseEnrollmentDesk(
        string courseCode,
        int capacity,
        decimal tuition)
    {
        if (string.IsNullOrWhiteSpace(courseCode))
            throw new ArgumentException("Course code is required.", nameof(courseCode));

        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        if (tuition < 0)
            throw new ArgumentOutOfRangeException(nameof(tuition));

        _courseCode = courseCode;
        Capacity = capacity;
        _tuition = tuition;

        _registration = new CourseRegistration(capacity);
        _waitlist = new CourseWaitlist();
    }

    public bool Register(string studentEmail)
    {
        if (_registration.IsFull())
        {
            _waitlist.Add(studentEmail);
            return false;
        }

        return _registration.Register(studentEmail);
    }

    public string WelcomePacketMarkdown(
        string studentEmail,
        string studentName)
    {
        return new WelcomePacketGenerator()
            .Generate(
                _courseCode,
                studentEmail,
                studentName,
                _tuition);
    }
}


public sealed class CourseRegistration
{
    private readonly List<string> _students = new();
    private readonly int _capacity;

    public CourseRegistration(int capacity)
    {
        _capacity = capacity;
    }

    public bool IsFull()
    {
        return _students.Count >= _capacity;
    }

    public bool Register(string studentEmail)
    {
        if (IsFull())
            return false;

        _students.Add(studentEmail);

        return true;
    }
}


public sealed class CourseWaitlist
{
    private readonly List<string> _students = new();

    public void Add(string studentEmail)
    {
        _students.Add(studentEmail);
    }
}


public sealed class WelcomePacketGenerator
{
    public string Generate(
        string courseCode,
        string studentEmail,
        string studentName,
        decimal tuition)
    {
        return $"# Welcome {studentName}\n\n" +
               $"Course: {courseCode}\n" +
               $"Student: {studentEmail}\n" +
               $"Tuition: {tuition:C}";
    }
}


public sealed class CourseInvoiceCalculator
{
    public decimal Calculate(
        decimal tuition,
        decimal discount)
    {
        if (tuition < 0)
            throw new ArgumentOutOfRangeException(nameof(tuition));

        if (discount < 0)
            throw new ArgumentOutOfRangeException(nameof(discount));

        return Math.Max(0, tuition - discount);
    }
}