<b>Subclasses and Helper methods for Selenium Remote Web Driver</b>

* <i>ExtendedChromeDriver.cs</i>
  Simplifies the code you need to write to be able to test or scrape web pages.
  With the Selenium remote web driver, in order to iterate through the rows of each HTML table,
  you would need to do something like this:
  ```CS
  using (ChromeDriver driver = new ChromeDriver())
  {
    IReadOnlyCollection<IWebElement> tableRows = driver.FindElement(By.TagName("table))
      .FindElements(By.TagName("tr"));
    foreach (IWebElement row in tableRows)
    {
      IReadOnlyCollection<IWebElement> tableCells = row.FindElements(By.TagName("td"));
    }
  }
  ```
  
  This subclass simplifies this workflow by 10 fold. Here is how you would accomplish the same thing:
  ```CS
  using (ExtendedChromeDriver driver = new ExtendedChromeDriver())
  {
    Table table = driver.Document.Tables[0];
    List<TableRow> tableRows = table.Rows;
    List<TableCells> tableCells = tableRows.Cells;
    string text = table.Rows[0].Cells[0].Text;
    string html = table.Rows[0].Cells[0].Html;
  }
  ```
  I also noticed an annoying issue with Selenium is that there is no helper method for waiting for the 
  page, or an element to load. Here is what you would have to do to wait for an element to appear in the web page:
  ```CS
  using (ChromeDriver driver = new ChromeDriver())
  {
    WebDriverWait wait = new WebDriverWait(this, new TimeSpan(30 * TimeSpan.TicksPerSecond));
    wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.TagName("table")));
    List<Table> table = driver.FindElements(By.TagName("table")).ToList();
  }
  ```
  
  Here is how you would accomplish the same thing with the subclass:
  ```CS
  using (ExtendedChromeDriver driver = new ExtendedChromeDriver)
  {
    List<Table> table = driver.Tables;
  }
  ```
  
  OR
  
  ```CS
  using (ExtendedChromeDriver driver = new ExtendedChromeDriver)
  {
    List<Table> table = driver.AwaitElements(By.TagName("table")).ToList();
  }
  ```
  
  Same goes for clicking on an element. Sometimes with Selenium, an element will be located 
  on the page, but it will not be clickable, and will throw an exception.
  Here is how you would normally have to handle the situation:
  ```CS
  using (ChromeDriver driver = new ChromeDriver())
  {
    WebDriverWait wait = new WebDriverWait(this, new TimeSpan(30 * TimeSpan.TicksPerSecond));
    wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("idOfTheButtonOrLink")));
    IWebElement element = driver.FindElement(By.Id("idOfTheButtonOrLink")));
    element.Click();
  }
  ```
  The implementation is much more simple with ExtendedChromeDriver:
  ```CS
  using (ExtendedChromeDriver driver = new ExtendedChromeDriver)
  {
    driver.AwaitClick(By.Id("idOfTheButtonOrLink"));
  }
  ```
  
