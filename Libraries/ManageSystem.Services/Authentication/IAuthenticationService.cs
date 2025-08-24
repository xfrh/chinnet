
using ManageSystem.Core;
using ManageSystem.Core.Domain.Sate;
using ManageSystem.Core.Domain.Users;

namespace ManageSystem.Services.Authentication
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public partial interface IAuthenticationService 
    {
        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="createPersistentCookie">A value indicating whether to create a persistent cookie</param>
        void SignIn(Account userinfo, bool createPersistentCookie);

        void SatelliteSignIn(SatelliteUser userinfo, bool createPersistentCookie);

        /// <summary>
        /// Sign out
        /// </summary>
        void SignOut();

        /// <summary>
        /// Get authenticated user
        /// </summary>
        /// <returns>Userinfo</returns>
        Account GetAuthenticatedUser();

        SatelliteUser GetAuthenticatedSatelliteUser();
    }
}