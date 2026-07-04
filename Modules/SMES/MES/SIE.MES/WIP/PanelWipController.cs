using SIE.Barcodes;
using SIE.Barcodes.Panels;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.PanelBindings;
using SIE.MES.WIP.Models;
using System;
using System.Linq;
using SIE.MES.WIP.Runtime;
using SIE.Tech.Processs;
using SIE.MES.WIP.Products;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 拼板采集控制器
    /// </summary>
    public class PanelWipController : WipController
    {
        /// <summary>
        /// 验证：
        /// 1.产品工艺路线。
        /// 2.工单状态。
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>返回产品信息</returns> 
        /// <exception cref="ArgumentNullException">采集条码为空、工作单元为空</exception>
        public override ProductInfo Validate(CollectBarcode barcode, Workcell workcell)
        {
            if (barcode == null)
                throw new ArgumentNullException(nameof(barcode));
            if (workcell == null)
                throw new ArgumentNullException(nameof(workcell));
            var productInfo = ValidatePanelCode(barcode.Code);  
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                product product = null;
                PanelInfo info = productInfo.PanelInfo;
                if (productInfo.BarcodeType == BarcodeType.CombinedCode && info.IsEmptyPanel)
                {
                    barcode.Type = BarcodeType.CombinedCode;
                    barcode.Code = productInfo.Barcode;
                    product = ValidateProduct(barcode, workcell);
                    var current = product.Routing.GetNext().FirstOrDefault(p => p.ProcessId == workcell.ProcessId);
                    if (current?.IsBinding == true)
                        info.IsBinding = true;
                }
                else
                {
                    if (info.SnList.Count == 0)
                        throw new ValidationException("数据异常，产品条码为空".L10N());
                    foreach (var sn in info.SnList)
                    {
                        //排除拼板中报废或者已完工的条码
                        var version = RT.Service.Resolve<WipProductVersionController>().GetWipProductVersion(sn.Sn);
                        if (version != null && (version.IsScrapped || version.IsFinish))
                            continue;
                        CollectBarcode collectBarcode = new CollectBarcode() { Code = sn.Sn, Type = BarcodeType.SN };
                        product = ValidateProduct(collectBarcode, workcell);
                        //ValidateProcessOvertime(collectBarcode, workcell, product);
                        ValidateWorkOrder(product);
                    }
                }
                var result = new ProductInfo();
                result.ItemId = product == null ? 0 : product.ItemId;
                result.Puid = product == null ? "" : product.Puid;
                result.WorkOrderId = product == null ? 0 : product.WorkOrderId; 
                OnValidateFinish(barcode, workcell, product, result);
                tran.Complete();
                return result;
            }
        } 

        /// <summary>
        /// 验证目检条码
        /// </summary>
        /// <param name="barcode">条码/拼板码</param>
        /// <returns>目检验证结果</returns>
        protected virtual ProductInfo ValidatePanelCode(string barcode)
        {
            var info = new ProductInfo();
            var panelController = RT.Service.Resolve<PanelController>();
            var panel = panelController.GetPanel(barcode);
            if (panel != null)
            {
                ValidatePanel(barcode, info, panel);
                return info;
            }
            //拼板不存在，可能是SN做为拼板码。通过拼板码绑定关系，获取到拼板码
            var isSnAsPanel = RT.Service.Resolve<CommonController>().IsExistData<PanelAndBarcode>(p => p.PanelCode == barcode);
            if (isSnAsPanel)
                ValidateSnPanel(barcode, info);
            else
            {
                //判断条码是否绑定拼板，绑定的话按拼板过站
                var bindingRecord = RT.Service.Resolve<PanelBindingController>().GetPanelAndBarcodeBySn(barcode);
                if (bindingRecord != null)
                    ValidateSnPanel(bindingRecord.PanelCode, info);
                else
                    ValidateSn(barcode, info);
            }
            return info;
        }

        /// <summary>
        /// 验证拼板
        /// </summary>
        /// <param name="barcode">拼板码</param>
        /// <param name="info"> </param>
        /// <param name="panel">拼板</param>
        private void ValidatePanel(string barcode, ProductInfo info, Panel panel)
        {
            //不绑定拼板码：拼板码必须是已绑定状态
            //绑定拼板码：拼板码已绑定直接过站，未绑定，进行绑定过站   
            if (panel.IsScrap)
                throw new ValidationException("拼板码[{0}]已报废".L10nFormat(barcode));
            info.BarcodeType = Tech.Processs.BarcodeType.CombinedCode;
            info.Barcode = barcode;
            var data = info.PanelInfo;
            var panelIsBinding = RT.Service.Resolve<PanelBindingController>().PanelIsBinding(panel.Id);
            if (panelIsBinding)
            {
                var panelBarcodes = RT.Service.Resolve<PanelBindingController>().GetPanelBindingSn(barcode);
                data.SnList.AddRange(panelBarcodes.Select(p => new SnData() { Sn = p, Qty = 1 }));
            }
            else
            {
                data.IsEmptyPanel = true;
                data.CanBindQty = panel.WorkOrder.PanelQty;
                data.ForkPlateQty = panel.ForkPlateQty;
            }
            //throw new ValidationException("拼板码[{0}]未绑定产品".L10nFormat(barcode));
        }

        /// <summary>
        /// 验证Sn做拼板
        /// </summary>
        /// <param name="barcode">条码</param> 
        /// <param name="info">采集数据</param>
        private void ValidateSnPanel(string barcode, ProductInfo info)
        {
            var panelBarcodes = RT.Service.Resolve<PanelBindingController>().GetPanelBindingSn(barcode);
            var data = info.PanelInfo;
            info.BarcodeType = Tech.Processs.BarcodeType.CombinedCode;
            info.Barcode = barcode;
            data.SnList.AddRange(panelBarcodes.Select(p => new SnData() { Sn = p, Qty = 1 }));
            data.SnAsPanel = true;
        }

        /// <summary>
        /// 验证Sn
        /// </summary>
        /// <param name="barcode">条码</param> 
        /// <param name="data">目检采集数据</param>
        private void ValidateSn(string barcode, ProductInfo data)
        {
            //2、验证条码信息
            ValidateSn(barcode);
            data.PanelInfo.SnList.Add(new SnData() { Sn = barcode, Qty = 1 });
        }

        /// <summary>
        /// 验证SN
        /// </summary>
        /// <param name="barcode">产品条码</param>
        private void ValidateSn(string barcode)
        {
            var sn = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode);
            if (sn == null)
                throw new ValidationException("条码[{0}]不存在".L10nFormat(barcode));
            if (sn.IsScraped)
                throw new ValidationException("条码[{0}]已经报废".L10nFormat(barcode));
            if (sn.IsPending)
                throw new ValidationException("条码[{0}]已经挂起".L10nFormat(barcode));
        }
    }
}