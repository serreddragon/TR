using AutoMapper;
using Core.Projects.DAL;
using Core.Projects.DAL.Entity;
using Core.Projects.Service.Interfaces;
using Core.Projects.Service.Projects.Command;
using Core.Projects.Service.Projects.Query.Request;
using Core.Projects.Service.Projects.Query.Response;
using Core.Projects.Service.Services;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Core.Projects.Service.Projects
{
    public sealed class ProjectsService : BaseService<Project, ProjectsResponse, ProjectsBaseResponse, ProjectRequest>, IProjectsService
    {

        private readonly AddProjectCommandValidator _addProjectCommandValidator;
        private readonly UpdateProjectCommandValidator _updateProjectCommandValidator;
        public ProjectsService(ProjectsDbContext ctx, AddProjectCommandValidator addProjectCommandValidator, UpdateProjectCommandValidator updateProjectCommandValidator, IMapper mapper) : base(ctx, mapper)
        {
            _addProjectCommandValidator = addProjectCommandValidator;
            _updateProjectCommandValidator = updateProjectCommandValidator;
        }
        public async Task<List<ProjectsResponse>> GetProjects()
        {
            List<Project> projects = new();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                projects = await _ctx.Projects.ToListAsync();
            }

            return _mapper.Map<List<ProjectsResponse>>(projects);
        }

        public async Task<ProjectsResponse> GetProject(int id)
        {
            Project project = null;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                project = await _ctx.Projects.FirstAsync(x => x.Id == id);
            }

            return _mapper.Map<ProjectsResponse>(project);
        }

        public async Task<ProjectsResponse> CreateProject(AddProjectCommand cmd)
        {
            _addProjectCommandValidator.Validate(cmd);
            Project project = null;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                project = new Project
                {
                    Name = cmd.Name,
                    Type = cmd.Type,
                    ProjectPrefix = cmd.Name.Substring(0, 3).ToUpper(),
                    Description = "",
                };

                await _ctx.Projects.AddAsync(project);
                await _ctx.SaveChangesAsync();

                scope.Complete();
            }

            project.CreatedById = project.Id;
            await _ctx.SaveChangesAsync();

            return _mapper.Map<ProjectsResponse>(project);
        }


        public async Task<ProjectsResponse> UpdateProject(int id, UpdateProjectCommand cmd)
        {
            _updateProjectCommandValidator.Validate(cmd);
            Project project = null;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                project = await _ctx.Projects.FirstAsync(x => x.Id == id);

                project.Name = cmd.Name;
                project.ProjectPrefix = cmd.ProjectPrefix;
                project.Description = cmd.Description;
                project.Type = cmd.Type;

                project.UpdatedById = id;

                await _ctx.SaveChangesAsync();

                scope.Complete();
            }

            return _mapper.Map<ProjectsResponse>(project);
        }

        public async Task<string> DeleteProject(int id)
        {
            _ctx.Projects.Remove(_ctx.Projects.First(x => x.Id == id));

            var response = await _ctx.SaveChangesAsync();

            return response.ToString();
        }
    }
}
