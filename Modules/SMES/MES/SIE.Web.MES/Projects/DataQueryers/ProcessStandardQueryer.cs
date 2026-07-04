using SIE.Domain;
using SIE.MES.Projects;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.Projects.DataQueryers
{
    /// <summary>
    /// 工序标准参数数据请求
    /// </summary>
    public class ProcessStandardQueryer : DataQueryer
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
        public List<EntityJson> GetExportData(int rangeOption, EntityList<ProcessStandardParam> selectEntity, int pagesize, int currentpage, ProcessStandardCriteria criteria)
        {
            List<EntityJson> res = new List<EntityJson>();
            StringBuilder sb = new StringBuilder();
            ////定义表头
            const string head = "<table>";
            sb.Append(head);
            if (rangeOption == 0) // 当前页
            {
                criteria.PagingInfo.PageSize = pagesize;
                criteria.PagingInfo.PageNumber = currentpage;
                EntityList<ProcessStandardParam> datas = RT.Service.Resolve<ProjectParamController>().QueryProcessStandardParams(criteria);
                sb.Append(Export(datas));
            }
            else if (rangeOption == 1) // 当前选择
            {
                if (selectEntity.Count > 0)
                {
                    sb.Append(Export(selectEntity));
                }

            }
            else // 导出全部
            {
                criteria.PagingInfo = null;
                EntityList<ProcessStandardParam> datas = RT.Service.Resolve<ProjectParamController>().QueryProcessStandardParams(criteria);
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
        private string Export(EntityList<ProcessStandardParam> datas)
        {
            StringBuilder exportData = new StringBuilder();
            exportData.Append("<tr style='background-color:#B3B3B3'><td>类型</td><td>产品编码</td><td>产品名称</td><td>工序编码</td><td>状态</td><td>描述</td>"
                + "<td>项目参数编码</td><td>项目参数名称</td><td>项目参数类型</td><td>参数值类型</td><td>单位</td><td>标准值</td><td>上限值</td><td>下限值</td></tr>");
            var dataIds = datas.Select(p => p.Id).ToList();
            var detailDic = RT.Service.Resolve<ProjectParamController>().GetProcessParamDtlByIds(dataIds);
            foreach (var data in datas)
            {
                string dataLine = string.Empty;
                string parentString = ("<tr>"
                    + "<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td>")
                    .FormatArgs(data.Type, data.ProductCode, data.ProductName, data.ProcessCode, data.ProcessName, data.ProcessStStatus.ToLabel(), data.Description);
                if (detailDic.TryGetValue(data.Id, out var details))
                {
                    foreach (var detail in details)
                    {
                        string childString = ("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td>")
                            .FormatArgs(detail.ProjectParamCode, detail.ProjectParamName, detail.ProjectParamType, detail.ProcessStDtlValueType.ToLabel(), detail.Unit, detail.SingleValue, detail.RangeMaxValue, detail.RangeMinValue);
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
