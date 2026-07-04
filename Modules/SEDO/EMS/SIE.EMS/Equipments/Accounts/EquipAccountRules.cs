using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Projects;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Projects;
using SIE.EMS.SpareParts.Applys;
using SIE.Equipments.Configs;
using SIE.Equipments.EquipAccountLocations;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.Equipments.Accounts
{
    #region 点检项目规则
    /// <summary>
    /// 点检项目非重验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("点检项目验证规则")]
    [System.ComponentModel.Description("点检项目不能重复")]
    public class NotDuplicateCheckProject : NotDuplicateRule<EquipAccountCheckProject>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateCheckProject()
        {
            Properties.Add(EquipAccountCheckProject.EquipAccountIdProperty);
            Properties.Add(EquipAccountCheckProject.ProjectDetailIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as EquipAccountCheckProject;
                return "已存在项目名称[{0}]的点检项目".L10nFormat(entity.ProjectDetail.Name);
            };
        }
    }
    #endregion


    #region 润滑项目规则
    /// <summary>
    /// 润滑项目非重验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("润滑项目验证规则")]
    [System.ComponentModel.Description("润滑项目不能重复")]
    public class NotDuplicateLubricationProject : NotDuplicateRule<EquipAccountLubricationProject>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateLubricationProject()
        {
            Properties.Add(EquipAccountLubricationProject.EquipAccountIdProperty);
            Properties.Add(EquipAccountLubricationProject.ProjectDetailIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as EquipAccountLubricationProject;
                return "已存在项目名称[{0}]的润滑项目".L10nFormat(entity.ProjectDetail.Name);
            };
        }
    }
    #endregion


    #region 保养项目规则
    /// <summary>
    /// 保养项目非重验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("保养项目验证规则")]
    [System.ComponentModel.Description("保养项目不能重复")]
    public class NotDuplicateMaintainProject : NotDuplicateRule<EquipAccountMaintainProject>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateMaintainProject()
        {
            Properties.Add(EquipAccountMaintainProject.EquipAccountIdProperty);
            Properties.Add(EquipAccountMaintainProject.ProjectDetailIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as EquipAccountMaintainProject;
                return "已存在项目名称[{0}]的保养项目".L10nFormat(entity.ProjectDetail.Name);
            };
        }
    }
    #endregion

    //#region 单元组成规则
    ///// <summary>
    ///// 单元组成非重验证规则
    ///// </summary>
    //[System.ComponentModel.DisplayName("单元组成验证规则")]
    //[System.ComponentModel.Description("单元组成设备单元不能重复")]
    //public class NotDuplicateUnit : NotDuplicateRule<EquipAccountUnit>
    //{
    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    public NotDuplicateUnit()
    //    {
    //        Properties.Add(EquipAccountUnit.EquipAccountIdProperty);
    //        Properties.Add(EquipAccountUnit.EquipUnitIdProperty);
    //        MessageBuilder = (e) =>
    //        {
    //            var entity = e as EquipAccountUnit;
    //            return "已存在设备单元名称[{0}]的单元组成".L10nFormat(entity.EquipUnit.Name);
    //        };
    //    }
    //}
    //#endregion

    //#region 单元组成物料规则
    ///// <summary>
    ///// 单元组成物料非重验证规则
    ///// </summary>
    //[System.ComponentModel.DisplayName("单元组成物料验证规则")]
    //[System.ComponentModel.Description("单元组成物料物料不能重复")]
    //public class NotDuplicateUnitItem : NotDuplicateRule<EquipAccountUnitItem>
    //{
    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    public NotDuplicateUnitItem()
    //    {
    //        Properties.Add(EquipAccountUnitItem.UnitIdProperty);
    //        Properties.Add(EquipAccountUnitItem.ItemIdProperty);
    //        MessageBuilder = (e) =>
    //        {
    //            var entity = e as EquipAccountUnitItem;
    //            return "已存在物料名称[{0}]的单元组成物料".L10nFormat(entity.Item.Name);
    //        };
    //    }
    //}
    //#endregion

    #region 设备台账不能删除验证

    #region 设备台账存在点检计划引用不能删除
    /// <summary>
    /// 设备台账存在点检计划引用不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("NoReferencedRule验证规则")]
    [System.ComponentModel.Description("设备台账存在点检计划引用不能删除")]
    public class CheckPlanToEquipAccountReferencedRule : NoReferencedRule<EquipAccount>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckPlanToEquipAccountReferencedRule()
        {
            Properties.Add(CheckPlan.EquipAccountIdProperty);
            MessageBuilder = (o, e) =>
            {
                return "不能删除，设备台账被点检计划引用".L10nFormat();
            };
        }
    }
    #endregion

    #region 设备台账存在保养计划引用不能删除
    /// <summary>
    /// 设备台账存在保养计划引用不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("NoReferencedRule验证规则")]
    [System.ComponentModel.Description("设备台账存在保养计划引用不能删除")]
    public class MaintainPlanToEquipAccountReferencedRule : NoReferencedRule<EquipAccount>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaintainPlanToEquipAccountReferencedRule()
        {
            Properties.Add(MaintainPlan.EquipAccountIdProperty);
            MessageBuilder = (o, e) =>
            {
                return "不能删除，设备台账被保养计划引用".L10nFormat();
            };
        }
    }
    #endregion

    #region 设备台账存在点检计划引用不能删除
    /// <summary>
    /// 设备台账存在设备台账点检保养项目引用不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("NoReferencedRule验证规则")]
    [System.ComponentModel.Description("设备台账存在点检计划引用不能删除")]
    public class CheckPlanToEquipAssetReferencedRule : NoReferencedRule<EquipAccount>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckPlanToEquipAssetReferencedRule()
        {
            Properties.Add(CheckProject.EquipAccountIdProperty);
            MessageBuilder = (o, e) =>
            {
                return "不能删除，设备台账被点检计划引用".L10nFormat();
            };
        }
    }
    #endregion

    #region 设备台账存在保养计划引用不能删除
    /// <summary>
    /// 设备台账存在保养计划引用不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("NoReferencedRule验证规则")]
    [System.ComponentModel.Description("设备台账存在保养计划引用不能删除")]
    public class MaintainPlanToEquipAssetReferencedRule : NoReferencedRule<EquipAccount>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaintainPlanToEquipAssetReferencedRule()
        {
            Properties.Add(MaintainProject.EquipAccountIdProperty);
            MessageBuilder = (o, e) =>
            {
                return "不能删除，固定资产被保养计划引用".L10nFormat();
            };
        }
    }
    #endregion

    #region 设备台账存在生产引用不能删除
    /// <summary>
    /// 设备台账存在生产资源引用不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("设备台账存在生产资源引用不能删除")]
    [System.ComponentModel.Description("设备台账存在生产资源引用不能删除")]
    public class WipResourceToEquipAssetReferencedRule : EntityRule<EquipAccount>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipResourceToEquipAssetReferencedRule()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 根据企业模型对应的资源有否被工位引用判断是否能删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var ent = entity as EquipAccount;
            if (RT.Service.Resolve<WipResourceController>().GetWipResource(ent.Id, Resources.WipResources.SyncSourceType.Equipment) != null)
            {
                e.BrokenDescription = "设备台账已经被生产资源引用，不能删除".L10N();
            }
        }
    }
    #endregion

    #region 设备台账存在备件备件申请单不能删除
    /// <summary>
    /// 设备台账存在备件备件申请单不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("设备台账存在备件备件申请单不能删除")]
    [System.ComponentModel.Description("设备台账存在备件备件申请单不能删除")]

    public class SparePartAppToEquipNoReferencedRule : NoReferencedRule<EquipAccount>
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public SparePartAppToEquipNoReferencedRule()
        {
            Properties.Add(SparePartApp.EquipAccountIdProperty);
            MessageBuilder = (o, e) =>
            {
                return "不能删除，设备台账被备件申请单引用".L10nFormat();
            };
        }
    }
    #endregion
    #endregion

    //#region 当设备为检验设备时,使用级别、使用部门和责任人非空验证
    ///// <summary>
    ///// 当设备为检验设备时,使用级别、使用部门和责任人非空验证
    ///// </summary>
    //[System.ComponentModel.DisplayName("当设备为检验设备时,使用级别、使用部门和责任人非空验证")]
    //[System.ComponentModel.Description("当设备为检验设备时,使用级别、使用部门和责任人非空验证")]
    //public class EquipAccountEquipTypeIsCheckRule : EntityRule<EquipAccount>
    //{
    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    public EquipAccountEquipTypeIsCheckRule()
    //    {
    //        Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
    //        ConnectToDataSource = true;
    //    }

    //    /// <summary>
    //    /// 验证方法
    //    /// </summary>
    //    /// <param name="entity">实体</param>
    //    /// <param name="e">规则参数</param>
    //    protected override void Validate(IEntity entity, RuleArgs e)
    //    {
    //        var equipAccount = entity as EquipAccount;

    //        var equipModel = RF.GetById<EquipModel>(equipAccount.EquipModelId, new EagerLoadOptions().LoadWithViewProperty());

    //        if (equipModel == null)
    //        {
    //            return;
    //        }

    //        if (equipModel.EquipTypeIsCheck && equipAccount.UseLevel == "")
    //        {
    //            e.BrokenDescription = "当设备为检验设备时,使用级别必填！".L10N();
    //        }
    //        if (equipModel.EquipTypeIsCheck && equipAccount.UseDepartment == null)
    //        {
    //            e.BrokenDescription = "当设备为检验设备时,使用部门必填！".L10N();
    //        }
    //        if (equipModel.EquipTypeIsCheck && equipAccount.ResPerson == null)
    //        {
    //            e.BrokenDescription = "当设备为检验设备时,责任人必填！".L10N();
    //        }
    //    }
    //}
    //#endregion

    #region 仪器参数
    /// <summary>
    /// 仪器参数验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("仪器参数验证规则")]
    [System.ComponentModel.Description("仪器参数最大值需要大于最小值")]
    public class EquipParamMaxMinValueRule : EntityRule<EquipParam>
    {
        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var param = entity as EquipParam;
            if (param.Max <= param.Min)
            {
                e.BrokenDescription = "仪器参数项目[{0}]的最大值需要大于最小值！".L10nFormat(param.Name);
            }
        }
    }
    #endregion

    #region 设备台账实体验证规则
    /// <summary>
    /// 设备台账实体验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("设备台账实体验证规则")]
    [System.ComponentModel.Description("配置的设备类型，必须填写位置列表至少一行提交前事件")]
    public class EquipAccountRule : EntityRule<EquipAccount>
    {
        /// <summary>
        /// 验证配置的设备类型，必须填写位置列表至少一行
        /// </summary>
        /// <param name="entity">设备台帐</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var equipAccount = entity as EquipAccount;
            //获取设备型号中配置项的值
            var config = ConfigService.GetConfig(new EquipAccountsLocationConfig(), typeof(EquipAccount));
            if (config.EquipTypeIds != null)
            {
                var equipmodel = RT.Service.Resolve<ElecEquipController>().GetEquipModel(equipAccount.EquipModelId);
                var list = new List<string>(config.EquipTypeIds.Split(','));
                //判断添加或者修改的设备型号-需维护位置列表的设备类型
                if (equipmodel.EquipTypeId != null && equipmodel.IndustryCategory == Core.Enums.IndustryCategory.ElecInitIndustry && list.Contains(equipmodel.EquipTypeId.ToString()))
                {
                    //实际存在的位置列表
                    var locations = new List<EquipAccountLocation>();
                    //数据库真正存在的位置列表
                    var orgLocationList = RT.Service.Resolve<EquipController>().GetLocationsOfAccounts(new List<double>() { equipAccount.Id });
                    //界面实际维护的位置列表
                    var uILocationList = equipAccount.EquipAccountLocationList;
                    if (uILocationList != null)
                    {
                        //界面删除的位置列表
                        var deleteLocations = uILocationList.DeletedList.Cast<EquipAccountLocation>();
                        var deleteIds = deleteLocations.Select(p => p.Id).Distinct();
                        locations.AddRange(orgLocationList.Where(p => !deleteIds.Contains(p.Id)));
                        foreach (var location in uILocationList)
                        {
                            if (!locations.Any(p => p.EquipAccountId == location.EquipAccountId && p.Stance == location.Stance))
                                locations.Add(location);
                        }
                    }
                    else
                        locations.AddRange(orgLocationList);
                    if (locations.Count <= 0)
                        e.BrokenDescription = "选择的[{0}]设备类型，必须填写位置列表至少一行".L10nFormat(equipmodel.TypeName);
                }
            }

            if (!equipAccount.RFID.IsNullOrEmpty())
            {
                var isExsited = RT.Service.Resolve<EquipController>().IsExsitedRFID(equipAccount.Id, equipAccount.RFID);
                if (isExsited)
                    e.BrokenDescription = "设备台账已存在：“{0}”的RFID".L10nFormat(equipAccount.RFID);


            }
            if (equipAccount.UsefulLife != null && equipAccount.UsefulLife <= 0) { e.BrokenDescription = "设备台账使用年限必须为正数".L10N(); }
        }
    }
    #endregion

    #region 设备台账位置非重验证规则
    /// <summary>
    /// 设备台账位置非重验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("设备台账位置非重验证规则")]
    [System.ComponentModel.Description("设备台账位置:同一设备下站位不能重复")]
    public class NotDuplicateEquipAccountLocation : NotDuplicateRule<EquipAccountLocation>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateEquipAccountLocation()
        {
            Properties.Add(EquipAccountLocation.StanceProperty);
            Properties.Add(EquipAccountLocation.EquipAccountIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as EquipAccountLocation;
                return "已存在站位[{0}]的设备台账位置".L10nFormat(entity.Stance);
            };
        }
    }
    #endregion


    #region 润滑项目实体验证规则
    /// <summary>
    /// 设备台账维护实体验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("润滑项目实体验证规则")]
    [System.ComponentModel.Description("添加润滑项目,必须填写周期,预警期,润滑方式,责任部门")]
    public class EquipModelRule : EntityRule<EquipAccountLubricationProject>
    {
        /// <summary>
        /// 验证配置的设备类型，必须填写位置列表至少一行
        /// </summary>
        /// <param name="entity">设备型号维护</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var lub = entity as EquipAccountLubricationProject;
            StringBuilder sb = new StringBuilder();

            if (lub.ProjectDetailId == 0)
            {
                sb.Append("润滑项目不能为空".L10N());
            }
            if (!lub.ProjectCycle.HasValue)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append("周期不能为空".L10N());
            }
            if (!lub.WarningPeriod.HasValue)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append("预警期不能为空".L10N());
            }
            if (lub.LubricatingType == null)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append("润滑方式不能为空".L10N());
            }
            if (sb.Length > 0)
            {
                //sb.Insert(0, "设备台账".L10N()+ "【"+lub.EquipAccount.Code+"】");
               // throw new ValidationException("设备台账".L10N() + string.Format("【{0}】", lub.EquipAccount.Code) + sb.ToString());

                throw new ValidationException("设备台账【{0}】".L10nFormat(lub.EquipAccount.Code) + sb.ToString());
            }
        }
    }
    #endregion


    #region 设备维修定标规则

    /// <summary>
    /// 设备维修定标规则
    /// </summary>
    [System.ComponentModel.DisplayName("设备台账维修定标类别非重验证规则")]
    [System.ComponentModel.Description("设备台账维修定标:同一设备下定标类别不能相同")]
    public class NotDuplicateEquipAccountRepairStandard : NotDuplicateRule<EquipAccountRepairStandard>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateEquipAccountRepairStandard()
        {
            Properties.Add(EquipAccountRepairStandard.StandardTypeProperty);
            Properties.Add(EquipAccountRepairStandard.EquipAccountIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as EquipAccountRepairStandard;
                return "已存在定标类别[{0}]的设备台账维修定标".L10nFormat(entity.StandardType.ToLabel());
            };
        }
    }
    #endregion
}
