using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

            using (var httpClient = HttpClientFactory.CreateClient("frontappexport"))
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
                    System.Threading.Thread.Sleep(10000);

                    res = await httpClient.GetAsync($"exports/{resObj.id}");
                    res.EnsureSuccessStatusCode();
                    content = await res.Content.ReadAsStringAsync();
                    resObj = JsonSerializer.Deserialize<ExportResponse>(content);
                    if (resObj.progress == 100)
                        break;
                }
                content = resObj.url;
            }

            return Ok(content);
        }
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
