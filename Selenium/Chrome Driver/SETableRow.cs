using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tobin.EFD.Server.BusinessLogic.Websites
{
    /// <summary>
    /// Object model to be bound to table row elements with the Selenium Remote Web Driver
    /// </summary>
    public class SETableRow : SEBaseElement
    {
        #region public properties
        /// <summary>
        /// Get a List<SETableCell> of all the cells within the table row
        /// </summary>
        public List<SETableCell> Cells
        {
            get
            {
                return this.element.FindElements(By.TagName("td")).Select((x, i) => new SETableCell(x, i)).ToList();
            }
        }
        /// <summary>
        /// Returns the Zero based position of the row in the table
        /// </summary>
        public int Row { get; private set; }
        #endregion
        #region constructors
        /// <summary>
        /// Instantiate a SETableRow from an IWebElement with the tag name "tr" and sets the position property
        /// </summary>
        /// <param name="element"></param>
        public SETableRow(IWebElement element, int index) : base(element)
        {
            string tagName = element.TagName;
            if (null == tagName || !"tr".Equals(tagName.ToLower()))
                throw new UnexpectedTagNameException("tr", tagName);

            this.Row = index;
        }
        /// <summary>
        /// Instantiate a SETableRow from an IWebElement with the tag name "tr"
        /// </summary>
        /// <param name="element"></param>
        public SETableRow(IWebElement element) : base(element)
        {
            string tagName = element.TagName;
            if (null == tagName || !"tr".Equals(tagName.ToLower()))
                throw new UnexpectedTagNameException("tr", tagName);
        }
        #endregion
    }
}
