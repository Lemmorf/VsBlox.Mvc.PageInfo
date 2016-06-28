using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VsBlox.Mvc.PageInfo.Attributes
{
  /// <summary>
  /// Defines tags associated with the page (Action / Url).
  /// The tags are used to build a list with related pages.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class TagInfoAttribute : Attribute
  {
    /// <summary>
    /// List with all associated tags.
    /// </summary>
    internal readonly List<string> TagsList = new List<string>();
    /// <summary>
    /// Gets a string with associated tags.
    /// </summary>
    /// <value>
    /// Tags.
    /// </value>
    public string Tags
    {
      get
      {
        var value = new StringBuilder();
        foreach (var tag in TagsList)
        {
          if (value.Length > 0) value.Append(",");
          value.Append(tag);
        }
        return value.ToString();
      }
      set
      {
        TagsList.Clear();

        if (string.IsNullOrEmpty(value)) return;
        TagsList.AddRange(value.Split(',').Select(t => t.Trim().ToLowerInvariant()));
      }
    }
  }
}