using SIE.Domain;
using SIE.EMS.Common.Utils;
using SIE.EMS.Enums;
using SIE.EMS.InspectionRules;
using SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.SpecialEquipment.RegularInspections.Handle
{
    /// <summary>
    /// 自动生产特种设备定检计划
    /// </summary>
    [Services.Service(FallbackType = typeof(AtuoScheduleHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]

    public class AtuoScheduleHandle
    {
        /// <summary>
        /// 检验规程配置池
        /// </summary>
        readonly InspectionRulePool ilpool;

        /// <summary>
        /// 快速生成关键事件进度ID对象
        /// </summary>
        protected QuickGenerateIdHelper GenerateIdHelper { get; set; }

        #region 定义

        /// <summary>
        /// 特种设备台账字典集合  key：特种设备台账id value:特种设备台账
        /// </summary>
        private Dictionary<double, EquipAccountSelect> DicSpecialEquipmentAccount { set; get; }

        /// <summary>
        /// 特种设备台账与设备定检规程关联关系字典集合
        /// </summary>
        private Dictionary<double, List<EquipAccountRegularInspection>> DicRegularIns;

        /// <summary>
        /// 检验规程字典集合    key：检验规程id value:检验规程
        /// </summary>
        private Dictionary<double, List<InspectionProjectItem>> DicInspectionPro;

        /// <summary>
        /// 保存的数据实体
        /// </summary>
        private EntityList<RegularInspection> RegularInspectionList { get; set; }

        private EntityList<EquipAccountRegularInspection> EquipAccountRegularInspectionList { get; set; }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public AtuoScheduleHandle()
        {
            GenerateIdHelper = new QuickGenerateIdHelper();
            ilpool = new InspectionRulePool();
            DicSpecialEquipmentAccount = new Dictionary<double, EquipAccountSelect>();
            DicRegularIns = new Dictionary<double, List<EquipAccountRegularInspection>>();
            DicInspectionPro = new Dictionary<double, List<InspectionProjectItem>>();
            RegularInspectionList = new EntityList<RegularInspection>();
            EquipAccountRegularInspectionList = new EntityList<EquipAccountRegularInspection>();
        }

        /// <summary>
        ///  入口
        /// </summary>
        public virtual void DoSchedule()
        {
            //加载基础数据
            LoadBase();
            //生成实体数据
            InitDate();
            //保存
            SaveDate();
        }

        /// <summary>
        /// 加载基础数据
        /// </summary>
        public virtual void LoadBase()
        {
            //初始化配置池数据
            ilpool.Load();

            //先查询所有的特种设备台账数据信息
            var specialEquipAccountList = RT.Service.Resolve<SpecialEquipAccountController>().GetAllSpecialEquipAccountList();
            List<double> specialEquipAccountIds = specialEquipAccountList.Select(p => p.Id).ToList();
            DicSpecialEquipmentAccount = specialEquipAccountList.GroupBy(p => p.Id).ToDictionary(p => p.Key, p => p.FirstOrDefault());

            //根据特种设备台账Id集合找所包含的所有相关联的 设备定检规程(特种设备台账)（并且首次定检时间和下次检验时间不可同时为null）
            List<EquipAccountRegularInspection> RegularInsList = RT.Service.Resolve<SpecialEquipAccountController>().GetEquipAccountRegularInsById(specialEquipAccountIds).ToList();
            DicRegularIns = RegularInsList.GroupBy(p => p.SpecialEquipmentAccountId).ToDictionary(p => p.Key, p => p.ToList());
            //根据特种设备台账与检验定检规程
            List<double> InspectionRuleIds = RegularInsList.Select(p => p.InspectionRuleId).Distinct().ToList();

            //根据检验规程找 检验规程与点检项目的关系数据转字典
            DicInspectionPro = RT.Service.Resolve<InspectionRuleController>().GetInspectionProjectItemList(InspectionRuleIds).GroupBy(p => p.InspectionRuleId).ToDictionary(p => p.Key, p => p.ToList());
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void InitDate()
        {
            //获取数据库时间
            var now = RF.Find<EquipAccountRegularInspection>().GetDbTime();
            List<EquipAccountRegularInspection> RegularInsList = null;
            RegularInspection reg = null;
            foreach (var Specialid in DicRegularIns.Keys)
            {
                if (DicRegularIns.TryGetValue(Specialid, out RegularInsList))
                {
                    foreach (var RegularIns in RegularInsList)
                    {
                        //下次检验时间取值为：先取下次检验日期的值，为空则取首次检验日期
                        DateTime? NextInspectionDate = RegularIns.NextInspectionDate == null ? RegularIns.FirstInspectionDate : RegularIns.NextInspectionDate;

                        if (now.AddDays(RegularIns.WarningPeriod) >= NextInspectionDate)
                        {
                            //一条关系创建一个特种设备定检
                            reg = CreateRegularInspection(RegularIns);
                            RegularInspectionList.Add(reg);
                            //创建的此数据存在未提交的情况,不能重复生成此条数据
                            RegularIns.NotSubmit = false;
                            EquipAccountRegularInspectionList.Add(RegularIns);
                        }
                    }
                }
            }
            InitDetail();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void InitDetail()
        {
            List<InspectionProjectItem> InspecProjectItemList = null;
            //为特种设备定检生成id和code
            if (RegularInspectionList.Any())
            {
                List<double> RegularInspectionIds = BatchDataEntity.GetBatchEntityId<RegularInspection>(RegularInspectionList.Count);
                List<string> InspectionNoList = RT.Service.Resolve<RegularInspectionController>().GetInspectionNo(RegularInspectionList.Count);
                for (int i = 0; i < RegularInspectionList.Count; i++)
                {
                    RegularInspectionList[i].Id = RegularInspectionIds[i];
                    RegularInspectionList[i].InspectionNo = InspectionNoList[i];
                }
            }
            RegularInspectionResume resume = null;
            RegularInspectionDetail detail = null;
            List<RegularInspectionDetail> detailList = null;
            List<RegularInspectionResume> resumeList = null;
            //根据特种设备定检的检验规程所关联的检验项目自动生成 特种设备定检的明细数据
            foreach (var regularIns in RegularInspectionList)
            {
                detailList = new List<RegularInspectionDetail>();
                resumeList = new List<RegularInspectionResume>();
                if (DicInspectionPro.TryGetValue(regularIns.InspectionRuleId, out InspecProjectItemList))
                {
                    foreach (var inspecproitem in InspecProjectItemList)
                    {
                        detail = CreateRegularInspectionDetail(regularIns.Id, inspecproitem);
                        detailList.Add(detail);
                    }
                }
                if (detailList.Any())
                {
                    regularIns.RegularInspectionDetailList.AddRange(detailList);
                }
                //操作记录值创建一次
                resume = CreateRegularInspectionResume(regularIns);
                resumeList.Add(resume);
                if (resumeList.Any())
                {
                    regularIns.RegularInspectionResumeList.AddRange(resumeList);
                }
            }
        }

        /// <summary>
        /// 创建操作记录对象
        /// </summary>
        /// <param name="regularInspection">设备定检对象</param>
        /// <returns></returns>
        public virtual RegularInspectionResume CreateRegularInspectionResume(RegularInspection regularInspection)
        {
            RegularInspectionResume resume = new RegularInspectionResume();
            if (regularInspection != null)
            {
                resume.GenerateId();
                resume.RegularInspectionId = regularInspection.Id;
                resume.OperationType = OperationType.CREATE;
                resume.OperatorId = RT.IdentityId;
                resume.OperationDateTime = DateTime.Now;
                resume.CreateBy = RT.IdentityId;
                resume.CreateDate = DateTime.Now;
            }
            return resume;
        }

        /// <summary>
        /// 创建特种设备定检
        /// </summary>
        /// <param name="RegularIns"></param>
        /// <returns></returns>
        public virtual RegularInspection CreateRegularInspection(EquipAccountRegularInspection RegularIns)
        {
            RegularInspection reg = new RegularInspection();
            if (RegularIns != null)
            {
                reg.InspectionRuleId = RegularIns.InspectionRuleId;
                reg.SpecialEquipmentAccountId = RegularIns.SpecialEquipmentAccountId;
                reg.PlanInspectionDate = DateTime.Today;
                reg.InspectionStatus = InspectionStatus.Pending;
                reg.ApprovalStatus = ApprovalStatus.Draft;
                reg.BillSourceType = BillSourceType.Automatically;
            }
            //创建操作记录对象(生成后统一再创建履历)
            return reg;
        }

        /// <summary>
        /// 创建特种设备定检明细
        /// </summary>
        /// <param name="RegularInspectionId">特种设备定检单Id</param>
        /// <param name="insproitem"></param>
        /// <returns></returns>
        public virtual RegularInspectionDetail CreateRegularInspectionDetail(double RegularInspectionId, InspectionProjectItem insproitem)
        {
            RegularInspectionDetail detail = new RegularInspectionDetail();
            detail.Id = GenerateIdHelper.GetNewId<RegularInspectionDetail>();
            detail.RegularInspectionId = RegularInspectionId;
            if (insproitem != null)
            {
                detail.ProjectDetailId = insproitem.ProjectDetailId;
                
                detail.Part = insproitem.Part;
                detail.Consumable = insproitem.Consumable;
                detail.Method = insproitem.Method;
                detail.Standard = insproitem.Standard;
                detail.MinValue = insproitem.MinValue;
                detail.MaxValue = insproitem.MaxValue;
                detail.ProjectName = insproitem.ProjectName;
                detail.CycleType = insproitem.CycleType;
                detail.Unit = insproitem.Unit;
                detail.UseTime = insproitem.UseTime;
            }
            return detail;
        }

        /// <summary>
        /// 保存数据d
        /// </summary>
        public virtual void SaveDate()
        {
            using (var tran = DB.TransactionScope(EntityDataProvider.ConnectionStringName))
            {
                if (RegularInspectionList.Any())
                {
                    RF.Save(RegularInspectionList);
                }
                if (EquipAccountRegularInspectionList.Any())
                {
                    RF.Save(EquipAccountRegularInspectionList);
                }
                tran.Complete();
            }
        }
    }
}
