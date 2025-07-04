﻿namespace Implementations.ContaCorrenteExemplo.Domain
{
    public class ContaCorrente
    {
        private const decimal SALDO_INICIAL = 10000;
        private const decimal LIMITE_INICIAL = 20000;

        public ContaCorrente(string responsavel, string documento, int agencia, int digito, int numero, string email, string telefone)
        {
            Responsavel = responsavel;
            Documento = documento;
            Agencia = agencia;
            Digito = digito;
            Numero = numero;
            Email = email;
            Telefone = telefone;

            Saldo = SALDO_INICIAL;
            Limite = LIMITE_INICIAL;
        }

        public string Responsavel { get; private set; }
        public string Documento { get; private set; }
        public int Agencia { get; private set; }
        public int Digito { get; private set; }
        public int Numero { get; private set; }
        public decimal Saldo { get; private set; }
        public decimal Limite { get; private set; }
        public string Email { get; private set; }
        public string Telefone { get; private set; }
        private bool TemLimite(decimal valor)
        {
            return Limite >= valor;
        }

        private bool TemSaldo(decimal valor)
        {
            return Saldo >= valor;
        }

        public bool PodeTransferir(decimal valor)
        {
            return TemLimite(valor) && TemSaldo(valor);
        }

        public void Debitar(decimal valor)
        {
            Saldo -= valor;
            Limite -= valor;
        }

        public void Creditar(decimal valor)
        {
            Saldo += valor;
        }
    }
}
