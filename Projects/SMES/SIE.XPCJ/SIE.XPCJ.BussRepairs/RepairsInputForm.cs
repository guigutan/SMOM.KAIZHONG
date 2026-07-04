using SIE.XPCJ.Common.Controls;
using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Models.WIP;
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
    public partial class RepairsInputForm : XPFormBaseDialog
    {
        private List<RepairMeasure> m_repairMeasures;
        private List<DefectResponsibility> m_defectResponsibility;
        private List<DefectResponsibilityCategory> m_DefectResponsibilityCategory;

        /// <summary>
        /// 所有缺陷责任分类
        /// </summary>
        protected List<ResponsibilityCategoryItem> CategoryList = new List<ResponsibilityCategoryItem>();


        /// <summary>
        /// 选中的缺陷责任分类
        /// </summary>
        protected List<ResponsibilityCategoryItem> SelectedCategoryList = new List<ResponsibilityCategoryItem>();

        /// <summary>
        /// 当前列表中的缺陷责任分类
        /// </summary>
        protected List<ResponsibilityCategoryItem> CurrentCategoryList = new List<ResponsibilityCategoryItem>();

        /// <summary>
        /// 缺陷责任分类项目 字典
        /// </summary>
        Dictionary<double, ResponsibilityCategoryItem> allCategoryItem = new Dictionary<double, ResponsibilityCategoryItem>();

        /// <summary>
        /// 缺陷责任分类 字典
        /// </summary>
        Dictionary<double, DefectResponsibilityCategory> _allCategory = new Dictionary<double, DefectResponsibilityCategory>();

        /// <summary>
        /// 重新加载责任分类
        /// </summary>
        bool _reloadCategory;

        /// <summary>
        /// 外部可重新取值
        /// </summary>
        public RepairDefectViewModel RepairDefectViewModel { get; set; }

        public RepairsInputForm(RepairDefectViewModel repairDefectViewModel)
        {
            InitializeComponent();
            RepairDefectViewModel = repairDefectViewModel;
        }


        private void RepairsInputForm_Load(object sender, EventArgs e)
        {
            m_repairMeasures = RepairsService.GetAllRepairMeasure();
            m_defectResponsibility = RepairsService.GetAllDefectResponsibility();
            m_DefectResponsibilityCategory = RepairsService.GetAllDefectResponsibilityCategory();
            flowLayoutPanelSelected.Controls.Clear();
            InitData();
            foreach (var item in m_repairMeasures)
            {
                HighlightButton highlightButton = new HighlightButton();
                highlightButton.BackColor = Color.White;
                highlightButton.Size = new Size(120, 40);
                highlightButton.Text = item.Name;
                highlightButton.DataId = item.Id;
                highlightButton.Name = item.Id.ToString();
                flowLayoutPanel1.Controls.Add(highlightButton);
            }
            if (RepairDefectViewModel != null)
            {
                this.richTextBoxMark.Text = RepairDefectViewModel.Remark;
                this.xpTextBoxDefectCode.AText = RepairDefectViewModel.DefectCode;
                this.xpTextBoxDefectDesc.AText = RepairDefectViewModel.DefectDesc;
                this.xpTextBoxProcess.AText = RepairDefectViewModel.ProcessName;
                this.xpTextBoxQty.AText = "1";
                this.xpTextBoxItemDesc.AText = RepairDefectViewModel.InspItemName;
                foreach (var measure in RepairDefectViewModel.MeasureList)
                {
                    var measureBtns = flowLayoutPanel1.Controls.Find(measure.Id.ToString(), false);
                    if (measureBtns.Length > 0)
                    {
                        var btn = measureBtns[0] as HighlightButton;
                        if (btn != null)
                        {
                            btn.SetHighLight();
                        }
                    }
                }
                foreach (var responsibility in RepairDefectViewModel.ResponsibilityList)
                {
                    ResponsibilitySelectedButton responsibilitySelectedButton = new ResponsibilitySelectedButton();
                    responsibilitySelectedButton.Name = responsibility.Id.ToString();
                    responsibilitySelectedButton.SetDefectResponsibility(responsibility);
                    flowLayoutPanelSelected.Controls.Add(responsibilitySelectedButton);
                    responsibilitySelectedButton.ClickAction = () =>
                    {
                        flowLayoutPanelSelected.Controls.Remove(responsibilitySelectedButton);
                        responsibilitySelectedButton.Dispose();
                    };
                }
            }


        }

        private void InitData()
        {
            flowLayoutPanelResponsibility.Controls.Clear();
            flowLayoutPanelSelectedCategory.Controls.Clear();

            foreach (var item in m_defectResponsibility)
            {
                AddCategory(item.CategoryId);
                SetResponsibilityButtonList(item);
            }
            flowLayoutPanelSelectedCategory.Controls.Add(xpButtonAll);
            flowLayoutPanelCategory.Controls.Clear();
            //加载分类
            SetCategoryButton(CurrentCategoryList);
        }

        /// <summary>
        /// 设置缺陷责任按钮列表
        /// </summary>
        /// <param name="item"></param>
        private void SetResponsibilityButtonList(DefectResponsibility item)
        {
            var responsibilityButton = new DefectButton();
            responsibilityButton.Size = new Size(100, 30);
            responsibilityButton.Name = item.Id.ToString();
            responsibilityButton.ClickAction = () =>
            {
                var isexsited = flowLayoutPanelSelected.Controls.Find(item.Id.ToString(), false);
                if (isexsited.Length <= 0)
                {
                    ResponsibilitySelectedButton responsibilitySelectedButton = new ResponsibilitySelectedButton();
                    responsibilitySelectedButton.Name = item.Id.ToString();
                    responsibilitySelectedButton.SetDefectResponsibility(item);
                    flowLayoutPanelSelected.Controls.Add(responsibilitySelectedButton);
                    responsibilitySelectedButton.ClickAction = () =>
                    {
                        flowLayoutPanelSelected.Controls.Remove(responsibilitySelectedButton);
                        responsibilitySelectedButton.Dispose();
                    };
                }
            };
            responsibilityButton.SetDefectResponsibility(item);
            flowLayoutPanelResponsibility.Controls.Add(responsibilityButton);
        }

        /// <summary>
        /// 设置分类列表
        /// </summary>
        /// <param name="responsibilityCategoryItems"></param>
        public void SetCategoryButton(List<ResponsibilityCategoryItem> responsibilityCategoryItems)
        {
            foreach (var category in responsibilityCategoryItems)
            {
                var categoryButton = new ResponsibilityCategoryButton();
                categoryButton.Name = category.Category.Id.ToString();
                categoryButton.SetDefectCategoryItem(category);
                flowLayoutPanelCategory.Controls.Add(categoryButton);

                categoryButton.ClickAction = () =>
                {
                    var isexsited = flowLayoutPanelSelectedCategory.Controls.Find(category.Category.Id.ToString(), false);
                    if (isexsited.Length <= 0)//不存在则加入
                    {
                        var selectedCategoryButton = new ResponsibilityCategoryButton();
                        selectedCategoryButton.Name = category.Category.Id.ToString();
                        selectedCategoryButton.SetDefectCategoryItem(category);

                        flowLayoutPanelSelectedCategory.Controls.Add(selectedCategoryButton);
                        //选中分类点击清除下方分类并加载子
                        selectedCategoryButton.ClickAction = () =>
                        {
                            flowLayoutPanelCategory.Controls.Clear();
                            SetCategoryButton(category.Children);
                            //递归清除自己子节点的分类
                            RemoveChildCategory(category);
                        };

                        //清空待分类列表
                        flowLayoutPanelCategory.Controls.Clear();
                        //生成子分类列表
                        SetCategoryButton(category.Children);
                        //清空责任代码列表
                        flowLayoutPanelResponsibility.Controls.Clear();
                        SetAllDefectResponsibility(category);
                    }

                };
            }
        }

        /// <summary>
        /// 设置当前级别下的包括下级的缺陷责任代码
        /// </summary>
        /// <param name="category"></param>
        private void SetAllDefectResponsibility(ResponsibilityCategoryItem category)
        {
            var responsibilityList = m_defectResponsibility.FindAll(m => m.CategoryId == category.Category.Id);
            foreach (var item in responsibilityList)
            {
                SetResponsibilityButtonList(item);
            }
            foreach (var item in category.Children)
            {
                SetAllDefectResponsibility(item);
            }
        }

        /// <summary>
        /// 递归移除已选分类
        /// </summary>
        /// <param name="childResponsibilityCategoryItem"></param>
        private void RemoveChildCategory(ResponsibilityCategoryItem childResponsibilityCategoryItem)
        {
            var childCategoryNames = childResponsibilityCategoryItem.Children.Select(m => m.Category.Id.ToString()).ToList();
            foreach (var childCategoryName in childCategoryNames)
            {
                var childCategoryBtns = flowLayoutPanelSelectedCategory.Controls.Find(childCategoryName, false);
                if (childCategoryBtns.Length > 0)
                {
                    var item = childCategoryBtns[0] as ResponsibilityCategoryButton;
                    if (item != null)
                    {

                        flowLayoutPanelSelectedCategory.Controls.Remove(childCategoryBtns[0]);
                        RemoveChildCategory(item.CurrentResponsibilityCategory);
                    }
                }
            }
        }

        Dictionary<double, DefectResponsibilityCategory> AllCategory
        {
            get
            {
                if (_allCategory == null || _reloadCategory)
                {
                    _allCategory = new Dictionary<double, DefectResponsibilityCategory>();
                    var all = m_DefectResponsibilityCategory;
                    all.ForEach(p =>
                    {
                        _allCategory.Add(p.Id, p);
                    });
                    _reloadCategory = false;
                }

                return _allCategory;
            }
        }

        protected virtual ResponsibilityCategoryItem AddCategory(double categoryId)
        {
            ResponsibilityCategoryItem item;
            if (!allCategoryItem.TryGetValue(categoryId, out item))
            {
                DefectResponsibilityCategory category;
                if (!AllCategory.TryGetValue(categoryId, out category))
                {
                    //新增的缺陷责任分类需要重新加载
                    _reloadCategory = true;
                    category = AllCategory[categoryId];
                }
                item = new ResponsibilityCategoryItem { Category = category };
                allCategoryItem.Add(item.Category.Id, item);
                var pid = category.TreePId;
                DefectResponsibilityCategory parent;
                if (pid == null || !AllCategory.TryGetValue(pid.Value, out parent))
                {
                    CategoryList.Add(item);
                    CurrentCategoryList.Add(item);
                }
                else
                {
                    ResponsibilityCategoryItem itemParent;
                    if (!allCategoryItem.TryGetValue(parent.Id, out itemParent))
                        itemParent = AddCategory(parent.Id);

                    itemParent.Children.Add(item);
                    item.Parent = itemParent;
                }
            }

            return item;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void xpButtonAll_Click(object sender, EventArgs e)
        {
            InitData();
        }

        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {

            if (RepairDefectViewModel == null)
            {
                return;
            }
            RepairDefectViewModel.MeasureList.Clear();
            RepairDefectViewModel.ResponsibilityList.Clear();

            foreach (var item in flowLayoutPanel1.Controls)
            {
                var itemBtn = item as HighlightButton;
                if (itemBtn != null&& itemBtn.IsHighlight)
                {
                    var dataId = Convert.ToDouble(itemBtn.Name);
                    var measure = m_repairMeasures.FirstOrDefault(m => m.Id == dataId);
                    if (measure != null)
                    {
                        RepairDefectViewModel.MeasureList.Add(measure);
                    }
                }
            }
            foreach (var item in flowLayoutPanelSelected.Controls)
            {
                var itemBtn = item as ResponsibilitySelectedButton;
                if (itemBtn != null)
                {
                    var dataId = Convert.ToDouble(itemBtn.Name);
                    var responsibility = m_defectResponsibility.FirstOrDefault(m => m.Id == dataId);
                    if (responsibility != null)
                    {
                        RepairDefectViewModel.ResponsibilityList.Add(responsibility);
                    }
                }
            }
            RepairDefectViewModel.Remark = this.richTextBoxMark.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
