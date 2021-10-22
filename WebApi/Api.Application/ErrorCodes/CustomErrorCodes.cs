using Furion.FriendlyException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application
{
    /// <summary>
    /// sample error codes
    /// </summary>
    [ErrorCodeType]
  public  enum CustomErrorCodes
    {
        /// <summary>
        /// 不匹配
        /// </summary>
        [ErrorCodeItemMetadata("NotMatchBetween")]
        C1000,
       
    }
}
