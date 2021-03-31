using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.VisualBasic;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Linq;
using System.Collections.Generic;

namespace ServiceTimeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceTimeController : ControllerBase
    {
        protected IHttpClientFactory HttpClientFactory { get; }
        private string csvFilePath = @"C:\FileDownload\Export.csv";
        private string companyFilePath = @"C:\FileDownload\Customers.csv";
        public ServiceTimeController(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        [HttpGet("GetServiceTimes")]
        public async Task<IActionResult> GetServiceTimes(int month, int year)
        {
            //await GetExportCSV(month, year);
            await Task.Delay(10);
            return Ok(ProcessCSV(csvFilePath));
        }



        private async Task GetExportCSV(int month, int year)
        {
            DateTime startTime = new DateTime(year, month, 1);
            startTime = DateTime.SpecifyKind(startTime, DateTimeKind.Utc);

            DateTime endTime = new DateTime(year, month + 1, 1).AddSeconds(-1);
            endTime = DateTime.SpecifyKind(endTime, DateTimeKind.Utc);

            ExportRequest req = new ExportRequest()
            {
                start = ((DateTimeOffset)startTime).ToUnixTimeSeconds(),
                end = ((DateTimeOffset)endTime).ToUnixTimeSeconds(),
                timezone = "America/Denver",
                should_export_events = true,
                columns = new string[]{
                    "Activity ID",
                    "Conversation ID",
                    "Tags at activity time"
                }
            };
            string content = JsonSerializer.Serialize(req);

            using (HttpClient httpClient = HttpClientFactory.CreateClient("frontappexport"))
            {
                StringContent reqJson = new StringContent(
                        JsonSerializer.Serialize(req),
                        Encoding.UTF8,
                        "application/json");

                HttpResponseMessage res = await httpClient.PostAsync("exports", reqJson);
                res.EnsureSuccessStatusCode();
                content = await res.Content.ReadAsStringAsync();
                ExportResponse resObj = JsonSerializer.Deserialize<ExportResponse>(content);

                while (true)
                {
                    System.Threading.Thread.Sleep(5000);

                    res = await httpClient.GetAsync($"exports/{resObj.id}");
                    res.EnsureSuccessStatusCode();
                    content = await res.Content.ReadAsStringAsync();
                    resObj = JsonSerializer.Deserialize<ExportResponse>(content);
                    if (resObj.url != null)
                        break;
                }

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, resObj.url))
                {
                    using (Stream contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(), stream = new FileStream(csvFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 10000, true))
                    {
                        await contentStream.CopyToAsync(stream);
                    }
                }
            }
        }

        private Hashtable ParseCSV(string filename)
        {
            Hashtable res = new Hashtable();
            using (TextFieldParser parser = new TextFieldParser(filename))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                string[] fields = parser.ReadFields();
                while (!parser.EndOfData)
                {
                    fields = parser.ReadFields();
                    if (res.ContainsKey(fields[1]))
                        res[fields[1]] = fields[2];
                    else
                        res.Add(fields[1], fields[2]);
                }
            }
            return res;
        }

        private Dictionary<string, string> ParseCompanyList()
        {
            Dictionary<string, string> companies = new Dictionary<string, string>();
            using (TextFieldParser parser = new TextFieldParser(companyFilePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                string[] fields = parser.ReadFields();
                while (!parser.EndOfData)
                {
                    fields = parser.ReadFields();
                    if (!companies.ContainsKey(fields[0]))
                        companies.Add(fields[0], fields[1]);
                }
            }
            return companies;
        }

        private Dictionary<string, ProductServiceTime> ProcessCSV(string fileName)
        {
            string[] productNames = { "ProChart", "NetFlow", "ProTrend", "ProMonitor" };
            Dictionary<string, ProductServiceTime> productServiceTimes = new Dictionary<string, ProductServiceTime>();
            Hashtable ht = ParseCSV(fileName);
            Dictionary<string, string> companyList = ParseCompanyList();


            foreach (string key in ht.Keys)
            {
                ProductServiceTime currentProduct;
                CompanyServiceTime currentCompany;
                List<string> tags = ht[key].ToString().Split(',').ToList();
                int i = 0;

                //Get minutes for conversation
                foreach (string tag in tags)
                    if (tag.Contains("Minutes"))
                        i = int.Parse(tag.Split(" ")[0]);

                //Figure out what product it is
                foreach (string product in productNames)
                {
                    if (tags.Contains(product))
                    {
                        //Add the product if its not in the dictionary
                        if (!productServiceTimes.ContainsKey(product))
                        {
                            productServiceTimes.Add(product, new ProductServiceTime());
                        }
                        //Add tickets and time to product
                        currentProduct = productServiceTimes[product];
                        currentProduct.TotalTickets++;
                        currentProduct.TotalServiceTime += i;

                        //Get the company for the ticket
                        currentCompany = null;
                        foreach (string tag in tags)
                        {
                            if (companyList.ContainsKey(tag))
                            {
                                //If company not in product yet create new company service time and add it. 
                                if (!currentProduct.companyServiceTimes.ContainsKey(tag))
                                    currentProduct.companyServiceTimes.Add(tag, new CompanyServiceTime()
                                    {
                                        Name = companyList[tag],
                                        Code = tag
                                    });
                                currentCompany = currentProduct.companyServiceTimes[tag];
                                break;
                            }
                        }
                        //If no company set company to unknown
                        if (currentCompany == null)
                        {
                            if (!currentProduct.companyServiceTimes.ContainsKey("Unknown"))
                                currentProduct.companyServiceTimes.Add("Unknown", new CompanyServiceTime()
                                {
                                    Name = "Unknown",
                                    Code = "0"
                                });
                            currentCompany = currentProduct.companyServiceTimes["Unknown"];
                        }

                        if (tags.Contains("Billable"))
                            currentCompany.BillableServiceTime += i;
                        else if (tags.Contains("Non-Billable"))
                            currentCompany.NonBillableServiceTime += i;
                        else
                            currentCompany.UnknownBillableServiceTime += i;
                    }
                }
            }
            return productServiceTimes;
        }
    }


    public class ProductServiceTime
    {
        public ProductServiceTime()
        {
            TotalTickets = 0;
            TotalServiceTime = 0;
            companyServiceTimes = new Dictionary<string, CompanyServiceTime>();
        }
        public int TotalTickets { get; set; }
        public int TotalServiceTime { get; set; }
        public Dictionary<string, CompanyServiceTime> companyServiceTimes { get; set; }
    }

    public class CompanyServiceTime
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int BillableServiceTime { get; set; }
        public int NonBillableServiceTime { get; set; }
        public int UnknownBillableServiceTime { get; set; }
    }


    public class ExportRequest
    {
        public string inbox_id { get; set; }
        public string tag_id { get; set; }
        public long start { get; set; }
        public long end { get; set; }
        public string timezone { get; set; }
        public bool should_export_events { get; set; }
        public string[] columns { get; set; }
    }

    public class ExportResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public int progress { get; set; }
        public string url { get; set; }
        public string filename { get; set; }
        public int? size { get; set; }
        public ExportRequest query { get; set; }
    }
}
