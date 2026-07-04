using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Tech.Processs;
using SIE.Tech.Routings.Technologys;
using SIE.Tech.Routings.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工艺路线控制器
    /// </summary>
    public partial class RoutingController : DomainController
    {
        /// <summary>
        /// 获取所有工艺路线
        /// </summary>
        /// <returns>工艺路线集合</returns>
        public virtual EntityList<Routing> GetRoutings()
        {
            return Query<Routing>().ToList(new PagingInfo { PageNumber = 1, PageSize = int.MaxValue - 1 }, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取所有工艺路线版本
        /// </summary>
        /// <returns>工艺路线版本集合</returns>
        public virtual EntityList<RoutingVersion> GetRoutingVersions()
        {
            return Query<RoutingVersion>().ToList(new PagingInfo { PageNumber = 1, PageSize = int.MaxValue - 1 }, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工艺路线所有版本
        /// </summary>
        /// <returns>工艺路线版本集合</returns>
        public virtual EntityList<RoutingVersion> GetRoutingVersions(double routingId)
        {
            return Query<RoutingVersion>()
                .Where(p => p.RoutingId == routingId && p.State == RoutingState.Release)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工艺路线所有版本
        /// </summary>
        /// <param name="routingId"></param>
        /// <param name="routingState"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="eagerLoadOptions"></param>
        /// <returns></returns>
        public virtual EntityList<RoutingVersion> GetRoutingVersions(double? routingId, RoutingState? routingState, string keyword, PagingInfo pagingInfo = null, EagerLoadOptions eagerLoadOptions = null)
        {
            var q = Query<RoutingVersion>();
            q.WhereIf(routingState.HasValue, p => p.State == routingState);
            q.WhereIf(routingId.HasValue, p => p.RoutingId == routingId);
            q.WhereIf(!string.IsNullOrEmpty(keyword), p => p.Name.Contains(keyword));
            return q.ToList(pagingInfo, eagerLoadOptions);
        }

        /// <summary>
        /// 获取所有工艺路线版本
        /// </summary>
        /// <returns>工艺路线版本集合</returns>
        public virtual EntityList<RoutingVersion> GetRoutingVersions(List<double> routingVersionIds)
        {
            return routingVersionIds.SplitContains(tempIds =>
            {
                return Query<RoutingVersion>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();

            });
        }

        /// <summary>
        /// 根据产品族分类查找工艺路线
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="eagerLoadOptions"></param>
        /// <returns></returns>
        public virtual EntityList<Routing> GetRoutingsByProductFamilyCategory(double? categoryId, string keyword, PagingInfo pagingInfo = null, EagerLoadOptions eagerLoadOptions = null)
        {
            var q = Query<Routing>();
            if (categoryId != null)
                q.Where(p => p.CategoryId == categoryId);
            q.WhereIf(!string.IsNullOrEmpty(keyword), p => p.Name.Contains(keyword));
            return q.ToList(pagingInfo, eagerLoadOptions);
        }

        /// <summary>
        /// 获取工艺路线
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public virtual EntityList<Routing> GetRoutings(PagingInfo pagingInfo, string keyword)
        {
            var q = Query<Routing>().WhereIf(keyword.IsNotEmpty(), p => p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return q;
            
        }

        /// <summary>
        /// 根据产品族分类查找工艺路线版本
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="eagerLoadOptions"></param>
        /// <returns></returns>
        public virtual EntityList<RoutingVersion> GetRoutingVersionsByProductFamilyCategory(double? categoryId, PagingInfo pagingInfo = null, EagerLoadOptions eagerLoadOptions = null)
        {
            var q = Query<RoutingVersion>();
            q.Join<Routing>((v, r) => v.RoutingId == r.Id);
            if (categoryId != null)
                q.Where<Routing>((v, r) => r.CategoryId == categoryId);
            q.Where(p => p.State == RoutingState.Release);
            return q.ToList(pagingInfo, eagerLoadOptions);
        }

        /// <summary>
        /// 获取物料列表
        /// </summary>
        /// <returns>关键物料列表</returns>
        public virtual EntityList<KeyItem> GetKeyItems()
        {
            return Query<KeyItem>().ToList();
        }

        /// <summary>
        /// 获取工艺路线版本号
        /// </summary>
        /// <param name="routingId">工艺路线Id</param>
        /// <returns>工艺路线版本号</returns>
        public virtual string GetRoutingVersion(double routingId)
        {
            var query = Query<RoutingVersion>();
            query = query.Where(e => e.RoutingId == routingId).OrderByDescending(t => t.Id);
            var routingVersion = query.FirstOrDefault();
            if (routingVersion == null)
                return "V0001";
            int number = 0;
            int.TryParse(routingVersion.Name.Substring(1), out number);
            number++;
            return "V{0:D4}".FormatArgs(number);
        }

        /// <summary>
        /// 获取工艺路线版本
        /// </summary>
        /// <param name="routingId">工艺路线Id</param>
        /// <param name="version">版本</param>
        /// <returns>工艺路线版本实体</returns>
        public virtual RoutingVersion GetRoutingVersion(double routingId, string version)
        {
            RoutingVersion rv = Query<RoutingVersion>().Where(e => e.RoutingId == routingId && e.Name == version).FirstOrDefault();
            return rv;
        }

        /// <summary>
        /// 获取工艺路线版本
        /// </summary>
        /// <param name="routingId">工艺路线Id</param>
        /// <param name="versionId">版本Id</param>
        /// <returns>工艺路线版本实体</returns>
        public virtual RoutingVersion GetRoutingVersion(double routingId, double versionId)
        {
            RoutingVersion rv = Query<RoutingVersion>().Where(e => e.RoutingId == routingId && e.Id == versionId).FirstOrDefault();
            return rv;
        }

        /// <summary>
        /// 获取某工艺路线的最大版本号
        /// </summary>
        /// <param name="routingId">工艺路线Id</param>
        /// <returns>最大版本号</returns>
        public virtual int GetMaxVersionNum(double routingId)
        {
            var query = Query<RoutingVersion>();
            query = query.Where(e => e.RoutingId == routingId).OrderByDescending(t => t.Id);
            var routingVersion = query.FirstOrDefault();
            int number = 0;
            if (routingVersion != null)
                int.TryParse(routingVersion.Name.Substring(1), out number);
            return number;
        }

        /// <summary>
        /// 获取缺省工艺路线版本
        /// </summary>
        /// <param name="routingId">工艺路线Id</param>
        /// <returns>工艺路线版本</returns>
        public virtual RoutingVersion GetDefaultRoutingVersion(double routingId)
        {
            var query = Query<RoutingVersion>();
            query = query.Where(e => e.RoutingId == routingId && e.IsDefault == YesNo.Yes).OrderByDescending(t => t.CreateDate);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// 发布工艺路线版本
        /// </summary>
        /// <param name="versionId">工艺路线版本ID</param>
        /// <param name="xml">序列化工艺路线</param>
        public virtual RoutingVersion ReleaseRoutingVersion(double versionId, string xml)
        {
            if (xml.IsNullOrWhiteSpace())
            {
                throw new ValidationException("工艺路线容器不能为空".L10N());
            }

            IContainer container = new ContainerModel();
            container.Deserialize(xml);
            var version = RF.Find<RoutingVersion>().GetById(versionId) as RoutingVersion;
            if (version == null)
            {
                throw new ValidationException("版本不能为空".L10N());
            }

            using (var tran = DB.TransactionScope(TechEntityDataProvider.ConnectionStringName))
            {
                container.ValidateSave();
                version.Layout.Layout = container.Serialize();
                RF.Save(version.Layout);
                version.State = RoutingState.Release;
                RF.Save(version);
                tran.Complete();
            }

            return version;
        }

        /// <summary>
        /// 创建工艺路线
        /// </summary>
        /// <param name="routLayoutMsg">工艺路线信息</param>
        /// <returns>工艺路线版本</returns>
        public virtual RoutingVersion CreateRoutingVersion(RoutingLayoutMsg routLayoutMsg)
        {
            var routingId = routLayoutMsg.RoutingId;
            var xml = routLayoutMsg.Layout;

            if (xml.IsNullOrWhiteSpace())
            {
                throw new ValidationException("工艺路线容器不能为空".L10N());
            }

            IContainer container = new ContainerModel();
            container.Deserialize(xml);
            var routing = RF.Find<Routing>().GetById(routingId) as Routing;
            if (routing == null)
            {
                throw new ValidationException("工艺路线不能为空".L10N());
            }

            using (var tran = DB.TransactionScope(TechEntityDataProvider.ConnectionStringName))
            {
                container.ValidateSave();
                var layout = new RoutingLayout();
                RF.Save(layout);
                var version = new RoutingVersion();
                version.RoutingId = routingId;
                version.State = RoutingState.Save;
                version.Layout = layout;
                if (routLayoutMsg.VersionName.IsNotEmpty())
                    version.Name = routLayoutMsg.VersionName;
                RF.Save(version);
                container.RoutingVersionId = version.Id;
                layout.Layout = container.Serialize();
                RF.Save(layout);
                tran.Complete();
                return version;
            }
        }

        /// <summary>
        /// 更新工艺路线版本引用次数
        /// </summary>
        /// <param name="versionId">工艺路线版本ID</param>
        /// <param name="times">次数，如果是添加传1，删除则传-1</param>
        public virtual void UpdateVersionRefTimes(double versionId, int times)
        {
            DB.Update<RoutingVersion>().Set(p => p.ReferenceQty, p => p.ReferenceQty + times).Where(p => p.Id == versionId).Execute();
        }

        /// <summary>
        /// 根据名称获取工艺路线
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>工艺路线</returns>
        public virtual Routing GetRoutingByName(string name)
        {
            return Query<Routing>().Where(p => p.Name == name).FirstOrDefault();
        }

        #region 工艺路线导入逻辑
        /// <summary>
        /// 导入工艺路线处理
        /// </summary>
        /// <param name="routingViewModel">工艺路线</param>
        public virtual RoutingVersion ImportRouting(RoutingImportSaveViewModel routingViewModel)
        {
            ValidateRouting(routingViewModel);
            Dictionary<int, int> sortAndSonsDic = InitProcessLevels(routingViewModel);
            Dictionary<int, Point> dicPoints = InitProcessPoints(routingViewModel, sortAndSonsDic);
            using (var tran = DB.TransactionScope(TechEntityDataProvider.ConnectionStringName))
            {
                Routing routing = CreateRouting(routingViewModel);
                var container = new ContainerModel()
                {
                    RoutingId = routing.Id,
                    State = ElementState.New,
                };
                ////工序分组，key为工序的序列，value为工序的参数列表
                var processGroup = routingViewModel.ProcessDetailModelList.GroupBy(p => p.SortOrder).OrderBy(p => p.Key);
                int seq = 1;
                int count = processGroup.Count();
                foreach (var item in processGroup)
                {
                    var processList = item.ToList();
                    var process = processList.FirstOrDefault();
                    if (seq == 1)
                    {
                        AddBeginActivity(dicPoints, container); //开始节点
                    }

                    var model = CreateModel(process, processList, container);
                    model.SetPoint(dicPoints[process.SortOrder]);
                    if (seq == count)
                    {
                        AddEndActivity(dicPoints, container); //结束节点 
                    }

                    seq++;
                }

                InitActivityRules(routingViewModel, container);

                var routLayoutMsg = new RoutingLayoutMsg()
                {
                    RoutingId = routing.Id,
                    Layout = container.Serialize()
                };

                var version = RT.Service.Resolve<RoutingController>().CreateRoutingVersion(routLayoutMsg);

                tran.Complete();
                return version;
            }
        }

        /// <summary>
        /// 导入工艺路线并发布
        /// </summary>
        /// <param name="routingViewModel">工艺路线</param>
        /// <param name="processParamerList">工序参数</param>
        public virtual double ImportRoutingThenRelease(RoutingImportSaveViewModel routingViewModel, IEnumerable<ProcessParameter> processParamerList)
        {
            ValidateRouting(routingViewModel);
            Dictionary<int, int> sortAndSonsDic = InitProcessLevels(routingViewModel);
            Dictionary<int, Point> dicPoints = InitProcessPoints(routingViewModel, sortAndSonsDic);
            double versionId;
            using (var tran = DB.TransactionScope(TechEntityDataProvider.ConnectionStringName))
            {
                Routing routing = CreateRouting(routingViewModel);
                var container = new ContainerModel()
                {
                    RoutingId = routing.Id,
                    State = ElementState.New,
                };
                ////工序分组，key为工序的序列，value为工序的参数列表
                var processGroup = routingViewModel.ProcessDetailModelList.GroupBy(p => p.SortOrder).OrderBy(p => p.Key);
                int seq = 1;
                int count = processGroup.Count();
                foreach (var item in processGroup)
                {
                    var processList = item.ToList();
                    var process = processList.FirstOrDefault();
                    if (seq == 1)
                    {
                        AddBeginActivity(dicPoints, container); //开始节点
                    }
                    var curProcessParamers = processParamerList.Where(p => p.ProcessId == process.ProcessId).ToList();
                    var model = CreateModelForRelease(process, processList, container, curProcessParamers);
                    model.SetPoint(dicPoints[process.SortOrder]);
                    if (seq == count)
                    {
                        AddEndActivity(dicPoints, container); //结束节点 
                    }

                    seq++;
                }

                InitActivityRules(routingViewModel, container);

                var routLayoutMsg = new RoutingLayoutMsg()
                {
                    RoutingId = routing.Id,
                    Layout = container.Serialize()
                };

                var version = RT.Service.Resolve<RoutingController>().CreateRoutingVersion(routLayoutMsg);

                // 发布工艺路线版本
                ReleaseRoutingVersion(version.Id, routLayoutMsg.Layout);

                versionId = version.Id;
                tran.Complete();
            }
            return versionId;
        }

        /// <summary>
        /// 初始化工序规则
        /// </summary>
        /// <param name="routingViewModel">导入工艺路线模型</param>
        /// <param name="container">容器</param>
        public virtual void InitActivityRules(RoutingImportSaveViewModel routingViewModel, ContainerModel container)
        {
            foreach (var activity in container.Activitys)
            {
                if (activity.Type == ActivityType.Completion)
                    continue;
                if (activity.Type == ActivityType.Initial)
                {
                    var firstProcess = routingViewModel.ProcessDetailModelList.OrderBy(p => p.SortOrder).FirstOrDefault();
                    var nextActivity = container.Activitys.FirstOrDefault(p => p.ProcessId == firstProcess.ProcessId);
                    var rule = activity.Rules.FirstOrDefault();
                    if (rule == null)
                        continue;
                    SetRule(activity, nextActivity, rule);
                    continue;
                }

                List<IRule> rules = new List<IRule>();
                rules.AddRange(activity.Rules);
                foreach (var e in rules)
                {
                    var rule = container.Rules.FirstOrDefault(p => p.Id == e.Id);
                    ////当前工序 
                    var currentProcess = routingViewModel.ProcessDetailModelList.FirstOrDefault(p => p.ParameterId == e.ParameterId && p.ProcessId == activity.ProcessId && p.RuleId == e.Id);
                    if (currentProcess == null)
                        continue;
                    if (currentProcess.SortOrderBack == 0)
                    {
                        var endActivity = container.Activitys.FirstOrDefault(p => p.Type == ActivityType.Completion);
                        if (endActivity == null) continue;
                        SetRule(activity, endActivity, rule);
                    }
                    else
                    {
                        var nextProcess = routingViewModel.ProcessDetailModelList.FirstOrDefault(p => p.SortOrder == currentProcess.SortOrderBack);
                        if (nextProcess == null) continue;
                        var nextActivity = container.Activitys.FirstOrDefault(p => p.ProcessId == nextProcess.ProcessId && p.Id == nextProcess.Id);
                        if (nextActivity == null) continue;
                        SetRule(activity, nextActivity, rule);
                    }
                }
            }
        }

        /// <summary>
        /// 添加结束模型
        /// </summary>
        /// <param name="dicPoints">模型位置字典</param>
        /// <param name="container">容器</param>
        public virtual void AddEndActivity(Dictionary<int, Point> dicPoints, ContainerModel container)
        {
            var endActivityModel = new ActivityModel();
            endActivityModel.Type = ActivityType.Completion;
            endActivityModel.Text = "结束".L10N();
            endActivityModel.SetPoint(dicPoints[9999]);
            container.AddChild(endActivityModel);
        }

        /// <summary>
        /// 添加开始模型
        /// </summary>
        /// <param name="dicPoints">模型位置字典</param>
        /// <param name="container">容器</param>
        public virtual void AddBeginActivity(Dictionary<int, Point> dicPoints, ContainerModel container)
        {
            var beginActivityModel = new ActivityModel();
            beginActivityModel.Type = ActivityType.Initial;
            beginActivityModel.Text = "开始".L10N();
            beginActivityModel.SetPoint(dicPoints[0]);
            var rule = new RuleModel();
            rule.SourceActivityId = beginActivityModel.Id;
            beginActivityModel.Rules.Add(rule);
            container.Rules.Add(rule);
            container.AddChild(beginActivityModel);
        }

        /// <summary>
        /// 设置模型规则
        /// </summary>
        /// <param name="activity">开始模型</param>
        /// <param name="nextActivity">下一模型</param>
        /// <param name="rule">规则</param>
        private void SetRule(IActivity activity, IActivity nextActivity, IRule rule)
        {
            ////规则设置开始节点、结束节点
            rule.SetBeginActivity(activity);
            rule.SetEndActivity(nextActivity);
            ////开始节点添加规则，开始规则
            activity.Rules.Add(rule);
            activity.BeginRules.Add(rule);
            ////结束节点添加结束规则
            nextActivity.EndRules.Add(rule);
            var start = activity.GetBottomPoint();
            var end = nextActivity.GetTopPoint();
            rule.Point2 = start;
            rule.StartPoint = start;
            rule.EndPoint = end;
            rule?.OnActivityMove(activity);
        }

        /// <summary>
        /// 初始化工序层级
        /// </summary>
        /// <param name="routingViewModel">导入工艺路线模型</param>
        /// <returns>层级字典</returns>
        public virtual Dictionary<int, int> InitProcessLevels(RoutingImportSaveViewModel routingViewModel)
        {
            Dictionary<int, int> sortAndSonsDic = new Dictionary<int, int>();
            var list = SetLevel(routingViewModel.ProcessDetailModelList);
            var sortOrderList = list.Select(p => p.SortOrder).Distinct().ToList();
            foreach (var process in sortOrderList)
            {
                list.ForEach(e => e.IsPass = false);
                sortAndSonsDic.Add(process, GetRootCount(list, process, 0));
            }

            return sortAndSonsDic;
        }

        /// <summary>
        /// 初始化工序位置信息
        /// </summary>
        /// <param name="routingViewModel">导入工艺路线模型</param>
        /// <param name="sortAndSonsDic">层级字典</param>
        /// <returns>工序位置信息字典</returns>
        public virtual Dictionary<int, Point> InitProcessPoints(RoutingImportSaveViewModel routingViewModel, Dictionary<int, int> sortAndSonsDic)
        {
            //计算工序位置
            Dictionary<int, Point> dicPoints = new Dictionary<int, Point>();
            //设置工序间距
            const double hightSpace = 120;
            const double widthSpace = 200;
            const double beginX = 400;
            double x;
            double y = 126;
            dicPoints.Add(0, new Point(beginX, y));
            var dd = routingViewModel.ProcessDetailModelList.DistinctBy(p => p.SortOrder).GroupBy(p => p.Level).OrderBy(p => p.Key);
            foreach (var item in dd)
            {
                var processList = item.ToList();
                y += hightSpace;
                x = beginX;
                for (int i = 0; i < processList.Count(); i++)
                {
                    dicPoints.Add(processList[i].SortOrder, new Point(i == 0 ? beginX : x, y));
                    x += widthSpace * sortAndSonsDic[processList[i].SortOrder];
                }
            }

            dicPoints.Add(9999, new Point(beginX, y + hightSpace));
            return dicPoints;
        }

        /// <summary>
        /// 创建工序模型
        /// </summary>
        /// <param name="model">导入工序视图模型</param>
        /// <param name="processList">工艺路线工序视图模型</param>
        /// <param name="container">容器</param>
        /// <param name="processParamerList">工序参数</param>
        /// <returns>工序模型</returns>
        ActivityModel CreateModelForRelease(ProcessViewModel model, IEnumerable<ProcessViewModel> processList, ContainerModel container, List<ProcessParameter> processParamerList)
        {
            var activity = new ActivityModel()
            {
                ProcessId = model.ProcessId,
                LayoutInfoId = model.LayoutInfoId,
                Vornr = model.Vornr,
                Steus = model.Steus,
                Text = model.ProcessName,
                State = ElementState.New,
                Type = ActivityType.Interaction,
                ProcessType = model.ProcessType,
                IsRepeat = model.IsRepeat != null && model.IsRepeat.Value,
                IsOptional = model.CanChoose != null && model.CanChoose.Value,
                CreateSku = model.IsCreateSku != null && model.IsCreateSku.Value,
                IsCalculate = model.IsCalculate != null && model.IsCalculate.Value,
                IsBuckleMaterial = model.IsBuckleMaterial != null && model.IsBuckleMaterial.Value,
                IsPassRate = model.IsPassRate != null && model.IsPassRate.Value,
                IsBinding = model.IsBinding != null && model.IsBinding.Value,
                IsUnBinding = model.IsUnBinding != null && model.IsUnBinding.Value,
                IsGenerateTask = model.IsGenerateTask != null && model.IsGenerateTask.Value,
                IsRequirementTask = model.IsRequirementTask != null && model.IsRequirementTask.Value,


                StartProcess = model.StartProcess,
                NormalVictory = model.NormalVictoryId,
                RepairVictory = model.RepairVictoryId,
                IsStricter = model.IsStricter,
                Overtime = model.Overtime,
                MaxPassNum = model.MaxPassNum,
                IsNextMoveIn = model.IsNextMoveIn,
                IsOutsourcing = model.IsOutsourcing,
                Index = model.SortOrder,
            };

            if (!model.ActivityId.IsNullOrEmpty())
            {
                activity.Id = model.ActivityId;
            }
            // 外部导入直接发布需要记录工序参数
            activity.ProcessParameter = new EntityList<ProcessParameter>();
            activity.ProcessParameter.AddRange(processParamerList);

            container.AddChild(activity);
            foreach (var process in processList)
            {
                var rule = new RuleModel();
                rule.Text = process.Result == ResultTypeForDesign.Custom ? process.ResultDesc : process.Result.ToLabel();
                rule.ParameterId = process.ParameterId;
                rule.ParamResultType = process.Result;
                rule.Expression = process.Script;
                rule.SourceActivityId = activity.Id;
                activity.Rules.Add(rule);
                container.Rules.Add(rule);
                process.RuleId = rule.Id;
                process.Id = activity.Id;
            }

            return activity;
        }

        /// <summary>
        /// 创建工序模型
        /// </summary>
        /// <param name="model">导入工序视图模型</param>
        /// <param name="processList">工艺路线工序视图模型</param>
        /// <param name="container">容器</param>
        /// <returns>工序模型</returns>
        public virtual ActivityModel CreateModel(ProcessViewModel model, List<ProcessViewModel> processList, ContainerModel container)
        {
            var activity = new ActivityModel()
            {
                ProcessId = model.ProcessId,
                LayoutInfoId = model.LayoutInfoId,
                Vornr = model.Vornr,
                Steus = model.Steus,
                Text = model.ProcessName,
                State = ElementState.New,
                Type = ActivityType.Interaction,
                ProcessType = model.ProcessType,
                IsRepeat = model.IsRepeat != null && model.IsRepeat.Value,
                IsOptional = model.CanChoose != null && model.CanChoose.Value,
                CreateSku = model.IsCreateSku != null && model.IsCreateSku.Value,
                IsCalculate = model.IsCalculate != null && model.IsCalculate.Value,
                IsBuckleMaterial = model.IsBuckleMaterial != null && model.IsBuckleMaterial.Value,
                IsPassRate = model.IsPassRate != null && model.IsPassRate.Value,
                IsBinding = model.IsBinding != null && model.IsBinding.Value,
                IsUnBinding = model.IsUnBinding != null && model.IsUnBinding.Value,
                IsGenerateTask = model.IsGenerateTask != null && model.IsGenerateTask.Value,
                IsRequirementTask= model.IsRequirementTask != null && model.IsRequirementTask.Value,
                StartProcess = model.StartProcess,
                NormalVictory = model.NormalVictoryId,
                RepairVictory = model.RepairVictoryId,
                IsStricter = model.IsStricter,
                Overtime = model.Overtime,
                MaxPassNum = model.MaxPassNum,
                IsNextMoveIn= model.IsNextMoveIn,
                IsOutsourcing = model.IsOutsourcing,
                Index = model.SortOrder,
                ProcessParameter = processList.Select(p => new ProcessParameter() { Id = p.ParameterId, Description = p.ResultDesc, Type = p.Result, ProcessId = p.ProcessId, Script = p.Script, Condition = p.Condition }).AsEntityList(),
            };

            if (!model.ActivityId.IsNullOrEmpty())
            {
                activity.Id = model.ActivityId;
            }

            container.AddChild(activity);
            foreach (var process in processList)
            {
                var rule = new RuleModel();
                rule.Text = process.Result == ResultTypeForDesign.Custom ? process.ResultDesc : process.Result.ToLabel();
                rule.ParameterId = process.ParameterId;
                rule.ParamResultType = process.Result;
                rule.Expression = process.Script;
                rule.SourceActivityId = activity.Id;
                activity.Rules.Add(rule);
                container.Rules.Add(rule);
                process.RuleId = rule.Id;
                process.Id = activity.Id;
            }

            return activity;
        }

        /// <summary>
        /// 创建工艺路线
        /// </summary>
        /// <param name="routingViewModel">工艺路线视图模型</param>
        /// <returns>工艺路线</returns>
        private Routing CreateRouting(RoutingImportSaveViewModel routingViewModel)
        {
            Routing routing;
            if (routingViewModel.RoutingId == null)   ////新工艺路线
            {
                ////重新查询一遍，避免前面导入时已经创建，导致重复创建
                var resultRouting = RT.Service.Resolve<RoutingController>().GetRoutingByName(routingViewModel.RoutingName);
                if (resultRouting != null)
                    routing = resultRouting;
                else
                {
                    routing = new Routing()
                    {
                        CategoryId = routingViewModel.CategoryId,
                        Name = routingViewModel.RoutingName,
                        Description = routingViewModel.RoutingDesc
                    };
                    routing.GenerateId();
                    RF.Save(routing);
                }
            }
            else
            {
                routing = RF.GetById<Routing>(routingViewModel.RoutingId);
            }

            return routing;
        }

        /// <summary>
        /// 验证工艺路线
        /// </summary>
        /// <param name="routing">导入工艺路线</param>
        private void ValidateRouting(RoutingImportSaveViewModel routing)
        {
            var processList = routing.ProcessDetailModelList;
            if (processList.Count < 1)
                throw new ValidationException("工序不能为空".L10N());
            var beginProcessList = processList.GroupBy(p => p.SortOrder).OrderBy(p => p.Key).FirstOrDefault();
            if (beginProcessList.GroupBy(p => p.ProcessName).Count() > 1)
                throw new ValidationException("只能存在一个开始工序".L10N());
            //验证一个工序是否存在多个序列
            //var repeatProcessList = processList.GroupBy(p => p.ProcessName).OrderBy(p => p.Key).ToList();
            //foreach (var item in repeatProcessList)
            //{
            //    if (item.GroupBy(p => p.SortOrder).Count() > 1)
            //        throw new ValidationException("工序[{0}]存在重复记录，请检查。".L10nFormat(item.Key));
            //}
            //验证一个序列是否存在多个工序
            var repeatSortOrderList = processList.GroupBy(p => p.SortOrder).OrderBy(p => p.Key).ToList();
            foreach (var item in repeatSortOrderList)
            {
                if (item.GroupBy(p => p.ProcessName).Count() > 1)
                    throw new ValidationException("序列[{0}]存在对应多个工序".L10nFormat(item.Key));
            }
            var beginProcess = beginProcessList.FirstOrDefault();
            beginProcessList.ForEach(p => p.Level = 0);
            if (processList.Any(p => p.IsBatch != beginProcess.IsBatch))
                throw new ValidationException("工艺路线不能存在混合类型的工序".L10N());
        }

        /// <summary>
        /// 获取每个节点的最根节点数
        /// </summary>
        /// <param name="processModelList">导入工艺路线工序集合</param>
        /// <param name="curSortOrder">当前序列号</param>
        /// <param name="rst">根节点数（包括本身）</param>
        /// <returns>返回根节点数（包括本身）</returns>
        public virtual int GetRootCount(EntityList<ProcessViewModel> processModelList, int? curSortOrder, int rst)
        {
            var items = processModelList.Where(p => p.SortOrder == curSortOrder).OrderBy(p => p.SortOrderBack).AsEntityList();
            foreach (var processItem in items)
            {
                if (processItem.SortOrderBack == 0) rst++;
                else
                {
                    var nextList = processModelList.Where(p => p.SortOrder == processItem.SortOrderBack && p.Level > processItem.Level && !p.IsPass).AsEntityList();
                    ////标记下这个节点已经走过，后面不用再走
                    processModelList.Where(p => p.SortOrder == processItem.SortOrder).ForEach(e => e.IsPass = true);
                    if (nextList.Count == 0)
                        rst++;
                    else
                    {
                        rst = GetRootCount(processModelList, processItem.SortOrderBack, rst);
                    }
                }
            }

            return rst;
        }

        /// <summary>
        /// 设置各元素所在层级
        /// </summary>
        /// <param name="processModelList">导入工艺路线工序集合</param>
        /// <returns>工艺路线工序集合</returns>
        public virtual EntityList<ProcessViewModel> SetLevel(EntityList<ProcessViewModel> processModelList)
        {
            if (processModelList.FirstOrDefault(p => p.Level == null) == null) 
                return processModelList;
            var cur = processModelList.Where(p => p.Level >= 0).OrderByDescending(p => p.Level).FirstOrDefault();
            var curLevel = cur.Level;
            var curList = processModelList.Where(p => p.Level == curLevel).ToList();
            var result = false;
            foreach (var item in curList)
            {
                if (item.SortOrder == item.SortOrderBack)
                    throw new ValidationException("工艺路线中工序[{0}]通过工序为它本身，工艺路线不正确".L10nFormat(item.ProcessName));
                var processList = processModelList.Where(p => p.SortOrder == item.SortOrderBack && p.Level == null);

                foreach (var process in processList)
                {
                    process.Level = curLevel + 1;
                    result = true;
                }
            }
            if (!result)
                throw new ValidationException("工艺路线不正确，存在不在工艺路线中的工序".L10N());

            return SetLevel(processModelList);
        }
        #endregion

        /// <summary>
        /// 导入工艺路线处理
        /// </summary>
        /// <param name="routingViewModel">工艺路线</param>
        public virtual ContainerModel GenerateRoutingContainerModel(RoutingImportSaveViewModel routingViewModel)
        {
            ValidateRouting(routingViewModel);
            Dictionary<int, int> sortAndSonsDic = InitProcessLevels(routingViewModel);
            Dictionary<int, Point> dicPoints = InitProcessPoints(routingViewModel, sortAndSonsDic);

            var container = new ContainerModel();

            ////工序分组，key为工序的序列，value为工序的参数列表
            var processGroup = routingViewModel.ProcessDetailModelList.GroupBy(p => p.SortOrder).OrderBy(p => p.Key);
            int seq = 1;
            int count = processGroup.Count();

            foreach (var item in processGroup)
            {
                var processList = item.ToList();

                var process = processList.FirstOrDefault();

                if (seq == 1)
                {
                    AddBeginActivity(dicPoints, container); //开始节点
                }

                var model = CreateModel(process, processList, container);

                model.SetPoint(dicPoints[process.SortOrder]);

                if (seq == count)
                {
                    AddEndActivity(dicPoints, container); //结束节点 
                }

                seq++;
            }

            InitActivityRules(routingViewModel, container);

            return container;
        }


        /// <summary>
        /// 获取工艺路线
        /// </summary>
        /// <returns>工艺路线列表</returns>
        public virtual EntityList<Routing> GetRouting()
        {
            return Query<Routing>().ToList();
        }

        /// <summary>
        /// 设置默认工艺路线版本
        /// </summary>
        /// <param name="routingId">工艺路线ID</param>
        /// <param name="versionId">默认工艺路线版本id</param>
        public virtual void SetDefaultVersion(double routingId, double versionId)
        {
            if (RF.GetById<RoutingVersion>(versionId)?.State != RoutingState.Release)
            {
                throw new ValidationException("未发布状态的工艺路线版本不允许设置默认".L10N());
            }

            using (var tran = DB.TransactionScope(TechEntityDataProvider.ConnectionStringName))
            {
                DB.Update<RoutingVersion>()
                .Set(p => p.IsDefault, YesNo.No)
                .Where(p => p.RoutingId == routingId && p.Id != versionId)
                .Execute();

                DB.Update<RoutingVersion>()
                    .Set(p => p.IsDefault, YesNo.Yes)
                    .Where(p => p.RoutingId == routingId && p.Id == versionId)
                    .Execute();
                DB.Update<Routing>()
                   .Set(p => p.DefaultVersionId, versionId)
                   .Where(p => p.Id == routingId)
                   .Execute();
                tran.Complete();
            }
        }

        /// <summary>
        /// 判断工序是否维护了工治具需求
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <returns>已维护工治具需求返回true，否则返回false</returns>
        public virtual bool IsExistFixtureDemand(double processId)
        {
            return Query<RoutingProcessFixture>()
                .Join<RoutingProcess>((f, p) => f.RoutingProcessId == p.Id && p.ProcessId == processId)
                .Count() > 0;
        }

        /// <summary>
        /// 判断工治具是否存在于工序工治具需求
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <param name="fixtureId">工治具编码ID</param>
        /// <returns>存在返回true，否则返回false</returns>
        public virtual bool IsExistFixtureDemand(double processId, double fixtureId)
        {
            return Query<RoutingProcessFixture>()
                .Join<RoutingProcess>((f, p) => f.RoutingProcessId == p.Id && p.ProcessId == processId)
                .Where(p => p.FixtureId == fixtureId)
                .Count() > 0;
        }

        /// <summary>
        /// 获取工序清单
        /// </summary>
        /// <param name="routingId">工艺路线Id</param>
        /// <param name="versionId">版本Id</param>
        /// <returns>工序清单列表</returns>
        public virtual EntityList<RoutingProcess> GetRoutingProcessList(double routingId, double versionId)
        {
            return Query<RoutingProcess>()
                .Join<RoutingVersion>((a, b) => a.VersionId == b.Id && b.RoutingId == routingId && b.Id == versionId)
                .OrderBy(o => o.Index).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工序清单
        /// </summary>
        /// <param name="routingVersionIds">工艺路线版本Id列表</param>        
        /// <returns>工序清单列表</returns>
        public virtual EntityList<RoutingProcess> GetRoutingProcessList(List<double> routingVersionIds)
        {
            return routingVersionIds.Select(x => (double?)x).SplitContains(tempIds =>
            {
                return Query<RoutingProcess>()
                    .Where(x => tempIds.Contains(x.VersionId))
                    .ToList();
            });
        }

        /// <summary>
        /// 获取粘贴后新增的工艺路线信息
        /// </summary>
        /// <param name="routingId">工艺路线Id</param>
        /// <param name="maxVersionNum">最大版本数</param>
        /// <param name="xml">序列化文本</param>
        /// <returns>工艺路线信息</returns>
        public virtual RoutingVersionInfo GetPraseRoutingVersionInfo(double routingId, int maxVersionNum, string xml)
        {
            if (xml.IsNullOrWhiteSpace())
                throw new ValidationException("工艺路线容器不能为空".L10N());

            IContainer container = new ContainerModel();
            container.Deserialize(xml);
            var routing = RF.Find<Routing>().GetById(routingId) as Routing;
            if (routing == null)
                throw new ValidationException("工艺路线不能为空".L10N());
            maxVersionNum++;
            var versionName = "V{0:D4}".FormatArgs(maxVersionNum);
            string name = "{0}(未保存)".L10nFormat(versionName);
            return new RoutingVersionInfo
            {
                Id = 0,
                Text = name,
                Leaf = true,
                Nodetype = "VersionNode",
                RoutingId = routingId,
                State = RoutingState.UnSave,
                IsDefault = false,
                IsCopy = true,
                VersionName = versionName,
                TargetRoutingId = routingId,
            };
        }

        /// <summary>
        /// 获取最新的版本名称
        /// </summary>
        /// <param name="maxVersionNum">最大版本号</param>
        /// <returns>版本对象</returns>
        public virtual RoutingVersionInfo GetAddRoutingVersionInfo(int maxVersionNum)
        {
            maxVersionNum++;
            var versionName = "V{0:D4}".FormatArgs(maxVersionNum);
            string name = "{0}(未保存)".L10nFormat(versionName);
            return new RoutingVersionInfo
            {
                Id = 0,
                Text = name,
                Leaf = true,
                Nodetype = "VersionNode",
                State = RoutingState.UnSave,
                IsDefault = false,
                IsNew = true,
                VersionName = versionName,
            };
        }

        /// <summary>
        /// 获取产品族分类信息
        /// </summary>
        /// <returns>产品族分类信息</returns>
        public virtual List<FamilyCategoryInfo> GetRoutingTreeInfos()
        {
            var categoryList = RT.Service.Resolve<ItemController>().GetProductFamilyCategories();
            var dicRouting = GetRoutings().GroupBy(p => p.CategoryId).ToDictionary(p => p.Key, f => f.ToList());
            var dicRoutingVersion = GetRoutingVersions().GroupBy(p => p.RoutingId).ToDictionary(p => p.Key, f => f.ToList());
            List<FamilyCategoryInfo> categories = new List<FamilyCategoryInfo>();
            categoryList.ForEach(category =>
            {
                var info = new FamilyCategoryInfo()
                {
                    Id = category.Id,
                    Code = category.Code,
                    Name = category.Name,
                };
                if (dicRouting.ContainsKey(category.Id))
                {
                    var routings = dicRouting[category.Id];
                    routings.ForEach(routing =>
                    {
                        var routingInfo = new RoutingInfo()
                        {
                            Id = routing.Id,
                            Name = routing.Name,
                            Description = routing.Description,
                            DefaultVersionId = routing.DefaultVersionId,
                            CategoryId = category.Id,
                            MaxVersionNum = 0
                        };
                        if (dicRoutingVersion.ContainsKey(routing.Id))
                        {
                            var versionNums = new List<int>();
                            var versions = dicRoutingVersion[routing.Id];
                            versions.OrderBy(p => p.Name).ForEach(version =>
                            {
                                int.TryParse(version.Name.Substring(1), out int number);
                                if (!versionNums.Contains(number))
                                    versionNums.Add(number);

                                RoutingVersoinInfo versionInfo = new RoutingVersoinInfo()
                                {
                                    Id = version.Id,
                                    Name = version.Name,
                                    EffectiveDate = version.EffectiveDate,
                                    IsDefault = version.IsDefault,
                                    LayoutId = version.LayoutId,
                                    ReferenceTime = version.ReferenceQty,
                                    State = (int)version.State,
                                    RoutingId = routing.Id
                                };
                                routingInfo.VersionList.Add(versionInfo);
                            });

                            routingInfo.MaxVersionNum = versionNums.Max();
                        }
                        info.RoutingList.Add(routingInfo);
                    });
                }
                categories.Add(info);
            });
            return categories;
        }

        /// <summary>
        /// 根据关键字获取产品族分类信息
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns>产品族分类信息</returns>
        public virtual List<FamilyCategoryInfo> GetRoutingTreeInfosByKeyword(string keyword)
        {
            List<FamilyCategoryInfo> categories = new List<FamilyCategoryInfo>();
            var routingTreeInfos = GetRoutingTreeInfos();
            if (keyword.IsNullOrEmpty())
                categories.AddRange(routingTreeInfos);
            else
            {
                var categoryList = routingTreeInfos.Where(p => p.Name.IndexOf(keyword) != -1);
                if (categoryList.Any())
                    categories.AddRange(categoryList);
                else
                    categories.AddRange(GetFamilyCategoryInfosByRoutingName(keyword, routingTreeInfos));
            }

            return categories;
        }

        /// <summary>
        /// 根据工艺路线名称获取产品族分类信息列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="routingTreeInfos">所有产品族分类信息列表</param>
        /// <returns>产品族分类信息列表</returns>
        private List<FamilyCategoryInfo> GetFamilyCategoryInfosByRoutingName(string keyword, List<FamilyCategoryInfo> routingTreeInfos)
        {
            List<FamilyCategoryInfo> categories = new List<FamilyCategoryInfo>();
            //满足条件的工艺路线
            var dicRouting = routingTreeInfos.SelectMany(p => p.RoutingList).Where(p => p.Name.IndexOf(keyword) != -1).GroupBy(p => p.CategoryId).ToDictionary(p => p.Key, p => p.ToList());
            //满足条件的产品族分类
            var categoryIds = dicRouting.Select(p => p.Key);
            foreach (var routingTreeInfo in routingTreeInfos)
            {
                if (!categoryIds.Any(p => p == routingTreeInfo.Id))
                    continue;
                routingTreeInfo.RoutingList.Clear();
                if (dicRouting.TryGetValue(routingTreeInfo.Id, out List<RoutingInfo> routings))
                {
                    routings.ForEach(routing =>
                    {
                        routingTreeInfo.RoutingList.Add(routing);
                    });
                }

                GetFamilyCategoryInfos(categories, routingTreeInfo, routings);
            }

            return categories;
        }

        /// <summary>
        /// 根据工艺路线名称获取产品族分类信息列表
        /// </summary>
        /// <param name="categories">过滤后的产品族分类信息列表</param>
        /// <param name="routingTreeInfo">产品族分类信息</param>
        /// <param name="routings">工艺路线信息列表</param>
        private void GetFamilyCategoryInfos(List<FamilyCategoryInfo> categories, FamilyCategoryInfo routingTreeInfo, List<RoutingInfo> routings)
        {
            if (categories.Any(p => p.Id == routingTreeInfo.Id))
            {
                //存在产品族分类则添加工艺路线
                var categoryInfo = categories.FirstOrDefault(p => p.Id == routingTreeInfo.Id);
                if (routings != null)
                {
                    routings.ForEach(routing =>
                    {
                        categoryInfo.RoutingList.Add(routing);
                    });
                }
            }
            else
                categories.Add(routingTreeInfo);
        }

        /// <summary>
        /// 获取工序清单参数
        /// </summary>
        /// <param name="processActivityId">工序清单ID</param>
        /// <returns>工序清单参数列表</returns>
        public virtual EntityList<RoutingProcessParameter> GetRoutingProcessParameters(string processActivityId)
        {
            return Query<RoutingProcessParameter>().Where(p => p.RoutingProcess.ActivityId == processActivityId).ToList();
        }

        /// <summary>
        /// 获取工序清单参数
        /// </summary>
        /// <param name="routingProcessIds">工序清单ID列表</param>
        /// <returns>工序清单参数列表</returns>
        public virtual EntityList<RoutingProcessParameter> GetRoutingProcessParameters(List<double> routingProcessIds)
        {
            return routingProcessIds.SplitContains(tempIds =>
            {
                return Query<RoutingProcessParameter>().Where(p => tempIds.Contains(p.RoutingProcessId)).ToList();
            });
        }

        /// <summary>
        /// 获取工序BOM配置
        /// </summary>
        /// <param name="routingProcessId">工序清单ID</param>
        /// <returns>工序BOM配置列表</returns>
        public virtual EntityList<RoutingProcessBomConfig> GetRoutingProcessBomConfigs(double routingProcessId)
        {
            return Query<RoutingProcessBomConfig>().Where(p => p.RoutingProcessId == routingProcessId).ToList();
        }

        /// <summary>
        /// 获取工序BOM配置
        /// </summary>
        /// <param name="routingProcessIds">工序清单ID列表</param>
        /// <returns>工序BOM配置列表</returns>
        public virtual EntityList<RoutingProcessBomConfig> GetRoutingProcessBomConfigs(List<double> routingProcessIds)
        {
            return routingProcessIds.SplitContains(tempIds =>
            {
                return Query<RoutingProcessBomConfig>().Where(p => tempIds.Contains(p.RoutingProcessId)).ToList();
            });
        }

        /// <summary>
        /// 获取工序清单采集步骤
        /// </summary>
        /// <param name="routingProcessId">工序清单ID</param>
        /// <returns>工序清单采集步骤列表</returns>
        public virtual EntityList<RoutingProcessCollectStep> GetProcessCollectSteps(double routingProcessId)
        {
            return Query<RoutingProcessCollectStep>().Where(p => p.RoutingProcessId == routingProcessId).ToList();
        }

        /// <summary>
        /// 获取工艺路线布局
        /// </summary>
        /// <param name="layoutIds">Id 列表</param>
        /// <returns></returns>
        public virtual EntityList<RoutingLayout> GetRoutingLayouts(List<double> layoutIds)
        {
            return layoutIds.SplitContains(tempIds =>
            {
                return Query<RoutingLayout>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });
        }
    }
}