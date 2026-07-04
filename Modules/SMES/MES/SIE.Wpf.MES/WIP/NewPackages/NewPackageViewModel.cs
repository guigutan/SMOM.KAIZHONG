using DevExpress.XtraRichEdit.Design;
using SIE.Barcodes;
using SIE.Common.Configs;
using SIE.Common.Prints;
using SIE.Core;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.PackingPrints;
using SIE.MES.WIP;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.NewPackages;
using SIE.MES.WIP.Packings.Configs;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Packages;
using SIE.Packages.Packages;
using SIE.Packages.Printables;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Tech.Stations.Configs;
using SIE.Threading;
using SIE.Wpf.Common.Prints;
using SIE.Wpf.MES.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP.NewPackages
{
    /// <summary>
    /// 包装采集（新）
    /// </summary>
    [RootEntity]
    [Label("包装采集（新）")]
    public class NewPackageViewModel : DataCollectionViewModel<NewPackageController>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NewPackageViewModel()
        {
            InitWorkstation(ProcessType.Packing);
            Printer = Settings.Default.NewPackingPrint;

        }

        /// <summary>
        /// 待扫描包装号
        /// </summary>
        public Queue<string> AdvanceBarcodeQueue { get; } = new Queue<string>();

        /// <summary>
        /// 待扫描包装号对应单位
        /// </summary>
        public Queue<PackingUnit> AdvancePackingUnit { get; set; } = new Queue<PackingUnit>();

        /// <summary>
        /// 当前条码
        /// </summary>
        public string CurrentBarcode { get; set; }

        #region 是否输入标签号 IsNeedPackageNo
        /// <summary>
        /// 是否输入标签号
        /// </summary>
        [Label("是否输入标签号")]
        public static readonly Property<bool> IsNeedPackageNoProperty = P<NewPackageViewModel>.Register(e => e.IsNeedPackageNo);

        /// <summary>
        /// 是否输入标签号
        /// </summary>
        public bool IsNeedPackageNo
        {
            get { return this.GetProperty(IsNeedPackageNoProperty); }
            set { this.SetProperty(IsNeedPackageNoProperty, value); }
        }
        #endregion

        #region 打印方式 PrintMode
        /// <summary>
        /// 打印方式
        /// </summary>
        [Label("打印方式")]
        public static readonly Property<PrintMode> PrintModeProperty = P<NewPackageViewModel>.Register(e => e.PrintMode);

        /// <summary>
        /// 打印方式
        /// </summary>
        public PrintMode PrintMode
        {
            get { return this.GetProperty(PrintModeProperty); }
            set { this.SetProperty(PrintModeProperty, value); }
        }
        #endregion

        #region 打印机 Printer
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        [Required]
        public static readonly Property<string> PrinterProperty = P<NewPackageViewModel>.Register(e => e.Printer,
        new RegisterRefIdArgs<string>()
        {
            PropertyChangedCallBack = (o, e) => (o as NewPackageViewModel).OnPrinterChanged()
        });

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return this.GetProperty(PrinterProperty); }
            set { this.SetProperty(PrinterProperty, value); }
        }

        /// <summary>
        /// 打印机
        /// </summary>
        protected virtual void OnPrinterChanged()
        {
            if (Settings.Default.NewPackingPrint != Printer)
            {
                Settings.Default.NewPackingPrint = Printer;
                Settings.Default.Save();
            }
        }
        #endregion 

        #region 包装规则
        /// <summary>
        /// 工单包装规则
        /// </summary>
        public static readonly ListProperty<EntityList<WorkOrderPackageRuleDetail>> PackageRuleDetailListProperty = P<NewPackageViewModel>.RegisterList(e => e.PackageRuleDetailList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as NewPackageViewModel).LoadPackageRuleDetailList()
        });

        /// <summary>
        /// 工单包装规则
        /// </summary>
        public EntityList<WorkOrderPackageRuleDetail> PackageRuleDetailList
        {
            get { return this.GetLazyList(PackageRuleDetailListProperty); }
        }

        /// <summary>
        /// 创建工单包装规则列表
        /// </summary>
        /// <returns>返回新的工单包装规则</returns>
        private EntityList<WorkOrderPackageRuleDetail> LoadPackageRuleDetailList()
        {
            return new EntityList<WorkOrderPackageRuleDetail>();
        }
        #endregion

        #region 条码明细
        /// <summary>
        /// 条码明细
        /// </summary>
        public static readonly ListProperty<EntityList<PackageSnRecord>> PackageSnRecordListProperty = P<NewPackageViewModel>.RegisterList(e => e.PackageSnRecordList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as NewPackageViewModel).LoadPackageSnRecordList()
        });

        /// <summary>
        /// 条码明细
        /// </summary>
        public EntityList<PackageSnRecord> PackageSnRecordList
        {
            get { return this.GetLazyList(PackageSnRecordListProperty); }
        }

        /// <summary>
        /// 条码明细
        /// </summary>
        /// <returns>条码明细</returns>
        private EntityList<PackageSnRecord> LoadPackageSnRecordList()
        {
            return new EntityList<PackageSnRecord>();
        }
        #endregion

        /// <summary>
        /// 扫码事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (Barcode.IsNullOrEmpty()) return;
            using (PerformenceTracer.Start("新包装采集【OnBarcodeChanged()】总用时"))
            {
                try
                {
                    ClearInfos();
                    bool isPass = true;
                    if (PrintMode == PrintMode.InAdvance)
                    {
                        isPass = IsNeedPackageNo ? AdvanceInputPackageNo(Barcode) : AdvanceInputBarcode(Barcode);
                    }
                    else
                    {
                        CurrentBarcode = Barcode;
                    }
                    if (isPass)
                    {
                        PackingCollect();
                    }
                }
                catch (Exception exc)
                {
                    using (PerformenceTracer.Start("新包装采集【ShowError()】总用时"))
                    {
                        ShowError(exc);
                    }
                }
                finally
                {
                    Barcode = null;
                }
            }
        }

        private readonly object _locker = new object();

        /// <summary>
        /// 打印包装
        /// </summary>
        /// <param name="printRelations"></param>
        /// <param name="invOrg"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void Print(EntityList<PackingRelation> printRelations, int invOrg)
        {
            if (!RT.InvOrg.HasValue)
            {
                RT.InvOrg = invOrg;
            }

            var wo = RF.GetById<WorkOrder>(printRelations.FirstOrDefault().WorkOrderId) ?? throw new ValidationException("找不到此包装[{0}]对应的工单信息".L10nFormat(printRelations.FirstOrDefault().PackageNo));
            foreach (PackingRelation pkg in printRelations.OrderBy(p => p.ItemQty))
            {
                //获取包装SN
                var rule = wo.PackageRuleDetailList.FirstOrDefault(f => f.PackageUnitId == pkg.PackageUnit.Id);
                if (rule.PrintTemplateId == null || rule.PrintTemplateId == 0)
                {
                    throw new ValidationException("找不到对应的包装规则【{0}】的【打印模板】".L10nFormat(rule.PackageUnit.Name));
                }
                var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(rule.PrintTemplateId.Value);
                var report = ReportFactory.Current.GetReportByExtension(rule.PrintTemplate.Type);
                pkg.Customer = wo.Customer?.Name;
                pkg.ItemCode = wo.Product?.Code;
                pkg.ItemName = wo.Product?.Name;
                lock (_locker)
                {
                    DoPrint(report, filePath, Printer, pkg);
                }
                RT.Service.Resolve<PackingRelationController>().UpdateRelationState(pkg.Id, LogisticState.Printed);

            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="report"></param>
        /// <param name="filePath"></param>
        /// <param name="printer"></param>
        /// <param name="relation"></param>
        private void DoPrint(IReport report, string filePath, string printer, PackingRelation relation)
        {
            var printable = new PackingRelationPrintable();
            report.Print(printable, filePath, printer, () =>
            {
                return new PackingRelation[] { relation };
            }, () =>
            {
            });
        }

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="e">托管属性变更事件参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == WorkOrderProperty && WorkOrder != null)
            {
                var oldWo = e.OldValue as WorkOrder;

                if (WorkOrder != null && WorkOrder != oldWo /*|| oldWo.ProductId != WorkOrder.ProductId*/)
                {
                    //产品不同才切换包装规则
                    PackageRuleDetailList.Clear();
                    if (WorkOrder.PackageRuleDetailList.Count > 0)
                    {
                        PackageRuleDetailList.AddRange(WorkOrder.PackageRuleDetailList);
                        PackageRuleDetailList.MarkSaved();
                    }
                }
            }
        }

        /// <summary>
        /// 资源变更事件
        /// </summary>
        /// <param name="resource">资源</param>
        protected override void ResourceChanged(WipResource resource)
        {
            base.ResourceChanged(resource);
        }

        /// <summary>
        /// 工位变更事件
        /// </summary>
        /// <param name="station"></param>
        protected override void StationChanged(Station station)
        {
            base.StationChanged(station);
            InitPrintModeConfig(station);
        }

        /// <summary>
        /// 验证是否需要提前输入
        /// </summary>
        /// <returns></returns>
        protected bool AdvanceInputPackageNo(string packageNo)
        {
            var curRuleUnit = AdvancePackingUnit.Peek() ?? throw new ValidationException("单位异常！".L10N());
            if (packageNo.IsNullOrEmpty())
            {
                throw new ValidationException("包装号不能为空".L10N());
            }
            var packingBarcode = RT.Service.Resolve<PackingBarcodeController>().GetPackingBarcode(packageNo) ?? throw new ValidationException("包装号[{0}]不存在".L10nFormat(packageNo));
            if (packingBarcode.IsUse)
            {
                throw new ValidationException("包装号[{0}]已使用".L10nFormat(packageNo));
            }
            if (packingBarcode.PackageUnitId != curRuleUnit.Id)
            {
                throw new ValidationException("包装号【{0}】包装单位是【{1}】与要扫描的包装单位不相符，请扫描【{2}】的包装号"
                    .L10nFormat(packageNo, packingBarcode.PackageUnitName, curRuleUnit.Name));
            }
            if (packingBarcode.IsUse)
            {
                throw new ValidationException("包装号[{0}]已使用".L10nFormat(packageNo));
            }
            if (packingBarcode.WorkOrderId != WorkOrder.Id)
            {
                throw new ValidationException("包装号[{0}]的工单与当前正在包装的工单不相同".L10nFormat(packageNo));
            }


            AdvanceBarcodeQueue.Enqueue(packageNo);
            AdvancePackingUnit.Dequeue();
            if (AdvancePackingUnit.Any())
            {
                IsNeedPackageNo = true;
                ShowTips("请扫描[{0}]包装条码".L10nFormat(AdvancePackingUnit.Peek().Name));
                return false;
            }
            else
            {
                IsNeedPackageNo = false;
                return true;
            }
        }

        /// <summary>
        /// 验证输入条码，预算包装层级
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        protected bool AdvanceInputBarcode(string barcode)
        {
            var workcell = GetWorkcell();
            var collectBarcode = new CollectBarcode { Code = barcode, Type = Step.CurrentStep.BarcodeType };
            Controller.Validate(collectBarcode, workcell);
            if (barcode.IsNullOrEmpty())
            {
                throw new ValidationException("条码不能为空！".L10N());
            }
            var barcodeEntity = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode) ?? throw new ValidationException("条码不存在！".L10N());
            var woId = barcodeEntity.WorkOrderId ?? throw new ValidationException("条码没有归属工单！".L10N());
            var workOrderRule = Controller.GetWorkOrderRule(woId) ?? throw new ValidationException("工单没有包装规则！".L10N());
            if (workOrderRule.Length <= 1)
            {
                throw new ValidationException("工单包装规则至少要维护2层！".L10N());
            }
            if (WorkOrder != null && woId != WorkOrder.Id)//切换工单
            {
                var newWorkOrder = RF.GetById<WorkOrder>(woId, new EagerLoadOptions().LoadWithViewProperty());
                WorkOrder = newWorkOrder;
            }


            var masterUnit = workOrderRule.FirstOrDefault();
            if (masterUnit == null || !masterUnit.PackageUnit.IsMasterUnit)
            {
                throw new ValidationException("请确保工单主单位已经维护并且是第一个！".L10N());
            }
            CurrentBarcode = barcode;
            var productId = Controller.GetWorkOrderProductId(woId);
            var records = Controller.GetPackageSnRecords(workcell.ResourceId, workcell.ProcessId, workcell.StationId, woId, productId);
            var record = Controller.GeneragePackageSnRecord(barcode, masterUnit.PackageUnitId, woId, productId, "", workcell.ResourceId, workcell.ProcessId, workcell.StationId, 1, 1);
            records.Add(record);

            // 预计算
            for (int i = 1; i < workOrderRule.Length; i++)
            {
                //上层包装规则
                var upperRule = workOrderRule[i - 1];
                //当前包装规则
                var currentRule = workOrderRule[i];
                //上层的所有数据
                var allRecords = records.Where(p => p.PackageUnitId == upperRule.PackageUnitId && p.PersistenceStatus != PersistenceStatus.Deleted).OrderBy(p => p.CreateDate).ToList();
                bool isLastRult = (i == workOrderRule.Length - 1);
                while (allRecords.Count >= currentRule.LevelQty)
                {
                    var curRecords = allRecords.Take(Convert.ToInt32(currentRule.LevelQty)).ToList();

                    //生成包装
                    if (!isLastRult)
                    {
                        var parentRecord = Controller.GeneragePackageSnRecord("", currentRule.PackageUnitId, woId, productId, "", workcell.ResourceId, workcell.ProcessId, workcell.StationId, curRecords.Count, curRecords.Sum(p => p.ItemQty));
                        records.Add(parentRecord);
                    }
                    // 生成新的包装
                    AdvancePackingUnit.Enqueue(currentRule.PackageUnit);

                    curRecords.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);

                    allRecords = records.Where(p => p.PackageUnitId == upperRule.PackageUnitId && p.PersistenceStatus != PersistenceStatus.Deleted).OrderBy(p => p.CreateDate).ToList();
                }
            }

            IsNeedPackageNo = false;

            if (AdvancePackingUnit.Any())
            {
                IsNeedPackageNo = true;
                ShowTips("请扫描[{0}]包装条码".L10nFormat(AdvancePackingUnit.Peek().Name));
                return false;
            }

            return true;
        }

        /// <summary>
        /// 包装采集
        /// </summary>
        protected void PackingCollect()
        {
            var workcell = GetWorkcell();
            if (Printer.IsNullOrEmpty())
                throw new ValidationException("打印机不能为空".L10N());
            var collectBarcode = new CollectBarcode { Code = CurrentBarcode, Type = SIE.Core.Barcodes.BarcodeType.SN };

            var barcodeWorkOrderId = RT.Service.Resolve<BarcodeController>().GetBarcodeWorkOrderId(Barcode);

            if (barcodeWorkOrderId != 0 && barcodeWorkOrderId != WorkOrderId)
            {
                var wo = RF.GetById<WorkOrder>(barcodeWorkOrderId);
                if (WorkOrder != null)
                {
                    ShowError("工单已切换,由[{0}]切换到[{1}]".L10nFormat(WorkOrder.No, wo.No));
                }

                //切换当前在制工单
                WorkOrder = wo;

                Task.Run(new Action(() =>
                {
                    //切换产线、工序、工位的在制工单
                    Controller.ChangeWipResourceWorkOrder(wo.Id, workcell);

                    //这个事件在ESOP有订阅
                    RT.EventBus.Publish(new ChangeWipResourceWorkOrderEvent { WorkOrderId = barcodeWorkOrderId });

                    //更新工单任务报工方式
                    //UpdateWorkOrdeReportModel(wo.Id);
                }).WithCurrentThreadContext());
            }

            if (PackageSnRecordList.Count > 0 && !PackageSnRecordList.Any(p => p.ProductId == WorkOrder.ProductId))
            {
                throw new ValidationException("条码明细不为空，不允许切换产品包装".L10N());
            }

            List<string> barcodes = new List<string>()
            {
                CurrentBarcode
            };
            var collectData = new CollectData
            {
                State = WipProductProcessState.Finish
            };
            collectData.Context["ADVANCE_PACKAGE_NO_LIST"] = AdvanceBarcodeQueue;
            collectData.Context["IS_ADVANCE"] = PrintMode == PrintMode.InAdvance;
            var productInfo = Controller.PkgCollect(barcodes.ToArray(), collectData, workcell);

            var sns = productInfo.Context["PACK_NO_LIST_STRING"] as string;

            //执行打印
            if (sns.IsNotEmpty())
            {
                var invOrg = RT.InvOrg.Value;
                var printRelations = RT.Service.Resolve<PackingRelationController>().GetPackingRelations(sns.Split(',').ToList());
                Task.Run(new Action(() =>
                {
                    Print(printRelations, invOrg);
                }).WithCurrentThreadContext());
            }

            //刷新包装清单
            ReloadPackingRelation();
            AddDetail(collectBarcode);
            // 包装号预输入
            AdvanceBarcodeQueue.Clear();
            AdvancePackingUnit.Clear();
            ShowTips("【{0}】采集成功".L10nFormat(collectBarcode));
        }

        /// <summary>
        /// 初始化包装号打印模式
        /// </summary>
        private void InitPrintModeConfig(Station station)
        {
            var config = ConfigService.GetConfig(new NewPackingPrintModeConfig(), typeof(Station), station);
            PrintMode = config == null ? PrintMode.Online : config.PrintMode;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            ReloadPackingRelation();
            Barcode = null;
            CurrentBarcode = null;
            IsNeedPackageNo = false;
            AdvancePackingUnit.Clear();
            AdvanceBarcodeQueue.Clear();
            base.Reset(resetType);
        }

        /// <summary>
        /// 重新加载工位工序未完成的包装关系
        /// </summary>
        public void ReloadPackingRelation()
        {
            if (Workstation.Process == null || Workstation.Station == null) return;
            var records = FindPackageSnRecords();
            PackageSnRecordList.Clear();
            PackageSnRecordList.AddRange(records);
            PackageSnRecordList.MarkSaved();
        }


        private EntityList<PackageSnRecord> FindPackageSnRecords()
        {
            return RT.Service.Resolve<NewPackageController>().GetPackageSnRecords(Workstation.ResourceId.Value, Workstation.ProcessId.Value, Workstation.StationId.Value);
        }
    }
}
