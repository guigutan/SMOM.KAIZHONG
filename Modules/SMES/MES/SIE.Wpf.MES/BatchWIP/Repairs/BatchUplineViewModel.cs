using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Repairs;
using SIE.MES.WIP.Repairs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Wpf.Common;
using System;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Repairs
{
    /// <summary>
    /// 返修完成视图模型
    /// </summary>
    [RootEntity, Serializable]
    public class BatchUplineViewModel : ViewModel
    {
        /// <summary>
        /// 批次维修ViewModel
        /// </summary>
        internal BatchRepairViewModel BatchRepairViewModel;

        /// <summary>
        /// 是否有下一工序
        /// </summary>
        internal bool HasNextProcess;

        /// <summary>
        /// 视图ViewKey
        /// </summary>
        internal string ViewKey;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchUplineViewModel() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="viewModel">维修视图模型</param>
        /// <param name="batch">转入批次</param>
        public BatchUplineViewModel(BatchRepairViewModel viewModel, InputBatch batch)
        {
            ViewKey = Guid.NewGuid().ToString("N");
            BatchRepairViewModel = viewModel;
            if (viewModel.Step.OutputCollectStep == null)
                throw new ValidationException("不存在出入类型为出站的采集步骤".L10N());
            IsContainer = viewModel.Step.OutputCollectStep.BarcodeType == BarcodeType.ContainerNo;

            var collectBarcode = new SIE.MES.WIP.CollectBarcode
            {
                Code = batch.SubBatchNo.IsNotEmpty() ? batch.SubBatchNo : batch.BatchNo,
                Type = BarcodeType.BatchBarocde
            };

            var process = RT.Service.Resolve<BatchRepairController>()
                .GetRoutingProcessList(collectBarcode, viewModel.GetWorkcell());

            //默认上线工序
            UplineProcess = process.FirstOrDefault(p => p.IsDefault);

            if (process.Any(x => x.IsDefault))
            {
                HasNextProcess = true;
            }

            ProcessList.AddRange(process);
        }

        #region Process 上线工序
        /// <summary>
        /// 上线工序ID
        /// </summary>
        public static readonly IRefIdProperty UplineProcessIdProperty =
            P<BatchUplineViewModel>.RegisterRefId(e => e.UplineProcessId, ReferenceType.Normal);

        /// <summary>
        /// 上线工序ID
        /// </summary>
        public string UplineProcessId
        {
            get { return (string)this.GetRefNullableId(UplineProcessIdProperty); }
            set { this.SetRefNullableId(UplineProcessIdProperty, value); }
        }

        /// <summary>
        /// 上线工序
        /// </summary>
        public static readonly RefEntityProperty<GotoProcessViewModel> UplineProcessProperty =
            P<BatchUplineViewModel>.RegisterRef(e => e.UplineProcess, UplineProcessIdProperty);

        /// <summary>
        /// 上线工序
        /// </summary>
        public GotoProcessViewModel UplineProcess
        {
            get { return this.GetRefEntity(UplineProcessProperty); }
            set { this.SetRefEntity(UplineProcessProperty, value); }
        }
        #endregion

        #region 关联载具 ContainerNo
        /// <summary>
        /// 关联载具
        /// </summary>
        [Label("关联载具")]
        public static readonly Property<string> ContainerNoProperty = P<BatchUplineViewModel>.Register(e => e.ContainerNo);

        /// <summary>
        /// 关联载具
        /// </summary>
        public string ContainerNo
        {
            get { return this.GetProperty(ContainerNoProperty); }
            set { this.SetProperty(ContainerNoProperty, value); }
        }
        #endregion

        #region 载具出站 IsContainer
        /// <summary>
        /// 载具出站
        /// </summary>
        [Label("载具出站")]
        public static readonly Property<bool> IsContainerProperty = P<BatchUplineViewModel>.Register(e => e.IsContainer);

        /// <summary>
        /// 载具出站
        /// </summary>
        public bool IsContainer
        {
            get { return this.GetProperty(IsContainerProperty); }
            set { this.SetProperty(IsContainerProperty, value); }
        }
        #endregion

        #region 工序清单 ProcessList
        /// <summary>
        /// 工序清单
        /// </summary>
        public static readonly ListProperty<EntityList<GotoProcessViewModel>> ProcessListProperty = P<BatchUplineViewModel>.RegisterList(e => e.ProcessList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as BatchUplineViewModel).LoadProcessList()
        });

        /// <summary>
        /// 工序清单
        /// </summary>
        public EntityList<GotoProcessViewModel> ProcessList
        {
            get { return this.GetLazyList(ProcessListProperty); }
        }

        /// <summary>
        /// 加载工序列表
        /// </summary>
        /// <returns>工序列表</returns>
        private EntityList<GotoProcessViewModel> LoadProcessList()
        {
            return new EntityList<GotoProcessViewModel>();
        }
        #endregion

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(ContainerNo) && ContainerNo.IsNotEmpty())
            {
                CRT.Workbench.CloseDialog(ViewKey, 0);
            }
        }
    }

    /// <summary>
    /// ViewModel实体配置
    /// </summary>
    internal class UplineViewModelConfig : EntityConfig<BatchUplineViewModel>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="rules">验证规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(BatchUplineViewModel.ContainerNoProperty, new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var model = o.CastTo<BatchUplineViewModel>();
                    if (model.HasNextProcess && model.ContainerNo.IsNullOrEmpty() && model.BatchRepairViewModel?.Step?.OutputCollectStep?.BarcodeType == BarcodeType.ContainerNo)
                        e.BrokenDescription = "转出条码类型为载具，关联载具不能为空！".L10N();
                }
            });

            rules.AddRule(BatchUplineViewModel.UplineProcessProperty, new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var model = o.CastTo<BatchUplineViewModel>();
                    if (model.HasNextProcess && model.UplineProcess == null)
                        e.BrokenDescription = "下一工序不是结束工序，上线工序不能为空！".L10N();
                }
            });
        }
    }

    /// <summary>
    /// 返修完工视图配置
    /// </summary>
    internal class UplineViewModelViewModelViewConfig : WPFViewConfig<BatchUplineViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("上线工序");
            View.HasDetailColumnsCount(1).ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.UplineProcess).HasLabel("上线工序").Show(ShowInWhere.All).UseDataSource(BatchUplineViewModel.ProcessListProperty);
                View.Property(p => p.ContainerNo).HasLabel("关联载具").UseBarcodeEditor().Show(ShowInWhere.All).UseFormSetting(p => p.Height = 40).Visibility(BatchUplineViewModel.IsContainerProperty);
            }
        }
    }
}
