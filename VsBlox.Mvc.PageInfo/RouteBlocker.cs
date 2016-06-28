using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VsBlox.Mvc.PageInfo.Attributes;

namespace VsBlox.Mvc.PageInfo
{
  /// <summary>
  /// Class to initialize the rout blocker.
  /// </summary>
  /// <example>
  /// Usage it like this:
  /// <code>
  /// public static void RegisterRoutes(RouteCollection routes)
  /// {
  ///     routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
  ///
  ///     routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
  ///
  ///     RouteBlocker.RegisterRoutes(routes);
  /// }
  /// </code>
  /// Decorate your actions:
  /// <code>
  /// [AllowRouteIfEqualsConfiguration("MyKey", "MyValue")]
  /// public ActionResult MyAction()
  /// {
  ///     ...
  /// }
  /// </code>
  /// In web.config:
  /// <code>
  /// <appSettings>
  ///     ...
  ///     <add key="MyKey" value="MyValue" />
  ///     ...
  /// </appSettings>     
  /// </code>
  /// </example>
  public static class RouteBlocker
  {
    /// <summary>
    /// Registers the specified routes.
    /// </summary>
    /// <param name="routes">The routes.</param>
    /// <param name="action">Error action.</param>
    /// <param name="controller">Error controller.</param>
    /// <param name="urlFilterFunc">The URL filter function.</param>
    /// <param name="ignorePatterns">The ignore patterns.</param>
    public static void RegisterRoutes(RouteCollection routes, string action, string controller, Func<string, bool> urlFilterFunc, IEnumerable<string> ignorePatterns = null)
    {
      if (routes == null) return;

      foreach (var route in routes.Cast<Route>()
        .Where(route => urlFilterFunc != null && urlFilterFunc(route.Url))
        .Where(route => !string.IsNullOrEmpty(route.Url)))
      {
        // ReSharper disable once PossibleMultipleEnumeration
        route.RouteHandler = new RouteProviderHandler(route.RouteHandler, action, controller, ignorePatterns);
      }
    }
  }

  /// <summary>
  /// Factory class. Used to instantiate our custom route handler.
  /// </summary>
  public class RouteProviderHandler : IRouteHandler
  {
    private IRouteHandler BaseRouteHandler { get; set; }
    private string Action { get; set; }
    private string Controller { get; set; }
    private string IgnorePattern { get; set; }
    private Regex PatternMacher { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RouteProviderHandler" /> class.
    /// </summary>
    /// <param name="baseRouteHandler">The base route handler.</param>
    /// <param name="action">Error action.</param>
    /// <param name="controller">Error controller.</param>
    /// <param name="ignorePatterns">The ignore patterns.</param>
    public RouteProviderHandler(IRouteHandler baseRouteHandler, string action, string controller, IEnumerable<string> ignorePatterns = null)
    {
      BaseRouteHandler = baseRouteHandler;
      Action = action;
      Controller = controller;

      if (ignorePatterns == null) return;

      IgnorePattern = string.Join("|", ignorePatterns);
      PatternMacher = new Regex($"^({IgnorePattern})");
    }

    /// <summary>
    /// Provides the object that processes the request.
    /// </summary>
    /// <param name="requestContext">An object that encapsulates information about the request.</param>
    /// <returns>
    /// An object that processes the request.
    /// </returns>
    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      if (requestContext.HttpContext.Request.Url != null && PatternMacher != null && PatternMacher.IsMatch(requestContext.HttpContext.Request.Url.LocalPath))
      {
        return BaseRouteHandler.GetHttpHandler(requestContext);
      }

      var execute = true;

      var controllerName = requestContext.RouteData.GetRequiredString("controller");
      var actionName = requestContext.RouteData.GetRequiredString("action");

      IController controller = null;
      IControllerFactory factory = null;

      try
      {
        factory = ControllerBuilder.Current.GetControllerFactory();
        controller = factory.CreateController(requestContext, controllerName);

        if (controller != null)
        {
          var controllerDescriptor = new ReflectedControllerDescriptor(controller.GetType());
          var controllerContext = new ControllerContext(requestContext, (ControllerBase)controller);
          var actionDescriptor = controllerDescriptor.FindAction(controllerContext, actionName);

          var reflectedActionDescriptor = actionDescriptor as ReflectedActionDescriptor;
          if (reflectedActionDescriptor != null)
          {
            var m = reflectedActionDescriptor.MethodInfo;
            if (m.IsPublic && m.ReturnParameter != null)
            {
              var methodOk =
                  typeof(ActionResult).IsAssignableFrom(m.ReturnParameter.ParameterType) ||
                  typeof(Task<ActionResult>).IsAssignableFrom(m.ReturnParameter.ParameterType);

              if (methodOk)
              {
                var allowRouteIfEqualsConfigurationAttributes = m.GetCustomAttributes(typeof(AllowRouteIfEqualConfigAttribute), true).Cast<AllowRouteIfEqualConfigAttribute>();
                var allowRouteIfEqualsConfigurationAttributesArray = allowRouteIfEqualsConfigurationAttributes as AllowRouteIfEqualConfigAttribute[] ?? allowRouteIfEqualsConfigurationAttributes.ToArray();

                execute = allowRouteIfEqualsConfigurationAttributesArray.Length <= 0 || allowRouteIfEqualsConfigurationAttributesArray.Any(allowRouteIfEqualsConfigurationAttribute => allowRouteIfEqualsConfigurationAttribute.IsAllowed());
              }
            }
          }
        }
      }
      // ReSharper disable RedundantCatchClause
      catch (Exception)
      {
        throw;
      }
      // ReSharper restore RedundantCatchClause
      finally
      {
        factory?.ReleaseController(controller);
      }

      if (!execute)
      {
        requestContext.RouteData.Values["controller"] = Controller ?? "Error";
        requestContext.RouteData.Values["action"] = Action ?? "Index";
      }

      //return BaseRouteHandler.GetHttpHandler(requestContext);
      var httpHandler = BaseRouteHandler.GetHttpHandler(requestContext) as MvcHandler;
      return httpHandler;
    }
  }
}
