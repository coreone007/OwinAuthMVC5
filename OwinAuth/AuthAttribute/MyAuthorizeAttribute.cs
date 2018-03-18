using OwinAuth.Models;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace OwinAuth.AuthAttribute
{
    public class MyAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        private string _roles;
        private string[] _rolesSplit = new string[0];
        public string Roles
        {
            get { return _roles ?? String.Empty; }
            set
            {
                _roles = value;
                _rolesSplit = SplitString(value);
            }
        }
        public MyAuthorizeAttribute(string roles)
        {
            this.Roles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            IPrincipal user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }

            if (_rolesSplit.Length > 0 && !_rolesSplit.Any(user.IsInRole))
            {
                return false;
            }

            return true;
            //return base.AuthorizeCore(httpContext);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.HttpContext.Response.StatusCode = 403;
            }
        }
        internal static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }
    }
}