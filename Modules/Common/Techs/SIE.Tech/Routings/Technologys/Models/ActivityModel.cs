using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.EMS.Fixtures;
using SIE.Items;
using SIE.Tech.Processs;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using SIE.Tech.Routings.Technologys.Models;
using System.Transactions;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
    /// 活动模型
    /// </summary>
    [Serializable]
    public class ActivityModel : ChildElementModel, IActivity
    {
        private const string ITEM_ID = "ItemId";

        /// <summary>
        /// 规则高度
        /// </summary>
        double ruleHeight = 50;

        /// <summary>
        /// 活动移动
        /// </summary>
        public event Action<IActivity> ActivityMove;

        /// <summary>
        /// 移动
        /// </summary>
        public void Move()
        {
            ActivityMove?.Invoke(this);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ActivityModel()
        {
            EndRules = new ObservableCollection<IRule>();
            Rules = new ObservableCollection<IRule>();
            BeginRules = new ObservableCollection<IRule>();
            Bom = new EntityList<ItemExtBom>();
            ProcessBoms = new ObservableCollection<ProcessBom>();
            Fixtures = new List<FixtureInfo>();
            Width = 100;
            Height = 60;
            ContainerHeight = Height + ruleHeight;
            Index = -1;
            ProcessParameter = new EntityList<ProcessParameter>();
        }

        /// <summary>
        /// 活动属性变更
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(ProcessState))
                base.OnPropertyChanged("Self");
        }

        /// <summary>
        /// 规则参数列表
        /// </summary>
        public IList<IRule> Rules { get; set; }

        /// <summary>
        /// 开始规则列表
        /// </summary>
        public IList<IRule> BeginRules { get; set; }

        /// <summary>
        /// 结束规则列表
        /// </summary>
        public IList<IRule> EndRules { get; set; }

        /// <summary>
        /// 工序BOM列表
        /// 产品工艺路线生成时添加，维护具体扣料物料及数量
        /// </summary>
        public IList<ProcessBom> ProcessBoms { get; set; }

        /// <summary>
        /// 物料小类列表
        /// 工艺路线设计时维护，维护扣料物料
        /// </summary>
        public EntityList<ItemExtBom> Bom { get; set; }

        /// <summary>
        /// 工序参数集合
        /// </summary>
        public EntityList<ProcessParameter> ProcessParameter { get; set; }

        /// <summary>
        /// 工治具ID集合
        /// </summary>
        public IList<FixtureInfo> Fixtures { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 高
        /// </summary>
        public double Height
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 容器高
        /// </summary>
        public double ContainerHeight
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 活动类型
        /// </summary>
        public ActivityType Type
        {
            get { return GetProperty<ActivityType>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 宽
        /// </summary>
        public double Width
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 面板左边坐标
        /// </summary>
        public double Left
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 获取或设置为在此元素显示的工具提示对象 用户界面 (UI)。
        /// </summary>
        public object ToolTip
        {
            get { return GetProperty<object>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 面板顶坐标
        /// </summary>
        public double Top
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 是否可选过站
        /// </summary>
        public bool IsOptional
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 是否重复过站
        /// </summary>
        public bool IsRepeat
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 创建SKU
        /// </summary>
        public bool CreateSku
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 是否计产
        /// </summary>
        public bool IsCalculate
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 是否生成任务单
        /// </summary>
        public bool IsGenerateTask
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
        /// <summary>
        /// 是否需求任务清单
        /// </summary>
        public bool IsRequirementTask
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
        

        /// <summary>
        /// 是否扣料
        /// </summary>
        public bool IsBuckleMaterial
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 起始工序
        /// </summary>
        public double? StartProcess
        {
            get { return GetProperty<double?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 正常胜制
        /// </summary>
        public double? NormalVictory
        {
            get { return GetProperty<double?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 维修胜制
        /// </summary>
        public double? RepairVictory
        {
            get { return GetProperty<double?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 是否加严
        /// </summary>
        public bool IsStricter
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 超时时间（分钟）
        /// </summary>
        public int? Overtime
        {
            get { return GetProperty<int?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 直通率取值
        /// </summary>
        public bool IsPassRate
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 绑定
        /// </summary>
        public bool IsBinding
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 解绑
        /// </summary>
        public bool IsUnBinding
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Index
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 最大过站次数
        /// </summary>
        public int? MaxPassNum
        {
            get { return GetProperty<int?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 是否下工序入站
        /// </summary>
        public bool? IsNextMoveIn
        {
            get { return GetProperty<bool?>(); }
            set { SetProperty(value); }
        }

        

        /// <summary>
        /// 工序状态
        /// </summary>
        public ProcessState ProcessState
        {
            get { return GetProperty<ProcessState>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType ProcessType
        {
            get { return GetProperty<ProcessType>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 是否工序组
        /// </summary>
        public bool IsGroup
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
        /// <summary>
        /// 工序组Id
        /// </summary>

        public string GroupId
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 是否委外
        /// </summary>
        public bool IsOutsourcing
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 线节点个数
        /// </summary>
        public int NodeCount
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 开始节点的锚点数
        /// </summary>
        public int SourceAnchorCount
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }



        /// <summary>
        /// 启用入站控制
        /// </summary>
        public bool? EnableMoveInControl
        {
            get { return GetProperty<bool?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 工序交接类型
        /// </summary>
        public TransferType? TransferType
        {
            get { return GetProperty<TransferType?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 父节点
        /// </summary>
        public string ParentNodeId
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 复制Id
        /// </summary>
        public string CopyId
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public double? LayoutInfoId
        {
            get { return GetProperty<double?>(); }
            set { SetProperty(value); }
        }

        public string Vornr
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Steus
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="point">位置</param>
        public void SetPoint(Point point)
        {
            Left = point.X - Width / 2;
            Top = point.Y - ContainerHeight / 2;
            Move();
        }

        /// <summary>
        /// 获取位置
        /// </summary>
        /// <returns>位置</returns>
        public Point GetPoint()
        {
            return new Point(Left + Width / 2, Top + ContainerHeight / 2);
        }

        /// <summary>
        /// 判断点是否在元素内部
        /// </summary>
        /// <param name="point">位置</param>
        /// <returns>在元素内部返回true，否则返回false</returns>
        public bool PointIsInside(Point point)
        {
            if (Left < point.X && point.X < Left + Width && Top < point.Y && point.Y < Top + ContainerHeight)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取底部中心位置
        /// </summary>
        /// <returns>位置</returns>
        public Point GetBottomPoint()
        {
            return new Point(Left + Width / 2, Top + ContainerHeight / 2 + 2);
        }

        /// <summary>
        /// 获取底部中心位置
        /// </summary>
        /// <returns>位置</returns>
        public Point GetTopPoint()
        {
            return new Point(Left + Width / 2, Top);
        }

        /// <summary>
        /// 删除活动节点
        /// </summary>
        public override void Delete()
        {
            if (Type != ActivityType.Interaction)
                throw new ValidationException("该工序为：{0} 不允许删除".L10nFormat(EnumViewModel.EnumToLabel(Type).L10N()));
            foreach (var rule in BeginRules)
            {
                if (rule.EndActivity != null && rule.EndActivity != this)
                {
                    rule.EndActivity.EndRules.Remove(rule);
                }
            }

            foreach (var rule in EndRules)
            {
                if (rule.BeginActivity != null && rule.BeginActivity != this)
                {
                    rule.BeginActivity.BeginRules.Remove(rule);
                    rule.BeginActivity.Rules.Add(rule);
                }
            }
        }

        /// <summary>
        /// 序列化活动节点
        /// </summary>
        /// <returns>序列化xml</returns>
        public override string Serialize()
        {
            XElement el = new XElement("Activity",
                new XElement("Boms", Bom.Select(p =>
                {
                    var bom = new XElement("Bom");
                    bom.SetAttributeValue(ITEM_ID, p.ItemId);
                    bom.SetAttributeValue("Qty", p.Qty);
                    bom.SetAttributeValue("ItemExtProp", p.ItemExtProp);
                    bom.SetAttributeValue("ItemExtPropName", p.ItemExtPropName);
                    return bom;
                })),
                new XElement("ProcessBoms", ProcessBoms.Select(p =>
                {
                    var processBom = new XElement("ProcessBom");
                    processBom.SetAttributeValue(ITEM_ID, p.ItemId);
                    processBom.SetAttributeValue("Qty", p.Qty);
                    processBom.SetAttributeValue("IsBuckleMaterial", p.IsBuckleMaterial);
                    processBom.SetAttributeValue("Point", p.Point);
                    processBom.SetAttributeValue("WorkStepId", p.WorkStepId);
                    processBom.SetAttributeValue("IsAttachment", p.IsAttachment);
                    processBom.SetAttributeValue("IsExternal", p.IsExternal);
                    processBom.SetAttributeValue("IsSingleLabel", p.IsSingleLabel);
                    processBom.SetAttributeValue("IsRepeat", p.IsRepeat);
                    processBom.SetAttributeValue("HasBarcodeRule", p.HasBarcodeRule);
                    processBom.SetAttributeValue("MainMaterialId", p.MainMaterialId);
                    return processBom;
                })),
                new XElement("Fixtures", Fixtures.Select(p =>
                {
                    var fixture = new XElement("Fixture");
                    fixture.SetAttributeValue("Id", p.Id);
                    return fixture;
                })));

            el.SetAttributeValue(nameof(Id), Id);
            el.SetAttributeValue(nameof(State), State);
            el.SetAttributeValue(nameof(IsSelected), IsSelected);
            el.SetAttributeValue(nameof(ZIndex), ZIndex);
            el.SetAttributeValue(nameof(Type), Type);
            el.SetAttributeValue(nameof(Text), Text);
            el.SetAttributeValue(nameof(ProcessId), ProcessId);
            el.SetAttributeValue(nameof(LayoutInfoId), LayoutInfoId);
            el.SetAttributeValue(nameof(Vornr), Vornr);
            el.SetAttributeValue(nameof(Steus), Steus);
            el.SetAttributeValue(nameof(Width), Width);
            el.SetAttributeValue(nameof(Height), Height);
            el.SetAttributeValue(nameof(ContainerHeight), ContainerHeight);
            el.SetAttributeValue(nameof(Left), Left);
            el.SetAttributeValue(nameof(Top), Top);
            el.SetAttributeValue(nameof(IsOptional), IsOptional);
            el.SetAttributeValue(nameof(IsRepeat), IsRepeat);
            el.SetAttributeValue(nameof(CreateSku), CreateSku);
            el.SetAttributeValue(nameof(IsCalculate), IsCalculate);
            el.SetAttributeValue(nameof(IsGenerateTask), IsGenerateTask);
            el.SetAttributeValue(nameof(IsRequirementTask), IsRequirementTask);
            
            el.SetAttributeValue(nameof(IsBuckleMaterial), IsBuckleMaterial);
            el.SetAttributeValue(nameof(IsPassRate), IsPassRate);
            el.SetAttributeValue(nameof(IsBinding), IsBinding);
            el.SetAttributeValue(nameof(IsUnBinding), IsUnBinding);
            el.SetAttributeValue(nameof(StartProcess), StartProcess);
            el.SetAttributeValue(nameof(NormalVictory), NormalVictory);
            el.SetAttributeValue(nameof(RepairVictory), RepairVictory);
            el.SetAttributeValue(nameof(IsStricter), IsStricter);
            el.SetAttributeValue(nameof(Overtime), Overtime);
            el.SetAttributeValue(nameof(ProcessState), ProcessState);
            el.SetAttributeValue(nameof(ProcessType), ProcessType);
            el.SetAttributeValue(nameof(Index), Index);
            el.SetAttributeValue(nameof(MaxPassNum), MaxPassNum);
            el.SetAttributeValue(nameof(IsNextMoveIn), IsNextMoveIn);

            el.SetAttributeValue(nameof(EnableMoveInControl), EnableMoveInControl);
            el.SetAttributeValue(nameof(TransferType), (int?)TransferType);
            el.SetAttributeValue(nameof(ParentNodeId), ParentNodeId);
            el.SetAttributeValue(nameof(GroupId), GroupId);
            el.SetAttributeValue(nameof(IsGroup), IsGroup);
            el.SetAttributeValue(nameof(ProcessParameter), JsonConvert.SerializeObject(ProcessParameter));
            el.SetAttributeValue(nameof(NodeCount), NodeCount);
            el.SetAttributeValue(nameof(SourceAnchorCount), SourceAnchorCount);
            el.SetAttributeValue(nameof(IsOutsourcing), IsOutsourcing);

            return el.ToString();
        }

        /// <summary>
        /// 反序列化活动节点
        /// </summary>
        /// <param name="xml">序列化xml</param>
        /// <param name="isCopy"></param>
        public override void Deserialize(string xml, bool isCopy = false)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            XElement xelement = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(bytes)));
            GetXmlNodeDataValues(xelement);
            if (Type == ActivityType.Initial)
            {
                SourceAnchorCount = xelement.GetAttributeValue<int>(nameof(SourceAnchorCount), SourceAnchorCount);
            }
            var processParameterJson = xelement.GetAttributeValue(nameof(ProcessParameter), "");
            if (!processParameterJson.IsNullOrEmpty())
            {
                ProcessParameter = JsonConvert.DeserializeObject<EntityList<ProcessParameter>>(processParameterJson);
            }
            NodeCount = xelement.GetAttributeValue(nameof(NodeCount), 0);

            ParentNodeId = xelement.GetAttributeValue(nameof(ParentNodeId), string.Empty);
            if (!xelement.GetAttributeValue(nameof(TransferType), string.Empty).IsNullOrEmpty() && xelement.GetAttributeValue(nameof(TransferType), string.Empty) != "undefined")
                TransferType = xelement.GetAttributeValue<TransferType?>(nameof(TransferType), null);
            var itemIds = new List<double>();
            foreach (XElement node in xelement.Descendants("Bom"))
            {
                GetNodeBomValues(itemIds, node);
            }
            if (itemIds.Count > 0)
            {
                var items = RT.Service.Resolve<ItemController>().GetItemList(itemIds);
                if (items.Any())
                {
                    foreach (var item in items)
                    {
                        var itemBom = Bom.FirstOrDefault(m => m.ItemId == item.Id);
                        if (itemBom != null)
                        {
                            itemBom.Code = item.Code;
                            itemBom.Name = item.Name;
                        }
                    }
                }
            }
            foreach (XElement node in xelement.Descendants("ProcessBom"))
            {
                GetProcessBomsValues(node);
            }
            foreach (XElement node in xelement.Descendants("Fixture"))
            {
                var id = node.GetAttributeValue("Id", 0d);
                if (id != 0)
                    Fixtures.Add(new FixtureInfo() { Id = id });
            }
        }

        /// <summary>
        /// 获取Xml中属性值
        /// </summary>
        /// <param name="xelement"></param>
        private void GetXmlNodeDataValues(XElement xelement)
        {
            Id = xelement.GetAttributeValue(nameof(Id), string.Empty);
            State = (ElementState)EnumViewModel.LabelToEnum(xelement.GetAttributeValue(nameof(State), string.Empty), typeof(ElementState));
            IsOptional = xelement.GetBoolAttributeValue(nameof(IsOptional));
            IsSelected = xelement.GetBoolAttributeValue(nameof(IsSelected));
            IsRepeat = xelement.GetBoolAttributeValue(nameof(IsRepeat));
            CreateSku = xelement.GetBoolAttributeValue(nameof(CreateSku));
            IsCalculate = xelement.GetBoolAttributeValue(nameof(IsCalculate));
            IsGenerateTask = xelement.GetBoolAttributeValue(nameof(IsGenerateTask));
            IsRequirementTask = xelement.GetBoolAttributeValue(nameof(IsRequirementTask));
            
            IsBuckleMaterial = xelement.GetBoolAttributeValue(nameof(IsBuckleMaterial));
            IsPassRate = xelement.GetBoolAttributeValue(nameof(IsPassRate));
            IsBinding = xelement.GetBoolAttributeValue(nameof(IsBinding));
            IsUnBinding = xelement.GetBoolAttributeValue(nameof(IsUnBinding));
            StartProcess = xelement.GetAttributeValue<double?>(nameof(StartProcess), null);
            Overtime = xelement.GetAttributeValue<int?>(nameof(Overtime), null);
            NormalVictory = xelement.GetAttributeValue<double?>(nameof(NormalVictory), null);
            RepairVictory = xelement.GetAttributeValue<double?>(nameof(RepairVictory), null);
            IsStricter = xelement.GetBoolAttributeValue(nameof(IsStricter));
            Text = xelement.GetAttributeValue(nameof(Text), string.Empty);
            ProcessId = xelement.GetAttributeValue(nameof(ProcessId), 0d);
            LayoutInfoId = xelement.GetAttributeValue<double?>(nameof(LayoutInfoId), null);
            Vornr= xelement.GetAttributeValue(nameof(Vornr), string.Empty);
            Steus = xelement.GetAttributeValue(nameof(Steus), string.Empty);
            Width = xelement.GetAttributeValue(nameof(Width), 0d);
            Height = xelement.GetAttributeValue(nameof(Height), 0d);
            ContainerHeight = xelement.GetAttributeValue(nameof(ContainerHeight), 0d);
            Left = xelement.GetAttributeValue(nameof(Left), 0d);
            Top = xelement.GetAttributeValue(nameof(Top), 0d);
            Index = xelement.GetAttributeValue(nameof(Index), -1);
            MaxPassNum = xelement.GetAttributeValue<int?>(nameof(MaxPassNum), null);

            IsNextMoveIn = xelement.GetAttributeValue<bool?>(nameof(IsNextMoveIn), null);
            EnableMoveInControl = xelement.GetAttributeValue<bool?>(nameof(EnableMoveInControl), null);
            Type = (ActivityType)EnumViewModel.LabelToEnum(xelement.GetAttributeValue(nameof(Type), string.Empty), typeof(ActivityType));
            ProcessState = (ProcessState)EnumViewModel.LabelToEnum(xelement.GetAttributeValue(nameof(ProcessState), ProcessState.Not.ToString()), typeof(ProcessState));
            ProcessType = (ProcessType)EnumViewModel.LabelToEnum(xelement.GetAttributeValue(nameof(ProcessType), ProcessType.Assembly.ToString()), typeof(ProcessType));
            IsGroup = xelement.GetAttributeValue(nameof(IsGroup), "") == "true";//是否工序组
            GroupId = xelement.GetAttributeValue(nameof(GroupId), "");
            IsOutsourcing = xelement.GetBoolAttributeValue(nameof(IsOutsourcing));
        }

        /// <summary>
        /// 获取节点BOM属性值
        /// </summary>
        /// <param name="itemIds"></param>
        /// <param name="node"></param>
        private void GetNodeBomValues(List<double> itemIds, XElement node)
        {
            var newItemExpBom = new ItemExtBom();
            newItemExpBom.ItemId = node.GetAttributeValue("ItemId", 0d);
            newItemExpBom.Qty = node.GetAttributeValue("Qty", 0m);
            newItemExpBom.ItemExtProp = node.GetAttributeValue("ItemExtProp", "");
            newItemExpBom.ItemExtPropName = node.GetAttributeValue("ItemExtPropName", "");
            if (newItemExpBom.ItemId != 0)
                itemIds.Add(newItemExpBom.ItemId);
            Bom.Add(newItemExpBom);
        }

        /// <summary>
        /// 获取节点的ProcessBom值
        /// </summary>
        /// <param name="node"></param>
        private void GetProcessBomsValues(XElement node)
        {
            var itemId = node.GetAttributeValue("ItemId", 0d);
            var qty = node.GetAttributeValue<decimal>("Qty", 0);
            var isBuckleMaterial = node.GetBoolAttributeValue("IsBuckleMaterial");
            var point = node.GetAttributeValue<string>("Point", string.Empty);
            var workStepId = node.GetAttributeValue<double?>("WorkStepId", null);
            var isAttachment = node.GetBoolAttributeValue("IsAttachment");
            var isExternal = node.GetBoolAttributeValue("IsExternal");
            var isSingleLabel = node.GetBoolAttributeValue("IsSingleLabel");
            var isRepeat = node.GetBoolAttributeValue("IsRepeat");
            var hasBarcodeRule = node.GetBoolAttributeValue("HasBarcodeRule");
            var mainMaterialId = node.GetAttributeValue<double?>("MainMaterialId", null);

            if (itemId > 0)
            {
                ProcessBoms.Add(new ProcessBom()
                {
                    ItemId = itemId,
                    Qty = qty,
                    IsBuckleMaterial = isBuckleMaterial,
                    Point = point,
                    WorkStepId = workStepId,
                    IsAttachment = isAttachment,
                    IsExternal = isExternal,
                    IsSingleLabel = isSingleLabel,
                    IsRepeat = isRepeat,
                    HasBarcodeRule = hasBarcodeRule,
                    MainMaterialId = mainMaterialId
                });
            }
        }

        /// <summary>
        /// 保存验证
        /// </summary>
        public override void ValidateSave()
        {
            if (!IsGroup)//非工序组才验证
            {
                if (Type == ActivityType.Interaction && ProcessId <= 0)
                    throw new ValidationException("工序不能为空".L10N());
                if (Rules.Count > 0)
                    throw new ValidationException("工序：{0} 还有 {1} 个参数未进行连线".L10nFormat(Text, Rules.Count));
                if (BeginRules.Any(p => p.EndActivity.Type == ActivityType.Completion) && IsOptional)
                    throw new ValidationException("工序[{0}]是结束工序，不可以为可选工序".L10nFormat(Text));
                if (BeginRules.Any(p => p.EndActivity.Type == ActivityType.Completion) && IsRepeat)
                    throw new ValidationException("工序[{0}]是结束工序，不可以为重复工序".L10nFormat(Text));
                if (ProcessBoms.GroupBy(p => p.ItemId).Any(p => p.Count() > 1))
                    throw new ValidationException("工序[{0]工序BOM存在多个重复物料".L10nFormat(Text));
                var itemIds = Bom.Select(m => m.ItemId).ToList();
                if (itemIds.Any())
                {
                    var bomItems = RT.Service.Resolve<ItemController>().GetItemList(itemIds, new EagerLoadOptions().LoadWithViewProperty());
                    foreach (var bom in Bom)
                    {
                        var item = bomItems.FirstOrDefault(m => m.Id == bom.ItemId);
                        if (item != null && item.EnableExtendProperty && bom.ItemExtProp.IsNullOrEmpty())
                        {
                            throw new ValidationException("工序BOM中物料[{0}]启用了扩展属性，必须选择扩展属性".L10nFormat(item.Name));
                        }
                    }
                }
                if (ProcessBoms.GroupBy(p => p.ItemId).Any(p => p.Count() > 1))
                    throw new ValidationException("工序[{0]工序BOM存在多个重复物料".L10nFormat(Text));
            }
        }
    }
}
