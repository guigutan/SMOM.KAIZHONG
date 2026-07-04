using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Resources.ProcessSegments;
using SIE.Resources.ProcessTechTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Resources.ProcessTechs.ImportProcessTech
{
    /// <summary>
    /// 导入制程工艺 处理逻辑类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportProcessTechHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportProcessTechHandle : IDisposable, IBusinessImport
    {
        private const string TRANSFER_TIME = "转款时间(秒)";
        private const string DEFAULT_PROCESS_TIME = "默认工艺定额(秒/单位)";
        private const string WORK_DURATION = "工作时长(时)";

        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        private List<string> columnNameList = new List<string>
        {
            "制程编号", "制程名称", "制程类型","工段","是否瓶颈", TRANSFER_TIME, DEFAULT_PROCESS_TIME, WORK_DURATION,"外协时长（天）"
        };

        #region 私有属性
        /// <summary>
        /// 制程工艺类型数据
        /// </summary>
        private List<ProcessTechType> ProcessTechTypeList = new List<ProcessTechType>();

        /// <summary>
        /// 工段数据
        /// </summary>
        private List<ProcessSegment> ProcessSegmentList = new List<ProcessSegment>();

        /// <summary>
        /// 瓶颈
        /// </summary>
        private readonly Dictionary<string, bool> DicBottleneck = new Dictionary<string, bool>();
        #endregion

        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList
        {
            get { return columnNameList; }
            set { columnNameList = value; }
        }

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建导入对象
        /// </summary>
        /// <returns>返回当前对象</returns>
        public IBusinessImport CreaetColumnValid()
        {
            DicBottleneck.Add("是", true);
            DicBottleneck.Add("否", false);
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("制程编号", new ValidColumn(ImportDataType._String, false, 80));
            this.ColumnValidList.Add("制程名称", new ValidColumn(ImportDataType._String, true, 80));
            this.ColumnValidList.Add("制程类型", new ValidColumn(ImportDataType._Enum, true, ValidProcessTechType));
            this.ColumnValidList.Add("工段", new ValidColumn(ImportDataType._String, false, ValidProcessSegment));
            this.ColumnValidList.Add("是否瓶颈", new ValidColumn(ImportDataType._String, false, ValidBottleneck));
            this.ColumnValidList.Add(TRANSFER_TIME, new ValidColumn(ImportDataType._PositiveDouble, false, ValidTransferTime));
            this.ColumnValidList.Add(DEFAULT_PROCESS_TIME, new ValidColumn(ImportDataType._PositiveDouble, false, ValidSAM));
            this.ColumnValidList.Add(WORK_DURATION, new ValidColumn(ImportDataType._Int, false, ValidWorkHour));
            this.ColumnValidList.Add("外协时长（天）", new ValidColumn(ImportDataType._Int, false, ValidWorkHour));

            return this;
        }

        #region 基础验证
        /// <summary>
        /// 验证默认工艺定额
        /// </summary>
        /// <param name="obj">默认工艺定额</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidSAM(object obj, out string messageTip, DataRow dr)
        {
            string sam = dr[ColIndex(DEFAULT_PROCESS_TIME)].ToString();
            bool isValid = true;
            messageTip = string.Empty;
            decimal result = 0;
            if (!decimal.TryParse(sam, out result))
            {
                messageTip = "输入格式不正确！".L10N();
                isValid = false;
            }
            else if (Convert.ToDecimal(sam) < 0)
            {
                messageTip = "不能为负数！".L10N();
                isValid = false;
            }
            else
            {
                // 
            }

            return isValid;
        }

        /// <summary>
        /// 验证转款时间
        /// </summary>
        /// <param name="obj">转款时间</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidTransferTime(object obj, out string messageTip, DataRow dr)
        {
            string transferTime = dr[ColIndex(TRANSFER_TIME)].ToString();
            bool isValid = true;
            messageTip = string.Empty;
            decimal result ;
            if (!decimal.TryParse(transferTime, out result))
            {
                messageTip = "输入格式不正确！".L10N();
                isValid = false;
            }
            else if (Convert.ToDecimal(transferTime) < 0)
            {
                messageTip = "不能为负数！".L10N();
                isValid = false;
            }
            else
            { 
                //
            }

            return isValid;
        }

        /// <summary>
        /// 验证工作时长
        /// </summary>
        /// <param name="obj">工作时长</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidWorkHour(object obj, out string messageTip, DataRow dr)
        {
            string workHour = dr[ColIndex(WORK_DURATION)].ToString();
            bool isValid = true;
            messageTip = string.Empty;
            if (string.IsNullOrEmpty(workHour))
            {
                return isValid;
            }

            decimal result;
            if (!decimal.TryParse(workHour, out result))
            {
                messageTip = "输入格式不正确！".L10N();
                isValid = false;
            }
            else if (Convert.ToDecimal(workHour) < 0)
            {
                messageTip = "不能为负数！".L10N();
                isValid = false;
            }
            else
            {
                //
            }

            return isValid;
        }

        /// <summary>
        /// 验证瓶颈
        /// </summary>
        /// <param name="obj">是否瓶颈</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidBottleneck(object obj, out string messageTip, DataRow dr)
        {
            string bottleneck = dr[ColIndex("是否瓶颈")].ToString();
            bool isValid = true;
            messageTip = string.Empty;
            if (string.IsNullOrEmpty(bottleneck))
            {
                return isValid;
            }

            if (!DicBottleneck.ContainsKey(bottleneck))
            {
                messageTip = "输入内容不正确！只能输入“是”或“否”".L10N();
                isValid = false;
            }

            return isValid;
        }

        /////// <summary>
        /////// 验证偏移时间
        /////// </summary>
        /////// <param name="obj">偏移时间</param>
        /////// <param name="messageTip">错误信息</param>
        /////// <param name="dr">验证数据当前行对象</param>
        /////// <returns>返回是否验证通过</returns>
        ////private bool ValidOffsetTime(object obj, out string messageTip, DataRow dr)
        ////{
        ////    string offsetTime = dr[ColIndex("偏移时间(秒)")].ToString();
        ////    bool isValid = true;
        ////    messageTip = string.Empty;
        ////    decimal result = 0;
        ////    if (!decimal.TryParse(offsetTime, out result))
        ////    {
        ////        messageTip = "输入格式不正确！".L10N();
        ////        isValid = false;
        ////    }
        ////    else if (Convert.ToDecimal(offsetTime) < 0)
        ////    {
        ////        messageTip = "不能为负数！".L10N();
        ////        isValid = false;
        ////    }

        ////    return isValid;
        ////}

        /////// <summary>
        /////// 验证是否排产
        /////// </summary>
        /////// <param name="obj">是否排产</param>
        /////// <param name="messageTip">错误信息</param>
        /////// <param name="dr">验证数据当前行对象</param>
        /////// <returns>返回是否验证通过</returns>
        ////private bool ValidIsScheduling(object obj, out string messageTip, DataRow dr)
        ////{
        ////    string isScheduling = dr[ColIndex("是否排产")].ToString();
        ////    string offsetTime = dr[ColIndex("偏移时间(秒)")].ToString();
        ////    string transferTime = dr[ColIndex("转款时间(秒)")].ToString();
        ////    bool isValid = true;
        ////    messageTip = string.Empty;

        ////    if (isScheduling == "否")
        ////    {
        ////        if (string.IsNullOrEmpty(offsetTime))
        ////        {
        ////            messageTip = "为否时，偏移时间必填！".L10N();
        ////            isValid = false;

        ////            return isValid;
        ////        }

        ////        if (!string.IsNullOrEmpty(transferTime))
        ////        {
        ////            messageTip = "为否时，转款时间不能导入！".L10N();
        ////            isValid = false;
        ////        }
        ////    }
        ////    else
        ////    {
        ////        if (!string.IsNullOrEmpty(offsetTime))
        ////        {
        ////            messageTip = "为是时，偏移时间不能导入！".L10N();
        ////            isValid = false;
        ////        }
        ////    }

        ////    return isValid;
        ////}

        /// <summary>
        /// 验证制程类型
        /// </summary>
        /// <param name="obj">制程类型</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidProcessTechType(object obj, out string messageTip, DataRow dr)
        {
            return ValidProcessTechDataHelper.ValidProcessTechType(ref ProcessTechTypeList, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证工段
        /// </summary>
        /// <param name="obj">工段</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidProcessSegment(object obj, out string messageTip, DataRow dr)
        {
            return ValidProcessTechDataHelper.ValidProcessSegment(ref ProcessSegmentList, obj.ToString(), out messageTip);
        }
        #endregion

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (ProcessTechTypeList != null)
            {
                ProcessTechTypeList.Clear();
                ProcessTechTypeList = null;
            }

            if (ProcessSegmentList != null)
            {
                ProcessSegmentList.Clear();
                ProcessSegmentList = null;
            }
        }

        /// <summary>
        /// 获取Excle数据赋值给实体并保存
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            var ctrl = RT.Service.Resolve<ProcessTechBaseController>();
            if (drs == null)
            {
                throw new ValidationException("需要处理的数据集不存在".L10N());
            }
            ///// 循环检验每一行主数据
            foreach (var dataItem in drs)
            {
                var proCode = dataItem.Field<string>(ColIndex("制程编号"));
                ProcessTech processTech = ctrl.GetProcessTechByproCode(proCode); //判断主数据是否存在
                using (var tran = DB.AutonomousTransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
                {
                    try
                    {
                        if (processTech == null)
                        {
                            processTech = InitProcessTech(dataItem);
                        }
                        else
                        {
                            throw new ValidationException("制程编号{0}重复".L10nFormat(processTech.Code));
                        }

                        RF.Save(processTech);
                    }
                    catch (Exception exc)
                    {
                        string strMsg = AppRuntime.Location.ConnectDataDirectly ? exc.Message : exc.InnerException.Message;
                        dataItem[ImportDataHandle.MessageColumnName] = dataItem[ImportDataHandle.MessageColumnName] + strMsg;
                        continue;
                    }

                    tran.Complete();
                }
            }
        }

        /// <summary>
        /// 制程工艺初始化
        /// </summary>
        /// <param name="dataItem">制程工艺数据行</param>
        /// <returns>制程工艺</returns>
        private ProcessTech InitProcessTech(DataRow dataItem)
        {
            var processTechNo = dataItem.Field<string>(ColIndex("制程编号"));
            var processTechName = dataItem.Field<string>(ColIndex("制程名称"));
            var processTechType = dataItem.Field<string>(ColIndex("制程类型"));
            var processSegment = dataItem.Field<string>(ColIndex("工段"));
            var isBottleneck = dataItem.Field<string>(ColIndex("是否瓶颈"));
            var transferTime = dataItem.Field<string>(ColIndex(TRANSFER_TIME));
            var SAM = dataItem.Field<string>(ColIndex(DEFAULT_PROCESS_TIME));
            var workHour = dataItem.Field<string>(ColIndex(WORK_DURATION));
            var outAssistDay = dataItem.Field<string>(ColIndex("外协时长（天）"));
            ProcessTechType ProcessTechType = GetProcessTechType(processTechType);

            var processTech = new ProcessTech();
            InitProcessTechNo(processTech, processTechNo); //设置制程编码
            processTech.Name = processTechName;
            processTech.WorkingHours = 1;
            processTech.OutAssistDay = null;
            processTech.ProcessTechType = ProcessTechType;
            processTech.ProcessTechTypeId = ProcessTechType.Id;
            processTech.IsScheduling = true;
            InitProcessTechTransferTime(processTech, transferTime);
            InitProcessTechSAM(processTech, SAM);
            if (!string.IsNullOrWhiteSpace(processSegment))
            {
                ProcessSegment ProcessSegment = GetProcessSegment(processSegment);
                processTech.ProcessSegment = ProcessSegment;
                processTech.ProcessSegmentId = ProcessSegment.Id;
            }
            if (!string.IsNullOrWhiteSpace(isBottleneck))
            {
                processTech.IsBottleneck = DicBottleneck[isBottleneck];
            }
            if (!string.IsNullOrWhiteSpace(workHour))
            {
                processTech.WorkingHours = decimal.Parse(workHour);
            }
            if (!string.IsNullOrWhiteSpace(outAssistDay))
            {
                processTech.OutAssistDay = decimal.Parse(outAssistDay);
            }

            return processTech;
        }

        /// <summary>
        /// 制程工艺初始化--设置默认工艺定额
        /// </summary>
        /// <param name="processTech">制程工艺</param>
        /// <param name="sam">默认工艺定额</param>
        private void InitProcessTechSAM(ProcessTech processTech, string sam)
        {
            if (!sam.IsNullOrEmpty())
            {
                processTech.SAM = decimal.Parse(sam);
            }
        }

        /// <summary>
        /// 制程工艺初始化--设置转款时间
        /// </summary>
        /// <param name="processTech">制程工艺</param>
        /// <param name="transferTime">转款时间</param>
        private void InitProcessTechTransferTime(ProcessTech processTech, string transferTime)
        {
            if (!transferTime.IsNullOrEmpty())
            {
                processTech.TransferTime = decimal.Parse(transferTime);
            }
        }

        /////// <summary>
        /////// 制程工艺初始化--设置偏移时间
        /////// </summary>
        /////// <param name="processTech">制程工艺</param>
        /////// <param name="offsetTime">偏移时间</param>
        ////private void InitProcessTechOffsetTime(ProcessTech processTech, string offsetTime)
        ////{
        ////    if (!offsetTime.IsNullOrEmpty())
        ////    {
        ////        processTech.OffsetTime = decimal.Parse(offsetTime);
        ////    }
        ////}

        /// <summary>
        ///  制程工艺初始化--设置制程编码
        /// </summary>
        /// <param name="processTech">制程工艺</param>
        /// <param name="processTechNo">制程编码</param>
        private void InitProcessTechNo(ProcessTech processTech, string processTechNo)
        {
            if (processTechNo.IsNullOrEmpty())
            {
                var ctl = RT.Service.Resolve<ProcessTechBaseController>();
                processTech.Code = ctl.GetProcessTechNo();
            }
            else
                processTech.Code = processTechNo;
        }

        #region 引用类型的属性从字典中取值
        /// <summary>
        /// 根据制程类型取值
        /// </summary>
        /// <param name="key">制程类型 string</param>
        /// <returns>制程类型</returns>
        private ProcessTechType GetProcessTechType(string key)
        {
            ProcessTechType result = ProcessTechTypeList.FirstOrDefault(p => p.Code == key || p.Name == key);
            return result;
        }

        /// <summary>
        /// 根据工段取值
        /// </summary>
        /// <param name="key">工段 string</param>
        /// <returns>工段</returns>
        private ProcessSegment GetProcessSegment(string key)
        {
            ProcessSegment result = ProcessSegmentList.FirstOrDefault(p => p.Code == key || p.Name == key);
            return result;
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
    }
}
