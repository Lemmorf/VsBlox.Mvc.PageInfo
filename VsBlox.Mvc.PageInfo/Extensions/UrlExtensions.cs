using System.Linq;
using System.Web.Mvc;

namespace VsBlox.Mvc.PageInfo.Extensions
{
  /// <summary>
  /// UrlHelper extension methods.
  /// </summary>
  internal static class UrlExtensions
  {
    /// <summary>
    /// Return absolute url.
    /// </summary>
    /// <param name="urlHelper">The URL helper.</param>
    /// <param name="relativeUrl">The relative URL.</param>
    /// <param name="routeValues">The route values.</param>
    /// <returns></returns>
    public static string AbsoluteUrlFromRelavtiveUrl(this UrlHelper urlHelper, string relativeUrl, object routeValues = null)
    {
      string absoluteUrl;

      if (!string.IsNullOrEmpty(relativeUrl) && relativeUrl.StartsWith("/")) relativeUrl = relativeUrl.TrimStart('/');

      if (urlHelper.RequestContext.HttpContext.Request.Url == null)
      {
        absoluteUrl = relativeUrl;
      }
      else
      {
        var scheme = urlHelper.RequestContext.HttpContext.Request.Url.Scheme;
        var port = new[] { 80, 443 }.Contains(urlHelper.RequestContext.HttpContext.Request.Url.Port) ? "" : $":{urlHelper.RequestContext.HttpContext.Request.Url.Port}";
        absoluteUrl = $"{scheme}://{urlHelper.RequestContext.HttpContext.Request.Url.Host}{port}/{relativeUrl}";
      }

      if (string.IsNullOrEmpty(absoluteUrl)) return absoluteUrl;
      if (!absoluteUrl.EndsWith("/")) absoluteUrl += "/";

      return absoluteUrl;
    }
  }
}