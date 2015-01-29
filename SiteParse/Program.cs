using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.IO;

namespace SiteParse
{
    class Program
    {
        static void Main(string[] args)
        {
            string chromeDriverPath = "WebDrivers";
            string projDir = Path.Combine(new DirectoryInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.FullName, chromeDriverPath);

            string url = "http://stats.nba.com/";

            IWebDriver webDriver = new ChromeDriver(projDir);

            webDriver.Navigate().GoToUrl(url);
            webDriver.FindElement(By.XPath(@"//*[@id=""main-container""]/div[2]/div/div[3]/div/div/div[1]/div[1]/div/div[2]/div/div/a[1]")).Click();
            var element = webDriver.FindElement(By.XPath(@"//*[@id=""main-container""]/div[2]/div/div[3]/div/div/div[3]"));

            var elements = element.FindElements(By.XPath(@"//*[@class='active']"));

            var temp = elements.First();
            temp.Click();
            //System.Threading.Thread.Sleep(5000);
            var temp2 = webDriver.FindElement(By.Id("tab-stats"));
            temp2.Click();
            Player pl1 = GetPlayer(webDriver);

            //foreach (var e in elements)
            //{
            //    e.Click();
            //    e.FindElement(By.Id("tab-stats")).Click();

            //}



                //Console.WriteLine(webDriver.PageSource);
            File.WriteAllText(@"e:\projects\out\out.html", element.Text);
        }

        private static Player GetPlayer(IWebDriver drv)
        {
            Player player = new Player();
            var infoElement = drv.FindElement(By.XPath(@"//*[@id='main-container']/div[2]/div/div/div[1]/div/div/div[3]/div"));
            player.Number = Convert.ToInt32(infoElement.FindElement(By.ClassName("player-number")).Text);
            var pliElement = infoElement.FindElement(By.ClassName("player-info"));
            player.Name = pliElement.FindElement(By.XPath(@"*[1]")).Text;
            player.Team = pliElement.FindElement(By.ClassName("player-team")).Text;
            player.Position = pliElement.FindElement(By.ClassName("player-position")).Text;
            return player;
        }
    }

    class Player
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Position { get; set; }
        public int Number { get; set; }
        public string Team { get; set; }
        
    }
}
