using Furion.DataEncryption;
using Furion.DataValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YizitApi.Application.Dtos.User
{
    /// <summary>
    /// 新建/编辑用户
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户名(登录名)
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string Username { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string JobNo { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [DataValidation(ValidationTypes.PhoneNumber,AllowNullValue =true,AllowEmptyStrings =true)]
        public string Mobile { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [DataValidation(ValidationTypes.EmailAddress, AllowNullValue = true, AllowEmptyStrings = true)]
        public string Email { get; set; }

        /// <summary>
        /// 停用/启用状态 
        /// </summary>
        public int? Available { get; set; } = 1;
        ///// <summary>
        ///// 登录名
        ///// </summary>
        //public string LoginName { get; set; }
        ///// <summary>
        ///// 密码
        ///// </summary>
        //public string Password { get; set; } = MD5Encryption.Encrypt("111111");
        /// <summary>
        /// 账户类型，默认本地用户
        /// </summary>
        public int? AccountType { get; set; }
    }
}
