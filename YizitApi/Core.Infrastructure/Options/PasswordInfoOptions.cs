using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion;
using Furion.ConfigurableOptions;
using Furion.DataEncryption;
using Microsoft.Extensions.Configuration;

namespace Core.Infrastructure.Options
{
    /// <summary>
    /// 密码选项实体
    /// 
    /// </summary>
   public class PasswordInfoOptions : IConfigurableOptionsListener<PasswordInfoOptions>
    {
        [Required(ErrorMessage = "超级密码不能为空")]
        public string SuperPassword { get; set; }

        /// <summary>
        /// 选项更改通知
        /// 在对应配置修改时会触发修改
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        public void OnListener(PasswordInfoOptions options, IConfiguration configuration)
        {
            SuperPassword = options.SuperPassword;
            App.GetOptions<PasswordInfoOptions>().SuperPassword= options.SuperPassword;

        }

        public void PostConfigure(PasswordInfoOptions options, IConfiguration configuration)
        {
           // options.SuperPassword??= MD5Encryption.Encrypt("111111");
        }
    }
}
