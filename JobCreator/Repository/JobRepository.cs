public class JobRepository : IJobRepository
{
    public async Task<IEnumerable<StoredJob>> GetStoredJobs()
    {
        return Enumerable.Empty<StoredJob>().Append(new StoredJob
        {
            Id = 1,
            Name = "TestJob",
            StoredProcedure = "Fancy Job",
            CronExpression = "0 56 15 ? * WED *",            
            ParameterName = "FECHA",
            LogMessage = "Prueba efectuada exitosamente en Stored Job"
        });
    }
}