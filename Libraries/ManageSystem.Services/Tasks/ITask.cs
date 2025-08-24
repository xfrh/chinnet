namespace ManageSystem.Services.Tasks
{
    /// <summary>
    /// 应该由每个任务实现的接口
    /// </summary>
    public partial interface ITask
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        void Execute();
    }
}
