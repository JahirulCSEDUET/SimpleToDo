using AutoMapper;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Application.Features.Todos.Commands;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;

namespace SimpleToDo.Application.Mappings
{
    public class TodoMappingProfile:Profile
    {
        public TodoMappingProfile()
        {

            //ViewModel --> Entity
            CreateMap<CreateTodoCommand, Todo>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Status.Pending))
                .ForMember(dest => dest.IsArchived, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
