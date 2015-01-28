using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using System.IO;
using System.Globalization;

namespace SiteParse
{
    class Program
    {
        static string chromeDriverPath = "WebDrivers";
        static string projDir = Path.Combine(new DirectoryInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.FullName, chromeDriverPath);
        //static IWebDriver webDriver = new ChromeDriver(projDir);
        static IWebDriver webDriver = new FirefoxDriver();

        static void Main(string[] args)
        {
            string url = "http://stats.nba.com/players/";

            webDriver.Navigate().GoToUrl(url);
            System.Threading.Thread.Sleep(3000);
            //System.Threading.Thread.Sleep(5000);
            //var elements = webDriver.FindElements(By.XPath(@"//div[@data-ng-controller='PlayerListCtrl']/div[3]//a[@class='active']"));
            var elements = webDriver.FindElements(By.CssSelector("div[data-ng-controller=PlayerListCtrl]>div:nth-child(3) a.active"));
            var links = elements.Select(e => e.GetAttribute("href")).ToList();
            //string attr = element.GetAttribute("class");
            //var e1 = elements.ElementAt(10);

            //webDriver.FindElement(By.XPath(@"//a[text()='Stats Profile']")).Click();
            //webDriver.FindElement(By.LinkText("Stats Profile")).Click();
            //var e2 = webDriver.FindElement(By.XPath(@"//div[@class='player-info']"));
            //var e2 = webDriver.FindElement(By.CssSelector("div.player-info"));

            List<Player> players = new List<Player>();
            //Console.WriteLine(e2.Text);
            for (int i = 0; i < 20; i++)
            {
                Player p = GetPlayer(links[i]);
                players.Add(p);
            }

            foreach (var pl in players)
            {
                Console.WriteLine(pl.Name);
                Console.WriteLine(pl.BirthDate);
                Console.WriteLine(pl.Height);
                Console.WriteLine(pl.Number);
                Console.WriteLine(pl.Position);
                Console.WriteLine(pl.Team);
                Console.WriteLine(pl.Weight);
            }
            //foreach (var e in elements)
            //{
            //    e.Click();
            //    e.FindElement(By.Id("tab-stats")).Click();

            //}

            Console.ReadLine();

            //Console.WriteLine(webDriver.PageSource);
            //File.WriteAllText(@"e:\projects\out\out.html", element.Text);
        }

        private static Player GetPlayer(string lnk)
        {
            Player player = new Player();
            webDriver.Navigate().GoToUrl(lnk);
            //webDriver.FindElement(By.ClassName("nbaVideoContainer"));
            //webDriver.FindElement(By.LinkText("Stats Profile")).Click();
            var playerInfoElm = webDriver.FindElement(By.Id("site-player-branding"));

            //var playerInfoElm = webDriver.FindElement(By.CssSelector("div.player-info"));
            //player.Name = new CultureInfo("en-us").TextInfo.ToTitleCase(playerInfoElm.FindElement(By.CssSelector("*:nth-child(1)")).Text.Replace("\r\n", " ").ToLower());
            player.Name = playerInfoElm.FindElement(By.ClassName("player-name")).Text;
            player.Team = playerInfoElm.FindElement(By.ClassName("player-team")).Text;
            string[] np = playerInfoElm.FindElement(By.ClassName("num-position")).Text.Split('|');
            //player.Position = playerInfoElm.FindElement(By.ClassName("player-position")).Text;
            player.Position = np[1].Trim();
            //player.Number = Convert.ToInt32(webDriver.FindElement(By.ClassName("player-number")).Text);
            player.Number = Convert.ToInt32(np[0]);
            //var playerBioElm = webDriver.FindElement(By.ClassName("player-bio"));
            var playerBioElm = webDriver.FindElement(By.Id("nbaVitalsOrigin"));
            DateTime dt;
            //player.BirthDate = DateTime.TryParse(playerBioElm.FindElement(By.XPath(@"div[contains(text(),'Born:')]")).Text.Replace("Born:", ""), new CultureInfo("en-us"), DateTimeStyles.None, out dt) ? dt : DateTime.MinValue;
            player.BirthDate = DateTime.TryParse(webDriver.FindElement(By.Id("nbaVitalsStats")).FindElement(By.XPath(@"div[1]/*[2]")).Text, new CultureInfo("en-us"), DateTimeStyles.None, out dt) ? dt : DateTime.MinValue;
            //string[] wh = playerBioElm.FindElement(By.XPath(@"div[1]")).Text.Split('/');
            //player.Weight = PoundToKg(Convert.ToDouble(wh[0].Replace("lbs", "")));
            player.Weight = Convert.ToDouble(playerBioElm.FindElement(By.XPath(@"div[2]/*[@class='nbaMeters']")).Text.Replace("kg", "").Replace("/", ""));
            //player.Height = InchToCm(Convert.ToDouble(wh[1].Split('-')[0]) * 12 + Convert.ToDouble(wh[1].Split('-')[1]));
            player.Height = Convert.ToDouble(playerBioElm.FindElement(By.XPath(@"div[1]/*[@class='nbaMeters']")).Text.Replace("m", "").Replace("/", ""));
            
            return player;
        }

        private static double PoundToKg(double val)
        {
            return val / 2.2046226218;
        }

        private static double InchToCm(double val)
        {
            return val * 2.54;
        }

        private static double FeetToCm(double val)
        {
            return val * 30.48;
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
