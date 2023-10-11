using AutoMapper;
using Common.Model.Response;
using Common.Model.Search;
using Core.Tenants.Service.Interfaces;
using Core.Tenants.Service.Tenant.Command;
using Core.Tenants.Service.Tenant.Querry.Request;
using Core.Tenants.Service.Tenant.Querry.Response;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Tenants.API.Controllers
{
    /// <summary>
    /// Tenants controller
    /// </summary>
    /// 
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantsService;
        private readonly IMapper _mapper;
        private readonly IHashids _hashids;

        public TenantsController(ITenantService tenantsService, IMapper mapper, IHashids hashids)
        {
            _tenantsService = tenantsService;
            _mapper = mapper;
            _hashids = hashids;
        }
        /// <summary>
        /// Return an Tenant by ID
        /// </summary>
        [Route("{id}")]
        [HttpGet]
        [Authorize(Roles = "super-admin, admin, client")]
        public async Task<Response<TenantResponse>> Get(string id)
        {
            var tenant = await _tenantsService.Get(_hashids.DecodeSingle(id));

            return new Response<TenantResponse>(tenant);
        }

        /// <summary>
        /// Creates new Tenant
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "super-admin, admin, client")]
        public async Task<Response<TenantResponse>> Create([FromBody] CreateTenantCommand tenant)
        {
            var created = await _tenantsService.CreateTenant(tenant);
            return await Get(created.Id);
        }

        /// <summary>
        /// Initial create new tenant if not exists, create it's roles and assign tenant to account
        /// </summary>
        /// <param name="createTenantCommand"></param>
        /// /// <param name="accountId"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("signup")]
        [AllowAnonymous]
        public async Task<Response<TenantResponse>> CreateTenant(string accountId, [FromBody] CreateTenantCommand createTenantCommand)
        {
            var created = await _tenantsService.SignUpTenant(accountId, createTenantCommand);

            return await Get(created.Id);
        }

        /// <summary>
        /// Updates tenant
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "super-admin, admin")]
        public async Task<Response<bool>> Update([FromBody] UpdateTenantCommand tenant)
        {
            return new Response<bool>(await _tenantsService.UpdateTenant(tenant));
        }

        /// <summary>
        /// Returns all tenants
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        [Authorize(Roles = "super-admin, admin, client")]
        public async Task<Response<List<TenantResponse>>> GetAll()
        {
            var accounts = await _tenantsService.GetAllTenants();

            return new Response<List<TenantResponse>>(accounts);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("name/{name}")]
        [Authorize(Roles = "super-admin, admin, client")]
        public async Task<Response<TenantResponse>> GetByName([FromRoute] string name)
        {
            var existingTenant = await _tenantsService.GetByName(name);

            return new Response<TenantResponse>(existingTenant);
        }

        /// <summary>
        /// Search tenants by given criteria
        /// </summary>
        /// <param name="tenantQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "super-admin, admin, client")]
        public async Task<Response<SearchResponse<TenantBaseResponse>>> Search([FromBody] TenantQuery tenantQuery)
        {
            var searchResult = await _tenantsService.Search(tenantQuery);

            return new Response<SearchResponse<TenantBaseResponse>>(searchResult);
        }
    }
}
