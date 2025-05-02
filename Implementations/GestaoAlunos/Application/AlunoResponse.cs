using Implementations.GestaoAlunos.Domain;

namespace Implementations.GestaoAlunos.Application
{
    public class AlunoResponse
    {
        public AlunoResponse(Guid id, string fullName, string document, string @class, DateTime birthDate)
        {
            Id = id;
            FullName = fullName;
            Document = document;
            Class = @class;
            BirthDate = birthDate;
        }

        public Guid Id { get; private set; }
        public string FullName { get; private set; }
        public string Document { get; private set; }
        public string Class { get; private set; }
        public DateTime BirthDate { get; private set; }

        public static AlunoResponse FromEntity(Aluno aluno)
            => new AlunoResponse(aluno.Id, aluno.FullName, aluno.Document, aluno.Class, aluno.BirthDate);
    }
}
