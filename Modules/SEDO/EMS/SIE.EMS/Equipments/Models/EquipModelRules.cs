using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipModels;
using SIE.MetaModel;
using System;
using System.ComponentModel;
using System.Text;

namespace SIE.EMS.Equipments.Models
{
    #region 点检项目规则
    /// <summary>
    /// 点检项目非重验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("点检项目验证规则")]
    [System.ComponentModel.Description("点检项目不能重复")]
    public class NotDuplicateCheckProject : NotDuplicateRule<EquipModelCheckProject>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateCheckProject()
        {
            Properties.Add(EquipModelCheckProject.EquipModelIdProperty);
            Properties.Add(EquipModelCheckProject.ProjectDetailIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as EquipModelCheckProject;
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
    public class NotDuplicateLubricationProject : NotDuplicateRule<EquipModelLubricationProject>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateLubricationProject()
        {
            Properties.Add(EquipModelLubricationProject.EquipModelIdProperty);
            Properties.Add(EquipModelLubricationProject.ProjectDetailIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as EquipModelLubricationProject;
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
    public class NotDuplicateMaintainProject : NotDuplicateRule<EquipModelMaintainProject>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateMaintainProject()
        {
            Properties.Add(EquipModelMaintainProject.EquipModelIdProperty);
            Properties.Add(EquipModelMaintainProject.ProjectDetailIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as EquipModelMaintainProject;
                return "已存在项目名称[{0}]的保养项目".L10nFormat(entity.ProjectDetail.Name);
            };
        }
    }
    #endregion

    #region 设备型号位置非重验证规则
    /// <summary>
    /// 设备型号位置非重验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("设备型号位置非重验证规则")]
    [System.ComponentModel.Description("设备型号位置:同一设备型号下站位不能重复")]
    public class NotDuplicateEquipModelLocation : NotDuplicateRule<EquipModelLocation>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateEquipModelLocation()
        {
            Properties.Add(EquipModelLocation.StanceProperty);
            Properties.Add(EquipModelLocation.EquipModelIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as EquipModelLocation;
                return "已存在站位[{0}]的设备型号位置".L10nFormat(entity.Stance);
            };
        }
    }
    #endregion

    #region 设备型号技术参数非重验证规则
    /// <summary>
    /// 设备型号位置非重验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("设备型号技术参数非重验证规则")]
    [System.ComponentModel.Description("设备型号技术参数:同一设备型号下技术参数不能重复")]
    public class NotDuplicateEquipModelTechParameter : NotDuplicateRule<EquipModelTechParameter>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateEquipModelTechParameter()
        {
            Properties.Add(EquipModelTechParameter.ParameterNameProperty);
            Properties.Add(EquipModelTechParameter.EquipModelIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as EquipModelTechParameter;
                return "设备型号{0}已存在{1}技术参数".L10nFormat(entity.EquipModel.Code, entity.ParameterName);
            };
        }
    }
    #endregion

    #region 润滑项目实体验证规则
    /// <summary>
    /// 设备型号维护实体验证规则
    /// </summary>
    [DisplayName("润滑项目实体验证规则")]
    [Description("添加润滑项目,必须填写周期,预警期,润滑方式")]
    public class EquipModelLubricationProjectRule : EntityRule<EquipModelLubricationProject>
    {
        /// <summary>
        /// 验证配置的设备类型，必须填写位置列表至少一行
        /// </summary>
        /// <param name="entity">设备型号维护</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var lub = entity as EquipModelLubricationProject;
            StringBuilder sb = new StringBuilder();

            if (lub.ProjectDetail == null)
            {
                sb.Append("润滑项目不能为空");
            }
            if (!lub.ProjectCycle.HasValue)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append("周期不能为空");
            }
            if (!lub.WarningPeriod.HasValue)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append("预警期不能为空");
            }
            if (lub.LubricatingType == null)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append("润滑方式不能为空");
            }
            if (sb.Length > 0)
            {
                sb.Insert(0, string.Format("设备型号【{0}】", lub.EquipModel.Code));
                throw new ValidationException(sb.ToString().L10N());
            }
        }
    }
    #endregion
}
