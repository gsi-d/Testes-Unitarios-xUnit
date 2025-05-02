using AutoFixture;
using Implementations.ContaCorrenteExemplo.Domain;
using Implementations.ContaCorrenteExemplo.Infraestructure.Interfaces;
using Implementations.ContaCorrenteExemplo.Infraestructure.Services;
using IntroducaoTestesUnitarios.ContaCorrenteExemplo.Factories;
using Moq;

namespace IntroducaoTestesUnitarios.ContaCorrenteExemplo.Application
{
    public class ContaCorrenteServiceTest
    {
        [Fact]
        public void ContaExistenteNotificacaoFuncionando_ChamadoDocumentoValido_RetornarSucesso()
        {
            // Arrange
            var fixture = new Fixture();
            var contaCorrente = fixture.Create<ContaCorrente>();
            // ANTES: var contaCorrente = ContaCorrenteFactory.GetContaOrigemValida();

            fixture.RepeatCount = 5;
            var operacoes = fixture.CreateMany<Operacao>().ToList();

            var respostaNotificacaoViewModel = RespostaNotificacaoViewModelFactory.ObterRespostaSucesso();

            var contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            var notificacaoServiceMock = new Mock<INotificacaoService>();

            contaCorrenteRepositoryMock.Setup(ccr => ccr.ObterPorDocumento(contaCorrente.Documento)).Returns(contaCorrente);
            notificacaoServiceMock.Setup(ns => ns.Notificar(contaCorrente)).Returns(respostaNotificacaoViewModel);

            var contaCorrenteService = new ContaCorrenteService(notificacaoServiceMock.Object, contaCorrenteRepositoryMock.Object);

            // Act
            var resposta = contaCorrenteService.NotificarContaCorrente(contaCorrente.Documento);

            // Assert
            contaCorrenteRepositoryMock.Verify(ccr => ccr.ObterPorDocumento(contaCorrente.Documento), Times.Once);
            notificacaoServiceMock.Verify(ns => ns.Notificar(It.IsAny<ContaCorrente>()), Times.Once);

            Assert.True(resposta);
        }

        [Fact]
        public void ContaExistenteMasNotificacaoNaoFuncionando_ChamadoDocumentoValido_RetornarFalha()
        {
            // Arrange
            var fixture = new Fixture();
            var contaCorrente = fixture.Create<ContaCorrente>();
            var respostaNotificacaoViewModel = RespostaNotificacaoViewModelFactory.ObterRespostaFalha();

            var contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            var notificacaoServiceMock = new Mock<INotificacaoService>();

            contaCorrenteRepositoryMock.Setup(ccr => ccr.ObterPorDocumento(contaCorrente.Documento)).Returns(contaCorrente);
            notificacaoServiceMock.Setup(ns => ns.Notificar(contaCorrente)).Returns(respostaNotificacaoViewModel);

            var contaCorrenteService = new ContaCorrenteService(notificacaoServiceMock.Object, contaCorrenteRepositoryMock.Object);

            // Act
            var resposta = contaCorrenteService.NotificarContaCorrente(contaCorrente.Documento);

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
            var contaCorrenteOrigem = ContaCorrenteFactory.GetContaOrigemValida();
            var contaCorrenteDestino = ContaCorrenteFactory.ObterContaDestinoValida();

            var contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            var notificacaoServiceMock = new Mock<INotificacaoService>();

            var operacaoFinanceiraService = new ContaCorrenteService(notificacaoServiceMock.Object, contaCorrenteRepositoryMock.Object);

            // Act
            var resultadoOperacao = operacaoFinanceiraService.Transferencia(contaCorrenteOrigem, contaCorrenteDestino, valorTransacao);

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
            var contaCorrenteOrigem = ContaCorrenteFactory.GetContaOrigemValida();
            var contaCorrenteDestino = ContaCorrenteFactory.ObterContaDestinoValida();

            var contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            var notificacaoServiceMock = new Mock<INotificacaoService>();

            var operacaoFinanceiraService = new ContaCorrenteService(notificacaoServiceMock.Object, contaCorrenteRepositoryMock.Object);

            // Act
            var resultadoOperacao = operacaoFinanceiraService.Transferencia(contaCorrenteOrigem, contaCorrenteDestino, valorTransacaoAcimaDoSaldoInicial);

            // Assert
            Assert.False(resultadoOperacao);
            Assert.Equal(contaCorrenteOrigemSaldoEsperado, contaCorrenteOrigem.Saldo);
            Assert.Equal(contaCorrenteOrigemLimiteEsperado, contaCorrenteOrigem.Limite);
            Assert.Equal(contaCorrenteDestinoSaldoEsperado, contaCorrenteDestino.Saldo);
            Assert.Equal(contaCorrenteDestinoLimiteEsperado, contaCorrenteDestino.Limite);

        }
    }
}
