using Quartz;

var builder = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {   
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
        });
        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });        
        services.AddSingleton(x => { 
            var factory = x.GetRequiredService<ISchedulerFactory>(); 
            var scheduler = factory.GetScheduler().Result;
            return scheduler;
        });
        services.AddSingleton<IJobRepository, JobRepository>();
    })    
    .Build();
var scheduler = builder.Services
    .GetRequiredService<IScheduler>();
var jobDeJobds = JobBuilder.Create<JobDeJobs>()
    .WithIdentity(nameof(JobDeJobs))
    .Build();
var triggerDeJobDeJobs = TriggerBuilder.Create()
    .WithIdentity(nameof(JobDeJobs))
    .ForJob(jobDeJobds)
    .WithDailyTimeIntervalSchedule(i => i.WithIntervalInSeconds(5))
    .Build();
await scheduler.ScheduleJob(jobDeJobds, triggerDeJobDeJobs);

await builder.RunAsync();
