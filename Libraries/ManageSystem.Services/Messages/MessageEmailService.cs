using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Messages;

namespace ManageSystem.Services.Messages
{
    /// <summary>
    /// 操作类 ，数据库表名：MessageEmail 
    /// </summary>
    public partial class MessageEmailService : BaseService<MessageEmail>, IMessageEmailService
    {

        public MessageEmailService(IRepository<MessageEmail> repository) : base(repository)
        {

        }

    }
}
