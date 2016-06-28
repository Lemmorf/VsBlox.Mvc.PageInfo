namespace VsBlox.Mvc.PageInfo
{
  /// <summary>
  /// Page meta data.
  /// </summary>
  public class MetaDataInfo
  {
    /// <summary>
    /// Specifies &lt;title&gt;
    /// </summary>
    /// <value>
    /// Title.
    /// </value>
    public string Title { get; set; }
    /// <summary>
    /// Specifies   &lt;meta name="description"&gt;.
    /// </summary>
    /// <value>
    /// Description.
    /// </value>
    public string Description { get; set; }
    /// <summary>
    /// Specifies &lt;meta name="robots"&gt;
    /// </summary>
    /// <value>
    ///   <c>true</c> if [no index]; otherwise, <c>false</c>.
    /// </value>
    public bool NoIndex { get; set; }
    /// <summary>
    /// Specifies &lt;meta name="robots"&gt;
    /// </summary>
    /// <value>
    ///   <c>true</c> if [no follow]; otherwise, <c>false</c>.
    /// </value>
    public bool NoFollow { get; set; }
    /// <summary>
    /// Generate string for robots meta tag.
    /// </summary>
    /// <returns></returns>
    public string Robots()
    {
      var robots = "";

      if (NoIndex) robots += "noindex";
      if (!NoFollow) return robots;

      if (robots.Length > 0) robots += ",";
      robots += "nofollow";

      return robots;
    }
  }
}