using SIE.Common.Import;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.TaskManagement.Specifications;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;

namespace SIE.Web.MES.TaskManagement.Specifications.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Specifications.Commands.ProductSpecificationDetailCommand")]
    public class ProductSpecificationDetailCommand : ImportExcelCommand
    {
        /// <summary>
        /// 重写导入
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cache"></param>
        protected override void OnRowDataRead(RowData data, CacheData cache)
        {
            base.OnRowDataRead(data, cache);

            var productSpecificationDetail = data.Entity as ProductSpecificationDetail;

            if (productSpecificationDetail.ProductSpecificationCode.IsNullOrEmpty())
            {
                throw new ValidationException("第{0}行的物料编码不能为空".L10nFormat(data.RowIndex));
            }
            var item = RT.Service.Resolve<ItemController>().GetItemFromCode(productSpecificationDetail.ProductSpecificationCode);
            if (item == null)
            {
                throw new ValidationException("第{0}行的物料编码系统不存在".L10nFormat(data.RowIndex));
            }
            var productSpecification = RT.Service.Resolve<ProductSpecificationController>().GetProductSpecification(item.Id);
            if (productSpecification == null)
            {
                throw new ValidationException("第{0}行的物料编码找不到规格件清单".L10nFormat(data.RowIndex));
            }
            productSpecificationDetail.ProductSpecificationId = productSpecification.Id;
        }
    }
}