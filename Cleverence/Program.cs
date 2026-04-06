using Cleverence.task3;
internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, Cleverence!");

        // task3
        string path_i = Path.Combine("task3", "input.log");
        string path_o = Path.Combine("task3", "output.log");
        string path_p = Path.Combine("task3", "problems.txt");

        await LogStandardizer.ProcessLogsAsync(path_i, path_o, path_p);
        Console.WriteLine("Обработка завершена.");
    }
}