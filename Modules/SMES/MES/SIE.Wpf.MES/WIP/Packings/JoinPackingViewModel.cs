using SIE.Common.Configs;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Packings.Configs;
using SIE.ObjectModel;

namespace SIE.Wpf.MES.WIP.Packings
{
    [EntityWithConfig(typeof(WeightConfig))]
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [EntityWithConfig(typeof(WeightSerialProtsConfig))]
    [EntityWithConfig(typeof(WipPackingConfig))]
    [EntityWithConfig(typeof(WipPackingBillConfig))]
    [RootEntity]
    [Label("包装采集")]
    public class JoinPackingViewModel : NewPackingViewModel
    {
        ///// <summary>
        ///// 条码变更事件
        ///// </summary>
        ///// <param name="e">托管属性变更事件参数</param>
        //protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        //{
        //    if (Barcode.IsNullOrEmpty()) 
        //        return;

        //    using (PerformenceWatcher.Start(logger, "OnBarcodeChanged"))
        //    {
        //        try
        //        {
        //            ClearInfos();
        //            bool isPass = false;
        //            if (IsPackage)
        //                // 层级包装
        //                isPass = ValidatePackageNo(Barcode);
        //            else
        //                // 产品包装
        //                isPass = ValidateBarcode(Barcode);

        //            if (isPass)
        //                PackingCollect();
        //        }
        //        catch (Exception exc)
        //        {
        //            var baseExc = exc.GetBaseException();
        //            if (baseExc is EmptyPackingNoException)
        //            {
        //                var backageNos = baseExc.Data as Queue<string>;
        //                IsPackage = true;
        //                ToScanPackageNos = backageNos;
        //                if (!ToScanPackageNos.Any())
        //                    ShowError("数据错误，请重新开始".L10N());
        //                else
        //                    ShowTips("请扫描[{0}]包装条码".L10nFormat(ToScanPackageNos.Peek()));
        //            }
        //            else
        //                ShowError(exc);
        //        }
        //        finally
        //        {
        //            Barcode = string.Empty;
        //        }
        //    }








        //    try
        //        {
        //            ClearInfos();
        //            var workcell = GetWorkcell();
        //            if (IsNeedToStep())
        //            {
        //                var currentStep = Step.CurrentStep;
        //                var collectBarcode = new CollectBarcode { Code = Barcode, Type = currentStep.BarcodeType };
        //                if (OuterPackingRelation != null || ScanMode != ScanMode.Join)
        //                    Validate(collectBarcode, workcell);
        //                Step.Barcodes.Add(collectBarcode.Code);
        //                var collectData = new CollectData() { CollectBarcode = collectBarcode };
        //                if (!Step.NextStep())
        //                    Collect(workcell, collectData);
        //                else
        //                    ShowTips("[条码:{0}]扫描成功，请扫描[{1}]".L10nFormat(collectBarcode, Step.ProcessSteps.FirstOrDefault(p => p.Step == (Step.StepIndex + 1)).BarcodeType.ToLabel()));//currentStep.BarcodeType.ToLabel()));
        //            }
        //            else
        //            {
        //                ValidateWipWorkOrder(Barcode);
        //                Collect(workcell, new CollectData());
        //            }
        //        }
        //        catch (Exception exc)
        //        {
        //            Step.Reset();
        //            ShowError(exc.GetBaseException().Message);
        //        }
        //        finally
        //        {
        //            ResetPacking();
        //        }
       
        //}


        //#region 加入包装
        ///// <summary>
        ///// 加入包装
        ///// </summary>
        ///// <param name="barcode">采集条码</param>
        ///// <param name="collectData">采集数据</param>
        ///// <param name="workcell">工作单元</param>
        //private void JoinPacking(string barcode, CollectData collectData, SIE.MES.WIP.Workcell workcell)
        //{
        //    try
        //    {
        //        if (IsChildNode(barcode))
        //        {
        //            ReadPackageCode(barcode, collectData.PackingData.DesignatedOuterPackingUnit);
        //        }
        //        else
        //        {
        //            ReadBulkCode(barcode, collectData, workcell);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        OnFaile(barcode, ex);
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 外包装条码
        ///// </summary>  
        ///// <param name="barcode">条码</param>
        ///// <param name="unit">包装单位</param>
        //void ReadPackageCode(string barcode, PackingUnit unit)
        //{
        //    SetParentNode(barcode);
        //    OnSucess(unit);
        //}

        ///// <summary>
        ///// 设置父节点
        ///// </summary>
        ///// <param name="barcode">编码</param>
        //protected virtual void SetParentNode(string barcode)
        //{
        //    var innerPackageRelation = RT.Service.Resolve<PackingRelationController>().GetPackingRelation(barcode, false);
        //    PackingRelation _outerPackingRelation = null;
        //    if (innerPackageRelation != null)
        //    {
        //        _outerPackingRelation = innerPackageRelation;
        //    }
        //    else if (innerPackageRelation == null)
        //    {
        //        var itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabel(barcode);
        //        if (itemLabel?.Relation != null) _outerPackingRelation = itemLabel.Relation;
        //    }

        //    if (_outerPackingRelation == null)
        //        throw new ValidationException("系统无此条码[{0}]记录".L10nFormat(barcode));

        //    if (_outerPackingRelation.RootId > 0)
        //        OuterPackingRelation = RF.GetById<PackingRelation>(_outerPackingRelation.RootId);
        //    else
        //    {
        //        while (_outerPackingRelation.GetTreePId() != null)
        //        {
        //            if (_outerPackingRelation.GetTreePId() != null)
        //                _outerPackingRelation = RF.GetById<PackingRelation>(_outerPackingRelation.TreePId) as PackingRelation;
        //        }

        //        OuterPackingRelation = _outerPackingRelation;
        //    }
        //}

        ///// <summary>
        ///// 读取扩展编码
        ///// </summary>
        ///// <param name="barcode">条码</param>
        ///// <param name="collectData">采集结果</param>
        ///// <param name="workcell">工作单元</param>
        //void ReadBulkCode(string barcode, CollectData collectData, SIE.MES.WIP.Workcell workcell)
        //{
        //    SetBrotherNode(barcode);
        //    InsideBarcode = barcode;
        //    if (!collectData.Context.Contains(typeof(PackingRuleValidMode).Name))
        //        collectData.Context.Add(typeof(PackingRuleValidMode).Name, _config.PackingRuleValidMode);
        //    else collectData.Context[typeof(PackingRuleValidMode).Name] = _config.PackingRuleValidMode;
        //    if (!IsSn)
        //    {
        //        OuterPackingRelation = Controller.JoinInPackage(barcode, collectData, workcell);
        //    }
        //    else
        //    {
        //        Controller.Collect(Step.Barcodes.ToArray(), collectData, workcell);
        //        var label = RT.Service.Resolve<ItemLabelController>().GetItemLabel(barcode);
        //        OuterPackingRelation = label.Relation;
        //    }

        //    OnSucess(collectData.PackingData.DesignatedOuterPackingUnit);
        //}

        ///// <summary>
        ///// 设置兄弟节点
        ///// </summary>
        ///// <param name="barcode">条码</param>
        //private void SetBrotherNode(string barcode)
        //{
        //    if (IsBrotherNode(barcode) && OuterPackingRelation.GetTreePId() != null)
        //    {
        //        OuterPackingRelation = RF.GetById<PackingRelation>(OuterPackingRelation.TreePId) as PackingRelation;
        //    }
        //}

        ///// <summary>
        ///// 是否兄弟结点
        ///// </summary>
        ///// <param name="barcode">条码</param>
        ///// <returns>bool</returns>
        //private bool IsBrotherNode(string barcode)
        //{
        //    var innerPackageRelation = RT.Service.Resolve<PackingRelationController>().GetPackingRelation(barcode, false);
        //    return OuterPackingRelation.PackageUnitId == innerPackageRelation?.PackageUnitId;
        //}

        ///// <summary>
        ///// 是否子节点
        ///// </summary>
        ///// <param name="barcode">编码</param>
        ///// <returns>bool</returns>
        //bool IsChildNode(string barcode)
        //{
        //    if (OuterPackingRelation == null) return true;

        //    if (!_config.IsContinuityControl) return false;

        //    var innerPackageRelation = RT.Service.Resolve<PackingRelationController>().GetPackingRelation(barcode, false);
        //    if (innerPackageRelation != null && ChildPackageRuleLevel.PackageUnitId != innerPackageRelation.PackageUnitId)
        //        return true;

        //    return false;
        //}
        //#endregion

    }
}