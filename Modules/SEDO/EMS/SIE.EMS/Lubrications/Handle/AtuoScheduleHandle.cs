using SIE.Common;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Common.Utils;
using SIE.EMS.Enums;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Lubrications.Configs;
using SIE.EMS.MainenanceProjects;
using SIE.Equipments.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Lubrications.Handle
{
    /// <summary>
    /// 自动产生润滑计划
    /// </summary>
    [Services.Service(FallbackType = typeof(AtuoScheduleHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]

    public class AtuoScheduleHandle
    {
        /// <summary>
        /// 快速生成关键事件进度ID对象
        /// </summary>
        protected QuickGenerateIdHelper GenerateIdHelper { get; set; }

        #region 定义

        /// <summary>
        /// 满足条件的标准润滑项目
        /// </summary>
        EntityList<EquipAccountLubricationProject> EquipAccountLubricationProjectList { get; set; }

        /// <summary>
        /// 生成润滑记录之后重计算下次润滑日期的标准润滑项目
        /// </summary>
        EntityList<EquipAccountLubricationProject> EditEquipAccountLubricationProjectList { get; set; }

        /// <summary>
        /// 润滑记录集合
        /// </summary>
        EntityList<Lubrication> LubricationList { get; set; }

        /// <summary>
        /// 计划规则配置项
        /// </summary>
        PlanTypeConfigValue PlanTypeConfig { get; set; }

        /// <summary>
        /// 分组字典 key：台账ID  value: key:责任部门id value: key:下次润滑日期 value 润滑项目
        /// </summary>
        Dictionary<double, Dictionary<double, Dictionary<DateTime, List<EquipAccountLubricationProject>>>> EquipAccountLubricationProjectDic { get; set; }


        /// <summary>
        /// 按标准项目ID分组
        /// </summary>
        Dictionary<double, EquipAccountLubricationProject> LubricationProjectDic { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public AtuoScheduleHandle()
        {
            GenerateIdHelper = new QuickGenerateIdHelper();
            EquipAccountLubricationProjectList = new EntityList<EquipAccountLubricationProject>();
            EditEquipAccountLubricationProjectList = new EntityList<EquipAccountLubricationProject>();
            LubricationList = new EntityList<Lubrication>();
            LubricationProjectDic = new Dictionary<double, EquipAccountLubricationProject>();
            EquipAccountLubricationProjectDic = new Dictionary<double, Dictionary<double, Dictionary<DateTime, List<EquipAccountLubricationProject>>>>();
        }

        /// <summary>
        ///  入口
        /// </summary>
        public virtual void DoSchedule()
        {
            //首先就需要验证计划规则,如无配置计划规则,则不允许运行
            PlanTypeConfig = ConfigService.GetConfig(new PlanTypeConfig(), typeof(Lubrication));
            if (PlanTypeConfig == null || PlanTypeConfig.PlanType == null)
            {
                return;
            }

            //加载基础数据
            LoadBase();
            //没有找到对应数据直接返回
            if (!EquipAccountLubricationProjectList.Any())
            {
                return;
            }
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
            //先查询所有的达到预警期的设备台账润滑项目
            if (PlanTypeConfig.PlanType == PlanType.CompleteDate)
            {
                //完工日期限制只取没有未提交的润滑记录
                EquipAccountLubricationProjectList = RT.Service.Resolve<LubricationController>().GetEquipAccountLubricationProjectList(false);
            }

            if (PlanTypeConfig.PlanType == PlanType.BaseDate)
            {
                //基准日期则不限制,不管是否有未提交的润滑记录都再生成新的
                EquipAccountLubricationProjectList = RT.Service.Resolve<LubricationController>().GetEquipAccountLubricationProjectList(null);
            }
            //当前时间+预警期>=下次润滑时间的数据
            EquipAccountLubricationProjectList = EquipAccountLubricationProjectList.Where(p => DateTime.Today.AddDays(p.WarningPeriod ?? 0) >= ((DateTime)p.NextDate)).AsEntityList();

            LubricationProjectDic = EquipAccountLubricationProjectList.GroupBy(p => p.Id).ToDictionary(p => p.Key, p => p.FirstOrDefault());
            EquipAccountLubricationProjectDic = EquipAccountLubricationProjectList.GroupBy(p => p.EquipAccountId)
               .ToDictionary(p => p.Key, p => p.GroupBy(d => d.DepartmentId ?? -1)
                  .ToDictionary(d => d.Key, d => d.GroupBy(pro => pro.NextDate ?? DateTime.MinValue)
                     .ToDictionary(pro => pro.Key, pro => pro.ToList())));
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void InitDate()
        {
            Dictionary<double, Dictionary<DateTime, List<EquipAccountLubricationProject>>> equipAccountDic;
            Dictionary<DateTime, List<EquipAccountLubricationProject>> departmentDic;
            List<EquipAccountLubricationProject> list;
            //循环每个设备台账需要生成润滑记录的标准润滑项目按台账ID,部门ID,润滑时间生成
            foreach (var pro in EquipAccountLubricationProjectDic.Keys)
            {
                if (EquipAccountLubricationProjectDic.TryGetValue(pro, out equipAccountDic))
                {
                    foreach (var DepartmentId in equipAccountDic.Keys)
                    {
                        if (equipAccountDic.TryGetValue(DepartmentId, out departmentDic))
                        {
                            foreach (var NextDate in departmentDic.Keys)
                            {
                                if (departmentDic.TryGetValue(NextDate, out list) && list.Any())
                                {
                                    CreateLubrication(list);
                                }
                            }
                        }
                    }
                }
            }

            //给润滑记录赋值单号
            if (LubricationList.Any())
            {
                List<string> CodeList = RT.Service.Resolve<LubricationController>().GetLubricationNo(LubricationList.Count());
                int i = 0;
                LubricationList.ForEach(p =>
                {
                    p.LubricationNo = CodeList[i];
                    i++;
                });
            }
        }


        /// <summary>
        /// 创建润滑记录
        /// </summary>
        public void CreateLubrication(List<EquipAccountLubricationProject> lubDetaillist)
        {
            if (lubDetaillist == null || !lubDetaillist.Any())
            {
                return;
            }
            EquipAccountLubricationProject proD = lubDetaillist.FirstOrDefault();
            EquipAccountLubricationProject editpro;
            //创建润滑记录
            //生成润滑计划
            //生成润滑记录后再统一生成编码赋值
            Lubrication lub = new Lubrication();
            lub.Id = GenerateIdHelper.GetNewId<Lubrication>();
            lub.EquipAccountId = proD.EquipAccountId;
            lub.DepartmentId = proD.DepartmentId == -1 ? null : proD.DepartmentId;
            lub.CycleType = CycleType.Day;
            lub.PlanDate = proD.NextDate.Value;
            lub.BillSourceType = BillSourceType.Automatically;
            lub.LubricationStatus = LubricationStatus.Pending;
            lub.ApprovalStatus = ApprovalStatus.Draft;

            LubricationDetail detail;
            //生成润滑项目
            foreach (var projectDetail in lubDetaillist)
            {
                if (!projectDetail.ProjectCycle.HasValue)
                {
                    throw new ValidationException("润滑项目【{0}】的周期为空".L10nFormat(projectDetail.ProjectName));
                }

                if (!projectDetail.LubricatingType.HasValue)
                {
                    throw new ValidationException("润滑项目【{0}】的润滑方式为空".L10nFormat(projectDetail.ProjectName));
                }

                if (!projectDetail.WarningPeriod.HasValue)
                {
                    throw new ValidationException("润滑项目【{0}】的预警期为空".L10nFormat(projectDetail.ProjectName));
                }


                detail = new LubricationDetail();
                detail.LubricationId = lub.Id;
                detail.ProjectDetailId = projectDetail.Id;
                detail.ProjectCycle = projectDetail.ProjectCycle.Value;
                detail.WarningPeriod = projectDetail.WarningPeriod.Value.ToString();
                detail.Part = projectDetail.Part;
                detail.Consumable = projectDetail.Consumable;
                detail.Method = projectDetail.Method;
                detail.Standard = projectDetail.Standard;
                detail.MinValue = projectDetail.MinValue;
                detail.MaxValue = projectDetail.MaxValue;
                detail.ProjectName = projectDetail.ProjectName;
                detail.CycleType = projectDetail.CycleType;             
                detail.Unit = projectDetail.Unit;
                detail.UseTime = projectDetail.UseTime;
                //上下次润滑日期在此处不需要记录,只需要反写
                detail.LubricatingType = projectDetail.LubricatingType.Value;
                lub.LubricationDetailList.Add(detail);

                //计算并反写台账标准润滑项目的上下次润滑时间
                if (LubricationProjectDic.TryGetValue(projectDetail.Id, out editpro) && editpro != null)
                {
                    CalculationDate(editpro);
                }
            }
            LubricationList.Add(lub);
        }

        /// <summary>
        /// 计算标准润滑项目的上次与下次润滑日期
        /// </summary>
        /// <param name="project">设备台账标准润滑项目</param>
        /// <returns></returns>
        public virtual void CalculationDate(EquipAccountLubricationProject project)
        {
            if (project != null)
            {
                //如果计划规则为基准日期反写标准项目的上次下次润滑日期 不考虑延期
                if (PlanTypeConfig.PlanType == PlanType.BaseDate)
                {
                    //上一次润滑时间 = 下次润滑日期
                    project.LastDate = project.NextDate;
                    //下一次润滑时间 = 上一次的下次润滑日期+周期
                    project.NextDate = project.LastDate.Value.AddDays(project.ProjectCycle.Value);
                }
                if (PlanTypeConfig.PlanType == PlanType.CompleteDate)
                {
                    //只给完工日期改变标识的原因，基准日期可按照下次润滑日期重复生成，而完工日期如果已生成过则不能重复生成。
                    //场景：如先按基准日期生成了润滑记录，未提交，再改变计划规则为完工日期，生成数据，则也不重复生成完工日期数据
                    //计划规则为完工日期时需要表示此润滑项目已经生成，不能重复生成
                    project.NotSubmit = true;
                }
                EditEquipAccountLubricationProjectList.Add(project);
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public virtual void SaveDate()
        {
            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                if (LubricationList.Any())
                {
                    RF.Save(LubricationList);
                }
                if (EditEquipAccountLubricationProjectList.Any())
                {
                    RF.Save(EditEquipAccountLubricationProjectList);
                }
                tran.Complete();
            }
        }
    }
}
