using Implementations.GestaoAlunos.Domain;
using MediatR;

namespace Implementations.GestaoAlunos.Application
{
    public class AlunoRequest : IRequest<AlunoResponse>
    {
        public string FullName { get; set; }
        public string Document { get; set; }
        public string Class { get; set; }
        public DateTime BirthDate { get; set; }

        public Aluno ToEntity()
            => new Aluno(FullName, Document, Class, BirthDate);
    }
}
