using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using System;

namespace SIE.Resources.Enterprises
{
    #region 企业层级验证
    /// <summary>
    /// 层级有关联架构时不允许修改验证
    /// </summary>
    [System.ComponentModel.DisplayName("企业层级验证规则-修改")]
    [System.ComponentModel.Description("层级有关联架构时不允许修改")]
    public class UnEditResourceLevelHasOrg : EntityRule<EnterpriseLevel>
    {

        /// <summary>
        /// 企业层级验证规则-更新（AddorUpdate）
        /// </summary>
        [System.ComponentModel.DisplayName("企业层级验证规则-更新")]
        [System.ComponentModel.Description("添加/修改确保不跟通用组织的编码/名称重复")]
        public class AddOrUpdateResourceLevel : EntityRule<EnterpriseLevel>
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            public AddOrUpdateResourceLevel()
            {
                Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
                ConnectToDataSource = true;
            }

            /// <summary>
            /// 验证
            /// </summary>
            /// <param name="entity"></param>
            /// <param name="e"></param>
            protected override void Validate(IEntity entity, RuleArgs e)
            {
                var resourceLevel = entity as EnterpriseLevel;
                double treePid = resourceLevel.TreePId ?? 0;
                if (treePid == 0)
                    return;
                var enterpriseLevel = RT.Service.Resolve<EnterpriseController>().GetEnterpriseLevel(resourceLevel);
                if (enterpriseLevel != null)//根节点不做验证
                    e.BrokenDescription = "已存在编码:{0} 或 名称：{1}".L10nFormat(resourceLevel.Code, resourceLevel.Name);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UnEditResourceLevelHasOrg()
        {
            Scope = EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var resourceLevel = entity as EnterpriseLevel;
            if (RT.Service.Resolve<EnterpriseController>().LevelHasResource(resourceLevel.Id))
            {
                var oldResourceLevel = RF.GetById<EnterpriseLevel>(resourceLevel.Id);
                if (oldResourceLevel.IsResource)
                {
                    e.BrokenDescription = "层级{0}有关联企业模型，不允许修改".L10nFormat(resourceLevel.Name);
                }
            }
        }
    }

    /// <summary>
    /// 层级有关联架构时不允许修改验证
    /// </summary>
    [System.ComponentModel.DisplayName("企业层级验证规则-添加")]
    [System.ComponentModel.Description("层级有关联架构时不允许添加")]
    public class AddEditResourceLevelHasOrg : EntityRule<EnterpriseLevel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AddEditResourceLevelHasOrg()
        {
            Scope = EntityStatusScopes.Add;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var resourceLevel = entity as EnterpriseLevel;
            if (RT.Service.Resolve<EnterpriseController>().LevelHasResource(resourceLevel.Id))
            {
                var oldResourceLevel = RF.GetById<EnterpriseLevel>(resourceLevel.TreePId);
                if (oldResourceLevel.IsResource)
                {
                    e.BrokenDescription = "父节点{0}有关联企业模型，不允许添加".L10nFormat(resourceLevel.Name);
                }
            }
        }
    }

    /// <summary>
    /// 层级有关联架构时不允许删除验证
    /// </summary>
    [System.ComponentModel.DisplayName("企业层级验证规则-删除")]
    [System.ComponentModel.Description("层级有关联架构时不允许删除")]
    public class UnDeleteResourceLevelHasOrg : EntityRule<EnterpriseLevel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnDeleteResourceLevelHasOrg()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var resourceLevel = entity as EnterpriseLevel;
            if (resourceLevel.TreePId == null)
                e.BrokenDescription = "层级{0}根节点，不允许删除".L10nFormat(resourceLevel.Name);
            if (RT.Service.Resolve<EnterpriseController>().LevelHasResource(resourceLevel.Id))
                e.BrokenDescription = "层级{0}有关联企业模型，不允许删除".L10nFormat(resourceLevel.Name);
        }
    }
    #endregion

    #region 企业架构验证
    /// <summary>
    /// 企业模型验证规则-更新（AddorUpdate）
    /// </summary>
    [System.ComponentModel.DisplayName("企业模型验证规则-更新")]
    [System.ComponentModel.Description("添加/修改确保不跟通用组织的编码/名称重复")]
    public class AddOrUpdateEnterpriseResource : EntityRule<Enterprise>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AddOrUpdateEnterpriseResource()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var resource = entity as Enterprise;
            double treePid = resource.TreePId ?? 0;
            if (treePid == 0)
                return;
            var enterpriseResource = RT.Service.Resolve<EnterpriseController>().GetInvOrgEnterprises(resource);
            if (enterpriseResource != null)//根节点不做验证
                e.BrokenDescription = "已存在编码:{0} 或 名称：{1}".L10nFormat(resource.Code, resource.Name);
        }
    }

    /// <summary>
    /// 架构有关联员工数据时不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("企业架构验证规则")]
    [System.ComponentModel.Description("架构下有关联员工时不允许删除")]
    public class UnDeleteResourceHasEmployee : EntityRule<Enterprise>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnDeleteResourceHasEmployee()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var resource = entity as Enterprise;
            if (resource.TreePId == null)
                e.BrokenDescription = "企业{0}根节点，不允许删除".L10nFormat(resource.Name);
            if (RT.Service.Resolve<EmployeeController>().ResourceHasEmployee(resource.Id))
            {
                e.BrokenDescription = "企业{0}有关联员工数据，不允许删除".L10nFormat(resource.Name);
            }
        }
    }

    /// <summary>
    /// 企业模型同步到生产资源后不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("企业模型验证规则")]
    [System.ComponentModel.Description("企业模型同步到生产资源后不能删除")]
    public class UnDeleteEnterpriseHasWipResource : EntityRule<Enterprise>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnDeleteEnterpriseHasWipResource()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var enterprise = entity as Enterprise;
            var wipResource = RT.Service.Resolve<WipResourceController>().GetWipResource(enterprise.Id, SyncSourceType.Enterprise);
            if (wipResource != null)
            {
                e.BrokenDescription = "企业模型 [{0}] 被生产资源 [{1}] 引用 , 不允许删除".L10nFormat(enterprise.Name, wipResource.Name);
            }
        }
    }

    /// <summary>
    /// 企业模型同步到生产资源后不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("企业模型验证规则")]
    [System.ComponentModel.Description("根据企业层级判断是否可以设置为资源")]
    public class SetEnterpriseResourceFromLevel : EntityRule<Enterprise>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SetEnterpriseResourceFromLevel()
        {
            Scope = EntityStatusScopes.Update | EntityStatusScopes.Add;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var enterprise = entity as Enterprise;
            if (enterprise != null && enterprise.IsResource)
            {
                var enterpriseLevel = RF.GetById<EnterpriseLevel>(enterprise.LevelId);
                if (enterpriseLevel != null && !enterpriseLevel.IsResource)
                {
                    e.BrokenDescription = "企业层级[{0}]不是资源，对应的企业模型不能设置为资源".L10nFormat(enterpriseLevel.Name);
                }
            }
        }
    }

    /// <summary>
    /// 更新层级企业模型存在子节点不可修改
    /// </summary>
    [System.ComponentModel.DisplayName("企业模型验证规则")]
    [System.ComponentModel.Description("更新层级企业模型存在子节点不可修改")]
    public class UpdateEnterpriseResourceFromLevel : EntityRule<Enterprise>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateEnterpriseResourceFromLevel()
        {
            Scope = EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var enterprise = entity as Enterprise;
            var enterpriseEntity = RF.GetById<Enterprise>(enterprise.Id);
            if (enterpriseEntity != null && enterpriseEntity.LevelId != enterprise.LevelId && RT.Service.Resolve<EnterpriseController>().EnterpriseHasChild(enterpriseEntity.Id))
            {
                e.BrokenDescription = "企业模型[{0}]存在子节点，不可修改层级".L10nFormat(enterprise.Name);
            }
        }
    }
    #endregion
}
