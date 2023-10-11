using Common.Interfaces;
using Core.Accounts.DAL;
using Core.Accounts.DAL.Entity;
using Core.Accounts.Service.Accounts.Command;
using Core.Accounts.Service.Accounts.Query.Request;
using Core.Accounts.Service.Accounts.Query.Response;
using System.Threading.Tasks;

namespace Core.Accounts.Service.Interface
{
    public interface IAccountsService : IBaseService<AccountsDbContext, Account, AccountsResponse, AccountsBaseResponse, AccountsQuery>
    {

        Task<Account> Update(int id, UpdateAccountCommand cmd);

        Task<Account> Create(RegisterAccountCommand cmd);

        Task<bool> QuickValidation(string field, string value);

        Task<bool> EmailVerification(EmailVerificationCommand cmd);

        Task<string> ResendEmailVerification(string email);

        Task<string> ResetPassword(string email);

        Task<bool> ChangePassword(ChangePasswordCommand cmd);

        Task<Account> CreateAccountRoles(CreateAccountRolesCommand cmd);

        Task<bool> AssignAccountTenantRoles(AssignAccountTenantRolesCommand cmd);

    }
}
