using Core.Projects.DAL;
using FluentValidation;

namespace Core.Projects.Service.ActivityGroups.Command
{
    public class ActivityGroupCommand
    {
        public string? Name { get; set; }
        public int? OrderNo { get; set; }

        public int ProjectPhaseId { get; set; }

    }
    public class ActivityGroupCommandValidator : AbstractValidator<ActivityGroupCommand>
    {
        private readonly ProjectsDbContext _ctx;

        public ActivityGroupCommandValidator(ProjectsDbContext ctx)
        {
            _ctx = ctx;

            RuleFor(cmd => cmd.Name).NotEmpty();
            RuleFor(cmd => cmd.OrderNo).NotEmpty();
            RuleFor(cmd => cmd.ProjectPhaseId).NotEmpty();
        }
    }
}
