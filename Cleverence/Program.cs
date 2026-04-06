using Cleverence.task3;
internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, Cleverence!");

        // task3
        string pathi = Path.Combine("task3", "input.log");
        string patho = Path.Combine("task3", "output.log");
        string pathp = Path.Combine("task3", "problems.txt");

        await LogStandardizer.ProcessLogsAsync(pathi, patho, pathp);
        Console.WriteLine("Обработка завершена.");
    }
}