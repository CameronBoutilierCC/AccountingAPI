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
using ServiceTimeAPI.Models;
using System.IO;

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
        protected dynamic AllTagsCount(Node root, List<string> tags, int i, dynamic serviceTimeDTO)
        {
            if (root.Key == "ProChart") {
                var res = tags.Where(tag => tag != "NetFlow").Where(t => root.Descendants().Any(node => (node.Key).Contains(t)));
                var count = res.Count();
                if (count > 0)
                {
                    foreach (string item in res)
                    {
                        switch (item)
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
                            //starting child tags
                            case "Batch General":
                                serviceTimeDTO.BatchGeneralConvos++;
                                serviceTimeDTO.BatchGeneralServiceTime += i;
                                break;
                            case "Batch Delete":
                                serviceTimeDTO.BatchDeleteConvos++;
                                serviceTimeDTO.BatchDeleteServiceTime += i;
                                break;
                            case "Batch Error":
                                serviceTimeDTO.BatchErrorConvos++;
                                serviceTimeDTO.BatchErrorServiceTime += i;
                                break;
                            case "Batch Inquiry":
                                serviceTimeDTO.BatchInquiryConvos++;
                                serviceTimeDTO.BatchInquiryServiceTime += i;
                                break;
                            case "Batch Rush":
                                serviceTimeDTO.BatchRushConvos++;
                                serviceTimeDTO.BatchRushServiceTime += i;
                                break;
                            case "Batch Chart Re-scan":
                                serviceTimeDTO.BatchReScanConvos++;
                                serviceTimeDTO.BatchReScanServiceTime += i;
                                break;
                            case "Chart Pickup":
                                serviceTimeDTO.ChartPickupConvos++;
                                serviceTimeDTO.ChartPickupServiceTime += i;
                                break;
                            case "Chart Audits":
                                serviceTimeDTO.ChartAuditsConvos++;
                                serviceTimeDTO.ChartAuditsServiceTime += i;
                                break;
                            case "Chart Review":
                                serviceTimeDTO.ChartReviewConvos++;
                                serviceTimeDTO.ChartReviewServiceTime += i;
                                break;
                            case "Rush Processing / Reprocessing":
                                serviceTimeDTO.RPRConvos++;
                                serviceTimeDTO.RPRServiceTime += i;
                                break;
                            case "Gas Analysis Updates":
                                serviceTimeDTO.GasAnaUpdatesConvos++;
                                serviceTimeDTO.GasAnaUpdatesServiceTime += i;
                                break;
                            case "Meter Setup":
                                serviceTimeDTO.MeterSetupConvos++;
                                serviceTimeDTO.MeterSetupServiceTime += i;
                                break;
                            case "Meter moves":
                                serviceTimeDTO.MeterMovesConvos++;
                                serviceTimeDTO.MeterMovesServiceTime += i;
                                break;
                            case "Shut-in":
                                serviceTimeDTO.ShutInConvos++;
                                serviceTimeDTO.ShutInServiceTime += i;
                                break;
                            case "Reading sheet/manual entries":
                                serviceTimeDTO.RSMEConvos++;
                                serviceTimeDTO.RSMEServiceTime += i;
                                break;
                            case "Read sheets":
                                serviceTimeDTO.ReadSheetConvos++;
                                serviceTimeDTO.ReadSheetServiceTime += i;
                                break;
                            case "ProChart Report":
                                serviceTimeDTO.ProChartReportConvos++;
                                serviceTimeDTO.ProChartReportServiceTime += i;
                                break;
                            case "New scanner request":
                                serviceTimeDTO.NewScanReqConvos++;
                                serviceTimeDTO.NewScanReqServiceTime += i;
                                break;
                            case "Scanner Parts":
                                serviceTimeDTO.ScannerPartsConvos++;
                                serviceTimeDTO.ScannerPartsServiceTime += i;
                                break;
                            case "Shipping scanner":
                                serviceTimeDTO.ShippingScannerConvos++;
                                serviceTimeDTO.ShippingScannerServiceTime += i;
                                break;
                            case "Add Client to In-house Scanner":
                                serviceTimeDTO.ACISConvos++;
                                serviceTimeDTO.ACISServiceTime += i;
                                break;
                            case "Scanner Troubleshooting":
                                serviceTimeDTO.STConvos++;
                                serviceTimeDTO.STServiceTime += i;
                                break;
                            case "Application Access":
                                serviceTimeDTO.AppAccessConvos++;
                                serviceTimeDTO.AppAccessServiceTime += i;
                                break;
                            case "Client Deactivation":
                                serviceTimeDTO.ClientDeactConvos++;
                                serviceTimeDTO.ClientDeactServiceTime += i;
                                break;
                            case "Existing client":
                                serviceTimeDTO.ExistclientConvos++;
                                serviceTimeDTO.ExistclientServiceTime += i;
                                break;
                            case "New Client Activation":
                                serviceTimeDTO.NewClientActConvos++;
                                serviceTimeDTO.NewClientActServiceTime += i;
                                break;
                            case "Temporary Password Request":
                                serviceTimeDTO.TempPwReqConvos++;
                                serviceTimeDTO.TempPwReqServiceTime += i;
                                break;
                            // starting grandchild tags
                            case "Chart Summary Report":
                                serviceTimeDTO.CSRConvos++;
                                serviceTimeDTO.CSRServiceTime += i;
                                break;
                            case "Chart Summary Report To Excel":
                                serviceTimeDTO.CSREConvos++;
                                serviceTimeDTO.CSREServiceTime += i;
                                break;
                            case "Chart Summary Report with Test Summary":
                                serviceTimeDTO.CSRTSConvos++;
                                serviceTimeDTO.CSRTSServiceTime += i;
                                break;
                            case "Custom Reports":
                                serviceTimeDTO.CustomReportsConvos++;
                                serviceTimeDTO.CustomReportsServiceTime += i;
                                break;
                            case "Chart Evaluation":
                                serviceTimeDTO.ChartEvalConvos++;
                                serviceTimeDTO.ChartEvalServiceTime += i;
                                break;
                            case "Chart Status":
                                serviceTimeDTO.ChartStatusConvos++;
                                serviceTimeDTO.ChartStatusServiceTime += i;
                                break;
                            case "Delivery Files":
                                serviceTimeDTO.DeliveryFilesConvos++;
                                serviceTimeDTO.DeliveryFilesServiceTime += i;
                                break;
                            case "EDI":
                                serviceTimeDTO.EDIConvos++;
                                serviceTimeDTO.EDIServiceTime += i;
                                break;
                            case "Estimates":
                                serviceTimeDTO.EstimatesConvos++;
                                serviceTimeDTO.EstimatesServiceTime += i;
                                break;
                            case "Gasmet measured":
                                serviceTimeDTO.GasmetMeasuredConvos++;
                                serviceTimeDTO.GasmetMeasuredServiceTime += i;
                                break;
                            case "Gasmet tested":
                                serviceTimeDTO.GasmetTestedConvos++;
                                serviceTimeDTO.GasmetTestedServiceTime += i;
                                break;
                            case "Meter Data Extract":
                                serviceTimeDTO.MDEConvos++;
                                serviceTimeDTO.MDEServiceTime += i;
                                break;
                            case "Meter Equipment Change Report":
                                serviceTimeDTO.MECRConvos++;
                                serviceTimeDTO.MECRServiceTime += i;
                                break;
                            case "Prism Measured":
                                serviceTimeDTO.PrismMeasuredConvos++;
                                serviceTimeDTO.PrismMeasuredServiceTime += i;
                                break;
                            case "Production Summary":
                                serviceTimeDTO.ProductionSummaryConvos++;
                                serviceTimeDTO.ProductionSummaryServiceTime += i;
                                break;
                            case "Receipt Files":
                                serviceTimeDTO.ReceiptFilesConvos++;
                                serviceTimeDTO.ReceiptFilesServiceTime += i;
                                break;
                            case "Updates":
                                serviceTimeDTO.UpdatesConvos++;
                                serviceTimeDTO.UpdatesServiceTime += i;
                                break;
                            case "Variance":
                                serviceTimeDTO.VarianceConvos++;
                                serviceTimeDTO.VarianceServiceTime += i;
                                break;
                            case "Volume Detailed Reports":
                                serviceTimeDTO.VDRConvos++;
                                serviceTimeDTO.VDRServiceTime += i;
                                break;
                            case "Volume Summary":
                                serviceTimeDTO.VolumeSummaryConvos++;
                                serviceTimeDTO.VolumeSummaryServiceTime += i;
                                break;
                            case "Weekly Cart Report":
                                serviceTimeDTO.WCRConvos++;
                                serviceTimeDTO.WCRServiceTime += i;
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
                var res = tags.Where(tag => tag != "ProTrend").Where(t => root.Descendants().Any(node => (node.Key).Contains(t)));
                var count = res.Count();
                if (count > 0)
                {
                    foreach (string item in res)
                    {
                        switch (item)
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
                var res = tags.Where(tag => tag != "ProMonitor").Where(t => root.Descendants().Any(node => (node.Key).Contains(t)));
                var count = res.Count();
                if (count > 0)
                {
                    foreach (string item in res)
                    {
                        switch (item)
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
                        new Node{ Key = "Batch",Children = new List<Node>{
                                    new Node{  Key = "Batch General" },
                                    new Node{  Key = "Batch Delete" },
                                    new Node{  Key = "Batch Error" },
                                    new Node{  Key = "Batch Inquiry" },
                                    new Node{  Key = "Batch Rush" },
                                    new Node{  Key = "Batch Chart Re-scan" },
                                    new Node{  Key = "Chart Pickup" }
                             }
                         },
                        new Node{ Key = "Chart Processing",Children = new List<Node>{
                                    new Node{  Key = "Chart Audits" },
                                    new Node{  Key = "Chart Review" },
                                    new Node{  Key = "Rush Processing / Reprocessing" }
                             }},
                        new Node{ Key = "Development Request"},
                        new Node{ Key = "Chart Processing"},
                        new Node{ Key = "FTP"},
                        new Node{ Key = "Gas Analysis",Children = new List<Node>{
                                    new Node{  Key = "Gas Analysis Updates" }
                        } },
                        new Node{ Key = "Label Request"},
                        new Node{ Key = "Meter",Children = new List<Node>{
                                    new Node{  Key = "Meter Setup" },
                                     new Node{  Key = "Meter moves" },
                                      new Node{  Key = "Shut-in" }
                        } },
                        new Node{ Key = "Reading Slips",Children = new List<Node>{
                                    new Node{  Key = "Reading sheet/manual entries" },
                                     new Node{  Key = "Read sheets" }
                        } },
                        new Node{ Key = "Reports",Children = new List<Node>{
                                    new Node{  Key = "ProChart Report",Children = new List<Node>{
                                        new Node{  Key = "Chart Summary Report" },
                                        new Node{  Key = "Chart Summary Report To Excel" },
                                        new Node{  Key = "Chart Summary Report with Test Summary" },
                                        new Node{  Key = "Custom Reports" },
                                        new Node{  Key = "Chart Evaluation" },
                                        new Node{  Key = "Chart Status" },
                                        new Node{  Key = "Delivery Files" },
                                        new Node{  Key = "EDI" },
                                        new Node{  Key = "Estimates" },
                                        new Node{  Key = "Gasmet measured" },
                                        new Node{  Key = "Gasmet tested" },
                                        new Node{  Key = "Meter Data Extract" },
                                        new Node{  Key = "Meter Equipment Change Report" },
                                        new Node{  Key = "Prism Measured" },
                                        new Node{  Key = "Production Summary" },
                                        new Node{  Key = "Receipt Files" },
                                        new Node{  Key = "Updates" },
                                        new Node{  Key = "Variance" },
                                        new Node{  Key = "Volume Detailed Reports" },
                                        new Node{  Key = "Volume Summary" },
                                        new Node{  Key = "Weekly Cart Report" }
                                    } },
                        } },
                        new Node{ Key = "Scanner",Children = new List<Node>{
                                    new Node{  Key = "New scanner request" },
                                    new Node{  Key = "Scanner Parts" },
                                    new Node{  Key = "Shipping scanner" },
                                    new Node{  Key = "Add Client to In-house Scanner" },
                                    new Node{  Key = "Scanner Troubleshooting" }
                        } },
                        new Node{ Key = "User Setup/Removal/Updates",Children = new List<Node>{
                                    new Node{  Key = "Application Access" },
                                    new Node{  Key = "Client Deactivation" },
                                    new Node{  Key = "Existing client" },
                                    new Node{  Key = "New Client Activation" },
                                    new Node{  Key = "Temporary Password Request" }
                        } },
                    }
                };
                AllTagsCount(root, tags, i, serviceTimeDTO);
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
                AllTagsCount(root, tags, i, serviceTimeDTO);
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
                AllTagsCount(root, tags, i, serviceTimeDTO);
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
                AllTagsCount(root, tags, i, serviceTimeDTO);
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
