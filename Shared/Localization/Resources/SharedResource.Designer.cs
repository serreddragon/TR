﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Localization.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class SharedResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SharedResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Localization.Resources.SharedResource", typeof(SharedResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email Not Verified.
        /// </summary>
        public static string AccountVeificationStatus_EmailNotVerified {
            get {
                return ResourceManager.GetString("AccountVeificationStatus_EmailNotVerified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password Reset Requested.
        /// </summary>
        public static string AccountVeificationStatus_PasswordResetRequested {
            get {
                return ResourceManager.GetString("AccountVeificationStatus_PasswordResetRequested", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Verified.
        /// </summary>
        public static string AccountVeificationStatus_Verified {
            get {
                return ResourceManager.GetString("AccountVeificationStatus_Verified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Insufficient parameters provided.
        /// </summary>
        public static string BadParams {
            get {
                return ResourceManager.GetString("BadParams", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to It is not possible to assign Super Admin role!.
        /// </summary>
        public static string CannotAssignSuperAdminRole {
            get {
                return ResourceManager.GetString("CannotAssignSuperAdminRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to It is not possible to delete Super Admin Account!.
        /// </summary>
        public static string CannotDeleteSuperAdminAccount {
            get {
                return ResourceManager.GetString("CannotDeleteSuperAdminAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to It is not possible to validate email/phone without setting valid password!.
        /// </summary>
        public static string CannotValidateAccountWithoutPassword {
            get {
                return ResourceManager.GetString("CannotValidateAccountWithoutPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Verification Token Expired.
        /// </summary>
        public static string InvalidEmailVerificationAttempt_TokenExpired {
            get {
                return ResourceManager.GetString("InvalidEmailVerificationAttempt_TokenExpired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid atttempt. Email address is already in use..
        /// </summary>
        public static string InvalidRegisterAttempt_EmailAlreadyUsed {
            get {
                return ResourceManager.GetString("InvalidRegisterAttempt_EmailAlreadyUsed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid atttempt. Phone number is already in use..
        /// </summary>
        public static string InvalidRegisterAttempt_PhoneNumberAlreadyUsed {
            get {
                return ResourceManager.GetString("InvalidRegisterAttempt_PhoneNumberAlreadyUsed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid atttempt. More than one role provided..
        /// </summary>
        public static string InvalidRegisterAttempt_SingleRole {
            get {
                return ResourceManager.GetString("InvalidRegisterAttempt_SingleRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Registration failed..
        /// </summary>
        public static string InvalidRegisterAttempt_UnknownError {
            get {
                return ResourceManager.GetString("InvalidRegisterAttempt_UnknownError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password needs to have at least 6 characters, needs to have a number, a capital letter and a special character..
        /// </summary>
        public static string PasswordRules {
            get {
                return ResourceManager.GetString("PasswordRules", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error has occured..
        /// </summary>
        public static string UnknownError {
            get {
                return ResourceManager.GetString("UnknownError", resourceCulture);
            }
        }
    }
}
