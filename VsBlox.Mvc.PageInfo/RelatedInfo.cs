namespace VsBlox.Mvc.PageInfo
{
  /// <summary>
  ///  Defines related (internal and external) url's with the page.
  /// </summary>
  public class RelatedInfo
  {
    /// <summary>
    /// Specifies the title of the external url.
    /// </summary>
    /// <value>
    /// Title.
    /// </value>
    public string Title { get; private set; }
    /// <summary>
    /// Specifies the external url.
    /// </summary>
    /// <value>
    /// Url.
    /// </value>
    public string Url { get; private set; }
    /// <summary>
    /// Specifies the associated Action.
    /// </summary>
    /// <value>
    /// Action.
    /// </value>
    public string Action { get; private set; }
    /// <summary>
    /// Specifies the associated controller.
    /// </summary>
    /// <value>
    /// Controller.
    /// </value>
    public string Controller { get; private set; }
    /// <summary>
    /// Specifies the associated area.
    /// </summary>
    /// <value>
    /// Area.
    /// </value>
    public string Area { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelatedInfo" /> class.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="action">The action.</param>
    /// <param name="controller">The controller.</param>
    /// <param name="area">The area.</param>
    public RelatedInfo(string title, string action, string controller, string area)
    {
      Title = title;
      Action = action;
      Controller = controller;
      Area = area;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelatedInfo"/> class.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="url">The URL.</param>
    public RelatedInfo(string title, string url)
    {
      Title = title;
      Url = url;
    }
  }
}