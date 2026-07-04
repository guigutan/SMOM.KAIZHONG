using DevExpress.Xpf.Core;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.LoadItems;
using SIE.MES.SingleLabels;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.Wpf.Common.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SIE.Wpf.MES.LoadItems
{
    /// <summary>
    /// 上料工具类
    /// </summary>
    public static class LoadItemHelper
    {
        /// <summary>
        /// 上料
        /// </summary>
        /// <param name="barcode">要上料的物料条码</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="dicLoadItemSourceType">可上料类型字典</param>
        /// <param name="workOrderId">要上料的工单</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public static LoadItemBarcodeInfo GetLoadBarcodeInfo(string barcode, Workcell workcell,
            Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType, double? workOrderId)
        {
            if (!workOrderId.HasValue)
            {
                throw new ValidationException("当前工位的在制工单为空，不允许上料，请先扫描条码或直接点击【切换在制工单】再上料！".L10N());
            }

            LoadItemBarcodeInfo barcodeInfo = null;
            var barcodeInfos = RT.Service.Resolve<LoadItemController>()
                .ValidateLoadItem(barcode, workcell, dicLoadItemSourceType, workOrderId.Value);
            
            if (barcodeInfos.Count == 1)
            {
                barcodeInfo = barcodeInfos.FirstOrDefault();
                var item = RF.GetById<Item>(barcodeInfo.ItemId, new EagerLoadOptions().LoadWithViewProperty());
                //非序列号管理 且 非推式物料
                if (barcodeInfo.IsSerialNumber != true && barcodeInfo.ConsumeMode != Items.ConsumeMode.Push)
                {
                    var qty = ShowQtyInput(barcodeInfo.Qty);
                    if (qty <= 0)
                    {
                        throw new ValidationException("数量必须大于0".L10N());
                    }
                    barcodeInfo.Qty =Math.Round( qty, item.UnitPrecision??3, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                var listView = AutoUI.ViewFactory.CreateListView(typeof(LoadItemBarcodeInfo));
                listView.Data = barcodeInfos;
                CRT.Workbench.ShowDialog(listView, w =>
                {
                    w.Width = 800;
                    w.Height = 600;
                    w.Title = "选择物料标签".L10N();
                    listView.Control.MouseDoubleClick += (o, e) =>
                    {
                        if (listView.Current != null)
                        {
                            w.Close(0);
                        }
                    };

                    w.Closing += (o, e) =>
                    {
                        if (w.Result == 0 && listView.Current != null && listView.SelectedEntities.Count > 0)
                        {
                            //选择后赋值
                            barcodeInfo = listView.SelectedEntities[0] as LoadItemBarcodeInfo;
                            var item = RF.GetById<Item>(barcodeInfo.ItemId, new EagerLoadOptions().LoadWithViewProperty());
                            //非序列号管理 且 非推式物料
                            if (barcodeInfo.IsSerialNumber != true && barcodeInfo.ConsumeMode != Items.ConsumeMode.Push)
                            {
                                var qty = ShowQtyInput(barcodeInfo.Qty);
                                if (qty <= 0)
                                {
                                    e.Cancel = true;
                                    throw new ValidationException("数量必须大于0".L10N());
                                }
                                barcodeInfo.Qty = Math.Round(qty, item.UnitPrecision ?? 3, MidpointRounding.AwayFromZero);
                            }
                        }
                    };
                });

                if (barcodeInfo == null)
                {
                    throw new ValidationException("扫描的条码【{0}】有多笔记录，您已取消选择！".L10nFormat(barcode));
                }
            }

            return barcodeInfo;
        }

        /// <summary>
        /// 弹出输入框
        /// </summary>
        /// <param name="defaultQty">默认数量</param>
        /// <returns></returns>
        private static decimal ShowQtyInput(decimal defaultQty)
        {
            decimal qty = 0m;
            var editor = new Calculator();
            var button = new SimpleButton();
            button.HorizontalContentAlignment = HorizontalAlignment.Right;
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), editor, sk =>
            {
                editor.Value = (double)defaultQty;
                sk.Title = "上料数量录入".L10N();
                sk.Height = 400;
                sk.Width = 400;
                sk.Closing += (a, b) =>
                {
                    if (sk.Result == 0)
                    {
                        if ((decimal)editor.Value <= 0)
                        {
                            b.Cancel = true;
                            throw new ValidationException("数量必须大于0".L10N());
                        }
                        if (editor.Value > (double)defaultQty)
                        {
                            b.Cancel = true;
                            throw new ValidationException("输入数量不能大于{0}".L10nFormat(defaultQty));
                        }
                        if (editor.HasError || editor.Value < 0)
                        {
                            b.Cancel = true;
                            return;
                        }
                        qty = (decimal)editor.Value;
                    }
                };
            });

            return qty;
        }

    }
}
