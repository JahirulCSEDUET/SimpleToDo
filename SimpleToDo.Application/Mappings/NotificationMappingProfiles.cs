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
                .ForCtorParam("RedirectLink", o => o.MapFrom(d => d.RedirectLink.ToString()))
                .ForCtorParam("TimeAgo", o=> o.MapFrom(src => (int)(DateTime.Now - src.CreatedTime).TotalMinutes));

        }
    }
}
