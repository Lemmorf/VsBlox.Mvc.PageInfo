using System;

namespace VsBlox.Mvc.PageInfo.Attributes
{
  /// <summary>
  /// The action associated with this attribute will be ignored bij the PageInfo factory.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class IgnorePageInfoAttribute : Attribute
  {
  }
}