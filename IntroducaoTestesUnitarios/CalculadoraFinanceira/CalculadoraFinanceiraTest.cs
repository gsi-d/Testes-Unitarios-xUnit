using Implementations.CalculadoraFinanceira;
using Moq;

namespace IntroducaoTestesUnitarios.CalculadoraFinanceira
{
    public class CalculadoraFinanceiraTest
    {
        private readonly Mock<ICalculadoraFinanceira> _mockCalculadoraFinanceira;
        private readonly CalculadoraFinanceiraService _serviceCalculadoraFinanceira;

        public CalculadoraFinanceiraTest()
        {
            _mockCalculadoraFinanceira = new Mock<ICalculadoraFinanceira>();
            _serviceCalculadoraFinanceira = new CalculadoraFinanceiraService(_mockCalculadoraFinanceira.Object);
        }

        #region Fact Testes

        [Fact]
        public void SomarValores_QuandoRecebeDoisValores_DeveRetornarSomaCorreta()
        {
            //Arrange
            const int a = 10, b = 20, resultadoEsperado = 30;
            _mockCalculadoraFinanceira.Setup(x => x.Somar(a, b)).Returns(resultadoEsperado);

            //Act
            int resultado = _serviceCalculadoraFinanceira.SomarValores(a, b);

            //Assert
            Assert.Equal(resultadoEsperado, resultado);

            _mockCalculadoraFinanceira.Verify(x => x.Somar(a, b), Times.Once);
        }

        [Fact]
        public async Task CalcularDivisaoAsync_QuantoRecebeDoisValores_DeveRetornarResultadoCorreto()
        {
            //Arrange
            const int numerador = 100;
            const int denominador = 5;

            _mockCalculadoraFinanceira.Setup(x => x.DividirAsync(numerador, denominador)).ReturnsAsync(20);

            //Act
            int resultado = await _serviceCalculadoraFinanceira.CalcularDivisaoAsync(numerador, denominador);

            //Assert
            Assert.Equal(20, resultado);

            _mockCalculadoraFinanceira.Verify(x => x.DividirAsync(numerador, denominador), Times.Once);
        }

        [Fact]
        public void CalcularJurosCompostos_QuandoValoresInvalidos_DeveLancarExcecao()
        {
            //Arrange
            const decimal principal = -100;
            const decimal taxa = 0.05m;
            const int periodo = 2;

            //Act

            ArgumentException exception = Assert.Throws<ArgumentException>(() => _serviceCalculadoraFinanceira.CalcularJurosCompostos(principal, taxa, periodo));

            //Assert

            Assert.Equal("Os valores devem ser positivos.", exception.Message);

            // Verifica se o método não foi chamado
            _mockCalculadoraFinanceira.Verify(x => x.CalcularJurosCompostos(
                It.IsAny<decimal>(),
                It.IsAny<decimal>(),
                It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void ConverterMoeda_QuandoValoresInvalidos_DeveLancarExcecao()
        {
            //Arrange
            const decimal valor = -100;
            const decimal taxaDeConversao = 5.25m;

            //Act
            ArgumentException exception = Assert.Throws<ArgumentException>(() => _serviceCalculadoraFinanceira.ConverterMoeda(valor, taxaDeConversao));

            //Assert
            Assert.Equal("Valor e taxa de conversão devem ser positivos.", exception.Message);

            _mockCalculadoraFinanceira.Verify(x => x.ConverterMoeda(
                It.IsAny<decimal>(),
                It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void CalcularDesconto_QuandoPercentualInvalido_DeveLancarExcecao()
        {
            //Arrange
            const decimal valorOriginal = 200;
            const decimal percentualDesconto = 110;

            //Act
            ArgumentException exception = Assert.Throws<ArgumentException>(() => _serviceCalculadoraFinanceira.CalcularDesconto(valorOriginal, percentualDesconto));

            //Assert
            Assert.Equal("Valores inválidos para o desconto.", exception.Message);

            _mockCalculadoraFinanceira.Verify(x => x.CalcularDesconto(
                It.IsAny<decimal>(),
                It.IsAny<decimal>()), Times.Never);
        }

        #endregion

        #region Testes com Theory
        [Theory]
        [InlineData(5, 5, 10)]    // Cenário 1: Soma de números positivos
        [InlineData(-3, 7, 4)]    // Cenário 2: Soma de positivo e negativo
        [InlineData(-2, -3, -5)]  // Cenário 3: Soma de negativo e negativo
        [InlineData(0, 0, 0)]     // Cenário 4: Soma de zeros
        public void SomarValores_RecebendoDoisNumeros_DeveRetornarSomaCorreta(int a, int b, int esperado)
        {
            // Arrange
            _mockCalculadoraFinanceira
                .Setup(m => m.Somar(a, b))
                .Returns(esperado);

            // Act
            int resultado = _serviceCalculadoraFinanceira.SomarValores(a, b);

            // Assert
            Assert.Equal(esperado, resultado);

            _mockCalculadoraFinanceira
                .Verify(m => m.Somar(a, b), Times.Once);
        }

        [Theory]
        [InlineData(10, 2, 5)]
        [InlineData(9, 3, 3)]
        public async Task CalcularDivisaoAsync_QuandoReceberDoisValores_DeveRetornarResultadoEsperado(int numerador, int denominador, int esperado)
        {
            // Arrange
            _mockCalculadoraFinanceira
                .Setup(m => m.DividirAsync(numerador, denominador))
                .ReturnsAsync(esperado);

            // Act
            int resultado = await _serviceCalculadoraFinanceira.CalcularDivisaoAsync(numerador, denominador);

            // Assert
            Assert.Equal(esperado, resultado);

            _mockCalculadoraFinanceira
                .Verify(m => m.DividirAsync(numerador, denominador), Times.Once);
        }

        [Theory]
        [InlineData(3, false)]  // Ímpar
        [InlineData(4, true)]   // Par
        [InlineData(0, true)]   // Zero é par
        [InlineData(-2, true)]  // Negativo par
        public void VerificarSeEhPar_QuandoReceberUmNumero_DeveIdentificarCorretamente(int numero, bool esperado)
        {
            // Arrange
            _mockCalculadoraFinanceira
                .Setup(m => m.EhPar(numero))
                .Returns(esperado);

            // Act
            bool resultado = _serviceCalculadoraFinanceira.VerificarSeEhPar(numero);

            // Assert
            Assert.Equal(esperado, resultado);

            _mockCalculadoraFinanceira
                .Verify(m => m.EhPar(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [InlineData(1000, 0.05, 2, 1102.50)]
        [InlineData(500, 0.10, 3, 665.50)]
        public void CalcularJurosCompostos_QuandoValoresValidos_DeveRetornarResultadoCorreto(decimal principal, decimal taxa, int periodo, decimal esperado)
        {
            // Arrange
            _mockCalculadoraFinanceira
                .Setup(m => m.CalcularJurosCompostos(principal, taxa, periodo))
                .Returns(esperado);

            // Act
            decimal resultado = _serviceCalculadoraFinanceira.CalcularJurosCompostos(principal, taxa, periodo);

            // Assert
            Assert.Equal(esperado, resultado);

            _mockCalculadoraFinanceira
                .Verify(m => m.CalcularJurosCompostos(
                    It.IsAny<decimal>(),
                    It.IsAny<decimal>(),
                    It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [InlineData(100, 5.25, 525)]
        [InlineData(200, 4.5, 900)]
        public void ConverterMoeda_QuandoValoresValidos_DeveRetornarValorConvertido(decimal valor, decimal taxaDeConversao, decimal esperado)
        {
            // Arrange
            _mockCalculadoraFinanceira
                .Setup(m => m.ConverterMoeda(valor, taxaDeConversao))
                .Returns(esperado);

            // Act
            decimal resultado = _serviceCalculadoraFinanceira.ConverterMoeda(valor, taxaDeConversao);

            // Assert
            Assert.Equal(esperado, resultado);

            _mockCalculadoraFinanceira
                .Verify(m => m.ConverterMoeda(
                    It.IsAny<decimal>(),
                    It.IsAny<decimal>()), Times.Once);
        }

        [Theory]
        [InlineData(200, 10, 180)]
        [InlineData(500, 20, 400)]
        public void CalcularDesconto_QuandoValoresValidos_DeveRetornarValorComDesconto(decimal valorOriginal, decimal percentualDesconto, decimal esperado)
        {
            // Arrange
            _mockCalculadoraFinanceira
                .Setup(m => m.CalcularDesconto(valorOriginal, percentualDesconto))
                .Returns(esperado);

            // Act
            decimal resultado = _serviceCalculadoraFinanceira.CalcularDesconto(valorOriginal, percentualDesconto);

            // Assert
            Assert.Equal(esperado, resultado);

            _mockCalculadoraFinanceira
               .Verify(m => m.CalcularDesconto(
                   It.IsAny<decimal>(),
                   It.IsAny<decimal>()), Times.Once);
        }
        #endregion
    }
}
