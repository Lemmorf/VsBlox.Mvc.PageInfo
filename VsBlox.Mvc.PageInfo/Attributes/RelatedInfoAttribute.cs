using System;
using System.Reflection;

namespace VsBlox.Mvc.PageInfo.Attributes
{
  /// <summary>
  /// Defines related (external) url's with the page (Action / Url).
  /// </summary>
  [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
  public class RelatedInfoAttribute : Attribute
  {
    private const BindingFlags ReflectionBindingFlags = BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public;
    private string _title;

    /// <summary>
    /// Specifies &lt;title&gt;
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
    /// Specifies the external url.
    /// </summary>
    /// <value>
    /// Url.
    /// </value>
    public string Url { get; set; }
  }
}