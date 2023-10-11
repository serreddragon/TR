using Common.Model.Enitites;
using System.Collections.Generic;

namespace Core.Accounts.DAL.Entity
{
    public class Role : BaseEntity
    {
        public int TenatId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<AccountRole> AccountRoles { get; set; }

        public List<Account> Accounts { get; set; }

    }
}
