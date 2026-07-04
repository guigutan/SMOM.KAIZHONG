using DevExpress.DataProcessing;
using DevExpress.Xpf.CodeView;
using DocumentFormat.OpenXml.Bibliography;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.OnOffDuty;
using SIE.MES.OnOffDutyB;
using SIE.MES.WIP;
using SIE.ObjectModel;
using SIE.Packages.Packings;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Wpf.MES.Controls;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SIE.Wpf.MES.OnOffDutyB
{

    /// <summary>
    /// B上下岗
    /// </summary>
    [RootEntity, Serializable]
    [Label("B上下岗")]
    public partial class OnOffDutyBViewModel : KZDataCollectionViewModel  // KZDataCollectionViewModel  DataCollectionViewModel<OnOffDutyBController>   //
    {
        /// <summary>
        /// 视图模型，初始化工序类型
        /// </summary>
        public OnOffDutyBViewModel()
        {
        }

        #region 模块KEY ModuleKey
        /// <summary>
        /// 模块KEY
        /// </summary>
        [Label("B模块KEY")]
        public static readonly Property<string> ModuleKeyProperty = P<OnOffDutyBViewModel>.Register(e => e.ModuleKey);

        /// <summary>
        /// 模块KEY
        /// </summary>
        public string ModuleKey
        {
            get { return this.GetProperty(ModuleKeyProperty); }
            set { this.SetProperty(ModuleKeyProperty, value); }
        }
        #endregion

        #region B采集结果 CollectDetailList
        /// <summary>
        /// B采集结果
        /// </summary>
        [Label("B采集结果")]
        public static readonly ListProperty<OnOffDutyBCollectDetailViewModelList> OnOffDutyBCollectDetailViewModelListProperty = P<OnOffDutyBViewModel>.RegisterList(e => e.OnOffDutyBCollectDetailViewModelList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as OnOffDutyBViewModel).LoadOnOffDutyCollectDetailViewModelList()
        });

        /// <summary>
        /// B采集结果
        /// </summary>
        public OnOffDutyBCollectDetailViewModelList OnOffDutyBCollectDetailViewModelList
        {
            get { return this.GetLazyList(OnOffDutyBCollectDetailViewModelListProperty); }
        }

        /// <summary>
        /// B加载采集结果
        /// </summary>
        /// <returns>B采集结果列表</returns>
        private OnOffDutyBCollectDetailViewModelList LoadOnOffDutyCollectDetailViewModelList()
        {
            return new OnOffDutyBCollectDetailViewModelList();
        }
        #endregion

        #region IsOffDuty 是否上岗
        /// <summary>
        /// 是否上岗
        /// </summary>
        [Label("是否上岗")]
        public static readonly Property<bool> IsOnDutyProperty = P<OnOffDutyBViewModel>.Register(e => e.IsOnDuty, new PropertyMetadata<bool>() { PropertyChangedCallBack = (o, e) => (o as OnOffDutyBViewModel).OnIsOffDuty(e) });

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
        /// 条码变更事件，采集条码(员工工号)
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (Barcode.IsNullOrEmpty()) { return; }
            ClearInfos();
            try
            {
                var staff = RT.Service.Resolve<EmployeeController>().GetEmployeeByCode(Barcode.Trim());
                if (staff == null) { throw new ValidationException("系统不存在当前扫描员工".L10N()); }
                var editor = WorkCellSelectorControlFactory.CreateWipResourceListControl();
                var wipResourcesOfEmployee = RT.Service.Resolve<WipResourceController>().GetWipResources(staff.Id);
                if (wipResourcesOfEmployee != null & wipResourcesOfEmployee.Count > 0)
                {
                    List<BWipResource> bWipResources = new List<BWipResource>();
                    foreach (var resource in wipResourcesOfEmployee)
                    {
                        BWipResource bWipResource = new BWipResource();
                        bWipResource.Id = resource.Id;
                        bWipResource.Name = resource.Name;
                        //true上岗，资源是否存在上岗信息
                        var existDuty = RT.Service.Resolve<OnOffDutyBController>().GetExistDuty(staff.Id, resource.Id, true);
                        bWipResource.OnDutyStatusName = existDuty ? "在岗中..." : "";

                        bWipResources.Add(bWipResource);
                    }
                    //  editor.LeftResources = bWipResources;                   
                    editor.LeftResources = new System.Collections.ObjectModel.ObservableCollection<BWipResource>(bWipResources);
                }

                editor.IsOnDuty = IsOnDuty;
                editor.DutyStypeName = (IsOnDuty ? "上岗" : "下岗");
                editor.EmployeeId = staff.Id;
                editor.EmployeeName = staff.Name;
                editor.TileName = $"当前即将要{(IsOnDuty ? "上岗" : "下岗")}的用户：{staff.Name}";

                CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), editor, w =>
                {

                    w.Title = "选择资源".L10N();
                    w.Height = 700;
                    w.Width = 900;
                    w.MinHeight = 400;
                    w.MinWidth = 500;
                    w.Commands.Clear();
                    w.Closing += (object sender, System.ComponentModel.CancelEventArgs e) =>
                    {
                        if (editor.Result && editor.RightResources != null)
                        {
                            string DutyStype = IsOnDuty ? "上岗" : "下岗";
                            int successCount = 0, failCount = 0;
                            foreach (var item in editor.RightResources)
                            {
                                try
                                {
                                    var onOffDutyBRecrod = new OnOffDutyBRecrods();
                                    onOffDutyBRecrod.EmployeeId = staff.Id;
                                    onOffDutyBRecrod.ResourceId = item.Id;
                                    onOffDutyBRecrod.OnOffDutyType = IsOnDuty ? OnOffDutyBType.OnDuty : OnOffDutyBType.OffDuty;
                                    RT.Service.Resolve<OnOffDutyBController>().OnOffDuty(onOffDutyBRecrod, IsOnDuty);
                                    AddDetail(onOffDutyBRecrod);
                                    successCount += 1;
                                }
                                catch { failCount += 1; }
                            }
                            string msg = $"用户{staff.Name}【{DutyStype}】{editor.RightResources.Count}个资源，成功{successCount}个，失败{failCount}个。".L10N();
                            if (failCount > 0) { this.ShowError(msg); } else { this.ShowTips(msg); }
                        }
                    };
                });
            }
            catch (Exception ex) { ShowError(ex); }
            finally { Barcode = null; }
        }



        /// <summary>
        /// 添加采集结果记录
        /// </summary>
        /// <param name="onOffDutyBRecrods"></param>
        protected virtual void AddDetail(OnOffDutyBRecrods onOffDutyBRecrods)
        {
            OnOffDutyBCollectDetailViewModelList.Add(new OnOffDutyBCollectDetailViewModel
            {
                OnOffDutyType = onOffDutyBRecrods.OnOffDutyType,
                CollectUseName = RT.Identity.Name,
                InputDate = DateTime.Now,
                CollectDate = DateTime.Now,
                StaffNO = onOffDutyBRecrods.Employee.Code,
                StaffName = onOffDutyBRecrods.Employee.Name,
                ResourceName = onOffDutyBRecrods.Resource.Name
            });
        }




        /// <summary>
        /// 重新开始
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            base.Reset(ResetType.None);
            ShowTips(!IsOnDuty ? "请扫描下岗员工工号".L10N() : "请扫描上岗员工工号".L10N());
        }


    }
}
