using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tobin.EFD.Server.BusinessLogic.Websites
{
    public class SELink : SEBaseElement
    {
        #region public properties
        public string Url
        {
            get
            {
                return this.element.GetAttribute("href");
            }
        }
        #endregion
        #region constructors
        /// <summary>
        /// Instantiate a Link from an IWebElement with the tag name "link"
        /// </summary>
        /// <param name="element"></param>
        public SELink(IWebElement element) : base(element)
        {
            string tagName = element.TagName;
            if (null == tagName || !"input".Equals(tagName.ToLower()))
                throw new UnexpectedTagNameException("input", tagName);
        }
        #endregion
    }
}
