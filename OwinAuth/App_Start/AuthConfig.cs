using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartupAttribute(typeof(OwinAuth.AuthConfig))]
namespace OwinAuth
{
    public class AuthConfig
    {
        public void Configuration(IAppBuilder app)
        {
            var cookieAuthOptions = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieHttpOnly = true,
                ExpireTimeSpan = TimeSpan.FromSeconds(120),
                SlidingExpiration = true,
                CookieSecure = CookieSecureOption.SameAsRequest,
                LoginPath = new PathString("/Account/Login")
            };

            app.UseCookieAuthentication(cookieAuthOptions);
        }
    }
}