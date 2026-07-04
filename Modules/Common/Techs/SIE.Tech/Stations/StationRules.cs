using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.ComponentModel;
using System.Linq;

namespace SIE.Tech.Stations
{
    /// <summary>
    /// 企业模型删除验证规则，被企业模型引用的不能删除
    /// </summary>
    [DisplayName("企业模型删除验证规则")]
    [Description("被工位引用的不能删除")]
    public class StationRefResourceRule : NoReferencedRule<WipResource>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationRefResourceRule()
        {
            Properties.Add(Station.ResourceIdProperty);
        }
    }

    /// <summary>
    /// 工序删除验证规则，被工位引用的不能删除
    /// </summary>
    [DisplayName("工序删除验证规则")]
    [Description("被工位引用的工序不能删除")]
    public class StationRefProcessRule : NoReferencedRule<Process>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationRefProcessRule()
        {
            Properties.Add(StationProcess.ProcessIdProperty);
        }
    }

    #region 工步删除验证
    /// <summary>
    /// 工步删除验证
    /// </summary>
    [System.ComponentModel.DisplayName("工步删除验证规则")]
    [System.ComponentModel.Description("被工位工序引用的工步不能删除")]
    public class StationRefWorkStep : NoReferencedRule<WorkStep>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationRefWorkStep()
        {
            Properties.Add(StationProcess.WorkStepIdProperty);
        }
    }

    #endregion
    /// <summary>
    /// 工位物料的新增、修改验证
    /// </summary>
    [DisplayName("工位物料新增修改验证规则")]
    [Description("工位物料新增修改验证规则")]
    public class StationItemAddUpdateRule : EntityRule<StationItem>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationItemAddUpdateRule()
        {
            ConnectToDataSource = true;
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">工位物料</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var curStationItem = entity as StationItem;
            if (curStationItem.Warning >= curStationItem.Capacity)
            {
                e.BrokenDescription = "预警值需小于容量值, 请修改后再保存!".L10N();
            }
        }
    }

    /// <summary>
    /// 工位物料不能添加相同物料
    /// </summary>
    [DisplayName("工位物料验证规则")]
    [Description("工位物料不能重复")]
    public class ItemUnitNotDuplicate : NotDuplicateRule<StationItem>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemUnitNotDuplicate()
        {
            Properties.Add(StationItem.StationIdProperty);
            Properties.Add(StationItem.ItemIdProperty);
            MessageBuilder = (e) =>
            {
                var stationItem = e as StationItem;
                return "工位[{0}]已存在物料[{1}]，不能重复添加".L10nFormat(stationItem.Station?.Name, stationItem.Item?.Name);
            };
        }
    }

    /// <summary>
    /// 工位工序不能添加相同工序
    /// </summary>
    [DisplayName("工位工序验证规则")]
    [Description("工位工序不能重复")]
    public class StationProcessNotDuplicate : NotDuplicateRule<StationProcess>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationProcessNotDuplicate()
        {
            Properties.Add(StationProcess.StationIdProperty);
            Properties.Add(StationProcess.ProcessIdProperty);
            MessageBuilder = (e) =>
            {
                var stationProcess = e as StationProcess;
                return "工位[{0}]已存在工序[{1}]，不能重复添加".L10nFormat(stationProcess.Station?.Name, stationProcess.Process?.Name);
            };
        }
    }

    /// <summary>
    /// 工位的新增、修改验证
    /// </summary>
    [DisplayName("工位新增修改验证规则")]
    [Description("工位新增修改验证规则")]
    public class StationProcessAddUpdateRule : EntityRule<StationProcess>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationProcessAddUpdateRule()
        {
            ConnectToDataSource = true;
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">工位</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var stationProcess = entity as StationProcess;
            if (stationProcess.WorkStepId == null || stationProcess.WorkStepId <= 0)
            {
                // 如果工步存在，则需要填写工步信息
                bool isExists = RT.Service.Resolve<SIE.Tech.Processs.ProcessController>()
                    .GetWorkSteps(stationProcess.ProcessId).Count > 0;
                if (isExists)
                {
                    e.BrokenDescription = "当前工序【{0}】存在工步信息，请选择工步!".L10nFormat(stationProcess.Process.Name);
                }
            }
        }
    }

    #region 工位设备验证规则

    /// <summary>
    /// 工位设备不能添加相同工序
    /// </summary>
    [DisplayName("工位设备验证规则")]
    [Description("工位设备不能重复")]
    public class StationEquipmentNotDuplicate : NotDuplicateRule<StationEquipment>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationEquipmentNotDuplicate()
        {
            Properties.Add(StationEquipment.StationIdProperty);
            Properties.Add(StationEquipment.EquipAccountIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as StationEquipment;
                return "工位[{0}]已存在设备[{1}]，不能重复添加".L10nFormat(entity.Station?.Name, entity.EquipAccount?.Name);
            };
        }
    }

    #endregion
}