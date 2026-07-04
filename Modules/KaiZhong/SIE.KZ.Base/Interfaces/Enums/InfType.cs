using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Enums
{
    /// <summary>
    /// 接口名
    /// </summary>
    public enum InfType
    {
        //基础数据枚举值0-100  业务接口  QMS枚举值101-200   WMS201-300 SCADA300-400 NC500-600

        #region 基础数据

        /// <summary>
        /// 参数项目
        /// </summary>
        //[Label("参数项目")]
        //CraftParameter = 0,

        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        Process = 1,

        /// <summary>
        /// 客户
        /// </summary>
        //[Label("客户")]
        //Customer = 2,

        /// <summary>
        /// 供应商
        /// </summary>
        //[Label("供应商")]
        //Supplier = 3,

        /// <summary>
        /// 供应商物料关系
        /// </summary>
        //[Label("供应商物料关系")]
        //SupplierItem = 4,

        /// <summary>
        /// 物料分类
        /// </summary>
        [Label("物料分类")]
        ItemCategory = 5,

        /// <summary>
        /// 工艺参数
        /// </summary>
        //[Label("工艺参数")]
        //ProductParameter = 6,

        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        Item = 7,

        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        WorkOrder = 8,

        /// <summary>
        /// 设备台账
        /// </summary>
        [Label("设备台账")]
        EquipAccount = 9,

        /// <summary>
        /// 备件库存
        /// </summary>
        //[Label("备件库存")]
        //StoreSummary = 10,

        /// <summary>
        /// 员工
        /// </summary>
        [Label("员工")]
        Employee = 11,

        /// <summary>
        /// 保养计划
        /// </summary>
        //[Label("保养计划")]
        //MaintainPlan = 12,

        /// <summary>
        /// 工位
        /// </summary>
        //[Label("工位")]
        //Station = 13,

        /// <summary>
        /// 产品项目
        /// </summary>
        //[Label("产品项目")]
        //ProductProject = 14,

        /// <summary>
        /// 技能失效删除
        /// </summary>
        //[Label("技能失效删除")]
        //DelteSkills = 15,

        /// <summary>
        /// 电子作业指导书
        /// </summary>
        //[Label("电子作业指导书")]
        //Esop = 16,

        /// <summary>
        /// 点检确认
        /// </summary>
        //[Label("点检确认")]
        //CheckConfirm = 17,

        /// <summary>
        /// 工序对应
        /// </summary>
        //[Label("同步二级工序")]
        //GradeProcessCompare = 18,

        /// <summary>
        /// 工序技能题库
        /// </summary>
        //[Label("工序技能题库")]
        //ProcessSkillQues = 19,

        //[Label("部门")]
        //Department = 20,

        //[Label("岗位")]
        //Position = 21,

        //[Label("供应商分类")]
        //SupplierCategory = 22,

        //[Label("客户分类")]
        //CustomerCategory = 23,

        //[Label("组织")]
        //Organization = 24,

        //[Label("仓库")]
        //Warehouse = 25,

        //[Label("主机厂")]
        //MainEnginePlant = 26,

        //[Label("失效模式")]
        //Defect = 27,

        //[Label("车灯项目")]
        //LampProject = 28,

        /// <summary>
        /// 检验标准
        /// </summary>
        //[Label("同步检验标准")]
        //InspectionStandard = 27,

        /// <summary>
        /// 人员组织架构
        /// </summary>
        [Label("人员组织架构")]
        OrgLevel = 28,

        #endregion 基础数据

        #region 业务接口

        #region QMS

        #region 来料 101-105

        ///// <summary>
        ///// 来料不良审核传飞书
        ///// </summary>
        //[Label("来料不良审核传飞书")]
        //IqcFailToLark = 101,

        ///// <summary>
        ///// 飞书来料不良审核回传QMS
        ///// </summary>
        //[Label("飞书来料不良审核回传QMS")]
        //LarkResToIqc = 102,

        ///// <summary>
        ///// WMS来料报检
        ///// </summary>
        //[Label("WMS来料报检")]
        //[Category("WMS")]
        //AddIqcBill = 103,

        ///// <summary>
        ///// 来料检测结果回传WMS
        ///// </summary>
        //[Label("来料检测结果回传WMS")]
        //[Category("WMS")]
        //IqcResToWms = 104,

        ///// <summary>
        ///// 来料不良审核结果回传WMS
        ///// </summary>
        //[Label("来料不良审核结果回传WMS")]
        //[Category("WMS")]
        //IqcFailResToWms = 105,

        ///// <summary>
        ///// 来料问题SRM
        ///// </summary>
        //[Label("来料问题SRM")]
        //IqcSRM = 401,

        ///// <summary>
        ///// 售后供应商问题SRM
        ///// </summary>
        //[Label("售后供应商问题SRM")]
        //AfterSaleSupplierSRM = 402,

        ///// <summary>
        ///// 来料供应商检验报告SRM
        ///// </summary>
        //[Label("来料供应商检验报告SRM")]
        //IqcSupplierReportSRM = 403,

        ///// <summary>
        ///// SRM回收QMS来料供应单
        ///// </summary>
        //[Label("SRM回收QMS来料供应单")]
        //RecSupplierReportSRM = 404,

        #endregion 来料 101-105

        #region 超期 106-110

        ///// <summary>
        ///// 超期不良审核传飞书
        ///// </summary>
        //[Label("超期不良审核传飞书")]
        //CqFailToLark = 106,

        ///// <summary>
        ///// 飞书超期不良审核回传QMS
        ///// </summary>
        //[Label("飞书超期不良审核回传QMS")]
        //LarkResToCq = 107,

        ///// <summary>
        ///// WMS超期报检
        ///// </summary>
        //[Label("WMS超期报检")]
        //[Category("WMS")]
        //AddRecheckInspBill = 108,

        ///// <summary>
        ///// 超期复检结果回传WMS
        ///// </summary>
        //[Label("超期复检结果回传WMS")]
        //[Category("WMS")]
        //RecheckresToWms = 109,

        ///// <summary>
        ///// 超期复检不良审核结果回传WMS
        ///// </summary>
        //[Label("超期复检不良审核结果回传WMS")]
        //[Category("WMS")]
        //RecheckFailresToWms = 110,

        #endregion 超期 106-110

        #region 出货 111-115

        ///// <summary>
        ///// 出货不良审核回传飞书
        ///// </summary>
        //[Label("出货不良审核回传飞书")]
        //OobInsFailToLark = 111,

        ///// <summary>
        ///// 飞书出货不良审核结果回传QMS
        ///// </summary>
        //[Label("飞书出货不良审核结果回传QMS")]
        //LarkResToOobIns = 112,

        ///// <summary>
        ///// WMS出货报检
        ///// </summary>
        //[Label("WMS出货报检")]
        //[Category("WMS")]
        //AddOobInspBill = 113,

        ///// <summary>
        ///// 出货检验结果传WMS
        ///// </summary>
        //[Label("出货检验结果传WMS")]
        //[Category("WMS")]
        //OobInspresToWms = 114,

        ///// <summary>
        ///// 出货检验不良审核结果回传WMS
        ///// </summary>
        //[Label("出货检验不良审核结果回传WMS")]
        //[Category("WMS")]
        //OobInspFailresToWms = 115,

        #endregion 出货 111-115

        #region 首检 116-117

        ///// <summary>
        ///// 首检不良审核回传飞书
        ///// </summary>
        //[Label("首检不良审核回传飞书")]
        //FirstFailToLark = 116,

        ///// <summary>
        ///// 飞书首检不良审核结果回传QMS
        ///// </summary>
        //[Label("飞书首检不良审核结果回传QMS")]
        //LarkResToFirst = 117,

        #endregion 首检 116-117

        #region 巡检 118-119

        ///// <summary>
        ///// 巡检不良审核回传飞书
        ///// </summary>
        //[Label("巡检不良审核回传飞书")]
        //PatrolFailToLark = 118,

        ///// <summary>
        ///// 飞书巡检不良审核结果回传QMS
        ///// </summary>
        //[Label("飞书巡检不良审核结果回传QMS")]
        //LarkResToPatrol = 119,

        #endregion 巡检 118-119

        #region 成品检测 120-121

        ///// <summary>
        ///// 成品检测不良审核回传飞书
        ///// </summary>
        //[Label("成品检测不良审核回传飞书")]
        //ShippingInspFailToLark = 120,

        ///// <summary>
        ///// 飞书成品检测不良审核结果回传QMS
        ///// </summary>
        //[Label("飞书成品检测不良审核结果回传QMS")]
        //LarkResToShippingInsp = 121,

        #endregion 成品检测 120-121

        #region 零公里 131-132

        ///// <summary>
        ///// 零公里退货单审核回传飞书
        ///// </summary>
        //[Label("零公里退货单审核回传飞书")]
        //ZeroBackItem = 131,

        ///// <summary>
        ///// 零公里退货推送质量经理飞书
        ///// </summary>
        //[Label("零公里退货推送质量经理飞书")]
        //ZeroBackItemToManager = 132,

        ///// <summary>
        ///// 零公里退货质量经理飞书超时提醒
        ///// </summary>
        //[Label("零公里退货质量经理飞书超时提醒")]
        //ZeroBackItemOverTimeToManager = 133,

        #endregion 零公里 131-132

        #region QMS二期 160-190

        ///// <summary>
        ///// 初流管理-质量工程师
        ///// </summary>
        //[Label("初流管理-质量工程师")]
        //EarlyFlowManageQuaeng = 160,

        ///// <summary>
        ///// 初流管理-通知质量工程师-当前节点
        ///// </summary>
        //[Label("初流管理-通知质量工程师-当前节点")]
        //EarlyFlowManageNoticeQuaeng = 161,

        ///// <summary>
        ///// 初流管理-待审批任务通知
        ///// </summary>
        //[Label("初流管理-待审批任务通知")]
        //EarlyFlowManageWaitApproval = 162,

        ///// <summary>
        ///// 质量目标-质量工程师
        ///// </summary>
        //[Label("质量目标-质量工程师")]
        //TargetDecomposeQuaeng = 163,

        ///// <summary>
        ///// 质量目标-质量经理
        ///// </summary>
        //[Label("质量目标-质量经理")]
        //TargetDecomposeManager = 164,

        ///// <summary>
        ///// 分层审核-计划调整
        ///// </summary>
        //[Label("分层审核-计划调整")]
        //LaueredAuditPlanAdjust = 165,

        ///// <summary>
        ///// 分层审核-不合格项目
        ///// </summary>
        //[Label("分层审核-不合格项目")]
        //LaueredAuditNgProject = 166,

        ///// <summary>
        ///// 分层审核-制定整改计划
        ///// </summary>
        //[Label("分层审核-制定整改计划")]
        //LaueredAuditFormulatePlan = 167,

        ///// <summary>
        ///// 分层审核-任务到期
        ///// </summary>
        //[Label("分层审核-任务到期")]
        //LaueredAuditDueTaskReminder = 168,

        ///// <summary>
        ///// 快反办理提醒
        ///// </summary>
        //[Label("快反办理提醒")]
        //QrqcDutyTodo = 169,

        ///// <summary>
        ///// 改善经验库待办提醒
        ///// </summary>
        //[Label("改善经验库待办提醒")]
        //DevExperienceAudit = 170,

        ///// <summary>
        ///// 计量器具定检资产责任人提醒
        ///// </summary>
        //[Label("计量器具定检资产责任人提醒")]
        //CalibrationAsset = 171,

        ///// <summary>
        ///// SPC预警
        ///// </summary>
        //[Label("SPC预警")]
        //SpcWarn = 172,

        ///// <summary>
        ///// 计量器具定检升级
        ///// </summary>
        //[Label("计量器具定检升级")]
        //CalibrationWarn = 173,

        ///// <summary>
        ///// 快反任务到期提醒
        ///// </summary>
        //[Label("快反任务到期提醒")]
        //QrqcTaskDue = 174,

        ///// <summary>
        ///// 供应商审核信息接口
        ///// </summary>
        //[Label("供应商审核信息接口")]
        //SupplierAudit = 175,

        ///// <summary>
        ///// FMEA接口
        ///// </summary>
        //[Label("FMEA接口")]
        //TcFmea = 176,

        ///// <summary>
        ///// 改善经验库推广
        ///// </summary>
        //[Label("改善经验库推广")]
        //DevExperiencePromote = 177,

        ///// <summary>
        ///// 产品审核计划审批
        ///// </summary>
        //[Label("产品审核计划审批")]
        //ProductAuditPlan = 178,

        ///// <summary>
        ///// 产品审核报告审批
        ///// </summary>
        //[Label("产品审核报告审批")]
        //ProductAuditBill = 179,

        ///// <summary>
        ///// 产品审核问题跟踪审批
        ///// </summary>
        //[Label("产品审核问题跟踪审批")]
        //ProductAuditProBill = 180,

        ///// <summary>
        ///// 公司级QRQC会议决策提醒
        ///// </summary>
        //[Label("公司级QRQC会议决策提醒")]
        //CorpQrqcMeeting = 181,

        ///// <summary>
        ///// 工厂级QRQC会议决策提醒
        ///// </summary>
        //[Label("工厂级QRQC会议决策提醒")]
        //QrqcMeeting = 182,

        ///// <summary>
        ///// 通用审核计划审核提醒
        ///// </summary>
        //[Label("通用审核计划审核提醒")]
        //CommonAuditPlan = 183,

        ///// <summary>
        ///// 体系审核计划驳回审核提醒
        ///// </summary>
        //[Label("体系审核计划驳回审核提醒")]
        //SystermAuditPlanReject = 184,

        ///// <summary>
        ///// 内部过程审核计划驳回审核提醒
        ///// </summary>
        //[Label("内部过程审核计划驳回审核提醒")]
        //InnerAuditPlanReject = 185,

        ///// <summary>
        ///// 通用审核确认驳回提醒
        ///// </summary>
        //[Label("通用审核确认驳回提醒")]
        //CommonAuditConfirmReject = 186,

        ///// <summary>
        ///// 通用问题跟踪提醒
        ///// </summary>
        //[Label("通用问题跟踪通过提醒")]
        //CommonProblemTrackPass = 187,

        ///// <summary>
        ///// 通用问题跟踪驳回提醒
        ///// </summary>
        //[Label("通用问题跟踪驳回提醒")]
        //CommonProblemTrackReject = 188,

        ///// <summary>
        ///// 通用驳回提醒
        ///// </summary>
        //[Label("通用驳回提醒")]
        //CommonReject = 189,

        ///// <summary>
        ///// 通用待办提醒
        ///// </summary>
        //[Label("通用待办提醒")]
        //CommonToDeal = 190,

        ///// <summary>
        ///// 质量门审核报告批准
        ///// </summary>
        //[Label("质量门审核报告批准提醒")]
        //QualituApproval = 191,

        ///// <summary>
        ///// 质量目标管理从NC取值
        ///// </summary>
        //[Label("质量目标管理从NC取值")]
        //TargetDecomposeGetValueFromNc = 192,

        ///// <summary>
        ///// 体系审核问题跟踪内审核通过提醒
        ///// </summary>
        //[Label("体系审核问题跟踪内审核通过提醒")]
        //SystemAuditProblemTrackInnerPass = 193,

        ///// <summary>
        ///// 质量门审核-整改证据上传提醒
        ///// </summary>
        //[Label("质量门审核-整改证据上传提醒")]
        //QualityApprovalUpload = 194,

        ///// <summary>
        ///// 内部过程审核-整改证据上传提醒
        ///// </summary>
        //[Label("内部过程审核-整改证据上传提醒")]
        //InnerApprovalUpload = 195,

        ///// <summary>
        ///// 质量门审核计划-驳回提醒
        ///// </summary>
        //[Label("质量门审核计划-驳回提醒")]
        //QualityApprovalPlanReject = 196,




        #endregion QMS二期 160-192

        #endregion QMS

        #region WMS

        #region 工单备料任务 201-204

        ///// <summary>
        ///// 工单备料任务回传WMS
        ///// </summary>
        //[Label("工单备料任务回传WMS")]
        //[Category("WMS")]
        //WorkFeedItemsToWms = 201,

        ///// <summary>
        ///// WMS更新MES工单备料明细当前库存数量
        ///// </summary>
        //[Label("WMS更新MES工单备料明细当前库存数量")]
        //WmsFeedItemToMes = 202,

        ///// <summary>
        ///// MES传递工单波次备料明细给WMS
        ///// </summary>
        //[Label("MES传递工单波次备料明细给WMS")]
        //MesFeedItemBcToWms = 203,

        ///// <summary>
        ///// WMS回传工单波次备料明细发料记录
        ///// </summary>
        //[Label("WMS回传工单波次备料明细发料记录")]
        //[Category("WMS")]
        //WmsFeedItemBcSendToMes = 204,

        ///// <summary>
        ///// 生产超领料备料任务回传WMS
        ///// </summary>
        //[Label("生产超领料备料任务回传WMS")]
        //[Category("WMS")]
        //ExceedCallItemItemsToWms = 205,

        #endregion 工单备料任务 201-204

        #region 工单退料 205-206

        ///// <summary>
        ///// 工单退料
        ///// </summary>
        //[Label("工单退料")]
        //[Category("WMS")]
        //WorkBackMatToWms = 2051,

        ///// 其它入库
        ///// </summary>
        //[Label("其它入库")]
        //[Category("WMS")]
        //MatReturnBillToWms = 2052,

        ///// <summary>
        ///// WMS更新MES生产退料明细数据
        ///// </summary>
        //[Label("WMS更新MES生产退料明细数据")]
        //[Category("WMS")]
        //WmsWorkBackMatToMes = 206,

        #endregion 工单退料 205-206

        #region 销售退货单 207

        ///// <summary>
        ///// 工单退料
        ///// </summary>
        //[Label("销售退货单")]
        //AddSaleZeroBack = 207,

        #endregion 销售退货单 207

        ///// <summary>
        ///// WMS回传发料信息
        ///// </summary>
        //[Label("WMS回传发料信息")]
        //[Category("WMS")]
        //WmsSendMatToMes = 208,

        ///// <summary>
        ///// 挪料回传WMS
        ///// </summary>
        //[Label("挪料回传WMS")]
        //[Category("WMS")]
        //MoveMatToWms = 209,

        ///// <summary>
        ///// WMS更新MES挪料数据
        ///// </summary>
        //[Label("WMS更新MES挪料数据")]
        //[Category("WMS")]
        //WmsMoveMatToMes = 210,

        ///// <summary>
        ///// 成品入库回传WMS
        ///// </summary>
        //[Label("成品入库回传WMS")]
        //[Category("WMS")]
        //InStorageToWms = 211,

        ///// <summary>
        ///// WMS更新MES成品入库明细数据
        ///// </summary>
        //[Label("WMS更新MES成品入库明细数据")]
        //[Category("WMS")]
        //WmsInStorageToMes = 212,

        ///// <summary>
        ///// 查询/锁定WMS库存
        ///// </summary>
        //[Label("查询/锁定WMS库存")]
        //[Category("WMS")]
        //QueryStoreToWms = 213,

        ///// <summary>
        ///// MES完成上料回传WMS
        ///// </summary>
        //[Label("MES完成上料回传WMS")]
        //[Category("WMS")]
        //MesFinishLoadBackToWms = 214,

        ///// <summary>
        ///// 工单叫料
        ///// </summary>
        //[Label("工单叫料")]
        //[Category("WMS")]
        //CallMeterialToWms = 220,

        ///// <summary>
        ///// 备料取消
        ///// </summary>
        //[Label("备料取消")]
        //[Category("WMS")]
        //FeedCancelToWms = 221,

        ///// 生产超领料叫料
        ///// </summary>
        //[Label("生产超领料叫料")]
        //[Category("WMS")]
        //ExceedCallItemToWms = 222,

        ///// 原料补料管理叫料
        ///// </summary>
        //[Label("原料补料管理叫料")]
        //[Category("WMS")]
        //RawCallItemToWms = 223,

        ///// WMS回传原料补料管理备料信息
        ///// </summary>
        //[Label("WMS回传原料补料管理备料信息")]
        //[Category("WMS")]
        //WmsMaterialReplenishBcSendToMes = 224,

        #endregion WMS

        #region SCADA

        ///// <summary>
        ///// 工单工艺参数下发给SCADA
        ///// </summary>
        //[Category("SCADA")]
        //[Label("工单工艺参数下发")]
        //DownloadWoParameter = 301,

        ///// <summary>
        ///// SCADA设备扫码
        ///// </summary>
        //[Category("SCADA")]
        //[Label("SCADA设备扫码")]
        //EapScanBarcode = 302,

        ///// <summary>
        ///// 设备数采
        ///// </summary>
        //[Category("SCADA")]
        //[Label("设备数采")]
        //EapSaveData = 303,

        ///// <summary>
        ///// 获取设备工艺参数
        ///// </summary>
        //[Category("SCADA")]
        //[Label("获取设备工艺参数")]
        //ReadEquipmentData = 304,

        ///// <summary>
        ///// 打开设备使能位
        ///// </summary>
        //[Category("SCADA")]
        //[Label("打开设备使能位")]
        //DownloadSnToSmdc = 305,

        ///// <summary>
        ///// 下发SN给Scada
        ///// </summary>
        //[Category("SCADA")]
        //[Label("下发SN")]
        //DownloadSnToScada = 306,

        ///// <summary>
        ///// SCADA设备状态
        ///// </summary>
        //[Category("SCADA")]
        //[Label("SCADA设备状态")]
        //ScadaEquipState = 307,

        ///// <summary>
        ///// 设备异常返回
        ///// </summary>
        //[Category("SCADA")]
        //[Label("设备异常返回")]
        //ScadaEquipAlarm = 308,

        ///// <summary>
        ///// 注塑机工单下发
        ///// </summary>
        //[Category("SCADA")]
        //[Label("注塑机工单下发")]
        //DownScadaMolding = 309,

        ///// <summary>
        ///// 注塑机工单回传
        ///// </summary>
        //[Category("SCADA")]
        //[Label("注塑机工单回传")]
        //ScadaMolding = 310,

        ///// <summary>
        ///// 向SCADA捞取注塑机工单数据
        ///// </summary>
        //[Category("SCADA")]
        //[Label("捞取注塑机工单数据")]
        //GetScadaMolding = 311,

        ///// <summary>
        ///// 样灯加工设备回传参数
        ///// </summary>
        //[Category("SCADA")]
        //[Label("样灯加工设备回传参数")]
        //SampleLampEquData = 312,

        ///// <summary>
        ///// 自动线设备数采
        ///// </summary>
        //[Category("SCADA")]
        //[Label("自动线设备数采")]
        //EapSaveDataAutoLine = 313,

        ///// <summary>
        ///// 工单暂停
        ///// </summary>
        //[Category("SCADA")]
        //[Label("工单暂停")]
        //WoPause = 314,

        ///// <summary>
        ///// 强制过站
        ///// </summary>
        //[Category("SCADA")]
        //[Label("强制过站")]
        //ForceMoveOut = 315,

        ///// <summary>
        ///// 激光打码
        ///// </summary>
        //[Category("SCADA")]
        //[Label("激光打码")]
        //LaserMark = 316,

        ///// <summary>
        ///// 工单临时工艺参数下发给SCADA
        ///// </summary>
        //[Category("SCADA")]
        //[Label("工单临时工艺参数下发")]
        //DownloadTempWoParameter = 317,


        ///// <summary>
        ///// 自动线下发工单
        ///// </summary>
        //[Category("SCADA")]
        //[Label("自动线下发工单")]
        //AutoLineWo = 318,

        ///// <summary>
        ///// 自动线下发上料信息
        ///// </summary>
        //[Category("SCADA")]
        //[Label("自动线下发上料信息")]
        //AutoLineLoadItem = 319,

        ///// <summary>
        ///// 自动线下发维修上线工序
        ///// </summary>
        //[Category("SCADA")]
        //[Label("自动线下发维修上线工序")]
        //AutoLineFixUpline = 320,

        ///// <summary>
        ///// 自动线下发客户码
        ///// </summary>
        //[Category("SCADA")]
        //[Label("自动线下发客户码")]
        //AutoLineCustomerCode = 321,

        ///// <summary>
        ///// 自动线维修换料
        ///// </summary>
        //[Category("SCADA")]
        //[Label("自动线维修换料")]
        //AutoLineChangeItem = 322,

        ///// <summary>
        ///// 自动线设备数采
        ///// </summary>
        //[Category("SCADA")]
        //[Label("自动线设备数采")]
        //AutoLineMove = 323,

        ///// <summary>
        ///// 任务单暂停
        ///// </summary>
        //[Category("SCADA")]
        //[Label("任务单暂停")]
        //TaskPause = 324,

        ///// <summary>
        ///// 任务单生产中
        ///// </summary>
        //[Category("SCADA")]
        //[Label("任务单生产中")]
        //TaskMaking = 325,
        ///// <summary>
        ///// 任务单生产中
        ///// </summary>
        //[Category("SCADA")]
        //[Label("任务单已完成")]
        //TaskFinished = 326,

        ///// <summary>
        ///// 解绑报废
        ///// </summary>
        //[Category("SCADA")]
        //[Label("解绑报废")]
        //ScarapItem = 327,
        //#endregion SCADA

        #region NC

        ///// <summary>
        ///// 材料申请单发送给NC
        ///// </summary>
        //[Label("材料申请单发送给NC")]
        //EquipApplyToNc = 501,

        ///// <summary>
        ///// 条码报工
        ///// </summary>
        //[Label("条码报工")]
        //[Category("WMS")]
        //BarReport = 502,

        #endregion NC

        #region TC

        ///// <summary>
        ///// 临时工艺日志发送给TC
        ///// </summary>
        //[Label("临时工艺日志发送给TC")]
        //TemporarySendToTc = 700,
        ///// <summary>
        ///// 获取TC新项目问题点
        ///// </summary>
        //[Label("获取TC新项目问题点")]
        //TcNewProjectProblem = 7000,

        #endregion TC

        ///// <summary>
        ///// 材料退库单发送给NC
        ///// </summary>
        //[Label("材料退库单发送给NC")]
        //MaterialReturnBillToNc = 503,

        ///// <summary>
        ///// NC发送材料出库记录
        ///// </summary>
        //[Label("NC发送材料出库记录")]
        //NcEquipApplyToWms = 504,

        //[Label("8d奖惩信息发送给NC")]
        //EightImproveToNc = 505,

        //[Label("8d关单信息同步CRM")]
        //EightImproveToCrm = 506,
        //#region 飞书

        ///// <summary>
        ///// 异常报警后发送飞书
        ///// </summary>
        //[Label("异常报警后发送飞书")]
        //SendAlarmToLark = 601,

        #endregion 飞书

        #region 飞书

        ///// <summary>
        ///// 协同维修后发送飞书
        ///// </summary>
        //[Label("协同维修后发送飞书")]
        //EquipSendToLark = 602,

        ///// <summary>
        ///// 材料退库单发送给NC
        ///// </summary>
        //[Label("设备保养预警后发送飞书")]
        //MaintainSendToLark = 603,

        ///// <summary>
        ///// 材料申请审批发送给NC
        ///// </summary>
        //[Label("材料申请审批后发送飞书")]
        //ApplySendToLark = 604,

        ///// <summary>
        ///// 设备工艺报警信息发送飞书
        ///// </summary>
        //[Label("设备工艺报警信息发送飞书")]
        //ProcessSendToLark = 605,

        ///// <summary>
        ///// 临时工艺修改信息发送飞书
        ///// </summary>
        //[Label("临时工艺修改信息发送飞书")]
        //TempSendToLark = 606,

        ///// <summary>
        ///// 首检单据生成提醒发送飞书
        ///// </summary>
        //[Label("首检单据生成提醒发送飞书")]
        //FirstSendToLark = 607,

        ///// <summary>
        ///// 来料单据生成提醒发送飞书
        ///// </summary>
        //[Label("来料单据生成提醒发送飞书")]
        //IncomingSendToLark = 608,

        ///// <summary>
        ///// 巡检单据生成提醒发送飞书
        ///// </summary>
        //[Label("巡检单据生成提醒发送飞书")]
        //PatrolSendToLark = 609,

        ///// <summary>
        ///// 巡检单据超时提醒发送飞书
        ///// </summary>
        //[Label("巡检单据超时提醒发送飞书")]
        //PatrolOvertimeSendToLark = 610,

        ///// <summary>
        ///// 首检单据超时提醒发送飞书
        ///// </summary>
        //[Label("首检单据超时提醒发送飞书")]
        //FirstOvertimeSendToLark = 611,

        ///// <summary>
        ///// 来料单据超时提醒发送飞书
        ///// </summary>
        //[Label("来料单据超时提醒发送飞书")]
        //IncomingOvertimeSendToLark = 612,

        ///// <summary>
        ///// 自动派单提醒发送飞书
        ///// </summary>
        //[Label("自动派单提醒发送飞书")]
        //AutoAssinSendToLark = 613,

        ///// <summary>
        ///// 巡检任务提醒发送飞书
        ///// </summary>
        //[Label("巡检任务提醒发送飞书")]
        //PatrolInspTaskSendToLark = 614,

        ///// <summary>
        ///// 一次解析提醒发送飞书
        ///// </summary>
        //[Label("一次解析提醒发送飞书")]
        //OneTimeAnalysisSendToLark = 615,

        ///// <summary>
        ///// 巡检任务未到场提醒发送飞书
        ///// </summary>
        //[Label("巡检任务未到场提醒发送飞书")]
        //PatrolInspNotArrivedSendToLark = 616,

        ///// <summary>
        ///// 保养任务未到场提醒发送飞书
        ///// </summary>
        //[Label("保养任务未到场提醒发送飞书")]
        //MaintainNotArrivedSendToLark = 617,

        ///// <summary>
        ///// 8D报告提醒发送飞书
        ///// </summary>
        //[Label("8D报告提醒发送飞书")]
        //EightImproveBillSendToLark = 618,

        ///// <summary>
        ///// 首检不合格单据提醒发送飞书
        ///// </summary>
        //[Label("首检不合格单据提醒发送飞书")]
        //FirstFailSendToLark = 619,

        ///// <summary>
        ///// 巡检任务提醒-模具换模巡检
        ///// </summary>
        //[Label("巡检任务提醒-模具换模巡检")]
        //ModelChangeSendToLark = 620,

        ///// <summary>
        ///// 安灯报警-待确认提醒
        ///// </summary>
        //[Label("安灯报警-待确认提醒")]
        //ToBeConfirmedSendToLark = 621,

        ///// <summary>
        ///// 不合格审批（消息提醒）
        ///// </summary>
        //[Label("不合格审批（消息提醒）")]
        //FlowTaskRemind = 622,

        ///// <summary>
        ///// 工单缺料提醒
        ///// </summary>
        //[Label("工单缺料提醒")]
        //FeedItemMsg = 623,

        ///// <summary>
        ///// 8D改善报告超时提醒发送飞书
        ///// </summary>
        //[Label("8D改善报告超时提醒发送飞书")]
        //EightImproveOvertimeSendToLark = 624,

        ///// <summary>
        ///// 质量目标-责任人发送飞书
        ///// </summary>
        //[Label("质量目标-责任人发送飞书")]
        //QmsTargetResponsibler = 625,

        ///// <summary>
        ///// 质量目标-质量工程师发送飞书
        ///// </summary>
        //[Label("质量目标-质量工程师发送飞书")]
        //QmsTargetQualityEngineer = 626,

        ///// <summary>
        ///// 8D报告变更处理人推送飞书
        ///// </summary>
        //[Label("8D报告变更处理人推送飞书")]
        //EightImproveChangeHandler = 627,

        ///// <summary>
        ///// 零公里客诉反馈单抄送提醒发送飞书
        ///// </summary>
        //[Label("零公里客诉反馈单抄送提醒发送飞书")]
        //EightImproveIssueCc = 628,

        ///// <summary>
        ///// 检具失效提醒发送飞书
        ///// </summary>
        //[Label("检具失效提醒发送飞书")]
        //InspectToolInvalidityRemind = 629,

        ///// <summary>
        ///// 超期复检单生成提醒发送飞书
        ///// </summary>
        //[Label("超期复检单生成提醒发送飞书")]
        //RecheckInspCreateRemind = 630,

        ///// <summary>
        ///// 超期复检单超时提醒发送飞书
        ///// </summary>
        //[Label("超期复检单超时提醒发送飞书")]
        //RecheckInspOverTimeRemind = 631,

        ///// <summary>
        ///// 成品检验单生成提醒发送飞书
        ///// </summary>
        //[Label("成品检验单生成提醒发送飞书")]
        //ShippingInspCreateRemind = 632,

        ///// <summary>
        ///// 成品检验单超时提醒发送飞书
        ///// </summary>
        //[Label("成品检验单超时提醒发送飞书")]
        //ShippingInspOverTimeRemind = 633,

        ///// <summary>
        ///// 材料申请审批发送给NC
        ///// </summary>
        //[Label("材料申请审批失败提醒发送飞书")]
        //ApplyFailSendToLark = 634,

        ///// <summary>
        ///// BOP参数下提醒发送飞书
        ///// </summary>
        //[Label("BOP参数下提醒发送飞书")]
        //BopMsgSendToLark = 635,

        ///// <summary>
        ///// 材料审批完成通知库管发送飞书
        ///// </summary>
        //[Label("材料审批完成通知库管发送飞书")]
        //NotifyWarehouseManagerAfterApproval = 636,

        ///// <summary>
        ///// 自动推送工艺首检单据生成提醒
        ///// </summary>
        //[Label("自动推送工艺首检单据生成提醒")]
        //ProcessFirstInsp = 637,

        ///// <summary>
        ///// 自动推送组装应力前首检生成提醒
        ///// </summary>
        //[Label("自动推送组装应力前首检生成提醒")]
        //AssemblyStressFirstInsp = 638,

        ///// <summary>
        ///// 自动推送应力前匹配首检生成提醒
        ///// </summary>
        //[Label("自动推送应力前匹配首检生成提醒")]
        //FirstInspectionBeforeStressMatching = 639,

        ///// <summary>
        ///// 自动推送应力前组装首检生成提醒
        ///// </summary>
        //[Label("自动推送应力前组装首检生成提醒")]
        //FirstInspectionOfPreStressAssembly = 640,

        ///// <summary>
        ///// 不良品补料发送飞书提醒
        ///// </summary>
        //[Label("不良品补料发送飞书提醒")]
        //DefectiveProductReplenishment = 641,

        ///// <summary>
        ///// 成品次品确认提醒发送飞书
        ///// </summary>
        //[Label("成品次品确认提醒发送飞书")]
        //ProductdefectiveJudgeSendToLark = 642,

        ///// <summary>
        ///// 原料次品确认提醒发送飞书
        ///// </summary>
        //[Label("原料次品确认提醒发送飞书")]
        //MatdefectiveJudgeSendToLark = 643,

        ///// <summary>
        ///// 零部件次品确认提醒发送飞书
        ///// </summary>
        //[Label("零部件次品确认提醒发送飞书")]
        //ProductdefectiveJudgeZsSendToLark = 644,

        ///// <summary>
        ///// 临时性计划维修生成提醒发送飞书
        ///// </summary>
        //[Label("临时性计划维修生成提醒发送飞书")]
        //TemporaryPlanRepairGeneratedRemindSendToLark = 645,

        ///// <summary>
        ///// 生成特种设备定检提醒飞书推送
        ///// </summary>
        //[Label("生成特种设备定检提醒飞书推送")]
        //ProcessRegularInspection = 646,

        ///// <summary>
        ///// 生成特种设备保养提醒飞书推送
        ///// </summary>
        //[Label("生成特种设备定检提醒飞书推送")]
        //ProcessSpecialEquipmentMaintenance = 647,

        ///// <summary>
        ///// 飞书审批材料申请单
        ///// </summary>
        //[Label("飞书审批材料申请单")]
        //LarkToEquipApply = 648,

        ///// <summary>
        ///// 飞书执行补料确认审批操作
        ///// </summary>
        //[Label("飞书执行补料确认审批操作")]
        //LarkProcessMaterialReplenishs = 649,

        ///// <summary>
        ///// 产品超时报警发送飞书
        ///// </summary>
        //[Label("产品超时报警发送飞书")]
        //ResProExpireTimeAlarmSendLark = 650,

        ///// <summary>
        ///// 不良品收取提醒
        ///// </summary>
        //[Label("不良品收取提醒")]
        //BadReportReceivingReminder = 651,

        ///// <summary>
        ///// 飞书回调
        ///// </summary>
        //[Label("飞书回调")]
        //LarkCallback = 652,

        ///// <summary>
        ///// SCADA超时提醒
        ///// </summary>
        //[Label("SCADA超时提醒")]
        //ScadaExpireTimeAlarm = 653,

        ///// <summary>
        ///// 异常归属确认
        ///// </summary>
        //[Label("异常归属确认")]
        //SetAbnormalAttribution = 654,

        ///// <summary>
        ///// 备件监控剩余时间发送飞书
        ///// </summary>
        //[Label("备件监控剩余时间发送飞书")]
        //SparePartsMonitorSendLark = 655,

        ///// <summary>
        ///// 调度日志异常发送飞书
        ///// </summary>
        //[Label("调度日志异常发送飞书")]
        //SendLogExceptionToLark = 656,

        ///// <summary>
        ///// 调度日志异常发送飞书
        ///// </summary>
        //[Label("设备状态改变发送飞书")]
        //SendEquipStateChanged = 657,

        ///// <summary>
        ///// 不合格审核同步飞书审批
        ///// </summary>
        //[Label("不合格审核同步飞书审批")]
        //CreateThirdApproval = 659,

        ///// <summary>
        ///// 发送同步审批消息提示
        ///// </summary>
        //[Label("发送同步审批消息提示")]
        //CreateThirdApprovalMessage = 660,

        ///// <summary>
        ///// 维修单据发送计划员发送飞书
        ///// </summary>
        //[Label("维修单据发送计划员发送飞书")]
        //SendProductionPlannerToLark = 658,

        ///// <summary>
        ///// 安灯单据发送计划员发送飞书
        ///// </summary>
        //[Label("安灯单据发送计划员发送飞书")]
        //SendAlarmPlannerToLark = 659,

        ///// <summary>
        ///// 8D报告驳回推送飞书
        ///// </summary>
        //[Label("8D报告驳回推送飞书")]
        //EightImproveRejectedSendToLark = 661,

        ///// <summary>
        ///// AI设备卫士报警推送飞书
        ///// </summary>
        //[Label("AI设备卫士报警推送飞书")]
        //AIALARMPUSH = 662,

        ///// <summary>
        ///// 异常报警发送飞书
        ///// </summary>
        //[Label("异常报警发送飞书")]
        //AbnormalAlarmSendToLark = 663,

        ///// <summary>
        ///// 产线产量预警发送飞书
        ///// </summary>
        //[Label("产线产量预警发送飞书")]
        //SendWaringLineToLark = 664,

        ///// <summary>
        ///// 供应商计划审核到期提醒
        ///// </summary>
        //[Label("供应商计划审核到期提醒")]
        //SendWaringAuditPlanToLark = 665,


        ///// <summary>
        ///// 改进计划供应商未回复飞书消息提醒
        ///// </summary>
        //[Label("改进计划供应商未回复飞书消息提醒")]
        //SupplierImprovementPlanWariningToLark = 666,

        ///// <summary>
        ///// 整改佐证供应商未完成飞书消息提醒
        ///// </summary>
        //[Label("整改佐证供应商未完成飞书消息提醒")]
        //SuppliersCorrectiveEvidenceWarningToLark = 667,

        ///// <summary>
        ///// QA设备采集NG推送飞书
        ///// </summary>
        //[Label("QA设备采集NG推送飞书")]
        //QaEquipmentAlarmPushToLark = 668,


        ///// <summary>
        /////生产进度提醒发送飞书
        ///// </summary>
        //[Label("生产进度提醒发送飞书")]
        //SendProductWarningDataToLark = 669,


        ///// <summary>
        /////报工信息推送物流人员
        ///// </summary>
        //[Label("报工信息推送物流人员")]
        //ReportInfoSendLogisticiansToLark = 670,

        //[Label("补料审批推送-班长审批发送飞书")]
        //ReplenishmentLeaderApproval = 671,

        //[Label("补料审批推送-抄送主任发送飞书")]
        //ReplenishmentCcToSupervisor = 672,

        //[Label("补料审批推送-驳回通知发送飞书")]
        //ReplenishmentRejectionNotice = 673,


        #endregion 飞书

        //[Label("检验标准同步SRM")]
        //SynQmsIqcInsp = 701,

        //[Label("OCR")]
        //OCR = 702,

        ///// <summary>
        ///// 原料补料管理备料任务回传WMS
        ///// </summary>
        //[Label("原料补料管理备料任务回传WMS")]
        //[Category("WMS")]
        //MaterialReplenishItemsToWms = 703,

        ///// <summary>
        ///// 补料超时发送飞书提醒
        ///// </summary>
        //[Label("仓库补料超时发送飞书提醒")]
        //MaterialReplenishOverTimeToLark = 704,

        ///// <summary>
        ///// 星宇自研OCR
        ///// </summary>
        //[Label("星宇自研OCR")]
        //XingYuOCR = 705,

        //[Label("数据埋点")]
        //DataTrack = 706,

        //[Label("计量器具审批发送飞书提醒")]
        //MeteringEquipmentSendLark = 707,

        //[Label("发送飞书多维表格")]
        //SendToLarkMultiTable = 708,

        //[Label("反馈AI分析信息")]
        //AiCallback = 709,

        ///// <summary>
        ///// QMS改善经验库数据同步到FMEA
        ///// </summary>
        //[Label("QMS数据同步FMEA")]
        //QMSToFMEA = 799,

        //[Label("产线工序回传关键件物料")]
        //BackKeyMaterials = 800,


        #region QMS与CMR相关接口

        ///// <summary>
        ///// 生成PDF-8D报告
        ///// </summary>
        //[Label("生成PDF-8D报告")]
        //GeneratePDFEightImproveBill = 801,

        ///// <summary>
        ///// 生成PDF-上传图片
        ///// </summary>
        //[Label("生成PDF-上传图片")]
        //GeneratePDFUploadFile = 802,

        ///// <summary>
        ///// 生成PDF-发送PDF报告
        ///// </summary>
        //[Label("CRM-发送PDF报告")]
        //SendPdfToCrm = 803,

        //[Label("CRM-发送D6策略")]
        //SendMeasureToCrm = 804,

        //[Label("CRM-三包数据同步工作质量台账")]
        //SyncWorkQualityLedger = 805,

        #endregion QMS与CMR相关接口

        #region SRM相关接口

        ///// <summary>
        ///// SRM-停线问题同步接口
        ///// </summary>
        //[Label("SRM-停线问题同步接口")]
        //StopLineProSRM = 810,

        #endregion SRM相关接口

        #endregion 业务接口

        #region QAD相关接口 （尼什专用）

        //[Label("上传报工到QAD")]
        //QAD_UploadProduct = 830,

        //[Label("上传组装工票号报工到QAD")]
        //QAD_UploadAssProduct = 831,

        ////[Label("QAD推送物料主档案到SMOM")] 使用 Item
        ////QAD_DownloadItem = 831,

        //[Label("QAD推送工单到SMOM")]
        //QAD_DownloadWO = 832,

        //[Label("叫料单生成提醒发送到飞书")]
        //CallMaterialSendToLark = 833,

        //[Label("工单叫料提醒发送到飞书")]
        //WorkOrderCallRemind = 834,


        //[Label("包装后报工到QAD")]
        //QAD_PackedUploadAssProduct = 835,

        #endregion QAD相关接口 （尼什专用）

        #region 子工厂传给总控数据 900-1000

        //[Label("子工厂OEE")]
        //[Category("Factory")]
        //FactoryOee = 900,

        //[Label("子工厂工单")]
        //[Category("Factory")]
        //FactoryWo = 901,

        //[Label("子工厂上下岗")]
        //[Category("Factory")]
        //FactoryUpdown = 902,

        //[Label("子工厂设备状态")]
        //[Category("Factory")]
        //FactoryEquipState = 903,

        //[Label("子工厂来料检验单")]
        //[Category("Factory")]
        //FactoryIqcBill = 904,

        //[Label("子工厂首检检验单")]
        //[Category("Factory")]
        //FactoryFirstBill = 905,

        //[Label("子工厂巡检检验单")]
        //[Category("Factory")]
        //FactoryPqcBill = 906,

        //[Label("子工厂生产通用报表")]
        //[Category("Factory")]
        //FactoryReport = 907,

        #endregion 子工厂传给总控数据 900-1000

        #region 外部（主机厂）接口 1001-1100

        ///// <summary>
        ///// 理想-同步产品关键过程数据
        ///// </summary>
        //[Label("理想-同步产品关键过程数据")]
        //SendLxKeyProcessData = 1001,

        ///// <summary>
        ///// EMS-获取token
        ///// </summary>
        //[Label("EMS-获取token")]
        //EmsGetToken = 1002,

        ///// <summary>
        ///// 主机厂-获取token
        ///// </summary>
        //[Label("获取主机厂接口token")]
        //GetMainToken = 1003,

        ///// <summary>
        ///// 赛力斯-发送过程质量检测数据
        ///// </summary>
        //[Label("赛力斯-发送过程质量检测数据")]
        //SendSeresQualityData = 1004,

        ///// <summary>
        ///// 吉利-发送过程质量检测数据
        ///// </summary>
        //[Label("吉利-发送过程质量检测数据")]
        //SendGeelyQualityData = 1005,

        ///// <summary>
        ///// EDO推送维修数据给AI设备卫士
        ///// </summary>
        //[Label("EDO推送维修数据给AI设备卫士")]
        //EDOToAIEquipmentGuard = 1006,

        ///// <summary>
        ///// AI设备卫士获取设备信息
        ///// </summary>
        //[Label("AI设备卫士获取设备信息")]
        //AIToEDOEquip = 1007,

        ///// <summary>
        ///// AI设备卫士推送设备预警信息
        ///// </summary>
        //[Label("AI设备卫士推送设备预警信息")]
        //AIToEDOEquipAlarm = 1008,

        ///// <summary>
        ///// 理想-同步来料检验数据
        ///// </summary>
        //[Label("理想-同步来料检验数据")]
        //SendLxMaterialIspectData = 1009,

        ///// <summary>
        ///// EMS-传输数据
        ///// </summary>
        //[Label("EMS-传输数据")]
        //EMSSendData = 1010,

        ///// <summary>
        ///// 奇瑞-传输过程检测数据
        ///// </summary>
        //[Label("奇瑞-传输过程检测数据")]
        //QRProceeData = 1011,

        ///// <summary>
        ///// 奇瑞-传输来料检验数据
        ///// </summary>
        //[Label("奇瑞-传输来料检验数据")]
        //QRMaterialData = 1012,

        ///// <summary>
        ///// 奇瑞-传输缺陷业务数据
        ///// </summary>
        //[Label("奇瑞-传输缺陷业务数据")]
        //QRDefectData = 1013,

        ///// <summary>
        ///// 奇瑞-产品一次合格率数据
        ///// </summary>
        //[Label("奇瑞-产品一次合格率数据")]
        //QRProFirstPassyieldData = 1014,

        ///// <summary>
        ///// 奇瑞-工位一次合格率数据
        ///// </summary>
        //[Label("奇瑞-工位一次合格率数据")]
        //QRStaFirstPassyieldData = 1015,

        ///// <summary>
        ///// E0206-数字孪生
        ///// </summary>
        //[Label("E0206-数字孪生")]
        //E0206DigitalTwinData = 1016,

        ///// <summary>
        ///// MOM-电检互联
        ///// </summary>
        //[Label("MQTT电检互联")]
        //MQTTEleInsInter = 1017,

        //[Label("理想-同步OEE数据")]
        //SendLxOEEData = 1018,

        ///// <summary>
        ///// 江淮-发送过程质量检测数据
        ///// </summary>
        //[Label("江淮-发送过程质量检测数据")]
        //SendJhQualityData = 1019,

        #endregion 外部（主机厂）接口 1001-1100

        /// <summary>
        /// 工作中心
        /// </summary>
        [Label("工作中心")]
        WorkCenter = 1020,

        /// <summary>
        /// 人员技能
        /// </summary>
        [Label("人员技能")]
        Skill = 1021,

        /// <summary>
        /// 物料标签
        /// </summary>
        [Label("物料标签")]
        ItemLabel = 1022,
        /// <summary>
        /// 蓝标标签
        /// </summary>
        [Label("蓝标标签")]
        BlueLabel = 1023,
        /// <summary>
        /// 报工结果
        /// </summary>
        [Label("报工结果")]
        ReportResult = 1024,

        /// <summary>
        /// 扣料
        /// </summary>
        [Label("扣料上传")]
        Deduction = 1025,

        /// <summary>
        /// 报工上传
        /// </summary>
        [Label("报工上传")]
        Report = 1026,

        /// <summary>
        /// IOT安灯写入
        /// </summary>
        [Label("IOT安灯写入")]
        IOTWrite = 1027,

        /// <summary>
        /// IOT数采指令下发
        /// </summary>
        [Label("IOT数采指令下发")]
        IOTWorkWrite = 1028,

        /// <summary>
        /// QV消息推送
        /// </summary>
        [Label("企业微信消息推送")]
        QvPush = 1029,


        /// <summary>
        /// 副产品上传
        /// </summary>
        [Label("副产品上传")]
        OutputProduct = 1030,

        /// <summary>
        /// MES标签推送老化系统
        /// </summary>
        [Label("MES标签推送老化系统")]
        BarcodeToSmdc = 1031,

        /// <summary>
        /// 企业模型
        /// </summary>
        [Label("企业模型")]
        Enterprise = 1032,

        /// <summary>
        /// 差异扣料
        /// </summary>
        [Label("差异扣料")]
        ScrapWeighing = 1033,

        /// <summary>
        /// 发货确认
        /// </summary>
        [Category("KZ")]
        [Label("发货确认")]
        OutboundConfirm = 1034,

        /// <summary>
        /// OA流程退回
        /// </summary>
        [Category("KZ")]
        [Label("OA流程退回")]
        OAFlowReturn = 1035,

        /// <summary>
        /// IOT数采指令下发(共模)
        /// </summary>
        [Label("IOT数采指令下发(共模)")]
        IOTPunchWorkWrite = 1036,

        /// <summary>
        /// 跨库存组织物料标签同步
        /// </summary>
        [Label("跨库存组织物料标签同步")]
        SyncItemLabel = 1037,
        /// <summary>
        /// 蓝标装箱推送SAP
        /// </summary>
        [Label("蓝标装箱推送SAP")]
        BlueLabelToSap = 1038,

        /// <summary>
        /// 可疑品阈值
        /// </summary>
        [Label("可疑品阈值")]
        Threshold = 1039,

        /// <summary>
        /// 产线区域维护
        /// </summary>
        [Label("产线区域维护")]
        AndonLine = 1040,

        /// <summary>
        /// 产品与产线的关系
        /// </summary>
        [Label("产品与产线的关系")]
        ProductLine = 1041,

        /// <summary>
        /// 工装维护
        /// </summary>
        [Label("工装维护")]
        FixtureUphold = 1042,

        /// <summary>
        /// 工装与产品的关系
        /// </summary>
        [Label("工装与产品的关系")]
        FixtureItem = 1043,

        /// <summary>
        /// 检具维护
        /// </summary>
        [Label("检具维护")]
        CheckerUphold = 1044,

        /// <summary>
        /// 检具与产品的关系
        /// </summary>
        [Label("检具与产品的关系")]
        CheckerItem = 1045,

        /// <summary>
        /// 模具与产品的关系
        /// </summary>
        [Label("模具与产品的关系")]
        EquipAccountItem = 1046,

        /// <summary>
        /// 委外出库同步工厂数据
        /// </summary>
        [Label("委外出库同步工厂数据")]
        Outsourcingouts = 1047,

        /// <summary>
        /// 委外入库同步工厂数据
        /// </summary>
        [Label("委外入库同步工厂数据")]
        OutsourcingIns = 1048,

        /// <summary>
        /// 委外报工同步工厂数据
        /// </summary>
        [Label("委外报工同步工厂数据")]
        OutsourcingReport = 1049,

        /// <summary>
        /// 委外报工同步可疑品标签数据
        /// </summary>
        [Label("委外报工同步可疑品标签数据")]
        OutsourcingSupWipBatch = 1050,

        /// <summary>
        /// 返工工艺路线版本
        /// </summary>
        [Label("返工工艺路线版本")]
        ReworkLayoutVersion= 1051,

        /// <summary>
        /// 返工信息
        /// </summary>
        [Label("返工信息")]
        ReworkInfoRecord = 1052,

        /// <summary>
        /// Iot推送设备状态
        /// </summary>
        [Label("IOT推送设备状态")]
        IotEquipStatus = 1053,

        /// <summary>
        /// 手动同步其他工厂
        /// </summary>
        [Label("手动同步其他工厂")]
        ManuallySynchronize = 1054,
    }
}
