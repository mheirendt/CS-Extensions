using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;
	
	/// <summary>
    /// Object model to be bound to table cell elements with the Selenium Remote Web Driver
    /// </summary>
    public class TableCell : BaseElement
    {
        #region constructors
        /// <summary>
        /// Instantiate a TableCell from an IWebElement with the tag name "td"
        /// </summary>
        public TableCell(IWebElement element) : base(element)
        {
            string tagName = element.TagName;
            if (null == tagName || !"td".Equals(tagName.ToLower()))
                throw new UnexpectedTagNameException("td", tagName);
        }
        #endregion
    }