using Newtonsoft.Json;
using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.WIP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class XPTitle : UserControl
    {
        /// <summary>
        /// 保留主页面引用
        /// </summary>
        public Form FormMain;

        public XPTitle()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public string ATitle
        {
            get { return this.labelTitle.Text; }
            set { this.labelTitle.Text = value; }
        }

        public Font ATileFont
        {
            get { return this.labelTitle.Font; }
            set { this.labelTitle.Font = value; }
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        public string AUserInfo
        {
            get { return this.labelUserInfo.Text; }
            set { this.labelUserInfo.Text = value; }
        }

        /// <summary>
        /// 当前库存组织
        /// </summary>
        public string AInvOrg
        {
            get { return this.xpButtonInvOrg.Text; }
            set { this.xpButtonInvOrg.Text = value; }
        }

        private EnumXPTitleType _AType = EnumXPTitleType.Default;
        public EnumXPTitleType AType
        {
            get { return _AType; }
            set 
            {
                _AType = value;
                switch (value)
                {
                    case EnumXPTitleType.WorkerCell:
                        panelDefault.Visible = false;
                        panelWorkCell.Visible = true;
                        break;
                    default:
                        panelDefault.Visible = true;
                        panelWorkCell.Visible = false;
                        break;
                }
            } 
        }


        #region 退出按钮事件
        /// <summary>
        /// 退出按钮事件
        /// </summary>
        public event EventHandler AExitClick;

        private void xpButtonExit_Click(object sender, EventArgs e)
        {
            if (AExitClick == null)
            {
                //默认事件
                if (this.ParentForm != null)
                {
                    if (this.FormMain != null)
                        this.FormMain.Show();
                    this.ParentForm.Close();
                }
            }
            else
            {
                AExitClick.Invoke(this, e);
            }
        }
        #endregion

        #region 切换库存组织事件
        /// <summary>
        /// 切换库存组织事件
        /// </summary>
        public event EventHandler AInvOrgClick;

        private void xpButtonInvOrg_Click(object sender, EventArgs e)
        {
            if (AInvOrgClick == null)
            {
                //默认事件
                XPFormChangeInvOrg frmChangeInvOrg = new XPFormChangeInvOrg();
                if (frmChangeInvOrg.ShowDialog() == DialogResult.OK)
                {
                    foreach (InvOrg org in LoginInfo.Instance.AllInvOrgs)
                    {
                        if (org.Id == LoginInfo.Instance.InvOrgId)
                        {
                            this.AInvOrg = "    " + org.Name;
                            break;
                        }
                    }
                }
            }
            else
            {
                AInvOrgClick.Invoke(this, e);
            }
        }
        #endregion

        #region 切换工作单元：产线/工序/工位
        private Workcell _workcell = null;
        public Workcell Workcell
        {
            get { return _workcell; }
        }

        /// <summary>
        /// 选中的工序信息
        /// </summary>
        private ProcessInfo m_processInfo;

        /// <summary>
        /// 选中的工位信息
        /// </summary>
        private StationInfo m_stationInfo;

        /// <summary>
        ///选中的资源信息
        /// </summary>

        private ResourceInfo m_resourceInfo;

        /// <summary>
        /// 采集类型
        /// </summary>
        public ProcessType AProcessType { get; set; } = ProcessType.Assembly;

        /// <summary>
        /// 获取控件所在窗体WorkCell在本地Properties.Settings.settings中的关键字
        /// </summary>
        /// <returns></returns>
        private string GetLocalSettingKey()
        {
            Type type = ParentForm.GetType();
            return $"{type.FullName},{type.Assembly.GetName().Name}";
        }

        /// <summary>
        /// 从Properties.Settings.settings中读取工作单元信息
        /// </summary>
        private void LoadWorkerCellFromLocalSetting()
        {
            if (this.AType != EnumXPTitleType.WorkerCell)
                return;

            var setting = Properties.Settings.Default.Workcell;
            if (string.IsNullOrEmpty(setting))
                return;

            string key = GetLocalSettingKey();

            Dictionary<string, Workcell> data = JsonConvert.DeserializeObject<Dictionary<string, Workcell>>(setting);
            if (data == null || !data.ContainsKey(key))
                return;

            _workcell = data[key];

            if (_workcell != null && _workcell.EmployeeId == LoginInfo.Instance.EmployeeId)
            {
                this.m_resourceInfo = new ResourceInfo { Id = _workcell.ResourceId, Name = _workcell.ResourceName };
                this.m_processInfo = new ProcessInfo { Id = _workcell.ProcessId, Name = _workcell.ProcessName };
                this.m_stationInfo = new StationInfo { Id = _workcell.StationId, Name = _workcell.StationName };
            }
            else
            {
                ///清空工作单元信息
                data[key] = null;
                Properties.Settings.Default.Workcell = JsonConvert.SerializeObject(data);
                Properties.Settings.Default.Save();
                _workcell = data[key];
            }
        }

        private void SaveWorkCellToLocalSetting()
        {
            string key = GetLocalSettingKey();

            _workcell = new Workcell()
            {
                EmployeeId = LoginInfo.Instance.EmployeeId,
                ResourceId = m_resourceInfo == null ? 0 : m_resourceInfo.Id,
                ResourceName = m_resourceInfo == null ? "" : m_resourceInfo.Name,
                ProcessId = m_processInfo == null ? 0 : m_processInfo.Id,
                ProcessName = m_processInfo == null ? "" : m_processInfo.Name,
                StationId = m_stationInfo == null ? 0 : m_stationInfo.Id,
                StationName = m_stationInfo == null ? "" : m_stationInfo.Name,
            };

            var setting = Properties.Settings.Default.Workcell;
            Dictionary<string, Workcell> data = null;
            if (!string.IsNullOrEmpty(setting))
            {
                data = JsonConvert.DeserializeObject<Dictionary<string, Workcell>>(setting);
            }

            if (data == null)
            {
                data = new Dictionary<string, Workcell>();
            }
            data[key] = _workcell;
            Properties.Settings.Default.Workcell = JsonConvert.SerializeObject(data);
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 切换工作单元：产线/工序/工位
        /// </summary>
        public event EventHandler AChangeWorkCellClick;

        /// <summary>
        /// 工作单元变更后触发的事件
        /// </summary>
        public event EventHandler AWorkCellChanged;

        /// <summary>
        /// 切换工作单元
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xpButtonChangeWorkCell_Click(object sender, EventArgs e)
        {
            if (AChangeWorkCellClick == null)
            {
                //默认事件
                ShowFormChangeWorkCell();
            }
            else
            {
                AChangeWorkCellClick.Invoke(this, e);
            }
        }

        /// <summary>
        /// 显示弹窗获取工作单元
        /// </summary>
        public void ShowFormChangeWorkCell()
        {
            XPFormChangeWorkCell switchWorkstationForm = new XPFormChangeWorkCell(this.AProcessType);
            
            //设置弹出窗口的已有值
            switchWorkstationForm.m_resourceInfo = this.m_resourceInfo;
            switchWorkstationForm.m_processInfo = this.m_processInfo;
            switchWorkstationForm.m_stationInfo = this.m_stationInfo;

            if (switchWorkstationForm.ShowDialog() == DialogResult.OK)
            {
                m_resourceInfo = switchWorkstationForm.m_resourceInfo;
                m_processInfo = switchWorkstationForm.m_processInfo;
                m_stationInfo = switchWorkstationForm.m_stationInfo;

                SaveWorkCellToLocalSetting();
                ResetSetWorkCellLabeText();
                AWorkCellChanged?.Invoke(this, null);
            }
        }

        /// <summary>
        /// 设置产线/工序/工位Label控件的文字
        /// </summary>
        public void ResetSetWorkCellLabeText()
        {
            this.labelWorkerCellLine.Text = this.m_resourceInfo == null ? string.Empty : this.m_resourceInfo.Name;
            this.labelWorkCellProcess.Text = this.m_processInfo == null ? string.Empty : this.m_processInfo.Name;
            this.labelWorkCellPosition.Text = this.m_stationInfo == null ? string.Empty : this.m_stationInfo.Name;
        }

        /// <summary>
        /// 显示工作单元完全信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelWorkCell_Click(object sender, EventArgs e)
        {
            XPFormWokerCellDetai.ShowInfo(this.panelWorkCell.Parent, this.panelWorkCell, this.m_resourceInfo, this.m_processInfo, this.m_stationInfo);
        }
        #endregion

        private void XPTitle_Load(object sender, EventArgs e)
        {
            //加载工作单元
            if (this.AType == EnumXPTitleType.WorkerCell)
            {
                this.LoadWorkerCellFromLocalSetting();
                this.ResetSetWorkCellLabeText();
            }
        }
    }

    public enum EnumXPTitleType
    {
        /// <summary>
        /// 主界面显示当前用户和组织架构
        /// </summary>
        Default,
        /// <summary>
        /// 采集界面显示工作单元
        /// </summary>
        WorkerCell,
    }
}
