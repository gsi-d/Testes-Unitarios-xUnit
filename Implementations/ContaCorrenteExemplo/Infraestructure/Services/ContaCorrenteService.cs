using Implementations.ContaCorrenteExemplo.Domain;
using Implementations.ContaCorrenteExemplo.Infraestructure.Interfaces;

namespace Implementations.ContaCorrenteExemplo.Infraestructure.Services
{
    public class ContaCorrenteService
    {
        private readonly INotificacaoService _notificacaoService;
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        public ContaCorrenteService(INotificacaoService notificacaoService, IContaCorrenteRepository contaCorrenteRepository)
        {
            _notificacaoService = notificacaoService;
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public bool NotificarContaCorrente(string documento)
        {
            var contaCorrente = _contaCorrenteRepository.ObterPorDocumento(documento);

            if (contaCorrente == null)
            {
                return false;
            }

            var respostaNotificacao = _notificacaoService.Notificar(contaCorrente);

            return respostaNotificacao.Sucesso;
        }

        public bool Transferencia(ContaCorrente contaOrigem, ContaCorrente contaDestino, decimal valor)
        {
            if (contaOrigem.PodeTransferir(valor))
            {
                contaOrigem.Debitar(valor);
                contaDestino.Creditar(valor);

                return true;
            }

            return false;
        }
    }
}
