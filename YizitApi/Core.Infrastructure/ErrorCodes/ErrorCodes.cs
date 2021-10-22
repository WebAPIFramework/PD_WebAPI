using Furion.FriendlyException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.ErrorCodes
{
    /// <summary>
    /// sample error codes
    /// </summary>
    [ErrorCodeType]
  public  enum ErrorCodes
    {
        /// <summary>
        /// 不匹配
        /// </summary>
        [ErrorCodeItemMetadata("NotMatchBetween")]
        z1000,
        /// <summary>
        /// 必填 {0}不允许为空
        /// </summary>
        [ErrorCodeItemMetadata("Required")]
        z1001,
        /// <summary>
        /// ObjectNotExisted  {0}不存在
        /// </summary>
        [ErrorCodeItemMetadata("ObjectNotExisted")]
        z1002,
        /// <summary>
        /// 不正确  {0}不正确
        /// </summary>
        [ErrorCodeItemMetadata("NotCorrect")]
        z1003,
        /// <summary>
        /// 重复
        /// </summary>
        [ErrorCodeItemMetadata("Duplicate")]
        z1004,
        /// <summary>
        /// 不允许删除{0}
        /// </summary>
        [ErrorCodeItemMetadata("NotAllowDelete")]
        z1005,
        /// <summary>
        /// 不允许修改{0}{1}
        /// </summary>
        [ErrorCodeItemMetadata("NotAllowChange")]
        z1006,
     
    }
}
