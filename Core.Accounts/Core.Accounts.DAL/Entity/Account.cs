using Common.Constants;
using Common.Extensions;
using Common.Model.Enitites;
using Core.Accounts.DAL.Constants;
using DelegateDecompiler;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Core.Accounts.DAL.Entity
{
    public class Account : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public AccountVerificationStatus Status { get; set; }

        public string VerificationToken { get; set; }

        public DateTime VerificationExp { get; set; }

        public string ResetToken { get; set; }

        public DateTime ResetExp { get; set; }

        public IEnumerable<AccountRole> AccountRoles { get; set; }

        [NotMapped]
        [Computed]
        public bool IsVerified => Status == AccountVerificationStatus.Verified;

        [NotMapped]
        [Computed]
        public string FullName => FirstName + " " + LastName;

        [NotMapped]
        [Computed]
        public bool IsSuperAdmin => Roles.First().Name == RoleConstants.SuperAdminRole;

        [NotMapped]
        public string StatusDisplay => Status.ToDisplayName();

        [NotMapped]
        [Computed]  
        public IEnumerable<Role> Roles => AccountRoles.Select(ar => ar.Role);

        public bool HasRole(string roleName) => Roles.Any(r => r.Name == roleName);

    }
}
