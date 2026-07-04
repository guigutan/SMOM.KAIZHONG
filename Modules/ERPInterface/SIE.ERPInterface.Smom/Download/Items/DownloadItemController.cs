using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Ebs.Download.Items;
using SIE.Inventory.Commom;
using SIE.Items;
using SIE.Items.Items;
using SIE.Resources.Employees;
using SIE.Utils;
using SIE.WMS;
using SIE.WMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 物料下载控制器
    /// </summary>
    public class DownloadItemController : DomainController
    {
        /// <summary>
        /// 从API下载物料到业务表
        /// </summary>
        /// <param name="itemDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadItemToBusiness(List<ItemData> itemDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<ItemData>(
                itemDatas,
                p => this.SaveItems(p.OrderByLastUpdateDate()),
                JobType.Item,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载物料到业务表
        /// </summary>
        /// <param name="isManual"></param>
        /// <returns></returns>
        public virtual ProcessResult DownloadItemInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<ItemInf>(
                () => ctl.GetUnprocessedDatas<ItemInf>(),      //物料中间表数据
                p =>
                {
                    var paras = this.GenerateItemPara(p);
                    return this.SaveItems(paras.OrderByLastUpdateDate());
                },
                JobType.Item, isManual);
        }

        /// <summary>
        /// 手动下载
        /// </summary>
        /// <param name="keyWord">查询关键字</param>
        public virtual string DownloadManual(string keyWord)
        {
            ProcessResult result = new ProcessResult();
            string resultMsg = string.Empty;

            try
            {
                if (keyWord.IsNullOrEmpty())
                    throw new ValidationException("唯一主键不能为空".L10N());
                using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
                {
                    RT.Service.Resolve<SoapItemController>().DownloadToInf(true, keyWord);                     //执行中间表下载
                    result = DownloadItemInfToBusiness(true);           //执行业务表下载
                    trans.Complete();
                }
            }
            catch (Exception ex)
            {
                result.AddFailMsg(ex.GetBaseException());
            }

            if (!result.Result) resultMsg = result.FailMsg.FirstOrDefault();
            return resultMsg;
        }

        /// <summary>
        /// ERP保存单位数据
        /// </summary>
        /// <param name="datas">通信数据类</param>
        /// <returns>错误信息</returns>
        public virtual List<ErpErrorData> SaveUnits(List<UnitData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            var ctl = RT.Service.Resolve<ItemController>();

            var units = ctl.GetUnitList(datas.Select(p => p.Code).Distinct().ToList());//已存在的数据
            var unitsDic = units.ToDictionary(p => p.Code);

            //按顺序处理数据
            foreach (var p in datas)
            {
                try
                {
                    SaveUnit(p, unitsDic);
                }
                catch (Exception ex)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = p.Infkey });
                }
            }

            return errors;
        }

        /// <summary>
        /// ERP保存物料
        /// </summary>
        /// <param name="datas">erp数据</param>
        /// <returns>错误信息</returns>
        private List<ErpErrorData> SaveItems(List<ItemData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            var ctl = RT.Service.Resolve<ItemController>();

            var items = ctl.GetItems(datas.Select(p => p.Code).Distinct().ToList());//已存在的数据
            var itemsDic = items.ToDictionary(p => p.Code);

            var units = ctl.GetUnitList(datas.Select(p => p.UnitCode).Distinct().ToList());//已存在的单位数据
            var unitsDic = units.ToDictionary(p => p.Code);

            var persons = RT.Service.Resolve<EmployeeController>().GetEmployeeList(datas.Select(p => p.PurchasingAgent).Distinct().ToList());//采购员数据
            var personsDic = persons.ToDictionary(p => p.Code);

            var smallCategory = ctl.GetItemSmallCategory(null, null, string.Empty, null);
            var smallCategoryDic = smallCategory.ToDictionary(p => p.Code);

            //按顺序处理数据
            foreach (var p in datas)
            {
                try
                {                   
                    SaveItem(p, itemsDic, unitsDic, personsDic, smallCategoryDic);
                }
                catch (Exception ex)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = p.Infkey });
                }
            }
            //var program = RT.Service.Resolve<ItemBatchProgramController>().GetInitItemBatchPrograms();
            //if (program == null)
            //{
            //    var lotDefault = RT.Service.Resolve<LotController>().GetLotDefault();
            //    if (lotDefault == null)
            //        RT.Service.Resolve<LotController>().InsertDefaultLot();
            //    program = RT.Service.Resolve<ItemBatchProgramController>().InsertDefaultProgram(true);
            //}

            var shItemIds = datas.Where(f => f.IsBatch).Select(a => a.ItemId).ToList();
            shItemIds.SplitDataExecute(ids =>
              {
                  //DB.Update<ItemStockData>().Where(f => ids.Contains(f.ItemId)).Set(p => p.IsBatch, true).Set(p => p.ItemBatchProgramId, program.Id).Execute();
              });

            datas.Where(f => f.IsSerialNumber || f.ShelfLife > 0).GroupBy(f => new
            {
                f.IsSerialNumber,
                f.ShelfLife
            }).ForEach(f =>
                    {
                        var shItemIds = f.Select(a => a.ItemId).ToList();
                        shItemIds.SplitDataExecute(ids =>
                {
                    //DB.Update<ItemStockData>().Where(f => ids.Contains(f.ItemId)).Set(f => f.IsSerialNumber, f.Key.IsSerialNumber).Set(p => p.ShelfLife, f.Key.ShelfLife).Execute();
                });
                    });


            return errors;
        }

        /// <summary>
        /// 生成物料参数
        /// </summary>
        /// <param name="itemInfs">中间表数据</param>
        /// <returns></returns>
        private List<ItemData> GenerateItemPara(IEnumerable<ItemInf> itemInfs)
        {
            var paras = new List<ItemData>();

            itemInfs.ForEach(p =>
            {
                var data = new ItemData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.Code = p.Code;
                data.Name = p.Name;
                data.Description = p.Description;
                data.DrawingNo = p.DrawingNo;
                data.Version = p.Version;
                data.BaseModel = p.BaseModel;
                data.Person = p.Person;
                data.MrpPerson = p.MrpPerson;
                data.UpperWeight = p.UpperWeight;
                data.LowerWeight = p.LowerWeight;
                data.MinPackingQty = p.MinPackingQty;
                data.SpecificationModel = p.SpecificationModel;
                data.EnglishDescription = p.EnglishDescription;
                data.ShortDescription = p.ShortDescription;
                data.Length = p.Length;
                data.Width = p.Width;
                data.Height = p.Height;
                data.Volume = p.Volume;
                data.Weight = p.Weight;
                data.Precision = p.Precision;
                data.GoodsBarcode = p.GoodsBarcode;
                data.UnitCode = p.Unit;
                data.PurchasingAgent = p.PurchasingAgent;
                data.CategoryCode = p.ItemCategoryCode;
                data.State = 1;
                data.ErpKey = p.ErpKey;
                data.IsSerialNumber = p.IsSerialNumber ?? false;
                data.IsBatch = p.IsBatch ?? false;
                data.ShelfLife = p.ShelfLife;
                data.Type = p.ItemTypeEbs;
                data.ItemSourceType = p.ItemSourceType;
                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// 执行单位数据保存
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="dic">数据字典</param>
        private void SaveUnit(UnitData data, Dictionary<string, Unit> dic)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            Unit entity;
            var key = data.Code;
            if (key.IsNullOrEmpty())
                throw new ValidationException("单位编码为空".L10nFormat(key));
            //处理待删除数据
            if (dic.ContainsKey(key))
            {
                if (data.IsDelete)
                {
                    ctl.DeleteEntity(dic, key, dic[key]);
                }
                return;
            }
            if (!dic.ContainsKey(key))
                dic.Add(key, new Unit());
            entity = dic[key];
            entity.Code = data.Code;
            entity.Name = data.Name;
            entity.Type = data.Type;
            entity.Precision = data.Precision;

            RF.Save(entity);
        }

        /// <summary>
        /// 执行物料数据保存
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="dic">物料数据字典</param>
        /// <param name="dicUnit">单位数据字典</param>
        /// <param name="dicEmployee">员工数据字典</param>
        /// <param name="dicCate">分类数据字典</param>
        private void SaveItem(ItemData data, Dictionary<string, Item> dic, Dictionary<string, Unit> dicUnit, Dictionary<string, Employee> dicEmployee, Dictionary<string, ItemCategory> dicCate)
        {
            Item entity;
            var key = data.Code;
            if (key.IsNullOrEmpty())
                throw new ValidationException("物料编码为空".L10nFormat(key));
            //处理待删除数据
            if (dic.ContainsKey(key))
            {
                if (data.IsDelete)
                {
                    dic[key].State = State.Disable;
                    RF.Save(dic[key]);
                }
                return;
            }
            if (!dic.ContainsKey(key))
                dic.Add(key, new Item());
            entity = dic[key];

            //采购员
            if (data.PurchasingAgent.IsNotEmpty())
            {
                if (dicEmployee.ContainsKey(data.PurchasingAgent))
                    entity.PurchasingAgent = dicEmployee[data.PurchasingAgent];
            }
            //单位
            if (data.UnitCode.IsNotEmpty())
            {
                if (!dicUnit.ContainsKey(data.UnitCode))
                {
                    Unit u = new Unit() { Code = data.UnitCode, Name = data.UnitCode };
                    RF.Save(u);
                    dicUnit.Add(data.UnitCode, u);
                }
                entity.Unit = dicUnit[data.UnitCode];
            }

            SetItemValue(entity, data);

            #region 创建物料与分类关系 
            //创建物料与分类关系
            var itemCategorys = entity.GetLazyList(ItemExtCategoryListProperty.ItemCategoryListProperty);
            if (itemCategorys != null && entity.PersistenceStatus == PersistenceStatus.New)
            {
                foreach (EnumViewModel enumViewModel in EnumViewModel.GetByEnumType(typeof(CategoryType)))
                {
                    itemCategorys.Add(new ItemCategoryRelation() { Type = (CategoryType)enumViewModel.EnumValue });
                }
            }
            if (itemCategorys != null && data.CategoryCode.IsNotEmpty())
            {
                //（实际项目需要确认分类重复键问题）
                if (!dicCate.Keys.Contains(data.CategoryCode))
                    throw new ValidationException("物料分类[{0}]不存在或不是最底层的编码".L10nFormat(data.CategoryCode));

                itemCategorys.OfType<ItemCategoryRelation>()
                    .FirstOrDefault(x => x.Type == CategoryType.Item).ItemCategory = dicCate[data.CategoryCode];
                itemCategorys.OfType<ItemCategoryRelation>()
                    .FirstOrDefault(x => x.Type == CategoryType.Quality).ItemCategory = dicCate[data.CategoryCode];
                itemCategorys.OfType<ItemCategoryRelation>()
                    .FirstOrDefault(x => x.Type == CategoryType.Kit).ItemCategory = dicCate[data.CategoryCode];
            }
            #endregion

            RF.Save(entity);
            data.ItemId = entity.Id;
        }

        /// <summary>
        /// 设置物料的值
        /// </summary>
        /// <param name="item">物料</param>
        /// <param name="p">ERP值</param>
        private void SetItemValue(Item item, ItemData p)
        {
            item.Code = p.Code;
            item.Name = p.Name;
            item.SpecificationModel = p.SpecificationModel;
            item.Description = p.Description;
            item.DrawingNo = p.DrawingNo;
            item.Version = p.Version;
            item.BaseModel = p.BaseModel;
            item.Person = p.Person;
            item.MrpPerson = p.MrpPerson;
            item.UpperWeight = p.UpperWeight;
            item.LowerWeight = p.LowerWeight;
            item.MinPackingQty = p.MinPackingQty;
            item.EnglishDescription = p.EnglishDescription;
            item.ShortDescription = p.ShortDescription;
            item.Length = p.Length;
            item.Width = p.Width;
            item.Height = p.Height;
            item.Volume = p.Volume;
            item.Weight = p.Weight;
            item.Precision = p.Precision;
            item.GoodsBarcode = p.GoodsBarcode;
            item.SourceKey = p.ErpKey;
            item.State = State.Enable;
            item.SourceType = SIE.Common.SourceType.External;
            item.ItemSourceType = p.ItemSourceType == "外购" ? ItemSourceType.Outsourcing : ItemSourceType.SelfMade;
            if (p.Type == 1)
            {
                item.Type = ItemType.Material;
            }
            else if (p.Type == 2)
            {
                item.Type = ItemType.SemiFinished;
            }
            else if (p.Type == 0)
            {
                item.Type = ItemType.Product;
            }
            else
            {
                item.Type = ItemType.Other;
            }
        }
    }
}
