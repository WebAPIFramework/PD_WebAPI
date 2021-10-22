using Furion.DataEncryption;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Dtos.User
{
    /// <summary>
    /// 修改密码dto
    /// </summary>
    public class UserPassword
    {
        public string OriginPassword { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
    }
}
