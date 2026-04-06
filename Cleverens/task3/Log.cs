namespace Cleverens.task3;

using System.Globalization;
using System.Text.RegularExpressions;

public class LogStandardizer
{
    // Формат 1: 10.03.2025 15:14:49.523 INFORMATION Сообщение
    private static readonly Regex RegexFormat1 = new(@"^(\d{2}\.\d{2}\.\d{4})\s+(\d{2}:\d{2}:\d{2}\.\d+)\s+(\w+)\s+(.*)$", RegexOptions.Compiled);
    
    // Формат 2: 2025-03-10 15:14:51.5882| INFO|11|Method| Сообщение
    private static readonly Regex RegexFormat2 = new(@"^(\d{4}-\d{2}-\d{2})\s+(\d{2}:\d{2}:\d{2}\.\d+)\|?\s*(\w+)\s*\|\d+\|([^|]+)\|\s*(.*)$", RegexOptions.Compiled);

    public static async Task ProcessLogsAsync(string inputPath, string outputPath, string problemsPath)
    {
        using var reader = new StreamReader(inputPath);
        using var writer = new StreamWriter(outputPath);
        using var problemWriter = new StreamWriter(problemsPath);

        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var entry = TryParseLine(line);
            if (entry != null)
            {
                await writer.WriteLineAsync(entry.ToString());
            }
            else
            {
                await problemWriter.WriteLineAsync(line);
            }
        }
    }

    public static LogEntry? TryParseLine(string line)
    {
        // Пробуем Формат 1
        var match1 = RegexFormat1.Match(line);
        if (match1.Success)
        {
            return new LogEntry
            {
                Date = NormalizeDate(match1.Groups[1].Value, "dd.MM.yyyy"),
                Time = match1.Groups[2].Value,
                Level = MapLogLevel(match1.Groups[3].Value),
                Method = "DEFAULT",
                Message = match1.Groups[4].Value.Trim()
            };
        }

        // Пробуем Формат 2
        var match2 = RegexFormat2.Match(line);
        if (match2.Success)
        {
            return new LogEntry
            {
                Date = NormalizeDate(match2.Groups[1].Value, "yyyy-MM-dd"),
                Time = match2.Groups[2].Value,
                Level = MapLogLevel(match2.Groups[3].Value),
                Method = match2.Groups[4].Value.Trim(),
                Message = match2.Groups[5].Value.Trim()
            };
        }

        return null;
    }

    public static string NormalizeDate(string dateStr, string inputFormat)
    {
        if (DateTime.TryParseExact(dateStr, inputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
        {
            return dt.ToString("yyyy-MM-dd"); // Использую ГГГГ для соответствия примеру.
        }
        return dateStr;
    }

    public static string MapLogLevel(string level) => level.ToUpper() switch
    {
        "INFORMATION" or "INFO" => "INFO",
        "WARNING" or "WARN" => "WARN",
        "ERROR" => "ERROR",
        "DEBUG" => "DEBUG",
        _ => level
    };
}

public record LogEntry
{
    public required string Date { get; init; }
    public required string Time { get; init; }
    public required string Level { get; init; }
    public required string Method { get; init; }
    public required string Message { get; init; }

    public override string ToString() => $"{Date}\t{Time}\t{Level}\t{Method}\t{Message}";
}
