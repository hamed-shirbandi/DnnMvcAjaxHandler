using DotNetNuke.Web.Mvc.Routing;
using System;

namespace Dnn.Modules.DnnMvcAjaxHandler
{


 
    public class RouteConfig : IMvcRouteMapper
    {

        public void RegisterRoutes(DotNetNuke.Web.Mvc.Routing.IMapRoute mapRouteManager)
        {
            mapRouteManager.MapRoute("DnnMvcAjaxHandlerExample", "DnnMvcAjaxHandlerExample", "{controller}/{action}", new[]
            {"Dnn.Modules.DnnMvcAjaxHandlerExample.Controllers"});
        }
    }
    
}
