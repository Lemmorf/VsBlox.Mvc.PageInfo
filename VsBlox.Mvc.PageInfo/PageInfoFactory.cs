using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using VsBlox.Mvc.PageInfo.Attributes;
using VsBlox.Mvc.PageInfo.Extensions;
using VsBlox.Mvc.PageInfo.Helpers;
using VsBlox.Mvc.PageInfo.SiteMap;

namespace VsBlox.Mvc.PageInfo
{
  /// <summary>
  /// Class to build and retrieve CMS like page information.
  /// </summary>
  public static class PageInfoFactory
  {
    static Dictionary<string, PageInfoCollection> PageInfoCollections { get; set; }
    static readonly Regex PlaceHolderRegex = new Regex("{.*?}/");

    /// <summary>
    /// Resets this instance.
    /// </summary>
    public static void Reset()
    {
      Init();
    }

    /// <summary>
    /// Finds the related pages for the specified page.
    /// </summary>
    /// <param name="pageInfo">The page information.</param>
    /// <param name="extraRelatedInfos">The extra related infos.</param>
    /// <returns></returns>
    public static IEnumerable<RelatedInfo> FindRelatedPages(PageInfo pageInfo, IEnumerable<RelatedInfo> extraRelatedInfos = null)
    {
      Init();

      var pageInfoCollection = FindCurrentPageInfoCollection();

      var relatedInfos = new List<RelatedInfo>();

      foreach (var tag in pageInfo.Tags.Split(','))
      {
        foreach (var info in pageInfoCollection.PageInfos.Where(info => info.IsValid))
        {
          if (info == pageInfo)
          {
            if (info.RelatedInfos.Any()) relatedInfos.AddRange(info.RelatedInfos);
            continue;
          }

          var tags = info.Tags.Split(',');
          if (tags.Contains(tag))
          {

            relatedInfos.Add(new RelatedInfo(info.Title, info.Action, info.Controller, info.Area));
          }
        }
      }

      if (extraRelatedInfos != null) relatedInfos.AddRange(extraRelatedInfos);

      return relatedInfos.DistinctBy(i => i.Title).OrderBy(pi => pi.Title);
    }

    /// <summary>
    /// Finds a page by key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static PageInfo FindByKey(string key)
    {
      Init();

      var pageInfoCollection = FindCurrentPageInfoCollection();

      return PageInfo.FindByKey(pageInfoCollection.PageInfos, key);
    }

    /// <summary>
    /// Finds the page by URL.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    public static PageInfo FindByUrl(string url)
    {
      Init();

      var pageInfoCollection = FindCurrentPageInfoCollection();

      if (!string.IsNullOrEmpty(url)) url = url.Trim('/') + '/';

      return PageInfo.FindByUrl(pageInfoCollection.PageInfos, url);
    }

    /// <summary>
    /// Finds a page by action/controller.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="controller">The controller.</param>
    /// <returns></returns>
    public static PageInfo FindByActionController(string action, string controller)
    {
      Init();

      var pageInfoCollection = FindCurrentPageInfoCollection();

      return PageInfo.FindByActionController(pageInfoCollection.PageInfos, action, controller);
    }

    /// <summary>
    /// Registers the routes.
    /// </summary>
    /// <param name="routes">The routes.</param>
    public static void RegisterRoutes(RouteCollection routes)
    {
      var routeInfos = CreateRouteInfos();

      foreach (var routeInfo in routeInfos)
      {
        MapRoute(routes, routeInfo);
      }
    }

    /// <summary>
    /// Generates a sitemap.
    /// </summary>
    /// <param name="urlHelper">The URL helper.</param>
    /// <param name="debug">if set to <c>true</c> [debug].</param>
    /// <returns></returns>
    public static IEnumerable<SiteMapItem> GenerateSiteMapItems(UrlHelper urlHelper, bool debug = false)
    {
      return CreateSiteMapItems(urlHelper, debug);
    }

    /// <summary>
    /// Build breadcrumbs for the specified action/contoller.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="controller">The controller.</param>
    /// <returns></returns>
    public static IEnumerable<PageInfo> Breadcrumbs(string action, string controller)
    {
      Init();

      var pageInfoCollection = FindCurrentPageInfoCollection();

      var item = PageInfo.FindByActionController(pageInfoCollection.PageInfosTree, action, controller) ?? new PageInfo();
      return item.Breadcrumbs();
    }

    /// <summary>
    /// Build page tree.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<PageInfo> TreeInfo()
    {
      Init();

      var pageInfoCollection = FindCurrentPageInfoCollection();

      return pageInfoCollection.PageInfosTree;
    }

    /// <summary>
    /// Build meta data for the specified action/controller.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="controller">The controller.</param>
    /// <returns></returns>
    public static MetaDataInfo MetaData(string action, string controller)
    {
      Init();

      var pageInfoCollection = FindCurrentPageInfoCollection();

      var item = PageInfo.FindByActionController(pageInfoCollection.PageInfosTree, action, controller) ?? new PageInfo();

      var title = string.IsNullOrEmpty(item.MetaTitle) ? item.Title : item.MetaTitle;
      var description = string.IsNullOrEmpty(item.MetaDescription) ? item.Title : item.MetaDescription;
      var noIndex = item.MetaNoIndex;
      var noFollow = item.MetaNoFollow;

      return new MetaDataInfo
      {
        Title = title ?? "",
        Description = description ?? "",
        NoIndex = noIndex,
        NoFollow = noFollow
      };
    }

    /// <summary>
    /// Build top level menu for the site.
    /// </summary>
    /// <param name="isInRole">The is in role.</param>
    /// <returns></returns>
    public static IEnumerable<PageInfo> Menu(Func<string, bool> isInRole = null)
    {
      var menuInfo = CreateMenuInfo();
      return menuInfo == null ? new List<PageInfo>() : menuInfo.Menu(isInRole);
    }

    /// <summary>
    /// Returns the home page.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<PageInfo> HomePage()
    {
      var homePageInfo = CreateHomePageInfo();
      return homePageInfo == null ? new List<PageInfo>() : homePageInfo.Children;
    }

    /// <summary>
    /// Permanents the redirect urls.
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, string> PermanentRedirectUrls()
    {
      Init();

      var pageInfoCollection = FindCurrentPageInfoCollection();

      if (pageInfoCollection.PermanentRedirectUrl == null)
      {
        Init();

        pageInfoCollection.PermanentRedirectUrl = new Dictionary<string, string>();

        foreach (var info in pageInfoCollection.PageInfos)
        {
          if (!info.IsValid) continue;

          foreach (var redirectUrl in info.RedirectUrls)
          {
            if (pageInfoCollection.PermanentRedirectUrl.ContainsKey(redirectUrl)) continue;
            pageInfoCollection.PermanentRedirectUrl.Add(redirectUrl, info.PageUrl);
          }
        }
      }

      return pageInfoCollection.PermanentRedirectUrl;
    }

    #region Helper functions

    /// <summary>
    /// Initializes/recreates the page info structure.
    /// </summary>
    static void Init()
    {
      if (PageInfoCollections == null) PageInfoCollections = new Dictionary<string, PageInfoCollection>();

      var culture = Thread.CurrentThread.CurrentCulture.Name;
      if (PageInfoCollections.ContainsKey(culture)) return;

      var pageInfoCollection = new PageInfoCollection();
      PageInfoCollections.Add(culture, pageInfoCollection);

      if (pageInfoCollection.PageInfos == null) pageInfoCollection.PageInfos = GetPageInfos();
      if (pageInfoCollection.PageInfosTree != null) return;

      var pageInfosTree = new PageInfo();

      foreach (var item in pageInfoCollection.PageInfos)
      {
        PageInfo.Add(pageInfosTree, item.ParentKey, item);

        var pageItem = item;
        var results = pageInfosTree.Children.Where(pi => !string.IsNullOrEmpty(pi.ParentKey) && pi.ParentKey == pageItem.Key).Select(pi => pi).ToArray();
        foreach (var result in results)
        {
          if (result.Parent == item) continue;

          result.Parent.Remove(result);
          item.Add(result);
        }
      }

      pageInfoCollection.PageInfosTree = pageInfosTree.Children;
    }

    /// <summary>
    /// Finds the current page information collection.
    /// </summary>
    /// <returns></returns>
    static PageInfoCollection FindCurrentPageInfoCollection()
    {
      var culture = Thread.CurrentThread.CurrentCulture.Name;
      return PageInfoCollections.ContainsKey(culture) ? PageInfoCollections[culture] : new PageInfoCollection();
    }

    /// <summary>
    /// Creates the site map items.
    /// </summary>
    /// <param name="urlHelper">The URL.</param>
    /// <param name="debug">if set to <c>true</c> shows all urls (visible, non visible and no index)</param>
    /// <returns></returns>
    private static IEnumerable<SiteMapItem> CreateSiteMapItems(UrlHelper urlHelper, bool debug = false)
    {
      Init();

      var pageInfoCollection = FindCurrentPageInfoCollection();
      if (pageInfoCollection.SiteMapItems != null) return pageInfoCollection.SiteMapItems;

      List<SiteMapItem> siteMapItems;

      if (debug)
      {
        siteMapItems = (from pageInfo in pageInfoCollection.PageInfos
                        select new SiteMapItem(urlHelper.AbsoluteUrlFromRelavtiveUrl(pageInfo.PageUrl), changeFrequency: SitemapChangeFrequency.Always, priority: 1.0)).ToList();
      }
      else
      {
        siteMapItems = (from pageInfo in pageInfoCollection.PageInfos
                        where !pageInfo.MetaNoIndex
                        where pageInfo.Visible
                        select new SiteMapItem(urlHelper.AbsoluteUrlFromRelavtiveUrl(pageInfo.PageUrl), changeFrequency: SitemapChangeFrequency.Always, priority: 1.0)).ToList();
      }

      pageInfoCollection.SiteMapItems = siteMapItems;

      return pageInfoCollection.SiteMapItems;
    }

    /// <summary>
    /// Creates the routes.
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<PageInfo> CreateRouteInfos()
    {
      Init();

      var pageInfoCollection = FindCurrentPageInfoCollection();

      if (pageInfoCollection.RouteInfos != null) return pageInfoCollection.RouteInfos;

      pageInfoCollection.RouteInfos = pageInfoCollection.PageInfosTree.Where(pi => !string.IsNullOrEmpty(pi.Key)).Select(pi => pi);

      return pageInfoCollection.RouteInfos;
    }

    /// <summary>
    /// Maps the route.
    /// </summary>
    /// <param name="routes">The routes.</param>
    /// <param name="pageInfo">The page information.</param>
    private static void MapRoute(RouteCollection routes, PageInfo pageInfo)
    {
      var sb = new StringBuilder();

      var pageUrls = new List<string>();

      var pageInfoTemp = pageInfo;
      while (pageInfoTemp != null)
      {
        if (pageInfoTemp.IsValid)
        {
          var pageUrl = pageInfoTemp.PageUrl;
          if (pageUrls.Count >= 1)
          {
            pageUrl = PlaceHolderRegex.Replace(pageUrl, string.Empty);
          }
          pageUrls.Add(pageUrl);
        }
        pageInfoTemp = pageInfoTemp.Parent;
      }

      pageUrls.Reverse();

      foreach (var url in pageUrls)
      {
        sb.Append(url);
      }

      var pageRoute = sb.ToString();
      if (!pageRoute.EndsWith("/")) pageRoute += "/";

      routes.MapRoute(pageInfo.Key, pageRoute, new { controller = pageInfo.Controller, action = pageInfo.Action }, new[] { pageInfo.ControllerType.Namespace });
      //todo: area routes
      //var route = routes.MapRoute(pageInfo.Key, pageRoute, new { controller = pageInfo.Controller, action = pageInfo.Action }, new[] { pageInfo.ControllerType.Namespace });
      //if (route != null)
      //{
      //  if (!string.IsNullOrEmpty(pageInfo.Area))
      //  {
      //    if (route.DataTokens.ContainsKey("area")) route.DataTokens["area"] = pageInfo.Area;
      //    else route.DataTokens.Add("area", pageInfo.Area);

      //    var url = route.Url;
      //    if (!string.IsNullOrEmpty(url) && !url.StartsWith(pageInfo.Area))
      //    {
      //      route.Url = pageInfo.Area + "/" + url;
      //    }
      //  }
      //}

      foreach (var child in pageInfo.Children) MapRoute(routes, child);
    }

    /// <summary>
    /// Creates the menu information.
    /// </summary>
    /// <returns></returns>
    private static PageInfo CreateMenuInfo()
    {
      Init();

      var pageInfoCollection = FindCurrentPageInfoCollection();

      if (pageInfoCollection.MenuInfo != null) return pageInfoCollection.MenuInfo;

      pageInfoCollection.MenuInfo = new PageInfo
      {
        Children = pageInfoCollection.PageInfosTree.Where(pi => pi.IsMenuItem).Select(pi => pi).ToList()
      };

      return pageInfoCollection.MenuInfo;
    }

    /// <summary>
    /// Creates the home page information.
    /// </summary>
    /// <returns></returns>
    private static PageInfo CreateHomePageInfo()
    {
      Init();

      var pageInfoCollection = FindCurrentPageInfoCollection();

      if (pageInfoCollection.HomePageInfo != null) return pageInfoCollection.HomePageInfo;

      if (pageInfoCollection.PageInfos == null) return pageInfoCollection.HomePageInfo;

      pageInfoCollection.HomePageInfo = new PageInfo
      {
        Children = pageInfoCollection.PageInfos.Where(pi => pi.Visible && pi.IsHomePageItem).Select(pi => pi).ToList()
      };

      return pageInfoCollection.HomePageInfo;
    }

    /// <summary>
    /// Gets the site map item information.
    /// </summary>
    /// <returns></returns>
    // ReSharper disable once FunctionComplexityOverflow
    private static IEnumerable<PageInfo> GetPageInfos()
    {
      var items = new List<PageInfo>();

      try
      {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.GlobalAssemblyCache))
        {
          Type[] assemblyTypes;

          try { assemblyTypes = assembly.GetTypes(); }
          catch (Exception) { continue; }

          foreach (var controllerType in assemblyTypes)
          {
            if (!controllerType.IsSubclassOf(typeof(Controller))) continue;

            var authorizeAttributeController = controllerType.GetCustomAttributes(typeof(AuthorizeAttribute), true).Cast<AuthorizeAttribute>().FirstOrDefault();
            bool? hasControllerAuthorizeAttribute = null; if (authorizeAttributeController != null) hasControllerAuthorizeAttribute = true;

            var allowAnonymousAttributeController = controllerType.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Cast<AllowAnonymousAttribute>().FirstOrDefault();
            bool? hasControllerAllowAnonymousAttribute = null; if (allowAnonymousAttributeController != null) hasControllerAllowAnonymousAttribute = true;

            var mi = controllerType.GetMethods();

            foreach (var m in mi)
            {
              if (!m.IsPublic || m.ReturnParameter == null) continue;

              var methodOk =
                  typeof(ActionResult).IsAssignableFrom(m.ReturnParameter.ParameterType) ||
                  typeof(Task<ActionResult>).IsAssignableFrom(m.ReturnParameter.ParameterType);

              if (!methodOk) continue;

              var httpGetAttribute = m.GetCustomAttributes(typeof(HttpGetAttribute), true).Cast<HttpGetAttribute>().FirstOrDefault();
              if (httpGetAttribute == null)
              {
                var httpPostAttribute = m.GetCustomAttributes(typeof(HttpPostAttribute), true).Cast<HttpPostAttribute>().FirstOrDefault();
                if (httpPostAttribute != null) continue; // no HttpPost actions
              }

              var authorizeAttributeAction = m.GetCustomAttributes(typeof(AuthorizeAttribute), true).Cast<AuthorizeAttribute>().FirstOrDefault();
              var authorizationRoles = authorizeAttributeAction != null ? authorizeAttributeAction.Roles : "";
              bool? hasActionAuthorizeAttribute = null; if (authorizeAttributeAction != null) hasActionAuthorizeAttribute = true;

              var allowAnonymousAttributeAction = m.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Cast<AllowAnonymousAttribute>().FirstOrDefault();
              bool? hasActionAllowAnonymousAttribute = null; if (allowAnonymousAttributeAction != null) hasActionAllowAnonymousAttribute = true;

              var area = FindAreaForControllerType(controllerType);

              var ignoreByPageInfoFactoryAttribute = m.GetCustomAttributes(typeof(IgnorePageInfoAttribute), true).Cast<IgnorePageInfoAttribute>().FirstOrDefault();
              if (ignoreByPageInfoFactoryAttribute != null) continue;

              var allowRouteIfEqualsConfigurationAttributes = m.GetCustomAttributes(typeof(AllowRouteIfEqualConfigAttribute), true).Cast<AllowRouteIfEqualConfigAttribute>();
              var allowRouteIfEqualsConfigurationAttributesArray = allowRouteIfEqualsConfigurationAttributes as AllowRouteIfEqualConfigAttribute[] ?? allowRouteIfEqualsConfigurationAttributes.ToArray();

              var include =
                  allowRouteIfEqualsConfigurationAttributesArray.Length <= 0 ||
                  allowRouteIfEqualsConfigurationAttributesArray.Any(allowRouteIfEqualsConfigurationAttribute => allowRouteIfEqualsConfigurationAttribute.IsAllowed());

              if (!include) continue;

              var pageInfoAttribute = m.GetCustomAttributes(typeof(PageInfoAttribute), true).Cast<PageInfoAttribute>().FirstOrDefault();
              var metaInfoAttribute = m.GetCustomAttributes(typeof(MetaInfoAttribute), true).Cast<MetaInfoAttribute>().FirstOrDefault();
              var menuInfoAttribute = m.GetCustomAttributes(typeof(MenuInfoAttribute), true).Cast<MenuInfoAttribute>().FirstOrDefault();
              var routeInfoAttribute = m.GetCustomAttributes(typeof(RouteInfoAttribute), true).Cast<RouteInfoAttribute>().FirstOrDefault();
              var tagInfoAttribute = m.GetCustomAttributes(typeof(TagInfoAttribute), true).Cast<TagInfoAttribute>().FirstOrDefault();
              var relatedInfoAttributes = m.GetCustomAttributes(typeof(RelatedInfoAttribute), true).Cast<RelatedInfoAttribute>();
              var visibleInfoAttribute = m.GetCustomAttributes(typeof(VisibleInfoAttribute), true).Cast<VisibleInfoAttribute>().FirstOrDefault();
              var propertyInfoAttributes = m.GetCustomAttributes(typeof(CustomPropertyInfoAttribute), true).Cast<CustomPropertyInfoAttribute>();

              if (pageInfoAttribute == null && metaInfoAttribute == null &&
                  menuInfoAttribute == null && routeInfoAttribute == null &&
                  tagInfoAttribute == null && relatedInfoAttributes == null &&
                  visibleInfoAttribute == null && propertyInfoAttributes == null) continue;

              if (pageInfoAttribute == null) continue;

              var controllerDescriptor = new ReflectedControllerDescriptor(controllerType);
              var actionDescriptor = new ReflectedActionDescriptor(m, m.Name, controllerDescriptor);

              var createController = typeof(ControllerContextFactory).GetMethod("CreateController");
              var genericCreateController = createController.MakeGenericMethod(controllerType);
              var controllerContext = ((Controller)genericCreateController.Invoke(null, new object[] { null })).ControllerContext;

              bool? hasGlobalAuthorizeAttribute = null;
              bool? hasGlobalAllowAnonymousAttribute = null;

              foreach (var provider in FilterProviders.Providers)
              {
                var filter = provider.GetFilters(controllerContext, actionDescriptor).FirstOrDefault(f => f.Instance is AuthorizeAttribute);
                if (filter != null) hasGlobalAuthorizeAttribute = true;

                filter = provider.GetFilters(controllerContext, actionDescriptor).FirstOrDefault(f => f.Instance is AllowAnonymousAttribute);
                if (filter != null) hasGlobalAllowAnonymousAttribute = true;

                if (hasGlobalAuthorizeAttribute.HasValue || hasGlobalAllowAnonymousAttribute.HasValue) break;
              }

              if (routeInfoAttribute == null) routeInfoAttribute = new RouteInfoAttribute();
              if (string.IsNullOrEmpty(routeInfoAttribute.Key)) routeInfoAttribute.Key = $"{area}{controllerType.Name.Replace("Controller", "")}{m.Name}";
              if (string.IsNullOrEmpty(routeInfoAttribute.Url)) routeInfoAttribute.Url = $"{m.Name}/";

              if (metaInfoAttribute == null) metaInfoAttribute = new MetaInfoAttribute();
              if (string.IsNullOrEmpty(metaInfoAttribute.Title)) metaInfoAttribute.Title = pageInfoAttribute.Title;
              if (string.IsNullOrEmpty(metaInfoAttribute.Description)) metaInfoAttribute.Description = pageInfoAttribute.Title;
              if (string.IsNullOrEmpty(metaInfoAttribute.Title) && string.IsNullOrEmpty(metaInfoAttribute.TitleResourceName))
              {
                metaInfoAttribute.TitleResourceType = pageInfoAttribute.TitleResourceType;
                metaInfoAttribute.TitleResourceName = pageInfoAttribute.TitleResourceName;
              }
              if (string.IsNullOrEmpty(metaInfoAttribute.Description) && string.IsNullOrEmpty(metaInfoAttribute.DescriptionResourceName))
              {
                metaInfoAttribute.DescriptionResourceType = pageInfoAttribute.TitleResourceType;
                metaInfoAttribute.DescriptionResourceName = pageInfoAttribute.TitleResourceName;
              }

              if (visibleInfoAttribute == null)
              {
                visibleInfoAttribute = new VisibleInfoAttribute();

                int authorizeAttribute = 0;
                int allowAnonymousAttribute = 0;

                if (hasGlobalAuthorizeAttribute.HasValue && hasGlobalAuthorizeAttribute.GetValueOrDefault(false)) authorizeAttribute = 1;
                if (hasControllerAuthorizeAttribute.HasValue && hasControllerAuthorizeAttribute.GetValueOrDefault(false)) authorizeAttribute = 2;
                if (hasActionAuthorizeAttribute.HasValue && hasActionAuthorizeAttribute.GetValueOrDefault(false)) authorizeAttribute = 3;

                if (hasGlobalAllowAnonymousAttribute.HasValue && hasGlobalAllowAnonymousAttribute.GetValueOrDefault(false)) allowAnonymousAttribute = 1;
                if (hasControllerAllowAnonymousAttribute.HasValue && hasControllerAllowAnonymousAttribute.GetValueOrDefault(false)) allowAnonymousAttribute = 2;
                if (hasActionAllowAnonymousAttribute.HasValue && hasActionAllowAnonymousAttribute.GetValueOrDefault(false)) allowAnonymousAttribute = 3;

                visibleInfoAttribute.Visible = authorizeAttribute < allowAnonymousAttribute;
              }

              items.Add(new PageInfo(
                  m.Name,
                  controllerType.Name.Replace("Controller", ""),
                  area,
                  controllerType,
                  pageInfoAttribute,
                  metaInfoAttribute,
                  menuInfoAttribute,
                  routeInfoAttribute,
                  tagInfoAttribute,
                  visibleInfoAttribute,
                  relatedInfoAttributes,
                  propertyInfoAttributes,
                  authorizationRoles));
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new DataMisalignedException("PageInfo attribute failure", ex);
      }

      return items;
    }

    /// <summary>
    /// Finds the type of the area for controller.
    /// </summary>
    /// <param name="controllerType">Type of the controller.</param>
    /// <returns></returns>
    private static string FindAreaForControllerType(Type controllerType)
    {
      var areaTypes = GetAllAreasRegistered();

      foreach (var area in
        from areaType in areaTypes
        where areaType.Namespace != null && controllerType.Namespace != null && controllerType.Namespace.StartsWith(areaType.Namespace)
        select (AreaRegistration)Activator.CreateInstance(areaType))
      {
        return area.AreaName;
      }

      return string.Empty;
    }

    /// <summary>
    /// Gets all areas registered.
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<Type> GetAllAreasRegistered()
    {
      var assembly = GetWebEntryAssembly();
      var areaTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AreaRegistration)));

      return areaTypes;
    }

    /// <summary>
    /// Gets the web entry assembly.
    /// </summary>
    /// <returns></returns>
    private static Assembly GetWebEntryAssembly()
    {
      if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.ApplicationInstance == null)
      {
        return null;
      }

      var type = System.Web.HttpContext.Current.ApplicationInstance.GetType();
      while (type != null && type.Namespace == "ASP")
      {
        type = type.BaseType;
      }

      return type?.Assembly;
    }

    #endregion
  }

  /// <summary>
  /// Language dependend PageInfo collection
  /// </summary>
  class PageInfoCollection
  {
    public IEnumerable<PageInfo> PageInfos { get; set; }
    public IEnumerable<PageInfo> PageInfosTree { get; set; }
    public IEnumerable<PageInfo> RouteInfos { get; set; }
    public PageInfo MenuInfo { get; set; }
    public PageInfo HomePageInfo { get; set; }
    public IEnumerable<SiteMapItem> SiteMapItems { get; set; }
    public Dictionary<string, string> PermanentRedirectUrl { get; set; }
  }
}
