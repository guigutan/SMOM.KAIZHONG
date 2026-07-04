using SIE.Domain;
using SIE.Fixtures.Models;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.InventoryTasks.ViewModels
{
    /// <summary>
    /// 工治具导入模型
    /// </summary>
    [Serializable]
    public class ImportFixtureModel:ViewModel
    {
        #region 工治具ID Sn
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> SnProperty = P<ImportFixtureModel>.Register(e => e.Sn);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        [Label("工治具编码")]
        public static readonly IRefIdProperty FixtureEncodeIdProperty = P<ImportFixtureModel>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double FixtureEncodeId
        {
            get { return (double)GetRefId(FixtureEncodeIdProperty); }
            set { SetRefId(FixtureEncodeIdProperty, value); }
        }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<ImportFixtureModel>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return GetRefEntity(FixtureEncodeProperty); }
            set { SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 实盘合格数 PassQty
        /// <summary>
        /// 实盘合格数
        /// </summary>
        [Label("实盘合格数")]
        public static readonly Property<int> PassQtyProperty = P<ImportFixtureModel>.Register(e => e.PassQty);

        /// <summary>
        /// 实盘合格数
        /// </summary>
        public int PassQty
        {
            get { return this.GetProperty(PassQtyProperty); }
            set { this.SetProperty(PassQtyProperty, value); }
        }
        #endregion


        #region 实盘不合格数 NgQty
        /// <summary>
        /// 实盘不合格数
        /// </summary>
        [Label("实盘不合格数")]
        public static readonly Property<int> NgQtyProperty = P<ImportFixtureModel>.Register(e => e.NgQty);

        /// <summary>
        /// 实盘不合格数
        /// </summary>
        public int NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion


        #region 实盘在线数 OnlineQty
        /// <summary>
        /// 实盘在线数
        /// </summary>
        [Label("实盘在线数")]
        public static readonly Property<int> OnlineQtyProperty = P<ImportFixtureModel>.Register(e => e.OnlineQty);

        /// <summary>
        /// 实盘在线数
        /// </summary>
        public int OnlineQty
        {
            get { return this.GetProperty(OnlineQtyProperty); }
            set { this.SetProperty(OnlineQtyProperty, value); }
        }
        #endregion
    }
}
