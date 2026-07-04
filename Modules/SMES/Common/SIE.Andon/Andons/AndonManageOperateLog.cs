using SIE.Andon.Andons.Enum;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯管理操作记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("安灯管理操作记录")]
    public class AndonManageOperateLog : DataEntity
    {
        #region 操作时间 OperateTime
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateTime> OperateTimeProperty = P<AndonManageOperateLog>.Register(e => e.OperateTime);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime
        {
            get { return this.GetProperty(OperateTimeProperty); }
            set { this.SetProperty(OperateTimeProperty, value); }
        }
        #endregion

        #region 操作类型 OperateType
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<AndonManageOperateType> OperateTypeProperty = P<AndonManageOperateLog>.Register(e => e.OperateType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public AndonManageOperateType OperateType
        {
            get { return this.GetProperty(OperateTypeProperty); }
            set { this.SetProperty(OperateTypeProperty, value); }
        }
        #endregion

        #region 操作人 Operater
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperaterIdProperty =
            P<AndonManageOperateLog>.RegisterRefId(e => e.OperaterId, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OperaterId
        {
            get { return (double)this.GetRefId(OperaterIdProperty); }
            set { this.SetRefId(OperaterIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperaterProperty =
            P<AndonManageOperateLog>.RegisterRef(e => e.Operater, OperaterIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee Operater
        {
            get { return this.GetRefEntity(OperaterProperty); }
            set { this.SetRefEntity(OperaterProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<AndonManageOperateLog>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 距离上一次操作时长(小时) LastOperate
        /// <summary>
        /// 距离上一次操作时长(小时)
        /// </summary>
        [Label("距离上一次操作时长(小时)")]
        public static readonly Property<double> LastOperateProperty = P<AndonManageOperateLog>.Register(e => e.LastOperate);

        /// <summary>
        /// 距离上一次操作时长(小时)
        /// </summary>
        public double LastOperate
        {
            get { return this.GetProperty(LastOperateProperty); }
            set { this.SetProperty(LastOperateProperty, value); }
        }
        #endregion

        #region 安灯管理 AndonManage
        /// <summary>
        /// 安灯管理Id
        /// </summary>
        [Label("安灯管理")]
        public static readonly IRefIdProperty AndonManageIdProperty =
            P<AndonManageOperateLog>.RegisterRefId(e => e.AndonManageId, ReferenceType.Parent);

        /// <summary>
        /// 安灯管理Id
        /// </summary>
        public double AndonManageId
        {
            get { return (double)this.GetRefId(AndonManageIdProperty); }
            set { this.SetRefId(AndonManageIdProperty, value); }
        }

        /// <summary>
        /// 安灯管理
        /// </summary>
        public static readonly RefEntityProperty<AndonManage> AndonManageProperty =
            P<AndonManageOperateLog>.RegisterRef(e => e.AndonManage, AndonManageIdProperty);

        /// <summary>
        /// 安灯管理
        /// </summary>
        public AndonManage AndonManage
        {
            get { return this.GetRefEntity(AndonManageProperty); }
            set { this.SetRefEntity(AndonManageProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 操作人名称 OperaterName
        /// <summary>
        /// 操作人名称
        /// </summary>
        [Label("操作人名称")]
        public static readonly Property<string> OperaterNameProperty = P<AndonManageOperateLog>.RegisterView(e => e.OperaterName, p => p.Operater.Name);

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperaterName
        {
            get { return this.GetProperty(OperaterNameProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库

        #region 距离上一次操作时长(分钟) LastOperateMinute
        /// <summary>
        /// 距离上一次操作时长(分钟)
        /// </summary>
        [Label("距离上一次操作时长(分钟)")]
        public static readonly Property<double> LastOperateMinuteProperty = P<AndonManageOperateLog>.RegisterReadOnly(
            e => e.LastOperateMinute, e => e.GetLastOperateMinute(), LastOperateProperty);
        /// <summary>
        /// 距离上一次操作时长(分钟)
        /// </summary>

        public double LastOperateMinute
        {
            get { return this.GetProperty(LastOperateMinuteProperty); }
        }
        private double GetLastOperateMinute()
        {
            return LastOperate * 60;
        }
        #endregion

        #endregion

    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class AndonManageOperateLogConfig : EntityConfig<AndonManageOperateLog>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_ANDONMANAGEOPERATELOG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
