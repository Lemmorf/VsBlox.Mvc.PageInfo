using System;
using System.Reflection;

namespace VsBlox.Mvc.PageInfo.Attributes
{
  /// <summary>
  /// Defines UI info for the page (Action / Url).
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class PageInfoAttribute : Attribute
  {
    private const BindingFlags ReflectionBindingFlags = BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public;
    private string _title;
    private string _subTitle;
    private string _summary;

    /// <summary>
    /// Specifies the page title (menu)
    /// </summary>
    /// <value>
    /// Title.
    /// </value>
    public string Title
    {
      get
      {
        if (string.IsNullOrEmpty(TitleResourceName) || TitleResourceType == null) return _title;
        var propertyInfo = TitleResourceType.GetProperty(TitleResourceName, ReflectionBindingFlags);
        return propertyInfo != null ? (string)propertyInfo.GetValue(null, null) : _title;
      }
      set { _title = value; }
    }
    /// <summary>
    /// Gets or sets the name of the title resource.
    /// </summary>
    /// <value>
    /// The name of the title resource.
    /// </value>
    public string TitleResourceName { get; set; }

    /// <summary>
    /// Gets or sets the type of the title resource.
    /// </summary>
    /// <value>
    /// The type of the title resource.
    /// </value>
    public Type TitleResourceType { get; set; }
    /// <summary>
    /// Specifies the (optional) header title (if not defined, Title will be used).
    /// </summary>
    /// <value>
    /// Subtitle.
    /// </value>
    public string SubTitle
    {
      get
      {
        if (string.IsNullOrEmpty(SubTitleResourceName) || SubTitleResourceType == null) return _subTitle;
        var propertyInfo = SubTitleResourceType.GetProperty(SubTitleResourceName, ReflectionBindingFlags);
        return propertyInfo != null ? (string)propertyInfo.GetValue(null, null) : _subTitle;
      }
      set { _subTitle = value; }
    }
    /// <summary>
    /// Gets or sets the name of the sub title resource.
    /// </summary>
    /// <value>
    /// The name of the sub title resource.
    /// </value>
    public string SubTitleResourceName { get; set; }
    /// <summary>
    /// Gets or sets the type of the sub title resource.
    /// </summary>
    /// <value>
    /// The type of the sub title resource.
    /// </value>
    public Type SubTitleResourceType { get; set; }
    /// <summary>
    /// Specifies whether the summary of this page is the home page.
    /// </summary>
    /// <value>
    /// <c>true</c> is home page; otherwise, <c>false</c>.
    /// </value>
    public bool IsHomePageItem { get; set; }
    /// <summary>
    /// Specifies the image associated with this page.
    /// </summary>
    /// <value>
    /// Image.
    /// </value>
    public string Image { get; set; }
    /// <summary>
    /// Specifies the summary text for this page.
    /// </summary>
    /// <value>
    /// Summary.
    /// </value>
    public string Summary
    {
      get
      {
        if (string.IsNullOrEmpty(SummaryResourceName) || SummaryResourceType == null) return _summary;
        var propertyInfo = SummaryResourceType.GetProperty(SummaryResourceName, ReflectionBindingFlags);
        return propertyInfo != null ? (string)propertyInfo.GetValue(null, null) : _summary;
      }
      set { _summary = value; }
    }
    /// <summary>
    /// Gets or sets the name of the summary resource.
    /// </summary>
    /// <value>
    /// The name of the summary resource.
    /// </value>
    public string SummaryResourceName { get; set; }
    /// <summary>
    /// Gets or sets the type of the summary resource.
    /// </summary>
    /// <value>
    /// The type of the summary resource.
    /// </value>
    public Type SummaryResourceType { get; set; }
  }
}