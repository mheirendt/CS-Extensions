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
public class BaseElement : IWebElement
{
    #region public properties
    /// <summary>
    /// Returns the plain text of the element
    /// </summary>
    public string Text
    {
        get
        {
            this.checkElement();
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
            this.checkElement();
            return this.element.GetAttribute("innerHtml");
        }
    }
    /// <summary>
    /// Returns a list of all tables within the element
    /// </summary>
    public List<Table> Tables
    {
        get
        {
            this.checkElement();
            return this.element.FindElements(By.TagName("table")).Select(x => new Table(x)).ToList();
        }
    }
    /// <summary>
    /// Returns a list of all spans located within the element
    /// </summary>
    public List<BaseElement> Spans
    {
        get
        {
            this.checkElement();
            return this.element.FindElements(By.TagName("span")).Select(x => new BaseElement(x)).ToList();
        }
    }
    /// <summary>
    /// Returns a list of all IWebElement Links within the element
    /// </summary>
    public List<BaseElement> Links
    {
        get
        {
            this.checkElement();
            return this.element.FindElements(By.TagName("a")).Select(x => new BaseElement(x)).ToList();
        }
    }
    /// <summary>
    /// Returns a list of all the urls within the element
    /// </summary>
    public List<string> Urls
    {
        get
        {
            this.checkElement();
            return this.element.FindElements(By.TagName("a")).Select(x => x.GetAttribute("href")).ToList();
        }
    }
    #endregion
    #region IWebElement Members

    public string TagName
    {
        get
        {
            this.checkElement();
            return this.element.TagName;
        }
    }

    public bool Enabled
    {
        get
        {
            this.checkElement();
            return this.element.Enabled;
        }
    }

    public bool Selected
    {
        get
        {
            this.checkElement();
            return this.element.Selected;
        }
    }

    public Point Location
    {
        get
        {
            this.checkElement();
            return this.element.Location;
        }
    }

    public Size Size
    {
        get
        {
            this.checkElement();
            return this.element.Size;
        }
    }

    public bool Displayed
    {
        get
        {
            this.checkElement();
            return this.element.Displayed;
        }
    }
    #endregion
    #region private properties
    protected internal IWebElement element { get; private set; }

    private ChromeDriver driver;
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
    public BaseElement(ChromeDriver driver)
    {
        this.driver = driver;
    }
    #endregion
    #region public methods

    public IWebElement FindElement(By selector)
    {
        this.checkElement();
        return this.element.FindElement(selector);
    }


    ReadOnlyCollection<IWebElement> ISearchContext.FindElements(By by)
    {
        this.checkElement();
        return this.element.FindElements(by);
    }


    public void SendKeys(string keys)
    {
        this.checkElement();
        this.element.SendKeys(keys);
    }


    public void Click()
    {
        this.checkElement();
        this.element.Click();
    }


    public void DoubleClick()
    {
        this.checkElement();
        this.element.Click();
        this.element.Click();
    }


    public void Clear()
    {
        this.checkElement();
        this.element.Clear();
    }


    public void Submit()
    {
        this.checkElement();
        this.element.Submit();
    }


    public string GetAttribute(string attributeName)
    {
        this.checkElement();
        return this.element.GetAttribute(attributeName);
    }


    public string GetCssValue(string propertyName)
    {
        this.checkElement();
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

    private void checkElement()
    {
        if (this.element == null && this.driver != null)
            this.element = this.awaitElement(By.TagName("body"));
        else if (this.element == null && this.driver == null)
            throw new ArgumentException("You cannot instantiate a BaseElement without a ChromeDriver, or IWebElement");
    }
    #endregion
}