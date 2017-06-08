using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

/// <summary>
/// Base class for all page elements with basic properties
/// </summary>
public class SEBaseElement : IWebElement
{
    #region Public Properties
    public SEBaseElement Parent
    {
        get
        {
            if (this.checkElement())
                return new SEBaseElement(this.element.FindElement(By.XPath("..")));
            return null;
        }
    }
    /// <summary>
    /// Returns the plain text of the element
    /// </summary>
    public string Text
    {
        get
        {
            if (this.checkElement())
                return this.element.Text;
            return null;
        }
    }
    /// <summary>
    /// Returns the innerHTML of the element
    /// </summary>
    public string Html
    {
        get
        {
            if (this.checkElement())
                return this.element.GetAttribute("innerHTML");
            return null;
        }
    }
    public string Id
    {
        get
        {
            if (this.checkElement())
                return this.element.GetAttribute("id");
            return null;
        }
    }
    public string Title
    {
        get
        {
            if (this.checkElement())
                return this.element.GetAttribute("title");
            return null;
        }
    }
    /// <summary>
    /// Returns a list of all tables within the element
    /// </summary>
    public SECollection<SETable> Tables
    {
        get
        {
            if (this.checkElement())
                return new SECollection<SETable>(this.element.FindElements(By.TagName("table")).Select(x => new SETable(x)));
            return null;
        }
    }
    /// <summary>
    /// Returns a list of all spans located within the element
    /// </summary>
    public SECollection<SESpan> Spans
    {
        get
        {
            if (this.checkElement())
                return new SECollection<SESpan>(this.element.FindElements(By.TagName("span")).Select(x => new SESpan(x)));
            return null;
        }
    }
    /// <summary>
    /// Returns a list of all IWebElement Links within the element
    /// </summary>
    public SECollection<SELink> Links
    {
        get
        {
            if (this.checkElement())
                return new SECollection<SELink>(this.element.FindElements(By.TagName("a")).Select(x => new SELink(x)));
            return null;
        }
    }
    /// <summary>
    /// Returns a list of all Images within the element
    /// </summary>
    public SECollection<SEImage> Images
    {
        get
        {
            if (this.checkElement())
                return new SECollection<SEImage>(this.element.FindElements(By.TagName("img")).Select(x => new SEImage(x)));
            return null;
        }
    }
    /// <summary>
    /// Returns a list of all the div tags within the element
    /// </summary>
    public SECollection<SEDiv> Divs
    {
        get
        {
            if (this.checkElement())
                return new SECollection<SEDiv>(this.element.FindElements(By.TagName("div")).Select(x => new SEDiv(x)));
            return null;
        }
    }
    /// <summary>
    /// Returns a list of all the input tags within the element
    /// </summary>
    public SECollection<SEInput> Inputs
    {
        get
        {
            if (this.checkElement())
                return new SECollection<SEInput>(this.element.FindElements(By.TagName("input")).Select(x => new SEInput(x)));
            return null;
        }
    }
    /// <summary>
    /// Returns a list of all the label tags within the element
    /// </summary>
    public SECollection<SELabel> Labels
    {
        get
        {
            if (this.checkElement())
                return new SECollection<SELabel>(this.element.FindElements(By.TagName("label")).Select(x => new SELabel(x)));
            return null;
        }
    }
    /// <summary>
    /// Returns a list of all table rows within the element
    /// </summary>
    public SECollection<SETableRow> TableRows
    {
        get
        {
            if (this.checkElement())
                return new SECollection<SETableRow>(this.element.FindElements(By.TagName("tr")).Select((x, i) => new SETableRow(x, i)));
            return null;
        }
    }
    /// <summary>
    /// Returns a list of all table cells within the element
    /// </summary>
    public SECollection<SETableCell> TableCells
    {
        get
        {
            if (this.checkElement())
                return new SECollection<SETableCell>(this.element.FindElements(By.TagName("td")).Select((x, i) => new SETableCell(x, i)));
            return null;
        }
    }
    public bool Exists
    {
        get
        {
            return this.element != null;
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
    #region Private Properties
    protected internal IWebElement element { get; private set; }
    private SEChromeDriver driver;
    #endregion
    #region Constructors
    /// <summary>
    /// Instantiate an SEBaseElement and bind it to the IWebElement
    /// </summary>
    /// <param name="element">
    /// The IWebElement for the class to be bound to
    /// </param>
    public SEBaseElement(IWebElement element)
    {
        this.element = element;
    }
    /// <summary>
    /// Instantiate an SEBaseElement and bind it to the body of the web page
    /// </summary>
    /// <param name="driver">
    /// The SEChromeDriver to query web page elements
    /// </param>
    public SEBaseElement(SEChromeDriver driver)
    {
        this.driver = driver;
    }
    public SEBaseElement() { }
    #endregion
    #region Public Methods

    public IWebElement FindElement(By selector)
    {
        this.checkElement();
        return this.element.FindElement(selector);
    }

    public SEBaseElement FindElement(string text)
    {
        this.checkElement();
        return new SEBaseElement(this.element.FindElement(By.XPath("//*[contains(text(), '" + text + "')]")));
    }

    public SEBaseElement FindElement(Regex regex)
    {
        this.checkElement();
        return new SEBaseElement(this.element.FindElement(By.XPath("//*[contains(text(), '" + regex + "')]")));
    }


    ReadOnlyCollection<IWebElement> ISearchContext.FindElements(By by)
    {
        this.checkElement();
        return this.element.FindElements(by);
    }

    public SECollection<T> Ancestors<T>()
    {
        this.checkElement();
        IWebElement elem = this.element;
        List<T> items = new List<T>();
        bool moreAncestors = true;
        while (moreAncestors == true)
        {
            elem = elem.FindElement(By.XPath(".."));
            try
            {
                items.Add((T)Activator.CreateInstance(typeof(T), elem));
            }
            catch { }
            if (elem.TagName == "html")
                moreAncestors = false;
        }

        return new SECollection<T>(items);
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
        return this.element?.GetAttribute(attributeName);
    }


    public string GetCssValue(string propertyName)
    {
        this.checkElement();
        return this.element?.GetCssValue(propertyName);
    }
    #endregion
    #region Overrides
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        IEnumerable<PropertyInfo> pis = typeof(SEBaseElement).GetRuntimeProperties();
        foreach (PropertyInfo pi in pis)
        {
            if (pi.GetValue(this).GetType() == typeof(string))
                sb.AppendLine(pi.Name + ": " + pi.GetValue(this));
            else if (pi.GetValue(this).GetType() == typeof(List<SEBaseElement>))
            {
                var item = (List<SEBaseElement>)pi.GetValue(this);
                sb.AppendLine(pi.Name + " count: " + item.Count);
            }
        }
        return sb.ToString();
    }
    #endregion
    #region Private Methods
    private IWebElement awaitElement(By selector)
    {
        try
        {
            WebDriverWait wait = new WebDriverWait(this.driver, new TimeSpan(10 * TimeSpan.TicksPerSecond));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selector));
        }
        catch (WebDriverTimeoutException)
        {
            this.driver.ReloadPage(selector);
        }
        return this.driver.FindElement(selector);
    }

    private bool checkElement()
    {
        if (this.element == null && this.driver == null)
            return false;

        int limit = 0;
        while ((isStale() || this.element.GetAttribute("innerHTML").Contains("<h1>Proxy Error</h1>")) && limit < 5)
        {
            if (this.isStale())
                this.element = null;

            if (this.element == null && this.driver != null)
            {
                WebDriverWait wait = new WebDriverWait(this.driver, new TimeSpan(10 * TimeSpan.TicksPerSecond));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.TagName("body")));
                this.element = this.awaitElement(By.TagName("body"));
            }

            if (this.element != null && this.element.GetAttribute("innerHTML").Contains("<h1>Proxy Error</h1>") && this.driver != null)
                this.driver.ReloadPage(By.TagName("body"));

            ++limit;
        }

        if ((isStale() || this.element.GetAttribute("innerHTML").Contains("<h1>Proxy Error</h1>")))
            return false;

        return true;
    }
    private bool isStale()
    {
        return ExpectedConditions.StalenessOf(this.element)(this.driver);
    }
    #endregion
}
