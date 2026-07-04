using SIE.Common;
using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.WIP.Products
{
    /// <summary>
    /// 生产通用报表导出查询
    /// </summary>
    public class WipProductDataQueryer : DataQueryer
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
        public List<EntityJson> GetWipProductData(int rangeOption, EntityList<WipProductVersion> seleneity, int pagesize, int currentpage, WipProductVersionCriteria criteria)
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
                EntityList<WipProductVersion> data = RT.Service.Resolve<WipProductVersionController>().GetWipProductVersions(criteria);
                sb.Append(ExportAll(data));
            }

            if (rangeOption == (int)ExportOption.Selected && seleneity.Count > 0)
            { //导出选中行
                sb.Append(ExportAll(seleneity));
            }

            if (rangeOption == (int)ExportOption.All)
            { //导出所有数据
                criteria.PagingInfo = null;
                EntityList<WipProductVersion> data = RT.Service.Resolve<WipProductVersionController>().GetWipProductVersions(criteria);
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
        public string ExportAll(EntityList<WipProductVersion> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr style='background-color:#B3B3B3'><td>条码</td><td>是否hold</td><td>工单号</td><td>工单类型</td>"
                          + "<td>工单数量</td><td>工艺流程名称</td><td>车间</td><td>产品型号</td><td>当前工序</td><td>当前工位资源</td>"
                          + "<td>产品等级</td><td>是否已完工下线</td></tr>");
            var versionIds = data.Select(p => p.Id).ToList();
            var inspectionItemAllList = RT.Service.Resolve<WipProductVersionController>().GetWipProductInspectionItems(versionIds);
            var wipProductProcessAllList = RT.Service.Resolve<WipProductVersionController>().GetWipProductProcess(versionIds);
            var processIds = wipProductProcessAllList.Select(p => p.Id).ToList();
            var wipProductProcessKeyItemAllList = RT.Service.Resolve<WipProductVersionController>().GetWipProductProcessKeyItems(processIds);
            var wipProductTestResultAllList = RT.Service.Resolve<WipProductVersionController>().GetWipProductTestResults(processIds);
            var wipProductRepairAllList = RT.Service.Resolve<WipProductVersionController>().GetWipProductRepairs(versionIds);
            var wipProductDefectAllList = RT.Service.Resolve<WipProductVersionController>().GetWipProductDefects(versionIds);
            var productDefectIds = wipProductDefectAllList.Select(p => p.Id).ToList();
            var wipDefectResponsibilityAllList = RT.Service.Resolve<WipProductVersionController>().GetWipDefectResponsibilitys(productDefectIds);
            var wipDefectMeasureAllList = RT.Service.Resolve<WipProductVersionController>().GetWipDefectMeasures(productDefectIds);

            data.ForEach(p =>
            {
                sb.Append(string.Format(_tableFormat
                      + "<td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td>"
                      + "<td>{10}</td><td>{11}</td></tr>", p.Sn, p.IsHold ? "是" : "否", p.WorkOrderNo, p.WoType.ToLabel(), p.WoPlanQty, p.VersionName, p.WorkShopName, p.Model, p.NowProcessName, p.ResourceName, p.Grade, p.IsFinish ? "是" : "否"));

                #region 判断产品检验记录表是否有数据，有数据就导出
                var testInfoList = inspectionItemAllList.Where(x => x.VersionId == p.Id).AsEntityList<WipProductInspectionItem>();
                if (testInfoList.Any())
                {
                    sb.Append(ExportTestData(testInfoList));
                }
                #endregion

                #region 判断生产采集记录表是否有数据，有数据就导出
                var processList = wipProductProcessAllList.Where(x => x.VersionId == p.Id).AsEntityList<WipProductProcess>();
                if (processList.Any())
                {
                    var testProcessIds = processList.Select(x => x.Id).ToList();
                    var testWipProductProcessKeyItemList = wipProductProcessKeyItemAllList.Where(x => testProcessIds.Contains(x.ProcessId)).AsEntityList<WipProductProcessKeyItem>();
                    var testWipProductTestResultList = wipProductTestResultAllList.Where(x => testProcessIds.Contains(x.ProcessId)).AsEntityList<WipProductTestResult>();
                    if (processList.Any())
                        sb.Append(ExportProcessData(processList, testWipProductProcessKeyItemList, testWipProductTestResultList));
                }
                #endregion

                #region 判断产品维修记录表是否有数据，有数据就导出
                var repairList = wipProductRepairAllList.Where(x => x.VersionId == p.Id).AsEntityList<WipProductRepair>();
                if (repairList.Any())
                {
                    foreach (var repair in repairList)
                    {
                        sb.Append(ExportRepairList(repair.WipProductRepairDefectList));
                    }

                }
                #endregion

                #region 判断产品缺陷记录表是否有数据，有数据就导出
                var defectList = wipProductDefectAllList.Where(x => x.VersionId == p.Id).AsEntityList<WipProductDefect>();
                if (defectList.Any())
                {
                    var testDefectIds = defectList.Select(x => x.Id).ToList();
                    var testWipDefectResponsibilityList = wipDefectResponsibilityAllList.Where(x => testDefectIds.Contains(x.WipProductDefectId)).AsEntityList<WipDefectResponsibility>();
                    var testWipDefectMeasureList = wipDefectMeasureAllList.Where(x => testDefectIds.Contains(x.WipProductDefectId)).AsEntityList<WipDefectMeasure>();
                    sb.Append(ExportDefectData(defectList, testWipDefectResponsibilityList, testWipDefectMeasureList));
                }
                #endregion
            });
            return sb.ToString();
        }

        /// <summary>
        /// 导出产品检验记录表
        /// </summary>
        /// <param name="data">产品检验记录表</param>
        /// <returns>产品检验记录</returns>
        public string ExportTestData(EntityList<WipProductInspectionItem> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr style='background-color:#CE8CFB'><td>产品检验记录</td><td>项目编码</td><td>项目名称</td><td>规范上限</td>"
                          + "<td>规范下限</td><td>测试值</td><td>检验结果</td><td>备注</td><td>检验人</td></tr>");
            data.ForEach(p =>
            {
                sb.Append(string.Format(_tableFormat
                      + "<td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td></tr>", string.Empty, string.Empty, p.InspectionItemName, p.LimitMax == null ? string.Empty : p.LimitMax.ToString(), p.LimitLow == null ? string.Empty : p.LimitLow.ToString(),
                      p.InspectionValue == null ? string.Empty : p.InspectionValue.ToString(), p.Result.ToLabel(), p.Remarks, p.InspectByName));
            });
            return sb.ToString();
        }

        /// <summary>
        /// 导出生产采集记录表
        /// </summary>
        /// <param name="processList">生产采集记录表</param>
        /// <param name="keyItemAllList">关键件记录表</param>
        /// <param name="testResultAllList">产品测试结果记录表</param>
        /// <returns>生产采集记录</returns>
        public string ExportProcessData(EntityList<WipProductProcess> processList, EntityList<WipProductProcessKeyItem> keyItemAllList, EntityList<WipProductTestResult> testResultAllList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr style='background-color:#CE8CFB'><td>生产采集记录</td><td>状态</td><td>操作时间</td><td>采集结果</td>"
                          + "<td>工位</td><td>工序</td><td>产线</td><td>操作人</td><td>条码</td></tr>");
            processList.ForEach(p =>
            {
                sb.Append(string.Format(_tableFormat
                      + "<td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td></tr>", string.Empty, p.State.ToLabel(), p.OperateTime == null ? string.Empty : p.OperateTime.Value.ToString(), p.Result.ToLabel(),
                      p.StationName, p.ProcessName, p.ResourceName, p.EmployeeName, p.Barcode));
                #region 判断产品生产关键件表是否有数据，有数据就导出
                var keyItemList = keyItemAllList.Where(x => x.ProcessId == p.Id).AsEntityList<WipProductProcessKeyItem>();
                if (keyItemList.Any())
                {
                    sb.Append(ExportKeyItemData(keyItemList));
                }
                #endregion
                #region 判断产品生产关键件表是否有数据，有数据就导出
                var testResultList = testResultAllList.Where(x => x.ProcessId == p.Id).AsEntityList<WipProductTestResult>();
                if (testResultList.Any())
                {
                    sb.Append(ExportTestResult(testResultList));
                }
                #endregion
            });
            return sb.ToString();
        }

        /// <summary>
        /// 导出产品生产关键件表
        /// </summary>
        /// <param name="data">产品生产关键件表</param>
        /// <returns>产品生产关键件</returns>
        public string ExportKeyItemData(EntityList<WipProductProcessKeyItem> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr style='background-color:#FFFFAA'><td>产品生产关键件</td><td></td><td>工序</td><td>工位</td>"
                          + "<td>来源条码</td><td>来源类型</td><td>用料数</td><td>物料编码</td><td>物料名称</td><td>物料描述</td><td>单位</td><td>操作人</td><td>操作时间</td></tr>");
            data.ForEach(p =>
            {
                sb.Append(string.Format(_tableFormat
                      + "<td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td></tr>", string.Empty, string.Empty, p.ProcessName, p.StationName,
                      p.SourceType.ToLabel(), p.Qty.ToString(), p.ItemCode, p.ItemName, p.ItemDescription, p.ItemUnitName, p.CreateByName, p.CreateDate.ToString()));
            });
            return sb.ToString();
        }

        /// <summary>
        /// 导出产品测试结果表
        /// </summary>
        /// <param name="data">产品测试结果表</param>
        /// <returns>产品测试结果</returns>
        public string ExportTestResult(EntityList<WipProductTestResult> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr style='background-color:#FFFFAA'><td>产品测试结果</td><td></td><td>测试项目</td><td>测试结果</td>"
                          + "<td>测试时间</td></tr>");
            data.ForEach(p =>
            {
                sb.Append(string.Format(_tableFormat
                      + "<td>{4}</td></tr>", string.Empty, string.Empty, p.Item, p.Result?.ToString(),
                      p.CreateDate.ToString()));
            });
            return sb.ToString();
        }

        /// <summary>
        /// 导出产品维修记录表
        /// </summary>
        /// <param name="data">产品维修记录表</param>
        /// <returns>产品维修记录</returns>
        public string ExportRepairList(EntityList<WipProductRepairDefect> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr style='background-color:#CE8CFB'><td>产品维修记录</td><td></td><td>缺陷代码</td><td>维修开始时间</td><td>维修完成时间</td>"
                          + "<td>返修人</td><td>工位</td><td>工序</td><td>产线</td><td>班次</td></tr>");
            data.ForEach(p =>
            {
                sb.Append(string.Format(_tableFormat
                      + "<td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td></tr>",
                      string.Empty, string.Empty, p.DefectCode, p.RepairStart.ToString(),
                      p.RepaireTime.ToString(), p.RepaireByName, p.StationName, p.ProcessName,
                      p.ResourceName, p.ShiftName));
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
        public string ExportDefectData(EntityList<WipProductDefect> data, EntityList<WipDefectResponsibility> responsibilitieAllList, EntityList<WipDefectMeasure> measureListAllList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr style='background-color:#CE8CFB'><td>产品缺陷记录</td><td>缺陷编码</td><td>缺陷描述</td><td>备注</td>"
                          + "<td>缺陷位置</td><td>工序</td><td>维修人</td><td>维修时间</td></tr>");
            data.ForEach(p =>
            {
                sb.Append(string.Format(_tableFormat
                      + "<td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td></tr>", string.Empty, p.DefectCode, p.DefectDesc, p.Remark,
                      p.Location, p.ProcessName, p.EmployeeName, p.FixedDate?.ToString()));

                #region 判断缺陷责任表是否有数据，有数据就导出
                var responsibilities = responsibilitieAllList.Where(x => x.WipProductDefectId == p.Id).AsEntityList<WipDefectResponsibility>();
                if (responsibilities.Any())
                {
                    sb.Append(ExportResponsibilityData(responsibilities));
                }
                #endregion

                #region 判断维修措施表是否有数据，有数据就导出
                var measureList = measureListAllList.Where(x => x.WipProductDefectId == p.Id).AsEntityList<WipDefectMeasure>();

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
        public string ExportResponsibilityData(EntityList<WipDefectResponsibility> data)
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
        public string ExportDefectMeasureData(EntityList<WipDefectMeasure> data)
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
