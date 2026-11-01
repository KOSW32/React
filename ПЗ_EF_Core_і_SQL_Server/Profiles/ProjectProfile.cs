using AutoMapper;
using TaskTracker.API.DTOs;
using TaskTracker.API.Models;

namespace TaskTracker.API.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectReadDto>()
                .ForMember(dto => dto.TasksCount, options => options.MapFrom(project => project.Tasks.Count));

            CreateMap<ProjectCreateDto, Project>()
                .ForMember(project => project.Id, options => options.Ignore())
                .ForMember(project => project.CreatedAt, options => options.Ignore())
                .ForMember(project => project.IsDeleted, options => options.Ignore())
                .ForMember(project => project.Tasks, options => options.Ignore());

            CreateMap<ProjectUpdateDto, Project>()
                .ForMember(project => project.Id, options => options.Ignore())
                .ForMember(project => project.CreatedAt, options => options.Ignore())
                .ForMember(project => project.IsDeleted, options => options.Ignore())
                .ForMember(project => project.Tasks, options => options.Ignore());
        }
    }
}
