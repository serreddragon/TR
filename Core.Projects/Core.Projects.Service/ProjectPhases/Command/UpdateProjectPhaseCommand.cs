using Core.Projects.DAL;
using FluentValidation;

namespace Core.Projects.Service.ProjectPhases.Command
{
    public class UpdateProjectPhaseCommand
    {
        public string? Name { get; set; }

    }
    public class UpdateProjectPhaseCommandValidator : AbstractValidator<UpdateProjectPhaseCommand>
    {
        private readonly ProjectsDbContext _ctx;

        public UpdateProjectPhaseCommandValidator(ProjectsDbContext ctx)
        {
            _ctx = ctx;

            RuleFor(cmd => cmd.Name).NotEmpty();
        }
    }
}
