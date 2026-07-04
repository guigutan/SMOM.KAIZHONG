using SIE.Barcodes;
using SIE.Barcodes.Configs;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Core;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP.Models;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Repairs;
using SIE.MES.WIP.Runtime;
using SIE.Resources.WipResources;
using SIE.Tech;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.WIP.TemporaryRepairs
{
    /// <summary>
    /// 临时维修控制器
    /// </summary>
    public class WipTemporaryRepairController : RepairController
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public override ProductInfo Validate(CollectBarcode barcode, Workcell workcell)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }
            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }
            ValidateBarcode(barcode, workcell);
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var snInfo = GetMoveSns(barcode);
                List<string> sns = snInfo.Item1;
                product product = null;
                List<SnData> snDatas = new List<SnData>();

                product = ValidateProduct(barcode, workcell);
                ValidateWorkOrder(product);
                foreach (var sn in sns)
                {
                    snDatas.Add(new SnData() { Sn = sn, Qty = product.Qty });
                }

                //WipProductProcessState wipProductProcessState = WipProductProcessState.Finish;

                //if (product != null)
                //{
                //    wipProductProcessState = product.GetNextWipProductProcessState(workcell.ProcessId);
                //}

                ProductInfo result = InitResult(barcode, workcell, snInfo, product, snDatas);
                if (product?.Routing.Current != null)
                {
                    result.LastResultType = product.Routing.Current.Result;
                }
                //result.WipProductProcessState = wipProductProcessState;

                //OnValidateFinish(barcode, workcell, product, result);
                tran.Complete();
                return result;
            }
        }

        /// <summary>
        /// 获取当前版本
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public virtual WipProductVersion GetCurrentVersion(string barcode)
        {
            return FindLastWipProductVersion(barcode);
        }


        /// <summary>
        /// 是否存在开始维修记录
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="sn"></param>
        /// <param name="processId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public virtual WipProductProcess IsExsitedWipProductProcess(double workOrderId,string sn,double processId,double versionId)
        {
             return Query<WipProductProcess>()
                .Where(p => p.Barcode == sn &&p.ProcessId==processId&&p.VersionId== versionId)
                .OrderByDescending(m=>m.UpdateDate).FirstOrDefault();
        }


        /// <summary>
        /// 验证产品
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <param name="wipResourceMove"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        protected override product ValidateProduct(CollectBarcode barcode, Workcell workcell, WipResourceMove wipResourceMove = null)
        {
            var product = RuntimeController.FindProduct(barcode);
            var version = FindLastWipProductVersion(barcode);
            if (product == null) //可能未上线，或者数据已清空
            {
                if (version == null) ////没有生产记录，创建新产品
                {
                    if (barcode.Type == BarcodeType.SN)
                    {
                        var rconfig = ConfigService.GetConfig(new CheckBcRangeStateConfig());
                        if (rconfig != null && rconfig.IsCheck)
                        {
                            var sn = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode.Code);
                            if (sn.Range.State == ReceiveState.NoReceive)
                                throw new ValidationException("[{0}]未领用,不允许上线".L10nFormat(barcode));
                        }
                    }
                    product = CreateNewProduct(barcode, workcell);
                }
                else if (version.IsFinish)
                {
                    if (version.IsScrapped) ////报废的尝试重用创建新产品
                        product = CreateScrapReuseProduct(version, barcode, workcell);
                    else
                    {
                        //完成的尝试创建新版本
                        product = CreateVersionProduct(version.Product, barcode, workcell);
                    }
                }
                else
                {
                    //根据version还原product
                    product = RecoverProduct(version);
                }
            }
            else
            {
                if (version != null)
                {
                    if (version.IsOutsourcing)
                    {
                        throw new ValidationException("产品【{0}】状态为【委外加工中】，不能继续过站，如委外加工完成，请确认是否已【委外入库】!"
                            .L10nFormat(barcode));
                    }
                }
            }
            return product;
        }

        /// <summary>
        /// 重写父类的采集
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="collectData"></param>
        /// <param name="workcell"></param>
        /// <param name="uplineProcess"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public override void Collect(string barcode, CollectData collectData, Workcell workcell, double? uplineProcess = null)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }

            if (collectData == null)
            {
                throw new ArgumentNullException(nameof(collectData));
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                Save(new[] { barcode }, collectData, workcell);
                tran.Complete();
            }
        }

        /// <summary>
        /// 维修完成
        /// </summary>
        /// <param name="barcodes"></param>
        /// <param name="collectData"></param>
        /// <param name="workcell"></param>
        /// <exception cref="EntityNotFoundException"></exception>
        private void Save(string[] barcodes, CollectData collectData, Workcell workcell)
        {
            //验证工作单元信息                
            ValidateWorkcellEx(workcell);

            var wipResourceMove = GetById<WipResourceMove>(workcell.ResourceId);
            if (wipResourceMove == null)
            {
                throw new EntityNotFoundException(typeof(WipResource), workcell.ResourceId);
            }

            ////验证采集步骤
            var collectStepList = RT.Service.Resolve<ProcessController>().GetProcessCollectSteps(workcell.ProcessId).OrderBy(p => p.Step).ToList();
            var collectBarcodes = ValidateCollectStep(barcodes, collectStepList.ToArray(), collectData.CollectBarcode, collectData.NoValidateStep);

            ////验证产品工艺路线
            var product = ValidateProduct(collectBarcodes[0], workcell, wipResourceMove);

            //验证条码
            for (int i = 1; i < collectBarcodes.Count; i++)
            {
                ValidateBarcode(collectBarcodes[i], workcell);
            }

            ////验证工单
            ValidateWorkOrder(product);

            ////验证暂停的产品
            if (product.IsHold)
            {
                ValidateHoldProductEx(workcell.ProcessId, collectData);
            }
            var dateTime = RF.Find<Process>().GetDbTime();

            //最后过站时间
            product.Routing.LastMoveDateTime = dateTime;

            //当前在制版本
            var version = GetWipProductVersion(product.Puid);

            //条码关联Puid
            MapBarcodes(product, version, collectBarcodes, workcell);

            //创建过站记录

            var wipProductProcess = CreateWipProductProcess(product, version, collectBarcodes, collectData, workcell, wipResourceMove);

            RF.Save(wipProductProcess);
            if (collectData.State == WipProductProcessState.Finish)
            {
                Finish(collectData, workcell, collectBarcodes, product, wipProductProcess);
            }
            RF.Save(version);
            RF.Save(version.Product);
        }

        /// <summary>
        /// 维修完成记录
        /// </summary>
        /// <param name="collectData"></param>
        /// <param name="workcell"></param>
        /// <param name="collectBarcodes"></param>
        /// <param name="product"></param>
        /// <param name="wipProductProcess"></param>
        /// <exception cref="ValidationException"></exception>
        private void Finish(CollectData collectData, Workcell workcell, IList<CollectBarcode> collectBarcodes, product product, WipProductProcess wipProductProcess)
        {
            if (collectData.State == WipProductProcessState.Finish)
            {
                //过站记录创建后事件处理
                OnWipProductProcessFinished(wipProductProcess, product, collectBarcodes, collectData, workcell);

                if (wipProductProcess.IsDirty)
                {
                    throw new ValidationException("请不要在【过站记录创建后事件处理】中修改工序过站记录。".L10N());
                }
            }
        }
    }
}
