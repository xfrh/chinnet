using System;
using Autofac;
using ManageSystem.Core.Configuration;
using ManageSystem.Core.Domain.Tasks;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Infrastructure;
using ManageSystem.Services.Log;

namespace ManageSystem.Services.Tasks
{
    /// <summary>
    /// Task
    /// </summary>
    public partial class Task
    {
        #region Ctor

        /// <summary>
        /// Ctor for Task
        /// </summary>
        private Task()
        {
            this.Enabled = true;
        }

        /// <summary>
        /// Ctor for Task
        /// </summary>
        /// <param name="task">Task </param>
        public Task(ScheduleTask task)
        {
            this.Type = task.Type;
            this.Enabled = task.Enabled;
            this.StopOnError = task.StopOnError;
            this.Name = task.Name;
        }

        #endregion

        #region Utilities

        private ITask CreateTask(ILifetimeScope scope)
        {
            ITask task = null;
            if (this.Enabled)
            {
                var type2 = System.Type.GetType(this.Type);
                if (type2 != null)
                {
                    object instance;
                    if (!EngineContext.Current.ContainerManager.TryResolve(type2, scope, out instance))
                    {
                        instance = EngineContext.Current.ContainerManager.ResolveUnregistered(type2, scope);
                    }
                    task = instance as ITask;
                }
            }
            return task;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="throwException">一个值指示是否应该抛出异常,如果发生了一些错误</param>
        /// <param name="dispose">一个值指示是否所有实例运行后应处理任务</param>
        /// <param name="ensureRunOnOneWebFarmInstance">值指示我们是否应该确保这个任务运行在一个农场节点</param>
        public void Execute(bool throwException = false, bool dispose = true, bool ensureRunOnOneWebFarmInstance = true)
        {
            //background tasks has an issue with Autofac
            //because scope is generated each time it's requested
            //that's why we get one single scope here
            //this way we can also dispose resources once a task is completed
            var scope = EngineContext.Current.ContainerManager.Scope();
            var scheduleTaskService = EngineContext.Current.ContainerManager.Resolve<IScheduleTaskService>("", scope);
            var scheduleTask = scheduleTaskService.GetTaskByType(this.Type);

            try
            {
                //task is run on one farm node at a time?
                if (ensureRunOnOneWebFarmInstance)
                {
                    //is web farm enabled (multiple instances)?
                    var nopConfig = EngineContext.Current.ContainerManager.Resolve<ManageSystemConfig>("", scope);
                    if (nopConfig.MultipleInstancesEnabled)
                    {
                        var machineNameProvider = EngineContext.Current.ContainerManager.Resolve<IMachineNameProvider>("", scope);
                        var machineName = machineNameProvider.GetMachineName();
                        if (String.IsNullOrEmpty(machineName))
                        {
                            throw new Exception("Machine name cannot be detected. You cannot run in web farm.");
                            //actually in this case we can generate some unique string (e.g. Guid) and store it in some "static" (!!!) variable
                            //then it can be used as a machine name
                        }

                        //lease can't be aquired only if for a different machine and it has not expired
                        if (scheduleTask.LeasedUntilUtc.HasValue &&
                            scheduleTask.LeasedUntilUtc.Value >= DateTime.Now &&
                            scheduleTask.LeasedByMachineName != machineName)
                            return;

                        //lease the task. so it's run on one farm node at a time
                        scheduleTask.LeasedByMachineName = machineName;
                        scheduleTask.LeasedUntilUtc = DateTime.Now.AddMinutes(30);
                        scheduleTaskService.Update(scheduleTask);
                    }
                }

                //初始化和执行任务
                var task = this.CreateTask(scope);
                if (task != null)
                {
                    this.LastStartUtc = DateTime.Now;
                    if (scheduleTask != null)
                    {
                        //更新相关操作时间
                        scheduleTask.LastStartUtc = this.LastStartUtc;
                        scheduleTaskService.Update(scheduleTask);
                    }
                    task.Execute();
                    this.LastEndUtc = this.LastSuccessUtc = DateTime.Now;
                }
            }
            catch (Exception exc)
            {
                this.Enabled = !this.StopOnError;
                this.LastEndUtc = DateTime.Now;

                //错误日志
                var logger = EngineContext.Current.ContainerManager.Resolve<ISystemLogService>("", scope);
                logger.Insert( string.Format("自动执行任务发送异常，任务名称： '{0}' ，异常信息： {1}", this.Name, exc.Message),exc.ToString(),Core.Domain.Log.SystemLogLevel.Error);

                if (throwException)
                    throw;
            }

            if (scheduleTask != null)
            {
                //更新相关操作时间
                scheduleTask.LastEndUtc = this.LastEndUtc;
                scheduleTask.LastSuccessUtc = this.LastSuccessUtc;
                scheduleTaskService.Update(scheduleTask);
            }

          //释放资源
            if (dispose)
            {
                scope.Dispose();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// 最后开始时间
        /// </summary>
        public DateTime? LastStartUtc { get; private set; }

        /// <summary>
        /// 最后执行结束的时间
        /// </summary>
        public DateTime? LastEndUtc { get; private set; }

        /// <summary>
        /// 最后一次执行成功的时间
        /// </summary>
        public DateTime? LastSuccessUtc { get; private set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// 发送错误是否停止
        /// </summary>
        public bool StopOnError { get; private set; }

        /// <summary>
        /// 获取任务的名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 是否启用任务
        /// </summary>
        public bool Enabled { get; set; }

        #endregion
    }
}
