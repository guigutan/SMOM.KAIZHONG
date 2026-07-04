using SIE.Domain;
using SIE.LES.LinesideWarehouses.Models;
using SIE.LES.MaterialPreparations;
using SIE.LES.MaterialPreparations.ApiModels;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.LES.MaterialPreparations
{
    /// <summary>
    /// 备料需求单
    /// </summary>
    public class MaterialPreparationDataqueryer : DataQueryer
    {
        /// <summary>
        /// 获取工单bom信息
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="preType">0-生产领料 1-生产超领 2-车间领料</param>
        /// <returns></returns>
        public virtual EntityList<MaterialPreparationDetail> GetWorkOrderBomPrepration(double woId, int preType)
        {
            return RT.Service.Resolve<MaterialPreparationController>().GetWorkOrderBomPrepration(woId, preType);
        }

        /// <summary>
        /// 根据车间产线获取产线线边仓
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="wipResourceId">产线Id</param>
        /// <returns></returns>
        public LinesideWareBaseData GetLinesideWarehouse(double? workShopId, double? wipResourceId)
        {
            if (workShopId == null && wipResourceId == null)
            {
                return new LinesideWareBaseData();
            }
            return RT.Service.Resolve<MaterialPreparationController>().GetLinesideWarehouse(workShopId, wipResourceId);
        }

        /// <summary>
        /// 导出命令
        /// </summary>
        /// <param name="rangeOption">导出类型</param>
        /// <param name="selectEntity">当前选择行</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="currentpage">当前页</param>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        public List<EntityJson> GetExportData(int rangeOption, EntityList<MaterialPreparation> selectEntity, int pagesize, int currentpage, MaterialPreparationCriteria criteria)
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
                EntityList<MaterialPreparation> datas = RT.Service.Resolve<MaterialPreparationController>().MaterialPreparationQuery(criteria);
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
                EntityList<MaterialPreparation> datas = RT.Service.Resolve<MaterialPreparationController>().MaterialPreparationQuery(criteria);
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
        private string Export(EntityList<MaterialPreparation> datas)
        {
            StringBuilder exportData = new StringBuilder();
            exportData.Append("<tr style='background-color:#B3B3B3'><td>备料单号</td><td>工单</td><td>车间</td><td>资源</td><td>状态</td>"
                + "<td>备料类型</td><td>备料原因</td><td>需求时间</td><td>发货仓库</td><td>产品编码</td><td>产品名称</td><td>工单状态</td>"
                + "<td>计划数量</td><td>完工数量</td><td>工单类型</td><td>计划开始时间</td><td>计划结束时间</td><td>实际开始时间</td><td>实际结束时间</td>"
                + "<td>销售订单号</td><td>客户订单号</td><td>工厂</td><td>创建人</td><td>创建时间</td><td>修改人</td><td>修改时间</td>"
                + "<td>备料状态</td><td>行号</td><td>物料编码</td><td>物料名称</td><td>单位</td><td>物料扩展属性</td>"
                + "<td>本次备料量</td><td>接收数</td><td>拒收数</td><td>待收数</td><td>发料数</td><td>取消数</td></tr>");
            var dataIds = datas.Select(p => p.Id).ToList();
            var details = RT.Service.Resolve<MaterialPreparationController>().GetMaterialPreDetail(dataIds);
            foreach (var data in datas)
            {
                var childs = details.Where(p => p.MaterialPreparationId == data.Id).ToList();
                string dataLine = string.Empty;
                string parentString = ("<tr>"
                    + "<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>"
                    + "<td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td>"
                    + "<td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td>"
                    + "<td>{12}</td><td>{13}</td><td>{14}</td><td>{15}</td>"
                    + "<td>{16}</td><td>{17}</td><td>{18}</td><td>{19}</td>"
                    + "<td>{20}</td><td>{21}</td><td>{22}</td><td>{23}</td>"
                    + "<td>{24}</td><td>{25}</td>")
                    .FormatArgs(
                        data.No, data.WoNo, data.WorkShopName, data.ResourceCode,
                        data.PrepareStatus.ToLabel(), data.PrepareType.ToLabel(), data.Reason, data.PrepareTime.ToString(),
                        data.WarehouseName, data.WoProductCode, data.WoProductName, data.WoState.ToLabel(),
                        data.WoPlanQty, data.WoFinishQty, data.WoType.ToLabel(), data.WoPlanBeginDate.ToString(),
                        data.WoPlanEndDate.ToString(), data.WoActuStartDate.ToString(), data.WoActuFinishDate.ToString(), data.WoSaleOrderNo,
                        data.WoCustomerOrderNo, data.FactoryName, data.CreateByName, data.CreateDate.ToString(),
                        data.UpdateByName, data.UpdateDate.ToString()
                    );
                if (childs.Any())
                {
                    foreach (var child in childs)
                    {
                        string childString = ("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td>"
                            + "<td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td></tr>")
                            .FormatArgs(child.PreDetailStatus.ToLabel(), child.LineNo, child.ItemCode, child.ItemName, child.UnitName, child.ItemExtPropName
                            , child.Qty, child.ReceiveQty, child.RefuseQty, child.ToReceiveQty, child.ShippingQty, child.CancelQty);
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
