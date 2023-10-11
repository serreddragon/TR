using AutoMapper;
using Core.Projects.DAL.Entity;
using Core.Projects.Service.Activities.Query.Responce;
using Core.Projects.Service.ActivityGroups.Query.Responce;
using Core.Projects.Service.ProjectPhases.Query.Response;
using Core.Projects.Service.Projects.Query.Response;

namespace Core.Projects.Service.MapperProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMapper();
        }

        private void CreateMapper()
        {
            // CreateMap<List<Project>, List<ProjectResponse>>();
            CreateMap<Project, ProjectsResponse>();


            CreateMap<ProjectPhase, ProjectPhaseResponse>();

            CreateMap<ActivityGroup, ActivityGroupResponse>();

            CreateMap<Activity, ActivityResponse>();
        }
    }
}
