using AutoMapper;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Mappings
{
    public class NotificationMappingProfiles:Profile
    {
        public NotificationMappingProfiles() 
        {
            CreateMap<Notification, NotificationDto>()
                .ForMember(d => d.RedirectLink, o => o.MapFrom(d => d.RedirectLink.ToString()))
                .ForMember(d=> d.TimeAgo, o=> o.MapFrom(d=> (DateTime.Now -d.CreatedTime).Minutes));

        }
    }
}
