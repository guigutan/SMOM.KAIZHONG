using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SIE.CrossPlatform.Collect.Common.ApiCall;
using SIE.CrossPlatform.Collect.Common.Controls;
using SIE.CrossPlatform.Collect.Common.Exceptions;
using SIE.CrossPlatform.Collect.Common.Extensions;
using SIE.CrossPlatform.Collect.Models.Enums;
using SIE.CrossPlatform.Collect.Models.WIP;
using System.Text;

namespace SIE.CrossPlatform.Collect.Common.Forms;

public partial class XPFormChangeWorkCell : BaseDialog
{

    private  readonly string ControllerName = "WinFormMoveApiController";
    /// <summary>
    /// 选中的工序信息
    /// </summary>
    public ProcessInfo m_processInfo { get; set; }

    /// <summary>
    /// 选中的工位信息
    /// </summary>
    public StationInfo m_stationInfo { get; set; }

    /// <summary>
    ///选中的资源信息
    /// </summary>

    public ResourceInfo m_resourceInfo { get; set; }

    /// <summary>
    /// 是否加载中
    /// </summary>
    private bool isLoaded = false;
    /// <summary>
    /// 选择后的值格式化
    /// </summary>
    private readonly string formateValues = "{0}-{1}-{2}";

    /// <summary>
    /// 工序类型
    /// </summary>

    private ProcessType m_processType = ProcessType.Assembly;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="processType"></param>
    public XPFormChangeWorkCell(ProcessType processType)
    {
        InitializeComponent();
        m_processType = processType;
        this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        this.Loaded += ChangeInvOrg_Loaded;
        this.XPDialogTitle.ATitle = "切换工作单元".L10N();
        this.XPDialogTitle.AOkClick += XPDialogTitle_AOkClick;
        this.PointerPressed += XPFormChangeWorkCell_PointerPressed;
        this.xpListBoxCtr1.SelectionChanged += XpListBoxCtr1_SelectionChanged;
        this.xpListBoxCtr2.SelectionChanged += XpListBoxCtr1_SelectionChanged;
        this.xpListBoxCtr3.SelectionChanged += XpListBoxCtr3_SelectionChanged;
    }

    private void XpListBoxCtr1_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (this.xpListBoxCtr1.SelectedItem != null && this.xpListBoxCtr2.SelectedItem != null)
        {
            var resource = this.xpListBoxCtr1.SelectedItem as ResourceInfo;
            var process = this.xpListBoxCtr2.SelectedItem as ProcessInfo;
            var stationList = GetStationDataInfos(new StationQueryInfo()
            {
                EmployeeId = LoginInfo.Instance.EmployeeId,
                ProcessId = process.Id,
                ProcessType = (int)m_processType,
                ResourceId = resource.Id
            });
            this.xpListBoxCtr3.ItemsSource = stationList.StationInfos;
        }
        else
        {
            this.xpListBoxCtr3.ItemsSource = null;
        }
        if (!isLoaded)
        {
            this.m_resourceInfo = this.xpListBoxCtr1.SelectedItem != null ? (ResourceInfo)this.xpListBoxCtr1.SelectedItem : null;
            this.m_processInfo = this.xpListBoxCtr2.SelectedItem != null ? (ProcessInfo)this.xpListBoxCtr2.SelectedItem : null;
            this.m_stationInfo = this.xpListBoxCtr3.SelectedItem != null ? (StationInfo)this.xpListBoxCtr3.SelectedItem : null;
        }
        this.XPDialogTitle.ATitle= string.Format(this.formateValues, m_resourceInfo?.Name, this.m_processInfo?.Name,
            this.m_stationInfo?.Name);
        
    }

    private void XpListBoxCtr3_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!isLoaded)
        {
            this.m_resourceInfo = this.xpListBoxCtr1.SelectedItem != null ? (ResourceInfo)this.xpListBoxCtr1.SelectedItem : null;
            this.m_processInfo = this.xpListBoxCtr2.SelectedItem != null ? (ProcessInfo)this.xpListBoxCtr2.SelectedItem : null;
            this.m_stationInfo = this.xpListBoxCtr3.SelectedItem != null ? (StationInfo)this.xpListBoxCtr3.SelectedItem : null;
            this.XPDialogTitle.ATitle = string.Format(this.formateValues, m_resourceInfo?.Name, this.m_processInfo?.Name,
            this.m_stationInfo?.Name);
        }
    }

    /// <summary>
    /// 保留 系统预览必要
    /// </summary>
    public XPFormChangeWorkCell()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 确定按钮回调
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void XPDialogTitle_AOkClick(object? sender, EventArgs e)
    {

        bool hasError = false;
        StringBuilder stringBuilder = new StringBuilder();
        if (this.m_resourceInfo == null)
        {
            hasError = true;
            stringBuilder.Append("产线不能为空;".L10N());
        }

        if (this.m_processInfo == null)
        {
            hasError = true;
            stringBuilder.Append("工序不能为空;".L10N());
        }

        if (this.m_stationInfo == null)
        {
            hasError = true;
            stringBuilder.Append("工位不能为空;".L10N());
        }
        if (this.m_processInfo != null && !IsEmpHasProcessSkill(this.m_processInfo.Id, LoginInfo.Instance.EmployeeId))
        {
            hasError = true;
            stringBuilder.AppendFormat("员工[{0}]不具有工序[{1}]所要求的技能；".L10N()
                , LoginInfo.Instance.UserName, this.m_processInfo.Name);
        }
        if (hasError)
        {
            MessageBox.ShowMessage(stringBuilder.ToString().TrimEnd(';'));
            return;
        }

        Close(true);//关闭并返回值
    }

    private void XPFormChangeWorkCell_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (e.Pointer.Type == PointerType.Mouse)
        {
            this.BeginMoveDrag(e);
        }
    }

    private void ChangeInvOrg_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        isLoaded = true;
        var resourceList = GetResourceDataInfos(new Models.WIP.ResourceQueryInfo()
        {
            EmployeeId = LoginInfo.Instance.EmployeeId,
        });
        var processList = GetProcessDataInfos(new Models.WIP.ProcessQueryInfo()
        {
            EmployeeId = LoginInfo.Instance.EmployeeId,
            ProcessType = (int)m_processType,
        });
        this.xpListBoxCtr1.ItemsSource = resourceList.ResourceInfos;
        if (resourceList.ResourceInfos.Any())
        {
            this.xpListBoxCtr1.SelectedIndex = -1;
        }
        this.xpListBoxCtr2.ItemsSource = processList.ProcessInfos;
        if (processList.ProcessInfos.Any())
        {
            this.xpListBoxCtr2.SelectedIndex = -1;
        }
        if (m_resourceInfo != null)
        {
            var resourceIndex = resourceList.ResourceInfos.FindIndex(m => m.Id == m_resourceInfo.Id);
            this.xpListBoxCtr1.SelectedIndex = resourceIndex;
        }
        if (m_processInfo != null)
        {
            var processIndex = processList.ProcessInfos.FindIndex(m => m.Id == m_processInfo.Id);
            this.xpListBoxCtr2.SelectedIndex = processIndex;
        }
        if (this.xpListBoxCtr3.ItemsSource != null && m_stationInfo != null)
        {
            var stationList = this.xpListBoxCtr3.ItemsSource as List<StationInfo>;
            var stationIndex = stationList.FindIndex(m => m.Id == this.m_stationInfo.Id);
            this.xpListBoxCtr3.SelectedIndex = stationIndex;
        }
        isLoaded = false;

    }

    /// <summary>
    /// 获取资源列表
    /// </summary>
    /// <returns></returns>
    public  ResourceDataInfo GetResourceDataInfos(ResourceQueryInfo queryInfo)
    {
        object[] parameters = new object[1];
        parameters[0] = queryInfo;
        var result = ApiHelper.Post<ResourceDataInfo>(ControllerName, "GetResourceInfos", parameters);
        if (result.Success)
        {
            return result.Result;
        }
        else
        {
            throw new ValidationException(result.Message);
        }
    }


    /// <summary>
    /// 获取工序列表
    /// </summary>
    /// <param name="queryInfo"></param>
    /// <returns></returns>

    public  ProcessDataInfo GetProcessDataInfos(ProcessQueryInfo queryInfo)
    {
        object[] parameters = new object[1];
        parameters[0] = queryInfo;
        var result = ApiHelper.Post<ProcessDataInfo>(ControllerName, "GetProcessDataInfos", parameters);
        if (result.Success)
        {
            return result.Result;
        }
        else
        {
            throw new ValidationException(result.Message);
        }

    }

    /// <summary>
    /// 获取工位列表
    /// </summary>
    /// <param name="queryInfo"></param>
    /// <returns></returns>

    public  StationDataInfo GetStationDataInfos(StationQueryInfo queryInfo)
    {
        object[] parameters = new object[1];
        parameters[0] = queryInfo;
        var result = ApiHelper.Post<StationDataInfo>(ControllerName, "GetStationDataInfos", parameters);
        if (result.Success)
        {
            return result.Result;
        }
        else
        {
            throw new ValidationException(result.Message);
        }
    }

    /// <summary>
    /// 当前员工是否存在某工序的技能
    /// </summary>
    /// <param name="processId"></param>
    /// <param name="empId"></param>
    /// <returns></returns>
    public static bool IsEmpHasProcessSkill(double processId, double empId)
    {
        object[] parameters = new object[2];
        parameters[0] = processId;
        parameters[1] = empId;
        var result = ApiHelper.Post<bool>("WinFormMoveApiController", "IsEmpHasProcessSkill", parameters);
        if (result.Success)
        {
            return result.Result;
        }
        else
        {
            throw new ValidationException(result.Message);
        }
    }


}