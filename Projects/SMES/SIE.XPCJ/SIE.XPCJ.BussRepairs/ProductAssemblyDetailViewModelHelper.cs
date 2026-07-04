using SIE.XPCJ.Common.Exceptions;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.WIP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.BussRepairs
{
    /// <summary>
    /// 换料帮助类
    /// </summary>
    public static class ProductAssemblyDetailViewModelHelper
    {

        /// <summary>
        /// 条码变更事件
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="productAssemblyDetailViewModel"></param>
        /// <param name="productAssemblyDetailViewModels"></param>
        public static ChangeItemViewModel OnBarcodeChanged(string barcode, ProductAssemblyDetailViewModel productAssemblyDetailViewModel,
               List<ProductAssemblyDetailViewModel> productAssemblyDetailViewModels,
               decimal replaceQty = 1
            )
        {
            if (string.IsNullOrEmpty(barcode))
            {
                return null;
            }
            try
            {
                productAssemblyDetailViewModel.Barcode = barcode;
                ValidationBarcode(productAssemblyDetailViewModel);

                LoadItemBarcodeInfo barcodeInfo = GetChangedBarcode(barcode, productAssemblyDetailViewModel.WorkOrderId.Value, productAssemblyDetailViewModel.Workcell,
                    productAssemblyDetailViewModels

                    );

                ValidationItem(barcodeInfo, productAssemblyDetailViewModel.KeyItem);

                IsOverLoadItem(barcodeInfo.Barcode, replaceQty, productAssemblyDetailViewModels);

                productAssemblyDetailViewModel.ChangeQty = replaceQty;

                var item = new ChangeItemViewModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    ChangeSn = barcode,
                    ChangeQty = replaceQty,
                    LoadItemBarcodeInfo = barcodeInfo,
                };


                productAssemblyDetailViewModel.ChangeItemViewModelList.Add(item);

                productAssemblyDetailViewModel.Barcode = null;

                //更新总换料数量
                productAssemblyDetailViewModel.TotalChangeQty = productAssemblyDetailViewModel.TotalChangeQty + productAssemblyDetailViewModel.ChangeQty;
                return item;

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.L10N());
                return null;
            }
        }

        /// <summary>
        /// 获取换料条码
        /// </summary>
        /// <param name="sn">换料条码</param>
        /// <returns>换料条码信息</returns>
        private static LoadItemBarcodeInfo GetChangedBarcode(string sn, double workOrderId, Workcell workcell,
            List<ProductAssemblyDetailViewModel> productAssemblyDetailViewModels)
        {
            LoadItemBarcodeInfo barcodeInfo;
            //匹配上料列表
            var loadItems = WipService.GetLoadItemList(workcell.ResourceId, workcell.StationId);
            var loadItem = loadItems.FirstOrDefault(p => p.SourceCode == sn);
            if (loadItem != null)
            {
                IsOverLoadItem(sn, loadItem.Qty, productAssemblyDetailViewModels);
                barcodeInfo = new LoadItemBarcodeInfo()
                {
                    Barcode = loadItem.SourceCode,
                    ItemId = loadItem.ItemId,
                    Qty = loadItem.Qty,
                    Type = loadItem.SourceType
                };

                barcodeInfo.ItemExtPropName = loadItem.ItemExtPropName;
                barcodeInfo.ItemExtProp = loadItem.ItemExtProp;
            }
            else
            {
                Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType = new Dictionary<LoadItemSourceType, bool>();
                dicLoadItemSourceType.Add(LoadItemSourceType.SN, true);

                barcodeInfo = GetLoadBarcodeInfo(sn, workcell, dicLoadItemSourceType, workOrderId);
            }

            return barcodeInfo;
        }

        /// <summary>
        /// 获取上料条码信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <param name="dicLoadItemSourceType"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public static LoadItemBarcodeInfo GetLoadBarcodeInfo(string barcode, Workcell workcell,
   Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType, double? workOrderId)
        {
            if (!workOrderId.HasValue || workOrderId == 0)
            {
                throw new ValidationException("当前工位的在制工单为空，不允许上料，请先扫描条码或直接点击【切换在制工单】再上料！".L10N());
            }

            LoadItemBarcodeInfo barcodeInfo = null;
            var barcodeInfos = WipService.ValidateLoadItem(barcode, workcell, dicLoadItemSourceType, workOrderId.Value);

            if (barcodeInfos.Count == 1)
            {
                barcodeInfo = barcodeInfos.FirstOrDefault();
                var item = WipService.GetItemInfo(barcodeInfo.ItemId);
                //非序列号管理 且 非推式物料
                if (barcodeInfo.IsSerialNumber != true && barcodeInfo.ConsumeMode != ConsumeMode.Push)
                {
                    if (XPFormNumberInput.ShowInput(barcodeInfo.Qty, out decimal newQty) == DialogResult.OK)
                    {
                        if (newQty <= 0)
                        {
                            throw new ValidationException("数量必须大于0".L10N());
                        }
                        barcodeInfo.Qty = Math.Round(newQty, item.UnitPrecision ?? 3, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        throw new ValidationException("上料失败，扫描的条码【{0}】输入数量时您已取消操作！".L10nFormat(barcode));
                    }
                }
            }
            else
            {
                XPSelectLoadItemForm selectLoadItemForm = new XPSelectLoadItemForm();
                selectLoadItemForm.loadItemBarcodeInfos = barcodeInfos;

                var dialogResult = selectLoadItemForm.ShowDialog();

                if (dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    barcodeInfo = selectLoadItemForm.CurrentLoadItemBarcodeInfo;

                    var item = WipService.GetItemInfo(barcodeInfo.ItemId);

                    //非序列号管理 且 非推式物料
                    if (barcodeInfo.IsSerialNumber != true && barcodeInfo.ConsumeMode != ConsumeMode.Push)
                    {
                        if (XPFormNumberInput.ShowInput(barcodeInfo.Qty, out decimal newQty) == DialogResult.OK)
                        {
                            if (newQty <= 0)
                            {
                                throw new ValidationException("数量必须大于0".L10N());
                            }
                            barcodeInfo.Qty = Math.Round(newQty, item.UnitPrecision ?? 3, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            throw new ValidationException("上料失败，扫描的条码【{0}】输入数量时您已取消操作！".L10nFormat(barcode));
                        }
                    }
                }

                if (barcodeInfo == null)
                {
                    throw new ValidationException("扫描的条码【{0}】有多笔记录，您已取消选择！".L10nFormat(barcode));
                }
            }

            return barcodeInfo;
        }

        public static void OnChangeQtyChanged(string _changeSn,decimal changeQty,
            ProductAssemblyDetailViewModel productAssemblyDetailViewModel,
            List<ProductAssemblyDetailViewModel> productAssemblyDetailViewModels
            )
        {
            if (string.IsNullOrEmpty(_changeSn))
            {
                return;
            }

            var changedItem = productAssemblyDetailViewModel.ChangeItemViewModelList.FirstOrDefault(p => p.ChangeSn == _changeSn);

            if (changedItem == null)
            {
                return;
            }

            if (changeQty > changedItem.LoadItemBarcodeInfo.Qty)
            {
                throw new ValidationException("换料数量不能大于条码可用数量".L10N());
            }

            IsOverLoadItem(_changeSn, changedItem.LoadItemBarcodeInfo.Qty, productAssemblyDetailViewModels);

            decimal orginalQty = changedItem.ChangeQty;

          //赋值新数量
            changedItem.ChangeQty = changeQty;


            //更新总换料数量
            productAssemblyDetailViewModel.TotalChangeQty = productAssemblyDetailViewModel.TotalChangeQty - orginalQty + changedItem.ChangeQty;
        }

        /// <summary>
        /// 验证条码
        /// </summary>
        public static void ValidationBarcode(ProductAssemblyDetailViewModel productAssemblyDetailViewModel)
        {
            if (productAssemblyDetailViewModel.Barcode == productAssemblyDetailViewModel.KeyItem.SourceCode)
            {
                throw new ValidationException("换料条码[{0}]与当前装配条码一致，不允许换料".L10nFormat(productAssemblyDetailViewModel.Barcode));
            }

            if (productAssemblyDetailViewModel.ChangeItemViewModelList.Any(p => p.ChangeSn == productAssemblyDetailViewModel.Barcode))
            {
                throw new ValidationException("条码[{0}]已存在换料列表".L10nFormat(productAssemblyDetailViewModel.Barcode));
            }
        }

        /// <summary>
        /// 判断换料是数量是否超过上料数
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="loadQty">缺料数</param>
        public static void IsOverLoadItem(string sn, decimal loadQty, List<ProductAssemblyDetailViewModel> detailList)
        {
            var changedList = detailList.SelectMany(p => p.ChangeItemViewModelList);
            var qty = changedList.Where(p => p.ChangeSn == sn).Sum(p => p.ChangeQty);  //已添加换料数量
            if (loadQty - qty <= 0)
            {
                throw new ValidationException("条码{0}数量不足".L10nFormat(sn));
            }
        }

        /// <summary>
        /// 验证物料信息
        /// </summary>
        /// <param name="barcodeInfo">换料条码信息</param>
        public static void ValidationItem(LoadItemBarcodeInfo barcodeInfo, WipProductProcessKeyItem KeyItem)
        {
            if (barcodeInfo.ItemId != KeyItem.ItemId)
            {
                throw new ValidationException("物料不匹配".L10N());
            }
            if (barcodeInfo.ItemExtProp != KeyItem.ItemExtProp)
            {
                throw new ValidationException("物料{0}扩展属性不匹配".L10nFormat(KeyItem.ItemCode));
            }
        }

        /// <summary>
        /// 重新计算总换料数量
        /// </summary>
        public static void ComputeTotalChangeQty(ProductAssemblyDetailViewModel productAssemblyDetailViewModel)
        {
            decimal totalChangeQty = 0;

            foreach (var changeItem in productAssemblyDetailViewModel.ChangeItemViewModelList)
            {
                totalChangeQty += changeItem.ChangeQty;
            }

            productAssemblyDetailViewModel.TotalChangeQty = totalChangeQty;
        }
    }
}
