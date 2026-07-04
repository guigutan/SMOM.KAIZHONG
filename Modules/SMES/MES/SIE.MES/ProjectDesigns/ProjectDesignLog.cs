using SIE.Domain;
using SIE.MES.ProjectDesigns.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns
{
    /// <summary>
    /// 项目号需求设计操作日志
    /// </summary>
    [ChildEntity, Serializable]
    [Label("项目号需求设计操作日志")]
    public class ProjectDesignLog : DataEntity
    {
        #region 需求设计 ProjectDesign
        /// <summary>
        /// 需求设计Id
        /// </summary>
        [Label("需求设计")]
        public static readonly IRefIdProperty ProjectDesignIdProperty =
            P<ProjectDesignLog>.RegisterRefId(e => e.ProjectDesignId, ReferenceType.Parent);

        /// <summary>
        /// 需求设计Id
        /// </summary>
        public double ProjectDesignId
        {
            get { return (double)this.GetRefId(ProjectDesignIdProperty); }
            set { this.SetRefId(ProjectDesignIdProperty, value); }
        }

        /// <summary>
        /// 需求设计
        /// </summary>
        public static readonly RefEntityProperty<ProjectDesign> ProjectDesignProperty =
            P<ProjectDesignLog>.RegisterRef(e => e.ProjectDesign, ProjectDesignIdProperty);

        /// <summary>
        /// 需求设计
        /// </summary>
        public ProjectDesign ProjectDesign
        {
            get { return this.GetRefEntity(ProjectDesignProperty); }
            set { this.SetRefEntity(ProjectDesignProperty, value); }
        }
        #endregion

        #region 操作节点 OperatePoint
        /// <summary>
        /// 操作节点
        /// </summary>
        [Label("操作节点")]
        public static readonly Property<OperatePoint> OperatePointProperty = P<ProjectDesignLog>.Register(e => e.OperatePoint);

        /// <summary>
        /// 操作节点
        /// </summary>
        public OperatePoint OperatePoint
        {
            get { return this.GetProperty(OperatePointProperty); }
            set { this.SetProperty(OperatePointProperty, value); }
        }
        #endregion

        #region 操作人 Operater
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperaterIdProperty =
            P<ProjectDesignLog>.RegisterRefId(e => e.OperaterId, ReferenceType.Normal);

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
            P<ProjectDesignLog>.RegisterRef(e => e.Operater, OperaterIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee Operater
        {
            get { return this.GetRefEntity(OperaterProperty); }
            set { this.SetRefEntity(OperaterProperty, value); }
        }
        #endregion

        #region 操作时间 OperateTime
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateTime> OperateTimeProperty = P<ProjectDesignLog>.Register(e => e.OperateTime);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime
        {
            get { return this.GetProperty(OperateTimeProperty); }
            set { this.SetProperty(OperateTimeProperty, value); }
        }
        #endregion

        #region 其他说明 OtherRemark
        /// <summary>
        /// 其他说明
        /// </summary>
        [Label("其他说明")]
        public static readonly Property<string> OtherRemarkProperty = P<ProjectDesignLog>.Register(e => e.OtherRemark);

        /// <summary>
        /// 其他说明
        /// </summary>
        public string OtherRemark
        {
            get { return this.GetProperty(OtherRemarkProperty); }
            set { this.SetProperty(OtherRemarkProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class ProjectDesignLogConfig : EntityConfig<ProjectDesignLog>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PRO_DESLOG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
