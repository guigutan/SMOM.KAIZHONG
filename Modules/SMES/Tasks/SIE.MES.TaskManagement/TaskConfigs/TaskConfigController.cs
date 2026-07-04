using SIE.Domain;
using SIE.Items;
using SIE.Items.ProductFamilys;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TaskManagement.TaskConfigs
{
    public class TaskConfigController : DomainController
    {
        /// <summary>
        /// 通过产品族Id获取任务单生成配置项
        /// </summary>
        /// <param name="productFamilyId">产品族Id</param>
        /// <returns>任务单生成配置项</returns>
        public virtual TaskConfig GetTaskConfig(double productFamilyId)
        {
            return Query<TaskConfig>().Where(p => p.ProductFamilyId == productFamilyId).FirstOrDefault();
        }

        /// <summary>
        /// 保存任务单生成规则列表
        /// </summary>
        /// <param name="configs">任务单生成规则配置信息</param>
        public virtual void SaveTaskConfigs(List<FamilyTaskConfig> configs)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                configs.ForEach(config =>
                {
                    SaveTaskConfig(config.FamilyId, config.Config);
                });
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存任务单生成规则列表
        /// </summary>
        /// <param name="configs">任务单生成规则配置信息</param>
        public virtual string ValidateTaskConfigs(List<FamilyTaskConfig> configs)
        {
            string errMsg = string.Empty;
            var familyIds = configs.Select(p => p.FamilyId).Distinct().ToList();
            var familys = RT.Service.Resolve<ProductFamilyController>().GetProductFamilyList(familyIds);
            var dicFamilys = familys.ToDictionary(p => p.Id);
            errMsg = ValidateConfigData(configs, dicFamilys);
            if (errMsg.Length > 0)
                errMsg = "保存失败," + errMsg + "固定数量需要大于0;";

            return errMsg;
        }

        /// <summary>
        /// 验证产品族任务单生成配置项
        /// </summary>
        /// <param name="configs">配置列表</param>
        /// <param name="dicFamilys">产品族字典</param>
        /// <returns>错误信息</returns>
        private string ValidateConfigData(List<FamilyTaskConfig> configs, Dictionary<double, ProductFamily> dicFamilys)
        {
            string errMsg = string.Empty;
            configs.ForEach(config =>
            {
                ProductFamily family = null;
                dicFamilys.TryGetValue(config.FamilyId, out family);
                if (config.Config.ByQty && config.Config.Qty <= 0)
                    errMsg += "产品族[{0}],".L10nFormat(family.Name);
            });

            return errMsg;
        }

        /// <summary>
        /// 保存任务单生成规则配置
        /// </summary>
        /// <param name="familyId">产品族ID</param>
        /// <param name="configInfo">任务单生成规则配置信息</param>
        void SaveTaskConfig(double familyId, TaskConfigInfo configInfo)
        {
            var config = GetTaskConfig(familyId);
            if (config == null)
                config = new TaskConfig() { ProductFamilyId = familyId };
            config.Qty = configInfo.Qty;
            config.ByQty = configInfo.ByQty;
            config.ByProcess = configInfo.ByProcess;
            config.BySpecification = configInfo.BySpecification;
            config.ByVirtualPart = configInfo.ByVirtualPart;
            RF.Save(config);
        }
    }
}
