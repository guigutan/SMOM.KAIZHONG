using Microsoft.Scripting.Utils;
using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.RoutingSettings;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.Packages.Packings;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品工艺路线控制器
    /// </summary>
    public class WipProductRoutingController : DomainController
    {
        /// <summary>
        /// 获取工单的工艺路线
        /// </summary>
        /// <param name="workOrderId">产品版本Id</param>
        /// <returns>产品工艺路线</returns>
        public virtual string GetWorkOrderLayout(double workOrderId)
        {
            using (DataAuth.DataAuths.LoadAll())
            {
                var wo = RF.GetById<WorkOrder>(workOrderId);

                if (wo != null)
                {
                    return wo.Layout?.Layout;
                }
                else
                {
                    throw new ValidationException("工单信息找不到".L10N());
                }
            }
        }

        /// <summary>
        /// 获取产品工艺路线
        /// </summary>
        /// <param name="versionId">产品版本Id</param>
        /// <returns>产品工艺路线</returns>
        public virtual WipProductRouting GetWipProductRouting(double versionId)
        {
            return Query<WipProductRouting>().Where(p => p.VersionId == versionId).FirstOrDefault();
        }

        /// <summary>
        /// 获取产品工艺路线修改事件
        /// </summary>
        /// <param name="versionId">产品版本Id</param>
        /// <returns>产品工艺路线变更事件列表</returns>
        public virtual EntityList<WipProductRoutingEvent> GetWipProductRoutingEvents(double versionId)
        {
            return Query<WipProductRoutingEvent>().Where(p => p.Routing.VersionId == versionId).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 暂停产品工艺路线
        /// </summary>
        /// <param name="versionId">产品版本ID</param>
        /// <param name="oldLayout">工艺路线旧布局</param>
        /// <param name="newLayout">工艺路线新布局</param>
        public virtual void PauseProductRouting(double versionId, string oldLayout, string newLayout)
        {
            var version = ValidationWipProductVersion(versionId, YesNo.Yes);
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                version.IsPause = YesNo.Yes;
                RF.Save(version);
                SaveProductRoutingInfo(version, oldLayout, newLayout, "暂停");
                tran.Complete();
            }
        }

        /// <summary>
        /// 启用产品工艺路线
        /// </summary>
        /// <param name="versionId">产品版本Id</param>
        /// <param name="oldLayout">旧布局</param>
        /// <param name="newLayout">新布局</param>
        public virtual void EnableProductRouting(double versionId, string oldLayout, string newLayout)
        {
            var version = ValidationWipProductVersion(versionId);
            var product = ValidationProduct(version);
            IContainer container = new ContainerModel();
            container.Deserialize(newLayout);
            container.ValidateSave();
            var currents = container.Activitys.Where(p => p.ProcessState == ProcessState.Current);
            if (currents.Count() != 1)
                throw new ValidationException("只可以有一个当前工序，当前工序数量：{0}".L10nFormat(currents.Count()));
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var current = currents.FirstOrDefault();
                product.Routing.CurrentId = current == null ? 0 : current.ProcessId;
                GenerateRouting(container, product);
                RT.Service.Resolve<RuntimeController>().Save(product);
                SaveProductRoutingInfo(version, oldLayout, newLayout, "启用");
                version.IsPause = YesNo.No;
                RF.Save(version);
                tran.Complete();
            }
        }

        /// <summary>
        /// 生成工艺路线数据
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="product">产品</param>
        void GenerateRouting(IContainer container, product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            product.Routing.Processes.Clear();
            bool isPassRate = false;
            StringBuilder sb = new StringBuilder();

            //工单工艺路线工序
            var workOrderRoutingProcesses = RT.Service.Resolve<WorkOrderController>().GetRoutingProcess(product.WorkOrderId);

            foreach (var activity in container.Activitys.Where(p => p.Type == ActivityType.Interaction))
            {
                var process = RF.GetById<Process>(activity.ProcessId);

                if (process == null && !activity.IsGroup)//工序组不提示
                {
                    throw new ValidationException("工序：{0} 不存在".L10nFormat(activity.Text));
                }

                if (isPassRate && activity.IsPassRate)
                {
                    sb.Append(activity.Text);
                    throw new ValidationException("直通率工序【{0}】最多只能勾选一个".L10nFormat(sb.ToString()));
                }

                if (activity.IsPassRate)
                {
                    isPassRate = true;
                    sb.Append(activity.Text);
                    sb.Append(",");
                }

                var workOrderRoutingProcess = workOrderRoutingProcesses.FirstOrDefault(x => x.ActivityId == activity.Id);

                //生成工序
                process runtimeProcess = CrearteProcess(activity, process, workOrderRoutingProcess);
                var beginRule = activity.EndRules.FirstOrDefault(p => p.BeginActivity.Type == ActivityType.Initial);
                if (beginRule != null)
                {
                    runtimeProcess.Sign = RoutingProcessSign.Start;
                }

                var endRule = activity.BeginRules.FirstOrDefault(p => p.EndActivity.Type == ActivityType.Completion);
                if (endRule != null)
                {
                    runtimeProcess.Sign |= RoutingProcessSign.End;
                }

                if (beginRule == null && endRule == null)
                {
                    runtimeProcess.Sign = RoutingProcessSign.Normal;
                }

                SetNextProcess(activity, runtimeProcess, workOrderRoutingProcesses);

                if (activity.ProcessState == ProcessState.Current)
                {
                    //设置当前工序ID
                    product.Routing.CurrentId = runtimeProcess.Id;
                    product.Routing.Next.Clear();
                    product.Routing.Next.AddRange(runtimeProcess.Next.SelectMany(p => p.Value));
                    if (!product.Routing.Next.Any())
                    {
                        throw new ValidationException("后工序：{0} 不能是当前工序".L10nFormat(activity.Text));
                    }
                }

                BuildProcessBom(activity, runtimeProcess);

                product.Routing.Processes.Add(runtimeProcess);
            }

            //假如设置的【当前工序】是开始节点，刚将所有开始工序加入到下工序列表中
            var beginActivity = container.Activitys
                .FirstOrDefault(p => p.Type == ActivityType.Initial && p.ProcessState == ProcessState.Current);

            if (beginActivity != null)
            {
                product.Routing.Next.Clear();

                foreach (var rule in beginActivity.BeginRules)
                {
                    var activity = rule.EndActivity;
                    double nextProcessId = activity.Index;
                    var workOrderRoutingProcess = workOrderRoutingProcesses.FirstOrDefault(x => x.ActivityId == activity.Id);
                    if (workOrderRoutingProcess != null)
                    {
                        nextProcessId = workOrderRoutingProcess.Id;
                    }

                    product.Routing.Next.Add(nextProcessId);
                }
            }
            else
            {
                //当前工序不是开始节点【非实际工序】，直接计算下工序
                RT.Service.Resolve<WipController>().ComputeNextProcess(product, ResultType.Pass, null);
            }
        }

        /// <summary>
        /// Set Next Process
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="runtimeProcess"></param>
        /// <param name="workOrderRoutingProcesses">工单工艺路线工序清单</param>
        private static void SetNextProcess(IActivity activity, process runtimeProcess, EntityList<WorkOrderRoutingProcess> workOrderRoutingProcesses)
        {
            foreach (var rule in activity.BeginRules)
            {
                if (rule.EndActivity == null || rule.EndActivity.Type != ActivityType.Interaction)
                {
                    continue;
                }

                var resultType = rule.ParamResultType;

                var workOrderRoutingProcessOfNext = workOrderRoutingProcesses
                    .FirstOrDefault(x => x.ActivityId == rule.EndActivity.Id);

                double nextRoutingProcessId = rule.EndActivity.Index;

                if (workOrderRoutingProcessOfNext != null)
                {
                    nextRoutingProcessId = workOrderRoutingProcessOfNext.Id;
                }

                if (((ResultType)resultType & ResultType.Pass) == ResultType.Pass)
                {
                    if (!runtimeProcess.Next.ContainsKey(ResultType.Pass))
                    {
                        runtimeProcess.Next.Add(ResultType.Pass, new List<double>() { nextRoutingProcessId });
                    }
                    else
                    {
                        var nextList = runtimeProcess.Next[ResultType.Pass];
                        if (nextList == null)
                        {
                            nextList = new List<double>();
                        }

                        nextList.Add(nextRoutingProcessId);
                    }
                }

                if (((ResultType)resultType & ResultType.Fail) == ResultType.Fail)
                {
                    if (!runtimeProcess.Next.ContainsKey(ResultType.Fail))
                    {
                        runtimeProcess.Next.Add(ResultType.Fail, new List<double>() { nextRoutingProcessId });
                    }
                    else
                    {
                        var nextList = runtimeProcess.Next[ResultType.Fail];
                        if (nextList == null)
                        {
                            nextList = new List<double>();
                        }

                        nextList.Add(nextRoutingProcessId);
                    }
                }


                if (((ResultType)resultType & ResultType.Custom) == ResultType.Custom)
                {
                    if (runtimeProcess.Next.ContainsKey(ResultType.Custom))
                    {
                        runtimeProcess.Next[ResultType.Custom].Add(nextRoutingProcessId);
                    }
                    else
                    {
                        runtimeProcess.Next.Add(ResultType.Custom, new List<double>() { nextRoutingProcessId });
                    }
                }

                if (((ResultType)resultType & ResultType.Optional) == ResultType.Optional)
                {
                    if (runtimeProcess.Next.ContainsKey(ResultType.Optional))
                    {
                        runtimeProcess.Next[ResultType.Optional].Add(nextRoutingProcessId);
                    }
                    else
                    {
                        runtimeProcess.Next.Add(ResultType.Optional, new List<double>() { nextRoutingProcessId });
                    }

                    if (!runtimeProcess.OptionalPathDictionary.ContainsKey(nextRoutingProcessId))
                    {
                        runtimeProcess.OptionalPathDictionary.Add(nextRoutingProcessId, rule.Text);
                    }
                }
            }
        }

        /// <summary>
        /// 生成工序bom
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="runtimeProcess"></param>
        private static void BuildProcessBom(IActivity activity, process runtimeProcess)
        {
            foreach (var processBom in activity.ProcessBoms)
            {
                if (processBom.Qty <= 0)
                {
                    throw new ValidationException("工序：{0} 的BOM数量必须大于 0".L10nFormat(activity.Text));
                }

                if (processBom.MainMaterialId == processBom.ItemId)
                {
                    throw new ValidationException("工序：{0} 的主物料编码不与能物料编码相同".L10nFormat(activity.Text));
                }

                runtimeProcess.Boms.Add(new bom()
                {
                    ItemId = processBom.ItemId,
                    Qty = processBom.Qty,
                    IsBuckleMaterial = processBom.IsBuckleMaterial,
                    Point = processBom.Point,
                    WorkStepId = processBom.WorkStepId,
                    IsAttachment = processBom.IsAttachment,
                    IsExternal = processBom.IsExternal,
                    IsSingleLabel = processBom.IsSingleLabel,
                    IsRepeat = processBom.IsRepeat,
                    HasBarcodeRule = processBom.HasBarcodeRule,
                    MainMaterialId = processBom.MainMaterialId,
                    ItemExtProp = processBom.ItemExtProp,
                    ItemExtPropName = processBom.ItemExtPropName
                });
            }
        }

        /// <summary>
        /// 生成工序
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="process"></param>
        /// <param name="workOrderRoutingProcess">工单工艺路线工序</param>
        /// <returns></returns>
        private static process CrearteProcess(IActivity activity, Process process, WorkOrderRoutingProcess workOrderRoutingProcess)
        {
            var result = new process()
            {
                ProcessId = activity.ProcessId,
                Optional = activity.IsOptional,
                Repeat = activity.IsRepeat,
                CreateSku = activity.CreateSku,
                IsCalculate = activity.IsCalculate,
                IsGenerateTask = activity.IsGenerateTask,
                IsRequirementTask = activity.IsRequirementTask,
                IsBuckleMaterial = activity.IsBuckleMaterial,
                IsPassRate = activity.IsPassRate,
                IsBinding = activity.IsBinding,
                IsUnBinding = activity.IsUnBinding,
                Overtime = activity.Overtime,
                StartProcess = activity.StartProcess,
                NormalVictory = activity.NormalVictory,
                RepairVictory = activity.RepairVictory,
                IsStricter = activity.IsStricter,
                Index = activity.Index,

                MaxPassNum = activity.MaxPassNum,
                EnableMoveInControl = activity.EnableMoveInControl,
                TransferType = activity.TransferType,
                ParentNodeId = activity.ParentNodeId,
                IsNextMoveIn = activity.IsNextMoveIn,
            };

            if (workOrderRoutingProcess != null)
            {
                result.Id = workOrderRoutingProcess.Id;
            }
            else
            {
                result.Id = activity.Index;
            }

            if (process != null)
            {
                result.Name = process.Name;
                result.Type = (ProcessType)process.Type;
            }
            else
            {
                result.Name = activity.Text;
            }

            result.IsGroup = activity.IsGroup;
            result.GroupId = activity.GroupId;
            return result;
        }

        /// <summary>
        /// 验证产品生产版本
        /// </summary>
        /// <param name="versionId">产品生产版本Id</param>
        /// <param name="isPause">是否暂停</param>
        /// <returns>产品生产版本</returns>
        WipProductVersion ValidationWipProductVersion(double versionId, YesNo isPause = YesNo.No)
        {
            var version = GetById<WipProductVersion>(versionId);
            if (version == null)
            {
                throw new EntityNotFoundException(typeof(WipProductVersion), versionId);
            }

            if (version.IsFinish)
            {
                throw new ValidationException("产品：{0} 已完工下线".L10nFormat(version.Sn));
            }

            if (version.IsPause == isPause)
            {
                if (isPause == YesNo.No)
                {
                    throw new ValidationException("产品：{0} 不是暂停状态".L10nFormat(version.Sn));
                }
                else
                {
                    throw new ValidationException("产品：{0} 已经是暂停状态".L10nFormat(version.Sn));
                }
            }

            return version;
        }

        /// <summary>
        /// 验证运行时产品
        /// </summary>
        /// <param name="version">产品生产版本</param>
        /// <returns>运行时产品</returns>
        product ValidationProduct(WipProductVersion version)
        {
            var product = RT.Service.Resolve<RuntimeController>().FindProduct(version.Product.Puid);
            if (product == null)
            {
                throw new ValidationException("未找到产品：{0} 的运行时数据".L10nFormat(version.Sn));
            }

            return product;
        }

        /// <summary>
        /// 保存产品工艺路线
        /// </summary>
        /// <param name="versionId">产品版本Id</param>
        /// <param name="oldLayout">旧布局</param>
        /// <param name="newLayout">新布局</param>
        public virtual void SaveProductRouting(double versionId, string oldLayout, string newLayout)
        {
            var version = ValidationWipProductVersion(versionId);
            var product = ValidationProduct(version);
            IContainer container = new ContainerModel();
            container.Deserialize(newLayout);
            var currents = container.Activitys.Where(p => p.ProcessState == ProcessState.Current);
            var currentCount = currents.Count();
            if (currentCount > 1)//不允许存在多个
            {
                throw new ValidationException("只可以有一个当前工序，当前工序数量：{0}".L10nFormat(currentCount));
            }
            //如果界面没有设置当前工序则去版本的取
            if (currentCount < 1 && version.CurrenrProcessIndex.HasValue)
            {
                var currentProcessActivityIndex = container.Activitys.FindIndex(p => p.Index == (int)version.CurrenrProcessIndex);
                if (currentProcessActivityIndex < 0)//如果版本的取不到也报错
                {
                    throw new ValidationException("只可以有一个当前工序，当前工序数量：{0}".L10nFormat(currentCount));
                }
                container.Activitys[currentProcessActivityIndex].ProcessState = ProcessState.Current;
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var current = currents.FirstOrDefault();
                product.Routing.CurrentId = current == null ? 0 : current.ProcessId;
                GenerateRouting(container, product);
                RT.Service.Resolve<RuntimeController>().Save(product);
                SaveNowAndNextProcess(current.ProcessId, product, version);
                SaveProductRoutingInfo(version, oldLayout, newLayout, "保存");
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存并计算下一工序
        /// </summary>
        /// <param name="currentProIds"></param>
        /// <param name="product"></param>
        /// <param name="version"></param>
        private void SaveNowAndNextProcess(double currentProIds, product product, WipProductVersion version)
        {
            // 更新当前工序
            version.NowProcessId = currentProIds;

            //获取工艺路线下一工序
            var nexts = product.Routing.GetNext();
            var nextProcess = nexts.FirstOrDefault(p => !p.Optional && p.ProcessId != currentProIds);
            if (nextProcess == null)
            {
                nextProcess = nexts.FirstOrDefault(p => !p.Optional && p.ProcessId == currentProIds);
            }
            if (!version.IsFinish)
            {
                version.CurrenrProcessIndex = product.Routing.Current.Index;
                version.NextProcessId = nextProcess?.ProcessId;
                version.NextProcessIndex = nextProcess?.Index;
            }
            RF.Save(version);
        }

        /// <summary>
        /// 保存产品工艺路线数据
        /// </summary>
        /// <param name="version">产品版本</param>
        /// <param name="oldLayout">旧布局</param>
        /// <param name="newLayout">新布局</param>
        /// <param name="remark">备注</param>
        internal virtual void SaveProductRoutingInfo(WipProductVersion version, string oldLayout, string newLayout, string remark)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var wipProductRouting = RT.Service.Resolve<WipProductRoutingController>().GetWipProductRouting(version.Id);
                if (wipProductRouting == null)
                {
                    var layout = new WipProductRoutingLayout() { Layout = newLayout };
                    RF.Save(layout);
                    wipProductRouting = new WipProductRouting()
                    {
                        Version = version,
                        WorkOrderId = version.WorkOrderId,
                        Layout = layout,
                    };
                    RF.Save(wipProductRouting);
                }
                else
                {
                    wipProductRouting.Layout.Layout = newLayout;
                    RF.Save(wipProductRouting.Layout);
                }

                var wipProductRoutingEvent = new WipProductRoutingEvent()
                {
                    Routing = wipProductRouting,
                    OldLayout = oldLayout,
                    NewLayout = newLayout,
                    ChangeDate = DateTime.Now,
                    ChangeUserId = AppRuntime.IdentityId,
                    Remark = remark.L10N(),
                };
                RF.Save(wipProductRoutingEvent);
                tran.Complete();
            }
        }

        /// <summary>
        /// 初始bom模型
        /// </summary>
        /// <param name="workOrderProcessBom">工单工序BOM</param>        
        /// <returns>bom模型</returns>
        public virtual ProductBomViewModel InitBomViewModesByWorkOrderProcessBom(WorkOrderProcessBom workOrderProcessBom)
        {
            if (workOrderProcessBom == null)
            {
                throw new ArgumentNullException(nameof(workOrderProcessBom));
            }

            var item = workOrderProcessBom.Item;

            var bomView = new ProductBomViewModel()
            {
                ProcessId = workOrderProcessBom.ProcessId,
                Item = item,
                Code = item.Code,
                Name = item.Name,
                Qty = workOrderProcessBom.SingleQty,
                ItemExtProp = workOrderProcessBom.ItemExtProp,
                ItemExtPropName = workOrderProcessBom.ItemExtPropName,
                IsBuckleMaterial = true,
                Id = Guid.NewGuid().ToString(),
                WorkStepId = workOrderProcessBom.WorkStepId,
                Alter = workOrderProcessBom.Alter,
            };

            if (bomView.WorkStepId.HasValue)
            {
                var workStep = workOrderProcessBom.WorkStep;
                bomView.WorkStepName = workStep.Name;
                bomView.WorkStepCode = workStep.Code;
            }

            SetBomExtProperty(bomView, workOrderProcessBom);

            return bomView;
        }


        /// 设置采集运行时工序BOM扩展属性
        /// </summary>
        /// <param name="bom">工序BOM</param>
        /// <param name="workOrderProcessBom">工单BOM</param>
        protected virtual void SetBomExtProperty(ProductBomViewModel bom, WorkOrderProcessBom workOrderProcessBom)
        {


        }
    }
}
