using System.Collections.Generic;
using ManageSystem.Core.Domain.Tasks;
using ManageSystem.Core;

namespace ManageSystem.Services.Tasks
{
    /// <summary>
    /// Task service interface
    /// </summary>
    public partial interface IScheduleTaskService : IBaseService<ScheduleTask>
    {
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<ScheduleTask> QueryPage(string name, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets a task by its type
        /// </summary>
        /// <param name="type">Task type</param>
        /// <returns>Task</returns>
        ScheduleTask GetTaskByType(string type);

        /// <summary>
        /// Gets all tasks
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Tasks</returns>
        IList<ScheduleTask> GetAllTasks(bool showHidden = false);

   
    }
}
