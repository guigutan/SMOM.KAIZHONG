using System;
using SIE.Common.Import;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Abnormal;
using SIE.Resources.WipResources;
using SIE.Web.Common.Import.Commands;

namespace SIE.Web.Equipments.Abnormal.Commands
{
    /// <summary>
    /// 导入异常停线
    /// </summary>
    public class AbnormalCauseImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 行数据读取, 默认按视图配置把数据读取后，还可以进行自定义处理
        /// </summary>
        /// <param name="data">行数据</param>
        /// <param name="cache">缓存数据</param>
        protected override void OnRowDataRead(RowData data, CacheData cache)
        {
            base.OnRowDataRead(data, cache);
            var abnormalCause = data.Entity as AbnormalCause;
            var resource = RF.GetById<WipResource>(abnormalCause.ResourceId);
            if (resource == null)
                throw new ValidationException("第{0}行的资源不存在".L10nFormat(data.RowIndex));
            if (resource.WorkShopId == null)
                throw new ValidationException("第{0}行的资源没有所属车间".L10nFormat(data.RowIndex));
            abnormalCause.ShopId = resource.WorkShopId.Value;
        }
    }
}
