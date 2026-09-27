namespace SrpLab;

public sealed class WardBoard
{
    private readonly Dictionary<string, string> _beds = new();

    public void AssignPatient(
        string bedNumber,
        string patientName)
    {
        _beds[bedNumber] = patientName;
    }

    public string? GetPatient(string bedNumber)
    {
        return _beds.TryGetValue(
            bedNumber,
            out var patient)
            ? patient
            : null;
    }

    public int CalculateAcuityScore(
        string patientName)
    {
        return new AcuityScoreCalculator()
            .Calculate(patientName);
    }

    public string CreatePagerAlert(
        string patientName,
        string message)
    {
        return new PagerAlertGenerator()
            .Generate(patientName, message);
    }

    public string CreateHandoffNotes(
        string patientName,
        string notes)
    {
        return new HandoffNoteGenerator()
            .Generate(patientName, notes);
    }

    public string ExportCsv()
    {
        return new WardCsvExporter()
            .Export(_beds);
    }
}


public sealed class AcuityScoreCalculator
{
    public int Calculate(string patientName)
    {
        if (string.IsNullOrWhiteSpace(patientName))
            return 0;

        return patientName.Length;
    }
}


public sealed class PagerAlertGenerator
{
    public string Generate(
        string patientName,
        string message)
    {
        return $"PAGER ALERT: {patientName} - {message}";
    }
}


public sealed class HandoffNoteGenerator
{
    public string Generate(
        string patientName,
        string notes)
    {
        return $"Patient: {patientName}\n" +
               $"Handoff Notes: {notes}";
    }
}


public sealed class WardCsvExporter
{
    public string Export(
        Dictionary<string, string> beds)
    {
        var lines = new List<string>
        {
            "Bed,Patient"
        };

        foreach (var bed in beds)
        {
            lines.Add($"{bed.Key},{bed.Value}");
        }

        return string.Join("\n", lines);
    }
}