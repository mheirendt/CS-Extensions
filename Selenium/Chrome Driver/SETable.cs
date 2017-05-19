using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tobin.EFD.Server.BusinessLogic.Websites
{
    /// <summary>
    /// Object model to be bound to table elements with the Selenium Remote Web Driver
    /// </summary>
    public class SETable : SEBaseElement
    {
        #region public properties
        /// <summary>
        /// Returns a list of all the rows within the table
        /// </summary>
        public List<SETableRow> Rows
        {
            get
            {
                return this.element.FindElements(By.TagName("tr")).Select(x => new SETableRow(x)).ToList();
            }
        }
        #endregion
        #region constructors
        /// <summary>
        /// Instantiate a new Table from an IWebElement with the tag name "table"
        /// </summary>
        /// <param name="element"></param>
        public SETable(IWebElement element) : base(element)
        {
            string tagName = element.TagName;
            if (null == tagName || !"table".Equals(tagName.ToLower()))
                throw new UnexpectedTagNameException("table", tagName);
        }
        #endregion
    }
}
