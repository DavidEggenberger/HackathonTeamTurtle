using Azure.AI.TextAnalytics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Azure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextAnalysisController : ControllerBase
    {
        private TextAnalyticsClient TextAnalyticsClient;
        public TextAnalysisController(TextAnalyticsClient textAnalyticsClient)
        {
            TextAnalyticsClient = textAnalyticsClient;
        }

        [HttpPost]
        public async Task<ActionResult<Response>> AnalyzeText(TextDTO textDto)
        {
            string s = await new HttpClient().GetStringAsync(textDto.WebPageURI);
            var batchInput = new List<string>
            {
                s
            };

            TextAnalyticsActions actions = new TextAnalyticsActions()
            {
                ExtractKeyPhrasesActions = new List<ExtractKeyPhrasesAction>() { new ExtractKeyPhrasesAction()}
            };

            AnalyzeActionsOperation operation = await TextAnalyticsClient.StartAnalyzeActionsAsync(batchInput, actions);
            await operation.WaitForCompletionAsync();
            await foreach (AnalyzeActionsResult documentsInPage in operation.Value)
            {
            }
            return Ok();
        }

        public class TextDTO
        {
            public string WebPageURI { get; set; }
        }
        public class Response
        {
            public string Description { get; set; }
        }
    }
}
