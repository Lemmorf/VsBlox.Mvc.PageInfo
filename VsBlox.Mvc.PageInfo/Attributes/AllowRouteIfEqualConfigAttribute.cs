using System;
using System.Configuration;

namespace VsBlox.Mvc.PageInfo.Attributes
{
  /// <summary>
  /// Defines whether the page (Action / Url) is visible in the web site or not.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  public class AllowRouteIfEqualConfigAttribute : Attribute
  {
    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    /// <value>
    /// The key.
    /// </value>
    public string Key { get; set; }
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public string Value { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AllowRouteIfEqualConfigAttribute" /> class.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public AllowRouteIfEqualConfigAttribute(string key, string value)
    {
      Key = key;
      Value = value;
    }

    /// <summary>
    /// Determines whether this instance is allowed.
    /// </summary>
    /// <returns></returns>
    public bool IsAllowed()
    {
      if (string.IsNullOrEmpty(Key)) return false;
      if (string.IsNullOrEmpty(Value)) return false;

      var value = ConfigurationManager.AppSettings.Get(Key);
      if (string.IsNullOrEmpty(value)) return false;

      return String.Equals(value, Value, StringComparison.InvariantCultureIgnoreCase);
    }
  }
}