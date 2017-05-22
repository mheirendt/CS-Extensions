using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tobin.EFD.Server.BusinessLogic.Websites
{
    /// <summary>
    /// Object model to be bound to table cell elements with the Selenium Remote Web Driver
    /// </summary>
    public class SETableCell : SEBaseElement
    {
        #region public properties
        /// <summary>
        /// Returns the Zero based position of the cell in the row
        /// </summary>
        public int Cell { get; private set; }
        #endregion
        #region constructors
        /// <summary>
        /// Instantiate a SETableCell from an IWebElement with the tag name "td" and sets the position of the cell
        /// </summary>
        public SETableCell(IWebElement element, int index) : base(element)
        {
            string tagName = element.TagName;
            if (null == tagName || !"td".Equals(tagName.ToLower()))
                throw new UnexpectedTagNameException("td", tagName);

            this.Cell = index;
        }

        /// <summary>
        /// Instantiate a SETableCell from an IWebElement with the tag name "td"
        /// </summary>
        public SETableCell(IWebElement element) : base(element)
        {
            string tagName = element.TagName;
            if (null == tagName || !"td".Equals(tagName.ToLower()))
                throw new UnexpectedTagNameException("td", tagName);
        }
        public SETableCell() : base() { }
        #endregion
    }
}
