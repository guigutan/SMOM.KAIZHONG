using Microsoft.Win32;
using SIE.Barcodes;
using SIE.Common.ImportHelper;
using SIE.Domain.Validation;
using SIE.MES.WorkOrders;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 导入工单条码
    /// </summary>
    [Command(ImageName = "Import", Label = "导入条码", Hierarchy = "条码", GroupType = CommandGroupType.Edit)]
    public class ImportBarcodeCommand : ListViewCommand
    {
        /// <summary>
        /// 命令是否可执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view != null && view.Current != null && view.SelectedEntities.Count == 1 && view.Current is WorkOrder;
        }

        /// <summary>
        /// 命令执行块
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var workOrder = view.Current as WorkOrder;
            if (workOrder == null)
                return;
            var retrospectType = RT.Service.Resolve<WorkOrderController>().GetRetrospectType(workOrder.ProductId);
            if (retrospectType != null && retrospectType == Core.Items.RetrospectType.Batch)
            {
                CRT.MessageService.ShowError("工单[{0}]的产品[{1}]是批次类型，不能导入条码！".L10nFormat(workOrder.No, workOrder.Product.Name));
                return;
            }

            var openFileDialog = new OpenFileDialog();        //开窗
            openFileDialog.Filter = "xls|*.xls|xlsx|*.xlsx|csv|*.csv";  //可选择的文件后缀
            if (openFileDialog.ShowDialog() != true)
                return;

            try
            {
                ExcelHelper excelHelper = new ExcelHelper(openFileDialog.FileName);
                DataTable importTable = excelHelper.ExcelToDataTable(string.Empty, true);
                if (importTable.Columns.Count == 0 || importTable.Columns[0].ColumnName != "条码")
                    throw new ValidationException("选择的Excel文件不符合要求，请下载条码导入模板后按格式整理条码数据。".L10N());
                var sns = new List<string>();
                if (importTable != null && importTable.Rows.Count > 0)
                {
                    foreach (DataRow row in importTable.Rows)
                    {
                        var sn = row[0].ToString();
                        if (!sn.IsNullOrWhiteSpace())
                            sns.Add(sn);
                    }
                    RT.Service.Resolve<BarcodeController>().ImportBarcode(workOrder.Id, sns);
                    CRT.MessageService.ShowMessage("导入成功".L10N());
                }
            }
            catch (Exception exc)
            {
                exc.Alert();
            }
        }
    }
}