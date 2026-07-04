using Newtonsoft.Json;
using SIE.XPCJ.Common;
using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Controls;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Common.Settings;
using SIE.XPCJ.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SIE.XPCJ
{
    public partial class FormMain : Form
    {
        
        List<SIE.XPCJ.Common.Controls.XPButton> listMenuButton = new List<SIE.XPCJ.Common.Controls.XPButton>();
        List<Label> listMenuLabel = new List<Label>();
        public FormMain()
        {
            LanguageHelper.StartUploadCultureResoure();
            InitializeComponent();
            VersionInfo versionInfo = VersionInfo.GetLocalVersion();
            Global.VersionName = versionInfo.Version;
            this.SetButtons();
        }

        private void SetButtons()
        {
            listMenuButton.Add(button1);
            listMenuLabel.Add(label2);

            listMenuButton.Add(button2);
            listMenuLabel.Add(label3);

            listMenuButton.Add(button3);
            listMenuLabel.Add(label4);

            listMenuButton.Add(button4);
            listMenuLabel.Add(label5);

            listMenuButton.Add(button8);
            listMenuLabel.Add(label9);

            listMenuButton.Add(button7);
            listMenuLabel.Add(label8);

            listMenuButton.Add(button6);
            listMenuLabel.Add(label7);

            listMenuButton.Add(button5);
            listMenuLabel.Add(label6);

            listMenuButton.Add(button12);
            listMenuLabel.Add(label13);

            listMenuButton.Add(button11);
            listMenuLabel.Add(label12);

            listMenuButton.Add(button10);
            listMenuLabel.Add(label11);

            listMenuButton.Add(button9);
            listMenuLabel.Add(label10);
        }


        /// <summary>
        /// 菜单按钮布局
        /// </summary>
        /// <param name="buttonCountInOneRow"></param>
        private void ResetButtonLocation(int buttonCountInOneRow)
        {
            int totalRow = listMenuButton.Count / buttonCountInOneRow;
            if (listMenuButton.Count % buttonCountInOneRow > 0)
                totalRow += 1;

            int buttonWidth = listMenuButton[0].Width;
            int buttonHeight = listMenuButton[0].Height;

            int spaceWidth = (this.ClientRectangle.Width - buttonCountInOneRow * buttonWidth) / (buttonCountInOneRow + 1);
            int contentHeight = this.ClientRectangle.Height - this.xpTitle1.Height - this.statusBar.Height;
            int spaceHeight = (contentHeight - totalRow * buttonHeight) / (totalRow + 1);
            int startY = this.xpTitle1.Height;

            this.xpButtonPrePage.Location = new System.Drawing.Point(this.xpButtonPrePage.Location.X, startY + spaceHeight);
            this.xpButtonNextPage.Location = new System.Drawing.Point(this.xpButtonNextPage.Location.X, this.xpButtonPrePage.Location.Y);

            for (int row = 0; row < totalRow; row++)
            {
                int y = startY + spaceHeight * (row + 1) + row * buttonHeight;
                for (int col = 0; col < buttonCountInOneRow; col++)
                {
                    int x = spaceWidth * (col + 1) + col * buttonWidth;
                    int index = buttonCountInOneRow * row + col;
                    listMenuButton[index].Location = new System.Drawing.Point(x, y);
                    listMenuLabel[index].Location = new System.Drawing.Point(x - (listMenuLabel[index].Width - buttonWidth) / 2, y + buttonHeight + 10);
                }
            }
        }

        private List<AppMenu> listVisibleMenus;
        private int PageCount = 0; //多少页
        private int PageIndex = 0; //页码
        private int MenuCountOnePage = 12;

        /// <summary>
        /// 从配置文件读取菜单按钮权限名称，按索引设置
        /// </summary>
        private void ResetMenuButtons()
        {
            int startIndex = this.PageIndex * this.MenuCountOnePage;
            int buttonIndex = 0;
            for (int i = startIndex; i < this.listVisibleMenus.Count; i++)
            {
                AppMenu menu = this.listVisibleMenus[i];

                listMenuButton[buttonIndex].Visible = true;
                listMenuLabel[buttonIndex].Visible = true;
                listMenuButton[buttonIndex].Tag = i;
                listMenuButton[buttonIndex].BackgroundImage = menu.GetIconImage();
                listMenuButton[buttonIndex].PrivilegeName = menu.Privilege;
                listMenuLabel[buttonIndex].Text = menu.Name.L10N();

                if (buttonIndex == MenuCountOnePage - 1)
                    break;

                buttonIndex++;
            }

            if (buttonIndex < MenuCountOnePage - 1)
            {
                for (int index = buttonIndex; index < MenuCountOnePage; index++)
                {
                    listMenuButton[index].Visible = false;
                    listMenuLabel[index].Visible = false;
                }
            }

            this.xpButtonNextPage.Visible = this.PageIndex < this.PageCount - 1;
            this.xpButtonPrePage.Visible = this.PageIndex > 0;

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Global.ScreenWidth = this.Width;
            Global.ScreenHeight = this.Height;

            this.Visible = false;
            this.ShowLogin();
        }

        private void ShowLogin()
        {
            FormLogin frmLogin = new FormLogin();
            if (frmLogin.ShowDialog() != DialogResult.OK)
            {
                this.Close();
                return;
            }

            //登录成功走下面的代码**************************************************************************************************
            this.Visible = true;
            this.xpTitle1.AUserInfo = LoginInfo.Instance.UserName;

            //设置组织架构名称
            InvOrg org = LoginInfo.Instance.AllInvOrgs.Find(p => p.Code == LoginInfo.Instance.InvOrgId);
            if (org != null)
                this.xpTitle1.AInvOrg = "    " + org.Name;
            else
                this.xpTitle1.AInvOrg = "    " + "请设置组织架构".L10N();

            //设置权限
            this.listVisibleMenus = new List<AppMenu>();
            foreach (AppMenu menu in AppMenu.All.FindAll(p => p.Visible && p.IsShowMenuButton))
            {
                if (LoginInfo.Instance.AllPermissions.Find(p => p.ModuleKey.Equals(menu.Privilege)) != null)
                    this.listVisibleMenus.Add(menu);
            }
            this.PageCount = this.listVisibleMenus.Count / this.MenuCountOnePage;
            if (this.listVisibleMenus.Count % this.MenuCountOnePage > 0)
                this.PageCount += 1;

            ResetMenuButtons();
            ResetButtonLocation(4);
            this.statusBar.RefreshInfo();

        }


        /// <summary>
        /// 过站采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonMenu_Click(object sender, EventArgs e)
        {
            XPButton curButton = sender as SIE.XPCJ.Common.Controls.XPButton;

            int buttonIndex = Convert.ToInt32(curButton.Tag);
            if (AppMenu.All[buttonIndex].OpenMainForm(this))
                this.Hide();
        }

        private void xpTitle1_AExitClick_1(object sender, EventArgs e)
        {
            FormQuitDialog frm = new FormQuitDialog();
            DialogResult result = frm.ShowDialog();

            if (result == DialogResult.Yes)
                this.Close();
            else if (result == DialogResult.No)
            {
                this.Visible = false;
                this.ShowLogin();
            }
        }

        private void xpButtonNextPage_Click(object sender, EventArgs e)
        {
            this.PageIndex += 1;
            this.ResetMenuButtons();
            this.ResetButtonLocation(4);
        }

        private void xpButtonPrePage_Click(object sender, EventArgs e)
        {
            this.PageIndex -= 1;
            this.ResetMenuButtons();
            this.ResetButtonLocation(4);
        }
    }
}
