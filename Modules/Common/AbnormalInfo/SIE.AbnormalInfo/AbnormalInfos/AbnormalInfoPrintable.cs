using SIE.Common.Prints;
using SIE.Domain;
using System.ComponentModel;

namespace SIE.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 异常信息打印
    /// </summary>
    [DisplayName("异常信息")]
    class AbnormalInfoPrintable : BillPrintable<AbnormalInfor>
    {
        /// <summary>
        /// 转换数据
        /// </summary>
        /// <param name="data">实体对象</param>
        /// <returns>转换后的数据</returns>
        public override string ConverterData(object data)
        {
            if (data is AbnormalInfor)
            {
                var abnormal = data as AbnormalInfor;
                if (abnormal != null)
                {
                    abnormal = RF.GetById<AbnormalInfor>(abnormal.Id, new EagerLoadOptions().LoadWithViewProperty());
                    data = RT.Service.Resolve<AbnormalInfoController>().GetDefectInfo(abnormal);
                }
            }
            var content = base.ConverterData(data);

            return content;
        }
    }
}
