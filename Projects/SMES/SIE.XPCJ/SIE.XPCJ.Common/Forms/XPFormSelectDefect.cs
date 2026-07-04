using SIE.XPCJ.Common.Controls;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.WIP.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Forms
{
    public partial class XPFormSelectDefect : XPFormBaseDialog
    {
        public bool ShowQtyInput = false;
        public XPFormSelectDefect()
        {
            InitializeComponent();
            DefectListData = new List<Defect>();
        }


        /// <summary>
        /// 所有按钮
        /// </summary>
        private List<DefectButton> allDefectButtonList = new List<DefectButton>();

        /// <summary>
        /// 所有分类按钮
        /// </summary>
        private List<DefectCategoryButton> defectCategoryButtons = new List<DefectCategoryButton>();

        /// <summary>
        /// 当前选中分类
        /// </summary>
        private List<DefectCategoryButton> currentDefectCategoryButtons = new List<DefectCategoryButton>();
        ///// <summary>
        ///// 缺陷分类子类按钮
        ///// </summary>
        //private List<DefectCategoryButton> defectCategoryChildrenButtons = new List<DefectCategoryButton>();
        /// summary>
        /// 当前工序的所有缺陷数据
        /// </summary>
        public List<Defect> DefectListData
        {
            get;
            set;
        }

        /// <summary>
        /// 所有缺陷项目
        /// </summary>
        public List<DefectItem> DefectList { get; set; } = new List<DefectItem>();

        /// <summary>
        /// 当前列表中的缺陷项目
        /// </summary>
        public List<DefectItem> CurrentDefectList { get; set; } = new List<DefectItem>();

        /// <summary>
        /// 所有分类
        /// </summary>
        public List<DefectCategoryItem> CategoryList { get; set; } = new List<DefectCategoryItem>();

        /// <summary>
        /// 选中的分类
        /// </summary>
        public List<DefectCategoryItem> SelectedCategoryList { get; set; } = new List<DefectCategoryItem>();

        /// <summary>
        /// 当前列表中的分类
        /// </summary>
        public List<DefectCategoryItem> CurrentCategoryList { get; set; } = new List<DefectCategoryItem>();

        /// <summary>
        /// 缺陷分类项目 字典
        /// </summary>
        Dictionary<double, DefectCategoryItem> allCategoryItem = new Dictionary<double, DefectCategoryItem>();

        /// <summary>
        /// 缺陷分类 字典
        /// </summary>
        Dictionary<double, DefectCategory> _allCategory = new Dictionary<double, DefectCategory>();

        /// <summary>
        /// 缺陷分类 字典
        /// </summary>
        Dictionary<double, DefectCategory> AllCategory
        {
            get
            {
                if (_allCategory == null)
                {
                    _allCategory = new Dictionary<double, DefectCategory>();
                    var all = WipService.GetDefectCategory();
                    all.ForEach(p =>
                    {
                        _allCategory.Add(p.Id, p);
                    });
                }

                return _allCategory;
            }
        }
        protected virtual DefectCategoryItem AddCategory(double categoryId)
        {
            DefectCategoryItem item;
            if (!allCategoryItem.TryGetValue(categoryId, out item))
            {
                if (!AllCategory.ContainsKey(categoryId))
                {
                    _allCategory = null;
                }

                DefectCategory category = AllCategory[categoryId];
                item = new DefectCategoryItem { Category = category };
                allCategoryItem.Add(item.Category.Id, item);
                var pid = category.TreePId;
                DefectCategory parent;
                if (pid == null || !AllCategory.TryGetValue(Convert.ToDouble(pid), out parent))
                {
                    CategoryList.Add(item);
                    CurrentCategoryList.Add(item);
                }
                else
                {
                    DefectCategoryItem itemParent;
                    if (!allCategoryItem.TryGetValue(parent.Id, out itemParent))
                        itemParent = AddCategory(parent.Id);

                    itemParent.Children.Add(item);
                    item.Parent = itemParent;
                }
            }

            return item;
        }


        /// <summary>
        /// 添加缺陷项目到选中列表
        /// </summary>
        /// <param name="defectItem"></param>
        public void AddSelectDefectItem(DefectItem defectItem)
        {
            var item = new DefectSelectedButton();
            item.ClickAction = () =>
            {
                this.flowLayoutPanel4.Controls.Remove(item);
            };
            item.SetDefectItem(defectItem);
            bool isexsited = false;
            foreach (var exsitedItem in this.flowLayoutPanel4.Controls)
            {
                var exsitedItemData = exsitedItem as DefectSelectedButton;
                if (exsitedItemData.DefectItem.Defect.Id == defectItem.Defect.Id)
                {
                    isexsited = true;
                }
            }
            if (!isexsited)
            {
                this.flowLayoutPanel4.Controls.Add(item);
            }
        }
        /// <summary>
        /// 添加缺陷项目到待选
        /// </summary>
        /// <param name="defectItem"></param>
        public void AddDefectItem(DefectItem defectItem)
        {
            var item = new DefectButton();
            item.Size = new Size(100, 30);
            item.ClickAction = () =>
            {
                if (ShowQtyInput)
                {
                    if (XPFormNumberInput.ShowInput(0, out decimal newQty) == DialogResult.OK)
                    {
                        defectItem.Qty = newQty > 0 ? (double)newQty : 1;
                        AddSelectDefectItem(defectItem);
                    }
                }
                else
                {
                    AddSelectDefectItem(defectItem);
                }
            };
            item.SetDefectItem(defectItem);
            this.flowLayoutPanel2.Controls.Add(item);
            allDefectButtonList.Add(item);
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InittalizeData()
        {
            foreach (var item in DefectListData)
            {
                var defectItem = new DefectItem()
                {
                    Qty = 1,
                    Location = "",
                    Defect = item

                };
                DefectList.Add(defectItem);
                AddCategory(item.DefectCategoryId);
                AddDefectItem(defectItem);

            }
            foreach (var select in CurrentDefectList)
            {
                AddSelectDefectItem(select);

            }
            SetCategoryBtns();
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefectSelectedForm_Load(object sender, EventArgs e)
        {
            this.InittalizeData();
        }

        /// <summary>
        /// 设置缺陷按钮根据分类Id
        /// </summary>
        /// <param name="defectCategoryId"></param>
        private void SetDefectBtnByCategoryId(DefectCategoryItem defectCategory)
        {
            this.flowLayoutPanel2.Controls.Clear();
            foreach (var ctr in allDefectButtonList)
            {
                var childCategoryIds = defectCategory.Children.Select(m => m.Category.Id).ToList();
                if (ctr.CurrentDefectItem.Defect.DefectCategoryId == defectCategory.Category.Id)
                {
                    this.flowLayoutPanel2.Controls.Add(ctr);
                }
                childCategoryIds.ForEach(d =>
                {
                    if (ctr.CurrentDefectItem.Defect.DefectCategoryId == d)
                    {
                        this.flowLayoutPanel2.Controls.Add(ctr);
                    }
                });
            }

        }

        /// <summary>
        /// 点击所有按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xpButton1_Click_1(object sender, EventArgs e)
        {
            if (defectCategoryButtons.Any())
            {
                foreach (var item in defectCategoryButtons)
                {
                    this.flowLayoutPanel3.Controls.Remove(item);
                }
            }
            if (currentDefectCategoryButtons.Any())
            {
                foreach (var item in currentDefectCategoryButtons)
                {
                    this.flowLayoutPanel1.Controls.Remove(item);
                }
            }
            this.flowLayoutPanel2.Controls.Clear();
            allDefectButtonList.ForEach(n =>
            {
                this.flowLayoutPanel2.Controls.Add(n);
            });
            SetCategoryBtns();
        }

        /// <summary>
        /// 设置分类按钮
        /// </summary>
        private void SetCategoryBtns()
        {
            foreach (var category in CategoryList)
            {
                var cateItemButton = new DefectCategoryButton();
                cateItemButton.SetDefectCategoryItem(category);
                this.flowLayoutPanel3.Controls.Add(cateItemButton);
                defectCategoryButtons.Add(cateItemButton);
                cateItemButton.ClickAction = () =>
                {
                    if (defectCategoryButtons.Any())
                    {
                        this.flowLayoutPanel3.Controls.Clear();
                        defectCategoryButtons.Clear();
                        this.flowLayoutPanel1.Controls.Add(cateItemButton);
                        currentDefectCategoryButtons.Add(cateItemButton);
                        cateItemButton.ClickAction = null;
                        cateItemButton.ClickAction = () =>
                        {
                            defectCategoryButtons.Clear();
                            this.flowLayoutPanel3.Controls.Clear();
                            for (int i = 0; i < currentDefectCategoryButtons.Count; i++)
                            {
                                var curBtn = currentDefectCategoryButtons[i];
                                if (curBtn == cateItemButton)
                                {
                                    continue;
                                }
                                this.flowLayoutPanel1.Controls.Remove(curBtn);
                                currentDefectCategoryButtons.Remove(curBtn);
                            }
                            foreach (var children in category.Children)
                            {
                                var childrenButton = new DefectCategoryButton();
                                childrenButton.SetDefectCategoryItem(children);
                                childrenButton.ClickAction = () =>
                                {
                                    this.flowLayoutPanel1.Controls.Add(childrenButton);
                                    currentDefectCategoryButtons.Add(childrenButton);
                                    childrenButton.ClickAction = null;
                                    SetDefectBtnByCategoryId(children);
                                };
                                this.flowLayoutPanel3.Controls.Add(childrenButton);
                                defectCategoryButtons.Add(childrenButton);
                            }

                            SetDefectBtnByCategoryId(category);
                        };
                        SetDefectBtnByCategoryId(category);
                    }
                    foreach (var children in category.Children)
                    {
                        var childrenButton = new DefectCategoryButton();
                        childrenButton.SetDefectCategoryItem(children);
                        childrenButton.ClickAction = () =>
                        {
                            this.flowLayoutPanel1.Controls.Add(childrenButton);
                            currentDefectCategoryButtons.Add(childrenButton);
                            childrenButton.ClickAction = null;
                            SetDefectBtnByCategoryId(children);
                        };
                        this.flowLayoutPanel3.Controls.Add(childrenButton);
                        defectCategoryButtons.Add(childrenButton);
                    }
                };
            }

        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.CurrentDefectList.Clear();
            foreach (var selectItem in flowLayoutPanel4.Controls)
            {
                var itemdata = (selectItem as DefectSelectedButton).DefectItem;
                this.CurrentDefectList.Add(itemdata);
            }

            if (!CurrentDefectList.Any())
            {
                MessageBox.Show("请至少选择一个缺陷".L10N());
                return;
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}
