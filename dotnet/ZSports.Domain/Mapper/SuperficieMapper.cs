using ZSports.Contracts.Superficies;
using ZSports.Domain.Entities;

namespace ZSports.Domain.Mapper;

public static class SuperficieMapper
{
    public static SuperficieDto Map(Superficie source)
    {
        return new SuperficieDto()
        {
            Id = source.Id,
            Nombre = source.Nombre,
            Activo = source.Activo
        };
    }

    public static Superficie Map(SuperficieDto source)
    {
        var destination = new Superficie();
        destination.SetId(source.Id);
        destination.SetNombre(source.Nombre);
        if (source.Activo)
            destination.Habilitar();
        else
            destination.Deshabilitar();

            return destination;
    }

    public static IEnumerable<SuperficieDto> MapCollection(IEnumerable<Superficie> sourceCollection)
    {
        foreach (var source in sourceCollection)
        {
            yield return Map(source);
        }
    }

    public static IEnumerable<Superficie> MapCollection(IEnumerable<SuperficieDto> sourceCollection)
    {
        foreach (var source in sourceCollection)
        {
            yield return Map(source);
        }
    }
}
