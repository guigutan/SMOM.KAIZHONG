using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.LoadItems;
using SIE.MES.SingleLabels;
using SIE.Security;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WIP.Repairs.Commands
{
    /// <summary>
    /// 换料命令
    /// </summary>
    [Command(ImageName = "PackageChange", Label = "换料", ToolTip = "换料", GroupType = CommandGroupType.Edit)]
    public class ChangeItemCommand : ListViewCommand
    {
        /// <summary>
        /// 判断换料命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，不能执行返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var mainView = view.Relations.Find("mainView");
            var record = view.Current as ProductAssemblyDetailViewModel;
            return view.Current != null && mainView?.Current != null && mainView.Current is RepairViewModel && record != null && record.KeyItem.Qty > 0;
        }

        /// <summary>
        /// 换料命令执行方法
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var repair = view.Relations.Find("mainView").Current.CastTo<RepairViewModel>();
            var workcell = repair.GetWorkcell();
            var assembly = view.Current as ProductAssemblyDetailViewModel;
            assembly.Barcode = null;
            assembly.WorkOrderId = repair.WorkOrderId;
            assembly.RepairViewModel = repair;

            if (assembly.KeyItem.SourceType != LoadItemSourceType.SN && !assembly.ChangeItemViewModelList.Any())
            {
                var loadItems = repair.LoadItemList.Where(p => ComparePropertyItem(p, assembly));

                if (loadItems.Any())
                {
                    var loadItem = loadItems.FirstOrDefault();

                    try
                    {
                        assembly.IsOverLoadItem(loadItem.SourceCode, loadItem.Qty);

                        var barcodeInfo = new LoadItemBarcodeInfo()
                        {
                            Barcode = loadItem.SourceCode,
                            ItemId = loadItem.ItemId,
                            Qty = Math.Round(loadItem.Qty, loadItem.UnitPrecision ?? 3, MidpointRounding.AwayFromZero),
                            Type = loadItem.SourceType
                        };

                        barcodeInfo.ItemExtPropName = loadItem.ItemExtPropName;
                        barcodeInfo.ItemExtProp = loadItem.ItemExtProp;

                        if (!assembly.ChangeItemViewModelList.Any(p => p.ChangeSn == loadItem.SourceCode))
                        {
                            ChangeItemViewModel changeItemViewModel = new ChangeItemViewModel()
                            {
                                ChangeSn = loadItem.SourceCode,
                                ChangeQty = Math.Round(loadItem.Qty >= assembly.KeyItem.Qty ? assembly.KeyItem.Qty : loadItem.Qty, loadItem.UnitPrecision ?? 3, MidpointRounding.AwayFromZero),
                                LoadItemBarcodeInfo = barcodeInfo,
                                IsLoadItem = true,
                                ProductAssemblyDetailViewModel = assembly
                            };

                            assembly.ChangeItemViewModelList.Add(changeItemViewModel);

                            assembly.TotalChangeQty = Math.Round(changeItemViewModel.ChangeQty, loadItem.UnitPrecision ?? 3, MidpointRounding.AwayFromZero);
                        }
                    }
                    catch (Exception ex)
                    {
                        CRT.MessageService.ShowInstantMessage(ex.Message, "错误提示".L10N(), 5);
                    }
                }
            }

            assembly.Workcell = workcell;
            var moduleKey = RT.Service.Resolve<IFindModule>().FindModuleKey(typeof(RepairViewModel));
            var template = new DetailsUITemplate(typeof(ProductAssemblyDetailViewModel), ViewConfig.DetailsView, moduleKey);
            var detailView = template.CreateUI();

            detailView.MainView.Data = assembly;
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), detailView.Control, w =>
            {
                w.Title = Meta.Label.L10N();
                w.Height = 500;
                w.Width = 600;
                w.DefaultButton = 1994;  ////避免回车关闭对话框
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        var broken = assembly.Validate(ValidatorActions.None);
                        if (broken.Count > 0)
                        {
                            CRT.MessageService.ShowInstantMessage(broken.ToString(), "错误提示".L10N(), 5);
                            e.Cancel = true;
                            return;
                        }

                        if (assembly.ChangeItemViewModelList.Any())
                        {
                            assembly.IsChangeSn = true;
                            ShowChangeBarcode(assembly);
                        }
                        else
                        {
                            assembly.IsChangeSn = false;
                            assembly.ChangeBarcode = string.Empty;
                        }
                    }
                };
            });
        }

        /// <summary>
        /// 显示换料条码
        /// </summary>
        /// <param name="assembly">装配信息</param>
        private void ShowChangeBarcode(ProductAssemblyDetailViewModel assembly)
        {
            string changeBarcode = string.Empty;
            assembly.ChangeItemViewModelList.ForEach(e => { changeBarcode += e.ChangeSn + ";"; });
            assembly.ChangeBarcode = changeBarcode;
        }

        /// <summary>
        /// 匹配上料列表属性值
        /// </summary>
        /// <param name="loadItem">上料</param>
        /// <param name="assembly">装配信息</param>
        /// <returns>匹配成功返回true，不匹配返回false</returns>
        private bool ComparePropertyItem(LoadItem loadItem, ProductAssemblyDetailViewModel assembly)
        {
            if (loadItem.ItemId != assembly.KeyItem.ItemId)
            {
                return false;
            }
            if (loadItem.ItemExtProp != assembly.KeyItem.ItemExtProp)
            {
                return false;
            }
            return true;
        }
    }
}
