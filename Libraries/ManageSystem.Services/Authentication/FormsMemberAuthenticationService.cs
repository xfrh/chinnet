using System;
using System.Web;
using System.Web.Security;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.Users;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Services.SystemSet;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core;
using ManageSystem.Services.Members;
using ManageSystem.Core.Domain.Sate;
using ManageSystem.Services.Satellites;

namespace ManageSystem.Services.Authentication
{
    /// <summary>
    /// 会员帐号服务登录相关 
    /// </summary>
    public partial class FormsMemberAuthenticationService : IAuthenticationService
    {
        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly IMemberService _memberService;
        private readonly ISatelliteUserService _satelliteUserService;
        private readonly TimeSpan _expirationTimeSpan;
        private readonly IFunctionService _functionService;


        private Member _cachedUserinfo
        {
            set
            {
                _httpContext.Session["LoginUserinfoSession"] = value;
            }
            get
            {
                if (_httpContext.Session["LoginUserinfoSession"] == null) return null;

                return _httpContext.Session["LoginUserinfoSession"] as Member;
            }
        }

        private SatelliteUser _cachedSatelliteUserinfo
        {
            set
            {
                _httpContext.Session["LoginUserinfoSession"] = value;
            }
            get
            {
                if (_httpContext.Session["LoginUserinfoSession"] == null) return null;

                return _httpContext.Session["LoginUserinfoSession"] as SatelliteUser;
            }
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="customerSettings">Customer settings</param>
        public FormsMemberAuthenticationService(HttpContextBase httpContext,
            IMemberService customerService,
            ISatelliteUserService satelliteUserService,
            IFunctionService functionService)
        {
            this._httpContext = httpContext;
            this._memberService = customerService;
            this._expirationTimeSpan = FormsAuthentication.Timeout;
            this._satelliteUserService = satelliteUserService;
            this._functionService = functionService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get authenticated customer
        /// </summary>
        /// <param name="ticket">Ticket</param>
        /// <returns>Customer</returns>
        protected virtual Account GetAuthenticatedCustomerFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            var usernameOrEmail = ticket.UserData;

            if (String.IsNullOrWhiteSpace(usernameOrEmail))
                return null;
            var customer = this._memberService.QueryModelByLoginId(usernameOrEmail);

            return customer;
        }

        /// <summary>
        /// Get authenticated customer
        /// </summary>
        /// <param name="ticket">Ticket</param>
        /// <returns>Customer</returns>
        protected virtual SatelliteUser GetAuthenticatedCustomerFromTicketMap(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            var usernameOrEmail = ticket.UserData;

            if (String.IsNullOrWhiteSpace(usernameOrEmail))
                return null;
            var customer = this._satelliteUserService.QueryEntityByUserName(usernameOrEmail);

            return customer;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="createPersistentCookie">A value indicating whether to create a persistent cookie</param>
        public virtual void SignIn(Account customer, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
               customer.LoginId,
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie,
                  customer.LoginId,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            _httpContext.Response.Cookies.Add(cookie);

            Member user = customer as Member;
            _cachedUserinfo = user;
        }


        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="createPersistentCookie">A value indicating whether to create a persistent cookie</param>
        public virtual void SatelliteSignIn(SatelliteUser customer, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
               customer.UserName,
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie,
                  customer.UserName,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            _httpContext.Response.Cookies.Add(cookie);

            SatelliteUser satellituser = customer as SatelliteUser;

            //设置该用户可用的功能集合
            satellituser.FunctionList = this._functionService.QueryByUserId(customer.Id);

            _cachedSatelliteUserinfo = satellituser;
        }
        /// <summary>
        /// Sign out
        /// </summary>
        public virtual void SignOut()
        {
            _cachedUserinfo = null;
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Get authenticated customer
        /// </summary>
        /// <returns>Customer</returns>
        public virtual Account GetAuthenticatedUser()
        {
           if (_cachedUserinfo != null)
              return _cachedUserinfo;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            var customer = GetAuthenticatedCustomerFromTicket(formsIdentity.Ticket);

            _cachedUserinfo = customer as Member;

            return _cachedUserinfo;
        }
        /// <summary>
        /// Get authenticated customer
        /// </summary>
        /// <returns>Customer</returns>
        public virtual SatelliteUser GetAuthenticatedSatelliteUser()
        {
            if (_cachedSatelliteUserinfo != null)
                return _cachedSatelliteUserinfo;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            var customer = GetAuthenticatedCustomerFromTicketMap(formsIdentity.Ticket);

            _cachedSatelliteUserinfo = customer as SatelliteUser;

            return _cachedSatelliteUserinfo;
        }


        #endregion

    }
}