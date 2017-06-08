using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

public class SELink : SEBaseElement
{
    #region Public Properties
    /// <summary>
    /// Returns the url of the link
    /// </summary>
    public string Url
    {
        get
        {
            return this.element.GetAttribute("href");
        }
    }
    #endregion
    #region Constructors
    /// <summary>
    /// Instantiate an SELink from an IWebElement with the tag name "a"
    /// </summary>
    /// <param name="element"></param>
    public SELink(IWebElement element) : base(element)
    {
        string tagName = element.TagName;
        if (null == tagName || !"a".Equals(tagName.ToLower()))
            throw new UnexpectedTagNameException("a", tagName);
    }
    public SELink() : base() { }
    #endregion
}
