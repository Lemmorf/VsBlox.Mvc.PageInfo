using System;

namespace VsBlox.Mvc.PageInfo.Attributes
{
  /// <summary>
  /// Defines a custom property for the page (Action / Url)
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  public class CustomPropertyInfoAttribute : Attribute
  {
    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    /// <value>
    /// The key.
    /// </value>
    public string Key { get; set; }
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public string Value { get; set; }
  }
}