using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.ProcessTaskLists
{
    /// <summary>
    /// 拆分任务ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("拆分任务")]
    public class SplitTaskViewModel : ViewModel
    {



        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<SplitTaskViewModel>.Register(e => e.WoNo);

        /// <summary>
        /// 工单Id
        /// </summary>
        public string WoNo
        {
            get { return GetProperty(WoNoProperty); }
            set { SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 已生成任务数 DispatchedTaskQty
        /// <summary>
        /// 已生成任务数
        /// </summary>
        [Label("已生成任务数")]
        public static readonly Property<decimal> DispatchedTaskQtyProperty = P<SplitTaskViewModel>.Register(e => e.DispatchedTaskQty);

        /// <summary>
        /// 已生成任务数
        /// </summary>
        public decimal DispatchedTaskQty
        {
            get { return GetProperty(DispatchedTaskQtyProperty); }
            set { SetProperty(DispatchedTaskQtyProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<SplitTaskViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 本次生成数 Qty
        /// <summary>
        /// 本次生成数
        /// </summary>
        [Label("本次生成数")]
        public static readonly Property<decimal> QtyProperty = P<SplitTaskViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 本次生成数
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion


        #region 工单工序数 DispatchQty
        /// <summary>
        /// 工单工序数
        /// </summary>
        [Label("工单工序数")]
        public static readonly Property<decimal> DispatchQtyProperty = P<SplitTaskViewModel>.Register(e => e.DispatchQty);

        /// <summary>
        /// 工单工序数
        /// </summary>
        public decimal DispatchQty
        {
            get { return GetProperty(DispatchQtyProperty); }
            set { SetProperty(DispatchQtyProperty, value); }
        }
        #endregion

        #region 剩余数 RemainQty
        /// <summary>
        /// 剩余数
        /// </summary>
        [Label("剩余数")]
        public static readonly Property<decimal> RemainQtyProperty = P<SplitTaskViewModel>.Register(e => e.RemainQty);

        /// <summary>
        /// 剩余数
        /// </summary>
        public decimal RemainQty
        {
            get { return GetProperty(RemainQtyProperty); }
            set { SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 份数 Copies
        /// <summary>
        /// 份数
        /// </summary>
        [Label("份数")]
        public static readonly Property<int> CopiesProperty = P<SplitTaskViewModel>.Register(e => e.Copies);

        /// <summary>
        /// 份数
        /// </summary>
        public int Copies
        {
            get { return GetProperty(CopiesProperty); }
            set { SetProperty(CopiesProperty, value); }
        }
        #endregion

        #region 工序清单Id RountingPrcossId
        /// <summary>
        /// 工序清单Id
        /// </summary>
        [Label("工序清单Id")]
        public static readonly Property<double> RountingPrcossIdProperty = P<SplitTaskViewModel>.Register(e => e.RountingPrcossId);

        /// <summary>
        /// 工序清单Id
        /// </summary>
        public double RountingPrcossId
        {
            get { return GetProperty(RountingPrcossIdProperty); }
            set { SetProperty(RountingPrcossIdProperty, value); }
        }
        #endregion


        #region 工序Id PrcossId
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序Id")]
        public static readonly Property<double> ProcessIdProperty = P<SplitTaskViewModel>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return GetProperty(ProcessIdProperty); }
            set { SetProperty(ProcessIdProperty, value); }
        }
        #endregion

    }
}
