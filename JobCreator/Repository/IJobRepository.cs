public interface IJobRepository
{
    Task<IEnumerable<StoredJob>> GetStoredJobs();
}
