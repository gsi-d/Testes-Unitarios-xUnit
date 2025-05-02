namespace Implementations.GestaoAlunos.Integracao.Interfaces
{
    public interface IErpService
    {
        Task<bool> SyncStudent(ErpAluno aluno);
    }
}
