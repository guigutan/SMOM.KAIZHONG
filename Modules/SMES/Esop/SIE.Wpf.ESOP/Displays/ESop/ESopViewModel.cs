using Newtonsoft.Json;
using SIE.Common.Configs;
using SIE.Common.ImportHelper;
using SIE.Common.Sort;
using SIE.Common.Utils;
using SIE.Core.Items;
using SIE.Core.WorkOrders;
using SIE.Dashboard.Definitions;
using SIE.Dashboard.ViewModels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ESop.Configs;
using SIE.ESop.Displays;
using SIE.ESop.Displays.Configs;
using SIE.ESop.Documents;
using SIE.ESop.EngDocuments.Services;
using SIE.ManagedProperty;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Tech.Processs;
using SIE.Wpf.ESop.Properties;
using SIE.Wpf.ESOP.Common;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace SIE.Wpf.ESop.Displays
{
    /// <summary>
    /// ESop视图模型
    /// </summary>
    [RootEntity]
    [Label("电子指导书")]
    public class ESopViewModel : DashboardViewModel, IEsopPlay
    {
        #region 工序 Process 
        /// <summary>
        /// 工序Id
        /// </summary>
        public static readonly IRefIdProperty ProcessIdProperty = P<ESopViewModel>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ESopViewModel>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 物料 Item 
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<ESopViewModel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<ESopViewModel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<ESopViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<ESopViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 文档集合 DocumentList
        /// <summary>
        /// 文档集合
        /// </summary>
        public static readonly ListProperty<EntityList<Document>> DocumentListProperty = P<ESopViewModel>.RegisterList(e => e.DocumentList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as ESopViewModel).LoadDocumentList()
        });

        /// <summary>
        /// 文档集合
        /// </summary>
        public EntityList<Document> DocumentList
        {
            get { return this.GetLazyList(DocumentListProperty); }
        }

        /// <summary>
        /// 创建新的文档集合
        /// </summary>
        /// <returns>返回新的文档集合</returns>
        private EntityList<Document> LoadDocumentList()
        {
            return new EntityList<Document>();
        }
        #endregion

        #region 当前播放文档 CurrentPlayDocument
        /// <summary>
        /// 当前播放文档
        /// </summary>
        public static readonly Property<Document> CurrentPlayDocumentProperty = P<ESopViewModel>.RegisterReadOnly(e => e.CurrentPlayDocument, e => e.GetCurrentPlayDocument());

        /// <summary>
        /// 当前播放文档
        /// </summary>
        public Document CurrentPlayDocument
        {
            get { return this.GetProperty(CurrentPlayDocumentProperty); }
        }

        /// <summary>
        /// 当前播放文档
        /// </summary>
        /// <returns>返回当前播放的文档</returns>
        private Document GetCurrentPlayDocument()
        {
            return DocumentPlaySort?.CurrentDocument ?? new Document { FilePath = @"pack://application:,,,/SIE.Wpf.ESop;component/Images/PauseNormalRed.png", DocumentType = DocumentType.Img };
        }
        #endregion

        #region 属性
        /// <summary>
        /// 文档播放排序类
        /// </summary>
        public DocumentPlaySort DocumentPlaySort { get; private set; }

        /// <summary>
        /// 显示点的配置信息
        /// </summary>
        public DisplayPointConfigValue Config
        {
            get; private set;
        }

        /// <summary>
        /// 显示点数据源配置项
        /// </summary>
        public DisplayPointDataConfigValue DataFromConfig { get; private set; }

        /// <summary>
        /// 播放间隔
        /// </summary>
        public int Interval
        {
            get
            {
                return Workstation?.DisplayPoint?.PlaySpace ?? Config.Interval;
            }
        }

        /// <summary>
        /// 工作站
        /// </summary>
        public EsopWorkstation Workstation { get { return _workstation ?? (_workstation = new EsopWorkstation()); } }

        /// <summary>
        /// 工作单元信息
        /// </summary>
        private ESopWorkcell _workcell;

        /// <summary>
        /// 工作站信息
        /// </summary>
        private EsopWorkstation _workstation;

        /// <summary>
        /// 工作站属性是否正在变更
        /// </summary>
        private bool isWorkstationPropertyChanging;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public string MetaKey;


        /// <summary>
        /// 构造函数，读取配置信息和创建文档播放排序类
        /// </summary>
        public ESopViewModel()
        {
            // 播放间隔配置
            Config = ConfigService.GetConfig(new DisplayConfig(), typeof(DisplayPoint));
            // 数据源配置
            DataFromConfig = ConfigService.GetConfig(new DisplayPointDataConfig(), typeof(DisplayPoint));
            DocumentPlaySort = new DocumentPlaySort(this);
        }

        /// <summary>
        /// 初始化工位信息
        /// </summary>
        protected virtual void LoadItemData()
        {
            try
            {
                if (!Workstation.DisplayPointId.HasValue)
                {
                    Item = null;
                    WorkOrder = null;
                    return;
                }

                var workcell = GetWorkcell();
                WipResourceWorkOrder wipLineWorkOrder = null;
                if (!workcell.StationId.HasValue)
                    wipLineWorkOrder = RT.Service.Resolve<WipController>().GetWipResourceWorkOrder(workcell.ResourceId);
                else
                    wipLineWorkOrder = RT.Service.Resolve<WipController>().GetWipResourceWorkOrder(new Workcell
                    {
                        ProcessId = this.ProcessId ?? 0,
                        StationId = workcell.StationId ?? 0,
                        ResourceId = workcell.ResourceId,
                        EmployeeId = workcell.UserId
                    });
                if (wipLineWorkOrder != null)
                {
                    if (wipLineWorkOrder.WorkOrderId != WorkOrderId)
                    {
                        var workOrder = RF.GetById<WorkOrder>(wipLineWorkOrder.WorkOrderId);
                        this.Item = workOrder.Product;
                        this.WorkOrderId = workOrder.Id;
                    }
                }
                else
                {
                    this.Item = null;
                    this.WorkOrder = null;
                }
            }
            catch (Exception exc)
            {
                throw new ValidationException(exc.GetBaseException().Message);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void OnClose()
        {
            object enableProces = null;
            SIE.Context.AppContext.Items.TryGetValue("ESop.EnableProces", out enableProces);
            if (((bool?)enableProces ?? false))
            {
                RT.EventBus.Unsubscribe<Workcell>(this);
            }

            RT.RemotingEventBus.Unsubscribe<ChangeWipResourceWorkOrderEvent>(this);
            Workstation.PropertyChanged -= OnWorkstationPropertyChanged;
        }

        /// <summary>
        /// 初始化工位信息
        /// </summary>
        public void Onload()
        {
            Application.Current.MainWindow.Dispatcher.BeginInvoke(new Action(() =>
            {
                LoadItemData();
            }));
        }

        #region 工作单元信息

        /// <summary>
        /// 初始化工作站信息并返回显示点Id
        /// </summary>
        public double? InitWorkstation()
        {
            Workstation.UserId = AppRuntime.IdentityId;

            Workstation.PropertyChanged += OnWorkstationPropertyChanged;

            if (!LoadWorkstation()) //如果工作站信息不存在，或者与上次登录用户的资源工序工位分配不一样，重新选择
            {
                ESopWorkstationSelector.SelectOperation(Workstation);
                SaveWorkstation();
            }
            return Workstation.DisplayPointId;
        }

        /// <summary>
        /// 工作单元属性变更
        /// </summary>
        /// <param name="sender">当前工作站</param>
        /// <param name="e">事件参数</param>
        private void OnWorkstationPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ////防止重复触发
            if (e.PropertyName == nameof(Workstation.UserId) || e.PropertyName == nameof(Workstation.ResourceId))
                return;

            if (e.PropertyName == nameof(Workstation.User) && Workstation.User != null)
            {
                if (!Workstation.ResourceId.HasValue || !CheckUserWorkStation(Workstation.UserId.Value, Workstation.ResourceId.Value, Workstation.DisplayPointId.Value))
                {
                    isWorkstationPropertyChanging = true;
                    Workstation.DisplayPoint = null;
                    isWorkstationPropertyChanging = false;
                    ESopWorkstationSelector.SelectOperation(Workstation);
                }
            }

            if (!isWorkstationPropertyChanging)
                WorkstationChanged();
        }

        /// <summary>
        /// 工作单元信息改变
        /// </summary>
        protected virtual void WorkstationChanged()
        {
            LoadItemData();
            _workcell = null;
            SaveWorkstation();
        }

        /// <summary>
        /// 保存工作单元信息
        /// </summary>
        private void SaveWorkstation()
        {
            if (Workstation == null)
                throw new PlatformException("工作单元未初始化".L10N());
            var workcell = new ESopWorkcell()
            {
                UserId = Workstation.UserId ?? 0,
                ResourceId = Workstation.ResourceId ?? 0,
                DisplayPointId = Workstation.DisplayPointId ?? 0,
            };

            var setting = Settings.Default.Workcell;
            Dictionary<string, ESopWorkcell> data = null;
            if (setting.IsNotEmpty())
                data = JsonConvert.DeserializeObject<Dictionary<string, ESopWorkcell>>(setting);
            if (data == null)
                data = new Dictionary<string, ESopWorkcell>();
            var key = MetaKey; //GetType().GetQualifiedName();
            data[key] = workcell;
            Settings.Default.Workcell = JsonConvert.SerializeObject(data);
            Settings.Default.Save();
        }

        /// <summary>
        /// 加载工作单元信息
        /// </summary>
        /// <returns>返回是否正常加载数据</returns>
        private bool LoadWorkstation()
        {
            var setting = Settings.Default.Workcell;
            if (setting.IsNotEmpty())
            {
                var workcells = JsonConvert.DeserializeObject<Dictionary<string, ESopWorkcell>>(setting);
                var key = MetaKey;//GetType().GetQualifiedName();
                if (workcells.ContainsKey(key))   //匹配工作单元
                {
                    var workcell = workcells[key];
                    if (workcell.DisplayPointId == 0 || workcell.ResourceId == 0)
                        return false;
                    //// 如果与上次登录用户的资源工序工位分配不一样，打开时需要重新选
                    if (!CheckUserWorkStation(AppRuntime.IdentityId, workcell.ResourceId, workcell.DisplayPointId))
                        return false;
                    Workstation.UserId = AppRuntime.IdentityId;
                    Workstation.ResourceId = workcell.ResourceId;
                    Workstation.DisplayPointId = workcell.DisplayPointId;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否存在用户、资源对应的显示点
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="resourceId">产线</param>
        /// <param name="displayPointId">显示点</param>
        /// <returns>返回是否存在用户、资源对应的显示点</returns>       
        private bool CheckUserWorkStation(double userId, double resourceId, double displayPointId)
        {
            var ctlResource = RT.Service.Resolve<EmployeeController>();
            if (!ctlResource.UserHasResource(userId, resourceId))
                return false;

            var hasPoint = RT.Service.Resolve<DisplayPointController>().HasResourceDisplayPoint(resourceId, displayPointId);
            if (!hasPoint)
                return false;
            return true;
        }

        /// <summary>
        /// 获取采集单元信息
        /// </summary>
        /// <returns>返回创建的采集单元信息</returns>
        public ESopWorkcell GetWorkcell()
        {
            if (_workcell == null)
            {
                _workcell = new ESopWorkcell();
            }

            var broken = Workstation.Validate(ValidatorActions.None);
            if (broken.Count > 0)
            {
                CRT.MessageService.ShowMessage(broken.ToString());
                return _workcell;
            }

            _workcell.UserId = Workstation.UserId ?? 0;
            _workcell.ResourceId = Workstation.ResourceId ?? 0;
            _workcell.DisplayPointId = Workstation.DisplayPointId ?? 0;
            _workcell.StationId = Workstation.StationId;
            return _workcell;
        }

        #endregion

        /// <summary>
        /// 运行看板，要实现加载数据，定时刷新数据
        /// </summary>
        /// <param name="setting">看板配置</param>
        public override void Initialize(DashboardSettings setting)
        {
            Onload();

            object enableProces = null;

            try
            {
                if (!SIE.Context.AppContext.Items.TryGetValue("ESop.EnableProces", out enableProces) || !(bool)enableProces)
                {
                    RT.RemotingEventBus.Subscribe<ChangeWipResourceWorkOrderEvent>(this, cmd =>
                    {
                        CRT.MainThread.InvokeAsync(() =>
                        {
                            this.ItemId = RF.GetById<WorkOrder>(cmd.WorkOrderId)?.ProductId;
                            this.WorkOrderId = cmd.WorkOrderId;
                        });
                    });
                }
                else
                {
                    RT.EventBus.Subscribe<ChangeWipResourceWorkOrderEvent>(this, c =>
                    {
                        CRT.MainThread.InvokeAsync(() =>
                        {
                            this.ItemId = RF.GetById<WorkOrder>(c.WorkOrderId)?.ProductId;
                            this.WorkOrderId = c.WorkOrderId;
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainWindow.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ex.Alert();
                }));
            }
        }

        /// <summary>
        /// 设置工作站信息
        /// </summary>
        /// <param name="workcell">工作站</param>
        /// <param name="displayerPoint">显示点</param>
        public void SetWorkstation(Workstation workcell, DisplayPoint displayerPoint)
        {
            Workstation.UserId = workcell.EmployeeId;
            Workstation.ResourceId = workcell.ResourceId;
            Workstation.DisplayPointId = displayerPoint?.Id;
            Workstation.StationId = workcell.StationId;
            ProcessId = workcell.ProcessId;
        }

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="e">变更事件参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            if (((e.Property == ESopViewModel.ItemProperty) || (e.Property == ESopViewModel.WorkOrderIdProperty) && (this.ItemId.HasValue && this.WorkOrderId.HasValue)))
            {
                ItemChanged();
                DocumentPlaySort.ResetOne();
            }

            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// 物料变更
        /// </summary>
        private void ItemChanged()
        {
            GetStationDocument(this);
            this.NotifyPropertyChanged(ESopViewModel.CurrentPlayDocumentProperty);
        }

        private EntityList<Document> GetDocuments(ESopViewModel eSopViewModel)
        {
            EntityList<Document> data = new EntityList<Document>();
            if (DataFromConfig.DataFrom == SIE.ESop.Displays.Enums.DisplayDataSource.Document)
            {
                data = RT.Service.Resolve<DocumentCollectionController>().GetList(new DocumentCriteria() { ProcessId = eSopViewModel.ProcessId, WorkOrderId = eSopViewModel.WorkOrderId });
                if (data.Count == 0)
                    data = RT.Service.Resolve<DocumentCollectionController>().GetList(new DocumentCriteria() { ItemId = eSopViewModel.ItemId, ProcessId = eSopViewModel.ProcessId });
            }
            else
            {
                data = RT.Service.Resolve<EngDocumentDetailService>().GetDocuments(eSopViewModel.WorkOrderId, eSopViewModel.ItemId);
            }
            return data;
        }

        /// <summary>
        /// 根据不同数据源决定excel播放特定页
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private string[] GetExcelSheet(Document doc)
        {
            if (DataFromConfig.DataFrom == SIE.ESop.Displays.Enums.DisplayDataSource.Document)
            {
                var excel = new ExcelHelper(doc.FilePath);
                return excel.ExcelSheetNames();
            }
            else
            {
                return new string[] { doc.SheetName };
            }
        }

        /// <summary>
        /// 获取工作站对应的文档集
        /// </summary>
        public void GetStationDocument(ESopViewModel eSopViewModel)
        {
            eSopViewModel.DocumentList.Clear();
            if (eSopViewModel.Workstation == null || eSopViewModel.Workstation.DisplayPoint == null || eSopViewModel.Workstation.DisplayPoint.DisplayPointProcessList == null) return;
            var processIds = eSopViewModel.Workstation.DisplayPoint.DisplayPointProcessList.Select(p => p.ProcessId).ToArray();
            // 根据数据源配置项决定读取文件 
            var data = GetDocuments(eSopViewModel);
            var docList = data.Where(f => processIds.Contains(f.ProcessId) && f.IsProcessed).OrderBy(f => SortExtension.GetIndex(f)).ToList();
            if (eSopViewModel.ProcessId.HasValue) docList = docList.Where(f => f.ProcessId == eSopViewModel.ProcessId).ToList();
            foreach (var doc in docList)
            {
                LoadDocumentData(doc);
            }

            for (int i = 0; i < docList.Count; i++)
            {
                if ((new string[] { ".xlsx", ".xls" }).Contains(docList[i].FileExtension.ToLower()) && docList[i].FilePath.IsNotEmpty() && docList[i].Md5.IsNotEmpty() && docList[i].IsProcessed && docList[i].FileSize.IsNotEmpty())
                {
                    var names = GetExcelSheet(docList[i]);
                    for (int j = 0; j < names.Length; j++)
                    {
                        if (j == 0)
                        {
                            docList[i].FileName = names[j];
                        }
                        else
                        {
                            var doc = new Document
                            {
                                Id = docList[i].Id + j * 0.001,
                                Code = docList[i].Code,
                                Name = docList[i].Name,
                                DocumentCollection = docList[i].DocumentCollection,
                                DocumentType = docList[i].DocumentType,
                                FileExtension = docList[i].FileExtension,
                                FileName = names[j],
                                FilePath = docList[i].FilePath,
                                FileSize = docList[i].FileSize,
                                IsProcessed = docList[i].IsProcessed,
                                Md5 = docList[i].Md5,
                                Process = docList[i].Process,
                                Source = docList[i].Source
                            };
                            SortExtension.SetIndex(doc, SortExtension.GetIndex(docList[i]));
                            docList.Insert(i + j, doc);
                        }
                    }

                    i = i + (names.Length - 1);
                }
            }
            eSopViewModel.DocumentList.AddRange(docList);
        }

        /// <summary>
        /// 加载文档数据
        /// </summary>
        /// <param name="doc">文档</param>
        public void LoadDocumentData(Document doc)
        {
            if (doc.FilePath.IsNullOrEmpty() && doc.DocumentCollection.FilePath.IsNullOrEmpty())
                return;
            var collection = doc.DocumentCollection;
            string filePath = doc.FilePath.IsNullOrEmpty() ? collection.FilePath : doc.FilePath;
            string md5 = doc.FilePath.IsNullOrEmpty() ? collection.Md5 : doc.Md5;
            string fileName = Path.GetFileName(filePath);
            //创建临时目录
            var esopPath = RT.Service.Resolve<DocumentCollectionController>().GetESopDir();
            var tempPath = Path.Combine(Path.GetTempPath(), esopPath);
            Directory.CreateDirectory(tempPath);
            var tempFile = Path.Combine(tempPath, fileName);
            //判断文件是否已经下载 MD5对比
            bool needLoad = true;
            if (File.Exists(tempFile))
            {
                needLoad = false;
                var localMd5 = FileHelper.ComputeHash(new FileInfo(tempFile));
                if (md5 == localMd5)
                {
                    doc.FilePath = tempFile;
                    doc.FileExtension = Path.GetExtension(tempFile);
                    doc.Md5 = md5;
                    return;
                }
            }
            if (needLoad)//不存在才下载
            {
                try
                {
                    DownloadHelper.FileDownload(filePath, tempPath);
                }
                catch (Exception ex)
                {
                    for (int i = 0; i < Config.ReTryCount; i++)
                    {
                        try
                        {
                            DownloadHelper.FileDownload(filePath, tempPath);
                        }
                        catch (Exception) when (i == Config.ReTryCount - 1)
                        {
                            throw new FileNotFoundException("文件[{0}]下载失败:".L10nFormat(filePath) + ex);
                        }
                    }
                }
            }
            doc.FilePath = tempFile;
            doc.FileExtension = Path.GetExtension(tempFile);
            doc.Md5 = md5;
        }

        /// <summary>
        /// 播放下一个文档
        /// </summary>
        public void PlayNextDocument()
        {
            LoadItemData();
            DocumentPlaySort.NextDocument();
            this.NotifyPropertyChanged(ESopViewModel.CurrentPlayDocumentProperty);
        }

        /// <summary>
        /// 播放前一个文档
        /// </summary>
        public void PlayPreviousDocument()
        {
            LoadItemData();
            DocumentPlaySort.PreviousDocument();
            this.NotifyPropertyChanged(ESopViewModel.CurrentPlayDocumentProperty);
        }
    }

    /// <summary>
    /// 文档播放排序类
    /// </summary>
    public class DocumentPlaySort
    {
        /// <summary>
        /// 当前播放索引
        /// </summary>
        public int CurrentIndex { get; private set; }

        /// <summary>
        /// Esop视图对象
        /// </summary>
        private ESopViewModel _viewModel { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model">ESop视图对象</param>
        public DocumentPlaySort(ESopViewModel model)
        {
            _viewModel = model;
        }

        /// <summary>
        /// 文档播放排序集合
        /// </summary>
        public IEnumerable<Document> DocumentPlaySorts
        {
            get { return _viewModel.DocumentList.OrderBy(p => SortExtension.GetIndex(p)).ThenBy(f => f.Id); }
        }

        /// <summary>
        /// 返回当前播放文档集
        /// </summary>
        public Document CurrentDocument
        {
            get
            {
                if (!DocumentPlaySorts.Any())
                    return null;
                if (CurrentIndex > DocumentPlaySorts.Count() - 1)
                    CurrentIndex = 0;
                return DocumentPlaySorts.Skip(CurrentIndex).First();
            }
        }

        /// <summary>
        /// 设置当前播放索引向后一个
        /// </summary>
        public void NextDocument()
        {
            if (CurrentIndex < DocumentPlaySorts.Count() - 1)
            {
                CurrentIndex++;
            }
            else
            {
                CurrentIndex = 0;
            }
        }

        /// <summary>
        /// 设置当前播放索引向前一个
        /// </summary>
        public void PreviousDocument()
        {
            if (CurrentIndex != 0)
            {
                CurrentIndex--;
            }
            else
            {
                CurrentIndex = DocumentPlaySorts.Count() - 1;
            }
        }

        /// <summary>
        /// 重置当前播放索引
        /// </summary>
        public virtual void Reset()
        {
            CurrentIndex = 0;
        }

        /// <summary>
        /// 更换Esop时下标重置-1
        /// </summary>
        public virtual void ResetOne()
        {
            CurrentIndex = -1;
        }
    }
}