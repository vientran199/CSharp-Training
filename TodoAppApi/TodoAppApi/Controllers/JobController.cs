using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAppApi.Data;
using TodoAppApi.Models;
using TodoAppApi.Services.JobService;

namespace TodoAppApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        [Route("")]
        public  async Task<ActionResult<List<Job>>> getAll()
        {
            var dbJobs = await _jobService.getAll();

            return Ok(dbJobs);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Job>> getById(int id)
        {
            var job = await _jobService.getById(id);

            return Ok(job);
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<Job>> create(Job job)
        {
            await _jobService.create(job);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Job>> update (int id, Job job)
        {
            await _jobService.update(id, job);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Job>> delete(int id)
        {
            await _jobService.delete(id);

            return Ok();
        }
    }
}
