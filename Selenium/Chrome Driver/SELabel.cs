using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

public class SELabel : SEBaseElement
{
    #region Constructors
    /// <summary>
    /// Instantiate an SELabel from an IWebElement with the tag name "label"
    /// </summary>
    public SELabel(IWebElement element) : base(element)
    {
        string tagName = element.TagName;
        if (null == tagName || !"label".Equals(tagName.ToLower()))
            throw new UnexpectedTagNameException("label", tagName);
    }
    public SELabel() : base() { }
    #endregion
}
