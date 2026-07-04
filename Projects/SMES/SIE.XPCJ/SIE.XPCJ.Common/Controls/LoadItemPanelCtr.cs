using SIE.XPCJ.Common.Controls;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.Exceptions;
using SIE.XPCJ.Models.WIP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class LoadItemPanelCtr : BaseUserControl
    {
        /// <summary>
        /// 刷新上料列表和下料列表的委托
        /// </summary>
        public Action<LoadItem, string> ReflashLoadItemListAction { get; set; }

        public Action<LoadItem, string> ReflashUnLoadItemListAction { get; set; }

        public LoadItem loadItem { get; set; }
        public LoadItemPanelCtr()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void LoadItemPanelCtr_Load(object sender, EventArgs e)
        {
            if (loadItem != null)
            {
                this.label13.Text = loadItem.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                this.label2.Text = loadItem.SourceCode;
                this.label4.Text = loadItem.ItemCode;
                this.label5.Text = loadItem.ItemName;
                this.label7.Text = loadItem.SourceType.ToLabel();
                this.lbQty.Text = loadItem.LoadQty.ToString();
                this.label11.Text = loadItem.Qty.ToString();
                this.label9.Text = loadItem.ItemExtPropName;
            }
        }

        private void xpButton2_Click(object sender, EventArgs e)
        {
            if (XPFormNumberInput.ShowInput(loadItem.Qty, out decimal newQty) == DialogResult.OK)
            {
                if (newQty <= 0)
                {
                    throw new ValidationException("数量必须大于0".L10N());
                }

                string erroMsg = "";
                try
                {
                    WipService.UnloadItem(loadItem.Id, newQty);
                }
                catch (Exception ex)
                {
                    erroMsg = ex.Message;
                }
                finally
                {
                     ReflashLoadItemListAction?.Invoke(loadItem, erroMsg);
                }
            }
        }

        /// <summary>
        /// 不良下料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xpButton1_Click(object sender, EventArgs e)
        {
            string erroMsg = "";
            bool isCancle = false;
            try
            {
                XPFormSelectDefect defectSelectForm = new XPFormSelectDefect();
                defectSelectForm.ShowQtyInput = true;
                var defects = WipService.GetDefects();
                if (!defects.Any())
                {
                    MessageBox.Show("系统未配置缺陷，请先配置缺陷".L10N());
                    return;
                }
                defectSelectForm.DefectListData.AddRange(defects);

                var result = defectSelectForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    List<DefectItem> defectItemList = defectSelectForm.CurrentDefectList;
                    if (!defectItemList.Any())
                    {
                        MessageBox.Show("请至少选择一个缺陷".L10N());
                        return;
                    }
                    var selectDefects = defectItemList.Select(p => new DefectData { Qty = p.Qty, DefectId = p.Defect.Id }).ToList();
                    WipService.DefectUnloadItem(loadItem.Id, selectDefects);
                }
                else
                {
                    isCancle = true;
                }
            }

            catch (Exception ex)
            {
                erroMsg = ex.Message;
            }
            finally
            {
                if (!isCancle)
                {
                    ReflashUnLoadItemListAction?.Invoke(loadItem, erroMsg);
                }
            }
        }
    }
}
