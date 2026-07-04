using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Linq;
using SIE.Items;

namespace SIE.ERPInterface.Ebs.Download.Items
{
    /// <summary>
    /// 物料下载控制器
    /// </summary>
    public class EbsItemController : DomainController
    {
        /// <summary>
        /// 下载到中间表
        /// </summary>
        /// <param name="isManual">是否手工下载</param>       
        /// <returns>处理结果</returns>
        public virtual ProcessResult DownloadToInf(int? invOrgId, bool isManual = false)
        {
            if (invOrgId.HasValue)
                AppRuntime.InvOrg = invOrgId.Value;
            var ebsPara = EbsHelper.GetEbsParameter();
            //Copy必改内容
            ebsPara.InterfaceCode = "S_E2W_SKU";//接口编码，接口卡有
            const JobType jobType = JobType.Item;
            //End

            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            var jobTime = ctl.GetDownloadJobTime(jobType);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            if (jobTime?.LastDownloadDate.HasValue == true)
                ebsPara.DownParameter.LastUpdateDate = jobTime.LastDownloadDate.Value;

            DateTime beginTime = DateTime.Now;
            //ERP数据获取
            var soapResult = EbsHelper.ExecuteEbs<ItemDataEbs>(ebsPara);
            var itemCodes = soapResult.XV_RESULT.Select(a => a.Item_Code).ToList();
            var categoryIds = soapResult.XV_RESULT.Select(p => p.Category_Id).ToList();

            var itemCtl = RT.Service.Resolve<ItemController>();
            var allItems = itemCtl.GetItemsIdByCode(itemCodes);
            var categorys = itemCtl.GetItemCategoryByErpCategoryId(categoryIds);

            //移除不存在并且是失效的数据
            soapResult.XV_RESULT.RemoveAll(x => !allItems.Select(a => a.Code).Contains(x.Item_Code) && x.Enable_Flag != "Y" || x.Organization_Id != RT.InvOrg);
            soapResult.XV_RESULT.ForEach(p =>
            {
                ItemCategory itemCategory = categorys.FirstOrDefault(q => q.Id == p.Category_Id);
                if (itemCategory != null)
                {
                    p.ItemCategoryCode = itemCategory.Code;
                }
            });
            var result = RT.Service.Resolve<DownloadInfBaseController>().DownloadBusData(soapResult, p =>
        {    
            //Copy必改内容
            var itemInf = new ItemInf()
            {
                Code = p.Item_Code,
                Name = p.Item_Name,
                SpecificationModel = p.Specification_Model,
                Description = p.Description,
                EnglishDescription = p.English_Description,
                Height = p.Unit_Height,
                Weight = p.Unit_Weight,
                Width = p.Unit_Width,
                MinPackingQty = p.Minpacking_Qty.IsNotEmpty() ? decimal.Parse(p.Minpacking_Qty) : null,
                IsManual = isManual,
                ItemSourceType = p.Item_Source_Type == "1" ? "外购" : "自制",
                ItemTypeEbs = p.Item_Type ?? 3,

                ShortDescription = p.Short_Description,
                Unit = p.Unit_Code,
                Volume = p.Unit_Volume,
                Length = p.Unit_Length,
                IsDelete = p.Is_Delete.IsNotEmpty() || p.Enable_Flag != "Y",
                PurchasingAgent = p.Buyer,
                MrpPerson = p.Planner,
                IsBatch = p.Lot_Control_Code == 2,
                ItemCategoryCode = p.ItemCategoryCode,
                ErpKey = p.Item_Code,
            };
            return itemInf;
        }, jobType, jobTime, jobTimeDetail, beginTime, isManual);

            return result;
        }
    }
}
