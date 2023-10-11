using AutoMapper;
using Common.Model.ServiceBus;
using Core.Accounts.DAL.Entity;
using Core.Accounts.Service.Accounts.Query.Response;
using Core.Accounts.Service.Roles.Query.Response;
using HashidsNet;

namespace Core.Accounts.Service.MapperProfile
{
    public class MapperProfile : Profile
    {
        private readonly IHashids _hashids;

        public MapperProfile(IHashids hashids)
        {
            _hashids = hashids;
            CreateMapper();
        }

        private void CreateMapper()
        {
            CreateMap<Account, AccountsResponse>().ForMember(dst => dst.Id, opt => opt.MapFrom(src => _hashids.Encode(src.Id)));

            CreateMap<Account, AccountsBaseResponse>().ForMember(dst => dst.Id, opt => opt.MapFrom(src => _hashids.Encode(src.Id)));

            CreateMap<Account, AccountServiceBusMessageObject>()
                .ForMember(dst => dst.AccountID, opt => opt.MapFrom(src => _hashids.Encode(src.Id)))
                .ForMember(dst => dst.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dst => dst.Token, opt => opt.MapFrom(src => src.VerificationToken != null ? src.VerificationToken : src.ResetToken))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email));


            CreateMap<Role, RoleBaseResponse>()
             .ForMember(dst => dst.Id, opt => opt.MapFrom(src => _hashids.Encode(src.Id)))
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<RoleBaseResponse, Role>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => _hashids.DecodeSingle(src.Id)))
               .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Role, RoleResponse>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => _hashids.Encode(src.Id)))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<RoleResponse, Role>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => _hashids.DecodeSingle(src.Id)))
               .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
