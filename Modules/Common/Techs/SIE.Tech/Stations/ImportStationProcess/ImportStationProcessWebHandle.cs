using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Tech.Stations.ImportStationProcess
{
    /// <summary>
    /// 导入工位工序 处理逻辑类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportStationProcessWebHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportStationProcessWebHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名列表
        /// </summary>
        private List<string> columnNameList = new List<string> { "工序编码", "工步编码" };

        #region 私有属性
        /// <summary>
        /// 工序
        /// </summary>
        private readonly Dictionary<string, double> _processDict = new Dictionary<string, double>();

        /// <summary>
        /// 工步
        /// </summary>
        private readonly Dictionary<string, double> _workStepDict = new Dictionary<string, double>();

        #endregion

        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList
        {
            get
            {
                return columnNameList;
            }

            set
            {
                columnNameList = value;
            }
        }

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建列
        /// </summary>
        /// <returns>IBusinessImport</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>
            {
                { "工序编码", new ValidColumn(ImportDataType._String, true, ValidateProcess) },
                { "工步编码", new ValidColumn(ImportDataType._String, false, ValidateWorkStep) }
            };

            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            _processDict.Clear();
            _workStepDict.Clear();
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Length == 0) return;
            var ctl = RT.Service.Resolve<StationController>();
            var stationIdstr = drs[0][ImportDataHandle.ParentId].ToString().Trim();
            if (string.IsNullOrEmpty(stationIdstr)) return;
            var stationId = double.Parse(stationIdstr);
            foreach (var mainDataProcess in drs)
            {
                var processCode = mainDataProcess.Field<string>(ColIndex("工序编码"));
                var workStepCode = mainDataProcess.Field<string>(ColIndex("工步编码"));
                double processId = GetProcessCode(processCode);
                double workStepId = GetWorkStepCode(processCode + "," + workStepCode);

                bool ifExist = ctl.IsExistsStationProcess(processId, stationId);
                if (ifExist)
                {
                    mainDataProcess[ImportDataHandle.MessageColumnName] = "工位已有该工序编号".L10N();
                    continue;
                }

                using (var tran = DB.TransactionScope(TechEntityDataProvider.ConnectionStringName))
                {
                    //如果不能新增记录错误信息
                    try
                    {
                        var StationProcess = new StationProcess();
                        StationProcess.ProcessId = processId;
                        StationProcess.StationId = stationId;
                        StationProcess.WorkStepId = workStepId;
                        RF.Save(StationProcess);
                    }
                    catch (Exception exc)
                    {
                        string strMsg = AppRuntime.Location.ConnectDataDirectly ? exc.Message : exc.InnerException.Message;
                        mainDataProcess[ImportDataHandle.MessageColumnName] = mainDataProcess[ImportDataHandle.MessageColumnName] + strMsg;
                        continue;
                    }

                    tran.Complete();
                }
            }
        }

        #region 引用类型的属性从字典中取值
        /// <summary>
        /// 根据工序编码取值
        /// </summary>
        /// <param name="key">工序编码</param>
        /// <returns>工序Id</returns>
        private double GetProcessCode(string key)
        {
            if (_processDict.ContainsKey(key))
                return _processDict[key];
            return 0;
        }

        /// <summary>
        /// 根据工步编码取值
        /// </summary>
        /// <param name="key">工步编码</param>
        /// <returns>工步Id</returns>
        private double GetWorkStepCode(string key)
        {
            if (_workStepDict.ContainsKey(key))
                return _workStepDict[key];
            return 0;
        }
        #endregion

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return columnNameList.IndexOf(columnName);
        }

        #region 基础验证
        /// <summary>
        /// 工序验证
        /// </summary>
        /// <param name="obj">结果值</param>
        /// <param name="messageTip">错误消息</param>
        /// <param name="row">当前行</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateProcess(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "工序编码"))
                return true;
            if (!_processDict.ContainsKey(value))
            {
                Process ps = RT.Service.Resolve<ProcessController>().GetProcessByCode(value);
                if (ps != null)
                {
                    _processDict.Add(value, ps.Id);
                }
                else
                {
                    messageTip = "[{0}]不存在".L10nFormat(value);
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 验证工步
        /// </summary>
        /// <param name="obj">工步编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateWorkStep(object obj, out string messageTip, DataRow row)
        {
            string value = obj.ToString().Trim();
            messageTip = string.Empty;
            var processCode = row.Field<string>(ColIndex("工序编码")).Trim();
            double processId = 0;
            if (!_processDict.TryGetValue(processCode, out processId))
            {
                messageTip = "没有对应的工序信息。".L10nFormat(processCode);
                return false;
            }

            string key = processCode + "," + value;
            if (_workStepDict.ContainsKey(key))
                return true;
            var lstWorkStep = RT.Service.Resolve<ProcessController>().GetWorkSteps(processId);
            if (lstWorkStep.Count > 0 && string.IsNullOrWhiteSpace(value))
            {
                messageTip = "[{0}]对应的工步信息存在，工步信息为必填项。".L10nFormat(processCode);
                return false;
            }
            if (lstWorkStep.Count == 0 && !string.IsNullOrWhiteSpace(value))
            {
                messageTip = "{0}工步信息不存在。".L10nFormat(value);
                return false;
            }

            var workStep = lstWorkStep.FirstOrDefault(p => p.Code == value);
            if (workStep == null)
            {
                messageTip = "工序[{0}]对应的工步[{1}]信息不存在。".L10nFormat(processCode, value);
                return false;
            }

            _workStepDict.Add(key, workStep.Id);

            return true;
        }

        /// <summary>
        /// 验证数据是否为空
        /// </summary>
        /// <param name="str">错误信息</param>
        /// <param name="value">值</param>
        /// <param name="colunmName">字段编码</param>
        /// <returns>是空返回true，否则返回false</returns>
        private bool ValidateIsNull(ref string str, string value, string colunmName)
        {
            if (!value.IsNullOrEmpty())
                return false;
            str = "{0}不能为空".L10nFormat(colunmName);
            return true;
        }
        #endregion      
    }
}
