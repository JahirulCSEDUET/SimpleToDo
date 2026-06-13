using AutoMapper;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Web.ViewModels.Project;
using SimpleToDo.Web.ViewModels.ToDo;

namespace SimpleToDo.Web.MappingProfiles
{
    public class ProjectMappingProfile:Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<ProjectMember, ProjectMemberListViewModel>()
                .ForMember(dst=> dst.UserName,opt=> opt.MapFrom(s=>s.User.FullName));
            CreateMap<Todo, ToDoItemListViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.HasValue ? src.UserId.Value : 0))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId.HasValue ? src.ProjectId.Value : 0));
            CreateMap<Project, ProjectDetailsViewModel>()
                .ForMember(dst => dst.TodoList, otp => otp.MapFrom(s => s.Todos))
                .ForMember(dst => dst.ProjectMemberList, otp => otp.MapFrom(s => s.ProjectMembers))
                .ForMember(dst => dst.DetailsViewerRole, otp => otp.Ignore());
            CreateMap<Project, ProjectListViewModel>();
        }
    }
}
