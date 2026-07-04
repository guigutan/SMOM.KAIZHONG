using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案生产采集
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单制造档案生产采集")]
    public class WoOrderArchiveProcessViewModel : ViewModel
    {
        #region 工序顺序 Index
        /// <summary>
        /// 工序顺序
        /// </summary>
        [Label("工序顺序")]
        public static readonly Property<int> IndexProperty = P<WoOrderArchiveProcessViewModel>.Register(e => e.Index);

        /// <summary>
        /// 工序顺序
        /// </summary>
        public int Index
        {
            get { return this.GetProperty(IndexProperty); }
            set { this.SetProperty(IndexProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<WoOrderArchiveProcessViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 过站数量 QtyMove
        /// <summary>
        /// 过站数量
        /// </summary>
        [Label("过站数量")]
        public static readonly Property<decimal> QtyMoveProperty = P<WoOrderArchiveProcessViewModel>.Register(e => e.QtyMove);

        /// <summary>
        /// 过站数量
        /// </summary>
        public decimal QtyMove
        {
            get { return this.GetProperty(QtyMoveProperty); }
            set { this.SetProperty(QtyMoveProperty, value); }
        }
        #endregion

        #region 成功数量 QtyPass
        /// <summary>
        /// 成功数量
        /// </summary>
        [Label("成功数量")]
        public static readonly Property<decimal> QtyPassProperty = P<WoOrderArchiveProcessViewModel>.Register(e => e.QtyPass);

        /// <summary>
        /// 成功数量
        /// </summary>
        public decimal QtyPass
        {
            get { return this.GetProperty(QtyPassProperty); }
            set { this.SetProperty(QtyPassProperty, value); }
        }
        #endregion

        #region 失败数量 QtyFailed
        /// <summary>
        /// 失败数量
        /// </summary>
        [Label("失败数量")]
        public static readonly Property<decimal> QtyFailedProperty = P<WoOrderArchiveProcessViewModel>.Register(e => e.QtyFailed);

        /// <summary>
        /// 失败数量
        /// </summary>
        public decimal QtyFailed
        {
            get { return this.GetProperty(QtyFailedProperty); }
            set { this.SetProperty(QtyFailedProperty, value); }
        }
        #endregion

        #region 堆积数 QtyStacked
        /// <summary>
        /// 堆积数
        /// </summary>
        [Label("堆积数")]
        public static readonly Property<decimal> QtyStackedProperty = P<WoOrderArchiveProcessViewModel>.Register(e => e.QtyStacked);

        /// <summary>
        /// 堆积数
        /// </summary>
        public decimal QtyStacked
        {
            get { return this.GetProperty(QtyStackedProperty); }
            set { this.SetProperty(QtyStackedProperty, value); }
        }
        #endregion

    }
}
