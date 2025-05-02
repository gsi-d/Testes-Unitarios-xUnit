namespace Implementations.GestaoAlunos.Domain
{
    public class Aluno
    {
        public Aluno(string fullName, string document, string @class, DateTime birthDate)
        {
            Id = Guid.NewGuid();

            FullName = fullName;
            Document = document;
            Class = @class;
            BirthDate = birthDate;

            CreatedAt = DateTime.Now;
        }

        public Guid Id { get; private set; }
        public string FullName { get; private set; }
        public string Document { get; private set; }
        public string Class { get; private set; }
        public DateTime BirthDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
    }
}
