using SIE.Domain;
using SIE.MES.PrepareProducts;
using SIE.MES.PrepareProducts.Services;
using SIE.Web.Data;
using System;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.PrepareProducts.DataQuerys
{
    /// <summary>
    /// 产前准备记录数据请求
    /// </summary>
    public class PrepareRecordDataQuery : DataQueryer
    {
        /// <summary>
        /// 创建产品准备记录
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public EntityList<PrepareRecordDetail> CreatePrepareRecordDetail(double workOrderId)
        {
            return RT.Service.Resolve<PrepareProductsController>().CreatePrepareRecordDetail(workOrderId);
        }

        /// <summary>
        /// 产前准备记录导出命令
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public string ExportPrepareProduct(PrepareRecord record)
        {
            var ctl = RT.Service.Resolve<PrepareRecordService>();
            if (record == null)
            {
                return string.Empty;
            }
            var recordDetailList = ctl.GetPrepareRecordDetailList(record.Id);
            if (!recordDetailList.Any())
            {
                return string.Empty;
            }
            StringBuilder exportData = new StringBuilder();
            const string headTitle = "<table><tr><td>工单号</td><td>产品编码</td><td>产品名称</td><td>工序</td>" +
                "<td>项目编码</td><td>项目名称</td><td>项目类型</td><td>项目描述</td><td>计数器</td>" +
                "<td>结果</td><td>备注</td><td>确认人</td><td>确认时间</td></tr>";
            string parentPart = "<tr><td>{0}</td><td>{1}</td><td>{2}</td>".FormatArgs(record.No, record.ProductCode, record.ProductName);
            exportData.Append(headTitle);
            recordDetailList.ForEach(child =>
            {
                string childPart = "<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td></tr>"
                .FormatArgs(child.ProcessName,  child.ProjectCode, child.ProjectName, child.ProjectType.ToLabel(), child.ProjectDesc,child.Counter
                , child.Result.ToLabel(), child.Remark, child.ConfirmerName, child.ConfirmTime);
                exportData.Append(parentPart + childPart);
            });
            exportData.Append("</table>");
            return exportData.ToString();
        }
    }
}
