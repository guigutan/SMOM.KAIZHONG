using SIE.Common;
using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.ObjectModel;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次生产通用报表导出查询
    /// </summary>
    public class BatchWipProductDataQueryer : DataQueryer
    {
        /// <summary>
        /// trtd
        /// </summary>
        private readonly string _tableFormat = "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>";

        /// <summary>
        /// 获取导出的生产通用报表的数据
        /// </summary>
        /// <param name="rangeOption">查询选项</param>
        /// <param name="seleneity">选择行</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="currentpage">当前页</param>
        /// <param name="criteria">查询条件</param>
        /// <returns>生产通用报表的数据</returns>
        public List<EntityJson> GetWipProductData(int rangeOption, EntityList<BatchWipProductVersion> seleneity, int pagesize, int currentpage, CriteriaQuery criteria)
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

                EntityList<BatchWipProductVersion> data = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductVersion(criteria);
                sb.Append(ExportAll(data));
            }

            if (rangeOption == (int)ExportOption.Selected && seleneity.Count > 0)
            { //导出选中行
                sb.Append(ExportAll(seleneity));
            }

            if (rangeOption == (int)ExportOption.All)
            { //导出所有数据
                criteria.PagingInfo = null;
                EntityList<BatchWipProductVersion> data = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductVersion(criteria);
                sb.Append(ExportAll(data));
            }

            sb.Append("</table>");
            EntityJson resNode = new EntityJson();
            resNode.SetProperty("exportData", sb.ToString());
            res.Add(resNode);
            return res;
        }

        /// <summary>
        /// 导出生产通用报表数据
        /// </summary>
        /// <param name="data">生产通用报表</param>
        /// <returns>生产通用报表数据</returns>
        public string ExportAll(EntityList<BatchWipProductVersion> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr style='background-color:#B3B3B3'><td>批次号</td><td>批次数量</td><td>批次报废数</td><td>工单编号</td><td>是否Hold</td><td>工单类型</td>"
                          + "<td>产品型号</td><td>工单数量</td><td>工单完工数</td><td>工单报废数</td><td>车间</td><td>当前资源</td>"
                          + "<td>当前工序</td><td>是否已完工下线</td></tr>");
            var versionIds = data.Select(p => p.Id).ToList();
            var wipProductProcessAllList = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductProcess(versionIds);

            var wipProductDefectAllList = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductDefects(versionIds);
            var productDefectIds = wipProductDefectAllList.Select(p => p.Id).ToList();
            var wipProductRepairAllList = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductRepairs(versionIds);

            var wipDefectResponsibilityAllList = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipDefectResponsibilitys(productDefectIds);
            var wipDefectMeasureAllList = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipDefectMeasures(productDefectIds);

            data.ForEach(p =>
            {
                sb.Append(string.Format(_tableFormat
                      + "<td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td>"
                      + "<td>{10}</td><td>{11}</td><td>{12}</td><td>{13}</td></tr>",
                      p.BatchNo, p.Qty, p.ScrapQty, p.WorkOrderNo, p.IsHold ? "是" : "否", 
                      p.WoType.ToLabel(),p.ProductModelName, p.WoPlanQty, p.WoFinishQty, p.ScrapQty,
                      p.WorkShopName,p.ResourceName,p.ProcessName,p.IsFinish ? "是" : "否"));

                #region 判断生产采集记录表是否有数据，有数据就导出
                var processList = wipProductProcessAllList.Where(x => x.VersionId == p.Id).AsEntityList<BatchWipProductProcess>();
                if (processList.Any())
                {
                    sb.Append(ExportProcessData(processList));
                }
                #endregion

                #region 判断产品维修记录表是否有数据，有数据就导出
                var repairList = wipProductRepairAllList.Where(x => x.VersionId == p.Id).AsEntityList<BatchWipProductRepaire>();
                if (repairList.Any())
                {
                    sb.Append(ExportRepairList(repairList));

                }
                #endregion

                #region 判断产品缺陷记录表是否有数据，有数据就导出
                var defectList = wipProductDefectAllList.Where(x => x.VersionId == p.Id).AsEntityList<BatchWipProductDefect>();
                if (defectList.Any())
                {
                    var testDefectIds = defectList.Select(x => x.Id).ToList();
                    var testWipDefectResponsibilityList = wipDefectResponsibilityAllList.Where(x => testDefectIds.Contains(x.DefectId)).AsEntityList<BatchWipDefectResponsibility>();
                    var testWipDefectMeasureList = wipDefectMeasureAllList.Where(x => testDefectIds.Contains(x.DefectId)).AsEntityList<BatchWipDefectMeasure>();
                    sb.Append(ExportDefectData(defectList, testWipDefectResponsibilityList, testWipDefectMeasureList));
                }
                #endregion
            });
            return sb.ToString();
        }


        /// <summary>
        /// 导出生产采集记录表
        /// </summary>
        /// <param name="processList">生产采集记录表</param>
        /// <returns>生产采集记录</returns>
        public string ExportProcessData(EntityList<BatchWipProductProcess> processList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr style='background-color:#CE8CFB'><td>批次采集记录</td><td></td><td>资源</td><td>工序</td><td>入站数量</td><td>出站数量</td>"
                          + "<td>入站时间</td><td>出站时间</td></tr>");
            processList.ForEach(p =>
            {
                sb.Append(string.Format(_tableFormat
                      + "<td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td></tr>", string.Empty, string.Empty, p.ResourceName, p.ProcessName,
                      p.InputQty, p.OutputQty, p.InputDate.ToString("yyyy/MM/dd HH:mm:ss"), p.OutputDate.HasValue? p.OutputDate.Value.ToString("yyyy/MM/dd HH:mm:ss"):""));
            });
            return sb.ToString();
        }

        /// <summary>
        /// 导出产品维修记录表
        /// </summary>
        /// <param name="data">产品维修记录表</param>
        /// <returns>产品维修记录</returns>
        public string ExportRepairList(EntityList<BatchWipProductRepaire> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr style='background-color:#CE8CFB'><td>产品维修记录</td><td></td><td>批次号</td><td>子批次号</td><td>缺陷代码</td><td>返修数量</td><td>修复数量</td>"
                          + "<td>废弃数量</td><td>报废原因</td><td>返修时间</td><td>返修人</td><td>工位</td><td>工序</td><td>资源</td><td>班次</td></tr>");
            data.ForEach(p =>
            {
                sb.Append(string.Format(_tableFormat
                      + "<td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td><td>{12}</td><td>{13}</td><td>{14}</td></tr>",
                       string.Empty, string.Empty, p.BatchNo, p.SubBatchNo, string.Empty, p.Qty,p.RepairQty,p.ScrapQty,p.ScrapReason,p.RepaireTime.ToString(),
                      p.ReparieByName,p.Station, p.ProcessName, p.ResourceName, p.ShiftName));
            });
            return sb.ToString();
        }

        /// <summary>
        /// 导出产品缺陷记录
        /// </summary>
        /// <param name="data">产品缺陷记录表</param>
        /// <param name="responsibilitieAllList">产品缺陷责任记录表</param>
        /// <param name="measureListAllList">产品缺陷维修措施记录表</param>
        /// <returns>产品缺陷记录</returns>
        public string ExportDefectData(EntityList<BatchWipProductDefect> data, EntityList<BatchWipDefectResponsibility> responsibilitieAllList, EntityList<BatchWipDefectMeasure> measureListAllList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr style='background-color:#CE8CFB'><td>产品缺陷记录</td><td></td><td>批次号</td><td>子批次号</td><td>载具号</td><td>批次数量</td><td>工位</td>"
                          + "<td>工序</td><td>资源</td><td>缺陷位置</td><td>维修人</td><td>维修时间</td><td>备注</td></tr>");
            data.ForEach(p =>
            {
                sb.Append(string.Format(_tableFormat
                      + "<td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td><td>{12}</td></tr>", string.Empty, string.Empty, p.BatchNo,
                      p.SubBatchNo, p.ContainerNo,p.Qty,p.StationName,p.ProcessName,p.ResourceName,p.Location,p.FixedByName, p.FixedDate?.ToString(),
                      p.Remark));

                #region 判断缺陷责任表是否有数据，有数据就导出
                var responsibilities = responsibilitieAllList.Where(x => x.DefectId == p.Id).AsEntityList<BatchWipDefectResponsibility>();
                if (responsibilities.Any())
                {
                    sb.Append(ExportResponsibilityData(responsibilities));
                }
                #endregion

                #region 判断维修措施表是否有数据，有数据就导出
                var measureList = measureListAllList.Where(x => x.DefectId == p.Id).AsEntityList<BatchWipDefectMeasure>();

                if (measureList.Any())
                {
                    sb.Append(ExportDefectMeasureData(measureList));
                }
                #endregion
            });
            return sb.ToString();
        }

        /// <summary>
        /// 导出缺陷责任
        /// </summary>
        /// <param name="data">缺陷责任表</param>
        /// <returns>缺陷责任</returns>
        public string ExportResponsibilityData(EntityList<BatchWipDefectResponsibility> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr style='background-color:#FFFFAA'><td>缺陷责任</td><td></td><td>编码</td><td>描述</td></tr>");
            data.ForEach(p =>
            {
                sb.Append(string.Format(_tableFormat, string.Empty, string.Empty, p.ResponseCode, p.ResponseDesc));
            });
            return sb.ToString();
        }

        /// <summary>
        /// 导出维修措施
        /// </summary>
        /// <param name="data">维修措施表</param>
        /// <returns>维修措施</returns>
        public string ExportDefectMeasureData(EntityList<BatchWipDefectMeasure> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr style='background-color:#FFFFAA'><td>维修措施</td><td></td><td>编码</td><td>名称</td><td>描述</td></tr>");
            data.ForEach(p =>
            {
                sb.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td>", string.Empty, string.Empty, p.MeasureCode, p.MeasureName, p.MeasureDesc));
            });
            return sb.ToString();
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
