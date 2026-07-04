using SIE.Domain;
using SIE.MES.Wip.Repairs;
using SIE.MES.WIP;
using SIE.MES.WIP.Repairs;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Repairs
{
    /// <summary>
    /// 维修提交视图模型
    /// </summary>
    [RootEntity, Serializable]
    public class RepairSubmitViewModel : ViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="barcode">维修视图模型</param>
        /// <param name="workcell">工作单元</param>
        public RepairSubmitViewModel(string barcode, Workcell workcell)
        {
            this.Id = Guid.NewGuid().ToString("N").ToUpper();

            var optionalPathViewModels = RT.Service.Resolve<RepairController>().GetOptionalPaths(barcode, workcell);

            OptionalPathViewModelList.AddRange(optionalPathViewModels);
        }

        #region 路径 OptionalPathViewModel
        /// <summary>
        /// 路径Id
        /// </summary>
        [Label("路径")]
        public static readonly IRefIdProperty OptionalPathViewModelIdProperty =
            P<RepairSubmitViewModel>.RegisterRefId(e => e.OptionalPathViewModelId, ReferenceType.Normal);

        /// <summary>
        /// 路径Id
        /// </summary>
        public string OptionalPathViewModelId
        {
            get { return (string)this.GetRefId(OptionalPathViewModelIdProperty); }
            set { this.SetRefId(OptionalPathViewModelIdProperty, value); }
        }

        /// <summary>
        /// 路径
        /// </summary>
        public static readonly RefEntityProperty<GotoChildProcessViewModel> OptionalPathViewModelProperty =
            P<RepairSubmitViewModel>.RegisterRef(e => e.OptionalPathViewModel, OptionalPathViewModelIdProperty);

        /// <summary>
        /// 路径
        /// </summary>
        public GotoChildProcessViewModel OptionalPathViewModel
        {
            get { return this.GetRefEntity(OptionalPathViewModelProperty); }
            set { this.SetRefEntity(OptionalPathViewModelProperty, value); }
        }
        #endregion

        #region 可选路径列表 OptionalPathViewModelList
        /// <summary>
        /// 可选路径列表
        /// </summary>
        [Label("可选路径列表")]
        public static readonly ListProperty<EntityList<GotoChildProcessViewModel>> OptionalPathViewModelListProperty
            = P<RepairSubmitViewModel>.RegisterList(e => e.OptionalPathViewModelList, new ListPropertyMeta
            {
                HasManyType = HasManyType.Aggregation,
                DataProvider = e => (e as RepairSubmitViewModel).LoadProductCategoryList()
            });

        /// <summary>
        /// 可选路径列表
        /// </summary>
        public EntityList<GotoChildProcessViewModel> OptionalPathViewModelList
        {
            get { return this.GetLazyList(OptionalPathViewModelListProperty); }
        }

        /// <summary>
        /// 
        /// </summary>
        private EntityList<GotoProcessViewModel> LoadProductCategoryList()
        {
            return new EntityList<GotoProcessViewModel>();
        }
        #endregion
    }
}