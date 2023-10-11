using Common.Enums;
using Core.Projects.DAL;
using FluentValidation;

namespace Core.Projects.Service.Projects.Command
{
    public class UpdateProjectCommand
    {
        public string? ProjectPrefix { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ProjectType Type { get; set; }

        public List<int>? ProjectPhasesIds { get; set; } = new();
    }

    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        private readonly ProjectsDbContext _ctx;

        public UpdateProjectCommandValidator(ProjectsDbContext ctx)
        {
            _ctx = ctx;

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();

            RuleFor(x => x.ProjectPrefix).NotEmpty().Must(x => x.Length == 3);
        }
    }
}
