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