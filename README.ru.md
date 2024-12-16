[![ru](https://img.shields.io/badge/lang-ru-green.svg)](./README.ru.md)

# Пул бесконечных задач C#

[![NuGet version (PoolInfiniteTasks)](https://img.shields.io/nuget/v/PoolInfiniteTasks.svg?style=flat-square)](https://www.nuget.org/packages/PoolInfiniteTasks)

Решение позволяет создавать пул для выполнения бесконечных задач и воссоздавать их в случае сбоя.

## Пример использования

```
using PoolInfiniteTasks;

var cts = new CancellationTokenSource();

cts.CancelAfter(5000);

Func<CancellationToken, Task> myTaskFactory = (cancellationToken) =>
{
    return Task.Run(async () =>
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // Any of your logic            
            await Task.Delay(1000, cancellationToken);
            Console.WriteLine("Success");
            throw new Exception("test");
        }
    }, cancellationToken);
};

await new PoolInfiniteTasksManager(myTaskFactory, 3).Run(cts.Token);
```
