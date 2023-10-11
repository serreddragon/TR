using AutoMapper;
using Common.Model.Response;
using Core.Accounts.Service.Interface;
using Core.Accounts.Service.Roles.Command;
using Core.Accounts.Service.Roles.Query.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Accounts.API.Controllers.V1
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
#pragma warning disable CS1591
    public class RolesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRolesService _roleService;

        public RolesController(IMapper mapper, IRolesService roleService)
        {
            _mapper = mapper;
            _roleService = roleService;
        }

        /// <summary>
        /// Return All Roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "super-admin, admin")]
        public async Task<Response<List<RoleResponse>>> GetAll()
        {
            var roles = await _roleService.GetAll();
            return new Response<List<RoleResponse>>(roles);
        }

        /// <summary>
        /// Create Initial Roles for new tenant
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-tenant-rolles")]
        [Authorize(Roles = "M2M_role")]
        public async Task<Response<bool>> CreateTenantRoles([FromBody] CreateRoleCommand cmd)
        {
            var result = await _roleService.CreateRoles(cmd);
            return new Response<bool>(result);
        }

        /// <summary>
        /// Create Initial Roles for newly created tenant
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "super-admin, admin")]
        public async Task<Response<bool>> Create([FromBody] CreateRoleCommand cmd)
        {
            var result = await _roleService.CreateRole(cmd);
            return new Response<bool>(result);
        }

        /// <summary>
        /// Get rolles for given tenantId
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{tenantId}")]
        public async Task<Response<List<RoleResponse>>> GetRolesByTenantId(string tenantId)
        {
            var roles = await _roleService.GetRolesByTenantId(tenantId);
            return new Response<List<RoleResponse>>(roles);
        }
    }
}
