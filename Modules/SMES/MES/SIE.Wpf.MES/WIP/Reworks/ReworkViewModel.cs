using DevExpress.Data.Extensions;
using DevExpress.DataProcessing;
using DevExpress.Xpf.CodeView;
using DocumentFormat.OpenXml.Bibliography;
using SIE.Barcodes;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Receipt;
using SIE.Items;
using SIE.LES.LinesideWarehouses;
using SIE.ManagedProperty;
using SIE.MES.LoadItems;
using SIE.MES.WIP;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Reworks;
using SIE.ObjectModel;
using SIE.Packages.ItemLabels;
using SIE.Packages.ItemLabels.Configs;
using SIE.Tech.Processs;
using SIE.Warehouses;
using SIE.Warehouses.ItemStockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace SIE.Wpf.MES.WIP.Reworks
{
    /// <summary>
    /// 返工采集视图模型
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [EntityWithConfig(typeof(KeyComponentsReplacementConfig))]
    [Label("返工采集")]
    public partial class ReworkViewModel : DataCollectionViewModel<ReworkController>
    {
        /// <summary>
        /// 弹出窗编辑器选中的数据
        /// </summary>
        private EditorSelecteInfo EditorFormSelecteInfo { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReworkViewModel()
        {
            Step = new ReworkStep(this);
            ReworkOperate = ReworkOperate.Permute;
            IsSelectedAll = false;
            WipKeyItems.Clear();
            InitWorkstation(ProcessType.Rework);
        }

        /// <summary>
        /// 返工治具配步骤
        /// </summary>
        protected new virtual ReworkStep Step
        {
            get { return base.Step as ReworkStep; }
            set { base.Step = value; }
        }

        #region 返工操作类型 ReworkOperate
        /// <summary>
        /// 返工操作类型
        /// </summary>
        [Label("返工操作类型")]
        public static readonly Property<ReworkOperate> ReworkOperateProperty = P<ReworkViewModel>.Register(e => e.ReworkOperate,
            new PropertyMetadata<ReworkOperate>() { PropertyChangedCallBack = (o, e) => (o as ReworkViewModel).OnReworkOperateChange(e) });

        /// <summary>
        /// 返工操作类型值变更
        /// </summary>
        /// <param name="e">属性变更事件参数</param>
        private void OnReworkOperateChange(ManagedPropertyChangedEventArgs e)
        {
            FocuseBarcode();
            try
            {
                Reset(ResetType.Init);
                var curRwkOpt = (ReworkOperate)e.NewValue;
                Step.RewkOperate = curRwkOpt;
                SetReworkTips(curRwkOpt);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        /// <summary>
        /// 返工操作类型
        /// </summary>
        public ReworkOperate ReworkOperate
        {
            get { return this.GetProperty(ReworkOperateProperty); }
            set { this.SetProperty(ReworkOperateProperty, value); }
        }
        #endregion

        #region 是否全部选中 IsSelectedAll
        /// <summary>
        /// 是否全部选中
        /// </summary>
        [Label("是否全部选中")]
        public static readonly Property<bool> IsSelectedAllProperty = P<ReworkViewModel>.Register(e => e.IsSelectedAll);

        /// <summary>
        /// 是否全部选中
        /// </summary>
        public bool IsSelectedAll
        {
            get { return this.GetProperty(IsSelectedAllProperty); }
            set { this.SetProperty(IsSelectedAllProperty, value); }
        }
        #endregion

        #region 关键件 KeyItems
        /// <summary>
        /// 关键件
        /// </summary>
        [Label("关键件")]
        public static readonly ListProperty<EntityList<WipProductProcessKeyItem>> WipKeyItemsProperty =
            P<ReworkViewModel>.RegisterList(e => e.WipKeyItems, new ListPropertyMeta
            {
                HasManyType = HasManyType.Aggregation,
                DataProvider = e => new EntityList<WipProductProcessKeyItem>()
            });

        /// <summary>
        /// 关键件
        /// </summary>
        public EntityList<WipProductProcessKeyItem> WipKeyItems
        {
            get { return this.GetLazyList(WipKeyItemsProperty); }
        }
        #endregion

        /// <summary>
        /// 重置采集信息
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            base.Reset(resetType);
            ReworkReset();
            SetReworkTips(this.ReworkOperate);
        }

        /// <summary>
        /// 返工采集的Reset方法
        /// </summary>
        private void ReworkReset()
        {
            IsSelectedAll = false;
            WipKeyItems.Clear();
        }

        /// <summary>
        /// 设置返工采集的Tips
        /// </summary>
        /// <param name="curRwkOpt">返工操作类型</param>
        private void SetReworkTips(ReworkOperate curRwkOpt)
        {
            if (curRwkOpt == ReworkOperate.Permute)
            {
                ShowTips("请扫描返工工单条码".L10N());
            }
            else if (curRwkOpt == ReworkOperate.PermuteUnbound)
            {
                ShowTips("请扫描返工工单条码".L10N());
            }
            else if (curRwkOpt == ReworkOperate.Unbound)
            {
                ShowTips("请扫描关键件条码".L10N());
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// 添加采集结果记录
        /// </summary>
        /// <param name="collectBarcode">采集条码</param>
        /// <param name="barcodes">条码集合</param>
        /// <param name="result">结果类型</param>
        /// <param name="rewkOprt">返工操作类型</param>
        protected void AddDetail(CollectBarcode collectBarcode, string[] barcodes, ResultType result, ReworkOperate rewkOprt)
        {
            ////base.AddDetail(collectBarcode);
            var collectDetailVml = new CollectDetailViewModel
            {
                Barcode = barcodes.FirstOrDefault(), //生产条码
                BarcodeType = collectBarcode.Type
            };
            if (rewkOprt != ReworkOperate.Unbound)
                collectDetailVml.BatchNo = collectBarcode.Code; //原条码
            collectDetailVml.Result = result;
            collectDetailVml.CollectDate = RF.Find<WipProductProcess>().GetDbTime();
            CollectDetailList.Add(collectDetailVml);
        }

        /// <summary>
        ///  条码扫完后处理逻辑
        /// </summary>
        /// <param name="e">属性变更参数</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (!Barcode.IsNotEmpty())
            {
                return;
            }
            try
            {
                ClearInfos();
                var workCell = GetWorkcell();
                if (ReworkOperate == ReworkOperate.Permute)
                {
                    PermuteAssemblyCollect(Barcode, workCell);
                }
                else if (ReworkOperate == ReworkOperate.PermuteUnbound)
                {
                    PermuteUnboundAssemblyCollect(Barcode, workCell);
                }
                else if (ReworkOperate == ReworkOperate.Unbound)
                {
                    KeyItemUnbound(Barcode, workCell);
                }
                else
                {
                    //
                }
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
            finally
            {
                Barcode = null;
            }
        }

        #region 条码置换 Begin
        /// <summary>
        /// 条码置换采集
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void PermuteAssemblyCollect(string barcode, Workcell workcell)
        {
            var collectBarcode = CreateCollectBarcode(barcode);

            if (Step.StepIndex == 0)
            {
                PermuteCheckReworkBarcode(barcode); //验证返工工单条码
                ValidatePermute(collectBarcode, workcell);
                Step.AddReworkBarcodes(collectBarcode.Code);
            }

            if (Step.StepIndex != 0)
            {
                PermuteCheckOriginalBarcode(barcode); //验证原工单条码
                SetModelKeyItems(barcode); //设置Model的关键件集合
                Controller.ValidateBarcode(collectBarcode, workcell);
            }

            Step.AddBarcodes(collectBarcode.Code);
            Step.ReworkCollectBarcodes.Add(collectBarcode);

            if (!Step.NextStep())
            {
                var barcodes = Step.ReworkBarcodes.ToArray();
                try
                {
                    var strNewLabels = "";
                    if (ReworkOperate == ReworkOperate.PermuteUnbound)
                    {
                        var res = ShowComfirmDialog();
                        if (res == 1)//取消按钮 不再执行下面代码
                        {
                            return;
                        }
                        if (EditorFormSelecteInfo != null && EditorFormSelecteInfo.SelectedBlankingWay == ReworkSelectedWay.Bad)
                        {
                            var wipKeyUnboundItems = WipKeyItems.Where(x => x.IsUnbound).Distinct().ToList();
                            if (wipKeyUnboundItems.Count > 0)
                            {
                                string newLabel = "";
                                foreach (var wipKeyUnboundItem in wipKeyUnboundItems)
                                {
                                    newLabel = UpdateWipkeyItemInfo(EditorFormSelecteInfo, wipKeyUnboundItem);
                                    if (!string.IsNullOrEmpty(newLabel))
                                    {
                                        strNewLabels += newLabel + ",";
                                    }
                                }
                            }
                        }
                        else if (EditorFormSelecteInfo != null && EditorFormSelecteInfo.SelectedBlankingWay == ReworkSelectedWay.Good)
                        {
                            var wipKeyUnboundItems = WipKeyItems.Where(x => x.IsUnbound).Distinct().ToList();
                            if (wipKeyUnboundItems.Count > 0)
                            {
                                string newLabel = "";
                                foreach (var wipKeyUnboundItem in wipKeyUnboundItems)
                                {
                                    newLabel = UpdateGoodWipkeyItemInfo(EditorFormSelecteInfo, wipKeyUnboundItem);
                                    if (!string.IsNullOrEmpty(newLabel))
                                    {
                                        strNewLabels += newLabel + ",";
                                    }
                                }
                            }
                        }
                    }
                    var collectData = SetCollectDataReworkData();
                    Controller.Collect(barcodes, collectData, workcell);
                    AddDetail(collectBarcode, barcodes, ResultType.Pass, ReworkOperate.Permute);
                    var curStepIndex = Step.StepIndex - 1 < 0 ? 0 : Step.StepIndex - 1;
                    var noNewLabel = "{0}[{1}]采集成功".L10nFormat(Step.StepBarcodeTypes[curStepIndex], collectData.CollectBarcode.Code);
                    var msg = strNewLabels.IsNullOrEmpty() ? noNewLabel : noNewLabel + ",关键件不良下料生成新物料标签：".L10N() + strNewLabels.TrimEnd(',');
                    ShowTips(msg);
                    Step.Reset();
                    EditorFormSelecteInfo = null;
                }
                catch (Exception)
                {
                    Step.Roolback();
                    EditorFormSelecteInfo = null;
                    throw;
                }
            }
            else
            {
                var curStepIndex = Step.StepIndex - 1 < 0 ? 0 : Step.StepIndex - 1;
                ShowTips("{0}[{1}]扫描采集成功，请扫描{2}".L10nFormat(Step.StepBarcodeTypes[curStepIndex], collectBarcode.Code, Step.StepBarcodeTypes[Step.StepIndex]));
            }
        }

        /// <summary>
        /// 置换采集时验证返工工单条码
        /// </summary>
        /// <param name="barcode">返工工单条码</param>
        private void PermuteCheckReworkBarcode(string barcode)
        {
            var result = CheckReworkBarcodeInReworkOrder(barcode); //Check是否为返工工单生产条码
            if (!result)
                throw new ValidationException("条码[{0}]非返工工单条码".L10nFormat(barcode));
            var flag = CheckReworkBarcodeHavePermuted(barcode); //Check返工条码是否已经置换
            if (flag)
                throw new ValidationException("条码[{0}]已置换, 无法操作".L10nFormat(barcode));
        }

        /// <summary>
        /// 验证:1.工艺路线。2.在制工单
        /// </summary>
        /// <param name="collectBarcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        private void ValidatePermute(CollectBarcode collectBarcode, Workcell workcell)
        {
            base.Validate(collectBarcode, workcell);
            var checkRefOriginal = CheckReworkBarcodeRefOriginal(collectBarcode.Code);
            if (checkRefOriginal)
            {
                throw new ValidationException("条码[{0}]不需要置换".L10nFormat(collectBarcode.Code));
            }
            else
            {
                ReworkCheckProcessStep(2);
            }
        }

        /// <summary>
        /// 判断返工工序的工序步骤
        /// </summary>
        /// <param name="stepCount">工序采集步骤</param>
        private void ReworkCheckProcessStep(int stepCount)
        {
            if (Step.ProcessSteps.Count() != stepCount)
                throw new ValidationException("返工采集工序的采集步骤必须是 {0} 步!".L10nFormat(stepCount));
        }

        /// <summary>
        /// Check是否为返工工单生产条码
        /// </summary>
        /// <param name="barcode">生产条码</param>
        /// <returns>true:条码存在 ; false:条码不存在</returns>
        private bool CheckReworkBarcodeInReworkOrder(string barcode)
        {
            var result = RT.Service.Resolve<BarcodeController>().Exists(barcode, WorkOrderType.Rework);
            return result;
        }

        /// <summary>
        /// Check返工工单条码是否使用原工单条码
        /// Check是否需要置换
        /// </summary>
        /// <param name="barcode">生产条码</param>
        /// <returns>true:返工条码使用原工单条码; false:使用新条码</returns>
        private bool CheckReworkBarcodeRefOriginal(string barcode)
        {
            var curCheckFlag = RT.Service.Resolve<ReworkController>().ExistUnionBarcode(this.WorkOrderId.Value, WorkOrderType.Rework, barcode, barcode);
            return curCheckFlag;
        }

        /// <summary>
        /// Check返工条码是否已经置换
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>true: 已置换；false:未置换</returns>
        private bool CheckReworkBarcodeHavePermuted(string barcode)
        {
            var result = RT.Service.Resolve<WipProductVersionController>().CheckReworkBarcodeHavePermuted(barcode);
            return result;
        }

        /// <summary>
        /// 验证原工单生产条码
        /// </summary>
        /// <param name="barcode">条码</param>
        private void PermuteCheckOriginalBarcode(string barcode)
        {
            CheckOriginalBarcodeScraped(barcode);
            CheckOriginalBarcodeUnionBarcode(barcode);
            CheckOriginalBarcodeHavePermuted(barcode); //条码是否已被置换
        }

        /// <summary>
        /// Check原工单生产条码是否已报废
        /// </summary>
        /// <param name="barcode">条码</param>
        private void CheckOriginalBarcodeScraped(string barcode)
        {
            const bool isScraped = true;
            var result = RT.Service.Resolve<BarcodeController>().Exists(barcode, isScraped);
            if (result)
                throw new ValidationException("条码[{0}]已报废".L10nFormat(barcode));
        }

        /// <summary>
        /// Check原工单生产条码是否进行返工配置
        /// </summary>
        /// <param name="originalBarcode">原工单条码</param>
        private void CheckOriginalBarcodeUnionBarcode(string originalBarcode)
        {
            var curUnionBarcode = RT.Service.Resolve<ReworkController>().ExistUnionBarcode(this.WorkOrderId.Value, WorkOrderType.Rework, originalBarcode, null);
            if (!curUnionBarcode)
                throw new ValidationException("原工单条码[{0}]未进行返工配置".L10nFormat(originalBarcode));
        }

        /// <summary>
        /// 判断原条码是否已经置换
        /// </summary>
        /// <param name="originalBarcode">原工单条码</param>
        private void CheckOriginalBarcodeHavePermuted(string originalBarcode)
        {
            var checkFlag = RT.Service.Resolve<ReworkController>().CheckOriginalBarcodeHavePermuted(this.WorkOrderId.Value, WorkOrderType.Rework, originalBarcode);
            if (checkFlag)
                throw new ValidationException("原工单条码[{0}]已被置换".L10nFormat(originalBarcode));
        }

        /// <summary>
        /// 设置Model的关键件集合
        /// </summary>
        /// <param name="barcode">条码</param>
        private void SetModelKeyItems(string barcode)
        {
            WipKeyItems.Clear();
            ////var curKeyItems = RT.Service.Resolve<WipProductVersionController>().GetWipKeyItems(barcode);
            var curKeyItems = RT.Service.Resolve<WipProductVersionController>().GetRecursionWipKeyItems(barcode);
            if (curKeyItems != null && curKeyItems.Any())
            {
                var notUnboundKeyItems = curKeyItems.Where(x => !x.IsUnbound).ToList();
                if (notUnboundKeyItems != null && notUnboundKeyItems.Count > 0)
                {
                    WipKeyItems.AddRange(notUnboundKeyItems);
                }
            }
        }
        #endregion 条码置换 End        

        #region 条码置换解绑关健件 Begin
        /// <summary>
        /// 条码置换解绑关健件采集
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void PermuteUnboundAssemblyCollect(string barcode, Workcell workcell)
        {
            if (!PermuteUnboundSubmitCheck())
                return;
            var collectBarcode = CreateCollectBarcode(barcode);

            bool checkRefOriginal = false;
            if (Step.StepIndex == 0)
            {
                PermuteUnboundCheckReworkBarcode(barcode); //验证返工工单条码
                checkRefOriginal = ValidatePermuteUnbound(collectBarcode, workcell);
                Step.AddReworkBarcodes(collectBarcode.Code);
            }

            if (Step.StepIndex != 0 || checkRefOriginal)
            {
                if (!checkRefOriginal)
                    PermuteUnboundCheckOriginalBarcode(barcode); //验证原工单条码
                PermuteUnboundSetModelkeyItems(barcode);
                Controller.ValidateBarcode(collectBarcode, workcell);
            }

            Step.AddBarcodes(collectBarcode.Code);
            Step.ReworkCollectBarcodes.Add(collectBarcode);

            if (!Step.NextStep())
            {
                if (checkRefOriginal)
                {
                    var curStepIndex = Step.StepIndex - 1 < 0 ? 0 : Step.StepIndex - 1;
                    ShowTips("{0}[{1}]扫描成功,请提交".L10nFormat(Step.StepBarcodeTypes[curStepIndex], collectBarcode.Code));
                }
                else
                {
                    ShowTips("{0}[{1}]扫描成功,请提交".L10nFormat(Step.StepBarcodeTypes[Step.StepIndex], collectBarcode.Code));
                }
            }
            else
            {
                var curStepIndex = Step.StepIndex - 1 < 0 ? 0 : Step.StepIndex - 1;
                ShowTips("{0}[{1}]扫描采集成功，请扫描{2}".L10nFormat(Step.StepBarcodeTypes[curStepIndex], collectBarcode.Code, Step.StepBarcodeTypes[Step.StepIndex]));
            }
        }

        /// <summary>
        /// 判断是否未完成提交，强制扫描下一个条码
        /// </summary>
        /// <returns>true: 验证通过；false: 未通过</returns>
        private bool PermuteUnboundSubmitCheck()
        {
            bool checkFlag = true;
            if (!Step.HasNextStep())
            {
                checkFlag = false;
                if (Barcode == SIE.Barcodes.Barcode.SubmitCode)
                {
                    Submit();
                }
                else
                {
                    ShowError("上一条码未提交,扫[{0}]提交或者重新开始".L10nFormat(SIE.Barcodes.Barcode.SubmitCode));
                }
            }

            return checkFlag;
        }

        /// <summary>
        /// 创建CollectBarcode
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>采集条码对象</returns>
        private CollectBarcode CreateCollectBarcode(string barcode)
        {
            var collectBarcode = new CollectBarcode(barcode, Step.CurrentStep.BarcodeType);
            return collectBarcode;
        }

        /// <summary>
        /// 置换解绑Check返工工单条码
        /// </summary>
        /// <param name="barcode">条码</param>
        private void PermuteUnboundCheckReworkBarcode(string barcode)
        {
            var result = CheckReworkBarcodeInReworkOrder(barcode); //Check是否为返工工单生产条码
            if (!result)
            {
                throw new ValidationException("条码[{0}]非返工工单条码".L10nFormat(barcode));
            }
            var flag = CheckReworkBarcodeHavePermuted(barcode); //Check返工条码是否已经置换
            if (flag)
            {
                throw new ValidationException("条码[{0}]已置换, 无法操作".L10nFormat(barcode));
            }
        }

        /// <summary>
        /// 验证:1.工艺路线。2.在制工单
        /// </summary>
        /// <param name="collectBarcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        private bool ValidatePermuteUnbound(CollectBarcode collectBarcode, Workcell workcell)
        {
            bool checkRefOriginal = false;
            base.Validate(collectBarcode, workcell);
            checkRefOriginal = CheckReworkBarcodeRefOriginal(collectBarcode.Code); //是否使用原工单条码
            if (checkRefOriginal)
            {
                ReworkCheckProcessStep(1);
            }
            else
            {
                ReworkCheckProcessStep(2);
            }
            return checkRefOriginal;
        }

        /// <summary>
        /// 置换解绑Check原工单条码
        /// </summary>
        /// <param name="barcode">条码</param>
        private void PermuteUnboundCheckOriginalBarcode(string barcode)
        {
            CheckOriginalBarcodeScraped(barcode);
            CheckOriginalBarcodeUnionBarcode(barcode);
            CheckOriginalBarcodeHavePermuted(barcode); //条码是否已被置换
        }

        /// <summary>
        /// 设置Model的关健件集合
        /// </summary>
        /// <param name="barcode">生产条码</param>
        private void PermuteUnboundSetModelkeyItems(string barcode)
        {
            SetModelKeyItems(barcode); //设置Model的关键件集合
            var keyItemUnbdCfgs = RT.Service.Resolve<ReworkController>().GetKeyItemUnboundConfigs(barcode);
            if (keyItemUnbdCfgs != null && keyItemUnbdCfgs.Count > 0)
            {
                foreach (var keyItem in WipKeyItems)
                {
                    var curKeyItemCfg = keyItemUnbdCfgs.FirstOrDefault(x => x.ItemId == keyItem.ItemId);
                    if (curKeyItemCfg != null)
                        keyItem.IsUnbound = curKeyItemCfg.IsUnbound;
                }

                SetIsSelectedAll();
            }
        }

        /// <summary>
        /// 设置属性值IsSelectedAll
        /// </summary>
        private void SetIsSelectedAll()
        {
            if (this.WipKeyItems.All(x => x.IsUnbound))
            {
                this.IsSelectedAll = true;
            }
            else
            {
                this.IsSelectedAll = false;
            }
        }

        /// <summary>
        /// 返回能否提交
        /// </summary>
        /// <returns>true:可以执行; false:不可以执行</returns>
        public virtual bool CanSubmit()
        {
            return !Step.HasNextStep()
                && Workstation.EmployeeId.HasValue
                && Workstation.ProcessId.HasValue
                && Workstation.StationId.HasValue
                && Workstation.ResourceId.HasValue;
        }

        /// <summary>
        /// 执行提交逻辑
        /// </summary>
        public virtual void Submit()
        {
            var collectBarcode = CreateCollectBarcode(Step.Barcodes.LastOrDefault());
            var workCell = GetWorkcell();
            var barcodes = Step.ReworkBarcodes.ToArray();
            try
            {
                var strNewLabels = "";
                if (ReworkOperate == ReworkOperate.PermuteUnbound && this.WipKeyItems.Any(m => m.IsUnbound))//没勾选的时候不弹出下料处理窗口
                {
                    var res = ShowComfirmDialog();
                    if (res == 1)//取消按钮 不再执行下面代码
                    {
                        return;
                    }
                    if (EditorFormSelecteInfo != null && EditorFormSelecteInfo.SelectedBlankingWay == ReworkSelectedWay.Bad)
                    {
                        var wipKeyUnboundItems = WipKeyItems.Where(x => x.IsUnbound).Distinct().ToList();
                        if (wipKeyUnboundItems.Count > 0)
                        {
                            string newLabel = "";
                            foreach (var wipKeyUnboundItem in wipKeyUnboundItems)
                            {
                                newLabel = UpdateWipkeyItemInfo(EditorFormSelecteInfo, wipKeyUnboundItem);
                            }
                            if (!string.IsNullOrEmpty(newLabel))
                            {
                                strNewLabels += newLabel + ",";
                            }
                        }
                    }
                    else if (EditorFormSelecteInfo != null && EditorFormSelecteInfo.SelectedBlankingWay == ReworkSelectedWay.Good)
                    {
                        var wipKeyUnboundItems = WipKeyItems.Where(x => x.IsUnbound).Distinct().ToList();
                        if (wipKeyUnboundItems.Count > 0)
                        {
                            string newLabel = "";
                            foreach (var wipKeyUnboundItem in wipKeyUnboundItems)
                            {
                                newLabel = UpdateGoodWipkeyItemInfo(EditorFormSelecteInfo, wipKeyUnboundItem);
                                if (!string.IsNullOrEmpty(newLabel))
                                {
                                    strNewLabels += newLabel + ",";
                                }
                            }
                        }
                    }
                }

                var collectData = SetCollectDataReworkData();
                Controller.Collect(barcodes, collectData, workCell);
                AddDetail(collectBarcode, barcodes, ResultType.Pass, ReworkOperate.PermuteUnbound);
                ClearInfos();
                var curStepIndex = Step.StepIndex - 1 < 0 ? 0 : Step.StepIndex - 1;

                var noNewLabel = "{0}[{1}]采集成功".L10nFormat(Step.StepBarcodeTypes[curStepIndex], collectData.CollectBarcode.Code);
                var msg = strNewLabels.IsNullOrEmpty() ? noNewLabel : noNewLabel + ",关键件不良下料生成新物料标签：".L10N() + strNewLabels.TrimEnd(',');
                ShowTips(msg);
                Step.Reset();
                ReworkReset();
                EditorFormSelecteInfo = null;
            }
            catch (Exception exc)
            {
                EditorFormSelecteInfo = null;
                ShowError(exc);
            }
        }


        /// <summary>
        /// 弹窗确认操作方式
        /// </summary>
        /// <exception cref="PlatformException"></exception>
        private int ShowComfirmDialog(WipProductProcessKeyItem curKeyItem = null)
        {
            if (Activator.CreateInstance(typeof(ReworkKeyItemComfrimControl)) is not ReworkKeyItemComfrimControl editor)
            {
                throw new PlatformException("{0}没有继承自ReworkKeyItemComfrimControl".L10nFormat(typeof(ReworkKeyItemComfrimControl).GetQualifiedName()));
            }
            var wipKeyItem = curKeyItem ?? WipKeyItems.FirstOrDefault();
            if (wipKeyItem == null)
            {
                return 0;
            }

            if (curKeyItem == null)
            {
                var wipKeyUnboundItems = WipKeyItems.Where(x => x.IsUnbound).Distinct().ToList();
                if (wipKeyUnboundItems != null && wipKeyUnboundItems.Count <= 0)
                {
                    return 0;
                }
            }

            var whs = RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehouses();
            if (whs.Any())
            {
                editor.WarehouseList.AddRange(whs);
            }
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), editor,
       w =>
       {
           w.Title = "请确定".L10N();
           w.Height = 350;
           w.Width = 500;
           w.MinHeight = 250;
           w.MinWidth = 400;
           w.Commands.Clear();
           w.Closing += (object sender, System.ComponentModel.CancelEventArgs e) =>
           {
               if (editor.Result == 0 && editor.SelectedBlankingWay)//点击确定和点选了下料处理
               {
                   if (editor.SelectedWarehouse == null)
                   {
                       CRT.MessageService.ShowMessage("请选择线边仓".L10N());
                       e.Cancel = true;
                       return;
                   }
                   EditorFormSelecteInfo = new EditorSelecteInfo();
                   EditorFormSelecteInfo.SelectedBlankingWay = ReworkSelectedWay.Bad;
                   EditorFormSelecteInfo.WarehouseId = editor.SelectedWarehouse.WarehouseId;
                   EditorFormSelecteInfo.StorageLocationId = editor.SelectedWarehouse.StorageLocationId;

               }
               if (editor.Result == 0 && editor.SelectedGoodBlankingWay)//点击确定和点选了下料处理
               {
                   if (editor.SelectedWarehouse == null)
                   {
                       CRT.MessageService.ShowMessage("请选择线边仓".L10N());
                       e.Cancel = true;
                       return;
                   }
                   EditorFormSelecteInfo = new EditorSelecteInfo();
                   EditorFormSelecteInfo.SelectedBlankingWay = ReworkSelectedWay.Good;
                   EditorFormSelecteInfo.WarehouseId = editor.SelectedWarehouse.WarehouseId;
                   EditorFormSelecteInfo.StorageLocationId = editor.SelectedWarehouse.StorageLocationId;
               }
           };
       });
            return editor.Result;
        }

        /// <summary>
        /// 更新物料标签 并调用接口
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="wipKeyUnboundItem"></param>

        private string UpdateWipkeyItemInfo(EditorSelecteInfo editor, WipProductProcessKeyItem wipKeyUnboundItem)
        {
            return Controller.RepairReWorkUnloadItem(wipKeyUnboundItem.SourceId, wipKeyUnboundItem.Qty, editor.WarehouseId, editor.StorageLocationId, true, wipKeyUnboundItem.Process.Version.WorkOrderId);
        }

        /// <summary>
        /// 更新物料标签 并调用接口
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="wipKeyUnboundItem"></param>

        private string UpdateGoodWipkeyItemInfo(EditorSelecteInfo editor, WipProductProcessKeyItem wipKeyUnboundItem)
        {
            return Controller.RepairReWorkUnloadItem(wipKeyUnboundItem.SourceId, wipKeyUnboundItem.Qty, editor.WarehouseId, editor.StorageLocationId, false, wipKeyUnboundItem.Process.Version.WorkOrderId);
        }

        /// <summary>
        /// 设置CollectData的返工数据的KeyItems
        /// </summary>
        /// <param name="collectData">采集数据</param>
        private void SetCollectDataReworkDataKeyItems(CollectData collectData)
        {
            var wipKeyItemIds = WipKeyItems.Where(x => x.IsUnbound).Select(x => x.Id).Distinct().ToList();
            if (wipKeyItemIds.Count > 0)
            {
                collectData.ReworkData.KeyItems.AddRange(wipKeyItemIds);
            }
        }

        /// <summary>
        /// 设置CollectData的返工数据的条码
        /// </summary>
        /// <param name="collectData">采集数据</param>
        /// <param name="barcodes">采集的条码</param>
        private void SetCollectDataReworkDataBarcodes(CollectData collectData, List<string> barcodes)
        {
            collectData.ReworkData.ReworkBarcode = barcodes.FirstOrDefault();
            collectData.ReworkData.OriginalBarcode = barcodes.LastOrDefault();
        }

        /// <summary>
        /// 设置CollectData的属性值
        /// </summary>
        /// <returns>采集数据</returns>
        private CollectData SetCollectDataReworkData()
        {
            CollectData collectData = new CollectData()
            {
                CollectBarcode = Step.ReworkCollectBarcodes.FirstOrDefault()
            };
            if (ReworkOperate == ReworkOperate.PermuteUnbound)
            {
                SetCollectDataReworkDataKeyItems(collectData);
            }
            SetCollectDataReworkDataBarcodes(collectData, Step.Barcodes);
            return collectData;
        }

        #endregion 条码置换解绑关健件 End

        #region 关健件解绑 Begin
        /// <summary>
        /// 关健件解绑
        /// </summary>
        /// <param name="sourceCode">条码</param>
        /// <param name="workcell">工单单元</param>
        private void KeyItemUnbound(string sourceCode, Workcell workcell)
        {
            WipKeyItems.Clear();
            var curKeyItem = RT.Service.Resolve<WipProductVersionController>().GetWipKeyItem(sourceCode);
            if (curKeyItem == null)
            {
                throw new ValidationException("关健件条码 [{0}] 不存在".L10nFormat(sourceCode));
            }
            else if (curKeyItem.IsUnbound)
            {
                throw new ValidationException("关健件条码 [{0}] 已解绑".L10nFormat(sourceCode));
            }
            else
            {
                var res = ShowComfirmDialog(curKeyItem);
                if (res == 1)//取消按钮 不再执行下面代码
                {
                    return;
                }
                string newItemLabel = "";
                if (EditorFormSelecteInfo != null && EditorFormSelecteInfo.SelectedBlankingWay == ReworkSelectedWay.Bad)
                {
                    newItemLabel = UpdateWipkeyItemInfo(EditorFormSelecteInfo, curKeyItem);
                }
                else if (EditorFormSelecteInfo != null && EditorFormSelecteInfo.SelectedBlankingWay == ReworkSelectedWay.Good)
                {
                    newItemLabel = UpdateGoodWipkeyItemInfo(EditorFormSelecteInfo, curKeyItem);
                }
                var wipProductProcess = curKeyItem.Process;
                var wipPrcVersion = wipProductProcess.Version;
                this.WorkOrder = wipPrcVersion.WorkOrder;
                var collectBarcode = CreateCollectBarcode(wipPrcVersion.Sn);
                string[] barcodes = new string[] { collectBarcode.Code };

                RT.Service.Resolve<ReworkController>().UnboundKeyItem(curKeyItem.Id);
                AddDetail(collectBarcode, barcodes, ResultType.Pass, ReworkOperate.Unbound);
                ////var curWipKeyItems = wipProductProcess.KeyItemList;
                var curWipKeyItems = wipPrcVersion.ProcessList.SelectMany(x => x.KeyItemList).ToList();
                WipKeyItems.AddRange(curWipKeyItems);

                var msg = newItemLabel.IsNullOrEmpty() ? "关健件[{0}]解绑成功".L10nFormat(sourceCode) : "关健件[{0}]解绑成功,关键件不良下料生成新条码【{1}】".L10nFormat(sourceCode, newItemLabel);
                ShowTips(msg);
                EditorFormSelecteInfo = null;//清空保证下次是最新数据
            }
        }
        #endregion 关健件解绑 End

        /// <summary>
        /// 弹出窗编辑器选中信息
        /// </summary>
        internal class EditorSelecteInfo
        {
            /// <summary>
            /// 下料处理方式
            /// </summary>
            internal ReworkSelectedWay SelectedBlankingWay { get; set; }

            /// <summary>
            /// 库位Id
            /// </summary>
            internal double StorageLocationId { get; set; }

            /// <summary>
            /// 所选仓库Id
            /// </summary>
            internal double WarehouseId { get; set; }
        }

        /// <summary>
        /// 选择类型
        /// </summary>
        public enum ReworkSelectedWay
        {
            /// <summary>
            /// 置换后作废
            /// </summary>
            [Label("置换后作废")]
            Cancel,

            /// <summary>
            /// 置换后不良下料
            /// </summary>
            [Label("置换后不良下料")]
            Bad,

            /// <summary>
            /// 置换后良品下料
            /// </summary>
            [Label("置换后良品下料")]
            Good,
        }


        /// <summary>
        /// 返工采集步骤控制
        /// </summary>
        protected class ReworkStep : CollectStep
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="vm">返工采集视图</param>
            public ReworkStep(ReworkViewModel vm) : base(vm)
            {
                RewkOperate = ReworkOperate.Permute;
                ////PermuteUnboundSubScenes = 0;
                ReworkCollectBarcodes = new List<CollectBarcode>();
                ReworkBarcodes = new List<string>();
                StepBatcodeTypesIni();
            }

            /// <summary>
            /// 返工采集类型
            /// </summary>
            public ReworkOperate RewkOperate { get; set; }

            /////* /// <summary>
            /////// 置换解绑子场景
            /////// 1: 使用新条码; 2:使用旧条码
            /////// </summary>
            ////public int PermuteUnboundSubScenes { get; set; }*/

            /// <summary>
            /// 返工采集条码集合
            /// </summary>
            public List<CollectBarcode> ReworkCollectBarcodes { get; }

            /// <summary>
            /// 返工条码集合
            /// </summary>
            public List<string> ReworkBarcodes { get; }

            /// <summary>
            /// 采集步骤条码类型
            /// </summary>
            public List<string> StepBarcodeTypes { get; } = new List<string>();

            /// <summary>
            /// 初始化采集步骤条码类型
            /// </summary>
            private void StepBatcodeTypesIni()
            {
                StepBarcodeTypes.Clear();
                StepBarcodeTypes.Add("返工工单条码".L10N());
                StepBarcodeTypes.Add("原工单条码".L10N());
            }

            /// <summary>
            /// 重置
            /// </summary>
            public override void Reset()
            {
                base.Reset();
                ReworkCollectBarcodes.Clear();
                ReworkBarcodes.Clear();
                ////PermuteUnboundSubScenes = 0;
            }

            /// <summary>
            /// 回滚一步
            /// </summary>
            public override void Roolback()
            {
                ReworkCollectBarcodes.RemoveAt(_stepIndex);
                if (_stepIndex < ReworkBarcodes.Count)
                {
                    ReworkBarcodes.RemoveAt(_stepIndex);
                }
                base.Roolback();
            }

            /// <summary>
            /// 设置StepIndex++
            /// </summary>
            public void AddStepIndex()
            {
                if (_stepIndex < ProcessSteps.Count() - 1)
                {
                    _stepIndex++;
                }
            }

            /// <summary>
            /// 设置StepIndex--
            /// </summary>
            public void SubStepIndex()
            {
                if (_stepIndex >= 1)
                {
                    _stepIndex--;
                }
            }

            /// <summary>
            /// 是否有下一步 
            /// </summary>
            /// <returns>有下一步返回true ; 没有下一步返回false</returns>
            public override bool NextStep()
            {
                bool checkFlag = base.NextStep();
                return checkFlag;
            }

            /// <summary>
            /// 是否有下一步
            /// </summary>
            /// <returns>返回是否有下一步</returns>
            public override bool HasNextStep()
            {
                /////*if (RewkOperate == ReworkOperate.PermuteUnbound && PermuteUnboundSubScenes == 2)
                ////{
                ////    ////checkFlag = _stepIndex < ProcessSteps.Count() - 1;
                ////    checkFlag = Barcodes.Count() < ProcessSteps?.Count() - 1;
                ////}
                ////else
                ////    checkFlag = base.HasNextStep();*/
                bool checkFlag = base.HasNextStep();
                return checkFlag;
            }

            /// <summary>
            /// 返工条码Add方法
            /// </summary>
            /// <param name="rewkBarcode">返工条码</param>
            public void AddReworkBarcodes(string rewkBarcode)
            {
                if (!this.ReworkBarcodes.Contains(rewkBarcode))
                {
                    this.ReworkBarcodes.Add(rewkBarcode);
                }
            }

            /// <summary>
            /// 按步骤采集的条码Add方法
            /// </summary>
            /// <param name="barcode">条码</param>
            public void AddBarcodes(string barcode)
            {
                if (!this.Barcodes.Contains(barcode))
                {
                    this.Barcodes.Add(barcode);
                }
            }

            /// <summary>
            /// 返工采集条码集合
            /// </summary>
            /// <param name="collectBarcode">采集条码</param>
            public void AddReworkCollectBarcodes(CollectBarcode collectBarcode)
            {
                var containCount = this.ReworkCollectBarcodes.Count(x => x.Code == collectBarcode.Code);
                if (containCount < 1)
                {
                    this.ReworkCollectBarcodes.Add(collectBarcode);
                }
            }
        }
    }
}