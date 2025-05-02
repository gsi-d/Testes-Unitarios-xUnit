using Implementations.GestaoAlunos.Infraestructure.Interfaces;
using Implementations.GestaoAlunos.Integracao;
using Implementations.GestaoAlunos.Integracao.Interfaces;
using MediatR;

namespace Implementations.GestaoAlunos.Application
{
    public class AddAlunoHandler : IRequestHandler<AlunoRequest, AlunoResponse>
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IErpService _erpIntegrationService;
        public AddAlunoHandler(IAlunoRepository alunoRepository, IErpService erpIntegrationService)
        {
            _alunoRepository = alunoRepository;
            _erpIntegrationService = erpIntegrationService;
        }

        public async Task<AlunoResponse> Handle(AlunoRequest request, CancellationToken cancellationToken)
        {
            var aluno = request.ToEntity();
            await _alunoRepository.AddAsync(aluno);

            var erpAluno = ErpAluno.FromEntity(aluno);
            await _erpIntegrationService.SyncStudent(erpAluno);

            return AlunoResponse.FromEntity(aluno);
        }
    }
}
