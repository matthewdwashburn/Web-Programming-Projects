using System.Collections.Generic;
using System.Net;
using DependencyInjection.Filters;
using DependencyInjection.Services;
using Microsoft.AspNetCore.Mvc;

namespace DependencyInjection.Controllers
{
    /*
	 * Main API controller for managing a list of hobbit names.
	 * Applies stopwatch and request ID filters to all actions.
	 */
    [Route("api/[controller]")]
    [TypeFilter(typeof(StopwatchFilter))]
    [TypeFilter(typeof(RequestIdFilter))]
    public class HobbitsController : Controller
    {
        // In-memory database for storing hobbit names
        private readonly MemoryDatabase database;

        // Service to track execution times
        private readonly StopwatchService watchService;

        // Logger for request or action info
        private readonly ILogger logger;

        // Constructor injects dependencies
        public HobbitsController(MemoryDatabase database, StopwatchService watchService, ILogger logger)
        {
            this.database = database;
            this.watchService = watchService;
            this.logger = logger;
        }

        // GET
        [HttpGet]
        public IEnumerable<string> Get()
        {
            watchService.Lap("Controller"); // Record a lap for timing
            return database.GetData("Hobbit"); // Return all stored hobbit names
        }

        // POST
        [HttpPost]
        public string Post([FromQuery] string hobbit)
        {
            watchService.Lap("Controller"); // Record a lap for timing
            database.AddString("Hobbit", hobbit); // Add the new hobbit name
            return hobbit; // Return the added name
        }

        // DELETE
        [HttpDelete]
        public IActionResult Delete()
        {
            watchService.Lap("Controller"); // Record a lap for timing
            database.DeleteAll(); // Remove all hobbit names
            return StatusCode((int)HttpStatusCode.NoContent); // Return 204 No Content
        }
    }
}
