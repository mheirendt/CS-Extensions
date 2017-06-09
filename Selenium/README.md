<b>Subclasses and Helper methods for [Selenium](http://docs.seleniumhq.org)</b>

* <b>Chrome Driver</b>

  Simplifies the code you need to write to be able to test or scrape web pages.
  * <b>Iterating through tables</b>
    ```CS
    using (SEChromeDriver driver = new SEChromeDriver())
    {
      SETable table = driver.Tables[0];
      List<SETableRow> tableRows = table.Rows;
      List<SETableCells> tableCells = tableRows.Cells;
      string text = table.Rows[0].Cells[0].Text;
      string html = table.Rows[0].Cells[0].Html;
    }
    ```
  * Waiting For an element to become available
    ```CS
    using (SEChromeDriver driver = new SEChromeDriver())
    {
      List<SETable> tables = driver.Tables;
    }
    ```
  
    Or another way:
    ```CS
    using (SEChromeDriver = new SEChromeDriver())
    {
      List<SETable> tables = driver.AwaitElements(By.TagName("table"));
    }
    ```
  
  * Waiting for an element to be clickable
    ```CS
    using SEChromeDriver driver = new SEChromeDriver())
    {
      driver.AwaitClick(By.Id("idOfTheButtonOrLink"));
    }
    ```
    
  * Searching for elements by text
    ```CS
    using (SEChromeDriver driver = new SEChromeDriver())
    {
      SEInput input = driver.Document.Inputs.FindByText("submit");
    }
    ```
    
  * Searching for elments by alt tag
    ```CS
    using (SEChromeDriver driver = new SEChromeDriver())
    {
      SEImage image = driver.Document.Images.FindByAlt("next");
    }
    ```

  * You can also search by regex if you are not sure exactly what the text will be
  ```CS
  using (SEChromeDriver driver = new SEChromeDriver())
  {
    Regex divRegex = new Regex(@"[A-Za-z]{2,8}\d+");
    SEDiv div = driver.Document.Divs.FindByText(divRegex);
  }
  ```
