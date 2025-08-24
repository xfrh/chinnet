using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.Meetings
{
    /// <summary>
    /// list 页面的数据实体
    /// </summary>
    public class MeetingListModel
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public long AreaId { get; set; }
        public string Area { get; set; }

        public string MeetingType { get; set; }

        public string Contact { get; set; }

        public string MemberName { get; set; }

        public string InsertTime { get; set; }


    }
}