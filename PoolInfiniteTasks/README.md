[![ru](https://img.shields.io/badge/lang-ru-green.svg)](./README.ru.md)

# Pool Infinite Tasks C#

[![NuGet version (PoolInfiniteTasks)](https://img.shields.io/nuget/v/PoolInfiniteTasks.svg?style=flat-square)](https://www.nuget.org/packages/PoolInfiniteTasks)


The solution allows you to create a pool to perform endless tasks and recreate them in case of failure.

## Usage example

```
using PoolInfiniteTasks;

var cts = new CancellationTokenSource();

cts.CancelAfter(5000);

Func<CancellationToken, Task> myTaskFactory = async (cancellationToken) =>
{
    await Task.Run(async () =>
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
