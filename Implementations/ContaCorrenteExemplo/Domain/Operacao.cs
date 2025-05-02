namespace Implementations.ContaCorrenteExemplo.Domain
{
    public class Operacao
    {
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public TipoOperacao Tipo { get; set; }
    }

    public enum TipoOperacao
    {
        Debito = 1,
        Credito = 2
    }
}
