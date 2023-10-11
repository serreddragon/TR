using Common.Model.Response;
using Common.Model.Search;
using Core.Accounts.Service;
using Core.Accounts.Service.Accounts.Command;
using Core.Accounts.Service.Accounts.Query.Request;
using Core.Accounts.Service.Accounts.Query.Response;
using Core.Accounts.Service.Interface;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core.Accounts.API.Controllers.V1
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
#pragma warning disable CS1591
    public class AccountsController : ControllerBase
    {

        private readonly IAccountsService _service;
        private readonly IHashids _hashids;

        public AccountsController(IAccountsService service,
                               IHashids hashids)
        {
            _service = service;
            _hashids = hashids;
        }

        /// <summary>
        /// Return an Account by ID
        /// </summary>
        [Route("{id}")]
        [HttpGet]
        public async Task<Response<AccountsResponse>> Get(string id)
        {
            var account = await _service.Get(_hashids.DecodeSingle(id));

            return new Response<AccountsResponse>(account);
        }

        /// <summary>
        /// Search all Accounts by search query
        /// </summary> 
        [HttpGet]
        public async Task<Response<SearchResponse<AccountsBaseResponse>>> Search([FromQuery] AccountsQuery searchQuery)
        {
            var accounts = await _service.Search(searchQuery);

            return new Response<SearchResponse<AccountsBaseResponse>>(accounts);
        }

        /// <summary>
        /// Update an Account
        /// </summary>s
        [HttpPut("{id}")]
        [Authorize(Roles = "super-admin, admin, client")]
        public async Task<Response<AccountsResponse>> Update(string id, [FromBody] UpdateAccountCommand cmd)
        {
            var account = await _service.Update(_hashids.DecodeSingle(id), cmd);

            return await Get(_hashids.Encode(account.Id));
        }

        /// <summary>
        /// Delete an Account
        /// </summary>s
        [HttpDelete("{id}")]
        [Authorize(Roles = "super-admin, admin")]
        public async Task<Response<bool>> Delete(string id)
        {
            var result = await _service.Delete(_hashids.DecodeSingle(id));

            return new Response<bool>(result);
        }

        /// <summary>
        /// SignUp an Account
        /// </summary>s
        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<Response<AccountsResponse>> SignUp([FromBody] RegisterAccountCommand cmd)
        {
            var account = await _service.Create(cmd);

            return await Get(_hashids.Encode(account.Id));
        }

        /// <summary>
        /// Email Verification
        /// </summary>
        [HttpPost("email-verification")]
        [AllowAnonymous]
        public async Task<Response<bool>> EmailVerification([FromBody] EmailVerificationCommand cmd)
        {
            var result = await _service.EmailVerification(cmd);

            return new Response<bool>(result);
        }

        /// <summary>
        /// Resend Email Verification
        /// </summary>s
        [HttpPost("resend-email-verification/{email}")]
        [AllowAnonymous]
        public async Task<Response<bool>> ResendEmailVerification(string email)
        {
            var token = await _service.ResendEmailVerification(email);

            return new Response<bool>(!string.IsNullOrEmpty(token));
        }

        /// <summary>
        /// Reset Password - starts/restarts reset password process
        /// </summary>
        [HttpPost("reset-password/{email}")]
        [AllowAnonymous]
        public async Task<Response<bool>> ResetPassword(string email)
        {
            var token = await _service.ResetPassword(email);

            return new Response<bool>(!string.IsNullOrEmpty(token));
        }

        /// <summary>
        /// Change Password - update Account password
        /// </summary>s
        [HttpPost("change-password")]
        [AllowAnonymous]
        public async Task<Response<bool>> ChangePassword([FromBody] ChangePasswordCommand cmd)
        {
            var result = await _service.ChangePassword(cmd);

            return new Response<bool>(result);
        }

        /// <summary>
        /// Quick Validatate if an Account with {field} having {value} exists
        /// </summary>s
        [HttpPost("quick-validation")]
        [AllowAnonymous]
        public async Task<Response<bool>> QuickValidation([FromQuery] string field, [FromQuery] string value)
        {
            var result = await _service.QuickValidation(field, value);

            return new Response<bool>(result);
        }

        /// <summary>
        /// Assign account roles
        /// </summary>
        [HttpPost("assign-roles")]
        [Authorize(Roles = "M2M_role, super-admin, admin")]
        public async Task<Response<bool>> AssignAccountRoles([FromBody] CreateAccountRolesCommand createAccountRolesCommand)
        {
            var result = await _service.CreateAccountRoles(createAccountRolesCommand);

            return new Response<bool>(result == null ? false : true);
        }

        /// <summary>
        /// Assign initial account-tenant rolles
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        /// 
        [HttpPost("assign-initial-roles")]
        [Authorize(Roles = "M2M_role")]
        public async Task<Response<bool>> AssignInitialAccountTenantRoles([FromBody] AssignAccountTenantRolesCommand cmd)
        {
            var result = await _service.AssignAccountTenantRoles(cmd);

            return new Response<bool>(result);
        }
    }
}
