using AutoFixture;
using Implementations.ContaCorrenteExemplo.Domain;
using Implementations.ContaCorrenteExemplo.Infraestructure.Interfaces;
using Implementations.ContaCorrenteExemplo.Infraestructure.Models;
using Implementations.ContaCorrenteExemplo.Infraestructure.Services;
using IntroducaoTestesUnitarios.ContaCorrenteExemplo.Factories;
using Moq;

namespace IntroducaoTestesUnitarios.ContaCorrenteExemplo.Application
{
    public class ContaCorrenteServiceTest
    {
        Mock<IContaCorrenteRepository> contaCorrenteRepositoryMock = new();
        Mock<INotificacaoService> notificacaoServiceMock = new();

        [Fact]
        public void ContaExistenteNotificacaoFuncionando_ChamadoDocumentoValido_RetornarSucesso()
        {
            // Arrange
            Fixture fixture = new Fixture();
            ContaCorrente contaCorrente = fixture.Create<ContaCorrente>();
            // ANTES: var contaCorrente = ContaCorrenteFactory.GetContaOrigemValida();

            fixture.RepeatCount = 5;
            List<Operacao> operacoes = fixture.CreateMany<Operacao>().ToList();

            RespostaNotificacaoViewModel respostaNotificacaoViewModel = RespostaNotificacaoViewModelFactory.ObterRespostaSucesso();

            contaCorrenteRepositoryMock.Setup(ccr => ccr.ObterPorDocumento(contaCorrente.Documento)).Returns(contaCorrente);
            notificacaoServiceMock.Setup(ns => ns.Notificar(contaCorrente)).Returns(respostaNotificacaoViewModel);

            ContaCorrenteService contaCorrenteService = new ContaCorrenteService(notificacaoServiceMock.Object, contaCorrenteRepositoryMock.Object);

            // Act
            bool resposta = contaCorrenteService.NotificarContaCorrente(contaCorrente.Documento);

            // Assert
            contaCorrenteRepositoryMock.Verify(ccr => ccr.ObterPorDocumento(contaCorrente.Documento), Times.Once);
            notificacaoServiceMock.Verify(ns => ns.Notificar(It.IsAny<ContaCorrente>()), Times.Once);

            Assert.True(resposta);
        }

        [Fact]
        public void ContaExistenteMasNotificacaoNaoFuncionando_ChamadoDocumentoValido_RetornarFalha()
        {
            // Arrange
            Fixture fixture = new Fixture();
            ContaCorrente contaCorrente = fixture.Create<ContaCorrente>();
            RespostaNotificacaoViewModel respostaNotificacaoViewModel = RespostaNotificacaoViewModelFactory.ObterRespostaFalha();

            contaCorrenteRepositoryMock.Setup(ccr => ccr.ObterPorDocumento(contaCorrente.Documento)).Returns(contaCorrente);
            notificacaoServiceMock.Setup(ns => ns.Notificar(contaCorrente)).Returns(respostaNotificacaoViewModel);

            ContaCorrenteService contaCorrenteService = new ContaCorrenteService(notificacaoServiceMock.Object, contaCorrenteRepositoryMock.Object);

            // Act
            bool resposta = contaCorrenteService.NotificarContaCorrente(contaCorrente.Documento);

            // Assert
            contaCorrenteRepositoryMock.Verify(ccr => ccr.ObterPorDocumento(contaCorrente.Documento), Times.Once);
            notificacaoServiceMock.Verify(ns => ns.Notificar(It.IsAny<ContaCorrente>()), Times.Once);

            Assert.False(resposta);
        }

        [Fact]
        public void ContaOrigemTemLimiteESaldoSuficiente_ChamadoComContasEValoresValidos_RetornarSucessoEDebitarDosSaldosELImiteCorretamente()
        {
            const decimal valorTransacao = 500;
            const decimal contaCorrenteOrigemSaldoEsperado = 9500;
            const decimal contaCorrenteOrigemLimiteEsperado = 19500;
            const decimal contaCorrenteDestinoSaldoEsperado = 10500;
            const decimal contaCorrenteDestinoLimiteEsperado = 20000;


            // Arrange

            //ANTES
            //var contaCorrenteOrigem = ContaCorrenteFactory.GetContaOrigemValida();
            //var contaCorrenteDestino = ContaCorrenteFactory.ObterContaDestinoValida();

            Fixture fixture = new Fixture();

            ContaCorrente contaCorrenteOrigem = fixture.Create<ContaCorrente>();
            ContaCorrente contaCorrenteDestino = fixture.Create<ContaCorrente>();

            ContaCorrenteService operacaoFinanceiraService = new ContaCorrenteService(notificacaoServiceMock.Object, contaCorrenteRepositoryMock.Object);

            // Act
            bool resultadoOperacao = operacaoFinanceiraService.Transferencia(contaCorrenteOrigem, contaCorrenteDestino, valorTransacao);

            // Assert
            Assert.True(resultadoOperacao);
            Assert.Equal(contaCorrenteOrigemSaldoEsperado, contaCorrenteOrigem.Saldo);
            Assert.Equal(contaCorrenteOrigemLimiteEsperado, contaCorrenteOrigem.Limite);
            Assert.Equal(contaCorrenteDestinoSaldoEsperado, contaCorrenteDestino.Saldo);
            Assert.Equal(contaCorrenteDestinoLimiteEsperado, contaCorrenteDestino.Limite);
        }

        [Fact]
        public void ContaOrigemTemLimiteMasSaldoInsuficiente_ChamadoComContasEValoresValidos_RetornarFalhaENaoAlterarSaldosELimites()
        {
            const decimal valorTransacaoAcimaDoSaldoInicial = 10500;
            const decimal contaCorrenteOrigemSaldoEsperado = 10000;
            const decimal contaCorrenteOrigemLimiteEsperado = 20000;
            const decimal contaCorrenteDestinoSaldoEsperado = 10000;
            const decimal contaCorrenteDestinoLimiteEsperado = 20000;

            // Arrange

            //ANTES
            //var contaCorrenteOrigem = ContaCorrenteFactory.GetContaOrigemValida();
            //var contaCorrenteDestino = ContaCorrenteFactory.ObterContaDestinoValida();

            Fixture fixture = new Fixture();

            ContaCorrente contaCorrenteOrigem = fixture.Create<ContaCorrente>();
            ContaCorrente contaCorrenteDestino = fixture.Create<ContaCorrente>();

            ContaCorrenteService operacaoFinanceiraService = new ContaCorrenteService(notificacaoServiceMock.Object, contaCorrenteRepositoryMock.Object);

            // Act
            bool resultadoOperacao = operacaoFinanceiraService.Transferencia(contaCorrenteOrigem, contaCorrenteDestino, valorTransacaoAcimaDoSaldoInicial);

            // Assert
            Assert.False(resultadoOperacao);
            Assert.Equal(contaCorrenteOrigemSaldoEsperado, contaCorrenteOrigem.Saldo);
            Assert.Equal(contaCorrenteOrigemLimiteEsperado, contaCorrenteOrigem.Limite);
            Assert.Equal(contaCorrenteDestinoSaldoEsperado, contaCorrenteDestino.Saldo);
            Assert.Equal(contaCorrenteDestinoLimiteEsperado, contaCorrenteDestino.Limite);

        }
    }
}
