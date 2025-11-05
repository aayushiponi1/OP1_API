using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OP1_API.Models;
using OP1_API.Areas.Login.Models;

namespace OP1_API.ClassCollection
{
    public class jwtValidation : IAuthorizationFilter
    {
        JsonToDataTableService jsnTable= new JsonToDataTableService();
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                GlobalClass cl = new GlobalClass();
                
                
                var data = context.RouteData.Values;
                List<contextRouteDataKeyModel> routeDataList = data
            .Select(kvp => new contextRouteDataKeyModel
            {
                Key = kvp.Key,
                Value = kvp.Value?.ToString()
            })
            .ToList();
                string dataAction = "";string dataController = ""; string action = ""; string controller = "";

                foreach (var item in routeDataList)
                {
                    if (item.Key == "action")
                    {
                        dataAction = item.Value;
                    }
                    else if (item.Key == "controller") {
                        dataController = item.Value;
                    }
                }

                List<contextRouteDataModel> s = new List<contextRouteDataModel>
                {
                    new contextRouteDataModel{ controller="Login", action="login" },
                    new contextRouteDataModel{ controller="Login", action="logout" },
                    new contextRouteDataModel{ controller="Login", action="finYearList" },
                    new contextRouteDataModel{ controller="Login", action="finYearListDecripted" }
                    //new contextRouteDataModel{ controller="Inward", action="InwardRequestSync" },
                    //new contextRouteDataModel{ controller="Inward", action="InwardRequestAck" },
                    //new contextRouteDataModel{ controller="Outward", action="OutwardRequestSync" },
                    //new contextRouteDataModel{ controller="Outward", action="OutwardRequestAck" },
                    //new contextRouteDataModel{ controller="Product", action="NewProduct" },
                    //new contextRouteDataModel{ controller="Product", action="ProductList" },
                };
                bool bl = true;
                foreach (var item in s)
                {
                   action=item.action;
                   controller=item.controller;
                    if (dataAction == action && dataController == controller) 
                    {
                        bl = false;
                    } 
                }

                if (bl == true) {
                    var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                    LoginResultModel l = cl.GetLiveUserDetails(authorizationHeader);
                    if (l.result == "FAILED")
                    {
                        context.Result = new UnauthorizedResult();
                    }
                }                
            }
            catch (Exception ex) {
                context.Result = new UnauthorizedResult();
            }
            // Custom logic to validate the token (e.g., check if it's blacklisted)
        }
    }
}
