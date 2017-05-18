using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;    
	
	/// <summary>
    /// Base class for all page elements with basic properties
    /// </summary>
    public class BaseElement
    {
        #region public properties
        /// <summary>
        /// Get the plain text of the element
        /// </summary>
        public string Text
        {
            get
            {
                if (this.element == null && this.driver != null)
                    this.element = this.driver.AwaitElement(By.TagName("body"));
                return this.element.Text;
            }
        }
        /// <summary>
        /// Get the innerHtml from the table cell
        /// </summary>
        public string Html
        {
            get
            {
                if (this.element == null && this.driver != null)
                    this.element = this.driver.AwaitElement(By.TagName("body"));
                return this.element.GetAttribute("innerHtml");
            }
        }
        /// <summary>
        /// Get a List<IWebElement> of all clickable links in the table cell
        /// </summary>
        public List<IWebElement> Links
        {
            get
            {
                if (this.element == null && this.driver != null)
                    this.element = this.driver.AwaitElement(By.TagName("body"));
                return this.element.FindElements(By.TagName("a")).ToList();
            }
        }
        /// <summary>
        /// Get a List<string> of all urls in the table cell
        /// </summary>
        public List<string> Urls
        {
            get
            {
                if (this.element == null && this.driver != null)
                    this.element = this.driver.AwaitElement(By.TagName("body"));
                return this.element.FindElements(By.TagName("a")).Select(x => x.GetAttribute("href")).ToList();
            }
        }
        #endregion
        #region private properties
        protected internal IWebElement element { get; private set; }
        private ExtendedChromeDriver driver;
        #endregion
        #region constructors
        /// <summary>
        /// Instantiate a BaseElement and bind it to the IWebElement
        /// </summary>
        /// <param name="element">
        /// The IWebElement for the class to be bound to
        /// </param>
        public BaseElement(IWebElement element)
        {
            this.element = element;
        }
        /// <summary>
        /// Instantiate a BaseElement and bind it to the body of the web page
        /// </summary>
        /// <param name="driver">
        /// The ChromeDriver to query web page elements
        /// </param>
        public BaseElement(ExtendedChromeDriver driver)
        {
            this.driver = driver;
        }
        #endregion
	}
