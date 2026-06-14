using AutoMapper;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Web.ViewModels.Queries;
using SimpleToDo.Web.ViewModels.ToDo;

namespace SimpleToDo.Web.MappingProfiles
{
    public class TodoMappingProfile:Profile
    {
        public TodoMappingProfile()
        {
            CreateMap<Query, QueryListViewModel>()
                .ForMember(dst => dst.UserName, otp => otp.MapFrom(src => src.User.FullName));
            CreateMap<Todo, ToDoItemDetailsViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.HasValue ? src.UserId.Value : 0))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dst=>dst.QueryList, opt => opt.MapFrom(src=>src.Queries));
            CreateMap<Todo, ToDoItemListViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.HasValue ? src.UserId.Value : 0))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId));

            //ViewModel --> Entity
            CreateMap<ToDoItemCreateViewModel, Todo>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Status.Pending))
                .ForMember(dest => dest.IsArchived, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreatorId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatorName, opt => opt.Ignore())
                .ForMember(dest => dest.FilePath, opt => opt.Ignore())
                .ForMember(dest => dest.FileName, opt => opt.Ignore());
        }
    }
}
