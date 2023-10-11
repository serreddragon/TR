using AutoMapper;
using Common.Services;
using Core.Accounts.DAL;
using Core.Accounts.DAL.Entity;
using Core.Accounts.Service.Interface;
using Core.Accounts.Service.Roles.Command;
using Core.Accounts.Service.Roles.Extensions;
using Core.Accounts.Service.Roles.Query.Request;
using Core.Accounts.Service.Roles.Query.Response;
using Core.Tenants.Service.Tenant.Command;
using HashidsNet;
using Localization.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Accounts.Service.Accounts
{
    public sealed class RolesService : BaseService<AccountsDbContext, Role, RoleResponse, RoleBaseResponse, RoleQuery>, IRolesService
    {
        private readonly CreateRoleCommandValidator _createRoleCommandValidator;
        public RolesService(AccountsDbContext ctx,
                    IMapper mapper,
                    IHashids hashids,
                    IStringLocalizer<SharedResource> stringLocalizer,
                   CreateRoleCommandValidator createRoleCommandValidator)
      : base(ctx, mapper, stringLocalizer, hashids)
        {
            _createRoleCommandValidator = createRoleCommandValidator;
        }
        public async Task<bool> CreateRole(CreateRoleCommand command)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                _createRoleCommandValidator.Validate(command);

                var existingRoles = _mapper.Map<List<Role>>(await GetRolesByTenantId(command.TenantId));
                var role = new Role { Name = command.RoleName, TenatId = _hashids.DecodeSingle(command.TenantId) };


                if (!existingRoles.Any(r => r == role))
                    await _ctx.Roles.AddAsync(role);

                await _ctx.SaveChangesAsync();
                scope.Complete();
                return true;
            }
        }

        public async Task<bool> CreateRoles(CreateRoleCommand command)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                _createRoleCommandValidator.Validate(command);

                List<Role> roles = command.InitialTenantRoles(_hashids);
                var existingRoles = _mapper.Map<List<Role>>(await GetRolesByTenantId(command.TenantId));

                existingRoles.ForEach(x => { if (roles.Contains(x)) roles.Remove(x); });
                await _ctx.Roles.AddRangeAsync(roles);
                await _ctx.SaveChangesAsync();
                scope.Complete();
                return true;
            }
        }

        public async Task<List<RoleResponse>> GetAll()
        {
            var result = await _ctx.Roles.ToListAsync();

            return _mapper.Map<List<RoleResponse>>(result);
        }
        public async Task<List<RoleResponse>> GetRolesByTenantId(string tenantId)
        {
            var roles = await _ctx.Roles.Where(r => r.TenatId == _hashids.DecodeSingle(tenantId))?.ToListAsync();
            return _mapper.Map<List<RoleResponse>>(roles);
        }
    }
}
