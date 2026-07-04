using SIE.Domain;
using SIE.MES.PrepareProducts;
using SIE.MES.PrepareProducts.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProcessProperty
{
    /// <summary>
    /// 产前准备项目明细
    /// </summary>
    [ChildEntity,Serializable]
    [Label("产前准备项目明细")]
    public class ProcessPtyDetail : DataEntity
    {
        #region 维护工序属性 ProcessPty
        /// <summary>
        /// 维护工序属性Id
        /// </summary>
        [Label("维护工序属性")]
        public static readonly IRefIdProperty ProcessPtyIdProperty =
            P<ProcessPtyDetail>.RegisterRefId(e => e.ProcessPtyId, ReferenceType.Parent);

        /// <summary>
        /// 维护工序属性Id
        /// </summary>
        public double ProcessPtyId
        {
            get { return (double)this.GetRefId(ProcessPtyIdProperty); }
            set { this.SetRefId(ProcessPtyIdProperty, value); }
        }

        /// <summary>
        /// 维护工序属性
        /// </summary>
        public static readonly RefEntityProperty<ProcessPty> ProcessPtyProperty =
            P<ProcessPtyDetail>.RegisterRef(e => e.ProcessPty, ProcessPtyIdProperty);

        /// <summary>
        /// 维护工序属性
        /// </summary>
        public ProcessPty ProcessPty
        {
            get { return this.GetRefEntity(ProcessPtyProperty); }
            set { this.SetRefEntity(ProcessPtyProperty, value); }
        }
        #endregion

        #region 产前准备项目 PrepareProject
        /// <summary>
        /// 产前准备项目Id
        /// </summary>
        [Label("产前准备项目")]
        public static readonly IRefIdProperty PrepareProjectIdProperty =
            P<ProcessPtyDetail>.RegisterRefId(e => e.PrepareProjectId, ReferenceType.Normal);

        /// <summary>
        /// 产前准备项目Id
        /// </summary>
        public double PrepareProjectId
        {
            get { return (double)this.GetRefId(PrepareProjectIdProperty); }
            set { this.SetRefId(PrepareProjectIdProperty, value); }
        }

        /// <summary>
        /// 产前准备项目
        /// </summary>
        public static readonly RefEntityProperty<PrepareProject> PrepareProjectProperty =
            P<ProcessPtyDetail>.RegisterRef(e => e.PrepareProject, PrepareProjectIdProperty);

        /// <summary>
        /// 产前准备项目
        /// </summary>
        public PrepareProject PrepareProject
        {
            get { return this.GetRefEntity(PrepareProjectProperty); }
            set { this.SetRefEntity(PrepareProjectProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 项目编码 ProCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProCodeProperty = P<ProcessPtyDetail>.RegisterView(e => e.ProCode, p => p.PrepareProject.ProCode);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProCode
        {
            get { return this.GetProperty(ProCodeProperty); }
        }
        #endregion

        #region 项目名称 ProName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProNameProperty = P<ProcessPtyDetail>.RegisterView(e => e.ProName, p => p.PrepareProject.ProName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProName
        {
            get { return this.GetProperty(ProNameProperty); }
        }
        #endregion

        #region 项目类型 ProType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<PrepareProjectType?> ProTypeProperty = P<ProcessPtyDetail>.RegisterView(e => e.ProType, p => p.PrepareProject.ProType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public PrepareProjectType? ProType
        {
            get { return this.GetProperty(ProTypeProperty); }
        }
        #endregion

        #region 项目描述 ProDesc
        /// <summary>
        /// 项目描述
        /// </summary>
        [Label("项目描述")]
        [MaxLength(200)]
        public static readonly Property<string> ProDescProperty = P<ProcessPtyDetail>.RegisterView(e => e.ProDesc, p => p.PrepareProject.ProDesc);

        /// <summary>
        /// 项目描述
        /// </summary>
        public string ProDesc
        {
            get { return this.GetProperty(ProDescProperty); }
        }
        #endregion

        #region 工序Id ProcessId
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序Id")]
        public static readonly Property<double> ProcessIdProperty = P<ProcessPtyDetail>.RegisterView(e => e.ProcessId, p => p.ProcessPty.ProcessId);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return this.GetProperty(ProcessIdProperty); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ProcessPtyDetail>.RegisterView(e => e.ProcessCode, p => p.ProcessPty.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ProcessPtyDetail>.RegisterView(e => e.ProcessName, p => p.ProcessPty.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 产前准备 IsPrepare
        /// <summary>
        /// 产前准备
        /// </summary>
        [Label("产前准备")]
        public static readonly Property<bool?> IsPrepareProperty = P<ProcessPtyDetail>.RegisterView(e => e.IsPrepare, p => p.ProcessPty.IsPrepare);

        /// <summary>
        /// 产前准备
        /// </summary>
        public bool? IsPrepare
        {
            get { return this.GetProperty(IsPrepareProperty); }
        }
        #endregion


        #endregion

    }

    internal class ProcessPtyDetailConfig : EntityConfig<ProcessPtyDetail>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("PROCESS_PTY_DETAIL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
