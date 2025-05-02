using Implementations.GestaoAlunos.Domain;

namespace Implementations.GestaoAlunos.Integracao
{
    public class ErpAluno
    {
        public ErpAluno(string fullName, string schoolClass, string document, DateTime birthDate)
        {
            full_name = fullName;
            school_class = schoolClass;
            this.document = document;
            birth_date = birthDate;
        }

        public string full_name { get; private set; }
        public string school_class { get; private set; }
        public string document { get; private set; }
        public DateTime birth_date { get; private set; }

        public static ErpAluno FromEntity(Aluno aluno)
            => new ErpAluno(aluno.FullName, aluno.Class, aluno.Document, aluno.BirthDate);
    }
}
