using Core.Tenants.Service.AccountTenantMembership.Command;
using Core.Tenants.Service.AccountTenantMembership.Querry.Response;
using Core.Tenants.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Tenants.API.Controllers
{

    /// <summary>
    /// AccountTenantMembership controller
    /// </summary>
    /// 
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class AccountTenantMembershipController : ControllerBase
    {
        private readonly IAccountTenantMembershipService _accountTenantMembershipService;

        public AccountTenantMembershipController(IAccountTenantMembershipService accountTenantMembershipService)
        {
            _accountTenantMembershipService = accountTenantMembershipService;
        }

        /// <summary>
        /// Create connection between accounts and tenants
        /// </summary>
        /// <param name="createCommand"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<AccountTenantMembershipResponse> Create(CreateAccountTenantMembershipCommand createCommand)
        {
            var membership = await _accountTenantMembershipService.Create(createCommand, true);
            return await _accountTenantMembershipService.Get(membership.Id);
        }

        /// <summary>
        /// Updates membership
        /// </summary>
        /// <param name="updateCommand"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<AccountTenantMembershipResponse> Update(UpdateAccountTenantMembershipCommand updateCommand)
        {
            var membership = await _accountTenantMembershipService.Update(updateCommand);
            return await _accountTenantMembershipService.Get(membership.Id);
        }

        /// <summary>
        /// Returs existing memberships by Tenant
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-by-tenant/{tenantId}")]
        public async Task<List<AccountTenantMembershipResponse>> GetByTenant(string tenantId)
        {
            return await _accountTenantMembershipService.GetByTenantId(tenantId);
        }

        /// <summary>
        /// Returns existing membership by Account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-by-account/{accountId}")]
        public async Task<List<AccountTenantMembershipResponse>> GetByAccount(string accountId)
        {
            return await _accountTenantMembershipService.GetByAccountId(accountId);
        }
    }
}
