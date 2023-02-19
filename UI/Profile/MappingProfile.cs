using AutoMapper;
using ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Models;

namespace UI.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, FirstEntranceAuthorViewModel>();
            CreateMap<FirstEntranceAuthorViewModel, AppUser>()
                .ForMember(x => x.ConcurrencyStamp, act => act.Ignore())
                .ForMember(x => x.CreateDate, act => act.Ignore())
                .ForMember(x => x.DeleteDate, act => act.Ignore())
                .ForMember(x => x.Articles, act => act.Ignore())
                .ForMember(x => x.AccessFailedCount, act => act.Ignore())
                .ForMember(x => x.EmailConfirmed, act => act.Ignore())
                .ForMember(x => x.Followers, act => act.Ignore())
                .ForMember(x => x.Id, act => act.Ignore())
                .ForMember(x => x.LockoutEnabled, act => act.Ignore())
                .ForMember(x => x.LockoutEnd, act => act.Ignore())
                .ForMember(x => x.NormalizedEmail, act => act.Ignore())
                .ForMember(x => x.NormalizedUserName, act => act.Ignore())
                .ForMember(x => x.PasswordHash, act => act.Ignore())
                .ForMember(x => x.PhoneNumberConfirmed, act => act.Ignore())
                .ForMember(x => x.ProfilePhotoPath, act => act.Ignore())
                .ForMember(x => x.SecurityStamp, act => act.Ignore())
                .ForMember(x => x.Status, act => act.Ignore())
                .ForMember(x => x.TwoFactorEnabled, act => act.Ignore())
                .ForMember(x => x.UpdateDate, act => act.Ignore())
                .ForMember(x => x.UserName, act => act.Ignore());
               
                



        }
    }
}
