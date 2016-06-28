using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VsBlox.Mvc.PageInfo.Helpers
{
  /// <summary>
  /// Helper class to create a controller context instance
  /// </summary>
  public class ControllerContextFactory
  {
    /// <summary>
    /// Creates an instance of an MVC controller from scratch 
    /// when no existing ControllerContext is present       
    /// </summary>
    /// <typeparam name="T">Type of the controller to create</typeparam>
    /// <returns>Controller Context for T</returns>
    /// <exception cref="InvalidOperationException">thrown if HttpContext not available</exception>
    public static T CreateController<T>(RouteData routeData = null) where T : Controller, new()
    {
      // create a disconnected controller instance
      var controller = new T();

      // get context wrapper from HttpContext if available
      HttpContextBase wrapper;
      if (HttpContext.Current != null)
      {
        wrapper = new HttpContextWrapper(HttpContext.Current);
      }
      else
      {
        HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.org", ""), new HttpResponse(new StringWriter()));
        wrapper = new HttpContextWrapper(HttpContext.Current);
      }

      if (routeData == null) routeData = new RouteData();

      // add the controller routing if not existing
      if (!routeData.Values.ContainsKey("controller") && !routeData.Values.ContainsKey("Controller"))
      {
        routeData.Values.Add("controller", controller.GetType().Name.ToLower().Replace("controller", ""));
      }

      controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);

      return controller;
    }
  }
}


