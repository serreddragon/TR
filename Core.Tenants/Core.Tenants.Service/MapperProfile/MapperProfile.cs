using AutoMapper;
using Core.Tenants.Service.AccountTenantMembership.Command;
using Core.Tenants.Service.Tenant.Command;
using Core.Tenants.Service.Tenant.Querry.Response;
using TenantType = Core.Tenants.DAL.Entity.Tenant;
using AccountTenantMembershipType = Core.Tenants.DAL.Entity.AccountTenantMembership;
using Core.Tenants.Service.AccountTenantMembership.Querry.Response;
using HashidsNet;
using Core.Tenants.Infrastructure.Models;

namespace Core.Tenants.Service.MapperProfile
{
    public class MapperProfile : Profile
    {
        private readonly IHashids _hashIds;

        public MapperProfile(IHashids hashIds)
        {
            _hashIds = hashIds;
            CreateMapper();

        }
        private void CreateMapper()
        {
            CreateMap<TenantType, TenantBaseResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => _hashIds.Encode(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<TenantType, TenantResponse>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => _hashIds.Encode(src.Id)))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<CreateTenantCommand, TenantType>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<AccountTenantMembershipType, AccountTenantMembershipResponse>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => _hashIds.Encode(src.Id)))
                 .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => _hashIds.Encode(src.AccountId)))
                 .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => _hashIds.Encode(src.TenantId)))
                 .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault));

            CreateMap<CreateAccountTenantMembershipCommand, AccountTenantMembershipType>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => _hashIds.DecodeSingle(src.AccountId)))
                .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => _hashIds.DecodeSingle(src.TenantId)))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault));

            CreateMap<CreateAccountTenantMembershipCommand, AssignAccountTenantRolesCommand>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId));
        }
    }
}