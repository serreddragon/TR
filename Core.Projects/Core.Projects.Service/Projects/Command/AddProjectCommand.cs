using Common.Enums;
using Core.Projects.DAL;
using FluentValidation;

namespace Core.Projects.Service.Projects.Command
{
    public class AddProjectCommand
    {
        public string? Name { get; set; }

        public ProjectType Type { get; set; }
    }

    public class AddProjectCommandValidator : AbstractValidator<AddProjectCommand>
    {
        private readonly ProjectsDbContext _ctx;

        public AddProjectCommandValidator(ProjectsDbContext ctx)
        {
            _ctx = ctx;

            RuleFor(cmd => cmd.Name).NotEmpty();
            RuleFor(cmd => cmd.Type).NotEmpty();
        }
    }
}
