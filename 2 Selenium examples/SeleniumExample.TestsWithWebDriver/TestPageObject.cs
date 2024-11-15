using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumExample.TestsWithWebDriver;
public class TestPageObject
{
    private readonly IWebDriver _driver;

    public TestPageFormControl Form { get; private set; }
    public IWebElement PreElement => _driver.FindElement(By.TagName("pre"));
    public IWebElement LoadingElement => _driver.FindElement(By.TagName("p"));

    public TestPageObject(IWebDriver driver)
    {
        Form = new TestPageFormControl(driver);
        _driver = driver;
    }

    public async void OpenPage(string url)
    {
        _driver.Navigate().GoToUrl(url);
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(180));
        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.TagName("form")));
    }

    public void WaitForRequestsEnd()
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(180));
        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("pre")));
    }

    public string GetCreateResult()
    {
        return PreElement.Text;
    }
}
