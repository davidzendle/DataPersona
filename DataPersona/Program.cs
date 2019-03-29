using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;

namespace DataPersona
{
    class Program
    {

        public static Random RNG;
        public static String FIRSTTAB;

        public class GoogleSearchResults
        {
            public List<IWebElement> SearchResults;
            public List<IWebElement> OtherSearchPages;


           
            public GoogleSearchResults(IWebDriver driver)
            {

                SearchResults = GetAllGoogleSearchResults(driver);
                OtherSearchPages = GetAllGoogleSearchPageLinks(driver);

            }

        }

        public static void GotoRandomSearchPage(GoogleSearchResults results, IWebDriver driver)
        {


            bool clickSuccessful = false;

            while (!clickSuccessful)
            {

                List<IWebElement> otherPages = results.OtherSearchPages;
            IWebElement randomLink = otherPages[RNG.Next(otherPages.Count)];

            try
            {

                Click(randomLink,driver);
                clickSuccessful = true;
            }
            catch(Exception e)
            {


                    results = new GoogleSearchResults(driver);


            }
            }

        }

        public static void BrowseToRandomPage(GoogleSearchResults results, IWebDriver driver)
        {


            bool clickSuccessful = false;

            while (!clickSuccessful)
            {
                results = new GoogleSearchResults(driver);
                List<IWebElement> googleSearchResults = results.SearchResults;
                List<IWebElement> otherLinks = GetAllLinks(driver);

                while(googleSearchResults.Count == 0 && otherLinks.Count == 0)
                {
                    driver.Navigate().Back();

                    results = new GoogleSearchResults(driver);
                   googleSearchResults = results.SearchResults;
                }


                IWebElement randomLink = otherLinks[RNG.Next(otherLinks.Count)];

                if (googleSearchResults.Count > 0)
                {

                    randomLink = googleSearchResults[RNG.Next(googleSearchResults.Count)];
                }

                try
                {

                    Click(randomLink, driver);
                    clickSuccessful = true;
                }
                catch (Exception e)
                {


                    results = new GoogleSearchResults(driver);


                }
            }

        }

        public static void Google(string search, IWebDriver driver)
        {
            driver.Url = "http://www.google.com";
            IWebElement searchBox = driver.FindElement(By.Name("q"));

            searchBox.SendKeys(search);
            searchBox.SendKeys(Keys.Return);
        }

        public static void Click(IWebElement link, IWebDriver driver)
        {

            driver.SwitchTo().Window(FIRSTTAB);
            Actions actions = new Actions(driver);
            actions.MoveToElement(link).Click().Perform();
        }

        static void Main(string[] args)
        {
            RNG = new Random();

            var driver = new ChromeDriver();

            driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 0, 5);

            FIRSTTAB = driver.CurrentWindowHandle;

            for (int i = 0; i<10; i++) {
               

            Google("latest news"+RandomSite(), driver);

            GoogleSearchResults results = new GoogleSearchResults(driver);    
            GotoRandomSearchPage(results,driver);

                results = new GoogleSearchResults(driver);
                BrowseToRandomPage(results, driver);

                for (int j = 0; j < 10; j++)
                {
                    BrowseToRandomPage(results, driver);

                }
            }


        }

        private static void TryToBrowseToNewPage(IWebDriver driver)
        {


            bool clickSuccessful = false;

            while (!clickSuccessful)
            {
                List<IWebElement> links = GetAllLinks(driver);

                while (links.Count < 3)
                {
                    driver.Navigate().Back();

                    links = GetAllLinks(driver);
                }

                IWebElement randomLink = links[RNG.Next(links.Count)];

                try
                {

                    Click(randomLink, driver);
                    clickSuccessful = true;
                }
                catch (Exception e)
                {


                    links = GetAllLinks(driver);


                }
            }



        }

        private static string RandomSite()
        {

            String site = " site:";
            List<String> sites = new List<string>(){"https://nation.foxnews.com/","https://www.drudgereport.com/",
"https://www.theblaze.com/","https://www.newsbusters.org/",
"https://dailycaller.com/","https://www.dailywire.com/",
"https://www.lifezette.com/","https://www.thegatewaypundit.com/",
"https://www.newsmax.com/","https://pjmedia.com/","https://thefederalist.com/",
"https://www.cnsnews.com/","https://freebeacon.com/","https://townhall.com/",
"https://www.truthrevolt.org/","https://twitchy.com/","https://www.freerepublic.com/",
"https://www.dailysignal.com/","https://www.wnd.com/","https://www.breitbart.com/",
"https://www.infowars.com/"};

            int random = RNG.Next(sites.Count);

            return site+sites[random];
        }

        public static List<IWebElement> GetAllLinks(IWebDriver driver)
        {
   
            var allLinks = driver.FindElements(By.TagName("a"));

            List<IWebElement> links = new List<IWebElement>();

            foreach(IWebElement l in allLinks)
            {
                links.Add(l);
            }
   
            return links;
            
        }

        public static List<IWebElement> GetAllGoogleSearchResults(IWebDriver driver)
        {
            var allSearchResults = driver.FindElements(By.ClassName("r"));

        

            List<IWebElement> links = new List<IWebElement>();

            foreach (IWebElement sr in allSearchResults)
            {
                foreach(IWebElement l in sr.FindElements(By.TagName("a"))) { 
                links.Add(l);
                }
            }

            return links;

        }

        public static List<IWebElement> GetAllGoogleSearchPageLinks(IWebDriver driver)
        {
            var allSearchResults = driver.FindElements(By.Id("nav"));
            List<IWebElement> links = new List<IWebElement>();

            foreach (IWebElement sr in allSearchResults)
            {
                //s
                foreach (IWebElement l in sr.FindElements(By.TagName("a")))
                {
                    links.Add(l);
                }
            }

            return links;

        }

    }
}
