using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.MES.BatchWIP;
using SIE.MES.WIP;
using SIE.Wpf.MES.BatchWIP.Repairs;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.MES.BatchWIP.ViewBehaviors
{
    /// <summary>
    /// 选择项变更加载缺陷信息（批次维修采集使用）
    /// </summary>
    public class LoadDefectsBehavior : ViewBehavior
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        protected override void OnAttach()
        {
            var dics = new Dictionary<double, EntityList<BatchRepairDefectViewModel>>();

            var listView = View as ListLogicalView;
            listView.Control.AllowInitiallyFocusedRow = true;
            listView.Control.SelectedItemChanged += (o, e) =>
            {
                var mainView = listView.Relations.Find("mainView") as DetailLogicalView;
                var model = mainView?.Current as BatchRepairViewModel;
                if (model == null)
                    return;

                var newItem = e.NewItem as InputBatch;
                if (newItem == null || !newItem.InputType.HasValue)
                {
                    model.BatchRepairDefectList.Clear();
                    return;
                }

                //缓存未保存数据
                var oldItem = e.OldItem as InputBatch;
                if (oldItem != null && model.BatchRepairDefectList.IsDirty)
                {
                    if (!dics.ContainsKey(oldItem.Id))
                        dics[oldItem.Id] = new EntityList<BatchRepairDefectViewModel>();
                    dics[oldItem.Id].Clear();
                    dics[oldItem.Id].AddRange(model.BatchRepairDefectList);
                }
                else if (oldItem != null && !model.BatchRepairDefectList.IsDirty && dics.ContainsKey(oldItem.Id))
                {
                    dics.Remove(oldItem.Id);
                }

                if (dics.ContainsKey(newItem.Id))
                {
                    model.BatchRepairDefectList.Clear();
                    model.BatchRepairDefectList.AddRange(dics[newItem.Id]);
                }
                else
                {
                    var collectBarcode = new CollectBarcode { Type = BarcodeType.BatchBarocde, Code = newItem.SubBatchNo.IsNotEmpty() ? newItem.SubBatchNo : newItem.BatchNo };
                    model.LoadDefects(model.GetWorkcell(), collectBarcode);
                    //dics[obj.Id] = new EntityList<BatchRepairDefectViewModel>();
                    //dics[obj.Id].AddRange(model.DefectList);
                }
            };

            listView.DataChanged += (o, e) =>
            {
                var view = o as ListLogicalView;
                view.Control.SelectItem(0);
                dics.Clear();
            };
        }
    }
}
