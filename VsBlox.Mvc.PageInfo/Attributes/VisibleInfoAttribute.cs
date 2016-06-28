using System;

namespace VsBlox.Mvc.PageInfo.Attributes
{
  /// <summary>
  /// Defines whether the page (Action / Url) is visible in the web site or not.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class VisibleInfoAttribute : Attribute
  {
    /// <summary>
    /// Specifies whether the page is visible or not.
    /// </summary>
    /// <value>
    ///   <c>true</c> if visible; otherwise, <c>false</c>.
    /// </value>
    public bool Visible { get; set; }
  }
}