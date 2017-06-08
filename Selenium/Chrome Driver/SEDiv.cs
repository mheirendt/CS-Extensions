using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

public class SEDiv : SEBaseElement
{
    #region Constructors
    /// <summary>
    /// Instantiate a Div from an IWebElement with the tag name "div"
    /// </summary>
    public SEDiv(IWebElement element) : base(element)
    {
        string tagName = element.TagName;
        if (null == tagName || !"div".Equals(tagName.ToLower()))
            throw new UnexpectedTagNameException("div", tagName);
    }
    public SEDiv() : base() { }
    #endregion
}
