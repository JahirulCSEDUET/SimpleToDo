using AutoMapper;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Domain.Entities;

namespace SimpleToDo.Application.Mappings
{
    public class ProjectMappingProfile:Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<ProjectMember, ProjectMemberDto>()
                .ForCtorParam("userName", opt => opt.MapFrom(src => src.User.FullName));
            CreateMap<Query, QueryDto>()
                .ForCtorParam("UserName", opt => opt.MapFrom(src => src.User.FullName));
            CreateMap<Todo, TodoDto>()
                .ForCtorParam("UserName", opt => opt.MapFrom(src => src.User.FullName))
                .ForCtorParam("ProjectName", opt => opt.MapFrom(src => src.Project.Name))
                .ForCtorParam("UserId", opt => opt.MapFrom(src => src.UserId.HasValue ? src.UserId.Value : 0))
                .ForCtorParam("QueryList", opt => opt.MapFrom(src => src.Queries));
            CreateMap<Project, ProjectDto>()
                .ForCtorParam("TodoList", opt => opt.MapFrom(src => src.Todos));
            CreateMap<Project, ProjectListDto>()
                .ForMember(dst => dst.ProjectMembers, otp => otp.MapFrom(s => s.ProjectMembers));

        }
    }
}
