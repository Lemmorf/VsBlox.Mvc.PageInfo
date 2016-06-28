using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace VsBlox.Mvc.PageInfo.SiteMap
{
  /// <summary>
  /// Returns the generated site map as an ActionResult.
  /// </summary>
  public class SitemapResult : ActionResult
  {
    private readonly IEnumerable<SiteMapItem> _items;
    private readonly SitemapGenerator _generator;

    /// <summary>
    /// Initializes a new instance of the <see cref="SitemapResult"/> class.
    /// </summary>
    /// <param name="items">The items.</param>
    public SitemapResult(IEnumerable<SiteMapItem> items) : this(items, new SitemapGenerator())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SitemapResult"/> class.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="generator">The generator.</param>
    public SitemapResult(IEnumerable<SiteMapItem> items, SitemapGenerator generator)
    {
      if (items == null) throw new ArgumentNullException(nameof(items));
      if (generator == null) throw new ArgumentNullException(nameof(generator));

      _items = items;
      _generator = generator;
    }

    /// <summary>
    /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.
    /// </summary>
    /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
    public override void ExecuteResult(ControllerContext context)
    {
      var response = context.HttpContext.Response;

      response.ContentType = "text/xml";
      response.ContentEncoding = Encoding.UTF8;

      using (var writer = new XmlTextWriter(response.Output))
      {
        writer.Formatting = Formatting.Indented;
        var sitemap = _generator.GenerateSiteMap(_items);

        sitemap.WriteTo(writer);
      }
    }
  }
}