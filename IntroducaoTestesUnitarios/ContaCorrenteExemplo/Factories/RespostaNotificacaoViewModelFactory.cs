using Implementations.ContaCorrenteExemplo.Infraestructure.Models;

namespace IntroducaoTestesUnitarios.ContaCorrenteExemplo.Factories
{
    public class RespostaNotificacaoViewModelFactory
    {
        public static RespostaNotificacaoViewModel ObterRespostaSucesso()
        {
            return new RespostaNotificacaoViewModel(true, string.Empty);
        }

        public static RespostaNotificacaoViewModel ObterRespostaFalha()
        {
            return new RespostaNotificacaoViewModel(false, "Serviço fora do ar");
        }
    }
}
