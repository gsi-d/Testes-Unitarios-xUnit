using AutoFixture;
using Implementations.GestaoAlunos.Application;
using Implementations.GestaoAlunos.Domain;
using Implementations.GestaoAlunos.Infraestructure.Interfaces;
using Implementations.GestaoAlunos.Integracao;
using Implementations.GestaoAlunos.Integracao.Interfaces;
using Moq;
using Shouldly;

namespace IntroducaoTestesUnitarios.GestaoAlunos
{
    public class GestaoAlunosTest
    {
        public Mock<IAlunoRepository> StudentRepositoryMock = new();
        public Mock<IErpService> ErpIntegrationService = new();

        [Fact]
        public async Task ValidaAluno_ViaMediator_RetornaAlunoResponseValido()
        {
            // Arrange
            AlunoRequest addAluno = new Fixture().Create<AlunoRequest>();

            ErpIntegrationService.Setup(e => e.SyncStudent(It.IsAny<ErpAluno>())).Returns(Task.FromResult(true));

            AddAlunoHandler addAlunoHandler = new AddAlunoHandler(StudentRepositoryMock.Object, ErpIntegrationService.Object);

            // Act
            AlunoResponse result = await addAlunoHandler.Handle(addAluno, new CancellationToken());

            // Assert
            result.FullName.ShouldBe(addAluno.FullName);
            result.Document.ShouldBe(addAluno.Document);
            result.Class.ShouldBe(addAluno.Class);
            result.BirthDate.ShouldBe(addAluno.BirthDate);

            StudentRepositoryMock.Verify(s => s.AddAsync(It.IsAny<Aluno>()), Times.Once);
            ErpIntegrationService.Verify(s => s.SyncStudent(It.IsAny<ErpAluno>()), Times.Once);
        }
    }
}
