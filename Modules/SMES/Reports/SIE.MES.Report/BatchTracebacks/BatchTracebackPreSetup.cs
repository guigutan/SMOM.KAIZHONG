using SIE.Domain;
using SIE.Domain.Query;
using SIE.Items;
using SIE.MES.Common;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MES.TaskManagement.PreStartupSetupRecords;
using SIE.MES.TaskManagement.Reports;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Report.BatchTracebacks
{
    /// <summary>
    /// 开机准备记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("开机准备记录")]
    public class BatchTracebackPreSetup : Entity<double>
    {
        #region 派工单 DispatchTask
        /// <summary>
        /// 派工单Id
        /// </summary>
        [Label("派工单")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<BatchTracebackPreSetup>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

        /// <summary>
        /// 派工单Id
        /// </summary>
        public double DispatchTaskId
        {
            get { return (double)this.GetRefId(DispatchTaskIdProperty); }
            set { this.SetRefId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 派工单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty =
            P<BatchTracebackPreSetup>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 派工单
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 编码 ToolCode
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> ToolCodeProperty = P<BatchTracebackPreSetup>.Register(e => e.ToolCode);

        /// <summary>
        /// 编码
        /// </summary>
        public string ToolCode
        {
            get { return this.GetProperty(ToolCodeProperty); }
            set { this.SetProperty(ToolCodeProperty, value); }
        }
        #endregion

        #region 名称 ToolName
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> ToolNameProperty = P<BatchTracebackPreSetup>.Register(e => e.ToolName);

        /// <summary>
        /// 名称
        /// </summary>
        public string ToolName
        {
            get { return this.GetProperty(ToolNameProperty); }
            set { this.SetProperty(ToolNameProperty, value); }
        }
        #endregion

        #region 图号 DrawingNo
        /// <summary>
        /// 图号
        /// </summary>
        [Label("图号")]
        public static readonly Property<string> DrawingNoProperty = P<BatchTracebackPreSetup>.Register(e => e.DrawingNo);

        /// <summary>
        /// 图号
        /// </summary>
        public string DrawingNo
        {
            get { return this.GetProperty(DrawingNoProperty); }
            set { this.SetProperty(DrawingNoProperty, value); }
        }
        #endregion

        #region 状态 ToolState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<string> ToolStateProperty = P<BatchTracebackPreSetup>.Register(e => e.ToolState);

        /// <summary>
        /// 状态
        /// </summary>
        public string ToolState
        {
            get { return this.GetProperty(ToolStateProperty); }
            set { this.SetProperty(ToolStateProperty, value); }
        }
        #endregion

        #region 唯一码 UniqueCode
        /// <summary>
        /// 唯一码
        /// </summary>
        [Label("唯一码")]
        public static readonly Property<string> UniqueCodeProperty = P<BatchTracebackPreSetup>.Register(e => e.UniqueCode);

        /// <summary>
        /// 唯一码
        /// </summary>
        public string UniqueCode
        {
            get { return this.GetProperty(UniqueCodeProperty); }
            set { this.SetProperty(UniqueCodeProperty, value); }
        }
        #endregion

        #region 类型 CheckerFixtureType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<CheckerFixtureType> CheckerFixtureTypeProperty = P<BatchTracebackPreSetup>.Register(e => e.CheckerFixtureType);

        /// <summary>
        /// 类型
        /// </summary>
        public CheckerFixtureType CheckerFixtureType
        {
            get { return this.GetProperty(CheckerFixtureTypeProperty); }
            set { this.SetProperty(CheckerFixtureTypeProperty, value); }
        }
        #endregion
    }

    internal class BatchTracebackPreSetupConfig : EntityConfig<BatchTracebackPreSetup>
    {
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<PreStartupSetupRecord>("pssr")
            
.Where(p => p.SQL<int>("pssr.IS_PHANTOM") == 0 && p.SQL<int?>("pssr.INV_ORG_ID") == RT.InvOrg)
.Select(pssr => new
{
    Id = pssr.Id,
    Tool_Code = pssr.ToolCode,
    Dispatch_Task_Id = pssr.DispatchTaskId,
    Tool_Name = pssr.ToolName,
    Tool_State = pssr.ToolState,
    Drawing_No = pssr.DrawingNo,
    Unique_Code = pssr.UniqueCode,
    Checker_Fixture_Type = pssr.CheckerFixtureType
})
.ToQuery();
            Meta.MapView(view).MapAllProperties();
        }
    }
}
