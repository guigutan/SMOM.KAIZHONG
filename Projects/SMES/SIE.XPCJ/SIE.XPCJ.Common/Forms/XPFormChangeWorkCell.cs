using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Exceptions;
using SIE.XPCJ.Common.Extensions;
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

namespace SIE.XPCJ.Common.Forms
{
    public partial class XPFormChangeWorkCell : XPFormBaseDialog
    {
        /// <summary>
        /// 选中的工序信息
        /// </summary>
        public ProcessInfo m_processInfo;

        /// <summary>
        /// 选中的工位信息
        /// </summary>
        public StationInfo m_stationInfo;

        /// <summary>
        ///选中的资源信息
        /// </summary>

        public ResourceInfo m_resourceInfo;

        /// <summary>
        /// 是否加载中
        /// </summary>
        private bool isLoaded = false;
        /// <summary>
        /// 选择后的值格式化
        /// </summary>
        private string formateValues = "{0}-{1}-{2}";

        /// <summary>
        /// 工序类型
        /// </summary>

        private ProcessType m_processType = ProcessType.Assembly;
        public XPFormChangeWorkCell(ProcessType processType)
        {
            InitializeComponent();
            m_processType = processType;
        }


        /// <summary>
        /// 获取资源列表
        /// </summary>
        /// <returns></returns>
        public static ResourceDataInfo GetResourceDataInfos(ResourceQueryInfo queryInfo)
        {
            object[] parameters = new object[1];
            parameters[0] = queryInfo;
            var result = ApiHelper.Post<ResourceDataInfo>("WinFormMoveApiController", "GetResourceInfos", parameters);
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

        public static ProcessDataInfo GetProcessDataInfos(ProcessQueryInfo queryInfo)
        {
            object[] parameters = new object[1];
            parameters[0] = queryInfo;
            var result = ApiHelper.Post<ProcessDataInfo>("WinFormMoveApiController", "GetProcessDataInfos", parameters);
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

        public static StationDataInfo GetStationDataInfos(StationQueryInfo queryInfo)
        {
            object[] parameters = new object[1];
            parameters[0] = queryInfo;
            var result = ApiHelper.Post<StationDataInfo>("WinFormMoveApiController", "GetStationDataInfos", parameters);
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

        private void SwitchWorkstationForm_Load(object sender, EventArgs e)
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

            this.xpListBoxCtr1.DataSource = resourceList.ResourceInfos;
            if (resourceList.ResourceInfos.Any())
            {
                this.xpListBoxCtr1.SelectedIndex = -1;
            }
            this.xpListBoxCtr2.DataSource = processList.ProcessInfos;
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
            if (this.xpListBoxCtr3.DataSource != null && m_stationInfo != null)
            {
                var stationList = this.xpListBoxCtr3.DataSource as List<StationInfo>;
                var stationIndex = stationList.FindIndex(m => m.Id == this.m_stationInfo.Id);
                this.xpListBoxCtr3.SelectedIndex = stationIndex;
            }
            isLoaded = false;
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void xpListBoxCtr1_SelectedIndexChanged(object sender, EventArgs e)
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
                this.xpListBoxCtr3.DataSource = stationList.StationInfos;
            }
            else
            {
                this.xpListBoxCtr3.DataSource = null;
            }
           
            if (!isLoaded)
            {
                this.m_resourceInfo = this.xpListBoxCtr1.SelectedItem != null ? (ResourceInfo)this.xpListBoxCtr1.SelectedItem : null;
                this.m_processInfo = this.xpListBoxCtr2.SelectedItem != null ? (ProcessInfo)this.xpListBoxCtr2.SelectedItem : null;
                this.m_stationInfo = this.xpListBoxCtr3.SelectedItem != null ? (StationInfo)this.xpListBoxCtr3.SelectedItem : null;
            }
            this.label1.Text = string.Format(this.formateValues, m_resourceInfo?.Name, this.m_processInfo?.Name,
               this.m_stationInfo?.Name);
        }

        private void button2_Click(object sender, EventArgs e)
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
                MessageBox.Show(stringBuilder.ToString().TrimEnd(';'));
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void xpListBoxCtr3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded)
            {
                this.m_resourceInfo = this.xpListBoxCtr1.SelectedItem != null ? (ResourceInfo)this.xpListBoxCtr1.SelectedItem : null;
                this.m_processInfo = this.xpListBoxCtr2.SelectedItem != null ? (ProcessInfo)this.xpListBoxCtr2.SelectedItem : null;
                this.m_stationInfo = this.xpListBoxCtr3.SelectedItem != null ? (StationInfo)this.xpListBoxCtr3.SelectedItem : null;
                this.label1.Text = string.Format(this.formateValues, m_resourceInfo?.Name, this.m_processInfo?.Name,
              this.m_stationInfo?.Name);
            }
        }
    }
}
