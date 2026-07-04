using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.OnOffDuty;
using SIE.MES.WIP;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Tech.Processs;
using SIE.Wpf.MES.WIP;
using System;
using System.Linq;

namespace SIE.Wpf.MES.OnOffDuty
{
    /// <summary>
    /// 上下岗
    /// </summary>
    [RootEntity, Serializable]
    [Label("上下岗")]
    public partial class OnOffDutyViewModel : DataCollectionViewModel<OnOffDutyController>
    {

        /// <summary>
        /// 维修采集视图模型，初始化工序类型
        /// </summary>
        public OnOffDutyViewModel()
        {
            InitWorkstation(ProcessType.Pqc, ProcessType.Fix, ProcessType.Rework,
                ProcessType.Assembly, ProcessType.Packing, ProcessType.BatchAssembly, ProcessType.BatchPqc,
                ProcessType.BatchFix, ProcessType.BatchPacking, ProcessType.Ageing);
        }

        #region 模块KEY ModuleKey
        /// <summary>
        /// 模块KEY
        /// </summary>
        [Label("模块KEY")]
        public static readonly Property<string> ModuleKeyProperty = P<OnOffDutyViewModel>.Register(e => e.ModuleKey);

        /// <summary>
        /// 模块KEY
        /// </summary>
        public string ModuleKey
        {
            get { return this.GetProperty(ModuleKeyProperty); }
            set { this.SetProperty(ModuleKeyProperty, value); }
        }
        #endregion

        #region 采集结果 CollectDetailList
        /// <summary>
        /// 采集结果
        /// </summary>
        [Label("采集结果")]
        public static readonly ListProperty<OnOffDutyCollectDetailViewModelList> OnOffDutyCollectDetailViewModelListProperty = P<OnOffDutyViewModel>.RegisterList(e => e.OnOffDutyCollectDetailViewModelList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as OnOffDutyViewModel).LoadOnOffDutyCollectDetailViewModelList()
        });

        /// <summary>
        /// 采集结果
        /// </summary>
        public OnOffDutyCollectDetailViewModelList OnOffDutyCollectDetailViewModelList
        {
            get { return this.GetLazyList(OnOffDutyCollectDetailViewModelListProperty); }
        }

        /// <summary>
        /// 加载采集结果
        /// </summary>
        /// <returns>采集结果列表</returns>
        private OnOffDutyCollectDetailViewModelList LoadOnOffDutyCollectDetailViewModelList()
        {
            return new OnOffDutyCollectDetailViewModelList();
        }
        #endregion


        /// <summary>
        /// 加载数据
        /// </summary>
        public override void Onload()
        {
            base.Onload();

        }

        #region IsOffDuty 是否上岗
        /// <summary>
        /// 是否上岗
        /// </summary>
        [Label("是否上岗")]
        public static readonly Property<bool> IsOnDutyProperty = P<OnOffDutyViewModel>.Register(e => e.IsOnDuty, new PropertyMetadata<bool>() { PropertyChangedCallBack = (o, e) => (o as OnOffDutyViewModel).OnIsOffDuty(e) });

        /// <summary>
        /// 是否上岗值变更
        /// </summary>
        /// <param name="e">参数</param>
        private void OnIsOffDuty(ManagedPropertyChangedEventArgs e)
        {
            FocuseBarcode();
        }

        /// <summary>
        /// 是否上岗
        /// </summary>
        public bool IsOnDuty
        {
            get { return this.GetProperty(IsOnDutyProperty); }
            set { this.SetProperty(IsOnDutyProperty, value); }
        }
        #endregion

        /// <summary>
        /// 属性变更事件，重置显示信息及数据
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == IsOnDutyProperty)
            {
                ShowTips(!IsOnDuty ? "请扫描下岗员工工号".L10N() : "请扫描上岗员工工号".L10N());
                FocuseBarcode();
            }
        }

        /// <summary>
        /// 条码变更事件，采集条码
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (Barcode.IsNullOrEmpty()) return;

            ClearInfos();

            var workcell = GetWorkcell();
            Controller.CheckedWorkcellParas(workcell);
            try
            {
                var staff = RT.Service.Resolve<EmployeeController>().GetEmployeeByCode(Barcode);
                if (staff == null)
                {
                    throw new ValidationException("系统不存在当前扫描员工".L10N());

                }
                var onoffDutyRecord = new OnOffDutyRecrods();
                onoffDutyRecord.ProcessId = workcell.ProcessId;
                onoffDutyRecord.EmployeeId = staff.Id;
                onoffDutyRecord.StationId = workcell.StationId;
                onoffDutyRecord.ResourceId = workcell.ResourceId;
                onoffDutyRecord.OnOffDutyType = IsOnDuty ? OnOffDutyType.OnDuty : OnOffDutyType.OffDuty;
                Controller.OnOffDuty(onoffDutyRecord, workcell, IsOnDuty);
                AddDetail(onoffDutyRecord);
                this.ShowTips(onoffDutyRecord.OnOffDutyType== OnOffDutyType.OffDuty?"下岗成功！".L10N(): "上岗成功！".L10N());
                //Reset(ResetType.Success);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
            finally
            {
                Barcode = null;
            }
        }

        /// <summary>
        /// 添加采集结果记录
        /// </summary>
        /// <param name="onOffDutyRecrods"></param>
        protected virtual void AddDetail(OnOffDutyRecrods onOffDutyRecrods)
        {
            OnOffDutyCollectDetailViewModelList.Add(new OnOffDutyCollectDetailViewModel
            {
                OnOffDutyType = onOffDutyRecrods.OnOffDutyType,
                CollectUseName = RT.Identity.Name,
                InputDate = DateTime.Now,
                CollectDate = DateTime.Now,
                ProcessName = onOffDutyRecrods.Process.Name,
                StationName = onOffDutyRecrods.Station.Name,
                StaffNO = onOffDutyRecrods.Employee.Code,
                StaffName = onOffDutyRecrods.Employee.Name,
                ResourceName = onOffDutyRecrods.Resource.Name

            });
        }

        /// <summary>
        /// 加载工作单元数据
        /// </summary>
        protected override void LoadWorkstationData()
        {
            base.LoadWorkstationData();
        }


        /// <summary>
        /// 重新开始
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            base.Reset(ResetType.None);
            ShowTips(!IsOnDuty ? "请扫描下岗员工工号".L10N() : "请扫描上岗员工工号".L10N());
        }

        /// <summary>
        /// 能否提交
        /// </summary>
        /// <returns>能提交返回true，否则返回false</returns>
        public bool CanSubmit()
        {
            return Workstation.EmployeeId.HasValue
                && Workstation.ProcessId.HasValue
                && Workstation.StationId.HasValue
                && Workstation.ResourceId.HasValue;
        }
    }
}