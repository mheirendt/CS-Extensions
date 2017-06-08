using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

public class SEInput : SEBaseElement
{
    #region Constructors
    /// <summary>
    /// Instantiate an Input from an IWebElement with the tag name "input"
    /// </summary>
    public SEInput(IWebElement element) : base(element)
    {
        string tagName = element.TagName;
        if (null == tagName || !"input".Equals(tagName.ToLower()))
            throw new UnexpectedTagNameException("input", tagName);
    }
    public SEInput() : base() { }
    #endregion
}