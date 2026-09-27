namespace SrpLab;

public sealed class SubscriptionBilling
{
    public decimal CalculateProration(
        decimal monthlyPrice,
        int remainingDays,
        int daysInMonth)
    {
        return new ProrationCalculator()
            .Calculate(monthlyPrice, remainingDays, daysInMonth);
    }

    public string GenerateInvoiceNumber()
    {
        return new InvoiceNumberGenerator()
            .Generate();
    }

    public void TrackFailedPayment(string customerId)
    {
        new FailedPaymentTracker()
            .Track(customerId);
    }

    public string CreateDunningEmail(
        string customerName,
        decimal amount)
    {
        return new DunningEmailGenerator()
            .Generate(customerName, amount);
    }

    public string RecordAccountingEntry(
        string invoiceNumber,
        decimal amount)
    {
        return new AccountingLedger()
            .Record(invoiceNumber, amount);
    }
}


public sealed class ProrationCalculator
{
    public decimal Calculate(
        decimal monthlyPrice,
        int remainingDays,
        int daysInMonth)
    {
        if (monthlyPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(monthlyPrice));

        if (remainingDays < 0 || daysInMonth <= 0)
            throw new ArgumentOutOfRangeException();

        return monthlyPrice * remainingDays / daysInMonth;
    }
}


public sealed class InvoiceNumberGenerator
{
    public string Generate()
    {
        return $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}";
    }
}


public sealed class FailedPaymentTracker
{
    private readonly HashSet<string> _failedCustomers = new();

    public void Track(string customerId)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            throw new ArgumentException(
                "Customer ID cannot be empty.");

        _failedCustomers.Add(customerId);
    }

    public bool HasFailedPayment(string customerId)
    {
        return _failedCustomers.Contains(customerId);
    }
}


public sealed class DunningEmailGenerator
{
    public string Generate(
        string customerName,
        decimal amount)
    {
        return $"Hello {customerName},\n" +
               $"Your payment of {amount:C} failed. " +
               "Please update your payment method.";
    }
}


public sealed class AccountingLedger
{
    public string Record(
        string invoiceNumber,
        decimal amount)
    {
        return $"Ledger Entry: {invoiceNumber} - {amount:C}";
    }
}