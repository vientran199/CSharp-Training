using Microsoft.EntityFrameworkCore;
using TodoAppApi.Data;
using TodoAppApi.Models;

namespace TodoAppApi.Services.JobService
{
    public class JobService : IJobService
    {
        private readonly MyDataContext _context;
        public JobService(MyDataContext context) { 
            _context = context;
        }
        public async Task create(Job job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task delete(int id)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);

            if (job is null)
            {
                throw new Exception($"Not found {id}");
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task<List<Job>> getAll()
        {
            return await _context.Jobs.ToListAsync();
        }

        public async Task<Job> getById(int id)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);

            if (job is null)
            {
                throw new Exception($"Not found {id}");
            }

            return job;
        }

        public async Task update(int id, Job job)
        {
            var _job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);

            if (_job is null)
            {
                throw new Exception($"Not found {id}");
            }

            _job.Name = job.Name;
            _job.Description = job.Description;
            _job.IsCompleted = job.IsCompleted;

            await _context.SaveChangesAsync();

            return;
        }
    }
}
