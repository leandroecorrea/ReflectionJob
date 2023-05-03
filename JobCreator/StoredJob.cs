public class StoredJob
{
    public int Id { get; set; }
    public string CronExpression { get; set; }
    public string Name { get; set; }
    public string StoredProcedure { get; set; }    
    public string LogMessage { get; set; }
    public string ParameterName { get; internal set; }
}