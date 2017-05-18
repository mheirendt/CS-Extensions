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
public class P2BaseElement : IWebElement
{
    #region public properties
    /// <summary>
    /// Returns the plain text of the element
    /// </summary>
    public string Text
    {
        get
        {
            if (this.element == null && this.driver != null)
                this.element = this.awaitElement(By.TagName("body"));
            return this.element.Text;
        }
    }
    /// <summary>
    /// Returns the innerHtml of the element
    /// </summary>
    public string Html
    {
        get
        {
            if (this.element == null && this.driver != null)
                this.element = this.awaitElement(By.TagName("body"));
            return this.element.GetAttribute("innerHtml");
        }
    }
    /// <summary>
    /// Returns a list of all tables within the element
    /// </summary>
    public List<P2Table> Tables
    {
        get
        {
            if (this.element == null && this.driver != null)
                this.element = this.awaitElement(By.TagName("body"));
            return this.element.FindElements(By.TagName("table")).Select(x => new P2Table(x)).ToList();
        }
    }
    /// <summary>
    /// Returns a list of all spans located within the element
    /// </summary>
    public List<P2BaseElement> Spans
    {
        get
        {
            if (this.element == null && this.driver != null)
                this.element = this.awaitElement(By.TagName("body"));
            return this.element.FindElements(By.TagName("span")).Select(x => new P2BaseElement(x)).ToList();
        }
    }
    /// <summary>
    /// Returns a list of all IWebElement Links within the element
    /// </summary>
    public List<P2BaseElement> Links
    {
        get
        {
            if (this.element == null && this.driver != null)
                this.element = this.awaitElement(By.TagName("body"));
            return this.element.FindElements(By.TagName("a")).Select(x => new P2BaseElement(x)).ToList();
        }
    }
    /// <summary>
    /// Returns a list of all the urls within the element
    /// </summary>
    public List<string> Urls
    {
        get
        {
            if (this.element == null && this.driver != null)
                this.element = this.awaitElement(By.TagName("body"));
            return this.element.FindElements(By.TagName("a")).Select(x => x.GetAttribute("href")).ToList();
        }
    }
    #endregion
    #region IWebElement Members

    public string TagName
    {
        get
        {
            if (this.element == null && this.driver != null)
                this.element = this.awaitElement(By.TagName("body"));
            return this.element.TagName;
        }
    }

    public bool Enabled
    {
        get
        {
            if (this.element == null && this.driver != null)
                this.element = this.awaitElement(By.TagName("body"));
            return this.element.Enabled;
        }
    }

    public bool Selected
    {
        get
        {
            if (this.element == null && this.driver != null)
                this.element = this.awaitElement(By.TagName("body"));
            return this.element.Selected;
        }
    }

    public Point Location
    {
        get
        {
            if (this.element == null && this.driver != null)
                this.element = this.awaitElement(By.TagName("body"));
            return this.element.Location;
        }
    }

    public Size Size
    {
        get
        {
            if (this.element == null && this.driver != null)
                this.element = this.awaitElement(By.TagName("body"));
            return this.element.Size;
        }
    }

    public bool Displayed
    {
        get
        {
            if (this.element == null && this.driver != null)
                this.element = this.awaitElement(By.TagName("body"));
            return this.element.Displayed;
        }
    }
    #endregion
    #region private properties
    protected internal IWebElement element { get; private set; }

    private P2ChromeDriver driver;
    #endregion
    #region constructors
    /// <summary>
    /// Instantiate a P2BaseElement and bind it to the IWebElement
    /// </summary>
    /// <param name="element">
    /// The IWebElement for the class to be bound to
    /// </param>
    public P2BaseElement(IWebElement element)
    {
        this.element = element;
    }
    /// <summary>
    /// Instantiate a P2BaseElement and bind it to the body of the web page
    /// </summary>
    /// <param name="driver">
    /// The P2ChromeDriver to query web page elements
    /// </param>
    public P2BaseElement(P2ChromeDriver driver)
    {
        this.driver = driver;
    }
    #endregion
    #region public methods

    public IWebElement FindElement(By selector)
    {
        if (this.element == null && this.driver != null)
            this.element = this.awaitElement(By.TagName("body"));
        return this.element.FindElement(selector);
    }


    ReadOnlyCollection<IWebElement> ISearchContext.FindElements(By by)
    {
        if (this.element == null && this.driver != null)
            this.element = this.awaitElement(By.TagName("body"));
        return this.element.FindElements(by);
    }


    public void SendKeys(string keys)
    {
        if (this.element == null && this.driver != null)
            this.element = this.awaitElement(By.TagName("body"));
        this.element.SendKeys(keys);
    }


    public void Click()
    {
        if (this.element == null && this.driver != null)
            this.element = this.awaitElement(By.TagName("body"));
        this.element.Click();
    }


    public void DoubleClick()
    {
        if (this.element == null && this.driver != null)
            this.element = this.awaitElement(By.TagName("body"));
        this.element.Click();
        this.element.Click();
    }


    public void Clear()
    {
        if (this.element == null && this.driver != null)
            this.element = this.awaitElement(By.TagName("body"));
        this.element.Clear();
    }


    public void Submit()
    {
        if (this.element == null && this.driver != null)
            this.element = this.awaitElement(By.TagName("body"));
        this.element.Submit();
    }


    public string GetAttribute(string attributeName)
    {
        if (this.element == null && this.driver != null)
            this.element = this.awaitElement(By.TagName("body"));
        return this.element.GetAttribute(attributeName);
    }


    public string GetCssValue(string propertyName)
    {
        if (this.element == null && this.driver != null)
            this.element = this.awaitElement(By.TagName("body"));
        return this.element.GetCssValue(propertyName);
    }


    #endregion
    #region private methods
    private IWebElement awaitElement(By selector)
    {
        try
        {
            WebDriverWait wait = new WebDriverWait(this.driver, new TimeSpan(10 * TimeSpan.TicksPerSecond));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selector));
        }
        catch (WebDriverTimeoutException)
        {
            Log.Debug("Web driver timed out while waiting 10 seconds for element to become available, reloading the page and trying again");
            string url = this.driver.Url;
            this.driver.Url = "";
            this.driver.GoToUrl(url);
            WebDriverWait wait = new WebDriverWait(this.driver, new TimeSpan(30 * TimeSpan.TicksPerSecond));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selector));
        }
        return this.driver.FindElement(selector);
    }
    #endregion
}