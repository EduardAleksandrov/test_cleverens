namespace Test;

using Xunit;
using Cleverence.task3;

public class LogStandardizerTests
{
    [Fact]
    public void Parse_Format1_ShouldReturnStandardizedEntry()
    {
        // Arrange
        string input = "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'";
        string expected = "2025-03-10\t15:14:49.523\tINFO\tDEFAULT\tВерсия программы: '3.4.0.48729'";

        // Act
        var entry = LogStandardizer.TryParseLine(input);

        // Assert
        Assert.NotNull(entry);
        Assert.Equal(expected, entry.ToString());
    }

    [Fact]
    public void Parse_Format2_ShouldReturnStandardizedEntry()
    {
        // Arrange
        string input = "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetId| Код: '@M40'";
        string expected = "2025-03-10\t15:14:51.5882\tINFO\tMobileComputer.GetId\tКод: '@M40'";

        // Act
        var entry = LogStandardizer.TryParseLine(input);

        // Assert
        Assert.NotNull(entry);
        Assert.Equal(expected, entry.ToString());
    }

    [Theory]
    [InlineData("INFORMATION", "INFO")]
    [InlineData("WARNING", "WARN")]
    [InlineData("ERROR", "ERROR")]
    public void LogLevelMapping_ShouldNormalizeLevels(string inputLevel, string expectedLevel)
    {
        // Проверка через формат 1
        string input = $"10.03.2025 10:00:00.000 {inputLevel} Test message";
        
        var entry = LogStandardizer.TryParseLine(input);
        
        Assert.Equal(expectedLevel, entry?.Level);
    }

    [Fact]
    public void Parse_InvalidLine_ShouldReturnNull()
    {
        // Arrange
        string invalidLine = "Это просто какая-то строка текста, а не лог";

        // Act
        var entry = LogStandardizer.TryParseLine(invalidLine);

        // Assert
        Assert.Null(entry);
    }
}
