using Common.Entities;
using Kuscotopia.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Kuscotopia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OverseerController
    {
        private readonly QueueService queueService;

        public OverseerController(QueueService queueService)
        {
            this.queueService = queueService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] int workCount)
        {

            //Validate workcount is from 1 to 10
            if (workCount < 1 || workCount > 10)
            {
                return new JsonResult(new { message = " Invalid Request!" })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }

            //Accept a number between 1 and 10, how many messages you should send to the queue
            await queueService.QueueWorkAsync(workCount);

            return new JsonResult(new { message = "Work Queued Successfully!" })
            {
                StatusCode = (int)HttpStatusCode.OK,
            };
        }
    } 
}
