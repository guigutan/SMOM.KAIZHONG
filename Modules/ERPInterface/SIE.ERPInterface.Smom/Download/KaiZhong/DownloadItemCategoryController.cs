using Newtonsoft.Json;
using SIE.Domain;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemCategoryData = SIE.KZ.Base.Interfaces.Datas.ItemCategoryData;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    /// <summary>
    /// 分类下载器
    /// </summary>
    public class DownloadItemCategoryController : DomainController
    {
        /// <summary>
        /// 保存分类到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveItemCategorysFactory(List<ItemCategoryData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<ItemCategoryData> list = new List<ItemCategoryData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.ItemCategory, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //判断是否存在分类为中的，如果不存在那么就创建一个，主要用于第一次传输数据
                    //这边的要求结构是(对分类的使用不严格)
                    /*  大
                          中
                            XX1
                            XX2
                    */
                    ItemCategoryLevel categoryLevel_Min = Query<ItemCategoryLevel>().Where(p => p.Code == "小").FirstOrDefault();
                    var middle = Query<ItemCategory>().Where(p => p.Code == "中").FirstOrDefault();
                    if (middle == null)
                    {
                        //判断是否同样存在大-中-小层级的分类层级，如果没有就创建一个
                        ItemCategoryLevel categoryLevel_Middle = Query<ItemCategoryLevel>().Where(p => p.Code == "中").FirstOrDefault();
                        ItemCategoryLevel categoryLevel_Max = Query<ItemCategoryLevel>().Where(p => p.Code == "大").FirstOrDefault();
                        if (categoryLevel_Min == null)
                        {
                            if (categoryLevel_Middle == null)
                            {
                                if (categoryLevel_Max == null)
                                {
                                    categoryLevel_Max = new ItemCategoryLevel();
                                    categoryLevel_Max.Code = "大";
                                    categoryLevel_Max.Name = "大";
                                    categoryLevel_Max.Type = Items.Items.CategoryType.Item;
                                    categoryLevel_Max.PersistenceStatus = PersistenceStatus.New;
                                    RF.Save(categoryLevel_Max);
                                }

                                categoryLevel_Middle = new ItemCategoryLevel();
                                categoryLevel_Middle.Code = "中";
                                categoryLevel_Middle.Name = "中";
                                categoryLevel_Middle.Type = Items.Items.CategoryType.Item;
                                categoryLevel_Middle.TreePId = categoryLevel_Max.Id;
                                RF.Save(categoryLevel_Middle);
                            }
                            categoryLevel_Min = new ItemCategoryLevel();
                            categoryLevel_Min.Code = "小";
                            categoryLevel_Min.Name = "小";
                            categoryLevel_Min.Type = Items.Items.CategoryType.Item;
                            categoryLevel_Min.TreePId = categoryLevel_Middle.Id;
                            RF.Save(categoryLevel_Min);
                        }

                        var max = Query<ItemCategory>().Where(p => p.Code == "大").FirstOrDefault();
                        if (max == null)
                        {
                            max = new ItemCategory();
                            max.Code = "大";
                            max.Name = "大";
                            max.LevelId = categoryLevel_Max.Id;
                            max.Level = categoryLevel_Max;
                            max.Type = categoryLevel_Max.Type;
                            max.ItemType = ItemType.Material;
                            max.PersistenceStatus = PersistenceStatus.New;
                            RF.Save(max);
                        }
                        middle = new ItemCategory();
                        middle = new ItemCategory();
                        middle.Code = "中";
                        middle.Name = "中";
                        middle.LevelId = categoryLevel_Middle.Id;
                        middle.Level = categoryLevel_Middle;
                        middle.Type = categoryLevel_Middle.Type;
                        middle.ItemType = ItemType.Material;
                        middle.TreePId = max.Id;
                        middle.PersistenceStatus = PersistenceStatus.New;
                        RF.Save(middle);
                    }

                    //获取所有分类
                    var categorys = RF.GetAll<ItemCategory>();
                    foreach (var item in datas)
                    {
                        //已经存在的直接跳过去
                        if (categorys.Any(p => p.Code == item.MATKL))
                        {
                            list.Add(item);
                            continue;
                        }
                        ItemCategory itemCategory = new ItemCategory();
                        itemCategory.Code = item.MATKL;
                        itemCategory.Name = item.WGBEZ;
                        itemCategory.Level = categoryLevel_Min;
                        itemCategory.LevelId = categoryLevel_Min.Id;
                        itemCategory.Type = categoryLevel_Min.Type;
                        itemCategory.ItemType = ItemType.Material;
                        itemCategory.TreePId = middle.Id;
                        RF.Save(itemCategory);
                        list.Add(item);

                        if (categorys.All(p => p.Id != itemCategory.Id))
                            categorys.Add(itemCategory);

                        apiResult.SuccessList.Add(item);

                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<ItemCategoryData>(erpDataInfLog, list, apiResult);

                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能为空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorObjList.Clear();
                apiResult.ErrorObjList.AddRange(datas);
                apiResult.ErrorList.Add(ex.Message);
                logController.UpadateLogData<ItemCategoryData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;

        }


        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveItemCategorys(List<ItemCategoryData> datas, ref InfNcDataLogGroup erpDataInfLog)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };

            List<ItemCategoryData> list = new List<ItemCategoryData>();
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            var dataJson = JsonConvert.SerializeObject(datas);
            int failCount = 0;

            try
            {
                if (datas != null || datas.Count > 0)
                {


                    //判断是否存在分类为中的，如果不存在那么就创建一个，主要用于第一次传输数据
                    //这边的要求结构是(对分类的使用不严格)
                    /*  大
                          中
                            XX1
                            XX2
                    */
                    ItemCategoryLevel categoryLevel_Min = Query<ItemCategoryLevel>().Where(p => p.Code == "小").FirstOrDefault();
                    //判断是否同样存在大-中-小层级的分类层级，如果没有就创建一个
                    ItemCategoryLevel categoryLevel_Middle = Query<ItemCategoryLevel>().Where(p => p.Code == "中").FirstOrDefault();
                    ItemCategoryLevel categoryLevel_Max = Query<ItemCategoryLevel>().Where(p => p.Code == "大").FirstOrDefault();
                    if (categoryLevel_Min == null)
                    {
                        if (categoryLevel_Middle == null)
                        {
                            if (categoryLevel_Max == null)
                            {
                                categoryLevel_Max = new ItemCategoryLevel();
                                categoryLevel_Max.Code = "大";
                                categoryLevel_Max.Name = "大";
                                categoryLevel_Max.Type = Items.Items.CategoryType.Item;
                                categoryLevel_Max.PersistenceStatus = PersistenceStatus.New;
                                RF.Save(categoryLevel_Max);
                            }

                            categoryLevel_Middle = new ItemCategoryLevel();
                            categoryLevel_Middle.Code = "中";
                            categoryLevel_Middle.Name = "中";
                            categoryLevel_Middle.Type = Items.Items.CategoryType.Item;
                            categoryLevel_Middle.TreePId = categoryLevel_Max.Id;
                            RF.Save(categoryLevel_Middle);
                        }
                        categoryLevel_Min = new ItemCategoryLevel();
                        categoryLevel_Min.Code = "小";
                        categoryLevel_Min.Name = "小";
                        categoryLevel_Min.Type = Items.Items.CategoryType.Item;
                        categoryLevel_Min.TreePId = categoryLevel_Middle.Id;
                        RF.Save(categoryLevel_Min);
                    }
                    var middle = Query<ItemCategory>().Where(p => p.Code == "中").FirstOrDefault();
                    if (middle == null)
                    {
                        var max = Query<ItemCategory>().Where(p => p.Code == "大").FirstOrDefault();
                        if (max == null)
                        {
                            max = new ItemCategory();
                            max.Code = "大";
                            max.Name = "大";
                            max.LevelId = categoryLevel_Max.Id;
                            max.Level = categoryLevel_Max;
                            max.Type = categoryLevel_Max.Type;
                            max.ItemType = ItemType.Material;
                            max.PersistenceStatus = PersistenceStatus.New;
                            RF.Save(max);
                        }
                        middle = new ItemCategory();
                        middle = new ItemCategory();
                        middle.Code = "中";
                        middle.Name = "中";
                        middle.LevelId = categoryLevel_Middle.Id;
                        middle.Level = categoryLevel_Middle;
                        middle.Type = categoryLevel_Middle.Type;
                        middle.ItemType = ItemType.Material;
                        middle.TreePId = max.Id;
                        middle.PersistenceStatus = PersistenceStatus.New;
                        RF.Save(middle);
                    }

                    //获取所有分类
                    var categorys = RF.GetAll<ItemCategory>();
                    foreach (var item in datas)
                    {
                        //已经存在的直接跳过去
                        if (categorys.Any(p => p.Code == item.MATKL))
                        {
                            list.Add(item);
                            continue;
                        }
                        ItemCategory itemCategory = new ItemCategory();
                        itemCategory.Code = item.MATKL;
                        itemCategory.Name = item.WGBEZ;
                        itemCategory.Level = categoryLevel_Min;
                        itemCategory.LevelId = categoryLevel_Min.Id;
                        itemCategory.Type = categoryLevel_Min.Type;
                        itemCategory.ItemType = ItemType.Material;
                        itemCategory.TreePId = middle.Id;

                        RF.Save(itemCategory);

                        if (categorys.All(p => p.Id != itemCategory.Id))
                            categorys.Add(itemCategory);

                        list.Add(item);
                        apiResult.SuccessList.Add(item);
                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能为空!".L10N());
                }

            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorList.Add(ex.Message);
            }
            finally
            {
                erpDataInfLog = RT.Service.Resolve<InfNcDataLogGroupController>().UpdateInfNcDataLogGroupData<ItemCategoryData>(erpDataInfLog, datas.Count, list, apiResult, false);
            }
            return apiResult;
        }



    }
}
