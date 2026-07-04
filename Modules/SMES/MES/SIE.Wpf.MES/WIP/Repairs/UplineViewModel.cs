using SIE.Domain;
using SIE.MES.WIP.Repairs;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WIP.Repairs
{
    /// <summary>
    /// 返修完成视图模型
    /// </summary>
    [RootEntity, Serializable]
    public class UplineViewModel : ViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UplineViewModel() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="viewModel">维修视图模型</param>
        public UplineViewModel(RepairViewModel viewModel)
        {
            var process = RT.Service.Resolve<RepairController>()
                .GetRoutingProcessList(viewModel.SubmitBarcode.Code, viewModel.GetWorkcell());

            //默认上线工序
            UplineProcess = process.FirstOrDefault(p => p.IsDefault);

            ProcessList.AddRange(process);
        }

        #region Process 上线工序
        /// <summary>
        /// 上线工序ID
        /// </summary>
        [Required]
        public static readonly IRefIdProperty UplineProcessIdProperty =
            P<UplineViewModel>.RegisterRefId(e => e.UplineProcessId, ReferenceType.Normal);

        /// <summary>
        /// 上线工序ID
        /// </summary>
        public string UplineProcessId
        {
            get { return (string)this.GetRefId(UplineProcessIdProperty); }
            set { this.SetRefId(UplineProcessIdProperty, value); }
        }

        /// <summary>
        /// 上线工序
        /// </summary>
        public static readonly RefEntityProperty<GotoProcessViewModel> UplineProcessProperty =
            P<UplineViewModel>.RegisterRef(e => e.UplineProcess, UplineProcessIdProperty);

        /// <summary>
        /// 上线工序
        /// </summary>
        public GotoProcessViewModel UplineProcess
        {
            get { return this.GetRefEntity(UplineProcessProperty); }
            set { this.SetRefEntity(UplineProcessProperty, value); }
        }
        #endregion

        #region 工序清单 ProcessList
        /// <summary>
        /// 工序清单
        /// </summary>
        public static readonly ListProperty<EntityList<GotoProcessViewModel>> ProcessListProperty = P<UplineViewModel>.RegisterList(e => e.ProcessList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as UplineViewModel).LoadProcessList()
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
    }

    /// <summary>
    /// 返修完工视图配置
    /// </summary>
    internal class UplineViewModelViewModelViewConfig : WPFViewConfig<UplineViewModel>
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
                View.Property(p => p.UplineProcess)
                    .HasLabel("上线工序")
                    .Show(ShowInWhere.All)
                    .UseDataSource(UplineViewModel.ProcessListProperty);
                View.Property(p => p.UplineProcess.PathDescription)
                    .HasLabel("后工序")
                    .Show(ShowInWhere.All)
                    .Readonly();
            }
        }
    }
}
