namespace Backend.Libs.Caching;

public class MemoryCacheOptions
{
    public static string Name = nameof(MemoryCacheOptions);
    public string RepositoryBaseKey { get; init; } = "backend-repo:";
    public int AbsoluteExpirationInHours { get; init; } = 1;
    public TimeSpan AbsoluteExpiration => TimeSpan.FromHours(AbsoluteExpirationInHours);
    public TimeSpan SlidingExpiration => AbsoluteExpiration.Divide(3); 
}