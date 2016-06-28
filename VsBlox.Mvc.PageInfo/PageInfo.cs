using System;
using System.Collections.Generic;
using System.Linq;
using VsBlox.Mvc.PageInfo.Attributes;

namespace VsBlox.Mvc.PageInfo
{
  /// <summary>
  /// Custom properties.
  /// </summary>
  public class PageInfo
  {
    /// <summary>
    /// Gets or sets the type of the controller.
    /// </summary>
    /// <value>
    /// The type of the controller.
    /// </value>
    public Type ControllerType { get; set; }
    /// <summary>
    /// Gets or sets the controller.
    /// </summary>
    /// <value>
    /// The controller.
    /// </value>
    public string Controller { get; private set; }
    /// <summary>
    /// Gets or sets the action.
    /// </summary>
    /// <value>
    /// The action.
    /// </value>
    public string Action { get; private set; }
    /// <summary>
    /// Gets or sets the area.
    /// </summary>
    /// <value>
    /// The area.
    /// </value>
    public string Area { get; private set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public string Title { get; private set; }
    /// <summary>
    /// Gets or sets the sub title.
    /// </summary>
    /// <value>
    /// The sub title.
    /// </value>
    public string SubTitle { get; private set; }
    /// <summary>
    /// Gets or sets the parent key.
    /// </summary>
    /// <value>
    /// The parent key.
    /// </value>
    public string ParentKey { get; private set; }
    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    /// <value>
    /// The key.
    /// </value>
    public string Key { get; private set; }
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="PageInfo"/> is visible.
    /// </summary>
    /// <value>
    ///   <c>true</c> if visible; otherwise, <c>false</c>.
    /// </value>
    public bool Visible { get; private set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance is menu item.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is menu item; otherwise, <c>false</c>.
    /// </value>
    public bool IsMenuItem { get; private set; }
    /// <summary>
    /// Gets or sets the menu order.
    /// </summary>
    /// <value>
    /// The menu order.
    /// </value>
    public int MenuOrder { get; private set; }
    /// <summary>
    /// Gets the menu only visible for roles.
    /// </summary>
    /// <value>
    /// The menu only visible for roles.
    /// </value>
    public List<string> MenuOnlyVisibleForRoles { get; private set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance is home page item.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is home page item; otherwise, <c>false</c>.
    /// </value>
    public bool IsHomePageItem { get; private set; }
    /// <summary>
    /// Gets or sets the page URL.
    /// </summary>
    /// <value>
    /// The page URL.
    /// </value>
    public string PageUrl { get; private set; }
    /// <summary>
    /// Specifies the redirect urls (301).
    /// </summary>
    /// <value>
    /// The redirect urls.
    /// </value>
    public string[] RedirectUrls { get; set; }
    /// <summary>
    /// Gets or sets the alternate page URL.
    /// </summary>
    /// <value>
    /// The alternate page URL.
    /// </value>
    public string AlternatePageUrl { get; private set; }
    /// <summary>
    /// Gets or sets a value indicating whether [block URL].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [block URL]; otherwise, <c>false</c>.
    /// </value>
    public bool BlockUrl { get; set; }
    /// <summary>
    /// Gets or sets the meta title.
    /// </summary>
    /// <value>
    /// The meta title.
    /// </value>
    public string MetaTitle { get; private set; }
    /// <summary>
    /// Gets or sets the meta description.
    /// </summary>
    /// <value>
    /// The meta description.
    /// </value>
    public string MetaDescription { get; private set; }
    /// <summary>
    /// Gets or sets a value indicating whether [meta no index].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [meta no index]; otherwise, <c>false</c>.
    /// </value>
    public bool MetaNoIndex { get; private set; }
    /// <summary>
    /// Gets or sets a value indicating whether [meta no follow].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [meta no follow]; otherwise, <c>false</c>.
    /// </value>
    public bool MetaNoFollow { get; private set; }
    /// <summary>
    /// Gets or sets the tags.
    /// </summary>
    /// <value>
    /// The tags.
    /// </value>
    public string Tags { get; private set; }
    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    /// <value>
    /// The image.
    /// </value>
    public string Image { get; private set; }
    /// <summary>
    /// Gets or sets the summary.
    /// </summary>
    /// <value>
    /// The summary.
    /// </value>
    public string Summary { get; private set; }
    /// <summary>
    /// Gets or sets the related infos.
    /// </summary>
    /// <value>
    /// The related infos.
    /// </value>
    public List<RelatedInfo> RelatedInfos { get; private set; }
    /// <summary>
    /// Gets or sets the parent.
    /// </summary>
    /// <value>
    /// The parent.
    /// </value>
    public PageInfo Parent { get; private set; }
    /// <summary>
    /// Gets or sets the children.
    /// </summary>
    /// <value>
    /// The children.
    /// </value>
    public List<PageInfo> Children { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether this instance is valid.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
    /// </value>
    public bool IsValid => !string.IsNullOrEmpty(Action) && !string.IsNullOrEmpty(Controller) && !string.IsNullOrEmpty(Key) && !string.IsNullOrEmpty(Title);

    /// <summary>
    /// Gets or sets the property infos.
    /// </summary>
    /// <value>
    /// The property infos.
    /// </value>
    public Dictionary<string, string> PropertyInfos { get; set; }

    /// <summary>
    /// Gets or sets the authorization roles.
    /// </summary>
    /// <value>
    /// The authorization roles.
    /// </value>
    public string AuthorizationRoles { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PageInfo"/> class.
    /// </summary>
    public PageInfo()
    {
      RelatedInfos = new List<RelatedInfo>();
      RedirectUrls = new string[0];
      PropertyInfos = new Dictionary<string, string>();

      Children = new List<PageInfo>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PageInfo" /> class.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="controller">The controller.</param>
    /// <param name="area">The area.</param>
    /// <param name="controllerType">Type of the controller.</param>
    /// <param name="pageInfoAttribute">The page information attribute.</param>
    /// <param name="metaInfoAttribute">The meta information attribute.</param>
    /// <param name="menuInfoAttribute">The menu information attribute.</param>
    /// <param name="routeInfoAttribute">The route information attribute.</param>
    /// <param name="tagInfoAttribute">The tag information attribute.</param>
    /// <param name="visibleInfoAttribute">The visible information attribute.</param>
    /// <param name="relatedInfoAttributes">The related information attributes.</param>
    /// <param name="propertyInfoAttributes">The property information attributes.</param>
    /// <param name="authorizationRoles">The authorization roles.</param>
    // ReSharper disable once FunctionComplexityOverflow
    public PageInfo(
        string action,
        string controller,
        string area,
        Type controllerType,
        PageInfoAttribute pageInfoAttribute,
        MetaInfoAttribute metaInfoAttribute,
        MenuInfoAttribute menuInfoAttribute,
        RouteInfoAttribute routeInfoAttribute,
        TagInfoAttribute tagInfoAttribute,
        VisibleInfoAttribute visibleInfoAttribute,
        IEnumerable<RelatedInfoAttribute> relatedInfoAttributes,
        IEnumerable<CustomPropertyInfoAttribute> propertyInfoAttributes,
        string authorizationRoles)
    {
      Children = new List<PageInfo>();

      Action = action;
      Controller = controller;
      Area = area;

      ControllerType = controllerType;

      Title = pageInfoAttribute != null ? pageInfoAttribute.Title : "";
      SubTitle = pageInfoAttribute != null ? pageInfoAttribute.SubTitle : "";

      AlternatePageUrl = (Action != "Index" ? Action : Controller).ToLowerInvariant().Trim() + "/";
      PageUrl = routeInfoAttribute != null ? (string.IsNullOrEmpty(routeInfoAttribute.Url) ? AlternatePageUrl : routeInfoAttribute.Url) : AlternatePageUrl;
      if (!string.IsNullOrEmpty(PageUrl) && !PageUrl.EndsWith("/")) PageUrl = PageUrl += "/";

      ParentKey = routeInfoAttribute != null ? routeInfoAttribute.ParentKey : "";
      Key = routeInfoAttribute != null ? routeInfoAttribute.Key : "";
      RedirectUrls = routeInfoAttribute != null ? (routeInfoAttribute.RedirectUrls ?? new string[0]) : new string[0];
      BlockUrl = routeInfoAttribute?.BlockUrl ?? false;

      Visible = visibleInfoAttribute == null || visibleInfoAttribute.Visible;

      IsMenuItem = menuInfoAttribute != null;
      MenuOrder = menuInfoAttribute?.Order ?? 0;

      MenuOnlyVisibleForRoles = menuInfoAttribute?.Roles?.ToList();
      IsHomePageItem = pageInfoAttribute != null && pageInfoAttribute.IsHomePageItem;

      MetaTitle = metaInfoAttribute != null ? metaInfoAttribute.Title : "";
      MetaDescription = metaInfoAttribute != null ? metaInfoAttribute.Description : "";
      MetaNoIndex = metaInfoAttribute != null && metaInfoAttribute.NoIndex;
      MetaNoFollow = metaInfoAttribute != null && metaInfoAttribute.NoFollow;

      Tags = tagInfoAttribute != null ? tagInfoAttribute.Tags : "";
      Image = pageInfoAttribute != null ? pageInfoAttribute.Image : "";
      Summary = pageInfoAttribute != null ? pageInfoAttribute.Summary : "";

      RelatedInfos = new List<RelatedInfo>();
      if (relatedInfoAttributes != null)
      {
        foreach (var relatedInfoAttribute in relatedInfoAttributes)
        {
          RelatedInfos.Add(new RelatedInfo(relatedInfoAttribute.Title, relatedInfoAttribute.Url));
        }
      }

      PropertyInfos = new Dictionary<string, string>();
      if (propertyInfoAttributes != null) PropertyInfos = propertyInfoAttributes.ToDictionary(p => p.Key, p => p.Value);

      AuthorizationRoles = authorizationRoles ?? "";
    }

    /// <summary>
    /// Breadcrumbses this instance.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<PageInfo> Breadcrumbs()
    {
      var breadcrumbs = new List<PageInfo>();

      var parent = this;
      while (parent != null)
      {
        if (parent.IsValid) breadcrumbs.Insert(0, parent);

        parent = parent.Parent;
      }

      return breadcrumbs;
    }

    /// <summary>
    /// Menus this instance.
    /// </summary>
    /// <param name="isInRole">The is in role.</param>
    /// <returns></returns>
    public IEnumerable<PageInfo> Menu(Func<string, bool> isInRole = null)
    {
      if (isInRole == null)
      {
        return Children.Where(pi => pi.IsMenuItem && pi.MenuOnlyVisibleForRoles == null).OrderBy(pi => pi.MenuOrder);
      }

      var pageInfos = new List<PageInfo>();
      foreach (var pageInfo in Children.Where(pi => pi.IsMenuItem))
      {
        if (pageInfo.MenuOnlyVisibleForRoles != null)
        {
          pageInfos.AddRange(from role in pageInfo.MenuOnlyVisibleForRoles where isInRole(role) select pageInfo);
        }
        else
        {
          pageInfos.Add(pageInfo);
        }
      }

      return pageInfos.OrderBy(pi => pi.MenuOrder);
    }

    /// <summary>
    /// Adds the specified site map item information.
    /// </summary>
    /// <param name="siteMapItemInfo">The site map item information.</param>
    public void Add(PageInfo siteMapItemInfo)
    {
      siteMapItemInfo.Parent = this;
      Children.Add(siteMapItemInfo);
    }

    /// <summary>
    /// Removes the specified site map item information.
    /// </summary>
    /// <param name="siteMapItemInfo">The site map item information.</param>
    public void Remove(PageInfo siteMapItemInfo)
    {
      siteMapItemInfo.Parent = null;
      Children.Remove(siteMapItemInfo);
    }

    /// <summary>
    /// Finds the by key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public PageInfo FindByKey(string key)
    {
      return FindByKey(this, key);
    }

    /// <summary>
    /// Finds the by action controller.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="controller">The controller.</param>
    /// <returns></returns>
    public PageInfo FindByActionController(string action, string controller)
    {
      return FindByActionController(this, action, controller);
    }

    /// <summary>
    /// Adds the specified root.
    /// </summary>
    /// <param name="root">The root.</param>
    /// <param name="parentKey">The parent key.</param>
    /// <param name="siteMapItemInfo">The site map item information.</param>
    public static void Add(PageInfo root, string parentKey, PageInfo siteMapItemInfo)
    {
      var item = FindByKey(root, parentKey) ?? root;
      item.Add(siteMapItemInfo);
    }

    /// <summary>
    /// Finds the by key.
    /// </summary>
    /// <param name="root">The root.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static PageInfo FindByKey(PageInfo root, string key)
    {
      if (root == null) return null;

      if (string.IsNullOrEmpty(key) || (root.Key ?? "").ToLowerInvariant() == key.ToLowerInvariant()) return root;

      return FindByKey(root.Children, key);
    }

    /// <summary>
    /// Finds the by key.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static PageInfo FindByKey(IEnumerable<PageInfo> items, string key)
    {
      return items?.Select(child => FindByKey(child, key)).FirstOrDefault(item => item != null);
    }

    /// <summary>
    /// Finds the by action controller.
    /// </summary>
    /// <param name="root">The root.</param>
    /// <param name="action">The action.</param>
    /// <param name="controller">The controller.</param>
    /// <returns></returns>
    public static PageInfo FindByActionController(PageInfo root, string action, string controller)
    {
      if (root == null) return null;

      if (string.Equals(root.Action, action, StringComparison.InvariantCultureIgnoreCase) && root.Controller.ToLowerInvariant() == controller.ToLowerInvariant()) return root;

      return FindByActionController(root.Children, action, controller);
    }

    /// <summary>
    /// Finds the by action controller.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="action">The action.</param>
    /// <param name="controller">The controller.</param>
    /// <returns></returns>
    public static PageInfo FindByActionController(IEnumerable<PageInfo> items, string action, string controller)
    {
      return items?.Select(child => FindByActionController(child, action, controller)).FirstOrDefault(item => item != null);
    }

    /// <summary>
    /// Finds the by URL.
    /// </summary>
    /// <param name="root">The root.</param>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    public static PageInfo FindByUrl(PageInfo root, string url)
    {
      if (root == null) return null;
      if (string.IsNullOrEmpty(url)) return null;
      if (string.IsNullOrEmpty(root.PageUrl)) return null;

      if (string.Equals(root.PageUrl, url, StringComparison.InvariantCultureIgnoreCase)) return root;

      return FindByUrl(root.Children, url);
    }

    /// <summary>
    /// Finds the by URL.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    public static PageInfo FindByUrl(IEnumerable<PageInfo> items, string url)
    {
      return items?.Select(child => FindByUrl(child, url)).FirstOrDefault(item => item != null);
    }

    /// <summary>
    /// Finds the by alternate URL.
    /// </summary>
    /// <param name="root">The root.</param>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    public static PageInfo FindByAlternateUrl(PageInfo root, string url)
    {
      if (root == null) return null;
      if (string.IsNullOrEmpty(url)) return null;
      if (string.IsNullOrEmpty(root.PageUrl)) return null;

      if (string.Equals(root.AlternatePageUrl, url, StringComparison.InvariantCultureIgnoreCase)) return root;

      return FindByAlternateUrl(root.Children, url);
    }

    /// <summary>
    /// Finds the by URL.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    public static PageInfo FindByAlternateUrl(IEnumerable<PageInfo> items, string url)
    {
      return items?.Select(child => FindByAlternateUrl(child, url)).FirstOrDefault(item => item != null);
    }
  }
}
