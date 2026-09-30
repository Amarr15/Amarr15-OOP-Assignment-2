namespace SrpLab;

public sealed class KitchenTicket
{
    private readonly List<string> _items = new();

    public void AddItem(string item)
    {
        if (string.IsNullOrWhiteSpace(item))
            throw new ArgumentException("Item cannot be empty.");

        _items.Add(item);
    }

    public IReadOnlyList<string> Items => _items;

    public bool ContainsAllergen(string allergen)
    {
        return new AllergenChecker()
            .Contains(_items, allergen);
    }

    public int CalculatePreparationTime()
    {
        return new PreparationTimeCalculator()
            .Calculate(_items);
    }

    public string PrepareTicket()
    {
        return new KitchenTicketFormatter()
            .Format(_items);
    }

    public string ChooseOrderLane()
    {
        return new KitchenLaneSelector()
            .Select(_items);
    }
}


public sealed class AllergenChecker
{
    public bool Contains(
        IEnumerable<string> items,
        string allergen)
    {
        foreach (var item in items)
        {
            if (item.Contains(
                    allergen,
                    StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}


public sealed class PreparationTimeCalculator
{
    public int Calculate(IEnumerable<string> items)
    {
        var count = 0;

        foreach (var item in items)
        {
            count++;
        }

        return count * 5;
    }
}


public sealed class KitchenTicketFormatter
{
    public string Format(IEnumerable<string> items)
    {
        return string.Join(
            Environment.NewLine,
            items);
    }
}


public sealed class KitchenLaneSelector
{
    public string Select(IEnumerable<string> items)
    {
        foreach (var item in items)
        {
            if (item.Contains(
                    "pizza",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "Pizza Lane";
            }

            if (item.Contains(
                    "burger",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "Grill Lane";
            }
        }

        return "General Lane";
    }
}