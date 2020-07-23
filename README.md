# Hangfire.SpecifyQueue
Hangfire创建任务时，可以指定队列名称

Startup.cs
```cs
public void ConfigureServices(IServiceCollection services)
{
    // ...

    services.AddHangfire(config =>
    {
        config.UseSqlServerStorage("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=hangfire;Integrated Security=True");
        config.UseFilter(new AutomaticRetryAttribute { Attempts = 1 });
    });

    services.AddHangfireSpecifyQueue();

    services.AddHangfireServer(options =>
    {
        options.ServerName = "SpecifyQueue_Sample";
        options.Queues = new string[] { "hello" };
    });

    services.AddTransient<IHelloService, HelloService>();
}

```

Crete Job
```cs
using Hangfire;
using Hangfire.SpecifyQueue;

public class SomeService
{
    private readonly IBackgroundJobClient _jobClient;

    public SomeService(IBackgroundJobClient jobClient)
    {
        _jobClient = jobClient;
    }

    public string Enqueue()
    {
        var jobId = _jobClient.Enqueue<IHelloService>(queueName: "hello", x => x.Hello("jack"));

        return jobId
    }

    public string Delay()
    {
        var jobId = _jobClient.Schedule<IHelloService>(
                    queueName: "hello",
                    methodCall: x => x.Hello("jack"),
                    delay: TimeSpan.FromSeconds(5));

        return jobId;
    }
}
```