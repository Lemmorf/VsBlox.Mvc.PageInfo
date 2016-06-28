using System;

namespace VsBlox.Mvc.PageInfo.Attributes
{
  /// <summary>
  /// Defines the menu order of the page (Action / Url). Default order is 0 (left most).
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class MenuInfoAttribute : Attribute
  {
    /// <summary>
    /// Gets or sets the order.
    /// </summary>
    /// <value>
    /// The order.
    /// </value>
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the roles.
    /// </summary>
    /// <value>
    /// The roles.
    /// </value>
    public string[] Roles { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuInfoAttribute"/> class.
    /// </summary>
    public MenuInfoAttribute()
    {
      Order = 1;
    }
  }
}