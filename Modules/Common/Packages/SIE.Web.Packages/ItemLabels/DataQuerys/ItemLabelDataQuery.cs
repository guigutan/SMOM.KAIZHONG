using SIE.Domain;
using SIE.Packages.ItemLabels;
using SIE.Web.Data;
using SIE.Web.Json;
using SIE.Web.Packages.ItemLabels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.Packages.ItemLabels.DataQuerys
{
    public class ItemLabelDataQuery : DataQueryer
    {
        /// <summary>
        /// 物料标签打印
        /// </summary>
        /// <param name="huId">物料标签ID</param>
        /// <returns>补打信息</returns>
        public ItemLabelsViewModels GetReprintInfo(double huId)
        {
            return null;
        }

        public List<EntityJson> GetItemLabelData(int rangeOption, EntityList<ItemLabel> seleneity, int pagesize, int currentpage, ItemLabelCriteria criteria)
        {
            List<EntityJson> res = new List<EntityJson>();
            StringBuilder sb = new StringBuilder();
            ////定义表头
            const string head = "<table>";
            sb.Append(head);
            if (rangeOption == (int)ExportOption.Current)
            { //导出当前页
                criteria.PagingInfo.PageSize = pagesize;
                criteria.PagingInfo.PageNumber = currentpage;
                EntityList<ItemLabel> data = RT.Service.Resolve<ItemLabelController>().GetItemLabels(criteria);
                sb.Append(ExportAll(data));
            }

            if (rangeOption == (int)ExportOption.Selected && seleneity.Count > 0)
            { //导出选中行
                sb.Append(ExportAll(seleneity));
            }

            if (rangeOption == (int)ExportOption.All)
            { //导出所有数据
                criteria.PagingInfo = null;
                EntityList<ItemLabel> data = RT.Service.Resolve<ItemLabelController>().GetItemLabels(criteria, false);
                sb.Append(ExportAll(data));
            }

            sb.Append("</table>");
            EntityJson resNode = new EntityJson();
            resNode.SetProperty("exportData", sb.ToString());
            res.Add(resNode);
            return res;
        }

        /// <summary>
        /// 导出所有
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string ExportAll(EntityList<ItemLabel> data)
        {
            StringBuilder exportData = new StringBuilder();
            exportData.Append("<tr style='background-color:#B3B3B3'><td>标签号</td><td>物料编码</td><td>物料名称</td><td>物料扩展属性</td>"
                          + "<td>规格型号</td><td>物料类型</td><td>条码信息来源</td><td>可用数量</td><td>来源工单</td><td>批次</td>"
                          + "<td>生产批次</td><td>供应商</td><td>工厂</td><td>仓库</td>"
                          + "<td>库位</td><td>生产日期</td><td>是否序列号管理</td><td>不良数量</td>"
                          + "<td>创建人</td><td>创建时间</td><td>修改人</td><td>修改时间</td>");
                          //+ "<td>物料投入工单-工单号</td><td>物料投入工单-数量</td></tr>");
            var itemLabelIds = data.Select(p => p.Id).ToList();
            var itemLabelsDetailAllList = RT.Service.Resolve<ItemLabelController>().GetItemLabelsDetail(itemLabelIds);

            data.ForEach(parent =>
            {
                //var children = itemLabelsDetailAllList.Where(p => p.ItemLabelId == parent.Id).ToList();
                string dataLine = string.Empty;
                string parentPart = ("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}<td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td>"
                + "<td>{8}</td><td>{9}</td><td>{10}</td><td>{11}<td>{12}</td><td>{13}</td><td>{14}</td><td>{15}</td>"
                + "<td>{16}</td><td>{17}</td><td>{18}</td><td>{19}<td>{20}</td><td>{21}</td>")
                .L10nFormat(parent.Label, parent.ItemCode, parent.ItemName, parent.ItemExtPropName,
                 parent.Specification, parent.ItemType.ToLabel(), parent.SourceType.ToLabel(), parent.Qty,
                 parent.WorkOrderNo,parent.Lot,parent.ProductBatch,parent.SupplierName,parent.FactoryName,
                 parent.WarehouseCode,parent.StorageLocationCode,parent.ProductionDate==null?"":parent.ProductionDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),parent.IsSerialNumber==true?"是":"否",
                 parent.NgQty,parent.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"), parent.CreateByName,parent.UpdateDate, parent.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"));
                //if (children.Any())
                //{
                //    children.ForEach(child =>
                //    {
                //        string childPart = "<td>{0}</td><td>{1}</td></tr>"
                //        .FormatArgs(child.WorkOrderNo, child.Qty);
                //        dataLine = parentPart + childPart;
                //        exportData.Append(dataLine);
                //    });
                //}
                //else
                {
                    dataLine = parentPart + "</tr>";
                    exportData.Append(dataLine);
                }
            });
            return exportData.ToString();
        }

    }
    /// <summary>
    /// 生产通用报表导出选项
    /// </summary>
    public enum ExportOption
    {
        /// <summary>
        /// 当前页
        /// </summary>
        Current = 0,

        /// <summary>
        /// 选中行
        /// </summary>
        Selected = 1,

        /// <summary>
        /// 查询结果
        /// </summary>
        All = 2,
    }
}
