using System;
using System.Reflection;

namespace VsBlox.Mvc.PageInfo.Attributes
{
  /// <summary>
  /// Defines the metadata for the page (Action / Url).
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class MetaInfoAttribute : Attribute
  {
    private const BindingFlags ReflectionBindingFlags = BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public;
    private string _title;
    private string _description;

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
    /// Specifies   &lt;meta name="description"&gt;.
    /// </summary>
    /// <value>
    /// Description.
    /// </value>
    public string Description
    {
      get
      {
        if (string.IsNullOrEmpty(DescriptionResourceName) || DescriptionResourceType == null) return _description;
        var propertyInfo = DescriptionResourceType.GetProperty(DescriptionResourceName, ReflectionBindingFlags);
        return propertyInfo != null ? _description = (string)propertyInfo.GetValue(null, null) : _description;
      }
      set { _description = value; }
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
    /// Gets or sets the name of the description resource.
    /// </summary>
    /// <value>
    /// The name of the description resource.
    /// </value>
    public string DescriptionResourceName { get; set; }

    /// <summary>
    /// Gets or sets the type of the description resource.
    /// </summary>
    /// <value>
    /// The type of the description resource.
    /// </value>
    public Type DescriptionResourceType { get; set; }
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
  }
}