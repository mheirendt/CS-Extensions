using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;

public class SEChromeDriver : ChromeDriver
{
    #region Public Properties
    /// <summary>
    /// Contains a reference to the base element of the web page for ease of access
    /// </summary>
    public SEBaseElement Document { get; private set; }
    #endregion
    #region Private Properties
    private string mainWindowHandle;
    #endregion
    #region Constructors
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
    #region Public Methods

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
            this.ReloadPage(By.TagName("body"));
        }
    }

    /// <summary>
    /// Refresh the web page and wait for specifed elements to be available
    /// </summary>
    /// <param name="selector"></param>
    public virtual void ReloadPage(By selector)
    {
        string url = this.Url;
        this.Url = "https://google.com";
        this.Url = url;
        WebDriverWait wait = new WebDriverWait(this, new TimeSpan(30 * TimeSpan.TicksPerSecond));
        wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(selector));
        this.reloadDocument();
    }
    /// <summary>
    /// Switch driver context to an iFrame by id
    /// </summary>
    /// <param name="id"></param>
    public virtual void SetFrame(string id)
    {
        this.SwitchTo().Frame(id);
        this.reloadDocument();
    }
    /// <summary>
    /// Switch driver context from an iFrame back to main frame
    /// </summary>
    public virtual void BackToMainFrame()
    {
        this.SwitchTo().DefaultContent();
        this.reloadDocument();
    }

    /// <summary>
    /// Switch driver context to the tab that starts with the specified url
    /// </summary>
    /// <param name="urlStartsWith"></param>
    public virtual void SwitchTabs(string urlStartsWith)
    {
        if (this.mainWindowHandle == null)
            this.mainWindowHandle = this.CurrentWindowHandle;

        foreach (string windowHandle in this.WindowHandles)
        {
            this.SwitchTo().Window(windowHandle);
            if (this.Url.StartsWith(urlStartsWith))
            {
                this.reloadDocument();
                break;
            }
        }
    }

    /// <summary>
    /// Switch driver context back to the main tab if you have already switched tabs
    /// </summary>
    public virtual void SwitchBackToMainTab()
    {
        if (this.mainWindowHandle == null)
            throw new NullReferenceException("You cannot switch back to the main tab from the main tab.");

        this.SwitchTo().Window(this.mainWindowHandle);
        this.reloadDocument();
    }

    /// <summary>
    /// Navigate the web page backwards
    /// </summary>
    public virtual void Back()
    {
        this.Navigate().Back();
        this.reloadDocument();
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
            this.ReloadPage(selector);
        }
        return new SEBaseElement(this.FindElement(selector));
    }


    /// <summary>
    /// Create an explicit wait for the element to become available and instantiate the time of element desired
    /// </summary>
    /// <typeparam name="SEType"></typeparam>
    /// <param name="selector">
    /// A Selenium By selector to query web elements from the page
    /// </param>
    /// <returns></returns>
    public virtual SEType AwaitElement<SEType>(By selector) where SEType : new()
    {
        try
        {
            WebDriverWait wait = new WebDriverWait(this, new TimeSpan(10 * TimeSpan.TicksPerSecond));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selector));
        }
        catch (WebDriverTimeoutException)
        {
            this.ReloadPage(selector);
        }
        return (SEType)Activator.CreateInstance(typeof(SEType), this.FindElement(selector));
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
            this.ReloadPage(selector);
        }
        return this.FindElements(selector).Select(x => new SEBaseElement(x)).ToList();
    }


    /// <summary>
    /// Create an explicit wait for the elements to become available
    /// </summary>
    /// <typeparam name="SEType"></typeparam>
    /// <param name="selector">
    /// A Selenium By selector to query web elements from the page
    /// </param>
    /// <returns></returns>
    public virtual List<SEType> AwaitElements<SEType>(By selector) where SEType : new()
    {
        try
        {
            WebDriverWait wait = new WebDriverWait(this, new TimeSpan(10 * TimeSpan.TicksPerSecond));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selector));
        }
        catch (WebDriverTimeoutException)
        {
            this.ReloadPage(selector);
        }
        return this.FindElements(selector).Select(x => (SEType)Activator.CreateInstance(typeof(SEType), x)).ToList();
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
    #endregion
    #region Private Methods
    private void reloadDocument()
    {
        this.Document = new SEBaseElement(this);
    }
    #endregion
}