using Quartz;
using System.Data.Common;
using System.Reflection.Emit;
using System.Reflection;

public partial class JobDeJobs : IJob
{
    private readonly IScheduler _scheduler;
    private readonly IJobRepository _jobRepository;

    public JobDeJobs(IScheduler scheduler, IJobRepository jobRepository)
    {
        _scheduler = scheduler;
        _jobRepository = jobRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var existentJobs = await _jobRepository.GetStoredJobs();
        foreach (var existentJob in existentJobs)
        {            
            if (!await _scheduler.CheckExists(new JobKey(existentJob.Name)))
            {
                var typeCreator = new StoredJobTypeCreator();                
                var nuevaClase = typeCreator.Create(existentJob);
                var jop = JobBuilder.Create(nuevaClase)
                                    .WithIdentity(existentJob.Name)
                                    .Build();
                var trigger = TriggerBuilder.Create()
                                            .WithIdentity(existentJob.Name)
                                            .WithDailyTimeIntervalSchedule(i => i.WithIntervalInSeconds(3))                                            
                                            .ForJob(existentJob.Name)
                                            .Build();
                await _scheduler.ScheduleJob(jop, trigger);
            }
        }
    }



}
