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

        public ServiceTimeController(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        [HttpGet("GetServiceTimes")]
        public async Task<IActionResult> GetServiceTimes(int month, int year)
        {
            ServiceTimeTally tally = new ServiceTimeTally();
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
                    System.Threading.Thread.Sleep(200);

                    res = await httpClient.GetAsync($"exports/{resObj.id}");
                    res.EnsureSuccessStatusCode();
                    content = await res.Content.ReadAsStringAsync();
                    resObj = JsonSerializer.Deserialize<ExportResponse>(content);
                    if (resObj.url != null)
                        break;
                }

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, resObj.url))
                {
                    using (Stream contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(), stream = new FileStream(@"C:\Users\tim.su.ES\Desktop\Export.csv", FileMode.Create, FileAccess.Write, FileShare.None, 10000, true))
                    {
                        await contentStream.CopyToAsync(stream);
                    }
                }
                tally = ProcessCSV(@"C:\Users\tim.su.ES\Desktop\Export.csv");
            }
            return Ok(tally);
        }
        private ServiceTimeTally TallyResult(List<string> tags, int i, ServiceTimeTally tally) {

            dynamic serviceTimeDTO = tally.ProchartServiceTimeDTO;
            if (tags.Contains("ProChart"))
            {
                 serviceTimeDTO = tally.ProchartServiceTimeDTO;
                 serviceTimeDTO.Convos++;

                if (tags.Contains("Non-Billable"))
                {
                    serviceTimeDTO.NonBillableConvos++;
                    serviceTimeDTO.NonBillableServiceTime += i;
                }
                else if (tags.Contains("Billable"))
                {
                    serviceTimeDTO.BillableConvos++;
                    serviceTimeDTO.BillableServiceTime += i;
                }
                else
                {
                    serviceTimeDTO.UnknownBillableConvos++;
                    serviceTimeDTO.UnknownBillableServiceTime += i;
                }
                serviceTimeDTO.ServiceTime =
                    serviceTimeDTO.NonBillableServiceTime
                    + serviceTimeDTO.BillableServiceTime
                    + serviceTimeDTO.UnknownBillableServiceTime;
            }
            else if (tags.Contains("NetFlow"))
            {
                serviceTimeDTO = tally.NetflowServiceTimeDTO;
                serviceTimeDTO.Convos++;

                if (tags.Contains("Non-Billable"))
                {
                    serviceTimeDTO.NonBillableConvos++;
                    serviceTimeDTO.NonBillableServiceTime += i;
                }
                else if (tags.Contains("Billable"))
                {
                    serviceTimeDTO.BillableConvos++;
                    serviceTimeDTO.BillableServiceTime += i;
                }
                else
                {
                    serviceTimeDTO.UnknownBillableConvos++;
                    serviceTimeDTO.UnknownBillableServiceTime += i;
                }
                serviceTimeDTO.ServiceTime =
                    serviceTimeDTO.NonBillableServiceTime
                    + serviceTimeDTO.BillableServiceTime
                    + serviceTimeDTO.UnknownBillableServiceTime;
            }
            else if (tags.Contains("ProTrend"))
            {
                serviceTimeDTO = tally.ProtrendServiceTimeDTO;
                serviceTimeDTO.Convos++;

                if (tags.Contains("Non-Billable"))
                {
                    serviceTimeDTO.NonBillableConvos++;
                    serviceTimeDTO.NonBillableServiceTime += i;
                }
                else if (tags.Contains("Billable"))
                {
                    serviceTimeDTO.BillableConvos++;
                    serviceTimeDTO.BillableServiceTime += i;
                }
                else
                {
                    serviceTimeDTO.UnknownBillableConvos++;
                    serviceTimeDTO.UnknownBillableServiceTime += i;
                }
                serviceTimeDTO.ServiceTime =
                    serviceTimeDTO.NonBillableServiceTime
                    + serviceTimeDTO.BillableServiceTime
                    + serviceTimeDTO.UnknownBillableServiceTime;
            }
            else if (tags.Contains("ProMonitor"))
            {
                serviceTimeDTO = tally.PromonitorServiceTimeDTO;
                serviceTimeDTO.Convos++;

                if (tags.Contains("Non-Billable"))
                {
                    serviceTimeDTO.NonBillableConvos++;
                    serviceTimeDTO.NonBillableServiceTime += i;
                }
                else if (tags.Contains("Billable"))
                {
                    serviceTimeDTO.BillableConvos++;
                    serviceTimeDTO.BillableServiceTime += i;
                }
                else
                {
                    serviceTimeDTO.UnknownBillableConvos++;
                    serviceTimeDTO.UnknownBillableServiceTime += i;
                }
                serviceTimeDTO.ServiceTime =
                    serviceTimeDTO.NonBillableServiceTime
                    + serviceTimeDTO.BillableServiceTime
                    + serviceTimeDTO.UnknownBillableServiceTime;
            }
            
            return tally;
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

        private ServiceTimeTally ProcessCSV(string fileName)
        {
            Hashtable ht = ParseCSV(fileName);
            ServiceTimeTally tally = new ServiceTimeTally();
            tally.ProchartServiceTimeDTO = new ProChartServiceTimeDTO { Convos = 0, ServiceTime = 0 };
            tally.NetflowServiceTimeDTO = new NetFlowServiceTimeDTO { Convos = 0, ServiceTime = 0 };
            tally.ProtrendServiceTimeDTO = new ProTrendServiceTimeDTO { Convos = 0, ServiceTime = 0 };
            tally.PromonitorServiceTimeDTO = new ProMonitorServiceTimeDTO { Convos = 0, ServiceTime = 0 };
            foreach (string key in ht.Keys)
            {
                List<string> tags = ht[key].ToString().Split(',').ToList();
                int i = 0;
                foreach (string tag in tags)
                    if (tag.Contains("Minutes"))
                        i = int.Parse(tag.Split(" ")[0]);
                TallyResult(tags, i, tally);
            }

            return tally;
        }

    }

    public class ServiceTimeTally
    {
        public ProChartServiceTimeDTO ProchartServiceTimeDTO { get; set; }
        public NetFlowServiceTimeDTO NetflowServiceTimeDTO { get; set; }
        public ProTrendServiceTimeDTO ProtrendServiceTimeDTO { get; set; }
        public ProMonitorServiceTimeDTO PromonitorServiceTimeDTO { get; set; }
    }

    public class ProChartServiceTimeDTO
    {
        public int Convos { get; set; } = 0;
        public int ServiceTime { get; set; } = 0;

        public int BillableConvos { get; set; } = 0;
        public int BillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> BillableTagServiceTime { get; set; }

        public int NonBillableConvos { get; set; } = 0;
        public int NonBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> NonBillableTagServiceTime { get; set; }

        public int UnknownBillableConvos { get; set; } = 0;
        public int UnknownBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> UnknownBillableTagServiceTime { get; set; }
    }
    public class NetFlowServiceTimeDTO
    {
        public int Convos { get; set; } = 0;
        public int ServiceTime { get; set; } = 0;

        public int BillableConvos { get; set; } = 0;
        public int BillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> BillableTagServiceTime { get; set; }

        public int NonBillableConvos { get; set; } = 0;
        public int NonBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> NonBillableTagServiceTime { get; set; }

        public int UnknownBillableConvos { get; set; } = 0;
        public int UnknownBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> UnknownBillableTagServiceTime { get; set; }
    }
    public class ProTrendServiceTimeDTO
    {
        public int Convos { get; set; } = 0;
        public int ServiceTime { get; set; } = 0;

        public int BillableConvos { get; set; } = 0;
        public int BillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> BillableTagServiceTime { get; set; }

        public int NonBillableConvos { get; set; } = 0;
        public int NonBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> NonBillableTagServiceTime { get; set; }

        public int UnknownBillableConvos { get; set; } = 0;
        public int UnknownBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> UnknownBillableTagServiceTime { get; set; }
    }
    public class ProMonitorServiceTimeDTO
    {
        public int Convos { get; set; } = 0;
        public int ServiceTime { get; set; } = 0;

        public int BillableConvos { get; set; } = 0;
        public int BillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> BillableTagServiceTime { get; set; }

        public int NonBillableConvos { get; set; } = 0;
        public int NonBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> NonBillableTagServiceTime { get; set; }

        public int UnknownBillableConvos { get; set; } = 0;
        public int UnknownBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> UnknownBillableTagServiceTime { get; set; }
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
