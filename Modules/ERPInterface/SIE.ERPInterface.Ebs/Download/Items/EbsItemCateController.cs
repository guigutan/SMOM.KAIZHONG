using Microsoft.Scripting.Utils;
using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.EbsData;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using SIE.Items;
using SIE.Items.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Ebs.Download.Items
{
    /// <summary>
    /// 物料分类下载
    /// </summary>
    public class EbsItemCateController : DomainController
    {
        /// <summary>
        /// 下载物料分类数据
        /// </summary>
        /// <param name="invOrgId">库存组织</param>
        /// <param name="isManual">是否手工下载</param>        
        /// <returns>处理结果</returns>
        public virtual ProcessResult Download(int? invOrgId, bool isManual = false)
        {
            if (invOrgId.HasValue)
                AppRuntime.InvOrg = invOrgId;
            var ebsPara = EbsHelper.GetEbsParameter(false);
            //Copy必改内容
            ebsPara.InterfaceCode = "S_E2W_SKU_CATEGORY";//接口编码，接口卡有
            const JobType jobType = JobType.ItemCategory;
            //End

            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            var jobTime = ctl.GetDownloadJobTime(jobType);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            if (jobTime?.LastDownloadDate.HasValue == true)
                ebsPara.DownParameter.LastUpdateDate = jobTime.LastDownloadDate.Value;

            DateTime beginTime = DateTime.Now;
            //ERP数据获取
            var soapResult = EbsHelper.ExecuteEbs<ItemCate>(ebsPara);

            var codes = soapResult.XV_RESULT.Select(a => a.Concatenated_Segments).ToList();
            var allDatas = codes.SplitContains(pCodes =>
            {
                return Query<ItemCategory>().Where(p => pCodes.Contains(p.Code)).ToList();
            });
            //移除不存在并且是失效的数据
            soapResult.XV_RESULT.RemoveAll(x => !allDatas.Select(a => a.Code).Contains(x.Concatenated_Segments) && x.Enable_Flag != "Y");


            var itemctl = RT.Service.Resolve<ItemController>();

            //获取物料分类层级信息
            var itemCategoryLevel = itemctl.GetItemCategoryLevel(CategoryType.Item);
            if (itemCategoryLevel == null)
            {
                AddItemCategoryLevel(itemCategoryLevel);
            }

            var largeCategoryLevel = itemctl.GetItemCategoryLevel(CategoryType.Item, "大类");
            if (largeCategoryLevel == null)
            {
                AddItemCategoryLevel(largeCategoryLevel, "大类", itemCategoryLevel.Id);
            }
            var middleCategoryLevel = itemctl.GetItemCategoryLevel(CategoryType.Item, "中类");
            if (middleCategoryLevel == null)
            {
                AddItemCategoryLevel(middleCategoryLevel, "中类", itemCategoryLevel.Id);
            }
            var smallCategoryLevel = itemctl.GetItemCategoryLevel(CategoryType.Item, "小类");
            if (smallCategoryLevel == null)
            {
                AddItemCategoryLevel(middleCategoryLevel, "小类", itemCategoryLevel.Id);
            }

            //获取物料分类信息
            var itemCategory = itemctl.GetItemCategoryByCodes(new List<string> { "ERP分类" }).FirstOrDefault();

            if (itemCategory == null)
            {
                itemCategory = new ItemCategory()
                {
                    Code = "ERP分类",
                    Name = "ERP分类",
                    LevelId = itemCategoryLevel.Id,
                    Type = CategoryType.Item,
                };
                RF.Save(itemCategory);
            }
            soapResult.XV_RESULT.ForEach(p =>
            {
                p.ItemCategory = allDatas.FirstOrDefault(a => a.Code == p.Concatenated_Segments);
                p.TreePId = itemCategory.Id;

                //erp物料分类下载的编码格式为CS01.1.1,因此需要做以下格式转换
                string newstr = p.Concatenated_Segments.Replace(".0", "");
                bool isLargeCategory = newstr.Contains(".");
                bool isZero = p.Concatenated_Segments.EndsWith("0");

                if (isLargeCategory)
                {
                    if (isZero)
                    {
                        p.LevelId = middleCategoryLevel.Id;
                    }
                    else
                    {
                        p.LevelId = smallCategoryLevel.Id;
                    }

                }
                else
                {
                    p.LevelId = largeCategoryLevel.Id;
                }
            });
            var result = RT.Service.Resolve<DownloadInfBaseController>().DownloadBusData(soapResult, p =>
             {    //Copy必改内容
                 if (p.ItemCategory == null)
                 {
                     ItemCategory itemCategoryTmp = new ItemCategory()
                     {
                         Code = p.Concatenated_Segments,
                         Name = p.Concatenated_Segments,
                         Type = CategoryType.Item,
                         TreePId = p.TreePId.Value,
                         LevelId = p.LevelId.Value,
                         ErpCategoryId = p.Category_Id
                     };
                     return itemCategoryTmp;
                 }
                 else
                 {

                     p.ItemCategory.Name = p.Concatenated_Segments;
                     p.ItemCategory.Type = CategoryType.Item;

                     return p.ItemCategory;
                 }
             }, jobType, jobTime, jobTimeDetail, beginTime, isManual);

            return result;
        }

        /// <summary>
        /// 添加物料分类层级信息
        /// </summary>
        /// <param name="itemCategoryLevel">物料分类层级</param>
        private void AddItemCategoryLevel(ItemCategoryLevel itemCategoryLevel)
        {
            EntityList<ItemCategoryLevel> itemCategoryLevels = new EntityList<ItemCategoryLevel>();

            //添加主节点
            itemCategoryLevel = new ItemCategoryLevel()
            {
                Code = "库存类别",
                Name = "库存类别",
                Type = CategoryType.Item
            };
            RF.Save(itemCategoryLevel);

            var largeCategoryLevel = new ItemCategoryLevel()
            {
                Code = "大类",
                Name = "大类",
                Type = CategoryType.Item,
                TreePId = itemCategoryLevel.Id,
            };
            itemCategoryLevels.Add(largeCategoryLevel);

            var middleCategoryLevel = new ItemCategoryLevel()
            {
                Code = "中类",
                Name = "中类",
                Type = CategoryType.Item,
                TreePId = itemCategoryLevel.Id,
            };
            itemCategoryLevels.Add(middleCategoryLevel);

            var smallCategoryLevel = new ItemCategoryLevel()
            {
                Code = "小类",
                Name = "小类",
                Type = CategoryType.Item,
                TreePId = itemCategoryLevel.Id,
            };
            itemCategoryLevels.Add(smallCategoryLevel);
            RF.Save(itemCategoryLevels);
        }

        /// <summary>
        /// 添加物料分类层级信息
        /// </summary>
        /// <param name="itemCategoryLevel">物料分类层级</param>
        /// <param name="type">类型</param>
        /// <param name="parentId">父节点ID</param>
        private void AddItemCategoryLevel(ItemCategoryLevel itemCategoryLevel, string type, double parentId)
        {
            itemCategoryLevel.Type = CategoryType.Item;
            itemCategoryLevel.TreePId = parentId;
            if (type == "大类")
            {
                itemCategoryLevel.Code = "大类";
                itemCategoryLevel.Name = "大类";
            }
            if (type == "中类")
            {
                itemCategoryLevel.Code = "中类";
                itemCategoryLevel.Name = "中类";
            }
            if (type == "小类")
            {
                itemCategoryLevel.Code = "小类";
                itemCategoryLevel.Name = "小类";
            }
            RF.Save(itemCategoryLevel);
        }
    }
}
