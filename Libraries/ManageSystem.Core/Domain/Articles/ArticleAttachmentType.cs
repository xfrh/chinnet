using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Articles
{
  
    /// <summary>
     /// 附件的类型
     /// </summary>
    public enum ArticleAttachmentType
    {
        [Description("图片(jpg、bmp、png)")]
        Image = 1,
        [Description("视频（mp4）")]
        Video = 2,
        [Description("音频（mp3）")]
        Audio = 3,
        [Description("其他（doc、rar、7z、zip）")]
        Other = 4,
    }

}
