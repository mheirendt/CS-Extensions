using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;

    class ExtendedChromeOptions : ChromeOptions
    {
        #region constructors
        public ExtendedChromeOptions() : base()
        {
            // The chrome driver instantiates a new user profile for each session. To auto
            // download PDF images, you must manually set the user preference to true

            // This was a PAIN. If this ever does not work, here is how I got the preference name:
            // 1. Navigate to : chrome://settings/content
            // 2. Scroll to the bottom "PDF Documents" section
            // 3. Right-Click and inspect element on the check box titled "Open PDF files in the default PDF viewer application"
            // 4. The preference name is the pref key for the input, in this case: pref="plugins.always_open_pdf_externally"
            this.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
        }
        #endregion
    }
    public class ExtendedChromeDriver : ChromeDriver
    {
        #region public properties
        /// <summary>
        /// Contains a reference to the base element of the web page for ease of access
        /// </summary>
        public BaseElement Document { get; private set; }
        /// <summary>
        /// Returns a List<IWebElement> of all tables on the web page
        /// </summary>
        public List<Table> Tables
        {
            get
            {
                return this.AwaitElements(By.TagName("table")).Select(x => new Table(x)).ToList();
            }
        }
        #endregion
        #region constructors
        /// <summary>
        /// Instantiate an ExtendedChromeDriver with an implicit wait and always_open_pdf_externally plugin enabled
        /// </summary>
        public ExtendedChromeDriver() : base(ChromeDriverService.CreateDefaultService(), new ExtendedChromeOptions(), RemoteWebDriver.DefaultCommandTimeout)
        {
            // The constructor must be pointed to the chrome driver .exe
            // Or you must set a PATH variable pointing to the location
            // For ExtendedChromeDriver, we rely on the PATH variable at C://program files//Chrome

            // Create an implicit wait for all requests to the web driver
            this.Manage().Timeouts().ImplicitWait = new TimeSpan(1 * TimeSpan.TicksPerSecond);

            // For the driver's document, we pass in the driver instead of an IWebElement. This is because
            // in most cases, the dom is manipulated after the page loads, which leads to stale reference exceptions.
            // By passing in the driver, we can locate the body element dynamically when necessary, and reset as necessary.
            this.Document = new BaseElement(this);
        }


        /// <summary>
        /// Instantiate an ExtendedChromeDriver with an implicit wait and always_open_pdf_externally plugin enabled and navigate to the specified url
        /// </summary>
        public ExtendedChromeDriver(string url) : base(ChromeDriverService.CreateDefaultService(), new ExtendedChromeOptions(), RemoteWebDriver.DefaultCommandTimeout)
        {
            // The constructor must be pointed to the chrome driver .exe
            // Or you must set a PATH variable pointing to the location
            // For ExtendedChromeDriver, we rely on the PATH variable at C://program files//Chrome

            // Create an implicit wait for all requests to the web driver
            this.Manage().Timeouts().ImplicitWait = new TimeSpan(1 * TimeSpan.TicksPerSecond);
            this.GoToUrl(url);

            // For the driver's document, we pass in the driver instead of an IWebElement. This is because
            // in most cases, the dom is manipulated after the page loads, which leads to stale reference exceptions.
            // By passing in the driver, we can locate the body element dynamically when necessary, and reset as necessary.
            this.Document = new BaseElement(this);
        }
        #endregion
        #region public methods
        /// <summary>
        /// Navigate to the url and wait for all <div> tags to become present
        /// </summary>
        /// <param name="url">
        /// The url to be navigated to upon instantiation
        /// </param>
        public virtual void GoToUrl(string url)
        {
            try
            {
                this.Url = url;
                WebDriverWait wait = new WebDriverWait(this, new TimeSpan(30 * TimeSpan.TicksPerSecond));
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.TagName("body")));
                this.reloadDocument();
            }
            catch (WebDriverTimeoutException)
            {
                this.Url = "";
                this.Url = url;
                WebDriverWait wait = new WebDriverWait(this, new TimeSpan(30 * TimeSpan.TicksPerSecond));
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.TagName("body")));
            }
        }


        /// <summary>
        /// Create an explicit wait for the element to become available
        /// </summary>
        /// <param name="selector">
        /// A Selenium By selector to query web elements from the page
        /// </param>
        /// <returns></returns>
        public virtual IWebElement AwaitElement(By selector)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(this, new TimeSpan(10 * TimeSpan.TicksPerSecond));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selector));
            }
            catch (WebDriverTimeoutException)
            {
                string url = this.Url;
                this.Url = "";
                this.GoToUrl(url);
                WebDriverWait wait = new WebDriverWait(this, new TimeSpan(30 * TimeSpan.TicksPerSecond));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selector));
            }
            return this.FindElement(selector);
        }


        /// <summary>
        /// Create an explicit wait for the elements to become available
        /// </summary>
        /// <param name="selector">
        /// A Selenium By selector to query web elements from the page
        /// </param>
        /// <returns></returns>
        public virtual IReadOnlyCollection<IWebElement> AwaitElements(By selector)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(this, new TimeSpan(10 * TimeSpan.TicksPerSecond));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selector));
            }
            catch (WebDriverTimeoutException)
            {
                string url = this.Url;
                this.Url = "";
                this.GoToUrl(url);
                WebDriverWait wait = new WebDriverWait(this, new TimeSpan(30 * TimeSpan.TicksPerSecond));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selector));
            }
            return this.FindElements(selector);
        }


        /// <summary>
        /// Create an explicit wait for the element to become clickable and click it
        /// </summary>
        /// <param name="selector">
        /// A Selenium By selector to query web elements from the page
        /// </param>
        /// <returns>
        /// True if element is clicked, false if it is not
        /// </returns>
        public virtual bool AwaitClick(By selector)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(this, new TimeSpan(20 * TimeSpan.TicksPerSecond));
                wait.Until(ExpectedConditions.ElementToBeClickable(selector));
                this.FindElement(selector).Click();
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Create an explicit wait for the element to become clickable and click it
        /// </summary>
        /// <param name="element">
        /// An IWebElement to be awaited
        /// </param>
        /// <returns>
        /// True if element is clicked, false if it is not
        /// </returns>
        public virtual bool AwaitClick(IWebElement element)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(this, new TimeSpan(20 * TimeSpan.TicksPerSecond));
                wait.Until(ExpectedConditions.ElementToBeClickable(element));
                element.Click();
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Check to see if By. selector returns an element with the specified text
        /// </summary>
        /// <param name="selector">
        /// A Selenium By selector to query web elements from the page
        /// </param>
        /// <param name="text">
        /// The text to be searched for
        /// </param>
        /// <returns>
        /// Returns true if the element contains the desired text
        /// </returns>
        public virtual bool ElementContainsText(By selector, string text)
        {
            return this.AwaitElement(selector).Text.Contains(text);
        }

        /// <summary>
        /// Check to see if an IWebElement contains the specified text
        /// </summary>
        /// <param name="element">
        /// A Selenium By selector to query web elements from the page
        /// </param>
        /// <param name="text">
        /// The text to be searched for
        /// </param>
        /// <returns>
        /// Returns true if the element contains the desired text
        /// </returns>
        public virtual bool ElementContainsText(IWebElement element, string text)
        {
            return element.Text.Contains(text);
        }


        /// <summary>
        /// Dispose of the ExtendedChromeDriver instance and close the window
        /// </summary>
        public virtual void DisposeDriver()
        {
            try
            {
                this.Close();
                this.Dispose();
            }
            catch { }
        }
        #endregion
        #region private methods
        private void reloadDocument()
        {
            this.Document = new BaseElement(this);
        }
        #endregion
    }




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
