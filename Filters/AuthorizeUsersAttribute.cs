﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProyectoJuegos.Models;

namespace ProyectoJuegos.Filters
{
    public class AuthorizeUsersAttribute: AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user?.Identity==null || user.Identity.IsAuthenticated == false)
            {
                context.Result= this.GetRoute("Managed", "Login");
            }
            
        }

        private RedirectToRouteResult GetRoute(string controller, string action)
        {
            RouteValueDictionary route = new RouteValueDictionary(new { controller = controller, action = action });
            RedirectToRouteResult result = new RedirectToRouteResult(route);
            return result;

        }

    }
}
