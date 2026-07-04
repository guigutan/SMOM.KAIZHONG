using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Andon.AndonStatisticsReports
{
    /// <summary>
    /// 安灯统计报表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AndonStatisticsViewModelCriteria))]
    [Label("安灯统计报表")]
    public class AndonStatisticsViewModel : ViewModel
    {
        #region 安灯大类 AndonClass
        /// <summary>
        /// 安灯大类
        /// </summary>
        [Required]
        [Label("安灯大类")]
        public static readonly Property<string> AndonClassProperty = P<AndonStatisticsViewModel>.Register(e => e.AndonClass);

        /// <summary>
        /// 安灯大类
        /// </summary>
        public string AndonClass
        {
            get { return this.GetProperty(AndonClassProperty); }
            set { this.SetProperty(AndonClassProperty, value); }
        }
        #endregion

        #region 安灯类型 AndonType
        /// <summary>
        /// 安灯类型
        /// </summary>
        [Label("安灯类型")]
        public static readonly Property<string> AndonTypeProperty = P<AndonStatisticsViewModel>.Register(e => e.AndonType);

        /// <summary>
        /// 安灯类型
        /// </summary>
        public string AndonType
        {
            get { return this.GetProperty(AndonTypeProperty); }
            set { this.SetProperty(AndonTypeProperty, value); }
        }
        #endregion


        #region 安灯名称 AndonName
        /// <summary>
        /// 安灯名称
        /// </summary>
        [Required]
        [Label("安灯名称")]
        public static readonly Property<string> AndonNameProperty = P<AndonStatisticsViewModel>.Register(e => e.AndonName);

        /// <summary>
        /// 安灯名称
        /// </summary>
        public string AndonName
        {
            get { return this.GetProperty(AndonNameProperty); }
            set { this.SetProperty(AndonNameProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<AndonStatisticsViewModel>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion


        #region 车间 WorkShop
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopProperty = P<AndonStatisticsViewModel>.Register(e => e.WorkShop);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShop
        {
            get { return this.GetProperty(WorkShopProperty); }
            set { this.SetProperty(WorkShopProperty, value); }
        }
        #endregion


        #region 产线 WipResource
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly Property<string> WipResourceProperty = P<AndonStatisticsViewModel>.Register(e => e.WipResource);

        /// <summary>
        /// 产线
        /// </summary>
        public string WipResource
        {
            get { return this.GetProperty(WipResourceProperty); }
            set { this.SetProperty(WipResourceProperty, value); }
        }
        #endregion


        #region 设备编码 EquipmentName
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipmentNameProperty = P<AndonStatisticsViewModel>.Register(e => e.EquipmentName);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentName
        {
            get { return this.GetProperty(EquipmentNameProperty); }
            set { this.SetProperty(EquipmentNameProperty, value); }
        }
        #endregion


        #region 部门 Department
        /// <summary>
        /// 部门
        /// </summary>
        [Label("部门")]
        public static readonly Property<string> DepartmentProperty = P<AndonStatisticsViewModel>.Register(e => e.Department);

        /// <summary>
        /// 部门
        /// </summary>
        public string Department
        {
            get { return this.GetProperty(DepartmentProperty); }
            set { this.SetProperty(DepartmentProperty, value); }
        }
        #endregion


        #region 安灯次数 AndonNum
        /// <summary>
        /// 安灯次数
        /// </summary>
        [Label("安灯次数")]
        public static readonly Property<int> AndonNumProperty = P<AndonStatisticsViewModel>.Register(e => e.AndonNum);

        /// <summary>
        /// 安灯次数
        /// </summary>
        public int AndonNum
        {
            get { return this.GetProperty(AndonNumProperty); }
            set { this.SetProperty(AndonNumProperty, value); }
        }
        #endregion

        #region 安灯时长 AndonTime
        /// <summary>
        /// 安灯时长
        /// </summary>
        [Label("安灯时长")]
        public static readonly Property<double> AndonTimeProperty = P<AndonStatisticsViewModel>.Register(e => e.AndonTime);

        /// <summary>
        /// 安灯时长
        /// </summary>
        public double AndonTime
        {
            get { return this.GetProperty(AndonTimeProperty); }
            set { this.SetProperty(AndonTimeProperty, value); }
        }
        #endregion

        #region 停线次数 AndonStopNum
        /// <summary>
        /// 停线次数
        /// </summary>
        [Label("停线次数")]
        public static readonly Property<int> AndonStopNumProperty = P<AndonStatisticsViewModel>.Register(e => e.AndonStopNum);

        /// <summary>
        /// 停线次数
        /// </summary>
        public int AndonStopNum
        {
            get { return this.GetProperty(AndonStopNumProperty); }
            set { this.SetProperty(AndonStopNumProperty, value); }
        }
        #endregion


        #region 停线时长 AndonStopLine
        /// <summary>
        /// 停线时长
        /// </summary>
        [Label("停线时长")]
        public static readonly Property<double> AndonStopLineProperty = P<AndonStatisticsViewModel>.Register(e => e.AndonStopLine);

        /// <summary>
        /// 停线时长
        /// </summary>
        public double AndonStopLine
        {
            get { return this.GetProperty(AndonStopLineProperty); }
            set { this.SetProperty(AndonStopLineProperty, value); }
        }
        #endregion

        #region 安灯名称变更率 TriggerAccuracy
        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        [Label("安灯名称变更率(%)")]
        public static readonly Property<double> TriggerAccuracyProperty = P<AndonStatisticsViewModel>.Register(e => e.TriggerAccuracy);

        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        public double TriggerAccuracy
        {
            get { return this.GetProperty(TriggerAccuracyProperty); }
            set { this.SetProperty(TriggerAccuracyProperty, value); }
        }
        #endregion


        #region 产品 Product
        /// <summary>
        /// 产品
        /// </summary>
        [Label("产品")]
        public static readonly Property<string> ProductProperty = P<AndonStatisticsViewModel>.Register(e => e.Product);

        /// <summary>
        /// 产品
        /// </summary>
        public string Product
        {
            get { return this.GetProperty(ProductProperty); }
            set { this.SetProperty(ProductProperty, value); }
        }
        #endregion



        #region 触发人 Trigger
        /// <summary>
        /// 触发人
        /// </summary>
        [Label("触发人")]
        public static readonly Property<string> TriggerProperty = P<AndonStatisticsViewModel>.Register(e => e.Trigger);

        /// <summary>
        /// 触发人
        /// </summary>
        public string Trigger
        {
            get { return this.GetProperty(TriggerProperty); }
            set { this.SetProperty(TriggerProperty, value); }
        }
        #endregion



    }
}
