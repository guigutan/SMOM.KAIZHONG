using SIE.Defects;
using SIE.Defects.Measures;
using SIE.Domain;
using SIE.MES.TemporaryRepairs;
using SIE.ObjectModel;
using System;

namespace SIE.Wpf.MES.Wip.TemporaryRepairs
{
    /// <summary>
    /// 维修录入视图模型
    /// </summary>
    [RootEntity]
    [Label("维修录入视图模型")]
    public class RepairInputViewModel : ViewModel
    {
        #region 缺陷ID DefectId
        /// <summary>
        /// 缺陷ID
        /// </summary>
        [Label("缺陷ID")]
        public static readonly Property<double> DefectIdProperty = P<RepairInputViewModel>.Register(e => e.DefectId);

        /// <summary>
        /// 缺陷ID
        /// </summary>
        public double DefectId
        {
            get { return this.GetProperty(DefectIdProperty); }
            set { this.SetProperty(DefectIdProperty, value); }
        }
        #endregion  

        #region 缺陷编码 DefectCode
        /// <summary>
        /// 缺陷编码
        /// </summary>
        [Label("缺陷编码")]
        public static readonly Property<string> DefectCodeProperty = P<RepairInputViewModel>.Register(e => e.DefectCode);

        /// <summary>
        /// 缺陷编码
        /// </summary>
        public string DefectCode
        {
            get { return this.GetProperty(DefectCodeProperty); }
            set { this.SetProperty(DefectCodeProperty, value); }
        }
        #endregion

        #region 缺陷描述 DefectDesc
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescProperty = P<RepairInputViewModel>.Register(e => e.DefectDesc);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDesc
        {
            get { return this.GetProperty(DefectDescProperty); }
            set { this.SetProperty(DefectDescProperty, value); }
        }
        #endregion

        #region 工序 ProcessName
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<RepairInputViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 维修措施 Measure
        /// <summary>
        /// 维修措施
        /// </summary>
        [Label("维修措施")]
        public static readonly Property<string> MeasureProperty = P<RepairInputViewModel>.Register(e => e.Measure, (o, e) => (o as RepairInputViewModel).OnMeasurePropertyChanged());

        /// <summary>
        /// 维修措施
        /// </summary>
        public string Measure
        {
            get { return this.GetProperty(MeasureProperty); }
            set { this.SetProperty(MeasureProperty, value); }
        }
        #endregion

        #region 维修方案 RepairSolution
        /// <summary>
        /// 维修方案
        /// </summary>
        [Label("维修方案")]
        public static readonly Property<DefectRepairSolution> RepairSolutionProperty = P<RepairInputViewModel>.Register(e => e.RepairSolution);

        /// <summary>
        /// 维修方案
        /// </summary>
        public DefectRepairSolution RepairSolution
        {
            get { return this.GetProperty(RepairSolutionProperty); }
            set { this.SetProperty(RepairSolutionProperty, value); }
        }
        #endregion

        #region 实际缺陷ID ActualDefectId
        /// <summary>
        /// 实际缺陷ID
        /// </summary>
        [Label("实际缺陷ID")]
        public static readonly Property<double> ActualDefectIdProperty = P<RepairInputViewModel>.Register(e => e.ActualDefectId);

        /// <summary>
        /// 实际缺陷ID
        /// </summary>
        public double ActualDefectId
        {
            get { return this.GetProperty(ActualDefectIdProperty); }
            set { this.SetProperty(ActualDefectIdProperty, value); }
        }
        #endregion

        #region 实际缺陷编码 ActualDefectCode
        /// <summary>
        /// 实际缺陷编码
        /// </summary>
        [Label("实际缺陷编码")]
        public static readonly Property<string> ActualDefectCodeProperty = P<RepairInputViewModel>.Register(e => e.ActualDefectCode);

        /// <summary>
        /// 实际缺陷编码
        /// </summary>
        public string ActualDefectCode
        {
            get { return this.GetProperty(ActualDefectCodeProperty); }
            set { this.SetProperty(ActualDefectCodeProperty, value); }
        }
        #endregion

        #region 实际缺陷描述 ActualDefectDesc
        /// <summary>
        /// 实际缺陷描述
        /// </summary>
        [Label("实际缺陷描述")]
        public static readonly Property<string> ActualDefectDescProperty = P<RepairInputViewModel>.Register(e => e.ActualDefectDesc);

        /// <summary>
        /// 实际缺陷描述
        /// </summary>
        public string ActualDefectDesc
        {
            get { return this.GetProperty(ActualDefectDescProperty); }
            set { this.SetProperty(ActualDefectDescProperty, value); }
        }
        #endregion

        #region 维修位置 RepairLocation
        /// <summary>
        /// 维修位置
        /// </summary>
        [Label("维修位置")]
        public static readonly Property<string> RepairLocationProperty = P<RepairInputViewModel>.Register(e => e.RepairLocation);

        /// <summary>
        /// 维修位置
        /// </summary>
        public string RepairLocation
        {
            get { return this.GetProperty(RepairLocationProperty); }
            set { this.SetProperty(RepairLocationProperty, value); }
        }
        #endregion

        #region 换料条码 ReloadSn
        /// <summary>
        /// 换料条码
        /// </summary>
        [Label("换料条码")]
        public static readonly Property<string> ReloadSnProperty = P<RepairInputViewModel>.Register(e => e.ReloadSn, (o, e) => (o as RepairInputViewModel).OnReloadSnPropertyChanged());

        /// <summary>
        /// 换料条码
        /// </summary>
        public string ReloadSn
        {
            get { return this.GetProperty(ReloadSnProperty); }
            set { this.SetProperty(ReloadSnProperty, value); }
        }
        #endregion

        #region 换料条码提示 ReloadSnTips
        /// <summary>
        /// 换料条码提示
        /// </summary>
        [Label("换料条码提示")]
        public static readonly Property<string> ReloadSnTipsProperty = P<RepairInputViewModel>.Register(e => e.ReloadSnTips);

        /// <summary>
        /// 换料条码提示
        /// </summary>
        public string ReloadSnTips
        {
            get { return this.GetProperty(ReloadSnTipsProperty); }
            set { this.SetProperty(ReloadSnTipsProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<RepairInputViewModel>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion 

        #region 维修措施 MeasureList
        /// <summary>
        /// 维修措施
        /// </summary> 
        [Label("维修措施")]
        public static readonly ListProperty<EntityList<RepairMeasure>> MeasureListProperty = P<RepairInputViewModel>.RegisterList(e => e.MeasureList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<RepairMeasure>()
        });

        /// <summary>
        /// 维修措施
        /// </summary>
        public EntityList<RepairMeasure> MeasureList
        {
            get { return this.GetLazyList(MeasureListProperty); }
        }
        #endregion

        #region 缺陷责任 ResponsibilityList
        /// <summary>
        /// 缺陷责任
        /// </summary> 
        [Label("缺陷责任")]
        public static readonly ListProperty<EntityList<DefectResponsibility>> ResponsibilityListProperty = P<RepairInputViewModel>.RegisterList(e => e.ResponsibilityList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<DefectResponsibility>()
        });

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public EntityList<DefectResponsibility> ResponsibilityList
        {
            get { return this.GetLazyList(ResponsibilityListProperty); }
        }
        #endregion

        #region 缺陷图片 PhotoList
        /// <summary>
        /// 缺陷图片
        /// </summary>
        [Label("缺陷图片")]
        public static readonly ListProperty<EntityList<DefectPhotoVeiwModel>> PhotoListProperty = P<RepairInputViewModel>.RegisterList(e => e.PhotoList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as RepairInputViewModel).LoadPhotoList()
        });

        /// <summary>
        /// 缺陷图片
        /// </summary>
        public EntityList<DefectPhotoVeiwModel> PhotoList
        {
            get { return this.GetLazyList(PhotoListProperty); }
        }

        /// <summary>
        /// 加载缺陷图片
        /// </summary>
        private EntityList<DefectPhotoVeiwModel> LoadPhotoList()
        {
            return new EntityList<DefectPhotoVeiwModel>();
        }
        #endregion 

        #region 维修方案数据源 RepairSolutionList
        /// <summary>
        /// 维修方案数据源
        /// </summary>
        [Label("维修方案数据源")]
        public static readonly ListProperty<EntityList<DefectRepairSolution>> RepairSolutionListProperty = P<RepairInputViewModel>.RegisterList(e => e.RepairSolutionList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as RepairInputViewModel).LoadRepairSolutionList()
        });

        /// <summary>
        /// 维修方案数据源
        /// </summary>
        public EntityList<DefectRepairSolution> RepairSolutionList
        {
            get { return this.GetLazyList(RepairSolutionListProperty); }
        }

        /// <summary>
        /// 加载维修方案数据源
        /// </summary>
        private EntityList<DefectRepairSolution> LoadRepairSolutionList()
        {
            return new EntityList<DefectRepairSolution>();
        }
        #endregion

        /// <summary>
        /// 维修措施属性变更
        /// </summary>
        private void OnMeasurePropertyChanged()
        {
            try
            {
                if (string.IsNullOrEmpty(Measure))
                    return;

            }
            catch (Exception exc)
            {
                CRT.MessageService.ShowError(exc.Message);
            }
            finally
            {
                Measure = null;
            }
        }

        /// <summary>
        /// 换料条码属性变更
        /// </summary>
        private void OnReloadSnPropertyChanged()
        {
            if (string.IsNullOrEmpty(ReloadSn))
                return;
            ReloadSnTips = ReloadSn;
            ReloadSn = "";
        }
    }

    /// <summary>
    /// 缺陷图片视图模型
    /// </summary>
    [RootEntity]
    [Label("缺陷图片视图模型")]
    public class DefectPhotoVeiwModel : ViewModel
    {
        #region 图片 Photo
        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        public static readonly Property<byte[]> PhotoProperty = P<DefectPhotoVeiwModel>.Register(e => e.Photo);

        /// <summary>
        /// 图片
        /// </summary>
        public byte[] Photo
        {
            get { return this.GetProperty(PhotoProperty); }
            set { this.SetProperty(PhotoProperty, value); }
        }
        #endregion 
    }
}