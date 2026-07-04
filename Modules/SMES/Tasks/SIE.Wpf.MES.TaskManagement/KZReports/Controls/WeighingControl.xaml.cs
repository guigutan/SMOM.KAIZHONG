using DocumentFormat.OpenXml.EMMA;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs.Datas;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Configs;
using SIE.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// WeighingControl.xaml 的交互逻辑
    /// </summary>
    public partial class WeighingControl : UserControl
    {

        KZTaskReportViewModelBase model;

        /// <summary>
        /// 工序BOM列表
        /// </summary>
        protected virtual ObservableCollection<CsWeighingProcessBomInfo> ProcessBomInfos { get; }

        

        public WeighingControl()
        {
            InitializeComponent();

        }

        public WeighingControl(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();

            this.model = _model;
            ProcessBomInfos = new ObservableCollection<CsWeighingProcessBomInfo>();

            //加载主表数据
            loadMainData();
            //加载工序Bom数据
            loadProcessBomInfos();
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReStart_Click(object sender, RoutedEventArgs e)
        {
            //加载主表数据
            loadMainData();
            //加载工序Bom数据
            loadProcessBomInfos();
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //找出修改的数据
            var datas = ProcessBomInfos.Where(p => p.IsDirty == true).ToList();
            if (datas.Count < 1)
            {
                MessageBox.Show("无可提交数据!".L10N());
                return;
            }
            if (datas.All(p => p.Weight == 0))
            {
                MessageBox.Show("取样净重不能都为0,为0数据不做更新!".L10N());
                return;
            }
            var errDatas = datas.Where(p =>
            {
                var values = p.WeightScope.Split('-').Select(p => Convert.ToDecimal(p)).ToList();
                if (p.Weight < values[0] || p.Weight > values[1])
                    return true;
                return false;
            }).ToList();

            if (errDatas.Count > 0)
            {
                List<string> errs = new List<string>();
                foreach (var errData in errDatas)
                {
                    errs.Add("物料{0}取样净重范围为[{1}]，当前为[{2}]".L10nFormat(errData.ItemCode, errData.WeightScope, errData.Weight));
                }
                MessageBox.Show(string.Join(Environment.NewLine, errs));
                return;
            }

            datas = datas.Where(p => p.Weight != null && p.Weight > 0).ToList();
            foreach (var data in datas)
            {
                //称重修改工序bom重量改为修改产品重量
                //var processBom = model.DispatchTask.WorkOrder.ProcessBomList.Where(p => p.Id == data.ProcessBomId).FirstOrDefault();
                //创建报表记录，更新工序BOM取样净重,必须要先存在报表里面，才能更新工序BOM的取样净重
                RT.Service.Resolve<ReportController>().CreateWeightOfSamplingReport(data.ProcessBomId, data.Weight ?? 0, model.DispatchTaskId.Value, data._oldWeight ?? 0);
                //存起来，保证在未关闭界面的情况下，工序BOM的数据是新的
                //processBom.Weight = data.Weight;
            }
            MessageBox.Show("提交成功".L10N());
            //加载主表数据
            loadMainData();
            //加载工序Bom数据
            loadProcessBomInfos();
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            close();
        }

        /// <summary>
        /// 加载主表数据
        /// </summary>
        private void loadMainData()
        {
            this.ProductName.Content = this.model.DispatchTask?.WorkOrder?.Product?.Name;
            this.ProductCode.Content = this.model.DispatchTask?.WorkOrder?.Product?.Code;
            this.WorkOrderNo.Content = this.model.DispatchTask?.WorkOrder?.No;
            this.PlanQty.Content = this.model.DispatchTask?.WorkOrder?.PlanQty;
        }

        /// <summary>
        /// 加载工序BOM
        /// </summary>
        private void loadProcessBomInfos()
        {
            ProcessBomInfos.Clear();

            var configValue = ConfigService.GetConfig(new ProcessBomWeightConfig(), typeof(WorkOrder));
            //var processBoms = RT.Service.Resolve<WorkOrderController>().GetWoProcessBom(model.DispatchTask.WorkOrderId.Value);
            //foreach (var processBom in processBoms)
            //{
            //    CsWeighingProcessBomInfo info = new CsWeighingProcessBomInfo();
            //    info.ProcessBomId = processBom.Id;
            //    info.ItemCode = processBom.Item?.Code;
            //    info.ItemName = processBom.Item?.Name;
            //    var bom = model.DispatchTask.WorkOrder.BomList.FirstOrDefault(p => p.ItemId == processBom.ItemId);
            //    if (bom != null)
            //        info.RequireQty = bom.RequireQty;
            //    info.SingleQty = processBom.SingleQty;
            //    info.Unit = processBom.Unit?.Code;
            //    info.Process = processBom.Process.Name;
            //    info._oldWeight = processBom.Weight;
            //    info._newWeight = processBom.Weight;
            //    info.WeightScope = processBom.SingleQty + "-" + processBom.SingleQty;
            //    if (configValue != null && configValue.Scope > 0)
            //    {
            //        var value = processBom.SingleQty * (configValue.Scope.Value / 100);
            //        info.WeightScope = (processBom.SingleQty - value) + "-" + (processBom.SingleQty + value);
            //    }

            //    ProcessBomInfos.Add(info);
            //}


            CsWeighingProcessBomInfo info = new CsWeighingProcessBomInfo();
            //info.ProcessBomId = processBom.Id;
            info.ItemCode = model.DispatchTask.Product?.Code;
            info.ItemName = model.DispatchTask.Product?.Name;
            //var bom = model.DispatchTask.WorkOrder.BomList.FirstOrDefault(p => p.ItemId == processBom.ItemId);
            //if (bom != null)
            //    info.RequireQty = bom.RequireQty;
            //info.SingleQty = processBom.SingleQty;
            info.Unit = model.DispatchTask.Product?.Unit?.Code;
            //info.Process = processBom.Process.Name;
            info._oldWeight = model.DispatchTask.Product?.Weight ?? 0;
            info._newWeight = model.DispatchTask.Product?.Weight ?? 0;
            info.WeightScope = info._oldWeight + "-" + info._oldWeight;
            if (configValue != null && configValue.Scope > 0)
            {
                var value = info._oldWeight * (configValue.Scope.Value / 100);
                info.WeightScope = (info._oldWeight - value) + "-" + (info._oldWeight + value);
            }

            ProcessBomInfos.Add(info);

            this.dataGrid.ItemsSource = ProcessBomInfos;
        }

        void close()
        {
            var parent = Window.GetWindow(this);
            if (parent != null && parent is Window)
            {
                (parent as Window)?.Close();
            }
        }
    }
}
