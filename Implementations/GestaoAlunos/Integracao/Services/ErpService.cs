using Implementations.GestaoAlunos.Integracao.Interfaces;

namespace Implementations.GestaoAlunos.Integracao.Services
{
    public class ErpService : IErpService
    {
        public Task<bool> SyncStudent(ErpAluno aluno)
        {
            var randomNumber = new Random().Next(100);

            return Task.FromResult(randomNumber % 2 == 0);
        }
    }
}
