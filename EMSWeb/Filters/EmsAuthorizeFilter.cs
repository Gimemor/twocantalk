
using EMSWeb.Controllers;
using EMSWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Security.Claims;

namespace EMSWeb.Filters
{
    public enum ClaimType 
    { 
        UserManagement = 0,
        TalkingTutor = 1,
        TextTutor = 2,
        TwoCanTalk = 3,
        PhraseBook = 4,
        Admin  = 5
    }
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(ClaimType claimType) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] {  claimType };
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        private ClaimType _claimType { get; set; }
        public ClaimRequirementFilter(ClaimType claimType)
        {
            _claimType = claimType;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var loggedInUser = context.HttpContext.Session.GetObjectFromJson<LoggedInUser>("userObject"); ;
            if (loggedInUser == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Index", action = "Index" }));
                return;
            }

            if (this._claimType == ClaimType.UserManagement && loggedInUser.PermAdmin != true)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Index", action = "Index" }));
                return;
            }

            if (this._claimType == ClaimType.TextTutor && loggedInUser.TextTutor != true)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Index", action = "Index" }));
                return;
            }

            if (this._claimType == ClaimType.TalkingTutor && loggedInUser.TalkingTutor != true)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Index", action = "Index" }));
                return;
            }

            if (this._claimType == ClaimType.TwoCanTalk && loggedInUser.TwoCanTalk != true)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Index", action = "Index" }));
                return;
            }

            if (this._claimType == ClaimType.PhraseBook && loggedInUser.PhraseBook != true)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Index", action = "Index" }));
                return;
            }

            if (this._claimType == ClaimType.Admin && loggedInUser.PermAdmin != true)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Index", action = "Index" }));
                return;
            }
        }
    }

}
