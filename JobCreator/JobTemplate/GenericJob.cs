using Quartz;

public partial class JobDeJobs
{
    public class GenericJob : IJob
    {
        private readonly ILogger<GenericJob> _logger;

        public GenericJob(ILogger<GenericJob> logger)
        {
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("groso sos");
        }
    }
}
