using System;
using System.Collections.Generic;

namespace VsBlox.Mvc.PageInfo.Attributes
{
  /// <summary>
  /// Defines route info associated with the page (Action / Url).
  /// Key and ParentKey are used to build a tree pages.
  /// The tree will be used to create breadcrumbs and menu structures.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class RouteInfoAttribute : Attribute
  {
    private string _url;
    private string[] _redirectUrls;

    /// <summary>
    /// Specifies the key of the parent of this page.
    /// </summary>
    /// <value>
    /// Parent key.
    /// </value>
    public string ParentKey { get; set; }
    /// <summary>
    /// Specifies the key of this page.
    /// </summary>
    /// <value>
    /// Key.
    /// </value>
    public string Key { get; set; }

    /// <summary>
    /// Specifies (overrules) the url of this page.
    /// </summary>
    /// <value>
    /// Url.
    /// </value>
    public string Url
    {
      get { return _url; }
      set
      {
        if (string.IsNullOrEmpty(value)) return;
        _url = value.ToLowerInvariant().Trim();
        if (!_url.EndsWith("/")) _url += "/";
      }
    }

    /// <summary>
    /// Specifies the redirect urls (301).
    /// </summary>
    /// <value>
    /// The redirect urls.
    /// </value>
    public string[] RedirectUrls
    {
      get { return _redirectUrls; }
      set
      {
        if (value == null) { _redirectUrls = new string[0]; return; }

        var list = new List<string>();
        foreach (var val in value)
        {
          var url = val.ToLowerInvariant().Trim();
          if (string.IsNullOrEmpty(url)) continue;
          if (!url.EndsWith("/")) url += "/";
          list.Add(url);
        }

        _redirectUrls = list.ToArray();
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [block URL].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [block URL]; otherwise, <c>false</c>.
    /// </value>
    public bool BlockUrl { get; set; }
  }
}