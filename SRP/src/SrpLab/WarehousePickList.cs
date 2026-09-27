namespace SrpLab;

public sealed class WarehousePickList
{
    private readonly List<string> _items = new();

    public void AddNeed(string item)
    {
        if (string.IsNullOrWhiteSpace(item))
            throw new ArgumentException(
                "Item cannot be empty.");

        _items.Add(item);
    }

    public IReadOnlyList<string> GetItems()
    {
        return _items;
    }

    public List<string> CreateWalkingOrder()
    {
        return new WalkingOrderCalculator()
            .Calculate(_items);
    }

    public string CreatePickerInstructions()
    {
        return new PickerInstructionGenerator()
            .Generate(_items);
    }

    public string ExportToWmsXml()
    {
        return new WmsXmlExporter()
            .Export(_items);
    }
}


public sealed class WalkingOrderCalculator
{
    public List<string> Calculate(
        IEnumerable<string> items)
    {
        var result = new List<string>();

        foreach (var item in items)
        {
            result.Add(item);
        }

        return result;
    }
}


public sealed class PickerInstructionGenerator
{
    public string Generate(
        IEnumerable<string> items)
    {
        var instructions = new List<string>();

        foreach (var item in items)
        {
            instructions.Add(
                $"Pick item: {item}");
        }

        return string.Join(
            Environment.NewLine,
            instructions);
    }
}


public sealed class WmsXmlExporter
{
    public string Export(
        IEnumerable<string> items)
    {
        var result = "<PickList>\n";

        foreach (var item in items)
        {
            result += $"  <Item>{item}</Item>\n";
        }

        result += "</PickList>";

        return result;
    }
}