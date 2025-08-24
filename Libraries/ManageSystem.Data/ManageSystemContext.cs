namespace ManageSystem.Data
{
 
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Core;
    using System.Collections.Generic;
    using System.Reflection;
    using Mapping;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.Common;
    using System.Data;
    using System.Data.Entity.Infrastructure;

    public class ManageSystemContext : DbContext, IDbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“ManageSystemContext”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“ManageSystem.Data.ManageSystemContext”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“ManageSystemContext”
        //连接字符串。

        public ManageSystemContext()
            : base("name=ManageSystemContext")
        {


            Database.SetInitializer<ManageSystemContext>(null);

            DbConnection con = ((IObjectContextAdapter)this).ObjectContext.Connection;
           

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ManageSystemContext>());
            ((IObjectContextAdapter)this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }

        public ManageSystemContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            ((IObjectContextAdapter)this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }

        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。

        //public virtual DbSet<NewsItem> NewsItem { get; set; }
        //public virtual DbSet<NewsComment> NewsComment { get; set; }

        /// <summary>
        /// 重写， 完成对派生上下文的模型的初始化后，并在该模型已锁定并用于初始化上下文之前，将调用此方法。
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //注册所有的Mapping
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(ManageSystemEntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            //取消默认创建的表名为复数问题
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }


        /// <summary>
        /// 附加一个实体的上下文或返回一个已连接的实体(如果已经连接)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                Set<TEntity>().Attach(entity);
                return entity;
            }

            return alreadyAttached;
        }

        /// <summary>
        /// 执行存储过程和加载实体的列表
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }

            var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();

            //performance hack applied as described here - http://www.nopcommerce.com/boards/t/25483/fix-very-important-speed-improvement.aspx
            bool acd = this.Configuration.AutoDetectChangesEnabled;
            try
            {
                this.Configuration.AutoDetectChangesEnabled = false;

                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);
            }
            finally
            {
                this.Configuration.AutoDetectChangesEnabled = acd;
            }

            return result;
        }

        /// <summary>
        /// 获取或设置一个值指示是否启用了自动检测设置更改(用于EF)
        /// </summary>
        public virtual bool AutoDetectChangesEnabled
        {
            get
            {
                return this.Configuration.AutoDetectChangesEnabled;
            }
            set
            {
                this.Configuration.AutoDetectChangesEnabled = value;
            }
        }


        /// <summary>
        /// 获取或设置一个值指示是否启用了代理创建设置(用于EF)
        /// </summary>
        public virtual bool ProxyCreationEnabled
        {
            get
            {
                return this.Configuration.ProxyCreationEnabled;
            }
            set
            {
                this.Configuration.ProxyCreationEnabled = value;
            }
        }



        /// <summary>
        /// 分离一个实体
        /// </summary>
        /// <param name="entity"></param>
        public void Detach(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }


        /// <summary>
        /// 对数据库执行给定的DDL和DML命令。
        /// </summary>
        /// <param name="sql">命令字符串</param>
        /// <param name="doNotEnsureTransaction">假——并不能确保事务创造;真——事务创造保证。</param>
        /// <param name="timeout">超时值,在几秒钟内。null值表明,底层的提供者将使用的默认值</param>
        /// <param name="parameters">适用于命令的参数字符串。</param>
        /// <returns>数据库执行命令后返回的结果。</returns>
        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = default(int?), params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;

            if (string.IsNullOrWhiteSpace(sql)) return 0;

            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            //return result
            return result;
        }


        /// <summary>
        ///创建一个原始SQL查询将返回给定泛型类型的元素。属性的类型可以是任何类型相匹配的查询返回的列的名称,或者可以是一个简单的原语类型。不需要一个实体类型的类型。这个查询的结果从来没有跟踪的背景下,即使是一个实体类型返回的类型的对象。
        /// </summary>
        /// <typeparam name="TElement">查询返回的对象的类型.</typeparam>
        /// <param name="sql">SQL查询字符串。</param>
        /// <param name="parameters">适用于SQL查询字符串参数。</param>
        /// <returns>Result</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        IDbSet<TEntity> IDbContext.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }
    }

 
}