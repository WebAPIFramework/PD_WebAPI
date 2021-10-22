using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion.DataValidation;
using YizitApi.Application.Enum;

namespace YizitApi.Application.Dtos.Notificaiton
{
    /// <summary>
    /// 通知
    /// </summary>
    public class NotficationDto<T> where T: class
    {
        /// <summary>
        /// 通知范围（默认all）
        /// </summary>
        public EnumNotificationRange PublishRange { get; set; } = EnumNotificationRange.All;
        /// <summary>
        /// 通知发布方式(默认立即)
        /// </summary>
        public EnumNotificationPublishType PublishType { get; set; } = EnumNotificationPublishType.Immediate;
        /// <summary>
        /// 发布时间（默认提交时间）
        /// </summary>
        public DateTime TimePublish 
        { get 
            {   if(string.IsNullOrEmpty(PublishTime)) return DateTime.Now;
                return  Convert.ToDateTime(PublishTime); 
            } 
        
        }

        public string PublishTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        /// <summary>
        /// 通知类型
        /// </summary>
        [Required(ErrorMessage ="Required")]
        public EnumNotificationType Type { get; set; }

        public T Data { get; set; }
    }

    #region 版本更新
    /// <summary>
    /// 版本更新 新增编辑
    /// </summary>
    public class ReleaseNtf
    {
        /// <summary>
        /// 更新时间（默认提交时间）
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public DateTime ReleaseTime { get; set; }= DateTime.Now; 
        /// <summary>
        /// 版本号
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string Version { get; set; }
        /// <summary>
        /// 版本名称
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string VersionName { get; set; }
        /// <summary>
        /// 发布内容
        /// </summary>

        public List<ReleaseDetail> Details { get; set; }
    }

    /// <summary>
    /// 版本更新详情实体
    /// </summary>
    public class ReleaseDetail
    {
        /// <summary>
        /// 版本更新id（编辑时需要传入）
        /// </summary>
        public string ReleaseId { get; set; }
        /// <summary>
        /// 版本更新类型 （0： fix; 1: feature；2: improvement; 3: design ; 4:doc）
        /// </summary>
        public EnumReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        public override string ToString()
        {
            return $"{ReleaseType.ToString()}: {Content}"; 
        }

    }
    #endregion

    #region 维护通知
    /// <summary>
    /// 维护通知 新增编辑参数
    /// </summary>
    public class MaintenanceNtf
    {
        /// <summary>
        /// 维护时间
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public DateTime MaintenanceTime { get; set; }= DateTime.Now;    
        /// <summary>
        /// 维护内容
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string Content { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
    }

    #endregion

    #region 其他通知
    /// <summary>
    /// 其他通知 新增编辑参数
    /// </summary>
    public class OtherNtf
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string Content { get; set; }
    }
    #endregion
}
