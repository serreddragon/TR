using Common.Utilities;
using Core.Accounts.DAL.Constants;
using Core.Accounts.DAL.Entity;
using System;
using System.Collections.Generic;

namespace Core.Accounts.Service.Accounts.Extensions
{
    public static class AccountExtensions
    {
        public static readonly List<char> SpecialCharsToRemoveFromPhoneNumber = new() { '(', ')', '-', '/', ' ' };

        public static Account GenerateVerificationToken(this Account Account)
        {
            Account.VerificationToken = AccountConstants.VerificationTokenPrefix + "_" + Guid.NewGuid().ToString().Replace("-", "");

            Account.VerificationExp = DateTime.Now.AddHours(AccountConstants.VerificationTokenValidityDuration);

            return Account;
        }

        public static Account GenerateResetToken(this Account Account)
        {
            Account.ResetToken = "reset_" + Guid.NewGuid().ToString().Replace("-", "");

            Account.ResetExp = DateTime.Now.AddHours(AccountConstants.ResetTokenValidityDuration);

            return Account;
        }

        public static Account SetPassword(this Account Account, string password)
        {
            if (!string.IsNullOrEmpty(password))
                Account.Password = SecurePasswordHasher.Hash(password);

            return Account;
        }   
    }
}
