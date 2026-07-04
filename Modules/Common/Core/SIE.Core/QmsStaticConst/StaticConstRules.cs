using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;

namespace SIE.Core.QmsStaticConst
{
    #region 统计系数表：控制图参数验证
    /// <summary>
    /// K值 不能为空
    /// </summary>
    [System.ComponentModel.DisplayName("统计系数表：控制图参数验证")]
    [System.ComponentModel.Description("统计系数表：控制图参数验证,至少维护一条系数")]
    public class ControlChartConstRequired : PropertyRule<ControlChartConst>
    {
        /// <summary>
        /// 属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {

                return ControlChartConst.IdProperty;
            }
        }

        /// <summary>
        /// K值 不能为空
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var en = entity as ControlChartConst;

            if (
                !CheckNotNull(en.A)
                && !CheckNotNull(en.A2)
                && !CheckNotNull(en.A3)
                && !CheckNotNull(en.B3)
                && !CheckNotNull(en.B4)
                && !CheckNotNull(en.B5)
                && !CheckNotNull(en.B6)
                && !CheckNotNull(en.C4)
                && !CheckNotNull(en.D1)
                && !CheckNotNull(en.D2)
                && !CheckNotNull(en.D3)
                && !CheckNotNull(en.D4)
                && !CheckNotNull(en.D2Nd)
                && !CheckNotNull(en.D3Nd)
                && !CheckNotNull(en.D4Nd)
                && !CheckNotNull(en.E2)
                && !CheckNotNull(en.MeA2))
                e.BrokenDescription = "样本为[{0}]的控制图参数，至少维护一条系数".L10nFormat(en.SampleQty);
        }

        private bool CheckNotNull(double? prop)
        {
            return prop.HasValue && prop != 0;
        }
    }
    #endregion


}