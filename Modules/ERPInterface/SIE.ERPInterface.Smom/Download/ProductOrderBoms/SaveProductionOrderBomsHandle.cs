using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.Items;
using SIE.Resources.ProcessTechs;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 生产订单BOM控制器
    /// </summary>
    public partial class SaveProductionOrderBomsHandle
    {
        #region 属性
        /// <summary>
        /// 错误信息
        /// </summary>
        private List<ErpErrorData> errorDatas;

        /// <summary>
        /// 物料控制器
        /// </summary>
        private readonly ItemController _itemController = RT.Service.Resolve<ItemController>();

        /// <summary>
        /// 生产订单控制器
        /// </summary>
        //private readonly ProductOrderController _productOrderController = RT.Service.Resolve<ProductOrderController>();

        /// <summary>
        /// 制程控制器
        /// </summary>
        private readonly ProcessTechBaseController _processTechController = RT.Service.Resolve<ProcessTechBaseController>();

        /// <summary>
        /// 生产订单BOM控制器
        /// </summary>
        //private readonly ProductionOrderBomController _productOrderBomController = RT.Service.Resolve<ProductionOrderBomController>();

        /// <summary>
        /// 生产订单字典 key：生产订单编号 value:生产订单
        /// </summary>
        //private Dictionary<string, ProductOrder> ProductOrderDic { get; set; }

        /// <summary>
        /// 物料字典 key:物料编号 value:物料
        /// </summary>
        private Dictionary<string, Item> ItemDic { get; set; }

        /// <summary>
        /// 制程工艺字典 key:制程工艺编号 value：制程工艺
        /// </summary>
        private Dictionary<string, ProcessTech> ProcessTechDic { get; set; }

        /// <summary>
        /// 生产订单BOM字典 key:生产订单ID value:生产订单BOM
        /// </summary>
        //private Dictionary<double, ProductionOrderBom> ProductionOrderBomDic { get; set; }

        /// <summary>
        /// 生产订单BOM明细字典 key:生产订单BOMid，key1：物料ID, value:生产订单bom明细
        /// </summary>
        //private Dictionary<double, Dictionary<double, ProductionOrderBomDetail>> ProductionOrderBomDetailDic { get; set; }
        #endregion

        #region 接口平台
        /// <summary>
        /// 用于接口中心下载数据保存到SMOM 生产订单BOM表中
        /// </summary>
        /// <param name="datas">生产订单BOM</param>
        /// <returns>错误数据列表</returns>
        public virtual List<ErpErrorData> SaveProductionOrderBoms(List<ProductionOrderBomData> datas)
        {
            errorDatas = new List<ErpErrorData>();
            List<ProductionOrderBomData> tmpPoBomData = new List<ProductionOrderBomData>();

            if (datas == null || datas.Count == 0) return errorDatas;

            tmpPoBomData.AddRange(datas);
            try
            {
                LoadData(tmpPoBomData);
                errorDatas.AddRange(ValidateData(tmpPoBomData));

                var tmpPoBomGroups = tmpPoBomData.GroupBy(p => p.Code).ToList();
                foreach (var tmpPoBomGroup in tmpPoBomGroups)
                {
                    List<ErpErrorData> tmpErrors = SaveProductionOrderBom(tmpPoBomGroup.ToList());
                    if (tmpErrors != null && tmpErrors.Count > 0) errorDatas.AddRange(tmpErrors);
                }
            }
            catch (Exception ex)
            {
                errorDatas.Clear();
                datas.ForEach(p => errorDatas.Add(new ErpErrorData() { ErrMsg = "代码数据级别错误:" + ex.Message, Infkey = p.Infkey, IsChild = false }));
            }

            return errorDatas;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 设置生产订单BOM明细信息（内存数据设置）
        /// </summary>
        /// <param name="poBom">生产订单BOM信息（接口）</param>
        /// <param name="poBomDetailDic">生产订单BOM明细信息（实体）</param>
        /// <returns></returns>
        //private ProductionOrderBomDetail SetProductionOrderBomDetail(ProductionOrderBomData poBom, Dictionary<double, ProductionOrderBomDetail> poBomDetailDic)
        //{
        //    Item item = ItemDic[poBom.ItemCode];
        //    ProductionOrderBomDetail tmpDetail = null;
        //    if (poBomDetailDic != null && poBomDetailDic.TryGetValue(item.Id, out tmpDetail))
        //    {
        //        // 如果已经存在明细
        //        if (poBom.IsDelete)
        //        {
        //            tmpDetail.PersistenceStatus = PersistenceStatus.Deleted;
        //        }
        //        else
        //        {
        //            tmpDetail.ReplateItemType = (ReplateItemType)poBom.ReplateItemType;
        //            tmpDetail.DemandQty = poBom.RequireQty;
        //            tmpDetail.Remark = poBom.Remark;
        //            tmpDetail.ItemId = item.Id;
        //            if (!string.IsNullOrEmpty(poBom.MainMaterialCode))
        //                tmpDetail.MainMaterialId = ItemDic[poBom.MainMaterialCode].Id;
        //            else
        //                tmpDetail.MainMaterialId = null;
        //            if (!string.IsNullOrEmpty(poBom.ProcessTech))
        //                tmpDetail.ProcessSegmentId = ProcessTechDic[poBom.ProcessTech].Id;
        //            else
        //                tmpDetail.ProcessSegmentId = null;
        //        }
        //    }
        //    else
        //    {
        //        // 如果不存在明细
        //        if (!poBom.IsDelete)
        //        {
        //            tmpDetail = new ProductionOrderBomDetail();
        //            tmpDetail.ReplateItemType = (ReplateItemType)poBom.ReplateItemType;
        //            tmpDetail.DemandQty = poBom.RequireQty;
        //            tmpDetail.Remark = poBom.Remark;
        //            tmpDetail.ItemId = item.Id;
        //            if (!string.IsNullOrEmpty(poBom.MainMaterialCode))
        //                tmpDetail.MainMaterialId = ItemDic[poBom.MainMaterialCode].Id;
        //            else
        //                tmpDetail.MainMaterialId = null;
        //            if (!string.IsNullOrEmpty(poBom.ProcessTech))
        //                tmpDetail.ProcessSegmentId = ProcessTechDic[poBom.ProcessTech].Id;
        //            else
        //                tmpDetail.ProcessSegmentId = null;
        //        }
        //    }

        //    return tmpDetail;
        //}
        /// <summary>
        /// 保存生产订单BOM信息
        /// </summary>
        /// <param name="poBomDatas">生产订单BOM信息</param>
        /// <returns>返回错误信息</returns>
        private List<ErpErrorData> SaveProductionOrderBom(List<ProductionOrderBomData> poBomDatas)
        {
            List<ErpErrorData> erpErrorDatas = new List<ErpErrorData>();
            if (poBomDatas == null || poBomDatas.Count == 0) return erpErrorDatas;

            string poCode = poBomDatas.First().Code;
            //ProductOrder po = ProductOrderDic[poCode];

            try
            {
                //bool isDelete = poBomDatas.All(p => p.IsDelete);
                //ProductionOrderBom poBom = null;
                //Dictionary<double, ProductionOrderBomDetail> poBomDetailDic = null;
                //if (ProductionOrderBomDic.TryGetValue(po.Id, out poBom))
                //{
                //    // 如果已经存在生产订单BOM数据
                //    if (!ProductionOrderBomDetailDic.TryGetValue(poBom.Id, out poBomDetailDic))
                //        poBomDetailDic = new Dictionary<double, ProductionOrderBomDetail>();
                //}
                //else
                //{
                //    if (!isDelete)
                //    {
                //        // 如果不存在生产订单BOM数据,并且不需要删除数据
                //        poBom = new ProductionOrderBom();
                //        poBom.ProductionOrderId = po.Id;
                //        poBom.GenerateId();
                //    }
                //    else
                //    {
                //        // 如果不存在生产订单BOM数据,并且需要删除数据
                //        return erpErrorDatas;
                //    }
                //}

                //EntityList<ProductionOrderBomDetail> detailList = new EntityList<ProductionOrderBomDetail>();
                //foreach (var poBomDetailData in poBomDatas)
                //{
                //    ProductionOrderBomDetail detail = SetProductionOrderBomDetail(poBomDetailData, poBomDetailDic);
                //    if (detail != null)
                //    {
                //        detail.ProductionOrderBomId = poBom.Id;
                //        detailList.Add(detail);
                //    }
                //}

                //if (poBomDetailDic != null && poBomDetailDic.Values.All(p => p.PersistenceStatus == PersistenceStatus.Deleted)
                //    && detailList.All(p => p.PersistenceStatus == PersistenceStatus.Deleted))
                //{
                //    poBom.PersistenceStatus = PersistenceStatus.Deleted;
                //}

                //SaveProductionOrderBom(poBom, detailList);
            }
            catch (Exception ex)
            {
                erpErrorDatas.Clear();
                erpErrorDatas.ForEach(p =>
                {
                    erpErrorDatas.Add(new ErpErrorData() { Infkey = p.Infkey, ErrMsg = "代码数据级别错误:" + ex.Message, IsChild = false });
                });
            }

            return erpErrorDatas;
        }

        /// <summary>
        /// 保存生产订单BOM信息（数据库）
        /// </summary>
        /// <param name="poBom"></param>
        /// <param name="poBomDetails"></param>
        //private void SaveProductionOrderBom(ProductionOrderBom poBom, EntityList<ProductionOrderBomDetail> poBomDetails)
        //{
        //    using (var trans = DB.TransactionScope(ApsCoreEntityDataProvider.ConnectionStringName))
        //    {
        //        RF.Save(poBom);
        //        RF.Save(poBomDetails);
        //        trans.Complete();
        //    }
        //}

        /// <summary>
        /// 删除生产订单BOM信息（数据库）
        /// </summary>
        /// <param name="poBom">生产订单BOM信息</param>
        /// <param name="poBomDetails">生产订单BOM明细信息</param>
        //private void DeleteProductionOrderBom(ProductionOrderBom poBom, List<ProductionOrderBomDetail> poBomDetails)
        //{
        //    poBom.PersistenceStatus = PersistenceStatus.Deleted;
        //    EntityList<ProductionOrderBomDetail> poBomDetailList = new EntityList<ProductionOrderBomDetail>();
        //    poBomDetails.ForEach(p =>
        //    {
        //        p.PersistenceStatus = PersistenceStatus.Deleted;
        //        poBomDetailList.Add(p);
        //    });

        //    using (var trans = DB.TransactionScope(ApsCoreEntityDataProvider.ConnectionStringName))
        //    {
        //        RF.Save(poBom);
        //        RF.Save(poBomDetailList);
        //        trans.Complete();
        //    }
        //}

        /// <summary>
        /// 加载所需数据
        /// </summary>
        /// <param name="datas">生产订单BOM数据</param>
        private void LoadData(List<ProductionOrderBomData> datas)
        {
            // 根据生产订单编号获取生产订单数据
            var poCodes = datas.Select(m => m.Code).Distinct().ToList();
            //ProductOrderDic = _productOrderController.GetProductOrderByCodes(poCodes).ToDictionary(p => p.Code);

            // 获取已经存在的生产订单BOM信息
            //ProductionOrderBomDic = _productOrderBomController.GetProductionOrderBoms(poCodes).ToDictionary(p => p.ProductionOrderId);

            // 获取已经存在的生产订单BOM明细信息
            //List<double> poBomIds = ProductionOrderBomDic.Select(p => p.Value.Id).ToList();
            //ProductionOrderBomDetailDic = _productOrderBomController.GetProductionOrderBomDetails(poBomIds).GroupBy(p => p.ProductionOrderBomId).ToDictionary(p => p.Key, p => p.ToDictionary(q => q.ItemId));

            // 根据物料编号获取物料数据
            List<string> itemCodes = datas.Select(p => p.ItemCode).ToList();
            itemCodes.AddRange(datas.Select(n => n.MainMaterialCode).ToList());
            itemCodes = itemCodes.Distinct().ToList();
            ItemDic = _itemController.GetItems(itemCodes).ToDictionary(p => p.Code);

            // 根据制程编号获取制程工艺数据
            var ptCodes = datas.Select(n => n.ProcessTech).Distinct().ToList();
            ProcessTechDic = _processTechController.GetProcessTechsFromCode(ptCodes).ToDictionary(p => p.Code);
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="datas">生产订单BOM数据</param>
        private List<ErpErrorData> ValidateData(List<ProductionOrderBomData> datas)
        {
            List<ErpErrorData> errors = new List<ErpErrorData>();
            var poBomGroupDatas = datas.GroupBy(p => p.Code).ToList();

            foreach (var poBomGroup in poBomGroupDatas)
            {
                string poCode = poBomGroup.Key;
                if (string.IsNullOrEmpty(poCode))
                {
                    poBomGroup.ForEach(p =>
                    {
                        errors.Add(new ErpErrorData() { Infkey = p.Infkey, ErrMsg = "编码不允许为空！", IsChild = false });
                        datas.Remove(p);
                    });
                }
                //else if (!ProductOrderDic.ContainsKey(poCode))
                //{
                //    poBomGroup.ForEach(p =>
                //    {
                //        errors.Add(new ErpErrorData() { Infkey = p.Infkey, ErrMsg = string.Format("匹配不到生产订单编号[{0}]！", poCode), IsChild = false });
                //        datas.Remove(p);
                //    });
                //}
                else
                {
                    bool isValid = true;
                    foreach (var pbDetal in poBomGroup)
                    {
                        if (string.IsNullOrEmpty(pbDetal.ItemCode))
                        {
                            errors.Add(new ErpErrorData() { Infkey = pbDetal.Infkey, ErrMsg = "物料编号不允许为空！", IsChild = false });
                            isValid = false;
                            break;
                        }
                        else if (!ItemDic.ContainsKey(pbDetal.ItemCode))
                        {
                            errors.Add(new ErpErrorData() { Infkey = pbDetal.Infkey, ErrMsg = string.Format("匹配不到物料编号[{0}]", pbDetal.ItemCode), IsChild = false });
                            isValid = false;
                            break;
                        }

                        if (!string.IsNullOrEmpty(pbDetal.MainMaterialCode) && !ItemDic.ContainsKey(pbDetal.MainMaterialCode))
                        {
                            errors.Add(new ErpErrorData() { Infkey = pbDetal.Infkey, ErrMsg = string.Format("匹配不到物料编号[{0}]", pbDetal.MainMaterialCode), IsChild = false });
                            isValid = false;
                            break;
                        }

                        if (!string.IsNullOrEmpty(pbDetal.ProcessTech) && !ProcessTechDic.ContainsKey(pbDetal.ProcessTech))
                        {
                            errors.Add(new ErpErrorData() { Infkey = pbDetal.Infkey, ErrMsg = string.Format("匹配不到制程工艺编号[{0}]", pbDetal.ProcessTech), IsChild = false });
                            isValid = false;
                            break;
                        }
                    }

                    if (!isValid)
                    {
                        poBomGroup.ForEach(p => datas.Remove(p));
                    }
                }
            }

            return errors;
        }
        #endregion
    }
}