using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.MES.BlueLable;
using SIE.MES.PackingQC;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using SIE.Warehouses.ItemStockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadBlueLabelController : DomainController
    {

        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveBlueLabels(List<BlueLabelData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<BlueLabelData> list = new List<BlueLabelData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.BlueLabel, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {

                    //蓝标
                    var blueLabelstr = datas.Select(p => p.EXIDV).Distinct().ToList();
                    var blueLabels = RT.Service.Resolve<BlueLableController>().GetBlueLableDatas(blueLabelstr);
                    //获取QC确认
                    var packingQcList = RT.Service.Resolve<PackingQcController>().GetPackingQcs(blueLabelstr);
                    //物料标签
                    var itemCodes = datas.Select(p => p.MATNR).Distinct().ToList();
                    var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes, new EagerLoadOptions().LoadWithViewProperty());

                    //获取工厂
                    var factoryCodes = datas.Select(p => p.WERKS).Distinct().ToList();
                    var factorys = RT.Service.Resolve<EnterpriseController>().GetFactoryByCode(factoryCodes);
                    //获取工单
                    //var wos=datas.Select(p=>p.AUFNR).Distinct().ToList();
                    //RT.Service.Resolve< WorkOrderController >()

                    foreach (var item in datas)
                    {
                        try
                        {
                            var blueLabel = blueLabels.FirstOrDefault(p => p.BlueLableBox == item.EXIDV);
                            Item product = new Item();
                            Enterprise factory = new Enterprise();
                            if (item.BIAOS != "删除")
                            {
                                product = items.FirstOrDefault(p => p.Code == item.MATNR);
                                if (product == null)
                                    throw new ValidationException("物料[{0}]不存在".L10nFormat(item.MATNR));
                                if (item.VEMNG <= 0)
                                    throw new ValidationException("包装数量小于或者等于[{0}]不存在".L10nFormat(item.VEMNG));
                                factory = factorys.FirstOrDefault(p => p.Code == item.WERKS);
                                if (factory == null)
                                    throw new ValidationException("企业模型中编码:[{0}]工厂不存在!".L10nFormat(item.WERKS));
                            }
                            //删除标识，如果等于0 就允许删除 等于1不允许删除
                            int deleteId = 0;
                            var packingqc = packingQcList.Where(p => p.BlueLabel == item.EXIDV).ToList();
                            if (packingqc.Count > 0)
                            {
                                deleteId = 1;
                            }
                            blueLabel = CreateBlueLable(item, blueLabel, product, factory, deleteId);
                            if (blueLabel != null && blueLabels.All(p => p.Id != blueLabel.Id))
                                blueLabels.Add(blueLabel);

                            list.Add(item);
                            apiResult.SuccessList.Add(item);
                        }
                        catch (Exception ex)
                        {
                            throw new ValidationException($"EXIDV:{item.EXIDV}:" + ex.GetBaseException()?.Message);
                        }
                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<BlueLabelData>(erpDataInfLog, list, apiResult);

                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能为空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorObjList.Clear();
                apiResult.ErrorObjList.AddRange(datas);
                apiResult.ErrorList.Add(ex.Message);
                logController.UpadateLogData<BlueLabelData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;
        }





        public virtual ApiCommonRes SaveBlueLabels(List<BlueLabelData> datas, ref InfNcDataLogGroup erpDataInfLog)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };

            List<BlueLabelData> list = new List<BlueLabelData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            int failCount = 0;

            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //蓝标
                    var blueLabelstr = datas.Select(p => p.EXIDV).Distinct().ToList();
                    var blueLabels = RT.Service.Resolve<BlueLableController>().GetBlueLableDatas(blueLabelstr);
                    //物料标签
                    var itemCodes = datas.Select(p => p.MATNR).Distinct().ToList();
                    var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes, new EagerLoadOptions().LoadWithViewProperty());

                    //获取工厂
                    var factoryCodes = datas.Select(p => p.WERKS).Distinct().ToList();
                    var factorys = RT.Service.Resolve<EnterpriseController>().GetFactoryByCode(factoryCodes);

                    //获取所有包装的蓝标
                    var packingQcList = RT.Service.Resolve<PackingQcController>().GetPackingQcs(blueLabelstr);

                    foreach (var item in datas)
                    {
                        try
                        {
                            if (item != null)
                            {
                                Item product = new Item();
                                Enterprise factory = new Enterprise();
                                var blueLabel = blueLabels.FirstOrDefault(p => p.BlueLableBox == item.EXIDV);
                                if (item.BIAOS != "删除")
                                {
                                    product = items.FirstOrDefault(p => p.Code == item.MATNR);
                                    if (product == null)
                                        throw new ValidationException("物料[{0}]不存在".L10nFormat(item.MATNR));
                                    if (item.VEMNG <= 0)
                                        throw new ValidationException("包装数量小于或者等于[{0}]不存在".L10nFormat(item.VEMNG));
                                    factory = factorys.FirstOrDefault(p => p.Code == item.WERKS);
                                    if (factory == null)
                                        throw new ValidationException("企业模型中编码:[{0}]工厂不存在!".L10nFormat(item.WERKS));
                                }
                                //删除标识，如果等于1 就允许删除 等于0不允许删除
                                int deleteId = 0;
                                var packingqc = packingQcList.Where(p => p.BlueLabel == item.EXIDV).ToList();
                                if (packingqc.Count > 0)
                                {
                                    deleteId = 1;
                                }
                                blueLabel = CreateBlueLable(item, blueLabel, product, factory, deleteId);
                                if (blueLabel != null && blueLabels.All(p => p.Id != blueLabel.Id))
                                    blueLabels.Add(blueLabel);
                                list.Add(item);
                                apiResult.SuccessList.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new ValidationException($"EXIDV:{item.EXIDV}:" + ex.GetBaseException()?.Message);
                        }
                    }
                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能未空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorList.Add(ex.Message);
            }
            finally
            {
                erpDataInfLog = RT.Service.Resolve<InfNcDataLogGroupController>().UpdateInfNcDataLogGroupData<BlueLabelData>(erpDataInfLog, datas.Count, list, apiResult, false);

            }
            return apiResult;
        }

        /// <summary>
        /// 创建蓝标
        /// </summary>
        /// <param name="data"></param>
        /// <param name="blueLable"></param>
        /// <param name="item"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        private BlueLable CreateBlueLable(BlueLabelData data, BlueLable blueLable, Item item, Enterprise factory, int deletedId)
        {
            //如果是删除的就不需要创建了
            if (blueLable == null)
            {
                blueLable = new BlueLable
                {
                    BlueLableBox = data.EXIDV
                };
            }
            else
            {
                blueLable.CreateDeleteident = data.BIAOS;
                if (blueLable.PersistenceStatus != PersistenceStatus.Unchanged)
                    RF.Save(blueLable);
                return blueLable;
            }

            blueLable.Item = item;
            blueLable.ItemId = item.Id;
            blueLable.BatchNo = data.CHARG;
            blueLable.ExternalIdent = data.EXIDV2;
            blueLable.StorageLocation = data.LGORT;
            blueLable.PackageNum = data.VEMNG;
            blueLable.CreateDeleteident = data.BIAOS;
            blueLable.ProductionNo = data.AUFNR;
            blueLable.FactoryId = factory.Id;
            if (blueLable.PersistenceStatus != PersistenceStatus.Unchanged)
                RF.Save(blueLable);
            return blueLable;

        }
    }
}
