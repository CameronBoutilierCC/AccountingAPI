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

                //while (true)
                //{
                //    System.Threading.Thread.Sleep(200);

                //    res = await httpClient.GetAsync($"exports/{resObj.id}");
                //    res.EnsureSuccessStatusCode();
                //    content = await res.Content.ReadAsStringAsync();
                //    resObj = JsonSerializer.Deserialize<ExportResponse>(content);
                //    if (resObj.url != null)
                //        break;
                //}

                //using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, resObj.url))
                //{
                //    using (Stream contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(), stream = new FileStream(@"C:\Users\tim.su.ES\Desktop\Export.csv", FileMode.Create, FileAccess.Write, FileShare.None, 10000, true))
                //    {
                //        await contentStream.CopyToAsync(stream);
                //    }
                //}
                tally = ProcessCSV(@"C:\Users\tim.su.ES\Desktop\Export.csv");
            }
            return Ok(tally);
        }
        protected dynamic AAT(List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (tags.Contains("Acquisition / Asset Transfer"))
            {
                serviceTimeDTO.AATConvos++;
                serviceTimeDTO.AATServiceTime += i;
            }
            return serviceTimeDTO;
        }
        protected dynamic Allocations(List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (tags.Contains("Allocations"))
            {
                serviceTimeDTO.AllocationsConvos++;
                serviceTimeDTO.AllocationsServiceTime += i;
            }
            return serviceTimeDTO;
        }
        protected dynamic Batch(List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (tags.Contains("Batch"))
            {
                serviceTimeDTO.BatchConvos++;
                serviceTimeDTO.BatchServiceTime += i;
            }
            return serviceTimeDTO;
        }
        protected dynamic ChartProcess(List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (tags.Contains("Chart Processing"))
            {
                serviceTimeDTO.ChartProcessConvos++;
                serviceTimeDTO.ChartProcessServiceTime += i;
            }
            return serviceTimeDTO;
        }
        protected dynamic DevReq(List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (tags.Contains("Development Request"))
            {
                serviceTimeDTO.DevReqConvos++;
                serviceTimeDTO.DevReqServiceTime += i;
            }
            return serviceTimeDTO;
        }
        protected dynamic FTP(List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (tags.Contains("FTP"))
            {
                serviceTimeDTO.FTPConvos++;
                serviceTimeDTO.FTPServiceTime += i;
            }
            return serviceTimeDTO;
        }
        protected dynamic GasAna(List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (tags.Contains("Gas Analysis"))
            {
                serviceTimeDTO.GasAnaConvos++;
                serviceTimeDTO.GasAnaServiceTime += i;
            }
            return serviceTimeDTO;
        }
        protected dynamic LabelRequest(List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (tags.Contains("Label Request"))
            {
                serviceTimeDTO.LabelRequestConvos++;
                serviceTimeDTO.LabelRequestServiceTime += i;
            }
            serviceTimeDTO.ServiceTime =
                serviceTimeDTO.NonBillableServiceTime
                + serviceTimeDTO.BillableServiceTime
                + serviceTimeDTO.UnknownBillableServiceTime;
            return serviceTimeDTO;
        }
        protected dynamic Meter(List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (tags.Contains("Meter"))
            {
                serviceTimeDTO.MeterConvos++;
                serviceTimeDTO.MeterServiceTime += i;
            }
            return serviceTimeDTO;
        }
        protected dynamic ReadSlips(List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (tags.Contains("Reading Slips"))
            {
                serviceTimeDTO.ReadSlipsConvos++;
                serviceTimeDTO.ReadSlipsServiceTime += i;
            }
            return serviceTimeDTO;
        }
        protected dynamic Reports(List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (tags.Contains("Reports"))
            {
                serviceTimeDTO.ReportsConvos++;
                serviceTimeDTO.ReportsServiceTime += i;
            }
            return serviceTimeDTO;
        }
        protected dynamic Scanner(List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (tags.Contains("Scanner"))
            {
                serviceTimeDTO.ScannerConvos++;
                serviceTimeDTO.ScannerServiceTime += i;
            }
            return serviceTimeDTO;
        }
        protected dynamic USRU(List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (tags.Contains("User Setup/Removal/Updates"))
            {
                serviceTimeDTO.USRUConvos++;
                serviceTimeDTO.USRUServiceTime += i;
            }
            return serviceTimeDTO;
        }
        protected dynamic NoParents_PC(List<string> tags, int i, dynamic serviceTimeDTO)
        {
            List<string> parentTags = new List<string> { 
            "Acquisition / Asset Transfer",
            "Allocations",
            "Batch",
            "Chart Processing",
            "Development Request",
            "FTP",
            "Gas Analysis",
            "Label Request",
            "Meter",
            "Reading Slips",
            "Reports",
            "Scanner",
            "User Setup/Removal/Updates"
            };

            if (parentTags.Any(x => tags.Any(y => y == x)))
            {}
            else
            {
                serviceTimeDTO.NoParentConvos++;
                serviceTimeDTO.NoParentServiceTime += i;
            }
            return serviceTimeDTO;
        }
        protected dynamic Bill(List<string> tags, int i, dynamic serviceTimeDTO) {
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
            return serviceTimeDTO;
        }
        private ServiceTimeTally TallyResult(List<string> tags, int i, ServiceTimeTally tally) {
            dynamic serviceTimeDTO = tally.ProchartServiceTimeDTO;
            if (tags.Contains("ProChart"))
            {
                serviceTimeDTO = tally.ProchartServiceTimeDTO;
                // convos' sum of parent tags + noParent tags  would be more than total convos
                // because some row would have more than 1 parent tags
                // also some row dont have parent tag but only child tag, e.g no'Batch' but Batch General exist in Prochart, it's counted as nonparent
                // some parent tag appear, and their children appear in other places too. e.g parent tag Chart Processing,children tag Chart Review appeared in various places
                // above 2 facts cause nonparent count would be larger than actual and some parent tag would be less than actual
                AAT(tags, i, serviceTimeDTO);
                Allocations(tags, i, serviceTimeDTO);
                Batch(tags, i, serviceTimeDTO);
                ChartProcess(tags, i, serviceTimeDTO);
                DevReq(tags, i, serviceTimeDTO);
                FTP(tags, i, serviceTimeDTO);
                GasAna(tags, i, serviceTimeDTO);
                LabelRequest(tags, i, serviceTimeDTO);
                Meter(tags, i, serviceTimeDTO);
                ReadSlips(tags, i, serviceTimeDTO);
                Reports(tags, i, serviceTimeDTO);
                Scanner(tags, i, serviceTimeDTO);
                USRU(tags, i, serviceTimeDTO);
                NoParents_PC(tags, i, serviceTimeDTO);
                Bill(tags, i, serviceTimeDTO);
            }
            else if (tags.Contains("NetFlow"))
            {
                serviceTimeDTO = tally.NetflowServiceTimeDTO;
                Bill(tags, i, serviceTimeDTO);
            }
            else if (tags.Contains("ProTrend"))
            {
                serviceTimeDTO = tally.ProtrendServiceTimeDTO;
                Bill(tags, i, serviceTimeDTO);
            }
            else if (tags.Contains("ProMonitor"))
            {
                serviceTimeDTO = tally.PromonitorServiceTimeDTO;
                Bill(tags, i, serviceTimeDTO);
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
            tally.ProchartServiceTimeDTO = new ProChartServiceTimeDTO {};
            tally.NetflowServiceTimeDTO = new NetFlowServiceTimeDTO {};
            tally.ProtrendServiceTimeDTO = new ProTrendServiceTimeDTO {};
            tally.PromonitorServiceTimeDTO = new ProMonitorServiceTimeDTO {};
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

        public int AATConvos { get; set; } = 0;
        public int AATServiceTime { get; set; } = 0;
        public int AllocationsConvos { get; set; } = 0;
        public int AllocationsServiceTime { get; set; } = 0;
        public int BatchConvos { get; set; } = 0;
        public int BatchServiceTime { get; set; } = 0;
        public int ChartProcessConvos { get; set; } = 0;
        public int ChartProcessServiceTime { get; set; } = 0;
        public int DevReqConvos { get; set; } = 0;
        public int DevReqServiceTime { get; set; } = 0;
        public int FTPConvos { get; set; } = 0;
        public int FTPServiceTime { get; set; } = 0;
        public int GasAnaConvos { get; set; } = 0;
        public int GasAnaServiceTime { get; set; } = 0;
        public int LabelRequestConvos { get; set; } = 0;
        public int LabelRequestServiceTime { get; set; } = 0;
        public int MeterConvos { get; set; } = 0;
        public int MeterServiceTime { get; set; } = 0;
        public int ReadSlipsConvos { get; set; } = 0;
        public int ReadSlipsServiceTime { get; set; } = 0;
        public int ReportsConvos { get; set; } = 0;
        public int ReportsServiceTime { get; set; } = 0;
        public int ScannerConvos { get; set; } = 0;
        public int ScannerServiceTime { get; set; } = 0;
        public int USRUConvos { get; set; } = 0;
        public int USRUServiceTime { get; set; } = 0;
        public int NoParentConvos { get; set; } = 0;
        public int NoParentServiceTime { get; set; } = 0;
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
