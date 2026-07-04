using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys;
using SIE.Utils;
using System;

namespace SIE.Wpf.Tech.Helpers
{
    /// <summary>
    /// 模块帮助类
    /// </summary>
    public class ModelHelper
    {
        /// <summary>
        /// 转换为活动模型
        /// </summary>
        /// <param name="process">工序</param>
        /// <returns>活动模型</returns>
        public ActivityModel ConvertToActivity(Process process)
        {
            var activity = new ActivityModel()
            {
                ProcessId = process.Id,
                IsSelected = true,
                Text = process.Name,
                State = ElementState.New,
                Type = ActivityType.Interaction,
                ProcessType = (ProcessType)process.Type,
            };

            foreach (var parameter in process.ParameterList)
            {
                var rule = new RuleModel();
                rule.Text = parameter.Type == ResultTypeForDesign.Custom ? parameter.Description : EnumViewModel.EnumToLabel(parameter.Type).L10N();
                rule.ParameterId = parameter.Id;
                rule.ParamResultType = parameter.Type;
                rule.Expression = parameter.Script;
                rule.SourceActivityId = activity.Id;
                activity.Rules.Add(rule);
            }

            return activity;
        }

        /// <summary>
        /// 转换为容器
        /// </summary>
        /// <param name="version">工艺路线版本</param>
        /// <returns>工艺路线设置容器</returns>
        public ContainerModel ConvertToContainer(RoutingVersion version)
        {
            return new ContainerModel()
            {
                State = ElementState.New,
            };
        }
    }
}