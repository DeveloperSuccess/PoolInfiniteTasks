[![ru](https://img.shields.io/badge/lang-ru-green.svg)](./README.ru.md)

# Пул бесконечных задач C#

[![NuGet version (PoolInfiniteTasks)](https://img.shields.io/nuget/v/PoolInfiniteTasks.svg?style=flat-square)](https://www.nuget.org/packages/PoolInfiniteTasks)

Решение позволяет создавать пул для выполнения бесконечных задач и воссоздавать их в случае сбоя.

## Пример использования

```
private Task Test(CancellationToken cancellationToken)
{
    Func<CancellationToken, Task> myTaskFactory = async (cancellationToken) =>
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            /// Any of your logic
            await Task.Delay(1000, cancellationToken);
        }
    };

    var poolInfiniteTasks = new PoolInfiniteTasks(myTaskFactory, 3);

    return poolInfiniteTasks.Run(cancellationToken);
}
```
