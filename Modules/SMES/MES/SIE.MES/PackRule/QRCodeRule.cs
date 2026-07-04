using NPOI.SS.Formula.Functions;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.ItemEquipAccount;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.PackRule
{
    /// <summary>
    /// 二维码规则表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("二维码规则表")]
    [DisplayMember(nameof(RuleNumber))]
    public partial class QRCodeRule : DataEntity
    {
        #region 规则编号 RuleNumber
        /// <summary>
        /// 规则编号
        /// </summary>
        [Required]
        [Label("规则编号")]
        public static readonly Property<string> RuleNumberProperty = P<QRCodeRule>.Register(e => e.RuleNumber);

        /// <summary>
        /// 规则编号
        /// </summary>
        public string RuleNumber
        {
            get { return GetProperty(RuleNumberProperty); }
            set { SetProperty(RuleNumberProperty, value); }
        }
        #endregion

        #region 规则描述 RuleNumberDesc
        /// <summary>
        /// 规则描述
        /// </summary>
        [Required]
        [Label("规则描述")]
        public static readonly Property<string> RuleNumberDescProperty = P<QRCodeRule>.Register(e => e.RuleNumberDesc);

        /// <summary>
        /// 规则描述
        /// </summary>
        public string RuleNumberDesc
        {
            get { return GetProperty(RuleNumberDescProperty); }
            set { SetProperty(RuleNumberDescProperty, value); }
        }
        #endregion

        #region 客户零件号开始位数 CustomPnStartDigit
        /// <summary>
        /// 客户零件号开始位数
        /// </summary>
        [Label("客户零件号开始位数")]
        public static readonly Property<string> CustomPnStartDigitProperty = P<QRCodeRule>.Register(e => e.CustomPnStartDigit);

        /// <summary>
        /// 客户零件号开始位数
        /// </summary>
        public string CustomPnStartDigit
        {
            get { return GetProperty(CustomPnStartDigitProperty); }
            set { SetProperty(CustomPnStartDigitProperty, value); }
        }
        #endregion

        #region 客户零件号结束位数 CustomPnEndDigit
        /// <summary>
        /// 客户零件号结束位数
        /// </summary>
        [Label("客户零件号结束位数")]
        public static readonly Property<string> CustomPnEndDigitProperty = P<QRCodeRule>.Register(e => e.CustomPnEndDigit);

        /// <summary>
        /// 客户零件号结束位数
        /// </summary>
        public string CustomPnEndDigit
        {
            get { return GetProperty(CustomPnEndDigitProperty); }
            set { SetProperty(CustomPnEndDigitProperty, value); }
        }
        #endregion

        #region 客户版本号开始位数 VersionNumberStartDigit
        /// <summary>
        /// 客户版本号开始位数
        /// </summary>
        [Label("客户版本号开始位数")]
        public static readonly Property<string> VersionNumberStartDigitProperty = P<QRCodeRule>.Register(e => e.VersionNumberStartDigit);

        /// <summary>
        /// 客户版本号开始位数
        /// </summary>
        public string VersionNumberStartDigit
        {
            get { return GetProperty(VersionNumberStartDigitProperty); }
            set { SetProperty(VersionNumberStartDigitProperty, value); }
        }
        #endregion

        #region 客户版本号结束位数 VersionNumberEndDigit
        /// <summary>
        /// 客户版本号结束位数
        /// </summary>
        [Label("客户版本号结束位数")]
        public static readonly Property<string> VersionNumberEndDigitProperty = P<QRCodeRule>.Register(e => e.VersionNumberEndDigit);

        /// <summary>
        /// 客户版本号结束位数
        /// </summary>
        public string VersionNumberEndDigit
        {
            get { return GetProperty(VersionNumberEndDigitProperty); }
            set { SetProperty(VersionNumberEndDigitProperty, value); }
        }
        #endregion

        #region 序列号开始位数 SerialNumberStartDigit
        /// <summary>
        /// 序列号开始位数
        /// </summary>
        [Required]
        [Label("序列号开始位数")]
        public static readonly Property<string> SerialNumberStartDigitProperty = P<QRCodeRule>.Register(e => e.SerialNumberStartDigit);

        /// <summary>
        /// 序列号开始位数
        /// </summary>
        public string SerialNumberStartDigit
        {
            get { return GetProperty(SerialNumberStartDigitProperty); }
            set { SetProperty(SerialNumberStartDigitProperty, value); }
        }
        #endregion

        #region 序列号结束位数 SerialNumberEndDigit
        /// <summary>
        /// 序列号结束位数
        /// </summary>
        [Required]
        [Label("序列号结束位数")]
        public static readonly Property<string> SerialNumberEndDigitProperty = P<QRCodeRule>.Register(e => e.SerialNumberEndDigit);

        /// <summary>
        /// 序列号结束位数
        /// </summary>
        public string SerialNumberEndDigit
        {
            get { return GetProperty(SerialNumberEndDigitProperty); }
            set { SetProperty(SerialNumberEndDigitProperty, value); }
        }
        #endregion

        #region 二维码总位数 TotalDigit
        /// <summary>
        /// 二维码总位数
        /// </summary>
        [Required]
        [Label("二维码总位数")]
        public static readonly Property<string> TotalDigitProperty = P<QRCodeRule>.Register(e => e.TotalDigit);

        /// <summary>
        /// 二维码总位数
        /// </summary>
        public string TotalDigit
        {
            get { return GetProperty(TotalDigitProperty); }
            set { SetProperty(TotalDigitProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 二维码规则表 实体配置
    /// </summary>
    internal class QRCodeRuleConfig : EntityConfig<QRCodeRule>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    QRCodeRule.RuleNumberProperty
                },
                MessageBuilder = (e) =>
                {
                    return "数据已存在!".L10N();
                }
            });
            base.AddValidations(rules);
        }
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("QRCODE_RULE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
