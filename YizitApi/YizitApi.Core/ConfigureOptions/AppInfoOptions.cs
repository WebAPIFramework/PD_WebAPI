using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion;
using Furion.ConfigurableOptions;
using Microsoft.Extensions.Configuration;

namespace YizitApi.Core.ConfigureOptions
{
    /// <summary>
    /// 选项实体，对应json配置文件中的结构，支持后续热重载，选项验证
    /// 
    /// </summary>
   public class AppInfoOptions: IConfigurableOptionsListener<AppInfoOptions>
    {
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }
        public string Version { get; set; }
        public string Company { get; set; }

        /// <summary>
        /// 选项更改通知
        /// 在对应配置修改时会触发修改
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        public void OnListener(AppInfoOptions options, IConfiguration configuration)
        {
            Name = options.Name;
            Version = options.Version;
        }

        public void PostConfigure(AppInfoOptions options, IConfiguration configuration)
        {
          
        }
    }
}
