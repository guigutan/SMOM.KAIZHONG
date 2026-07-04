using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Newtonsoft.Json;
using SIE.CrossPlatform.Collect.Common.ApiCall;
using SIE.CrossPlatform.Collect.Common.Forms;
using SIE.CrossPlatform.Collect.Models;
using SIE.CrossPlatform.Collect.Models.Enums;
using SIE.CrossPlatform.Collect.Models.WIP;

namespace SIE.CrossPlatform.Collect.Common.Controls;

public partial class XPTitle : UserControl
{


    /// <summary>
    /// 保留出程序的引用
    /// </summary>
    public Window FormMain { get; set; }
    public XPTitle()
    {

        InitializeComponent();

    }

    public string ATitle
    {
        get { return this.labelTitle.Text; }
        set { this.labelTitle.Text = value; }
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
        get { return this.xpInvOrgTextBlock.Text; }
        set { this.xpInvOrgTextBlock.Text = value; }
    }

    private EnumXPTitleType _AType = EnumXPTitleType.Default;
    public EnumXPTitleType AType
    {
        get { return _AType; }
        set
        {
            _AType = value;
            panelDefault.IsVisible = value == EnumXPTitleType.Default;
            panelWorkCell.IsVisible = value == EnumXPTitleType.WorkerCell;
        }
    }

    /// <summary>
    /// 获取用户控件的窗体
    /// </summary>
    /// <returns></returns>
    private Window GetWindow()
    {
        var topLevel = TopLevel.GetTopLevel(this);
        //默认事件
        if (topLevel is Window parentWin)
        {
            return parentWin;
        }
        return null;
    }

    #region 退出按钮事件
    /// <summary>
    /// 退出按钮事件
    /// </summary>
    public event EventHandler AExitClick;
    private void xpButtonExit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (AExitClick == null)
        {

            var parentWin = GetWindow();
            if (FormMain != null)//如果是主界面
            {
                this.FormMain.Show();
            }
            parentWin?.Close();
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

    private async void xpButtonInvOrg_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (AInvOrgClick == null)
        {

            XPFormChangeInvOrg changeInvOrg = new XPFormChangeInvOrg();
            var result = await changeInvOrg.ShowDialog<bool>(this.GetWindow());
            if (result)
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

        var parentForm = GetWindow();
        if (parentForm != null)
        {
            Type type = parentForm.GetType();
            return $"{type.FullName},{type.Assembly.GetName().Name}";
        }
        return "";
    }

    /// <summary>
    /// 从Properties.Settings.settings中读取工作单元信息
    /// </summary>
    private void LoadWorkerCellFromLocalSetting()
    {
        if (this.AType != EnumXPTitleType.WorkerCell)
            return;

        var setting = PropertiesSettingsUlits.Workcell;
        if (string.IsNullOrEmpty(setting))//为空尝试获取一次
        {
            setting = PropertiesSettingsUlits.GetProperties(nameof(PropertiesSettingsUlits.Workcell));
        }
        if (string.IsNullOrEmpty(setting))//再次空不再获取
        {
            return;
        }

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
            PropertiesSettingsUlits.Workcell = JsonConvert.SerializeObject(data);
            PropertiesSettingsUlits.SetProperties( nameof(PropertiesSettingsUlits.Workcell), PropertiesSettingsUlits.Workcell);
            _workcell = data[key];
        }
    }

    /// <summary>
    /// 保存工作单元信息到本地文件
    /// </summary>
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

        var setting = PropertiesSettingsUlits.Workcell;
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
        PropertiesSettingsUlits.Workcell = JsonConvert.SerializeObject(data);
        PropertiesSettingsUlits.SetProperties(nameof(PropertiesSettingsUlits.Workcell), PropertiesSettingsUlits.Workcell);
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
    private async void xpButtonChangeWorkCell_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (AChangeWorkCellClick == null)
        {
            //默认事件
            await ShowFormChangeWorkCell();
        }
        else
        {
            AChangeWorkCellClick.Invoke(this, e);
        }
    }

    /// <summary>
    /// 显示弹窗获取工作单元
    /// </summary>
    public async Task ShowFormChangeWorkCell()
    {
        XPFormChangeWorkCell switchWorkstationForm = new XPFormChangeWorkCell(this.AProcessType);

        ////设置弹出窗口的已有值
        switchWorkstationForm.m_resourceInfo = this.m_resourceInfo;
        switchWorkstationForm.m_processInfo = this.m_processInfo;
        switchWorkstationForm.m_stationInfo = this.m_stationInfo;
        var result = await switchWorkstationForm.ShowDialog<bool>(this.GetWindow());
        if (result)
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
    /// <param name="resourceInfo"></param>
    /// <param name="processInfo"></param>
    /// <param name="stationInfo"></param>
    public void ShowInfo(ResourceInfo resourceInfo, ProcessInfo processInfo, StationInfo stationInfo)
    {
        this.lbResouce.Text = resourceInfo == null ? string.Empty : resourceInfo.Name;
        this.lbProcess.Text = processInfo == null ? string.Empty : processInfo.Name;
        this.lbStation.Text = stationInfo == null ? string.Empty : stationInfo.Name;
    }
    #endregion


    private void UserControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (this.AType == EnumXPTitleType.WorkerCell)
        {
            this.LoadWorkerCellFromLocalSetting();
            this.ResetSetWorkCellLabeText();
        }
    }

    /// <summary>
    /// 工作单元详细信息显示
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Grid_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var ctl = sender as Control;
        if (ctl != null)
        {
            ShowInfo(this.m_resourceInfo, this.m_processInfo, this.m_stationInfo);
            FlyoutBase.ShowAttachedFlyout(ctl);
        }
    }
    #endregion
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