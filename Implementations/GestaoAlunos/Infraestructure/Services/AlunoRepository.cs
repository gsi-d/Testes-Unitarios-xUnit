using Implementations.GestaoAlunos.Domain;

namespace Implementations.GestaoAlunos.Infraestructure.Services
{
    public class AlunoRepository
    {
        public Task AddAsync(Aluno aluno)
        {
            return Task.CompletedTask;
        }
    }
}
