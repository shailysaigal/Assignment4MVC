using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Simple_API_Database.Models.EF_Models;
using static Simple_API_Database.Models.KeyStat;
using static Simple_API_Database.Models.Quote;
using static Simple_API_Database.Models.HistoricalData;
using static Simple_API_Database.Models.CompanyInfo;
using Simple_API_Database.Models;
using Simple_API_Database.DataAccess;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Simple_API_Database.Controllers
{
    public class HomeController : Controller
    {
        /*
            These lines are needed to use the Database context,
            define the connection to the API, and use the
            HttpClient to request data from the API
        */
        public ApplicationDbContext dbContext;
        //Base URL for the IEXTrading API. Method specific URLs are appended to this base URL.
        string BASE_URL = "https://api.iextrading.com/1.0/";
        HttpClient httpClient;

        /*
             These lines create a Constructor for the HomeController.
             Then, the Database context is defined in a variable.
             Then, an instance of the HttpClient is created.

        */
        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new
                System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        /*
            Calls the IEX reference API to get the list of symbols.
            Returns a list of the companies whose information is available. 
        */
        public List<Company> GetSymbols()
        {
            string IEXTrading_API_PATH = BASE_URL + "ref-data/symbols";
            string companyList = "";
            List<Company> companies = null;

            // connect to the IEXTrading API and retrieve information
            httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

            // read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                companyList = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // now, parse the Json strings as C# objects
            if (!companyList.Equals(""))
            {
                // https://stackoverflow.com/a/46280739
                companies = JsonConvert.DeserializeObject<List<Company>>(companyList);
                //companies = companies.GetRange(0,100);
            }

            return companies;
        }
        
        public IActionResult Index()
        {
            //Set ViewBag variable first
            ViewBag.dbSuccessComp = 0;
            List<Company> companies = GetSymbols();

            //Save companies in TempData, so they do not have to be retrieved again
            TempData["Companies"] = JsonConvert.SerializeObject(companies);

            return View(companies);
        }

        /*
            The Symbols action calls the GetSymbols method that returns a list of Companies.
            This list of Companies is passed to the Symbols View.
        */
        public IActionResult Symbols()
        {
            //Set ViewBag variable first
            ViewBag.dbSuccessComp = 0;
            List<Company> companies = GetSymbols();

            //Save companies in TempData, so they do not have to be retrieved again
           TempData["Companies"] = JsonConvert.SerializeObject(companies);

           // String companiesData = JsonConvert.SerializeObject(companies);

            //HttpContext.Session.SetString("CompaniesData", companiesData);

            return View(companies);
        }
 

        public IActionResult PopulateSymbols()
        {
            if (TempData["Companies"] != null)
            {
                //string companiesData = HttpContext.Session.GetString("CompaniesData");
                // Retrieve the companies that were saved in the symbols method
                List<Company> companies = JsonConvert.DeserializeObject<List<Company>>(TempData["Companies"].ToString());

                foreach (Company company in companies)
                {
                    //Database will give PK constraint violation error when trying to insert record with existing PK.
                    //So add company only if it doesnt exist, check existence using symbol (PK)
                    if (dbContext.Companies.Where(c => c.symbol.Equals(company.symbol)).Count() == 0)
                    {
                        dbContext.Companies.Add(company);
                    }
                }

                dbContext.SaveChanges();
                ViewBag.dbSuccessComp = 1;
                return View("Index", companies);
            }
            return View("Index");
        }


        public List<KeyStat> GetKeyStats(string stocksymbol1, string stocksymbol2)
        {
            List<KeyStat> KeyStatList = new List<KeyStat>();
            KeyStat keystpercmp = null;
            if (stocksymbol1 != "")
            {
                keystpercmp = GetKeyStats(stocksymbol1);
                if (keystpercmp != null)
                    KeyStatList.Add(keystpercmp);
            }
            KeyStat keystpercmp1 = null;
            if (stocksymbol2 != "")
            {
                keystpercmp1 = GetKeyStats(stocksymbol2);
                if (keystpercmp1 != null)
                    KeyStatList.Add(keystpercmp1);
            }
            return KeyStatList;
        }


        public KeyStat GetKeyStats(string stocksymbol1)
        {
            string IEXTrading_API_PATH = BASE_URL + "stock/" + stocksymbol1+ "/stats";
            string KeyStats = "";
            
            KeyStat keystpercmp = null;

            // connect to the IEXTrading API and retrieve information
            //httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

            // read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                KeyStats = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // now, parse the Json strings as C# objects
            if (!KeyStats.Equals(""))
            {
                // https://stackoverflow.com/a/46280739
                keystpercmp = JsonConvert.DeserializeObject<KeyStat>(KeyStats);
                //companies = companies.GetRange(0, 50);
            }

            return keystpercmp;
        }

          
        public ActionResult StockStats(string txtName = "", string txtName2 = "")
        {
          
            ViewBag.Name = txtName;
            ViewBag.Name2 = txtName2;
                
            //Set ViewBag variable first
            ViewBag.dbSuccessComp = 0;
            List<KeyStat> keystats = GetKeyStats(ViewBag.Name, ViewBag.Name2);

            //Save companies in TempData, so they do not have to be retrieved again
            TempData["keystats"] = JsonConvert.SerializeObject(keystats);

            TempData.Keep("keystats");

            return View(keystats);
        }

       
        public IActionResult PopulateKeyStats()
        {
            if (TempData["keystats"] != null)
            {
                // Retrieve the companies that were saved in the symbols method
                List<KeyStat> keystats = JsonConvert.DeserializeObject<List<KeyStat>>(TempData["keystats"].ToString());

                foreach (KeyStat ks in keystats)
                {
                    //Database will give PK constraint violation error when trying to insert record with existing PK.
                    //So add company only if it doesnt exist, check existence using symbol (PK)
                    if (dbContext.KeyStats.Where(c => c.symbol.Equals(ks.symbol)).Count() == 0)
                    {
                        dbContext.KeyStats.Add(ks);
                    }
                }

                dbContext.SaveChanges();
                ViewBag.dbSuccessComp = 1;
                return View("StockStats", keystats);
            }
            return View("StockStats");
        }

        public Quote GetQuote(string stocksymbol1)
        {
            string IEXTrading_API_PATH = BASE_URL + "stock/" + stocksymbol1 + "/quote";
            string Quotes = "";

            Quote keystpercmp = null;

            // connect to the IEXTrading API and retrieve information
            //httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

            // read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                Quotes = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // now, parse the Json strings as C# objects
            if (!Quotes.Equals(""))
            {
                // https://stackoverflow.com/a/46280739
                keystpercmp = JsonConvert.DeserializeObject<Quote>(Quotes);
                //companies = companies.GetRange(0, 50);
            }

            return keystpercmp;
        }

        public ActionResult Quotes(string txtName = "", string txtName2 = "")
        {

            ViewBag.Name = txtName;
            ViewBag.Name2 = txtName2;

            //Set ViewBag variable first
            ViewBag.dbSuccessComp = 0;
            List<Quote> Quotes = GetQuote(ViewBag.Name, ViewBag.Name2);

            //Save companies in TempData, so they do not have to be retrieved again
            TempData["Quotes"] = JsonConvert.SerializeObject(Quotes);

            return View(Quotes);
        }

        public List<Quote> GetQuote(string stocksymbol1, string stocksymbol2)
        {
            List<Quote> QuoteList = new List<Quote>();
            Quote keystpercmp = null;
            if (stocksymbol1 != "")
            {
                keystpercmp = GetQuote(stocksymbol1);
                if (keystpercmp != null)
                    QuoteList.Add(keystpercmp);
            }
            Quote keystpercmp1 = null;
            if (stocksymbol2 != "")
            {
                keystpercmp1 = GetQuote(stocksymbol2);
                if (keystpercmp1 != null)
                    QuoteList.Add(keystpercmp1);
            }
            return QuoteList;
        }

        public IActionResult PopulateQuote()
        {

            if (TempData["Quotes"] != null)
            {
                // Retrieve the companies that were saved in the symbols method
                List<Quote> Quotes = JsonConvert.DeserializeObject<List<Quote>>(TempData["Quotes"].ToString());

                foreach (Quote ks in Quotes)
                {
                    if (dbContext.Quotes.Where(c => c.symbol.Equals(ks.symbol)).Count() == 0)
                    {
                        dbContext.Quotes.Add(ks);
                    }
                }

                dbContext.SaveChanges();
                ViewBag.dbSuccessComp = 1;
                return View("Quotes", Quotes);
            }
            return View("Quotes");
        }

        public List<CompanyInfo> GetCompanyInfo(List<String> symbols)
        {
            
            string companyInfoList = "";
            CompanyInfo companiesInfo = null;
            List<CompanyInfo> companies = new List<CompanyInfo>();
            // connect to the IEXTrading API and retrieve information

            foreach (String item in symbols)
            {
                string IEXTrading_API_PATH = BASE_URL + "stock/" + item + "/company";
                httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
                HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

                // read the Json objects in the API response
                if (response.IsSuccessStatusCode)
                {
                    companyInfoList = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                // now, parse the Json strings as C# objects
                if (!companyInfoList.Equals(""))
                {
                    // https://stackoverflow.com/a/46280739
                    companiesInfo = JsonConvert.DeserializeObject<CompanyInfo>(companyInfoList);
                    //companies = companies.GetRange(0,100);
                }
                //dbContext.CompanyInfo.Add(companiesInfo);
                companies.Add(companiesInfo);
            }
            //dbContext.SaveChanges();
            return companies;
        }

        public IActionResult CompanyInfo()
        {
            //Set ViewBag variable first
            ViewBag.dbSuccessComp = 0;
            //List<Company> companies = dbContext.Companies.ToList();
            List<Company> companies = GetSymbols().GetRange(0, 10);
            List<String> companyNames = new List<string>();
            List<CompanyInfo> companyInfos = new List<CompanyInfo>();
            foreach(Company company in companies)
            {
                //CompanyInfo companyInfo = GetCompanyInfo(company.symbol);
                companyNames.Add(company.symbol);
            }
            companyInfos = GetCompanyInfo(companyNames);
            //List<CompanyInfo> companyInfo = GetCompanyInfo(symbol);

            //Save companies in TempData, so they do not have to be retrieved again
            //TempData["CompanyInfo"] = JsonConvert.SerializeObject(companyInfo);

            return View(companyInfos);
        }
        public IActionResult PopulateCompanyInfo()
        {
            // Retrieve the companies that were saved in the symbols method
            List<CompanyInfo> companyInfo = JsonConvert.DeserializeObject<List<CompanyInfo>>(TempData["CompanyInfo"].ToString());

            foreach (CompanyInfo companyInfo1 in companyInfo)
            {
                //Database will give PK constraint violation error when trying to insert record with existing PK.
                //So add company only if it doesnt exist, check existence using symbol (PK)
                if (dbContext.CompanyInfo.Where(c => c.symbol.Equals(companyInfo1.symbol)).Count() == 0)
                {

                    dbContext.CompanyInfo.Add(companyInfo1);
                }
            }

            dbContext.SaveChanges();
            ViewBag.dbSuccessComp = 1;
            return View("CompanyInfo", companyInfo);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Stocks()
        {
            return View();
        }
       
        public IActionResult Quote()
        {
            return View();
        }

        public List<List<HistoricalData>> GetHistoricalData(string stocksymbol1, string stocksymbol2)
        {
            List<List<HistoricalData>> histdatalist = new List<List<HistoricalData>>();
            List<HistoricalData> histdata = null;
            if (stocksymbol1 != "")
            {
                histdata = GetHistoricalData(stocksymbol1);
                if (histdata != null)
                    histdatalist.Add(histdata);
            }
            List<HistoricalData> histdata1 = null;
            if (stocksymbol2 != "")
            {
                histdata1 = GetHistoricalData(stocksymbol2);
                if (histdata1 != null)
                    histdatalist.Add(histdata1);
            }
            return histdatalist;
        }


        public List<HistoricalData> GetHistoricalData(string stocksymbol1)
        {
            string IEXTrading_API_PATH = BASE_URL + "stock/" + stocksymbol1 + "/chart/1y";
            string histdata = "";

            List<HistoricalData> histdatacmp = null;

            // connect to the IEXTrading API and retrieve information
            //httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

            // read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                histdata = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // now, parse the Json strings as C# objects
            if (!histdata.Equals(""))
            {
                // https://stackoverflow.com/a/46280739
                histdatacmp = JsonConvert.DeserializeObject<List<HistoricalData>>(histdata);
                histdatacmp = histdatacmp.GetRange(0, 5);
            }

            return histdatacmp;
        }


        public ActionResult HistoricalDatas(string txtName = "", string txtName2 = "")
        {

            ViewBag.Name = txtName;
            ViewBag.Name2 = txtName2;

            //Set ViewBag variable first
            ViewBag.dbSuccessComp = 0;
            List<List<HistoricalData>> histdt = GetHistoricalData(ViewBag.Name, ViewBag.Name2);

            //Save companies in TempData, so they do not have to be retrieved again
            TempData["HistoricalData"] = JsonConvert.SerializeObject(histdt);

            return View(histdt);
        }


        public IActionResult PopulateHistoricalData()
        {
            if (TempData["HistoricalData"] != null)
            {
                // Retrieve the companies that were saved in the symbols method
                List<List<HistoricalData>> histdatalist = JsonConvert.DeserializeObject<List<List<HistoricalData>>>(TempData["HistoricalData"].ToString());

                foreach (List<HistoricalData> hs1 in histdatalist)
                {
                    foreach (HistoricalData hs in hs1)
                    {
                        //Database will give PK constraint violation error when trying to insert record with existing PK.
                        //So add company only if it doesnt exist, check existence using symbol (PK)
                        if (dbContext.HistoricalDatas.Where(c => c.high.Equals(hs.high)).Count() == 0)
                        {
                            dbContext.HistoricalDatas.Add(hs);
                        }
                    }
                }

                dbContext.SaveChanges();
                ViewBag.dbSuccessComp = 1;
                return View("HistoricalDatas", histdatalist);
            }
            return View("HistoricalDatas");
        }
        public IActionResult AboutUs()
        {
            return View();
        }
//Self-Reflection
        //In todays world, numerous stock market applications are available with lots of complex and sometimes confusing functionalities
   //Our " Simple Stock Search Application " aims at simplifying this confusion. It gives you a simple search functionality!
//	  This application aims at the users who are confused between two prominent stocks where they wish to buy one!
// 	Our application will help these users in getting the results within few minutes!!
//	  In addition, No Logins Required.. This application is free to everyone !!



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
