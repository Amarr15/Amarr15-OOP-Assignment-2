namespace SrpLab;

public sealed class SupportTicket
{
    public string Title { get; }
    public string Description { get; }

    public SupportTicket(
        string title,
        string description)
    {
        Title = title;
        Description = description;
    }

    public string CalculatePriority()
    {
        return new TicketPriorityCalculator()
            .Calculate(Description);
    }

    public DateTime CalculateSlaDeadline(
        DateTime createdAt,
        string priority)
    {
        return new SlaDeadlineCalculator()
            .Calculate(createdAt, priority);
    }

    public bool IsSlaBreached(
        DateTime createdAt,
        string priority,
        DateTime currentTime)
    {
        var deadline = CalculateSlaDeadline(
            createdAt,
            priority);

        return currentTime > deadline;
    }

    public string CreateCustomerReply(
        string customerName)
    {
        return new CustomerReplyGenerator()
            .Generate(customerName, Title);
    }

    public string CreateEscalationMessage(
        string reason)
    {
        return new InternalEscalationGenerator()
            .Generate(Title, reason);
    }
}


public sealed class TicketPriorityCalculator
{
    public string Calculate(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return "Low";

        if (description.Contains(
                "urgent",
                StringComparison.OrdinalIgnoreCase) ||
            description.Contains(
                "down",
                StringComparison.OrdinalIgnoreCase))
        {
            return "High";
        }

        if (description.Contains(
                "error",
                StringComparison.OrdinalIgnoreCase))
        {
            return "Medium";
        }

        return "Low";
    }
}


public sealed class SlaDeadlineCalculator
{
    public DateTime Calculate(
        DateTime createdAt,
        string priority)
    {
        return priority.ToLower() switch
        {
            "high" => createdAt.AddHours(4),
            "medium" => createdAt.AddHours(12),
            _ => createdAt.AddDays(2)
        };
    }
}


public sealed class CustomerReplyGenerator
{
    public string Generate(
        string customerName,
        string ticketTitle)
    {
        return $"Hello {customerName},\n" +
               $"We received your ticket: {ticketTitle}.\n" +
               "Our support team will review it shortly.";
    }
}


public sealed class InternalEscalationGenerator
{
    public string Generate(
        string ticketTitle,
        string reason)
    {
        return $"ESCALATION\n" +
               $"Ticket: {ticketTitle}\n" +
               $"Reason: {reason}";
    }
}