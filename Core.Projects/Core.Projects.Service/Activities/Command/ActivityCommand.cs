using Common.Enums;
using Core.Projects.DAL;
using FluentValidation;

namespace Core.Projects.Service.Activities.Command
{
    public class ActivityCommand
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsInternal { get; set; } = false;
        public DateTime? DueDate { get; set; }
        public int? ParentId { get; set; }
        public ActivityPriority Priority { get; set; }

        public int ActivityGroupId { get; set; }

    }
    public class ActivityCommandValidator : AbstractValidator<ActivityCommand>
    {
        private readonly ProjectsDbContext _ctx;

        public ActivityCommandValidator(ProjectsDbContext ctx)
        {
            _ctx = ctx;

            RuleFor(cmd => cmd.Name).NotEmpty();
            RuleFor(cmd => cmd.IsInternal).NotEmpty();
            RuleFor(cmd => cmd.ParentId).NotEmpty();
            RuleFor(cmd => cmd.ActivityGroupId).NotEmpty();
        }
    }
}
