using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 员工中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("员工中间表")]
    public partial class EmployeeInf : DownloadBaseEntity
    {
        #region 工号 Code
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> CodeProperty = P<EmployeeInf>.Register(e => e.Code);

        /// <summary>
        /// 工号
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 姓名 Name
        /// <summary>
        /// 姓名
        /// </summary>
        [Label("姓名")]
        public static readonly Property<string> NameProperty = P<EmployeeInf>.Register(e => e.Name);

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 入职时间 HireDate
        /// <summary>
        /// 入职时间
        /// </summary>
        [Label("入职时间")]
        public static readonly Property<DateTime?> HireDateProperty = P<EmployeeInf>.Register(e => e.HireDate);

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? HireDate
        {
            get { return GetProperty(HireDateProperty); }
            set { SetProperty(HireDateProperty, value); }
        }
        #endregion

        #region 电话号码 Phone
        /// <summary>
        /// 电话号码
        /// </summary>
        [Label("电话号码")]
        public static readonly Property<string> PhoneProperty = P<EmployeeInf>.Register(e => e.Phone);

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone
        {
            get { return GetProperty(PhoneProperty); }
            set { SetProperty(PhoneProperty, value); }
        }
        #endregion

        #region 电子邮箱 Email
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Label("电子邮箱")]
        public static readonly Property<string> EmailProperty = P<EmployeeInf>.Register(e => e.Email);

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email
        {
            get { return GetProperty(EmailProperty); }
            set { SetProperty(EmailProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        [MaxLength(2000)]
        public static readonly Property<string> RemarkProperty = P<EmployeeInf>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 性别 Sex
        /// <summary>
        /// 性别
        /// </summary>
        [Label("性别")]
        public static readonly Property<int> SexProperty = P<EmployeeInf>.Register(e => e.Sex);

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex
        {
            get { return GetProperty(SexProperty); }
            set { SetProperty(SexProperty, value); }
        }
        #endregion

        #region 账户ID AccountCode
        /// <summary>
        /// 账户ID
        /// </summary>
        [Label("账户ID")]
        public static readonly Property<string> AccountCodeProperty = P<EmployeeInf>.Register(e => e.AccountCode);

        /// <summary>
        /// 账户ID
        /// </summary>
        public string AccountCode
        {
            get { return this.GetProperty(AccountCodeProperty); }
            set { this.SetProperty(AccountCodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 员工中间表 实体配置
    /// </summary>
    internal class EmployeeInfConfig : EntityConfig<EmployeeInf>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(EmployeeInf.RemarkProperty, new StringLengthRangeRule() { Max = 4000 });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_EMP").MapAllProperties();
            Meta.Property(EmployeeInf.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}