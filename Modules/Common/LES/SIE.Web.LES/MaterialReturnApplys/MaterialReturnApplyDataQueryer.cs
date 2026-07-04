using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.LES.LinesideWarehouses.Models;
using SIE.LES.MaterialPreparations.ApiModels;
using SIE.LES.MaterialPreparations;
using SIE.LES.MaterialPreparations.ViewModels;
using SIE.LES.MaterialReturnApplys;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialReturnApplys
{
    /// <summary>
    /// 退料申请前端数据请求
    /// </summary>
    public class MaterialReturnApplyDataQueryer : DataQueryer
    {
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
            return RT.Service.Resolve<MaterialReturnApplyController>().GetLinesideWarehouse(workShopId, wipResourceId);
        }

        /// <summary>
        /// 计算明细lpn库存
        /// </summary>
        /// <param name="details">明细</param>
        /// <param name="wareId">仓库Id</param>
        /// <param name="storageId">库位Id</param>
        /// <returns></returns>
        public List<MaterialReturnApplyDetailSelect> CaseLpnOnHand(List<MaterialReturnApplyDetailSelect> details, double? wareId, double? storageId)
        {
            return RT.Service.Resolve<MaterialReturnApplyController>().CaseLpnOnHand(details, wareId, storageId);
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
        public List<EntityJson> GetExportData(int rangeOption, EntityList<MaterialReturnApply> selectEntity, int pagesize, int currentpage, MaterialReturnApplyCriteria criteria)
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
                EntityList<MaterialReturnApply> datas = RT.Service.Resolve<MaterialReturnApplyController>().GetMaterialReturnApplies(criteria);
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
                EntityList<MaterialReturnApply> datas = RT.Service.Resolve<MaterialReturnApplyController>().GetMaterialReturnApplies(criteria);
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
        private string Export(EntityList<MaterialReturnApply> datas)
        {
            StringBuilder exportData = new StringBuilder();
            exportData.Append("<tr style='background-color:#B3B3B3'><td>退料单号</td><td>状态</td><td>工单号</td><td>车间</td>"+
                "<td>资源</td><td>退料仓库</td><td>库位</td><td>退料原因</td>" +
                "<td>行号</td><td>退料状态</td><td>物料编码</td><td>物料名称</td><td>单位</td><td>良品状态</td><td>管控方式</td>" +
                "<td>物料扩展属性</td><td>物料标签</td><td>退料数</td><td>在途数</td><td>收货数</td>" +
                "</tr>");
            var ids = datas.Select(p => p.Id).ToList();
            var details = RT.Service.Resolve<MaterialReturnApplyController>().GetReturnApplyDetailByPIds(ids);
            foreach (var data in datas)
            {
                var childs = details.Where(p => p.MaterialReturnApplyId == data.Id).ToList();
                string dataLine = string.Empty;
                var parentString = ("<tr>" +
                    "<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>" +
                    "<td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td>").FormatArgs(data.No, data.ReStatus.ToLabel(), data.WoNo, data.WorkShopCode,
                    data.WipResourceCode, data.WarehouseName, data.StorageLocationName, data.Reason);
                if (details.Any())
                {
                    foreach(var child in childs)
                    {
                        var childString = ("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td>" +
                            "<td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td>" +
                            "</tr>").FormatArgs(child.LineNo, child.ReDetailStatus, child.ItemCode, child.ItemName, child.UnitName, child.ReDetailQuality.ToLabel(), child.CtrlMode,
                            child.ItemExtPropName, child.Label, child.ReturnQty, child.OnWayQty, child.CollectQty);
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
