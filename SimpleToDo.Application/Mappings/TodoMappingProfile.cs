using AutoMapper;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Application.Features.Todos.Commands;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;

namespace SimpleToDo.Web.MappingProfiles
{
    public class TodoMappingProfile:Profile
    {
        public TodoMappingProfile()
        {
            CreateMap<Query, QueryDto>()
                .ForMember(dst => dst.UserName, otp => otp.MapFrom(src => src.User.FullName));
            CreateMap<Todo, TodoDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.HasValue ? src.UserId.Value : 0))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dst=>dst.QueryList, opt => opt.MapFrom(src=>src.Queries));

            //ViewModel --> Entity
            CreateMap<CreateTodoCommand, Todo>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Status.Pending))
                .ForMember(dest => dest.IsArchived, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
