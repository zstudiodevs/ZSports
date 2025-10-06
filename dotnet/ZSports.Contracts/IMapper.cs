namespace ZSports.Contracts;

public interface IMapper
{
    static TDestination Map<TSource, TDestination>(TSource source) where TDestination : new()
    {
        throw new NotImplementedException();
    }
    IEnumerable<TDestination> MapCollection<TSource, TDestination>(IEnumerable<TSource> sourceCollection);
}
