using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace VsBlox.Mvc.PageInfo.SiteMap
{
  /// <summary>
  /// A class for creating XML Sitemaps (see http://www.sitemaps.org/protocol.html)
  /// </summary>
  public class SitemapGenerator
  {
    private static readonly XNamespace Xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
    private static readonly XNamespace Xsi = "http://www.w3.org/2001/XMLSchema-instance";

    /// <summary>
    /// Generates the site map.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <returns></returns>
    public virtual XDocument GenerateSiteMap(IEnumerable<SiteMapItem> items)
    {
      if (items == null) throw new ArgumentNullException(nameof(items));

      var sitemap = new XDocument(
        new XDeclaration("1.0", "utf-8", "yes"),
        new XElement(Xmlns + "urlset",
        new XAttribute("xmlns", Xmlns),
        new XAttribute(XNamespace.Xmlns + "xsi", Xsi),
        new XAttribute(Xsi + "schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd"),
        from item in items select CreateItemElement(item)));

      return sitemap;
    }

    /// <summary>
    /// Creates the item element.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    private XElement CreateItemElement(SiteMapItem item)
    {
      var itemElement = new XElement(Xmlns + "url", new XElement(Xmlns + "loc", item.Url.ToLowerInvariant()));

      // all other elements are optional
      if (item.LastModified.HasValue) itemElement.Add(new XElement(Xmlns + "lastmod", item.LastModified.Value.ToString("yyyy-MM-dd")));
      if (item.ChangeFrequency.HasValue) itemElement.Add(new XElement(Xmlns + "changefreq", item.ChangeFrequency.Value.ToString().ToLower()));
      if (item.Priority.HasValue) itemElement.Add(new XElement(Xmlns + "priority", item.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));

      return itemElement;
    }
  }
}