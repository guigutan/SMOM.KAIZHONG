using SIE.Domain.Validation;
using System;
using System.ComponentModel;

namespace SIE.EMS.MeteringEquipment.Calibrations
{

    #region 检验明细
    /// <summary>
    /// 检验明细非重复验证规则
    /// </summary>
    [DisplayName("检验明细非重复验证规则")]
    [Description("检验明细非重复验证规则")]
    public class CalibrationDetailNotDuplicateRule : NotDuplicateRule<CalibrationDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CalibrationDetailNotDuplicateRule()
        {
            Properties.Add(CalibrationDetail.MeteringEquipmentAccountIdProperty);
            Properties.Add(CalibrationDetail.CalibrationItemIdProperty);
            MessageBuilder = (e) => { return "设备明细不能重复".L10N(); };
        }
    }
    #endregion
}
