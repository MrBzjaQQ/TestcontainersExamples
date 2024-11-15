using OpenQA.Selenium;
using SeleniumExample.Portal.Server.Dtos;

namespace SeleniumExample.TestsWithWebDriver;
public class TestPageFormControl
{
    private readonly IWebDriver _driver;
    public IWebElement PhoneInput => _driver.FindElement(By.Id("phone"));
    public IWebElement UserNameInput => _driver.FindElement(By.Id("userName"));
    public IWebElement EmailInput => _driver.FindElement(By.Id("email"));
    public IWebElement PositionInput => _driver.FindElement(By.Id("position"));
    public IWebElement SubmitButton => _driver.FindElement(By.CssSelector("button[type=\"submit\"]"));

    public TestPageFormControl(IWebDriver driver)
    {
        _driver = driver;
    }

    public void FillForm(CreateEmployeeRequest formData)
    {
        PhoneInput.SendKeys(formData.Phone);
        UserNameInput.SendKeys(formData.UserName);
        EmailInput.SendKeys(formData.Email);
        PositionInput.SendKeys(formData.Position);
    }

    public void SubmitForm()
    {
        SubmitButton.Click();
    }
}
