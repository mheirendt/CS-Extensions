using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

public class SEImage : SEBaseElement
{
    #region Public Properties
    public string Alt
    {
        get
        {
            return base.element.GetAttribute("alt");
        }
    }

    public string Src
    {
        get
        {
            return base.element.GetAttribute("src");
        }
    }
    #endregion
    #region Constructors
    /// <summary>
    /// Instantiate an Image from an IWebElement with the tag name "img"
    /// </summary>
    public SEImage(IWebElement element) : base(element)
    {
        if (element != null)
        {
            string tagName = element.TagName;
            if (null == tagName || !"img".Equals(tagName.ToLower()))
                throw new UnexpectedTagNameException("img", tagName);
        }
    }
    #endregion
}
