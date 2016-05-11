using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightspeed_Product_Management
{
    public class AccessToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
    }

    class Authentication
    {
        public static string Authenticate(string username)
        {
            string redirectURL = GetTemporaryToken(username);
            string[] temp = redirectURL.Split('=');
            string temporaryToken = temp[1];

            string url = "https://cloud.merchantos.com/oauth/access_token.php";

            var client = new RestClient(url);
            var request = new RestRequest("", Method.POST);
            request.AddParameter("client_id", "michaelf@511tactical.com");
            request.AddParameter("client_secret", "H$lfl!fE57");
            request.AddParameter("code", temporaryToken);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("redirect_uri", @"https://www.511tactical.com");

            IRestResponse response = client.Execute(request);
            var content = response.Content;
            return JsonConvert.DeserializeObject<AccessToken>(content).access_token;
        }

        private static string GetTemporaryToken(string username)
        {
            string url = "https://cloud.merchantos.com/oauth/authorize.php?response_type=code&client_id=michaelf@511tactical.com&scope=employee:all";
            var client = new RestClient(url);
            client.Authenticator = new HttpBasicAuthenticator(username, "511N!mbl3");

            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);

            IWebDriver driver = new ChromeDriver("phantomjs.exe");
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(15));
            driver.Navigate().GoToUrl(response.ResponseUri.ToString());

            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys(username);

            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("511N!mbl3");

            IWebElement submit = driver.FindElement(By.Id("submitButton"));
            submit.Click();

            IWebElement authorize = driver.FindElement(By.XPath("//*[@id=\"subscription_form\"]/div/input[1]"));
            authorize.Click();

            driver.Navigate().Back();

            IWebElement authorize2 = driver.FindElement(By.XPath("//*[@id=\"subscription_form\"]/div/input[1]"));
            authorize2.Click();

            string result = driver.Url;
            driver.Quit();

            return result;
        }
    }
}
