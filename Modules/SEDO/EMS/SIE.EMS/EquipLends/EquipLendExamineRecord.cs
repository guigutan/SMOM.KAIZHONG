using SIE.Domain;
using SIE.EMS.EquipLends.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.EquipLends
{
    /// <summary>
    /// 设备借还审核记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备借还审核记录")]
    public class EquipLendExamineRecord : DataEntity
    {
        #region 设备借还 EquipLendManage
        /// <summary>
        /// 设备借还Id
        /// </summary>
        [Label("设备借还")]
        public static readonly IRefIdProperty EquipLendManageIdProperty =
            P<EquipLendExamineRecord>.RegisterRefId(e => e.EquipLendManageId, ReferenceType.Parent);

        /// <summary>
        /// 设备借还Id
        /// </summary>
        public double EquipLendManageId
        {
            get { return (double)this.GetRefId(EquipLendManageIdProperty); }
            set { this.SetRefId(EquipLendManageIdProperty, value); }
        }

        /// <summary>
        /// 设备借还
        /// </summary>
        public static readonly RefEntityProperty<EquipLendManage> EquipLendManageProperty =
            P<EquipLendExamineRecord>.RegisterRef(e => e.EquipLendManage, EquipLendManageIdProperty);

        /// <summary>
        /// 设备借还
        /// </summary>
        public EquipLendManage EquipLendManage
        {
            get { return this.GetRefEntity(EquipLendManageProperty); }
            set { this.SetRefEntity(EquipLendManageProperty, value); }
        }
        #endregion

        #region 类型 ExamineType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<ExamineType> ExamineTypeProperty = P<EquipLendExamineRecord>.Register(e => e.ExamineType);

        /// <summary>
        /// 类型
        /// </summary>
        public ExamineType ExamineType
        {
            get { return this.GetProperty(ExamineTypeProperty); }
            set { this.SetProperty(ExamineTypeProperty, value); }
        }
        #endregion

        #region 审核结果 ExamineResult
        /// <summary>
        /// 审核结果
        /// </summary>
        [Label("审核结果")]
        public static readonly Property<ExamineResult> ExamineResultProperty = P<EquipLendExamineRecord>.Register(e => e.ExamineResult);

        /// <summary>
        /// 审核结果
        /// </summary>
        public ExamineResult ExamineResult
        {
            get { return this.GetProperty(ExamineResultProperty); }
            set { this.SetProperty(ExamineResultProperty, value); }
        }
        #endregion

        #region 审核人 Employee
        /// <summary>
        /// 审核人Id
        /// </summary>
        [Label("审核人")]
        public static readonly IRefIdProperty EmployeeIdProperty =
            P<EquipLendExamineRecord>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 审核人Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)this.GetRefId(EmployeeIdProperty); }
            set { this.SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty =
            P<EquipLendExamineRecord>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 审核人
        /// </summary>
        public Employee Employee
        {
            get { return this.GetRefEntity(EmployeeProperty); }
            set { this.SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 审核时间 ExamineDate
        /// <summary>
        /// 审核时间
        /// </summary>
        [Label("审核时间")]
        public static readonly Property<DateTime> ExamineDateProperty = P<EquipLendExamineRecord>.Register(e => e.ExamineDate);

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime ExamineDate
        {
            get { return this.GetProperty(ExamineDateProperty); }
            set { this.SetProperty(ExamineDateProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<EquipLendExamineRecord>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 实体元配置
    /// </summary>
    public class EquipLendExamineRecordEntityConfig : EntityConfig<EquipLendExamineRecord>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.MapTable("EMS_EQUIP_LEND_EXRECORD").MapAllProperties();
            Meta.Property(EquipLendExamineRecord.RemarkProperty).ColumnMeta.HasLength(4000);
        }
    }
}
