using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tobin.EFD.Server.BusinessLogic.Websites
{
    public class SEDiv : SEBaseElement
    {
        #region constructors
        /// <summary>
        /// Instantiate a Div from an IWebElement with the tag name "div"
        /// </summary>
        public SEDiv(IWebElement element) : base(element)
        {
            string tagName = element.TagName;
            if (null == tagName || !"div".Equals(tagName.ToLower()))
                throw new UnexpectedTagNameException("div", tagName);
        }
        #endregion
    }
}
