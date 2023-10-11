using Common.Model;

namespace Common.Interface
{
    public interface IDatabaseContext
    {
        string Token { get; set; }

        AuthenticatedAccount CurrentAccount { get; set; }


    }
}
