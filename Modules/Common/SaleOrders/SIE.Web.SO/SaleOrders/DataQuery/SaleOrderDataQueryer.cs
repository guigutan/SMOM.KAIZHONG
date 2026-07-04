using SIE.Core.Common.Models;
using SIE.SO.SaleOrders;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Web.SO.SaleOrders.DataQuery
{
    /// <summary>
    /// 销售订单数据导出
    /// </summary>
    public class SaleOrderDataQueryer : DataQueryer
    {
        /// <summary>
        /// 导出销售订单
        /// </summary>
        /// <param name="criter">销售订单查询实体</param>
        /// <returns>导出数据</returns>
        public virtual ExportDataTable ExportSaleOrder(SaleOrderCriteria criter)
        {
            criter.IsExport = true;
            var stores = RT.Service.Resolve<SaleOrderController>().GetSaleOrderReachViewModel(criter);
            ExportDataTable exportDataTable = new ExportDataTable();
            List<DataTable> dataTables = new List<DataTable>();
            DataTable dataTable = new DataTable();
            List<string> arr = new List<string>()
            {  "序号","销售订单", "行号", "客户编号","销售人员",
               "物料编码","物料名称","版本号", "行业类型", "订单类型",
                "产品类型","产品等级",
                "是否新单","行状态","数量","单位", 
                "库存组织代号","库存组织名称","MI完成时间",
                "总面积M2","大板尺寸","开料PNL数",
                "SETPNL数","PCSPNL数","客户交期","承诺交期","订单行挂起"};

            var dataColumnCount = arr.Count; //数据列数  
            string[] columns = new string[dataColumnCount];
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = arr[i].L10N();
                dataTable.Columns.Add(columns[i]);
            }
            int XH = 1;
            //行数据
            stores.ForEach(exportDate =>
                {
                    var row = dataTable.NewRow();
                    row[0] = XH;
                    row[1] = exportDate.Code;
                    row[2] = exportDate.LineNo;
                    row[3] = exportDate.Customer;
                    row[4] = exportDate.Employee;

                    row[5] = exportDate.ItemCode;
                    row[6] = exportDate.ItemName;
                    row[7] = exportDate.Version;
                    row[8] = exportDate.IndustryType;
                    row[9] = exportDate.OrderType;

                    row[10] = exportDate.ProductType;
                    row[11] = exportDate.ProductLevel;
                    //row[12] = exportDate.SpecialProcess;
                    row[12] = exportDate.IsNew;
                    row[13] = exportDate.LineState;
                    row[14] = exportDate.Qty;

                    row[15] = exportDate.Unit;
                    //row[17] = exportDate.TargetOrderCode;
                    row[16] = exportDate.EnterpriseCode;
                    row[17] = exportDate.EnterpriseName;
                    row[18] = exportDate.MiDateTime;

                    row[19] = exportDate.Area;
                    row[20] = exportDate.PlateSize;
                    row[21] = exportDate.MaterialPnl;

                    row[22] = exportDate.SetPnl;
                    row[23] = exportDate.PcsPnl;
                    row[24] = exportDate.RequireDelivery;
                    row[25] = exportDate.PromiseDelivery;
                    row[26] = exportDate.IsHangUp;
                    XH++;
                    dataTable.Rows.Add(row);
                });
            exportDataTable.SheetNames.Add("sheet");
            exportDataTable.Tables.Add(dataTable);
            exportDataTable.Columns.Add(columns);
            return exportDataTable;
        }
    }
}
