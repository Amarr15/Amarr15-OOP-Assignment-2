namespace SrpLab;

public sealed class GradeBook
{
    private readonly List<double> _grades = new();

    public void AddGrade(double grade)
    {
        if (grade < 0 || grade > 100)
            throw new ArgumentOutOfRangeException(nameof(grade));

        _grades.Add(grade);
    }

    public double CalculateAverage()
    {
        if (_grades.Count == 0)
            return 0;

        return _grades.Average();
    }

    public string GetLetterGrade(double average)
    {
        return new GradingPolicy().GetLetterGrade(average);
    }

    public bool IsHonorRoll()
    {
        return CalculateAverage() >= 90;
    }

    public string ExportTranscript(
        string studentName,
        string studentId)
    {
        return new TranscriptExporter()
            .Export(studentName, studentId, _grades);
    }

    public string ExportCsv(
        string studentName,
        string studentId)
    {
        return new GradeCsvExporter()
            .Export(studentName, studentId, _grades);
    }
}


public sealed class GradingPolicy
{
    public string GetLetterGrade(double average)
    {
        if (average >= 90)
            return "A";

        if (average >= 80)
            return "B";

        if (average >= 70)
            return "C";

        if (average >= 60)
            return "D";

        return "F";
    }
}


public sealed class TranscriptExporter
{
    public string Export(
        string studentName,
        string studentId,
        IEnumerable<double> grades)
    {
        var average = grades.Any()
            ? grades.Average()
            : 0;

        return $"Student: {studentName}\n" +
               $"ID: {studentId}\n" +
               $"Average: {average:F2}\n";
    }
}


public sealed class GradeCsvExporter
{
    public string Export(
        string studentName,
        string studentId,
        IEnumerable<double> grades)
    {
        var lines = new List<string>
        {
            "StudentId,StudentName,Grade"
        };

        foreach (var grade in grades)
        {
            lines.Add($"{studentId},{studentName},{grade}");
        }

        return string.Join("\n", lines);
    }
}