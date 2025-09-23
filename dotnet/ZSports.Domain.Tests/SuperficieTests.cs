using ZSports.Domain.Entities;

namespace ZSports.Domain.Tests;

[TestFixture]
public class SuperficieTests
{
    private Superficie _superficie;

    [SetUp]
    public void Setup()
    {
        _superficie = new Superficie();
    }

    [Test]
    public void NuevaSuperficie_ConNombreVacio_DebeFallar()
    {
        Assert.Throws<ArgumentException>(() => _superficie.SetNombre(string.Empty));
    }

    [Test]
    public void NuevaSuperficie_ConNombreMuyLargo_DebeFallar()
    {
        var nombreMuyLargo = new string('a', 101);
        Assert.Throws<ArgumentException>(() => _superficie.SetNombre(nombreMuyLargo));
    }

    [Test]
    public void NuevaSuperficie_ConNombreValido_DebeTenerNombre()
    {
        var nombreValido = "Césped Artificial";
        _superficie.SetNombre(nombreValido);
        Assert.That(nombreValido, Is.EqualTo(_superficie.Nombre));
    }
}
