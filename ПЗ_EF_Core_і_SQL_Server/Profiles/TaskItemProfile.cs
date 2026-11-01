using AutoMapper;
using TaskTracker.API.DTOs;
using TaskTracker.API.Models;

namespace TaskTracker.API.Profiles
{
    public class TaskItemProfile : Profile
    {
        public TaskItemProfile()
        {
            CreateMap<TaskItem, TaskItemReadDto>()
                .ForMember(dto => dto.ProjectName, options => options.MapFrom(task => task.Project.Name));

            CreateMap<TaskItemCreateDto, TaskItem>()
                .ForMember(task => task.Id, options => options.Ignore())
                .ForMember(task => task.CreatedAt, options => options.Ignore())
                .ForMember(task => task.IsDeleted, options => options.Ignore())
                .ForMember(task => task.Project, options => options.Ignore());

            CreateMap<TaskItemUpdateDto, TaskItem>()
                .ForMember(task => task.Id, options => options.Ignore())
                .ForMember(task => task.ProjectId, options => options.Ignore())
                .ForMember(task => task.CreatedAt, options => options.Ignore())
                .ForMember(task => task.IsDeleted, options => options.Ignore())
                .ForMember(task => task.Project, options => options.Ignore());
        }
    }
}
