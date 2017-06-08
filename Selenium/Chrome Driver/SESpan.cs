using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

public class SESpan : SEBaseElement
{
    #region constructors
    /// <summary>
    /// Instantiate a Span from an IWebElement with the tag name "span"
    /// </summary>
    public SESpan(IWebElement element) : base(element)
    {
        string tagName = element.TagName;
        if (null == tagName || !"span".Equals(tagName.ToLower()))
            throw new UnexpectedTagNameException("span", tagName);
    }
    public SESpan() : base() { }
    #endregion
}