using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Models;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.InventoryTasks.ViewModels
{
    /// <summary>
    /// 新增盘盈 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("新增盘盈")]
    public class AddFixtureProfitViewModel : ViewModel
    {
        #region 盘点任务id InventoryTaskId
        /// <summary>
        /// 盘点任务id
        /// </summary>
        [Label("盘点任务id")]
        public static readonly Property<double> InventoryTaskIdProperty = P<AddFixtureProfitViewModel>.Register(e => e.InventoryTaskId);

        /// <summary>
        /// 盘点任务id
        /// </summary>
        public double InventoryTaskId
        {
            get { return this.GetProperty(InventoryTaskIdProperty); }
            set { this.SetProperty(InventoryTaskIdProperty, value); }
        }
        #endregion


        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        [Label("工治具编码")]
        public static readonly IRefIdProperty FixtureEncodeIdProperty = P<AddFixtureProfitViewModel>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<AddFixtureProfitViewModel>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return GetRefEntity(FixtureEncodeProperty); }
            set { SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 在库数 StockQty
        /// <summary>
        /// 在库数
        /// </summary>
        [Label("在库数")]
        public static readonly Property<int> StockQtyProperty = P<AddFixtureProfitViewModel>.Register(e => e.StockQty);

        /// <summary>
        /// 在库数
        /// </summary>
        public int StockQty
        {
            get { return this.GetProperty(StockQtyProperty); }
            set { this.SetProperty(StockQtyProperty, value); }
        }
        #endregion

/*        #region 在库合格数 StockPassQty
        /// <summary>
        /// 在库合格数
        /// </summary>
        [Label("在库合格数")]
        public static readonly Property<int?> StockPassQtyProperty = P<AddFixtureProfitViewModel>.Register(e => e.StockPassQty);

        /// <summary>
        /// 在库合格数
        /// </summary>
        public int? StockPassQty
        {
            get { return this.GetProperty(StockPassQtyProperty); }
            set { this.SetProperty(StockPassQtyProperty, value); }
        }
        #endregion

        #region 在库不合格数 StockNgQty
        /// <summary>
        /// 在库不合格数
        /// </summary>
        [Label("在库不合格数")]
        public static readonly Property<int?> StockNgQtyProperty = P<AddFixtureProfitViewModel>.Register(e => e.StockNgQty);

        /// <summary>
        /// 在库不合格数
        /// </summary>
        public int? StockNgQty
        {
            get { return this.GetProperty(StockNgQtyProperty); }
            set { this.SetProperty(StockNgQtyProperty, value); }
        }
        #endregion*/



        #region 在线数 Online
        /// <summary>
        /// 在线数
        /// </summary>
        [Label("在线数")]
        public static readonly Property<int> OnlineProperty = P<AddFixtureProfitViewModel>.Register(e => e.Online);

        /// <summary>
        /// 在线数
        /// </summary>
        public int Online
        {
            get { return GetProperty(OnlineProperty); }
            set { SetProperty(OnlineProperty, value); }
        }
        #endregion


        
        #region 库存状态 FixtureStatus
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<FixtureStatus> FixtureStatusProperty = P<AddFixtureProfitViewModel>.Register(e => e.FixtureStatus);

        /// <summary>
        /// 库存状态
        /// </summary>
        public FixtureStatus FixtureStatus
        {
            get { return GetProperty(FixtureStatusProperty); }
            set { SetProperty(FixtureStatusProperty, value); }
        }
        #endregion

        #region 自动生成序列号 GenerateSn
        /// <summary>
        /// 自动生成序列号
        /// </summary>
        [Label("自动生成序列号")]
        public static readonly Property<bool> GenerateSnProperty = P<AddFixtureProfitViewModel>.Register(e => e.GenerateSn);

        /// <summary>
        /// 自动生成序列号
        /// </summary>
        public bool GenerateSn
        {
            get { return this.GetProperty(GenerateSnProperty); }
            set { this.SetProperty(GenerateSnProperty, value); }
        }
        #endregion

        #region 序列号 Sn
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SnProperty = P<AddFixtureProfitViewModel>.Register(e => e.Sn);

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region PDA盘点状态 PdaType
        /// <summary>
        /// PDA盘点状态
        /// </summary>
        [Label("PDA盘点状态")]
        public static readonly Property<int> PdaTypeProperty = P<AddFixtureProfitViewModel>.Register(e => e.PdaType);

        /// <summary>
        /// PDA盘点状态
        /// </summary>
        public int PdaType
        {
            get { return this.GetProperty(PdaTypeProperty); }
            set { this.SetProperty(PdaTypeProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工治具型号编码 ModelCode
        /// <summary>
        /// 工治具型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<AddFixtureProfitViewModel>.RegisterView(e => e.ModelCode, p => p.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 工治具型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { this.SetProperty(ModelCodeProperty, value); }
        }
        #endregion

        #region 工治具型号名称 ModelName
        /// <summary>
        /// 工治具型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<AddFixtureProfitViewModel>.RegisterView(e => e.ModelName, p => p.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 工治具型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
            set { this.SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeProperty = P<AddFixtureProfitViewModel>.RegisterView(e => e.FixtureType, p => p.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType
        {
            get { return this.GetProperty(FixtureTypeProperty); }
            set { this.SetProperty(FixtureTypeProperty, value); }
        }
        #endregion

        #region 工治具编码（PDA用） Encode
        /// <summary>
        /// 工治具编码（PDA用）
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> EncodeProperty = P<AddFixtureProfitViewModel>.Register(e => e.Encode);

        /// <summary>
        /// 工治具编码（PDA用）
        /// </summary>
        public string Encode
        {
            get { return this.GetProperty(EncodeProperty); }
            set { this.SetProperty(EncodeProperty, value); }
        }
        #endregion



        #region 管控方式 ManageMode
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<AddFixtureProfitViewModel>.RegisterView(e => e.ManageMode, p => p.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ManageMode ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
            set
            {
                this.SetProperty(ManageModeProperty, value);
            }
        }
        #endregion
        #endregion
    }
}
