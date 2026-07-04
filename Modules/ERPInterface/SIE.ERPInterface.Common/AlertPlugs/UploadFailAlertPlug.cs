using SIE.Common.Alert;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using System;
using System.Linq;
using System.Text;

namespace SIE.ERPInterface.Common.AlertPlugs
{
    /// <summary>
    /// ERP接口调度上传失败预警
    /// </summary>
    [RootEntity, Serializable]
    [Alert("ERP接口调度上传失败预警", typeof(UploadFailAlertPlugConfig), typeof(StringAlertResult), "ERP接口调度上传失败预警")]
    public class UploadFailAlertPlug : AlertBase
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override AlertResultBase Run()
        {
            var config = this.Context.Config as DownloadFailAlertPlugConfig;
            if (config == null) throw new ValidationException("预警配置为空，请维护。".L10N());

            //获取数据
            var period = config.Period;
            var endDate = DateTime.Now;
            var beginDate = endDate.AddDays(-period);
            var dataStatistics = RT.Service.Resolve<InterfaceAlertController>().GetUploadFailStatistics(beginDate, endDate);

            //构建预警信息
            if (dataStatistics.Any())
            {
                var result = new StringAlertResult();
                var message = new StringBuilder();
                message.Append("ERP接口调度上传失败统计:\\n\r".L10N());
                message.Append("日期:{0} - {1}\\n\r".L10nFormat(beginDate.ToString(), endDate.ToString()));

                var i = 1;
                foreach (var data in dataStatistics)
                {
                    message.Append("({0}){1} - {2} - {3}\\n\r".L10nFormat(i.ToString("00"), data.OrderType, data.TransactionType, data.FailCount));
                    i++;
                }

                message.Append("\\n\r");
                message.Append("请接口负责人及时查看并处理接口失败问题。");

                result.Message = message.ToString();
                result.Value = dataStatistics.Sum(p => p.FailCount);        //失败数量赋值预警严重数值
                return result;
            }

            return null;
        }
    }
}
