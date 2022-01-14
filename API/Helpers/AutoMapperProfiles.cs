using AutoMapper;
using API.Entities;
using API.DTOs;
using System.Linq;
using API.Extensions;
using System;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // ForMember for Age was added, because we removed GetAge() method in AppUser class
            CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                     src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                     src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Message, MessageDto>()
             .ForMember(dst => dst.SenderPhotoUrl, opt => opt.MapFrom(src => 
              src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
             .ForMember(dst => dst.RecipientPhotoUrl, opt => opt.MapFrom(src =>
              src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));

            //// we no longer need this code to convert the date to UTcNow, because we added 
            //// class in DataContext class
            // CreateMap<DateTime, DateTime>().ConvertUsing
            //     (d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        }
    }
}