namespace Test;

using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Cleverens.task2;

public class ServerTests
{
    [Fact]
    public async Task ConcurrentAccess_ShouldMaintainDataIntegrity()
    {
        // Arrange
        int writersCount = 10;
        int incrementsPerWriter = 1000;
        int readersCount = 50;
        int expectedSum = writersCount * incrementsPerWriter;

        // Сбрасываем значение (в реальном коде лучше не использовать статику в тестах, 
        // но здесь следуем условию задачи)
        // Для сброса можно добавить внутренний метод Reset() в Server
        
        var tasks = new List<Task>();

        // Act
        
        // Запускаем писателей
        for (int i = 0; i < writersCount; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                for (int j = 0; j < incrementsPerWriter; j++)
                {
                    Server.AddToCount(1);
                }
            }));
        }

        // Запускаем читателей параллельно
        for (int i = 0; i < readersCount; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                for (int j = 0; j < 100; j++)
                {
                    _ = Server.GetCount(); // Отброс значения
                }
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        Assert.Equal(expectedSum, Server.GetCount());
    }
}