
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Tech.Processs;
using SIE.Tech.Routings.Technologys;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SIE.Items;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工艺路线版本提交前事件
    /// </summary>
    [DisplayName("工艺路线版本提交前事件")]
    [Description("")]
    public class RoutingVersionSubmitting : OnSubmitting<RoutingVersion>
    {

        /// <summary>
        /// 执行事件操作
        /// </summary>
        /// <param name="entity">工艺路线版本</param>
        /// <param name="e">参数</param>
        protected override void Invoke(RoutingVersion entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                InsertAction(entity);
            }
            else if (e.Action == SubmitAction.Update)
            {
                var oldValue = RF.Find<RoutingVersion>().GetById(entity.Id) as RoutingVersion;
                UpdateAction(oldValue, entity);
            }
            else if (e.Action == SubmitAction.Delete)
            {
                DeleteAction(entity);
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 新增工艺路线版本
        /// </summary>
        /// <param name="version">工艺路线版本</param>
        void InsertAction(RoutingVersion version)
        {
            if (version.Routing != null)
            {
                if (version.Name.Length <= 0)
                {
                    version.Name = RT.Service.Resolve<RoutingController>().GetRoutingVersion(version.RoutingId);
                }

                if (version.IsDefault == YesNo.Yes)
                {
                    version.Routing.DefaultVersionId = version.Id;
                }
            }
        }

        /// <summary>
        /// 修改工艺路线版本
        /// </summary>
        /// <param name="oldVersion">旧工艺路线版本</param>
        /// <param name="newVersion">新工艺路线版本</param>
        void UpdateAction(RoutingVersion oldVersion, RoutingVersion newVersion)
        {
            if (oldVersion.State == RoutingState.Save && newVersion.State == RoutingState.Release && newVersion.Routing != null)
            {
                newVersion.IsDefault = YesNo.Yes;
                SetReleaseVersion(newVersion);
            }

            if (newVersion.IsDefault == YesNo.Yes && newVersion.Routing != null)
            {
                SetIsDefaultVersion(newVersion);
            }
        }

        /// <summary>
        /// 发布工艺路线版本
        /// </summary>
        /// <param name="version">工艺路线版本</param>
        void SetReleaseVersion(RoutingVersion version)
        {
            if (version.Layout == null || version.Layout.Layout.IsNullOrWhiteSpace())
            {
                throw new ValidationException("Layout 不能为空".L10N());
            }

            version.EffectiveDate = DateTime.Now;
            var container = new ContainerModel();
            container.Deserialize(version.Layout.Layout);
            var tipMsg = "";
            var checkResult = CheckProcessParameter(container, out tipMsg);
            if (!checkResult)
            {
                throw new ValidationException(tipMsg.L10N());
            }


            var routingProcesss = new EntityList<RoutingProcess>();
            bool isPassRate = false;
            var isPassRateProcess = new StringBuilder();
            //记录开始工序的Activity 可存在多个开始的第一个工序
            List<IActivity> startActivityList = new List<IActivity>();
            EntityList<RoutingProcessFixture> fixtures = new EntityList<RoutingProcessFixture>();
            CreateRoutingProcess(version, container, routingProcesss, ref isPassRate, isPassRateProcess, startActivityList, fixtures);

            RF.Save(routingProcesss);
            RF.Save(fixtures);
            foreach (var activity in container.Activitys.Where(p => p.Type == ActivityType.Interaction))
            {
                var routingProcess = routingProcesss.FirstOrDefault(p => p.ActivityId == activity.Id);
                foreach (var rule in activity.BeginRules)
                {
                    var processParameter = RF.Find<ProcessParameter>().GetById(rule.ParameterId) as ProcessParameter;
                    var routingProcessParameter = new RoutingProcessParameter()
                    {
                        Description = rule.Text,
                        RoutingProcess = routingProcess,
                        Type = processParameter.Type,
                        Expression = processParameter.Script,
                        RuleId = rule.Id,
                    };
                    if (rule.EndActivity != null)
                    {
                        routingProcessParameter.NextProcess = routingProcesss.FirstOrDefault(p => p.ActivityId == rule.EndActivity.Id);
                    }

                    RF.Save(routingProcessParameter);
                }
            }
            if (startActivityList.Any())
            {
                for (int i = 0; i < startActivityList.Count; i++)
                {
                    var startActivity = startActivityList[i];
                    bool isWhile = true;
                    while (isWhile)
                    {
                        var routingProcess = routingProcesss.FirstOrDefault(p => p.ActivityId == startActivity.Id);

                        var rule = startActivity.BeginRules.FirstOrDefault(p => p.Text == "任意" || p.Text == "通过");
                        if (rule != null && rule.EndActivity.Id.IsNotEmpty() && routingProcess.IsOptional)
                        {
                            var nextProcess = routingProcesss.FirstOrDefault(p => p.ActivityId == rule.EndActivity.Id);
                            if (nextProcess.ProcessSign == RoutingProcessSign.Normal)
                            {
                                nextProcess.ProcessSign = RoutingProcessSign.Start;
                            }
                            else if (nextProcess.ProcessSign == RoutingProcessSign.End)
                            {
                                nextProcess.ProcessSign |= RoutingProcessSign.Start;
                            }
                            else
                            {
                                //
                            }

                            RF.Save(nextProcess);
                            if (!nextProcess.IsOptional)
                            {
                                ////isWhile = false;
                                break;
                            }
                            //控制取值从下一节点开始
                            startActivity = container.Activitys.FirstOrDefault(p => p.Id == nextProcess.ActivityId);
                            if (startActivity == null)
                            {
                                isWhile = false;
                            }
                        }
                        else
                        {
                            isWhile = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 检查工序参数是否有更新 
        /// </summary>
        /// <param name="containerModel">内容保存</param>
        /// <param name="tipMsg"></param>
        /// <returns></returns>
        private bool CheckProcessParameter(ContainerModel containerModel, out string tipMsg)
        {
            tipMsg = "";
            StringBuilder msgBuilder = new StringBuilder();
            var result = true;
            const string msgTlp = "工序{0}的工序参数已调整,请移除后重新添加";
            const string processNameTlp = "[{0}]、";
            if (!containerModel.Activitys.Any())
            {
                return result;
            }
            var processCtl = RT.Service.Resolve<ProcessController>();
            var processIds = containerModel.Activitys.Where(activity => (activity is ActivityModel) && !activity.IsGroup && activity.ProcessId > 0).Select(p => p.ProcessId).ToList();
            var dicProcessParameter = processCtl.GetProcessParameterByProcessIds(processIds);//获取所有工序的工序参数

            foreach (var activityModel in containerModel.Activitys)
            {
                if (activityModel != null && !activityModel.IsGroup && activityModel.ProcessId > 0)
                {
                    //获取最新的工序参数
                    List<ProcessParameter> parameters;
                    dicProcessParameter.TryGetValue(activityModel.ProcessId, out parameters);
                    if (activityModel.ProcessParameter.Count != parameters.Count)
                    {
                        msgBuilder.Append(processNameTlp.L10nFormat(activityModel.Text));
                        result = false;
                    }
                    //个数相等判断Id是否互相能找到
                    else
                    {
                        foreach (var processParameter in parameters)
                        {
                            var param = activityModel.ProcessParameter.FirstOrDefault(m => m.Id == processParameter.Id);
                            //旧参数中无法找到新的参数 则要求更新，或者类型变化也要求更新
                            if (param == null || param.Type != processParameter.Type)
                            {
                                msgBuilder.Append(processNameTlp.L10nFormat(activityModel.Text));
                                result = false;
                                break;
                            }
                        }
                    }
                }
            }

            tipMsg = msgTlp.L10nFormat(msgBuilder.ToString().TrimEnd('、'));
            return result;
        }



        /// <summary>
        /// 创建工序清单
        /// </summary>
        /// <param name="version"></param>
        /// <param name="container"></param>
        /// <param name="routingProcesss"></param>
        /// <param name="isPassRate"></param>
        /// <param name="isPassRateProcess"></param>
        /// <param name="startActivities"></param>
        /// <param name="fixtures"></param>
        private static void CreateRoutingProcess(RoutingVersion version, ContainerModel container, EntityList<RoutingProcess> routingProcesss, ref bool isPassRate, StringBuilder isPassRateProcess, List<IActivity> startActivities, EntityList<RoutingProcessFixture> fixtures)
        {
            foreach (var activity in container.Activitys.Where(p => p.Type == ActivityType.Interaction))
            {
                var isGroup = activity.IsGroup;
                var process = RF.Find<Process>().GetById(activity.ProcessId) as Process;
                if (process == null && !isGroup)
                {
                    throw new ValidationException("工序：{0} 不存在".L10nFormat(activity.Text));
                }
                if (isPassRate && activity.IsPassRate)
                {
                    isPassRateProcess.Append(activity.Text);
                    throw new ValidationException("直通率工序【{0}】最多只能勾选一个".L10nFormat(isPassRateProcess));
                }
                if (activity.IsPassRate)
                {
                    isPassRate = true;
                    isPassRateProcess.Append(activity.Text);
                }
                RoutingProcess routingProcess = SetRoutingProcessValue(version, activity, process);

                routingProcess.GenerateId();

                var beginRule = activity.EndRules.FirstOrDefault(p => p.BeginActivity.Type == ActivityType.Initial);
                if (beginRule != null)
                {
                    routingProcess.ProcessSign = RoutingProcessSign.Start;
                    startActivities.Add(activity);
                }

                var endRule = activity.BeginRules.FirstOrDefault(p => p.EndActivity.Type == ActivityType.Completion);
                if (endRule != null)
                {
                    routingProcess.ProcessSign |= RoutingProcessSign.End;
                }

                if (beginRule == null && endRule == null)
                {
                    routingProcess.ProcessSign = RoutingProcessSign.Normal;
                }

                foreach (var item in activity.Bom)
                {
                    var routingProcessBomConfig = new RoutingProcessBomConfig()
                    {
                        Item = RF.GetById<Item>(item.ItemId),
                        RoutingProcess = routingProcess,
                        ItemExtProp = item.ItemExtProp,
                        ItemExtPropName=item.ItemExtPropName,

                    };
                    routingProcess.BomConfigList.Add(routingProcessBomConfig);
                }

                foreach (var item in activity.Fixtures)
                {
                    fixtures.Add(new RoutingProcessFixture()
                    {
                        FixtureId = item.Id,
                        RoutingProcess = routingProcess,
                    });
                }

                if (process != null)
                {
                    foreach (var step in process.CollectStepList)
                    {
                        var routingProcessCollectStep = new RoutingProcessCollectStep()
                        {
                            IsUnbound = step.IsUnbound,
                            RoutingProcess = routingProcess,
                            Step = step.Step,
                        };
                        routingProcess.CollectStepList.Add(routingProcessCollectStep);
                    }

                    foreach (var defect in process.DefectList)
                    {
                        var routingProcessDefect = new RoutingProcessDefect()
                        {
                            Defect = defect.Defect,
                            RoutingProcess = routingProcess,
                        };
                        routingProcess.DefectList.Add(routingProcessDefect);
                    }

                    process.ReferenceTimes += 1;
                    RF.Save(process);
                }
                routingProcesss.Add(routingProcess);
            }
        }

        /// <summary>
        /// 工序清单赋值
        /// </summary>
        /// <param name="version"></param>
        /// <param name="activity"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        private static RoutingProcess SetRoutingProcessValue(RoutingVersion version, IActivity activity, Process process)
        {
            var result = new RoutingProcess()
            {
                ActivityId = activity.Id,
                LayoutInfoId = activity.LayoutInfoId,
                Vornr = activity.Vornr,
                Steus = activity.Steus,
                Version = version,
                IsOptional = activity.IsOptional,
                IsRepeat = activity.IsRepeat,
                CreateSku = activity.CreateSku,
                IsCalculate = activity.IsCalculate,
                IsGenerateTask = activity.IsGenerateTask,
                IsRequirementTask= activity.IsRequirementTask,
                IsBuckleMaterial = activity.IsBuckleMaterial,
                IsPassRate = activity.IsPassRate,
                IsBinding = activity.IsBinding,
                IsUnBinding = activity.IsUnBinding,
                StartProcess = activity.StartProcess,
                NormalVictoryId = activity.NormalVictory,
                RepairVictoryId = activity.RepairVictory,
                IsStricter = activity.IsStricter,
                Overtime = activity.Overtime,
                Index = activity.Index,
                Name = activity.Text,
                MaxPassNum = activity.MaxPassNum,
                IsNextMoveIn= activity.IsNextMoveIn,
                EnableMoveInControl = activity.EnableMoveInControl,
                TransferType = activity.TransferType,
                ParentNodeId = activity.ParentNodeId,
            };

            if (process != null)
            {
                result.Name = process.Name;
                result.Process = process;
                result.ProcessSegmentId = process.SegmentId;
                result.Type = (ProcessType)process.Type;
            }

            result.IsGroup = activity.IsGroup;
            result.GroupId = activity.GroupId;
            result.Outsourcing = activity.IsOutsourcing;
            return result;
        }

        /// <summary>
        /// 删除工艺路线版本
        /// </summary>
        /// <param name="version">工艺路线版本</param>
        void DeleteAction(RoutingVersion version)
        {
            if (version != null && version.Layout != null)
            {
                ////更新工序引用次数
                var container = new ContainerModel();
                container.Deserialize(version.Layout.Layout);
                foreach (var activity in container.Activitys.Where(p => p.Type == ActivityType.Interaction))
                {
                    var process = RF.Find<Process>().GetById(activity.ProcessId) as Process;
                    if (process != null)
                    {
                        process.ReferenceTimes -= 1;
                        RF.Save(process);
                    }
                }

                version.Layout.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(version.Layout);
            }
        }

        /// <summary>
        /// 设置默认工艺路线版本
        /// </summary>
        /// <param name="newVersion">工艺路线版本</param>
        void SetIsDefaultVersion(RoutingVersion newVersion)
        {
            var version = newVersion.Routing.VersionList.FirstOrDefault(p => p.IsDefault == YesNo.Yes && p.Id != newVersion.Id);
            if (version != null)
            {
                newVersion.IsDefault = YesNo.No;
            }
            else
            {
                newVersion.Routing.DefaultVersionId = newVersion.Id;
                RF.Save(newVersion.Routing);
            }
        }
    }
}
