using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Abnormal;
using SIE.MES.Workbench;
using SIE.MES.Workbench.AlertLights;
using SIE.Wpf.Common.Diagram;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.Workbench.AlertLights
{
    /// <summary>
    /// AlertLightsControl.xaml 的交互逻辑
    /// </summary>
    [Category("高效作业")]
    public partial class AlertLightsControl : ComponentItem
    {
        #region 字段定义
        /// <summary>
        /// 停线呼叫Img
        /// </summary>
        private const string StopImg = @"../Images/AndonStopLine.png";

        /// <summary>
        /// 停线呼叫恢复Img
        /// </summary>
        private const string StopRenewImg = @"../Images/AndonStopRenew.png";

        /// <summary>
        /// 异常呼叫Img
        /// </summary>
        private const string ExcepImg = @"../Images/AndonExcep.png";

        /// <summary>
        /// 异常呼叫恢复Img
        /// </summary>
        private const string ExcepRenewImg = @"../Images/AndonExcepRenew.png";

        /// <summary>
        /// 求助呼叫Img
        /// </summary>
        private const string HelpImg = @"../Images/AndonHelp.png";

        /// <summary>
        /// 求助呼叫恢复Img
        /// </summary>
        private const string HelpRenewImg = @"../Images/AndonHelpRenew.png";

        /// <summary>
        /// 异常恢复Content
        /// </summary>
        private const string ResumeContent = "恢复";

        /// <summary>
        /// 异常类型的返回类型编码
        /// </summary>
        private const string ExpTypeRtnCode = "ExpTypeRtn";

        /// <summary>
        /// 异常类型的返回类型名称
        /// </summary>
        private const string ExpTypeRtnName = "返回";

        /// <summary>
        /// 呼叫类型集合
        /// </summary>
        private List<AlertCallType> alertCallTypes;

        /// <summary>
        /// 呼叫类型Img字典
        /// </summary>
        private Dictionary<string, string> dicAlertCallTypeImgs;

        /// <summary>
        /// 呼叫类型恢复Img字典
        /// </summary>
        private Dictionary<string, string> dicAlertCallTypeRenewImgs;

        /// <summary>
        /// 呼叫类型标记
        /// </summary>
        private AlertCallType _curAlertCallType = AlertCallType.Stop;

        /// <summary>
        /// 呼叫类型信息集合
        /// </summary>
        private ObservableCollection<AlertCallTypeElement> callTypeElems;

        /// <summary>
        /// 异常类型信息集合
        /// </summary>
        private ObservableCollection<AlertExceptionTypeElement> expTypeElems;

        /// <summary>
        /// 工位Id
        /// </summary>
        private double? _stationId = null;

        /// <summary>
        /// 员工Id
        /// </summary>
        private double? _employeeId = null;

        /// <summary>
        /// 工单Id
        /// </summary>
        private double? _workOrderId = null;

        /// <summary>
        /// 产品Id
        /// </summary>
        private double? _productId = null;

        /// <summary>
        /// 组件通信的输入参数
        /// </summary>
        private AlertLightsInput _input;

        #endregion 字段定义-End

        /// <summary>
        /// 构造函数
        /// </summary>
        public AlertLightsControl()
        {
            InitializeComponent();

            AlertLightIni();
            _input = UseInput<AlertLightsInput>();
            UseProperty<AlertLightsControlProperty>();
            _input.PropertyChanged += AlertLightsInput_PropertyChanged;
        }

        /// <summary>
        /// 软安灯初始化
        /// </summary>
        private void AlertLightIni()
        {
            AlertCallTypeIni();
            CreateCallTypeBtn();
            CreateExceptionTypeBtn();
        }

        /// <summary>
        /// 安灯呼叫初始化
        /// </summary>
        private void AlertCallTypeIni()
        {
            alertCallTypes = new List<AlertCallType>() { AlertCallType.Stop, AlertCallType.Exception, AlertCallType.Help };
            dicAlertCallTypeImgs = new Dictionary<string, string>();
            dicAlertCallTypeImgs.Add(GetCallTypeCode(AlertCallType.Stop), StopImg);
            dicAlertCallTypeImgs.Add(GetCallTypeCode(AlertCallType.Exception), ExcepImg);
            dicAlertCallTypeImgs.Add(GetCallTypeCode(AlertCallType.Help), HelpImg);

            dicAlertCallTypeRenewImgs = new Dictionary<string, string>();
            dicAlertCallTypeRenewImgs.Add(GetCallTypeCode(AlertCallType.Stop), StopRenewImg);
            dicAlertCallTypeRenewImgs.Add(GetCallTypeCode(AlertCallType.Exception), ExcepRenewImg);
            dicAlertCallTypeRenewImgs.Add(GetCallTypeCode(AlertCallType.Help), HelpRenewImg);

            /*alertCallTypes.Add(AlertCallType.CallTypeTest30);
            alertCallTypes.Add(AlertCallType.CallTypeTest40);
            dicAlertCallTypeImgs.Add(GetCallTypeCode(AlertCallType.CallTypeTest30), HelpImg);
            dicAlertCallTypeImgs.Add(GetCallTypeCode(AlertCallType.CallTypeTest40), HelpImg);
            dicAlertCallTypeRenewImgs.Add(GetCallTypeCode(AlertCallType.CallTypeTest30), HelpRenewImg);
            dicAlertCallTypeRenewImgs.Add(GetCallTypeCode(AlertCallType.CallTypeTest40), HelpRenewImg);*/
        }

        /// <summary>
        /// 呼叫类型按钮创建
        /// </summary>
        private void CreateCallTypeBtn()
        {
            callTypeElems = new ObservableCollection<AlertCallTypeElement>();
            foreach (var curCallTypeItem in alertCallTypes)
            {
                var curCallTypeElem = new AlertCallTypeElement();
                curCallTypeElem.CallTypeId = (int)curCallTypeItem;
                curCallTypeElem.CallTypeCode = GetCallTypeCode(curCallTypeItem); ////((int)curCallTypeItem).ToString();
                curCallTypeElem.CallTypeName = curCallTypeItem.ToLabel();
                curCallTypeElem.CallTypeState = false;
                curCallTypeElem.CallTypeLblContent = curCallTypeItem.ToLabel();
                curCallTypeElem.CallTypeImgSrc = dicAlertCallTypeImgs[curCallTypeElem.CallTypeCode];

                callTypeElems.Add(curCallTypeElem);
            }

            ctlAlertType.ItemsSource = callTypeElems;
        }

        /// <summary>
        /// 呼叫异常类型按钮的数据源
        /// </summary>
        /// <param name="typeCode">快码组编码</param>
        /// <returns>对应的快码列表</returns>
        private EntityList<Catalog> GetExpTypeBtnSources(string typeCode)
        {
            var catalogs = RT.Service.Resolve<CatalogController>().GetCatalogList(typeCode);
            var rtnCatalog = new Catalog();
            rtnCatalog.Code = ExpTypeRtnCode;
            rtnCatalog.Name = ExpTypeRtnName.L10N();
            catalogs.Add(rtnCatalog);
            return catalogs;
        }

        /// <summary>
        /// 异常类型按钮创建
        /// </summary>
        private void CreateExceptionTypeBtn()
        {
            var catalogs = GetExpTypeBtnSources(AbnormalCause.AbnormalTypeCatalog);
            expTypeElems = new ObservableCollection<AlertExceptionTypeElement>();
            foreach (var curCatalog in catalogs)
            {
                var curExpTypeElem = new AlertExceptionTypeElement(curCatalog.Id, curCatalog.Code, curCatalog.Name);
                expTypeElems.Add(curExpTypeElem);
            }

            ctlExceptionType.ItemsSource = expTypeElems;
        }

        /// <summary>
        /// 获取呼叫类型编码(值)
        /// </summary>
        /// <param name="callType">呼叫类型</param>
        /// <returns>呼叫类型值</returns>
        private string GetCallTypeCode(AlertCallType callType)
        {
            var callTypeCode = ((int)callType).ToString();
            return callTypeCode;
        }

        /// <summary>
        /// 获取呼叫类型
        /// </summary>
        /// <param name="callTypeCode">呼叫类型编码</param>
        /// <returns>呼叫类型</returns>
        private AlertCallType GetCallType(string callTypeCode)
        {
            var callType = (AlertCallType)int.Parse(callTypeCode);
            return callType;
        }

        /// <summary>
        /// 组件通信的属性变更事件
        /// </summary>
        /// <param name="sender">属性变更发送对象</param>
        /// <param name="e">事件参数</param>
        private void AlertLightsInput_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            GetComponentData();
            AlertLightsIniLoaded();
        }

        /// <summary>
        /// OnRun方法
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            GetComponentData();
            AlertLightsIniLoaded();
        }

        /// <summary>
        /// 获取组件通信的数据--员工Id、工位Id
        /// </summary>
        private void GetComponentData()
        {
            _employeeId = _input.EmployeeId;
            _stationId = _input.StationId;
            _workOrderId = _input.WorkOrderId;
            _productId = _input.ProductId;
        }

        /// <summary>
        /// 初始化呼叫类型按钮
        /// </summary>
        private void AlertLightsIniLoaded()
        {
            if (_stationId > 0 && _employeeId > 0)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var processStatusTypes = new List<ProcessStatusType>() { ProcessStatusType.Waitting, ProcessStatusType.Processing };
                    var alertLights = RT.Service.Resolve<AlertLightsController>().GetAlertLights(_employeeId, _stationId, processStatusTypes);
                    LoadCallTypeBtnIni(alertLights);
                }));
            }
        }

        /// <summary>
        /// 设置呼叫类型按钮为"XX呼叫"状态
        /// </summary>
        /// <param name="callTypeElem">呼叫类型</param>
        private void SetCallTypeBtnToInitial(AlertCallTypeElement callTypeElem)
        {
            callTypeElem.CallTypeLblContent = callTypeElem.CallTypeName;
            callTypeElem.CallTypeImgSrc = dicAlertCallTypeImgs[callTypeElem.CallTypeCode];
            callTypeElem.CallTypeState = false;
        }

        /// <summary>
        /// 设置呼叫类型按钮为"恢复"状态
        /// </summary>
        /// <param name="callTypeElem">呼叫类型</param>
        private void SetCallTypeBtnToResume(AlertCallTypeElement callTypeElem)
        {
            callTypeElem.CallTypeLblContent = ResumeContent.L10N();
            callTypeElem.CallTypeImgSrc = dicAlertCallTypeRenewImgs[callTypeElem.CallTypeCode];
            callTypeElem.CallTypeState = true;
        }

        /// <summary>
        /// 组件UI加载时初始化呼叫类型按钮
        /// </summary>
        /// <param name="alertLights">安灯异常集合</param>
        private void LoadCallTypeBtnIni(EntityList<AlertLight> alertLights)
        {
            foreach (var curCallTypeElem in callTypeElems)
            {
                ////var enumCallType = (AlertCallType)int.Parse(curCallTypeElem.CallTypeCode);
                var enumCallType = GetCallType(curCallTypeElem.CallTypeCode);
                if (alertLights.Any(p => p.AlertType == enumCallType))
                {
                    SetCallTypeBtnToResume(curCallTypeElem);
                }
                else
                {
                    SetCallTypeBtnToInitial(curCallTypeElem);
                }
            }
        }

        /// <summary>
        /// 设置呼叫类型按钮为"恢复"状态
        /// </summary>
        /// <param name="alertType">呼叫类型</param>
        private void SetAlertLightBtnToResume(AlertCallType alertType)
        {
            ShowAlertType();
            var curCallTypeCode = ((int)alertType).ToString();
            AlertCallTypeElement callTypeElem = callTypeElems.FirstOrDefault(x => x.CallTypeCode == curCallTypeCode);
            SetCallTypeBtnToResume(callTypeElem);
        }

        /// <summary>
        /// 呼叫类型按钮的Click方法
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void CallTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_stationId > 0 && _employeeId > 0)
            {
                var curBtn = sender as Button;
                var curCallTypeElem = curBtn.Tag as AlertCallTypeElement;
                _curAlertCallType = GetCallType(curCallTypeElem.CallTypeCode);

                if (!curCallTypeElem.CallTypeState)
                {
                    ShowExceptionType();
                }
                else
                {
                    SetCallTypeBtnToInitial(curCallTypeElem);
                    RT.Service.Resolve<AlertLightsController>().AlertLightsResume(_employeeId, _stationId, _curAlertCallType, _workOrderId, _productId);
                }
            }
            else
            {
                throw new ValidationException("员工信息不能为空".L10N());
            }
        }

        /// <summary>
        /// 异常类型的按钮Click方法
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void ExceptionTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            var curBtn = sender as Button;
            var curExpTypeElem = curBtn.Tag as AlertExceptionTypeElement;

            if (curExpTypeElem.ExpTypeCode == ExpTypeRtnCode)
                ShowAlertType();
            else
            {
                var alertType = _curAlertCallType;  //curExpTypeElem.CallType;
                var excepTypeId = curExpTypeElem.ExpTypeId;
                RT.Service.Resolve<AlertLightsController>().AlertLightsCallSave(alertType, excepTypeId, _employeeId, _stationId, _workOrderId, _productId);
                SetAlertLightBtnToResume(alertType);
            }
        }

        /// <summary>
        /// 显示安灯异常类型组件, 隐藏安灯呼叫类型组件
        /// </summary>
        private void ShowExceptionType()
        {
            gdAlertType.Visibility = Visibility.Hidden;
            gdExceptionType.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 显示安灯呼叫类型组件, 隐藏安灯异常类型组件
        /// </summary>
        private void ShowAlertType()
        {
            gdAlertType.Visibility = Visibility.Visible;
            gdExceptionType.Visibility = Visibility.Hidden;
        }
    }

    /// <summary>
    /// 软安灯属性
    /// </summary>
    public class AlertLightsControlProperty : ComponentProperty<AlertLightsControl>
    {
    }
}