using Implementations.OperadoresMatematicos;

namespace IntroducaoTestesUnitarios.OperadoresMatematicos
{
    public class CalculadoraTest
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FallingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }

        bool IsOdd(int value)
        {
            return value % 2 == 1;
        }

        [Fact]
        public void Soma_DeveRetornarOValorCorreto()
        {
            Calculadora calc = new Calculadora();

            int resultado = calc.Soma(10, 20);
            //Verifica se o resultado é igual a 30
            Assert.Equal(30, resultado);
        }

        [Fact]
        public void Subtracao_DeveRetornarOValorCorreto()
        {
            Calculadora calc = new Calculadora();
            int resultado = calc.Subtracao(20, 10);
            //Verifica se o resultado é igual a 10
            Assert.Equal(10, resultado);
        }

        [Fact]
        public void Divisao_DeveRetornarOValorCorreto()
        {
            Calculadora calc = new Calculadora();
            int resultado = calc.Divisao(100, 10);
            //Verifica se o resultado é igual a 10
            Assert.Equal(10, resultado);
        }

        [Fact]
        public void Multiplicao_DeveRetornarOValorCorreto()
        {
            Calculadora c = new Calculadora();
            int resultado = c.Multiplicacao(5, 2);
            //Verifica se o resultado é igual a 10
            Assert.Equal(10, resultado);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)]
        public void Theory(int value)
        {
            Assert.True(IsOdd(value));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void RestoDivisao_DeveRetornarZero(int value)
        {
            Calculadora c = new Calculadora();
            var resultado = c.RestoDivisao(12, value);
            //Verifica se o resto da divisão é 0
            Assert.Equal(0, resultado.resto);
        }
    }
}