using Implementations.ContaCorrenteExemplo.Domain;

namespace Implementations.ContaCorrenteExemplo.Infraestructure.Interfaces
{
    public interface IContaCorrenteRepository
    {
        ContaCorrente ObterPorDocumento(string documento);
    }
}
