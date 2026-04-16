using AutoMapper;
using DTO.EmailNotificationDTOs;
using OfisUrunTakip.WebApi.Entity;

namespace Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            CreateMap<EmailNotification, EmailNotificationListDto>()
                .ForMember(d => d.UserName,
                    o => o.MapFrom(s => s.User != null ? s.User.Name : ""))
                .ForMember(d => d.UserEmail,
                    o => o.MapFrom(s => s.User != null ? s.User.Email : ""))
                .ForMember(d => d.UserRole,
                    o => o.MapFrom(s => s.User != null ? s.User.Role : ""));

            CreateMap<EmailNotificationAddDto, EmailNotificationSetting>();

            
            CreateMap<EmailNotificationUpdateDto, EmailNotificationSetting>();

            CreateMap<EmailNotificationSetting, EmailNotificationListDto>();
        }
    }
}
