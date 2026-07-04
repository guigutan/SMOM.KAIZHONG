using SIE.Domain;
using SIE.LES.MaterialPreparations;
using SIE.LES.MaterialPreparations.ApiModels;
using SIE.LES.MaterialPreparations.ViewModels;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialPreparations
{
    /// <summary>
    /// 工单备料汇总Query
    /// </summary>
    public class MaterialWoMpDataqueryer : DataQueryer
    {
        /// <summary>
        /// 导出命令
        /// </summary>
        /// <param name="rangeOption">导出类型</param>
        /// <param name="selectEntity">当前选择行</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="currentpage">当前页</param>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        public List<EntityJson> GetExportData(int rangeOption, EntityList<WorkOrderMpViewModel> selectEntity, int pagesize, int currentpage, WorkOrderMpViewModelCriteria criteria)
        {
            List<EntityJson> res = new List<EntityJson>();
            StringBuilder sb = new StringBuilder();
            ////定义表头
            const string head = "<table>";
            sb.Append(head);
            if (rangeOption == (int)MpExportInfo.Current) // 当前页
            {
                criteria.PagingInfo.PageSize = pagesize;
                criteria.PagingInfo.PageNumber = currentpage;
                EntityList<WorkOrderMpViewModel> datas = RT.Service.Resolve<MaterialPreparationController>().GetWorkOrderMpViewModels(criteria);
                sb.Append(Export(datas));
            }
            else if (rangeOption == (int)MpExportInfo.Selected) // 当前选择
            {
                if (selectEntity.Count > 0)
                {
                    sb.Append(Export(selectEntity));
                }

            }
            else // 导出全部
            {
                criteria.PagingInfo = null;
                EntityList<WorkOrderMpViewModel> datas = RT.Service.Resolve<MaterialPreparationController>().GetWorkOrderMpViewModels(criteria);
                sb.Append(Export(datas));
            }

            sb.Append("</table>");
            EntityJson resNode = new EntityJson();
            resNode.SetProperty("exportData", sb.ToString());
            res.Add(resNode);
            return res;
        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="datas">导出数据</param>
        /// <returns></returns>
        private string Export(EntityList<WorkOrderMpViewModel> datas)
        {
            StringBuilder exportData = new StringBuilder();
            exportData.Append("<tr style='background-color:#B3B3B3'><td>工单</td><td>产品编码</td><td>产品名称</td>" +
                "<td>车间</td><td>资源</td><td>工单状态</td><td>计划数量</td><td>完工数量</td><td>工单类型</td><td>计划开始时间</td><td>计划完成时间</td>" +
                "<td>实际开始时间</td><td>实际完成时间</td><td>销售订单号</td><td>客户订单号</td><td>工厂</td>" +
                "<td>行号</td><td>物料编码</td><td>物料名称</td><td>单位</td><td>物料扩展属性</td><td>物料消耗方式</td><td>需求量</td>" +
                "<td>已建备料数</td><td>可备料数</td><td>已接收数</td><td>待接收数</td><td>已发料数</td><td>取消数</td><td>退料数</td>" +
                "</tr>");
            var woIds = datas.Select(p => p.Id).ToList();
            var details = RT.Service.Resolve<MaterialPreparationController>().GetWorkOrderMpDetailViewModels(woIds);
            foreach (var data in datas)
            {
                var childs = details.Where(p => p.WoId == data.Id).ToList();
                string dataLine = string.Empty;
                var parentString = ("<tr>" +
                    "<td>{0}</td><td>{1}</td><td>{2}</td>" +
                    "<td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td>" +
                    "<td>{11}</td><td>{12}</td><td>{13}</td><td>{14}</td><td>{15}</td>"
                    ).FormatArgs(data.No, data.ProductCode, data.ProductName,
                    data.WorkShopName, data.ResourceName, data.WoState.ToLabel(), data.PlanQty, data.FinishQty, data.WoType, data.PlanBeginDate.ToString("yyyy-MM-dd HH:mm:ss"), data.PlanEndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    data.ActuStartDate != null ? data.ActuStartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "", data.ActuFinishDate != null ? data.ActuFinishDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "", data.SaleOrderNo, data.CustomerOrderNo, data.FactoryName);

                if (childs.Any())
                {
                    foreach(var child in childs)
                    {
                        var childString = ("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td>" +
                            "<td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td><td>{12}</td><td>{13}</td>" +
                            "</tr>").FormatArgs(child.LineNo, child.ItemCode, child.ItemName, child.UnitName, child.ItemExtPropName, child.ConsumeMode.ToLabel(), child.BomNeedQty,
                            child.HasQty, child.CanPrepareQty, child.HasReceiveQty, child.ToReceiveQty, child.HasShippingQty, child.CancelQty, child.ReturnQty);
                        dataLine = parentString + childString;
                        exportData.Append(dataLine);
                    }
                }
                else
                {
                    dataLine = parentString + "</tr>";
                    exportData.Append(dataLine);
                }
            }
            return exportData.ToString();
        }
    }
}
