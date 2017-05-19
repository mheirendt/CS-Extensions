using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;
using Tobin.EFD.Common;

namespace Tobin.EFD.Server.BusinessLogic.Websites
{
    public class SEChromeDriver : ChromeDriver
    {
        #region public properties
        /// <summary>
        /// Contains a reference to the base element of the web page for ease of access
        /// </summary>
        public SEBaseElement Document { get; private set; }
        #endregion
        #region constructors
        /// <summary>
        /// Instantiate a SEChromeDriver with an implicit wait and always_open_pdf_externally plugin enabled
        /// </summary>
        public SEChromeDriver() : base(ChromeDriverService.CreateDefaultService(), new SEChromeOptions(), RemoteWebDriver.DefaultCommandTimeout)
        {
            // The constructor must be pointed to the chrome driver .exe
            // Or you must set a PATH variable pointing to the location
            // For SEChromeDriver, we rely on the PATH variable at C://program files//Chrome

            // Create an implicit wait for all requests to the web driver
            this.Manage().Timeouts().ImplicitWait = new TimeSpan(1 * TimeSpan.TicksPerSecond);

            // For the driver's document, we pass in the driver instead of an IWebElement. This is because
            // in most cases, the dom is manipulated after the page loads, which leads to stale reference exceptions.
            // By passing in the driver, we can locate the body element dynamically when necessary, and reset as necessary.
            this.Document = new SEBaseElement(this);
        }


        /// <summary>
        /// Instantiate a SEChromeDriver with an implicit wait and always_open_pdf_externally plugin enabled and navigate to the specified url
        /// </summary>
        public SEChromeDriver(string url) : base(ChromeDriverService.CreateDefaultService(), new SEChromeOptions(), RemoteWebDriver.DefaultCommandTimeout)
        {
            // The constructor must be pointed to the chrome driver .exe
            // Or you must set a PATH variable pointing to the location
            // For SEChromeDriver, we rely on the PATH variable at C://program files//Chrome

            // Create an implicit wait for all requests to the web driver
            this.Manage().Timeouts().ImplicitWait = new TimeSpan(1 * TimeSpan.TicksPerSecond);
            this.GoToUrl(url);

            // For the driver's document, we pass in the driver instead of an IWebElement. This is because
            // in most cases, the dom is manipulated after the page loads, which leads to stale reference exceptions.
            // By passing in the driver, we can locate the body element dynamically when necessary, and reset as necessary.
            this.Document = new SEBaseElement(this);
        }
        #endregion
        #region public methods

        /// <summary>
        /// Instantiate a webpage inheriting from BasePage and bind the driver to the base class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Page<T>() where T : new()
        {
            if (typeof(T).BaseType != typeof(BasePage))
                throw new ArgumentException("The object being initialized must inherit from the class BasePage");

            T page = new T();
            BasePage basePage = page as BasePage;
            basePage.SetDriver(this);
            return page;
        }


        /// <summary>
        /// Instantiate a webpage inheriting from BasePage and bind the driver to the base class, and navigate to the given url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Page<T>(string url) where T : new()
        {
            if (typeof(T).BaseType != typeof(BasePage))
                throw new ArgumentException("The object being initialized must inherit from the class BasePage");

            T page = new T();
            var basePage = page as BasePage;
            basePage.SetDriver(this);
            this.GoToUrl(url);
            return page;
        }


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
                Log.Debug("Web driver timed out while waiting 30 seconds for the web page to load, refreshing page and trying again");
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
        public virtual SEBaseElement AwaitElement(By selector)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(this, new TimeSpan(10 * TimeSpan.TicksPerSecond));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selector));
            }
            catch (WebDriverTimeoutException)
            {
                Log.Debug("Web driver timed out while waiting 10 seconds for element to become available, reloading the page and trying again");
                string url = this.Url;
                this.Url = "";
                this.GoToUrl(url);
                WebDriverWait wait = new WebDriverWait(this, new TimeSpan(30 * TimeSpan.TicksPerSecond));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selector));
            }
            return new SEBaseElement(this.FindElement(selector));
        }


        /// <summary>
        /// Create an explicit wait for the elements to become available
        /// </summary>
        /// <param name="selector">
        /// A Selenium By selector to query web elements from the page
        /// </param>
        /// <returns></returns>
        public virtual List<SEBaseElement> AwaitElements(By selector)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(this, new TimeSpan(10 * TimeSpan.TicksPerSecond));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selector));
            }
            catch (WebDriverTimeoutException)
            {
                Log.Debug("Web driver timed out while waiting 10 seconds for elements to become available, reloading the page and trying again");
                string url = this.Url;
                this.Url = "";
                this.GoToUrl(url);
                WebDriverWait wait = new WebDriverWait(this, new TimeSpan(30 * TimeSpan.TicksPerSecond));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selector));
            }
            return this.FindElements(selector).Select(x => new SEBaseElement(x)).ToList();
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
                WebDriverWait elementWait = new WebDriverWait(this, new TimeSpan(20 * TimeSpan.TicksPerSecond));
                WebDriverWait clickWait = new WebDriverWait(this, new TimeSpan(20 * TimeSpan.TicksPerSecond));
                elementWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selector));
                clickWait.Until(ExpectedConditions.ElementToBeClickable(selector));
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
        public virtual bool AwaitClick(SEBaseElement element)
        {
            try
            {
                WebDriverWait clickWait = new WebDriverWait(this, new TimeSpan(20 * TimeSpan.TicksPerSecond));
                clickWait.Until(ExpectedConditions.ElementToBeClickable(element));
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
        /// Dispose of the SEChromeDriver instance and close the window
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
            this.Document = new SEBaseElement(this);
        }
        #endregion
    }
}
