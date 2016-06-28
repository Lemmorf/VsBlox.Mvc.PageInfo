namespace VsBlox.Mvc.PageInfo.SiteMap
{
  /// <summary>
  /// How frequently the page is likely to change. 
  /// This value provides general information to search engines 
  /// and may not correlate exactly to how often they crawl the page.
  /// </summary>
  /// <remarks>
  /// The value "always" should be used to describe documents that change 
  /// each time they are accessed. The value "never" should be 
  /// used to describe archived URLs.
  /// </remarks>
  public enum SitemapChangeFrequency
  {
    /// <summary>
    /// Changes always.
    /// </summary>
    Always,
    /// <summary>
    /// Changes every hour.
    /// </summary>
    Hourly,
    /// <summary>
    /// Changes daily.
    /// </summary>
    Daily,
    /// <summary>
    /// Changes weekly.
    /// </summary>
    Weekly,
    /// <summary>
    /// Changes monthly.
    /// </summary>
    Monthly,
    /// <summary>
    /// Changes yearly.
    /// </summary>
    Yearly,
    /// <summary>
    /// Changes never.
    /// </summary>
    Never
  }
}