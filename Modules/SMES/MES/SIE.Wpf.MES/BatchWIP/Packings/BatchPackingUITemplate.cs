using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using SIE.Packages;
using SIE.Wpf.MES.BatchWIP.Packings.Layouts;
using SIE.Wpf.MES.WIP.Packings;
using System;
using System.Linq;
using static Stimulsoft.Report.WpfDesign.StiBuilder;

namespace SIE.Wpf.MES.BatchWIP.Packings
{
    /// <summary>
    /// 批次包装模板
    /// </summary>
    public class BatchPackingUITemplate : BatchCollectionUITemplate
    {
        /// <summary>
        /// 包装关系视图
        /// </summary>
        ListLogicalView packingRelationView;

        /// <summary>
        /// 批次包装ViewModel
        /// </summary>
        BatchPackingViewModel packingViewData;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchPackingUITemplate() : base(typeof(BatchPackingViewModel))
        {
            ViewGroup = BatchPackingModelViewConfig.PackingView;
        }

        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var block = base.DefineBlocks();
            block.Layout = new LayoutMeta(typeof(BatchPackingLayout));
            return block;
        }

        /// <summary>
        /// UI创建后
        /// </summary>
        /// <param name="ui">ui</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            packingViewData = new BatchPackingViewModel();
            var packingView = ui.MainView as DetailLogicalView;
            packingView.Data = packingViewData;

            var layout = ui.Control as BatchPackingLayout;

            var inputBatchUI = new ListUITemplate(typeof(InputBatch), InputBatchViewConfig.BatchPackingView, packingView.ModuleKey).CreateUI();
            inputBatchUI.MainView.Data = packingViewData.InputBatchList;
            inputBatchUI.MainView.Relations.Add(new RelationView("mainView", packingView));
            layout.InitBatchListView(inputBatchUI);

            var pkgRelaUI = new ListUITemplate(typeof(BatchPackingRelation), BatchPackingRelationViewConfig.BatchPackingView, packingView.ModuleKey).CreateUI();
            pkgRelaUI.MainView.Data = packingViewData.PackingRelationList;
            pkgRelaUI.MainView.Relations.Add(new RelationView("mainView", packingView));
            layout.InitPackingListView(pkgRelaUI);

            var pkgRuleUI = new ListUITemplate(typeof(WorkOrderPackageRuleDetail), PackageRuleDetailViewConfig.BatchPackingView, packingView.ModuleKey).CreateUI();
            pkgRuleUI.MainView.Data = packingViewData.PackageRuleDetailList;
            pkgRuleUI.MainView.Relations.Add(new RelationView("mainView", packingView));
            layout.InitPkgRuleDetailView(pkgRuleUI);
            
            CreateReportTaskControl(layout.childrenView, ui.MainView, packingViewData, "CollectionView");

            //如果不用显示消息列表，则注释下面这句
            layout.childrenView.Items.Add(CreateTabItem("消息列表", CreateMessagerControl(packingViewData)));

            var workstation = CreateOperationControl(packingViewData.Workstation);
            layout.InitWorkstation(workstation);
            IsExistRelationView(packingView);
            base.OnUIGenerated(ui);
        }

        /// <summary>
        /// 设置变量
        /// </summary>
        /// <exception cref="ValidationException">
        /// 1、包装关系未配置权限
        /// 2、包装规则未配置权限
        /// 3、物料标签未配置权限
        /// </exception>
        private void IsExistRelationView(DetailLogicalView packingView)
        {
            packingRelationView = packingView.ChildrenViews.FirstOrDefault(f => f.EntityType == typeof(BatchPackingRelation)) as ListLogicalView;
            if (packingRelationView == null)
                throw new ValidationException("[批次包装清单]块无权限,请联系管理员配置".L10N());
            packingRelationView.Data = packingViewData.PackingRelationList;
            if (packingView.ChildrenViews.Any(f => f.EntityType == typeof(PackageRuleDetail)))
                throw new ValidationException("[包装规则]块无权限,请联系管理员角色权限配置".L10N());
            var itemLableView = packingView.ChildrenViews.FirstOrDefault(f => f.EntityType == typeof(SIE.MES.BatchWIP.InputBatch)) as ListLogicalView;
            if (itemLableView == null)
                throw new ValidationException("[工位批次清单]块无权限,请联系管理员配置".L10N());
        }
    }
}
