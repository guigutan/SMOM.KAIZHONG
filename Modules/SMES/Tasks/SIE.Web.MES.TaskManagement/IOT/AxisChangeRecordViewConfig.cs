using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.IOT;
using SIE.MetaModel.View;
using SIE.Utils;
using SIE.Web.MES.TaskManagement.Dispatchs;

namespace SIE.Web.MES.TaskManagement.IOT
{
    /// <summary>
    /// 
    /// </summary>
    public class AxisChangeRecordViewConfig : WebViewConfig<AxisChangeRecord>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.IotEntity).ShowInList(150).Readonly();
                View.Property(p => p.TaskNo).ShowInList(180);
                View.Property(p => p.MeterCount).Show();
                View.Property(p => p.AxisQty).Show();
                View.Property(p => p.ChangeFlag).Show();
                View.Property(p => p.IsReport).ShowInList().HasLabel("是否已处理");
                View.Property(p => p.ReportQty).ShowInList();
                View.Property(p => p.Remark).ShowInList(200);
                View.Property(p => p.ResourceName).ShowInList();
                View.Property(p => p.ResourceCode).ShowInList();
                View.Property(p => p.CollectionTime).ShowInList(150);
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        //protected override void ConfigQueryView()
        //{
        //    using (View.OrderProperties())
        //    {
        //        View.Property(p => p.IotEntity).Show();
        //        View.Property(p => p.TaskNo).Show();
        //        View.Property(p => p.ChangeFlag).UseCheckDropDownEditor().Show();
        //        View.Property(p => p.IsReport).UseCheckDropDownEditor().Show().HasLabel("是否已处理");
        //        View.Property(p => p.Factory).ShowInList();
        //        View.Property(p => p.CollectionTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show();
                
        //    }
        //}
    }
}
