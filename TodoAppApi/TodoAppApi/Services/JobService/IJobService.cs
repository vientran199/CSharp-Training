using TodoAppApi.Models;

namespace TodoAppApi.Services.JobService
{
    public interface IJobService
    {
        Task<List<Job>> getAll();
        Task<Job> getById(int id);
        Task create (Job job);
        Task update (int id, Job job); 
        Task delete (int id);
    }
}
