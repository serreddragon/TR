using Core.Projects.DAL;
using FluentValidation;

namespace Core.Projects.Service.ProjectPhases.Command
{
    public class AddProjectPhaseCommand
    {
        public string? Name { get; set; }
        public int ProjectId { get; set; }

    }
    public class AddProjectPhaseCommandValidator : AbstractValidator<AddProjectPhaseCommand>
    {
        private readonly ProjectsDbContext _ctx;

        public AddProjectPhaseCommandValidator(ProjectsDbContext ctx)
        {
            _ctx = ctx;

            RuleFor(cmd => cmd.Name).NotEmpty();
            RuleFor(cmd => cmd.ProjectId).NotEmpty();
        }
    }

}
