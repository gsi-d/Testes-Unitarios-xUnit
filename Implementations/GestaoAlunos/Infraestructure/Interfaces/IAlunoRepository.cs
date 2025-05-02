using Implementations.GestaoAlunos.Domain;

namespace Implementations.GestaoAlunos.Infraestructure.Interfaces
{
    public interface IAlunoRepository
    {
        Task AddAsync(Aluno aluno);
    }
}
