namespace SrpLab;

public sealed class LoanDesk
{
    public decimal CalculateRisk(
        decimal income,
        decimal debt)
    {
        return new LoanRiskCalculator()
            .Calculate(income, debt);
    }

    public bool IsEligible(
        decimal income,
        decimal debt)
    {
        return new LoanEligibilityChecker()
            .IsEligible(income, debt);
    }

    public IReadOnlyList<string> GetRequiredDocuments(
        bool isEmployee,
        bool isSelfEmployed)
    {
        return new LoanDocumentChecker()
            .GetRequiredDocuments(isEmployee, isSelfEmployed);
    }

    public string CreateDecisionLetter(
        string customerName,
        bool approved)
    {
        return new LoanDecisionLetter()
            .Generate(customerName, approved);
    }

    public string ExportCsv(
        string customerName,
        decimal income,
        decimal debt,
        bool approved)
    {
        return new LoanCsvExporter()
            .Export(customerName, income, debt, approved);
    }
}


public sealed class LoanRiskCalculator
{
    public decimal Calculate(
        decimal income,
        decimal debt)
    {
        if (income <= 0)
            throw new ArgumentOutOfRangeException(nameof(income));

        if (debt < 0)
            throw new ArgumentOutOfRangeException(nameof(debt));

        return debt / income;
    }
}


public sealed class LoanEligibilityChecker
{
    public bool IsEligible(
        decimal income,
        decimal debt)
    {
        if (income <= 0 || debt < 0)
            return false;

        var risk = debt / income;

        return risk <= 0.4m;
    }
}


public sealed class LoanDocumentChecker
{
    public IReadOnlyList<string> GetRequiredDocuments(
        bool isEmployee,
        bool isSelfEmployed)
    {
        var documents = new List<string>
        {
            "National ID"
        };

        if (isEmployee)
            documents.Add("Salary Certificate");

        if (isSelfEmployed)
            documents.Add("Business Registration");

        return documents;
    }
}


public sealed class LoanDecisionLetter
{
    public string Generate(
        string customerName,
        bool approved)
    {
        if (approved)
        {
            return $"Dear {customerName},\n" +
                   "Your loan application has been approved.";
        }

        return $"Dear {customerName},\n" +
               "Your loan application has been rejected.";
    }
}


public sealed class LoanCsvExporter
{
    public string Export(
        string customerName,
        decimal income,
        decimal debt,
        bool approved)
    {
        return "Customer,Income,Debt,Approved\n" +
               $"{customerName},{income},{debt},{approved}";
    }
}