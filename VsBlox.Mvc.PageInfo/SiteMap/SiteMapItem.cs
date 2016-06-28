using System;

namespace VsBlox.Mvc.PageInfo.SiteMap
{
  /// <summary>
  /// Defines an item in the site map.
  /// </summary>
  public class SiteMapItem
  {
    /// <summary>
    /// Creates a new instance of <see cref="SiteMapItem"/>
    /// </summary>
    /// <param name="url">URL of the page. Optional.</param>
    /// <param name="lastModified">The date of last modification of the file. Optional.</param>
    /// <param name="changeFrequency">How frequently the page is likely to change. Optional.</param>
    /// <param name="priority">The priority of this URL relative to other URLs on your site. Valid values range from 0.0 to 1.0. Optional.</param>
    /// <exception cref="System.ArgumentNullException">If the <paramref name="url"/> is null or empty.</exception>
    public SiteMapItem(string url, DateTime? lastModified = null, SitemapChangeFrequency? changeFrequency = null, double? priority = null)
    {
      if (url == null) throw new ArgumentNullException(nameof(url));
      if (url == string.Empty) throw new ArgumentException("Argument cannot be empty (url)");

      Url = url;
      LastModified = lastModified;
      ChangeFrequency = changeFrequency;
      Priority = priority;
    }

    /// <summary>
    /// URL of the page.
    /// </summary>
    public string Url { get; protected set; }

    /// <summary>
    /// The date of last modification of the file.
    /// </summary>
    public DateTime? LastModified { get; protected set; }

    /// <summary>
    /// How frequently the page is likely to change.
    /// </summary>
    public SitemapChangeFrequency? ChangeFrequency { get; protected set; }

    /// <summary>
    /// The priority of this URL relative to other URLs on your site. Valid values range from 0.0 to 1.0.
    /// </summary>
    public double? Priority { get; protected set; }
  }
}