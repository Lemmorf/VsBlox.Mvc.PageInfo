using System.Web.Mvc;

namespace VsBlox.Mvc.PageInfo.Extensions
{
  /// <summary>
  /// ViewContext extensions
  /// </summary>
  public static class ViewContextExtensions
  {
    /// <summary>
    /// Returns context information for the current request.
    /// </summary>
    /// <param name="viewContext">The view context.</param>
    /// <returns></returns>
    public static ViewContextInformation ContextInformation(this ViewContext viewContext)
    {
      var viewContextInformation = new ViewContextInformation();

      if (viewContext == null) return viewContextInformation;

      var parentRouteValues = viewContext.RouteData.Values;
      viewContextInformation.Action = (string)parentRouteValues["action"];
      viewContextInformation.Controller = (string)parentRouteValues["controller"];
      viewContextInformation.MetaDataInfo = PageInfoFactory.MetaData(viewContextInformation.Action, viewContextInformation.Controller);
      viewContextInformation.PageInfo = PageInfoFactory.FindByActionController(viewContextInformation.Action, viewContextInformation.Controller);

      return viewContextInformation;
    }
  }

  /// <summary>
  /// ViewContext innformation
  /// </summary>
  public class ViewContextInformation
  {
    /// <summary>
    /// Gets or sets the action.
    /// </summary>
    /// <value>
    /// The action.
    /// </value>
    public string Action { get; set; }
    /// <summary>
    /// Gets or sets the controller.
    /// </summary>
    /// <value>
    /// The controller.
    /// </value>
    public string Controller { get; set; }
    /// <summary>
    /// Gets or sets the meta data information.
    /// </summary>
    /// <value>
    /// The meta data information.
    /// </value>
    public MetaDataInfo MetaDataInfo { get; set; }
    /// <summary>
    /// Gets or sets the page information.
    /// </summary>
    /// <value>
    /// The page information.
    /// </value>
    public PageInfo PageInfo { get; set; }
  }
}