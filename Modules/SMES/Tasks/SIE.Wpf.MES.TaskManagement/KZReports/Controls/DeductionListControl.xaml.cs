using DevExpress.XtraEditors.Repository;
using SIE.Domain.Validation;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.Datas;
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
    /// DeductionListControl.xaml 的交互逻辑
    /// </summary>
    public partial class DeductionListControl : UserControl
    {
        KZTaskReportViewModelBase model;
        KZReportHelper kZReportHelper;

        protected virtual ObservableCollection<CsDeductionLabelInfo> LabelInfos { get; set; }

        public DeductionListControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        public DeductionListControl(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = _model;
            kZReportHelper = model.kZReportHelper;

            if (model.ResourceId == null)
                return;

            LabelInfos = new ObservableCollection<CsDeductionLabelInfo>();
            LoadData(model.ResourceId.Value);
            this.dataGridRecord.ItemsSource = LabelInfos;
        }

        private void dataGridRecord_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //var info = this.dataGridRecord.SelectedItem as CsDeductionLabelInfo;
            //info.IsSelected = true;
            //LabelInfos.FirstOrDefault(p => p.RecordId == info.RecordId).IsSelected = true;

            //this.dataGridRecord.ItemsSource = LabelInfos;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="ResourceId"></param>
        public void LoadData(double ResourceId)
        {
            LabelInfos.Clear();
            var pdaInfos = RT.Service.Resolve<DispatchController>().BlankingGetFeedRecords(ResourceId, null, true);
            foreach (var pdaInfo in pdaInfos)
            {
                CsDeductionLabelInfo info = new CsDeductionLabelInfo();
                info.Sn = pdaInfo.Label;
                info.ItemCode = pdaInfo.ItemCode;
                info.ItemDesc = pdaInfo.ItemDesc;
                info.ItemLabelLot= pdaInfo.ItemLabelLot;
                info.FeedingTime= pdaInfo.FeedingTime;
                info.Qty = pdaInfo.FeedingQty ?? 0;
                info.RemainingQty = pdaInfo.RemainingQty ?? 0;
                info.RecordId = pdaInfo.RecordId;
                info.IsSelected = false;
                LabelInfos.Add(info);
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LabelInfos == null || LabelInfos.Count < 1 || LabelInfos.All(p => p.IsSelected == false))
                    throw new ValidationException("没有可提交的数据!".L10N());
                var RecordIds = LabelInfos.Where(p => p.IsSelected == true).Select(p => p.RecordId).Distinct().ToList();
                //下料提交
                RT.Service.Resolve<DispatchController>().BlankingSubmit(RecordIds);
                reset();
                //LoadData(model.ResourceId.Value);
            }
            catch (Exception ex)
            {
                CRT.MessageService.ShowError(ex.GetBaseException().Message);
                return;
            }
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReStart_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        /// <summary>
        /// 重置
        /// </summary>
        void reset()
        {
            LoadData(model.ResourceId.Value);
            //LabelInfos.Select(p => p.IsSelected = false);
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
