using AutoMapper;
using Core.Projects.DAL;
using Core.Projects.DAL.Entity;
using Core.Projects.Service.Interfaces;
using Core.Projects.Service.ProjectPhases.Command;
using Core.Projects.Service.ProjectPhases.Query.Request;
using Core.Projects.Service.ProjectPhases.Query.Response;
using Core.Projects.Service.Services;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Core.Projects.Service.ProjectPhases
{
    public class ProjectPhasesService : BaseService<ProjectPhase, ProjectPhaseResponse, ProjectPhaseBaseResponse, ProjectPhaseRequest>, IProjectPhasesService
    {
        private readonly AddProjectPhaseCommandValidator _addProjectPhaseCommandValidator;
        private readonly UpdateProjectPhaseCommandValidator _updateProjectPhaseCommandValidator;

        public ProjectPhasesService(ProjectsDbContext ctx, AddProjectPhaseCommandValidator addProjectPhaseCommandValidator, UpdateProjectPhaseCommandValidator updateProjectPhaseCommandValidator, IMapper mapper) : base(ctx, mapper)
        {
            _addProjectPhaseCommandValidator = addProjectPhaseCommandValidator;
            _updateProjectPhaseCommandValidator = updateProjectPhaseCommandValidator;
        }

        public async Task<List<ProjectPhaseResponse>> GetProjectPhases()
        {
            List<ProjectPhase> projectPhases = new();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                projectPhases = await _ctx.ProjectPhases.ToListAsync();
            }

            return _mapper.Map<List<ProjectPhaseResponse>>(projectPhases);
        }

        public async Task<ProjectPhaseResponse> GetProjectPhase(int id)
        {
            ProjectPhase projectPhase = null;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                projectPhase = await _ctx.ProjectPhases.FirstAsync(x => x.Id == id);
            }

            return _mapper.Map<ProjectPhaseResponse>(projectPhase);
        }

        public async Task<ProjectPhaseResponse> CreateProjectPhase(AddProjectPhaseCommand cmd)
        {
            _addProjectPhaseCommandValidator.Validate(cmd);
            ProjectPhase projectPhase = null;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                projectPhase = new ProjectPhase
                {
                    Name = cmd.Name,
                    ProjectId = cmd.ProjectId,
                };

                await _ctx.ProjectPhases.AddAsync(projectPhase);
                await _ctx.SaveChangesAsync();

                scope.Complete();
            }

            projectPhase.CreatedById = projectPhase.Id;
            await _ctx.SaveChangesAsync();

            return _mapper.Map<ProjectPhaseResponse>(projectPhase);
        }
        public async Task<ProjectPhaseResponse> UpdateProjectPhase(int id, UpdateProjectPhaseCommand cmd)
        {
            _updateProjectPhaseCommandValidator.Validate(cmd);
            ProjectPhase projectPhase = null;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                projectPhase = await _ctx.ProjectPhases.FirstAsync(x => x.Id == id);

                projectPhase.Name = cmd.Name;

                projectPhase.UpdatedById = id;

                await _ctx.SaveChangesAsync();

                scope.Complete();
            }

            return _mapper.Map<ProjectPhaseResponse>(projectPhase);
        }
        public async Task<string> DeleteProjectPhase(int id)
        {
            _ctx.ProjectPhases.Remove(_ctx.ProjectPhases.First(x => x.Id == id));

            var response = await _ctx.SaveChangesAsync();

            return response.ToString();
        }
    }
}
