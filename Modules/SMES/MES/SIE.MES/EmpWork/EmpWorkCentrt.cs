using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Fixture;
using SIE.MES.ItemEquipAccount;
using SIE.MES.ItemFixture;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WorkCenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.EmpWork
{
    /// <summary>
    /// 人员与工作中心
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EmpWorkCentrtCriterial))]
    [Label("人员与工作中心")]
    public partial class EmpWorkCentrt : DataEntity
    {
        #region 名称 Employee
        /// <summary>
        /// 名称Id
        /// </summary>
        [Label("名称")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<EmpWorkCentrt>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 名称Id
        /// </summary>
        public double? EmployeeId
        {
            get { return (double?)GetRefNullableId(EmployeeIdProperty); }
            set { SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<EmpWorkCentrt>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 名称
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 工号 EmpNo
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> EmpNoProperty = P<EmpWorkCentrt>.Register(e => e.EmpNo);

        /// <summary>
        /// 工号
        /// </summary>
        public string EmpNo
        {
            get { return this.GetProperty(EmpNoProperty); }
            set { this.SetProperty(EmpNoProperty, value); }
        }
        #endregion

        #region 工作中心编码 WorkCenter
        /// <summary>
        /// 工作中心编码Id
        /// </summary>
        [Label("工作中心编码")]
        public static readonly IRefIdProperty WorkCenterIdProperty = P<EmpWorkCentrt>.RegisterRefId(e => e.WorkCenterId, ReferenceType.Normal);

        /// <summary>
        /// 工作中心编码Id
        /// </summary>
        public double WorkCenterId
        {
            get { return (double)GetRefNullableId(WorkCenterIdProperty); }
            set { SetRefNullableId(WorkCenterIdProperty, value); }
        }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public static readonly RefEntityProperty<WorkCenter> WorkCenterProperty = P<EmpWorkCentrt>.RegisterRef(e => e.WorkCenter, WorkCenterIdProperty);

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public WorkCenter WorkCenter
        {
            get { return GetRefEntity(WorkCenterProperty); }
            set { SetRefEntity(WorkCenterProperty, value); }
        }
        #endregion

        #region 工作中心名称 WorkName
        /// <summary>
        /// 工作中心名称
        /// </summary>
        [Label("工作中心名称")]
        public static readonly Property<string> WorkNameProperty = P<EmpWorkCentrt>.Register(e => e.WorkName);

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string WorkName
        {
            get { return this.GetProperty(WorkNameProperty); }
            set { this.SetProperty(WorkNameProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工作中心编码 WorkCenterCode
        /// <summary>
        /// 工作中心编码
        /// </summary>
        [Label("工作中心编码")]
        public static readonly Property<string> WorkCenterCodeProperty = P<EmpWorkCentrt>.RegisterView(e => e.WorkCenterCode, p => p.WorkCenter.Code);

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode
        {
            get { return this.GetProperty(WorkCenterCodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 人员与工作中心关系 实体配置
    /// </summary>
    internal class EmpWorkCentrtConfig : EntityConfig<EmpWorkCentrt>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    EmpWorkCentrt.EmployeeIdProperty,
                    EmpWorkCentrt.WorkCenterIdProperty
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
            Meta.MapTable("EMP_WORK_CENTRT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
    /// <summary>
    /// 人员被人员与工作中心的关系引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("人员被人员与工作中心的关系引用不允许删除")]
    [System.ComponentModel.Description("人员被人员与工作中心的关系引用不允许删除")]
    public partial class UndeleteEquipAccountItem : NoReferencedRule<Employee>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteEquipAccountItem()
        {
            Properties.Add(EmpWorkCentrt.EmployeeIdProperty);
            MessageBuilder = (o, e) =>
            {
                var item = o as Employee;
                return "人员[{0}]已经被[{1}]引用,不能删除".L10nFormat(item.Name, "人员与工作中心的关系".L10N());
            };
        }
    }
    /// <summary>
    /// 工作中心被模具与产品关系引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("工作中心被人员与工作中心关系引用不允许删除")]
    [System.ComponentModel.Description("工作中心被人员与工作中心关系引用不允许删除")]
    public partial class UndeleteFixtureProcess : NoReferencedRule<WorkCenter>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteFixtureProcess()
        {
            Properties.Add(EmpWorkCentrt.WorkCenterIdProperty);
            MessageBuilder = (o, e) =>
            {
                var process = o as WorkCenter;
                return "工作中心[{0}]已经被[{1}]引用,不能删除".L10nFormat(process.Code, "人员与工作中心的关系".L10N());
            };
        }
    }
}
