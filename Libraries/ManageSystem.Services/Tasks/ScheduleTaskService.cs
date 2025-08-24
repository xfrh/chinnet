using System;
using System.Collections.Generic;
using System.Linq;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Tasks;
using ManageSystem.Core;

namespace ManageSystem.Services.Tasks
{
    /// <summary>
    /// Task service
    /// </summary>
    public partial class ScheduleTaskService :BaseService<ScheduleTask>, IScheduleTaskService 
    {

            public ScheduleTaskService(IRepository<ScheduleTask> repository) : base(repository)
            {

            }


  

        #region Methods



        /// <summary>
        /// Gets a task by its type
        /// </summary>
        /// <param name="type">Task type</param>
        /// <returns>Task</returns>
        public virtual ScheduleTask GetTaskByType(string type)
        {
            if (String.IsNullOrWhiteSpace(type))
                return null;

            var query =base._repository.Table;
            query = query.Where(st => st.Type == type);
            query = query.OrderByDescending(t => t.Id);

            var task = query.FirstOrDefault();
            return task;
        }

        /// <summary>
        /// Gets all tasks
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Tasks</returns>
        public virtual IList<ScheduleTask> GetAllTasks(bool showHidden = false)
        {
             var query = base._repository.Table;
            if (!showHidden)
            {
                query = query.Where(t => t.Enabled);
            }
            query = query.OrderByDescending(t => t.Seconds);

            var tasks = query.ToList();
            return tasks;
        }


        public IPagedList<ScheduleTask> QueryPage(string name, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table;
            if (!string.IsNullOrEmpty(name)) query = query.Where(m => m.Name.Contains(name));

            query = query.OrderByDescending(b => b.InsertTime);

            return new PagedList<ScheduleTask>(query, pageIndex, pageSize);
        }



        #endregion
    }
}
