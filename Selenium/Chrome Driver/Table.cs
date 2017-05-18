using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;

/// <summary>
/// Object model to be bound to table elements with the Selenium Remote Web Driver
/// </summary>
public class Table : BaseElement
    {
        #region public properties
        /// <summary>
        /// Get a List<TableRows> of all the rows within the table
        /// </summary>
        public List<TableRow> Rows
        {
            get
            {
                return this.element.FindElements(By.TagName("tr")).Select(x => new TableRow(x)).ToList();
            }
        }
        #endregion
        #region constructors
        /// <summary>
        /// Instantiate a new Table from an IWebElement with the tag name "table"
        /// </summary>
        /// <param name="element"></param>
        public Table(IWebElement element) : base(element)
        {
            string tagName = element.TagName;
            if (null == tagName || !"table".Equals(tagName.ToLower()))
                throw new UnexpectedTagNameException("table", tagName);
        }
        #endregion
    }