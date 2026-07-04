using SIE.XPCJ.Common.Controls;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.Exceptions;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.WIP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.BussLoadItems
{
    public static class LoadItemHelper
    {
        /// <summary>
        /// 获取上料条码
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <param name="dicLoadItemSourceType"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public static LoadItemBarcodeInfo GetLoadBarcodeInfo(string barcode, Workcell workcell,
    Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType, double? workOrderId)
        {
            if (!workOrderId.HasValue|| workOrderId==0)
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
    }
}
