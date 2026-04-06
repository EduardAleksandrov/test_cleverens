namespace Cleverence.task2;

using System.Threading;

// Шаблон Читатели-писатели
public static class Server
{
    private static int _count = 0;
    
    // Используем ReaderWriterLockSlim для управления доступом.
    private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    public static int GetCount()
    {
        // Входим в режим чтения: несколько потоков могут зайти сюда одновременно
        _lock.EnterReadLock();
        try
        {
            return _count;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public static void AddToCount(int value)
    {
        // Входим в режим записи: только один поток может быть здесь, 
        // читатели в это время будут ждать на EnterReadLock
        _lock.EnterWriteLock();
        try
        {
            _count += value;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}
