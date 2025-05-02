using Implementations.ContaCorrenteExemplo.Domain;
using Implementations.ContaCorrenteExemplo.Infraestructure.Models;

namespace Implementations.ContaCorrenteExemplo.Infraestructure.Interfaces
{
    public interface INotificacaoService
    {
        RespostaNotificacaoViewModel Notificar(ContaCorrente contaCorrente);
    }
}
