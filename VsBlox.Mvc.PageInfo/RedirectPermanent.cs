using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VsBlox.Mvc.PageInfo
{
  /// <summary>
  /// Helper class to permanently redirect urls (301 - redirection)
  /// </summary>
  public static class RedirectPermanent
  {
    private static readonly Dictionary<string, string> RedirectInfo = new Dictionary<string, string>();

    /// <summary>
    /// Adds the redirect.
    /// </summary>
    /// <param name="redirectUrl">The redirect URL.</param>
    /// <param name="targetUrl">The target URL.</param>
    /// <exception cref="System.ArgumentNullException">
    /// </exception>
    /// <exception cref="System.InvalidOperationException">Cannot add redirection. Target already exists.</exception>
    public static void AddRedirect(string redirectUrl, string targetUrl)
    {
      if (string.IsNullOrEmpty(redirectUrl)) throw new ArgumentNullException(nameof(redirectUrl));
      if (string.IsNullOrEmpty(targetUrl)) throw new ArgumentNullException(nameof(targetUrl));

      if (RedirectInfo.ContainsKey(redirectUrl))
      {
        if (!string.Equals(RedirectInfo[redirectUrl], targetUrl, StringComparison.InvariantCultureIgnoreCase)) throw new InvalidOperationException("Cannot add redirection. Target already exists.");
        return;
      }

      RedirectInfo.Add(redirectUrl, targetUrl.TrimStart('/'));
    }

    /// <summary>
    /// Adds the redirect.
    /// </summary>
    /// <param name="redirectUrl">The redirect URL.</param>
    /// <param name="action">The action.</param>
    /// <param name="controller">The controller.</param>
    /// <param name="area">The area.</param>
    /// <exception cref="System.ArgumentNullException">
    /// </exception>
    /// <exception cref="System.InvalidOperationException">Cannot add redirection. Target already exists.</exception>
    public static void AddRedirect(string redirectUrl, string action, string controller, string area = null)
    {
      if (string.IsNullOrEmpty(redirectUrl)) throw new ArgumentNullException(nameof(redirectUrl));
      if (string.IsNullOrEmpty(action)) throw new ArgumentNullException(nameof(action));
      if (string.IsNullOrEmpty(controller)) throw new ArgumentNullException(nameof(controller));

      HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.org", ""), new HttpResponse(new StringWriter()));
      var wrapper = new HttpContextWrapper(HttpContext.Current);

      var urlHelper = new UrlHelper(new RequestContext(wrapper, new RouteData()), RouteTable.Routes);

      AddRedirect(redirectUrl, urlHelper.Action(action, controller, new { Area = area ?? "" }));
    }

    /// <summary>
    /// Adds the range redirect.
    /// </summary>
    /// <param name="redirectInfo">The redirect information.</param>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static void AddRangeRedirect(Dictionary<string, string> redirectInfo)
    {
      if (redirectInfo == null) throw new ArgumentNullException(nameof(redirectInfo));

      foreach (var info in redirectInfo)
      {
        RedirectInfo.Add(info.Key, info.Value);
      }
    }

    /// <summary>
    /// Maps the redirect.
    /// </summary>
    /// <param name="redirectUrl">The redirect URL.</param>
    /// <returns></returns>
    public static string MapRedirect(string redirectUrl)
    {
      redirectUrl = redirectUrl.TrimStart('/').ToLowerInvariant();

      if (!redirectUrl.EndsWith("/")) redirectUrl += "/";
      if (string.IsNullOrEmpty(redirectUrl) || !RedirectInfo.ContainsKey(redirectUrl)) return string.Empty;

      // 301 it to the new url
      var targetUrl = RedirectInfo[redirectUrl];

      // empty means: should not occur, return a 404.
      if (!string.IsNullOrEmpty(targetUrl) &&
          !string.Equals(redirectUrl, targetUrl, StringComparison.InvariantCultureIgnoreCase))
      {
        if (!targetUrl.StartsWith("/")) targetUrl = "/" + targetUrl;
        return targetUrl;
      }

      return string.Empty;
    }
  }
}
