
using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;
	
	/// <summary>
    /// Object model to be bound to table row elements with the Selenium Remote Web Driver
    /// </summary>
    public class TableRow : BaseElement
    {
        #region public properties
        /// <summary>
        /// Get a List<TableCell> of all the cells within the table row
        /// </summary>
        public List<TableCell> Cells
        {
            get
            {
                return this.element.FindElements(By.TagName("td")).Select(x => new TableCell(x)).ToList();
            }
        }
        #endregion
        #region constructors
        /// <summary>
        /// Instantiate a TableRow from an IWebElement with the tag name "tr"
        /// </summary>
        /// <param name="element"></param>
        public TableRow(IWebElement element) : base(element)
        {
            string tagName = element.TagName;
            if (null == tagName || !"tr".Equals(tagName.ToLower()))
                throw new UnexpectedTagNameException("tr", tagName);
        }
        #endregion
    }