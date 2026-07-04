using DevExpress.Utils.StructuredStorage.Internal.Reader;
using SIE.Domain;
using SIE.MES.PrepareProducts;
using SIE.MES.PrepareProducts.Daos;
using SIE.MES.PrepareProducts.Services;
using SIE.Web.Data;
using System;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.PrepareProducts.DataQuerys
{
    /// <summary>
    /// 产品产前准备设置数据请求
    /// </summary>
    public class PrepareProductDataQuery : DataQueryer
    {
        public string ExportPrepareProduct(ExportPrepareProductModel model)
        {
            var ctl = RT.Service.Resolve<PrepareProductService>();
            var preProductList = ctl.QueryPreProductEntityList(model);
            if (preProductList.Count <= 0)
            {
                return string.Empty;
            }
            var preProductIds = preProductList.Select(p => p.Id).ToList();
            // 考虑到全表导出，子表数据量可能超过50000条，需要分页取
            var totalDetailCount = ctl.GetDetailTotalCount(preProductIds);
            EntityList<PrepareProductDetail> preProductDetailList = new EntityList<PrepareProductDetail>();
            for(int i = 0; i < Math.Ceiling((double)totalDetailCount / 50000); i++)
            {
                PagingInfo pagingInfo = new PagingInfo { PageNumber = i + 1,TotalCount = totalDetailCount,  PageSize = 50000};
                preProductDetailList.AddRange(ctl.GetPrepareProductDetailList(preProductIds, pagingInfo));
            }

            StringBuilder exportData = new StringBuilder();
            const string headTitle = "<table><tr><td>产品编码</td><td>产品名称</td><td>产品族编码</td><td>产品族名称</td><td>工序</td>" +
                "<td>项目编码</td><td>项目名称</td><td>项目类型</td><td>项目描述</td><td>创建人</td><td>创建时间</td><td>修改人</td><td>修改时间</td></tr>";
            exportData.Append(headTitle);
            preProductList.ForEach(parent =>
            {
                var children = preProductDetailList.Where(p => p.PrepareProductId == parent.Id).ToList();
                string dataLine = string.Empty;
                string parentPart = "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>".FormatArgs(parent.ProductCode, parent.ProductName, parent.ProductFamilyCode, parent.ProductFamilyName);
                if (children.Any())
                {
                    children.ForEach(child =>
                    {
                        string childPart = "<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td></tr>"
                        .FormatArgs(child.ProcessName, child.PrepareProjectCode, child.PrepareProjectName, child.PrepareProjectType.ToLabel(), child.PrepareProjectDesc
                        , child.CreateByName, child.CreateDate, child.UpdateByName, child.UpdateDate);
                        dataLine = parentPart + childPart;
                        exportData.Append(dataLine);
                    });
                }
                else
                {
                    dataLine = parentPart + "</tr>";
                    exportData.Append(dataLine);
                }
            });
            exportData.Append("</table>");
            return exportData.ToString();
        }
    }
}
