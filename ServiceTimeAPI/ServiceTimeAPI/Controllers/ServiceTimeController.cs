using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections;
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
        protected dynamic ParentTagsCount(Node root, List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (root.Key == "ProChart") {
                var res = root.Descendants().Where(node => node.Key != "ProChart").Where(node => tags.Any(tag => (node.Key).Contains(tag)));
                var count = res.Count();
                if (count > 0)
                {
                    foreach (Node item in res)
                    {
                        switch (item.Key)
                        {
                            case "User Setup/Removal":
                                serviceTimeDTO.AATConvos++;
                                serviceTimeDTO.AATServiceTime += i;
                                break;
                            case "Allocations":
                                serviceTimeDTO.AllocationsConvos++;
                                serviceTimeDTO.AllocationsServiceTime += i;
                                break;
                            case "Batch":
                                serviceTimeDTO.BatchConvos++;
                                serviceTimeDTO.BatchServiceTime += i;
                                break;
                            case "Chart Processing":
                                serviceTimeDTO.ChartProcessConvos++;
                                serviceTimeDTO.ChartProcessServiceTime += i;
                                break;
                            case "Development Request":
                                serviceTimeDTO.DevReqConvos++;
                                serviceTimeDTO.DevReqServiceTime += i;
                                break;
                            case "FTP":
                                serviceTimeDTO.FTPConvos++;
                                serviceTimeDTO.FTPServiceTime += i;
                                break;
                            case "Gas Analysis":
                                serviceTimeDTO.GasAnaConvos++;
                                serviceTimeDTO.GasAnaServiceTime += i;
                                break;
                            case "Label Request":
                                serviceTimeDTO.LabelRequestConvos++;
                                serviceTimeDTO.LabelRequestServiceTime += i;
                                break;
                            case "Meter":
                                serviceTimeDTO.MeterConvos++;
                                serviceTimeDTO.MeterServiceTime += i;
                                break;
                            case "Reading Slips":
                                serviceTimeDTO.ReadSlipsConvos++;
                                serviceTimeDTO.ReadSlipsServiceTime += i;
                                break;
                            case "Reports":
                                serviceTimeDTO.ReportsConvos++;
                                serviceTimeDTO.ReportsServiceTime += i;
                                break;
                            case "Scanner":
                                serviceTimeDTO.ScannerConvos++;
                                serviceTimeDTO.ScannerServiceTime += i;
                                break;
                            case "User Setup/Removal/Updates":
                                serviceTimeDTO.USRUConvos++;
                                serviceTimeDTO.USRUServiceTime += i;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    serviceTimeDTO.NoParentConvos++;
                    serviceTimeDTO.NoParentServiceTime += i;
                }
            }
            else if (root.Key == "NetFlow")
            {
                // comment method would cause nftsConvos in netflow and its noParentConvos makeno sense
                // var res = root.Descendants().Where(node => node.Key != "NetFlow").Where(node => tags.Any(tag => (node.Key).Contains(tag)));
                var res = tags.Where(tag => tag != "NetFlow").Where(t => root.Descendants().Any(node => (node.Key).Contains(t)));
                var count = res.Count();
                if (count > 0)
                {
                    foreach (string item in res)
                    {
                        switch (item)
                        {
                            case "User Setup/Removal":
                                serviceTimeDTO.USRConvos++;
                                serviceTimeDTO.AddRegConvos += i;
                                break;
                            case "Add Registers":
                                serviceTimeDTO.AddRegConvos++;
                                serviceTimeDTO.AddRegServiceTime += i;
                                break;
                            case "Alarm Management Support":
                                serviceTimeDTO.AMSConvos++;
                                serviceTimeDTO.AMSServiceTime += i;
                                break;
                            case "App Inquiry":
                                serviceTimeDTO.AppInqConvos++;
                                serviceTimeDTO.AppInqServiceTime += i;
                                break;
                            case "Acquisition/Asset Transfer":
                                serviceTimeDTO.AATConvos++;
                                serviceTimeDTO.AATServiceTime += i;
                                break;
                            case "Comms":
                                serviceTimeDTO.CommsConvos++;
                                serviceTimeDTO.CommsServiceTime += i;
                                break;
                            case "Commissioning":
                                serviceTimeDTO.CommissionConvos++;
                                serviceTimeDTO.CommissionServiceTime += i;
                                break;
                            case "Contract":
                                serviceTimeDTO.ContractConvos++;
                                serviceTimeDTO.ContractServiceTime += i;
                                break;
                            case "Decommission":
                                serviceTimeDTO.DecommissionConvos++;
                                serviceTimeDTO.DecommissionServiceTime += i;
                                break;
                            case "Modems":
                                serviceTimeDTO.ModemsConvos++;
                                serviceTimeDTO.ModemsServiceTime += i;
                                break;
                            case "NetFlow Troubleshooting":
                                serviceTimeDTO.NFTSConvos++;
                                serviceTimeDTO.NFTSServiceTime += i;
                                break;
                            case "Reports":
                                serviceTimeDTO.ReportsConvos++;
                                serviceTimeDTO.ReportsServiceTime += i;
                                break;
                            case "Development Request":
                                serviceTimeDTO.DevReqConvos++;
                                serviceTimeDTO.DevReqServiceTime += i;
                                break;
                            case "Update(s)":
                                serviceTimeDTO.UpdateConvos++;
                                serviceTimeDTO.UpdateServiceTime += i;
                                break;
                            case "Implementation":
                                serviceTimeDTO.ImpConvos++;
                                serviceTimeDTO.ImpServiceTime += i;
                                break;
                            case "Meters":
                                serviceTimeDTO.MetersConvos++;
                                serviceTimeDTO.MetersServiceTime += i;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    serviceTimeDTO.NoParentConvos++;
                    serviceTimeDTO.NoParentServiceTime += i;
                }
            }
            else if (root.Key == "ProTrend")
            {
                var res = root.Descendants().Where(node => node.Key != "ProTrend").Where(node => tags.Any(tag => (node.Key).Contains(tag)));
                var count = res.Count();
                if (count > 0)
                {
                    foreach (Node item in res)
                    {
                        switch (item.Key)
                        {
                            case "Existing Asset Update":
                                serviceTimeDTO.EAUConvos++;
                                serviceTimeDTO.EAUServiceTime += i;
                                break;
                            case "Client Meeting/Demo":
                                serviceTimeDTO.CMDConvos++;
                                serviceTimeDTO.CMDServiceTime += i;
                                break;
                            case "Client Support":
                                serviceTimeDTO.ClientSupConvos++;
                                serviceTimeDTO.ClientSupServiceTime += i;
                                break;
                            case "Data Export":
                                serviceTimeDTO.DataExpConvos++;
                                serviceTimeDTO.DataExpServiceTime += i;
                                break;
                            case "Data Import":
                                serviceTimeDTO.DataImpConvos++;
                                serviceTimeDTO.DataImpServiceTime += i;
                                break;
                            case "Data Report Generation":
                                serviceTimeDTO.DRGConvos++;
                                serviceTimeDTO.DRGServiceTime += i;
                                break;
                            case "Data Validation":
                                serviceTimeDTO.DataValConvos++;
                                serviceTimeDTO.DataValServiceTime += i;
                                break;
                            case "Development Request":
                                serviceTimeDTO.DevReqConvos++;
                                serviceTimeDTO.DevReqServiceTime += i;
                                break;
                            case "Bug Fix TFS":
                                serviceTimeDTO.BFTConvos++;
                                serviceTimeDTO.BFTServiceTime += i;
                                break;
                            case "Implementation":
                                serviceTimeDTO.ImpConvos++;
                                serviceTimeDTO.ImpServiceTime += i;
                                break;
                            case "PAS File Submission":
                                serviceTimeDTO.PFSConvos++;
                                serviceTimeDTO.PFSServiceTime += i;
                                break;
                            case "Scheduling":
                                serviceTimeDTO.SchConvos++;
                                serviceTimeDTO.SchServiceTime += i;
                                break;
                            case "User Setup/Removal/Updates":
                                serviceTimeDTO.USRUConvos++;
                                serviceTimeDTO.USRUServiceTime += i;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    serviceTimeDTO.NoParentConvos++;
                    serviceTimeDTO.NoParentServiceTime += i;
                }
            }
            else if (root.Key == "ProMonitor")
            {
                var res = root.Descendants().Where(node => node.Key != "ProMonitor").Where(node => tags.Any(tag => (node.Key).Contains(tag)));
                var count = res.Count();
                if (count > 0)
                {
                    foreach (Node item in res)
                    {
                        switch (item.Key)
                        {
                            case "Data Import":
                                serviceTimeDTO.DataImpConvos++;
                                serviceTimeDTO.DataImpServiceTime += i;
                                break;
                            case "User Setup/Removal/Updates":
                                serviceTimeDTO.USRUConvos++;
                                serviceTimeDTO.USRUServiceTime += i;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    serviceTimeDTO.NoParentConvos++;
                    serviceTimeDTO.NoParentServiceTime += i;
                }
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
        private ServiceTimeTally StartTally(List<string> tags, int i, ServiceTimeTally tally) {
            dynamic serviceTimeDTO = tally.ProchartServiceTimeDTO;
            if (tags.Contains("ProChart"))
            {
                serviceTimeDTO = tally.ProchartServiceTimeDTO;
                Node root = new Node
                {
                    Key = "ProChart",
                    Children = new List<Node>{
                        new Node{ Key = "Acquisition / Asset Transfer"},
                        new Node{ Key = "Allocations"},
                        new Node{ Key = "Batch"},
                        new Node{ Key = "Chart Processing"},
                        new Node{ Key = "Development Request"},
                        new Node{ Key = "Chart Processing"},
                        new Node{ Key = "FTP"},
                        new Node{ Key = "Gas Analysis"},
                        new Node{ Key = "Label Request"},
                        new Node{ Key = "Meter"},
                        new Node{ Key = "Reading Slips"},
                        new Node{ Key = "Reports"},
                        new Node{ Key = "Scanner"},
                        new Node{ Key = "User Setup/Removal/Updates"},
                    }
                };
                ParentTagsCount(root, tags, i, serviceTimeDTO);
                Bill(tags, i, serviceTimeDTO);
            }
            else if (tags.Contains("NetFlow"))
            {
                serviceTimeDTO = tally.NetflowServiceTimeDTO;
                Node root = new Node
                {
                    Key = "NetFlow",
                    Children = new List<Node>{
                        new Node{ Key = "User Setup/Removal"},
                        new Node{ Key = "Add Registers"},
                        new Node{ Key = "Alarm Management Support"},
                        new Node{ Key = "App Inquiry"},
                        new Node{ Key = "Acquisition/Asset Transfer"},
                        new Node{ Key = "Comms"},
                        new Node{ Key = "Commissioning"},
                        new Node{ Key = "Contract"},
                        new Node{ Key = "Decommission"},
                        new Node{ Key = "Modems"},
                        new Node{ Key = "NetFlow Troubleshooting"},
                        new Node{ Key = "Reports"},
                        new Node{ Key = "Development Request"},
                        new Node{ Key = "Update(s)"},
                        new Node{ Key = "Implementation"},
                        new Node{ Key = "Meters"},
                    }
                };
                ParentTagsCount(root, tags, i, serviceTimeDTO);
                Bill(tags, i, serviceTimeDTO);
            }
            else if (tags.Contains("ProTrend"))
            {
                serviceTimeDTO = tally.ProtrendServiceTimeDTO;
                Node root = new Node
                {
                    Key = "ProTrend",
                    Children = new List<Node>{
                        new Node{ Key = "Existing Asset Update"},
                        new Node{ Key = "Client Meeting/Demo"},
                        new Node{ Key = "Client Support"},
                        new Node{ Key = "Data Export"},
                        new Node{ Key = "Data Import"},
                        new Node{ Key = "Data Report Generation"},
                        new Node{ Key = "Data Validation"},
                        new Node{ Key = "Development Request"},
                        new Node{ Key = "Bug Fix TFS"},
                        new Node{ Key = "Implementation"},
                        new Node{ Key = "PAS File Submission"},
                        new Node{ Key = "Scheduling"},
                        new Node{ Key = "User Setup/Removal/Updates"}
                    }
                };
                ParentTagsCount(root, tags, i, serviceTimeDTO);
                Bill(tags, i, serviceTimeDTO);
            }
            else if (tags.Contains("ProMonitor"))
            {
                serviceTimeDTO = tally.PromonitorServiceTimeDTO;
                Node root = new Node
                {
                    Key = "ProMonitor",
                    Children = new List<Node>{
                        new Node{ Key = "User Setup/Removal/Updates"},
                        new Node{ Key = "Data Import"}
                    }
                };
                ParentTagsCount(root, tags, i, serviceTimeDTO);
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
                StartTally(tags, i, tally);
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

        public int USRConvos { get; set; } = 0;
        public int USRServiceTime { get; set; } = 0;
        public int AddRegConvos { get; set; } = 0;
        public int AddRegServiceTime { get; set; } = 0;
        public int AMSConvos { get; set; } = 0;
        public int AMSServiceTime { get; set; } = 0;
        public int AppInqConvos { get; set; } = 0;
        public int AppInqServiceTime { get; set; } = 0;
        public int AATConvos { get; set; } = 0;
        public int AATServiceTime { get; set; } = 0;
        public int CommsConvos { get; set; } = 0;
        public int CommsServiceTime { get; set; } = 0;
        public int CommissionConvos { get; set; } = 0;
        public int CommissionServiceTime { get; set; } = 0;
        public int ContractConvos { get; set; } = 0;
        public int ContractServiceTime { get; set; } = 0;
        public int DecommissionConvos { get; set; } = 0;
        public int DecommissionServiceTime { get; set; } = 0;
        public int ModemsConvos { get; set; } = 0;
        public int ModemsServiceTime { get; set; } = 0;
        public int NFTSConvos { get; set; } = 0;
        public int NFTSServiceTime { get; set; } = 0;
        public int ReportsConvos { get; set; } = 0;
        public int ReportsServiceTime { get; set; } = 0;
        public int DevReqConvos { get; set; } = 0;
        public int DevReqServiceTime { get; set; } = 0;
        public int UpdateConvos { get; set; } = 0;
        public int UpdateServiceTime { get; set; } = 0;
        public int ImpConvos { get; set; } = 0;
        public int ImpServiceTime { get; set; } = 0;
        public int MetersConvos { get; set; } = 0;
        public int MetersServiceTime { get; set; } = 0;
        public int NoParentConvos { get; set; } = 0;
        public int NoParentServiceTime { get; set; } = 0;
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
        public int EAUConvos { get; set; } = 0;
        public int EAUServiceTime { get; set; } = 0;
        public int CMDConvos { get; set; } = 0;
        public int CMDServiceTime { get; set; } = 0;
        public int ClientSupConvos { get; set; } = 0;
        public int ClientSupServiceTime { get; set; } = 0;
        public int DataExpConvos { get; set; } = 0;
        public int DataExpServiceTime { get; set; } = 0;
        public int DataImpConvos { get; set; } = 0;
        public int DataImpServiceTime { get; set; } = 0;
        public int DRGConvos { get; set; } = 0;
        public int DRGServiceTime { get; set; } = 0;
        public int DataValConvos { get; set; } = 0;
        public int DataValServiceTime { get; set; } = 0;
        public int DevReqConvos { get; set; } = 0;
        public int DevReqServiceTime { get; set; } = 0;
        public int BFTConvos { get; set; } = 0;
        public int BFTServiceTime { get; set; } = 0;
        public int ImpConvos { get; set; } = 0;
        public int ImpServiceTime { get; set; } = 0;
        public int PFSConvos { get; set; } = 0;
        public int PFSServiceTime { get; set; } = 0;
        public int SchConvos { get; set; } = 0;
        public int SchServiceTime { get; set; } = 0;
        public int USRUConvos { get; set; } = 0;
        public int USRUServiceTime { get; set; } = 0;
        public int NoParentConvos { get; set; } = 0;
        public int NoParentServiceTime { get; set; } = 0;
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
        public int USRUConvos { get; set; } = 0;
        public int USRUServiceTime { get; set; } = 0;
        public int DataImpConvos { get; set; } = 0;
        public int DataImpServiceTime { get; set; } = 0;
        public int NoParentConvos { get; set; } = 0;
        public int NoParentServiceTime { get; set; } = 0;
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
