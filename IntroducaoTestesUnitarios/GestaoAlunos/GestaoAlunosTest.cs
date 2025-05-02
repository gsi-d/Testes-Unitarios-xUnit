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
        [Fact]
        public async Task ValidaAluno_ViaMediator_RetornaAlunoResponseValido()
        {
            // Arrange
            var addAluno = new Fixture().Create<AlunoRequest>();

            var studentRepositoryMock = new Mock<IAlunoRepository>();
            var erpIntegrationService = new Mock<IErpService>();
            erpIntegrationService.Setup(e => e.SyncStudent(It.IsAny<ErpAluno>())).Returns(Task.FromResult(true));

            var addAlunoHandler = new AddAlunoHandler(studentRepositoryMock.Object, erpIntegrationService.Object);

            // Act
            var result = await addAlunoHandler.Handle(addAluno, new CancellationToken());

            // Assert
            result.FullName.ShouldBe(addAluno.FullName);
            result.Document.ShouldBe(addAluno.Document);
            result.Class.ShouldBe(addAluno.Class);
            result.BirthDate.ShouldBe(addAluno.BirthDate);

            studentRepositoryMock.Verify(s => s.AddAsync(It.IsAny<Aluno>()), Times.Once);
            erpIntegrationService.Verify(s => s.SyncStudent(It.IsAny<ErpAluno>()), Times.Once);
        }
    }
}
