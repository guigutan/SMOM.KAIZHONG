using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Grid;
using SIE.Domain;
using SIE.MES.Workbench.ProductingReadies;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.Workbench.ShiftProductions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace SIE.Wpf.MES.Workbench.ProductingReadies
{
    /// <summary>
    /// 产前准备 的交互逻辑
    /// </summary>
    [Category("过程分析")]
    public partial class ProductingReadyControl : ComponentItem, IfaceKeyEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductingReadyControl()
        {
            InitializeComponent();
            UseProperty<ProductingReadyProperty>();

        }

        #region 产前准备信息集合
        /// <summary>
        ///产前准备信息集合
        /// </summary>
        private ObservableCollection<ProductingReadyInfo> _entityinfos;

        /// <summary>
        /// 产前准备信息集合
        /// </summary>
        public ObservableCollection<ProductingReadyInfo> EntityInfos
        {
            get
            {
                if (_entityinfos == null)
                {
                    _entityinfos = new ObservableCollection<ProductingReadyInfo>();
                }

                return _entityinfos;
            }
        }
        #endregion

        public override void Refresh()
        {
            LoadProductingReady();
        }

        private void LoadProductingReady()
        {
            EntityInfos.Clear();
            EntityList<ProductingReady> entity = RT.Service.Resolve<ProductingReadyController>().GetProductingReadies();
            foreach (var en in entity)
            {
                ProductingReadyInfo ei = new ProductingReadyInfo();
                ei.WorkOrderNo = en.WorkOrder.No;
                ei.LineName = en.WorkOrder.Resource.Name;
                ei.ItemCode = en.WorkOrder.Product.Code;
                ei.ProductQty = en.WorkOrder.PlanQty;
                ei.ItemState = en.ItemState;
                ei.ToolState = en.ToolState;
                ei.PersonnelState = en.EmployeeState;
                ei.EsopState = en.EsopState;
                ei.QualityState = en.QualityState;
                EntityInfos.Add(ei);
            }
            ctlProductReady.ItemsSource = EntityInfos;
            var curTblView = ctlProductReady.View as TableView;
            SetTableViewStyle(curTblView);
        }

        /// <summary>
        /// TableView的列栏位格式定义
        /// </summary>
        /// <param name="curTableView"> TableView</param>
        public void SetTableViewStyle(TableView curTableView)
        {
            //检验项目的检验结果为不合格时，将检验结果标红
            FormatCondition conditionItemState = SetReadyStateFormatCondition(nameof(ProductingReadyInfo.ItemState));
            FormatCondition conditionToolState = SetReadyStateFormatCondition(nameof(ProductingReadyInfo.ToolState));
            FormatCondition conditionPersonnelState = SetReadyStateFormatCondition(nameof(ProductingReadyInfo.PersonnelState));
            FormatCondition conditionEsopState = SetReadyStateFormatCondition(nameof(ProductingReadyInfo.EsopState));
            FormatCondition conditionQualityState = SetReadyStateFormatCondition(nameof(ProductingReadyInfo.QualityState));

            curTableView.FormatConditions.Clear();
            curTableView.FormatConditions.Add(conditionItemState);
            curTableView.FormatConditions.Add(conditionToolState);
            curTableView.FormatConditions.Add(conditionPersonnelState);
            curTableView.FormatConditions.Add(conditionEsopState);
            curTableView.FormatConditions.Add(conditionQualityState);
        }

        /// <summary>
        /// 列栏位格式定义
        /// </summary>
        /// <param name="fieldName">列栏位名称</param>
        /// <returns>列格式</returns>
        private FormatCondition SetReadyStateFormatCondition(string fieldName)
        {
            FormatCondition conditionReadyState = new FormatCondition()
            {
                FieldName = fieldName,
                ValueRule = ConditionRule.Equal,
                Value1 = ReadyState.NG,
                ApplyToRow = false,
                Format = new Format() { Foreground = Brushes.Red }
            };
            return conditionReadyState;
        }
    }

    /// <summary>
    /// 产前准备属性
    /// </summary>
    public class ProductingReadyProperty : ComponentProperty<ProductingReadyControl>
    {
    }
}