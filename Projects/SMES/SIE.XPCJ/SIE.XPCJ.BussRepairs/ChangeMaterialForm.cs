using Newtonsoft.Json;
using SIE.XPCJ.Common.Exceptions;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using SIE.XPCJ.WIP.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.BussRepairs
{
    public partial class ChangeMaterialForm : XPFormBaseDialog
    {
        public ChangeMaterialForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 置换后下料选项
        /// </summary>
        private List<KeyValuePair<int, string>> DicChangeItemHandleMethod = new List<KeyValuePair<int, string>>()
        {
           new KeyValuePair<int,string>(10,"置换后作废".L10N() ),
            new KeyValuePair<int,string>(20,"置换后正常下料".L10N() ),
             new KeyValuePair<int,string>(30,"置换后不良下料".L10N() ),
        };

        /// <summary>
        /// 上料列表
        /// </summary>
        public List<LoadItem> LoadItemList { get; set; } = new List<LoadItem>();

        /// <summary>
        /// 装配记录
        /// </summary>
        public List<ProductAssemblyDetailViewModel> ProductAssemblyDetails { get; set; } = new List<ProductAssemblyDetailViewModel>();

        public ProductAssemblyDetailViewModel CurrentProductAssemblyDetailViewModel { get; set; }

        /// <summary>
        /// 设置初始化数据
        /// </summary>
        /// <param name="productAssemblyDetailViewModel"></param>
        public void SetData(ProductAssemblyDetailViewModel assembly)
        {
            var copyAssembly = JsonConvert.DeserializeObject<ProductAssemblyDetailViewModel>(JsonConvert.SerializeObject(assembly));
            this.CurrentProductAssemblyDetailViewModel = copyAssembly;
            GetLoadItemAssembly(this.CurrentProductAssemblyDetailViewModel);

            foreach (var changeItemViewModel in this.CurrentProductAssemblyDetailViewModel.ChangeItemViewModelList)
            {
                ChangeMaterialCard changeMaterialCard = new ChangeMaterialCard();
                changeMaterialCard.SetData(changeItemViewModel);
                changeMaterialCard.SetProductAssemblyDetails(this.ProductAssemblyDetails);
                changeMaterialCard.CurrentProductAssemblyDetailViewModel = this.CurrentProductAssemblyDetailViewModel;
                changeMaterialCard.ReflashForm = () => { RefalshTotalQty(); };
                ContainPanel.Controls.Add(changeMaterialCard);
            }
            this.xpTextBoxBarcode.AText = this.CurrentProductAssemblyDetailViewModel.SourceCode;
            this.xpTextBoxQty.AText = this.CurrentProductAssemblyDetailViewModel.KeyItem.Qty.ToString();

            this.xpTextBoxItemCode.AText = this.CurrentProductAssemblyDetailViewModel.KeyItemItemCode;
            this.xpTextBoxItemName.AText = this.CurrentProductAssemblyDetailViewModel.KeyItemItemName;
            this.xpTextBoxTotalQty.AText = this.CurrentProductAssemblyDetailViewModel.TotalChangeQty.ToString();

            this.xpComboBoxHandel.DisplayMember = "Value";
            this.xpComboBoxHandel.ValueMember = "Key";
            this.xpComboBoxHandel.DataSource = DicChangeItemHandleMethod;


            var changeItemHandleMethodValue = (int)this.CurrentProductAssemblyDetailViewModel.HandleMethod;

            if (changeItemHandleMethodValue > 0)
            {
                this.xpComboBoxHandel.SelectedIndex = this.DicChangeItemHandleMethod.FindIndex(m => m.Key == changeItemHandleMethodValue);
            }
        }

        /// <summary>
        /// 获取已上料的列表作为装配
        /// </summary>
        /// <param name="assembly"></param>
        private void GetLoadItemAssembly(ProductAssemblyDetailViewModel assembly)
        {
            if (assembly.KeyItem.SourceType != LoadItemSourceType.SN && !assembly.ChangeItemViewModelList.Any())
            {
                var loadItems = LoadItemList.Where(p => ComparePropertyItem(p, assembly));

                if (loadItems.Any())
                {
                    var loadItem = loadItems.FirstOrDefault();

                    try
                    {
                        this.IsOverLoadItem(loadItem.SourceCode, loadItem.Qty);

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
                            };

                            assembly.ChangeItemViewModelList.Add(changeItemViewModel);

                            assembly.TotalChangeQty = Math.Round(changeItemViewModel.ChangeQty, loadItem.UnitPrecision ?? 3, MidpointRounding.AwayFromZero);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.L10N());
                    }
                }
            }
        }

        /// <summary>
        ///  校验条码
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="loadQty"></param>
        public void IsOverLoadItem(string sn, decimal loadQty)
        {
            var changedList = this.ProductAssemblyDetails.SelectMany(p => p.ChangeItemViewModelList);
            var qty = changedList.Where(p => p.ChangeSn == sn).Sum(p => p.ChangeQty);  //已添加换料数量
            if (loadQty - qty <= 0)
            {
                MessageBox.Show("条码{0}数量不足".L10nFormat(sn));
            }
        }

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

        /// <summary>
        /// 刷新总数
        /// </summary>
        public void RefalshTotalQty()
        {
            ProductAssemblyDetailViewModelHelper.ComputeTotalChangeQty(CurrentProductAssemblyDetailViewModel);
            this.xpTextBoxTotalQty.AText = CurrentProductAssemblyDetailViewModel.TotalChangeQty.ToString();

        }

        private void xpTextBox1_Click(object sender, EventArgs e)
        {
            var nowQty = xpButton1.Text.Length > 0 ? decimal.Parse(xpButton1.Text.Trim()) : 0;

            if (XPFormNumberInput.ShowInput(nowQty, out decimal newQty) == DialogResult.OK)
            {
                if (newQty <= 0)
                {
                    MessageBox.Show("换料数量必须大于0".L10N());
                }
                xpButton1.Text = newQty.ToString();
            }
        }

        /// <summary>
        /// 扫描条码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void watermarkTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    var replaceQty = xpButton1.Text.Length > 0 ? decimal.Parse(xpButton1.Text) : 1;
                    var newChangeItemViewModel = ProductAssemblyDetailViewModelHelper.OnBarcodeChanged(this.watermarkTextBox1.Text.Trim(), CurrentProductAssemblyDetailViewModel,
                            ProductAssemblyDetails,
                            replaceQty

                            );
                    if (newChangeItemViewModel != null)
                    {
                        ChangeMaterialCard changeMaterialCard = new ChangeMaterialCard();
                        changeMaterialCard.SetData(newChangeItemViewModel);
                        changeMaterialCard.SetProductAssemblyDetails(this.ProductAssemblyDetails);
                        changeMaterialCard.CurrentProductAssemblyDetailViewModel = CurrentProductAssemblyDetailViewModel;
                        changeMaterialCard.ReflashForm = () => { RefalshTotalQty(); };
                        ContainPanel.Controls.Add(changeMaterialCard);
                    }
                    ProductAssemblyDetailViewModelHelper.ComputeTotalChangeQty(CurrentProductAssemblyDetailViewModel);
                    this.xpTextBoxTotalQty.AText = CurrentProductAssemblyDetailViewModel.TotalChangeQty.ToString();
                    this.watermarkTextBox1.Text = "";
                }
                catch (Exception ex)
                {
                    throw new ValidationException(ex.Message);
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xpButtonDel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ContainPanel.Controls.Count; i++)
            {
                var changeMaterialCard = ContainPanel.Controls[i] as ChangeMaterialCard;
                if (changeMaterialCard != null && changeMaterialCard.IsCheck)
                {
                    CurrentProductAssemblyDetailViewModel.ChangeItemViewModelList.Remove(changeMaterialCard.curChangeItemViewModel);
                    ContainPanel.Controls.Remove(changeMaterialCard);
                    i--;
                }
            }
            ProductAssemblyDetailViewModelHelper.ComputeTotalChangeQty(CurrentProductAssemblyDetailViewModel);
            this.xpTextBoxTotalQty.AText = this.CurrentProductAssemblyDetailViewModel.TotalChangeQty.ToString();
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.CurrentProductAssemblyDetailViewModel.ChangeItemViewModelList.Any())
            {
                var m = this.CurrentProductAssemblyDetailViewModel;
                if (m.ChangeItemViewModelList.Exists(p => p.ChangeQty <= 0))
                {
                    MessageBox.Show("换料数量必须大于0".L10N());
                    return;
                }
                if (m.ChangeItemViewModelList.Exists(p => p.ChangeQty > p.LoadItemBarcodeInfo.Qty))
                {
                    MessageBox.Show("换料数量不能大于条码可用数量".L10N());
                    return;
                }
                if (m.ChangeItemViewModelList.Sum(p => p.ChangeQty) > m.KeyItem.Qty)
                {
                    MessageBox.Show("换料数量不能大于装配数量".L10N());
                    return;
                }

                this.CurrentProductAssemblyDetailViewModel.HandleMethod = (ChangeItemHandleMethod)(int)xpComboBoxHandel.SelectedValue;
                this.CurrentProductAssemblyDetailViewModel.IsChangeSn = true;
                ShowChangeBarcode(this.CurrentProductAssemblyDetailViewModel);
            }
            else
            {
                CurrentProductAssemblyDetailViewModel.IsChangeSn = false;
                CurrentProductAssemblyDetailViewModel.ChangeBarcode = string.Empty;
            }
            DialogResult = DialogResult.OK;
            this.Close();

        }
        private void ShowChangeBarcode(ProductAssemblyDetailViewModel assembly)
        {
            string changeBarcode = string.Empty;
            assembly.ChangeItemViewModelList.ForEach(e => { changeBarcode += e.ChangeSn + ";"; });
            assembly.ChangeBarcode = changeBarcode;
        }
    }
}
