using ZSports.Contracts.Deportes;
using ZSports.Domain.Entities;

namespace ZSports.Domain.Mapper;

public static class DeporteMapper
{
    public static DeporteDto Map(Deporte source)
    {
        return new DeporteDto()
        {
            Id = source.Id,
            Nombre = source.Nombre,
            Codigo = source.Codigo
        };
    }

    public static Deporte Map(DeporteDto source)
    {
        var destination = new Deporte();
        destination.SetNombre(source.Nombre);
        destination.SetCodigo(source.Codigo);
        return destination;
    }

    public static IEnumerable<DeporteDto> MapCollection(IEnumerable<Deporte> sourceCollection)
    {
        foreach (var source in sourceCollection)
        {
            yield return Map(source);
        }
    }

    public static IEnumerable<Deporte> MapCollection(IEnumerable<DeporteDto> sourceCollection)
    {
        foreach (var source in sourceCollection)
        {
            yield return Map(source);
        }
    }
}
