using Castle.MicroKernel;
using SIE.Domain.Validation;
using SIE.Tech.Processs;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
    /// 工艺路线设计容器
    /// </summary>
    [Serializable]
    public class ContainerModel : ElementModel, IContainer
    {
        /// <summary>
        /// 元素选中事件
        /// </summary>
        public event Action<IElement> SelectedElementChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ContainerModel()
        {
            Activitys = new ObservableCollection<IActivity>();
            Rules = new ObservableCollection<IRule>();
            Notes = new ObservableCollection<INote>();
            SelectElements = new ObservableCollection<IElement>();
            Width = 1920;
            Height = 1080;
            ShowGridLine = true;
        }

        /// <summary>
        /// 子元素集合
        /// </summary>
        public IEnumerable<IChildElement> Children
        {
            get
            {
                foreach (var activity in Activitys)
                    yield return activity;
                foreach (var rule in Rules)
                    yield return rule;
                foreach (var note in Notes)
                    yield return note;
            }
        }

        /// <summary>
        /// 活动元素集合
        /// </summary>
        public IList<IActivity> Activitys { get; set; }

        /// <summary>
        /// 规则集合
        /// </summary>
        public IList<IRule> Rules { get; set; }

        /// <summary>
        /// 备注集合
        /// </summary>
        public IList<INote> Notes { get; set; }

        /// <summary>
        /// 选中元素集合
        /// </summary>
        public IList<IElement> SelectElements { get; set; }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(SelectElement) && SelectedElementChanged != null)
            {
                SelectedElementChanged(SelectElement);
            }
        }

        #region 属性
        /// <summary>
        /// 容器高
        /// </summary>
        public double Height
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 当前选中元素
        /// </summary>
        public IElement SelectElement
        {
            get { return GetProperty<IElement>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 显示网格
        /// </summary>
        public bool ShowGridLine
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 容器宽
        /// </summary>
        public double Width
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 变焦深
        /// </summary>
        public double ZoomDeep
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public double RoutingId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 工艺路线版本ID
        /// </summary>
        public double RoutingVersionId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }
        #endregion

        /// <summary>
        /// 默认Step
        /// </summary>
        private int indexStep = 10;

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="child">元素</param>
        public void AddChild(IChildElement child)
        {
            if (child is IActivity)
            {
                AddActivity(child as IActivity);
            }

            if (child is INote)
            {
                AddNote(child as INote);
            }

            if (child is IRule)
            {
                AddRule(child as IRule);
            }

            SelectedElement(child);
        }

        /// <summary>
        /// 添加活动元素
        /// </summary>
        /// <param name="activity">元素</param>
        void AddActivity(IActivity activity)
        {
            if (!Activitys.Contains(activity))
            {
                Activitys.Add(activity);
            }
        }

        /// <summary>
        /// 添加规则
        /// </summary>
        /// <param name="rule">规则</param>
        void AddRule(IRule rule)
        {
            if (!Rules.Contains(rule))
            {
                Rules.Add(rule);
            }
        }

        /// <summary>
        /// 添加备注
        /// </summary>
        /// <param name="note">备注</param>
        void AddNote(INote note)
        {
            if (!Notes.Contains(note))
            {
                Notes.Add(note);
            }
        }

        /// <summary>
        /// 移除元素
        /// </summary>
        /// <param name="child">元素</param>
        public void RemoveChild(IChildElement child)
        {
            if (child == null)
            {
                return;
            }

            if (SelectElement == child)
            {
                SelectElement = null;
            }

            if (Activitys.Contains(child))
            {
                Activitys.Remove(child as IActivity);
            }

            if (Notes.Contains(child))
            {
                Notes.Remove(child as INote);
            }
            ////规则不能移除，不然会导致删除后点保存，丢失这个规则
            ////if (Rules.Contains(child))
            ////    Rules.Remove(child as IRule);
            child.Delete();
        }

        /// <summary>
        /// 删除选中元素
        /// </summary>
        public void DeleteSelectedElement()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 选中元素
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="isMultiSelected">是否多选</param>
        /// <param name="isClear">是否清除选中元素</param>
        public void SelectedElement(IElement element, bool isMultiSelected = false, bool isClear = true)
        {
            if (element is IChildElement && !Children.Contains(element))
            {
                AddChild(element as IChildElement);
                return;
            }

            if (isClear || !isMultiSelected)
            {
                Children.ForEach(p => p.IsSelected = false);
                SelectElements.Clear();
            }

            if (element == null)
            {
                SelectElement = null;
                return;
            }

            if (!SelectElements.Any(p => p.Id == element.Id))
            {
                SelectElements.Add(element);
            }

            if (element is IChildElement)
            {
                element.IsSelected = true;
                (element as IChildElement).ZIndex = NextIndex();
                SelectElement = element;
                return;
            }

            if (element is IContainer)
            {
                element.IsSelected = true;
                SelectElement = element;
            }
        }

        /// <summary>
        /// 索引
        /// </summary>
        int nextIndex = 1;

        /// <summary>
        /// 获取索引
        /// </summary>
        /// <returns>索引</returns>
        public int NextIndex()
        {
            return nextIndex++;
        }

        #region 序列化工艺路线
        /// <summary>
        /// 序列化工艺路线
        /// </summary>
        /// <returns>序列化后字符串</returns>
        public override string Serialize()
        {
            UpdateIndex();
            StringBuilder xml = new StringBuilder(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes"" ?>");
            xml.Append(Environment.NewLine);
            //xml.Append("<!--工作流设计器描述文件-->");
            xml.Append(Environment.NewLine);
            xml.Append(@"<Container Id=""{0}"" State=""{1}"" IsSelected=""{2}"" RoutingId=""{3}"" RoutingVersionId=""{4}"" Width=""{5}"" Height=""{6}"" ShowGridLine=""{7}"" ZoomDeep=""{8}"">".FormatArgs(Id, State, IsSelected, RoutingId, RoutingVersionId, Width, Height, ShowGridLine, ZoomDeep));

            SerializeActivitys(xml);
            SerializeRules(xml);
            SerializeNotes(xml);
            xml.Append(Environment.NewLine);
            xml.Append("</Container>");
            return xml.ToString();
        }

        /// <summary>
        /// 更新索引顺序
        /// </summary>
        void UpdateIndex()
        {
            if (Activitys.Count == 0)
                return;
            var activityIds = new List<string>();
            Activitys.ForEach(p => p.Index = -1);
            var startActivity = Activitys.FirstOrDefault(p => p.Type == ActivityType.Initial);
            startActivity.Index = 0;
            UpdateTrunkIndex(activityIds, startActivity?.BeginRules.FirstOrDefault()?.EndActivity, indexStep);

            //更新 Ng 工序的索引顺序
            var maxIndex = Activitys.Max(p => p.Index);
            foreach (var negtiveActivity in Activitys.Where(p => p.Index < 0).OrderBy(p => p.Text))
            {
                maxIndex += indexStep;
                negtiveActivity.Index = maxIndex;
            }
        }

        /// <summary>
        /// 更新主干索引顺序（主干表示：Pass 或 Any 通过的工序）
        /// </summary>
        /// <param name="activityIds">已遍历的所有活动节点Id集合</param>
        /// <param name="activity">活动</param>
        /// <param name="index">索引顺序</param>
        void UpdateTrunkIndex(List<string> activityIds, IActivity activity, int index)
        {
            if (activity == null)
                return;
            if (IsExistActivityId(activityIds, activity.Id))
                return;
            else
                activityIds.Add(activity.Id);
            activity.Index = index;
            var nextPassActivity = activity.BeginRules.FirstOrDefault(p => p.Text == ResultTypeForDesign.Pass.ToLabel());
            if (nextPassActivity != null && nextPassActivity.EndActivity != null)
            {
                UpdateTrunkIndex(activityIds, nextPassActivity.EndActivity, index + indexStep);
                return;
            }

            //如果 Pass 工序找不到，就找 Any 工序
            var nextAnyActivity = activity.BeginRules.FirstOrDefault(p => p.Text == ResultTypeForDesign.Any.ToLabel());
            UpdateTrunkIndex(activityIds, nextAnyActivity?.EndActivity, index + indexStep);
        }

        /// <summary>
        /// 是否已遍历活动节点
        /// </summary>
        /// <param name="activityIds">已遍历的所有活动节点Id集合</param>
        /// <param name="activityId">当前将要遍历的活动节点Id</param>
        /// <returns>true/false</returns>
        bool IsExistActivityId(List<string> activityIds, string activityId)
        {
            if (activityIds.Contains(activityId))
                return true;
            return false;
        }

        /// <summary>
        /// 序列化活动列表
        /// </summary>
        /// <param name="xml">序列化xml</param>
        void SerializeActivitys(StringBuilder xml)
        {
            if (Activitys.Count == 0)
                return;
            xml.Append(Environment.NewLine);
            xml.Append("    <Activitys>");
            foreach (var activity in Activitys)
            {
                xml.Append(Environment.NewLine);
                xml.Append(activity.Serialize());
            }

            xml.Append(Environment.NewLine);
            xml.Append("    </Activitys>");
        }

        /// <summary>
        /// 序列化规则列表
        /// </summary>
        /// <param name="xml">序列化xml</param>
        void SerializeRules(StringBuilder xml)
        {
            if (Rules.Count == 0)
                return;
            xml.Append(Environment.NewLine);
            xml.Append("    <Rules>");
            foreach (var rule in Rules)
            {
                xml.Append(Environment.NewLine);
                xml.Append(rule.Serialize());
            }

            xml.Append(Environment.NewLine);
            xml.Append("    </Rules>");
        }

        /// <summary>
        /// 序列化备注列表
        /// </summary>
        /// <param name="xml">序列化xml</param>
        void SerializeNotes(StringBuilder xml)
        {
            if (Notes.Count == 0)
                return;
            xml.Append(Environment.NewLine);
            xml.Append("    <Notes>");
            foreach (var note in Notes)
            {
                xml.Append(Environment.NewLine);
                xml.Append(note.Serialize());
            }

            xml.Append(Environment.NewLine);
            xml.Append("    </Notes>");
        }
        #endregion

        #region 反序列化工艺路线
        /// <summary>
        /// 反序列工艺路线
        /// </summary>
        /// <param name="xml">序列化xml</param>
        /// <param name="isCopy">是否复制</param>
        public override void Deserialize(string xml, bool isCopy = false)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            XElement xelement = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(bytes)));
            Id = xelement.GetAttributeValue(nameof(Id), string.Empty);
            State = (ElementState)EnumViewModel.LabelToEnum(xelement.GetAttributeValue(nameof(State), string.Empty), typeof(ElementState));
            IsSelected = xelement.GetBoolAttributeValue(nameof(IsSelected));
            RoutingId = xelement.GetAttributeValue(nameof(RoutingId), 0d);
            RoutingVersionId = xelement.GetAttributeValue(nameof(RoutingVersionId), 0d);
            Width = xelement.GetAttributeValue(nameof(Width), 0d);
            Height = xelement.GetAttributeValue(nameof(Height), 0d);
            ShowGridLine = xelement.GetBoolAttributeValue(nameof(ShowGridLine));
            ZoomDeep = xelement.GetAttributeValue(nameof(ZoomDeep), 0d);
            foreach (XElement node in xelement.Descendants("Activity"))
            {
                var activity = new ActivityModel();
                activity.Deserialize(node.ToString());
                if (isCopy)
                {
                    activity.CopyId = activity.Id;//记录复制的旧Id
                    activity.Id = Guid.NewGuid().ToString();
                }
                Activitys.Add(activity);
            }
            if (isCopy)
            {
                Rules.Clear();
                //处理工序组复制
                var groupActivities = Activitys.Where(m => m.IsGroup).ToList();
                groupActivities.ForEach(activity =>
                {
                    var groupChildrens = Activitys.Where(m => m.GroupId == activity.GroupId).ToList();
                    groupChildrens.ForEach(child =>
                    {
                        child.GroupId = activity.Id;
                    });


                });
            }

            foreach (XElement node in xelement.Descendants("Rule"))
            {
                var rule = new RuleModel();
                rule.Deserialize(node.ToString(), isCopy);
                var activity = Activitys.FirstOrDefault(p => p.Id == rule.SourceActivityId);
                if (isCopy)
                {//复制之后ID发生变更 通过CopyId找回Activity
                    activity = Activitys.FirstOrDefault(p => p.CopyId == rule.SourceActivityId);
                    if (activity != null)
                    {
                        rule.SourceActivityId = activity.Id;
                    }
                }
                if (activity == null)
                    continue;
                activity.Rules.Add(rule);
                Rules.Add(rule);
                if (!isCopy)
                {
                    if (rule.BeginActivity != null && rule.EndActivity != null && activity.Id == rule.BeginActivity.Id)
                    {
                        var endActivity = Activitys.FirstOrDefault(p => p.Id == rule.EndActivity.Id);
                        if (endActivity != null)
                        {
                            activity.Rules.Remove(rule);
                            activity.BeginRules.Add(rule);
                            endActivity.EndRules.Add(rule);
                            rule.SetBeginActivity(activity);
                            rule.SetEndActivity(endActivity);
                        }
                        else
                        {
                            rule.BeginActivity = null;
                            rule.EndActivity = null;
                        }
                    }
                }
                else//copy的情况
                {
                    if (rule.BeginActivity != null && rule.EndActivity != null && activity.CopyId == rule.BeginActivity.CopyId)
                    {
                        var endActivity = Activitys.FirstOrDefault(p => p.CopyId == rule.EndActivity.CopyId);
                        if (endActivity != null)
                        {
                            activity.Rules.Remove(rule);
                            activity.BeginRules.Add(rule);
                            endActivity.EndRules.Add(rule);
                            rule.SetBeginActivity(activity);
                            rule.SetEndActivity(endActivity);
                        }
                        else
                        {
                            rule.BeginActivity = null;
                            rule.EndActivity = null;
                        }
                    }
                }
            }

            //foreach (XElement node in xelement.Descendants("Note"))
            //{

            //}
        }
        #endregion

        /// <summary>
        /// 保存验证
        /// </summary>
        public override void ValidateSave()
        {
            var beginActivity = Activitys.FirstOrDefault(p => p.Type == ActivityType.Initial);
            if (beginActivity == null)
            {
                throw new ValidationException("未找到开始工序".L10N());
            }

            var endActivity = Activitys.FirstOrDefault(p => p.Type == ActivityType.Completion);
            if (endActivity == null)
            {
                throw new ValidationException("未找到结束工序".L10N());
            }

            var activity = Activitys.FirstOrDefault(p => p.Type == ActivityType.Interaction);
            if (activity == null)
            {
                throw new ValidationException("至少要有一个活动工序".L10N());
            }
            ////项目上反馈禁用掉：工艺路线中不允许两个相同的工序，经测试流程可通 XHJ20190730 (注意：如果工序跳转到有多个相同工序的工序时有问题)
            ////var group = Activitys.Where(p => p.Type == ActivityType.Interaction).GroupBy(p => p.ProcessId).FirstOrDefault(p => p.Count() > 1);
            ////if (group != null)
            ////    throw new ValidationException("存在 {0} 个工序：{1}".L10nFormat(group.Count(), group.FirstOrDefault().Text));
            ///
            //多分支的时候以下逻辑不适用
            //var createSkuProcess = Activitys.Where(p => p.CreateSku);
            //if (createSkuProcess.Count() > 1)
            //{
            //    throw new ValidationException("只能存在一个Sku\n已存在Sku工序：{0}".L10nFormat(string.Join(";", Activitys.Where(p => p.CreateSku).Select(p => p.Text))));
            //}
            //////else if (Activitys.Any(p => p.ProcessType == ProcessType.Packing || p.ProcessType == ProcessType.BatchPacking) && createSkuProcess.Count() == 0)
            //////    throw new ValidationException("存在包装工序，必须存在一个Sku".L10N());
            //else if (Activitys.Count(p => p.ProcessType == ProcessType.Packing || p.ProcessType == ProcessType.BatchPacking) > 1 && createSkuProcess.Count() == 1)
            //{
            //    var createSkuProcessIndex = createSkuProcess.FirstOrDefault().Index;
            //    var firstPacking = Activitys.Where(p => p.ProcessType == ProcessType.Packing || p.ProcessType == ProcessType.BatchPacking).OrderBy(p => p.Index).FirstOrDefault();
            //    if (createSkuProcessIndex > firstPacking.Index)
            //    {
            //        throw new ValidationException("存在多个包装工序，必须在第一个包装工序[{0}]前创建Sku".L10nFormat(firstPacking.Text));
            //    }
            //}
            //else
            //{
            //    //
            //}
            var isCalculateProcess = Activitys.Where(p => p.IsCalculate);
            if (isCalculateProcess.Count() > 1)
            {
                throw new ValidationException("只能存在一个计产工序\n已存在计产工序".L10N()+"：{0}".L10nFormat(string.Join(";", Activitys.Where(p => p.IsCalculate).Select(p => p.Text))));
            }
            //增加维修工序不能连结束节点验证
            var fixProcessList = Activitys.Where(p => p.ProcessType == ProcessType.BatchFix || p.ProcessType == ProcessType.Fix).ToList();
            foreach (var fix in fixProcessList)
            {
                //var passnRule = fix.BeginRules.FirstOrDefault(p => p.Text == "通过");
                // 24.5.27 lyp 验证改用工序参数结果而不是结果描述
                var passnRule = fix.BeginRules.FirstOrDefault(p => p.ParamResultType == ResultTypeForDesign.Pass);
                if (passnRule == null)
                    throw new ValidationException("维修工序需存在".L10N() +"通过".L10N()+"连线".L10N());
                if (passnRule.EndActivity.Text == "结束".L10N())
                    throw new ValidationException("维修工序不能连接结束节点".L10N());
            }
            foreach (var child in Children)
            {
                child.ValidateSave();
            }

            //验证是否存在连线出现死循环
            var activitys = Activitys.Where(p => p.Type == ActivityType.Interaction).ToList();
            foreach (var tempActivity in activitys.OrderBy(p => p.Index))
            {
                var activityIds = new List<string>();
                activityIds.Add(tempActivity.Id);
                ComputeNextActivity(tempActivity, tempActivity, activityIds);
                //验证工序组的内部工序 是否满足要求
                ValidateGroupProcess(tempActivity);

            }
            var interactions = Activitys.Where(p => p.Type == ActivityType.Interaction);
            foreach (var interaction in interactions)
            {
                if (!interaction.GroupId.IsNullOrEmpty())//工序组内的工序不校验是否在主线中
                {
                    continue;
                }

                var okInteractions = new List<IActivity>();
                IsExistInitial(okInteractions, interaction);
                if (!okInteractions.Any(p => p.Type == ActivityType.Initial))
                {
                    throw new ValidationException("工序[{0}]不在工艺路线主线中，请检查".L10nFormat(interaction.Text));
                }
            }
        }

        /// <summary>
        /// 校验工序组中的工序
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        internal void ValidateGroupProcess(IActivity activity)
        {
            if (activity.IsGroup)
            {
                var groupProcesss = Activitys.Where(m => m.GroupId == activity.GroupId && !m.IsGroup).ToList();
                foreach (var processModel in groupProcesss)
                {
                    if (activity.BeginRules.Any(p => p.EndActivity.Type == ActivityType.Completion))//当前工序组是最后一个工序
                    {
                        if (processModel.IsOptional)
                        {
                            throw new ValidationException("[{1}]是结束工序，工序[{0}]不可以为可选工序".L10nFormat(processModel.Text, activity.Text));
                        }
                        if (processModel.IsRepeat)
                        {
                            throw new ValidationException("[{1}]是结束工序，工序[{0}]不可以为重复工序".L10nFormat(processModel.Text, activity.Text));
                        }
                    }
                    if (processModel.ProcessBoms.GroupBy(p => p.ItemId).Any(p => p.Count() > 1))
                    {
                        throw new ValidationException("[{1}]中工序[{0]工序BOM存在多个重复物料".L10nFormat(processModel.Text, activity.Text));
                    }
                }
            }
        }

        /// <summary>
        /// 递归计算后工序
        /// </summary>
        /// <param name="startActivity">工序</param>
        /// <param name="nextActivity">运行时产品</param>
        /// <param name="activityIds">结果类型</param>
        /// <exception cref="ValidationException">采集结果无效</exception>
        internal virtual void ComputeNextActivity(IActivity startActivity, IActivity nextActivity, List<string> activityIds)
        {
            var next = nextActivity.BeginRules.FirstOrDefault(p => p.Text == "通过".L10N() || p.Text == "任意".L10N())?.EndActivity;
            if (next != null && startActivity.Id == next.Id)
                throw new ValidationException("工序[{0}]连线后出现环路，请检查".L10nFormat(startActivity.Text));

            //递归到当前节点后 如果是包装 则判断其前方是否存在一个SKU
            var currentNodeIsPacking = nextActivity.ProcessType == ProcessType.BatchPacking || nextActivity.ProcessType == ProcessType.Packing;

            //确定第一个开始节点同时第一个开始节点不是包装工序
            var beginRule = startActivity.EndRules.FirstOrDefault(p => p.BeginActivity.Type == ActivityType.Initial);
            var startNotPacking = startActivity.ProcessType != ProcessType.BatchPacking && startActivity.ProcessType != ProcessType.Packing && beginRule != null;
            if (currentNodeIsPacking && startNotPacking)
            {
                var fontActivitys = this.Activitys.Where(m => activityIds.Contains(m.Id)).ToList();

                var groupNodes = new List<IActivity>();
                fontActivitys.ForEach(model =>
                {
                    //工序组时需验证里面的工序是否存在创建SKU
                    if (model != null && model.IsGroup)
                    {
                        var groupProcessList = this.Activitys.Where(m => m.GroupId == model.GroupId).ToList();
                        if (groupProcessList.Any())
                        {
                            groupNodes.AddRange(groupProcessList);
                        }
                    }
                });
                if (groupNodes.Any())
                {
                    fontActivitys.AddRange(groupNodes);
                }
                if (fontActivitys.Count(m => m.CreateSku && m.Id != nextActivity.Id) <=0)
                {
                    throw new ValidationException("工序[{0}]前需存在一个启用创建SKU的工序，请检查".L10nFormat(nextActivity.Text));
                }
                if (fontActivitys.Count(m => m.CreateSku && m.Id != nextActivity.Id) > 1)
                {
                    throw new ValidationException("工序[{0}]前只能允许一个启用创建SKU的工序，请检查".L10nFormat(nextActivity.Text));
                }

            }

            if (next == null || activityIds.Contains(next.Id))
                return;
            else
                activityIds.Add(next.Id);
            if (next.Text != "结束".L10N())
            {
                ComputeNextActivity(startActivity, next, activityIds);
            }
        }

        /// <summary>
        /// 是否存在开始工序
        /// </summary>
        /// <param name="interactions">已遍历的活动工序</param>
        /// <param name="interaction">当前遍历的活动工序</param>
        private void IsExistInitial(List<IActivity> interactions, IActivity interaction)
        {
            interactions.Add(interaction);
            if (interaction.EndRules.Count > 0)
            {
                ValidateActivitys(interactions, interaction.EndRules);
            }
        }

        /// <summary>
        /// 验证活动工序
        /// </summary>
        /// <param name="interactions">已遍历的活动工序</param>
        /// <param name="endRules">工序前规则列表</param>
        private void ValidateActivitys(List<IActivity> interactions, IList<IRule> endRules)
        {
            for (int i = 0; i < endRules.Count; i++)
            {
                IRule rule = endRules[i];
                var activity = rule.BeginActivity;
                if (interactions.Contains(activity))
                {
                    continue;
                }

                IsExistInitial(interactions, activity);
            }
        }

        /// <summary>
        /// 水平居中
        /// </summary>
        public void HorizontalCenter()
        {
            var activitys = SelectElements.OfType<IActivity>().Select(p => p).ToList();
            if (activitys.Count == 0)
            {
                return;
            }

            double left = activitys.Select(p => p.Left).Min();
            double right = activitys.Select(p => p.Left).Max();
            foreach (var activity in activitys)
            {
                activity.ZIndex = NextIndex();
                activity.Left = (left + right) / 2;
                activity.Move();
            }
        }

        /// <summary>
        /// 垂直居中
        /// </summary>
        public void VerticalCenter()
        {
            var activitys = SelectElements.OfType<IActivity>().Select(p => p).ToList();
            if (activitys.Count == 0)
            {
                return;
            }

            double top = activitys.Select(p => p.Top).Min();
            double bottom = activitys.Select(p => p.Top).Max();
            foreach (var activity in activitys)
            {
                activity.ZIndex = NextIndex();
                activity.Top = (top + bottom) / 2;
                activity.Move();
            }
        }

        /// <summary>
        /// 横向分布
        /// </summary>
        public void HorizontalDistribution()
        {
            var activitys = SelectElements.OfType<IActivity>().Select(p => p).ToList();
            if (activitys.Count < 3)
            {
                return;
            }

            double left = activitys.Select(p => p.Left).Min();
            double right = activitys.Select(p => p.Left).Max();
            double spacing = (right - left) / (activitys.Count - 1);
            int i = 0;
            foreach (var activity in activitys.OrderBy(p => p.Left))
            {
                activity.ZIndex = NextIndex();
                activity.Left = left + spacing * i;
                i++;
                activity.Move();
            }
        }

        /// <summary>
        /// 纵向分布
        /// </summary>
        public void VerticalDistribution()
        {
            var activitys = SelectElements.OfType<IActivity>().Select(p => p).ToList();
            if (activitys.Count < 3)
                return;
            double top = activitys.Select(p => p.Top).Min();
            double bottom = activitys.Select(p => p.Top).Max();
            double spacing = (bottom - top) / (activitys.Count - 1);
            int i = 0;
            foreach (var activity in activitys.OrderBy(p => p.Left))
            {
                activity.ZIndex = NextIndex();
                activity.Top = top + spacing * i;
                i++;
                activity.Move();
            }
        }

        /// <summary>
        /// 移动选中项
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public void MoveSelectItems(double x, double y)
        {
            var activitys = SelectElements.OfType<IActivity>().Select(p => p).ToList();
            if (activitys.Count == 0)
                return;
            if (activitys.Any(p => p.Left + x <= 0 || p.Top + y <= 0))
                return;

            ////因可动态增加画布大小，所以注释此限制条件
            ////if (activitys.Any(p => p.Left + p.Width + x >= Width || p.Top + p.ContainerHeight + y >= Height))
            ////    return;

            foreach (var activity in activitys)
            {
                var point = activity.GetPoint();
                activity.SetPoint(new Point(point.X + x, point.Y + y));
            }
        }
    }
}
