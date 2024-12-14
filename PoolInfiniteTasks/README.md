[![ru](https://img.shields.io/badge/lang-ru-green.svg)](./README.ru.md)

# Pool Infinite Tasks C#

[![NuGet version (PoolInfiniteTasks)](https://img.shields.io/nuget/v/PoolInfiniteTasks.svg?style=flat-square)](https://www.nuget.org/packages/PoolInfiniteTasks)


The solution allows you to create a pool to perform endless tasks and recreate them in case of failure.

## Usage example

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
