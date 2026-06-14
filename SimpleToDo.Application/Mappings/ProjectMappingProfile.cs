using AutoMapper;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Domain.Entities;

namespace SimpleToDo.Application.MappingProfiles
{
    public class ProjectMappingProfile:Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<ProjectMember, ProjectMemberDto>()
                .ForMember(dst=> dst.UserName,opt=> opt.MapFrom(s=>s.User.FullName));
            CreateMap<Todo, TodoDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.HasValue ? src.UserId.Value : 0))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId));
            CreateMap<Project, ProjectDto>()
                .ForMember(dst => dst.TodoList, otp => otp.MapFrom(s => s.Todos))
                .ForMember(dst => dst.ProjectMemberList, otp => otp.MapFrom(s => s.ProjectMembers))
                .ForMember(dst => dst.DetailsViewerRole, otp => otp.Ignore());
            CreateMap<Project, ProjectListDto>()
                .ForMember(dst => dst.ProjectMembers, otp => otp.MapFrom(s => s.ProjectMembers));
        }
    }
}
