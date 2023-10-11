using Common.Model.Enitites;

namespace Core.Accounts.DAL.Entity
{
    public class AccountRole : BaseEntity
    {
        public int AccountId { get; set; }
        public int RoleId { get; set; }
        public Account Account { get; set; }
        public Role Role { get; set; }
    }
}
