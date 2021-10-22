using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Utils.Cron
{
    public class CronUtils
    {
        //private static final SimpleDateFormat sdf = new SimpleDateFormat("ss mm HH dd MM ? yyyy");

        ///***
        // *  功能描述：日期转换cron表达式
        // * @param date
        // * @return
        // */
        //public static string formatDateByPattern(DateTime date)
        //{
        //    String formatTimeStr = null;
        //    if (Objects.nonNull(date))
        //    {
        //        formatTimeStr = sdf.format(date);
        //    }
        //    return formatTimeStr;
        //}

        ///***
        // * convert Date to cron, eg "0 07 10 15 1 ? 2016"
        // * @param date  : 时间点
        // * @return
        // */
        //public static string getCron(DateTime date)
        //{
        //    return formatDateByPattern(date);
        //}
    }
}
