using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.FixtureReceives.Configs;
using SIE.Equipments.Enums;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 工治具扫描接收控制器
    /// </summary>
    public class FixtureReceiveScanController : DomainController
    {
        /// <summary>
        /// 保存扫描接收
        /// </summary>
        /// <param name="model"></param>
        /// <param name="details"></param>
        /// <param name="snList"></param>
        public virtual void SaveReceiveScan(ReceiveScanViewModel model, IList<FixtureReceiveDetail> details, IList<FixtureReceiveSn> snList)
        {
            if (model == null || details == null || !details.Any())
            {
                throw new ValidationException("数据异常，保存数据为空".L10N());
            }
            snList = snList ?? new List<FixtureReceiveSn>();
            var receive = RT.Service.Resolve<FixtureReceiveController>().GetFixtureReceiveById(model.FixtureReceiveId);
            if (receive == null)
            {
                throw new ValidationException("数据异常，找不到id为：{0}的工治具接收".L10nFormat(model.FixtureReceiveId));
            }
            var fixtureIDAccountList = new EntityList<FixtureIDAccount>();

            foreach (var sn in snList)
            {
                var receiveDetail = RT.Service.Resolve<FixtureReceiveController>().GetFixtureReceiveDetailById(sn.FixtureReceiveDetailId);
                if (receiveDetail == null)
                {
                    throw new ValidationException("序列号数据异常，找不到id为：{0}的接收明细".L10nFormat(sn.FixtureReceiveDetailId));
                }
                if (model.ReceiveType != ReceiveType.Outsourced && !sn.IsCreatedFixtureAccount)//不是【委外返厂】时,写入工治具台账表
                {
                    fixtureIDAccountList.Add(GenerateFixtureIdAccount(receive, receiveDetail, sn));
                    sn.IsCreatedFixtureAccount = true;
                }
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                if (details.Any())
                {
                    var detailIds = details.Select(m => m.Id).ToList();
                    var orgFixtureReceiveSns = Query<FixtureReceiveSn>().Where(m => detailIds.Contains(m.FixtureReceiveDetailId)).ToList().ToList();//原来数据库的SN

                    var deleteList = ExceptGetDeleteItem(snList.ToList(), orgFixtureReceiveSns);
                    if (deleteList.Any())
                    {
                        var selectedIds = deleteList.Select(m => m.Id).ToList();
                        if (model.ReceiveType != ReceiveType.Outsourced)
                        {
                            var deleteSnList = Query<FixtureReceiveSn>().Where(p => selectedIds.Contains(p.Id)).ToList();
                            if (deleteSnList.Any())//取出工治具序列号删除工治具台账
                            {
                                var sns = deleteSnList.Select(m => m.Sn).ToList();
                                DB.Delete<FixtureIDAccount>().Where(p => sns.Contains(p.Code)).Execute();
                            }
                        }
                        DB.Delete<FixtureReceiveSn>().Where(p => selectedIds.ToList().Contains(p.Id)).Execute();
                    }
                }
                var saveSnList = new EntityList<FixtureReceiveSn>();
                if (snList.Any())
                {
                    saveSnList.AddRange(snList);
                    RF.Save(saveSnList);
                }
                if (fixtureIDAccountList.Any())
                {
                    RF.Save(fixtureIDAccountList);
                }

                var saveDetailList = new EntityList<FixtureReceiveDetail>();
                details.ForEach(detail =>
                {
                    detail.PersistenceStatus = PersistenceStatus.Modified;
                    saveDetailList.Add(detail);
                });

                RF.Save(saveDetailList);
                trans.Complete();
            }
        }

        /// <summary>
        /// 交集判断
        /// </summary>
        /// <param name="newSn"></param>
        /// <param name="exsitFixtureReceiveSn"></param>
        /// <returns></returns>
        private List<FixtureReceiveSn> ExceptGetDeleteItem(List<FixtureReceiveSn> newSn, List<FixtureReceiveSn> exsitFixtureReceiveSn)
        {
            List<FixtureReceiveSn> result = new List<FixtureReceiveSn>();
            foreach (var item in exsitFixtureReceiveSn)
            {
                if (!newSn.Any(m => m.Id == item.Id))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// 创建工治具ID
        /// </summary>
        /// <param name="receive"></param>
        /// <param name="receiveDetail"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        private FixtureIDAccount GenerateFixtureIdAccount(FixtureReceive receive, FixtureReceiveDetail receiveDetail, FixtureReceiveSn sn)
        {
            if (receive == null || receiveDetail == null || sn == null)
            {
                return null;
            }
            return new FixtureIDAccount()
            {
                AccountState = FixtureAccountState.WaitShelf,
                Code = sn.Sn,
                FixtureEncodeId = receiveDetail.FixtureEncodeId,
                CustomerId = receiveDetail.CustomerId,
                OriginalSN = sn.OriginalSn,
                FixtureTypeId = receiveDetail.FixtureEncode.FixtureModel.FixtureTypeId,
                SupplierId = receiveDetail.SupplierId,
                Proprietorship = receive.ReceiveType == ReceiveType.Customer ? Proprietorship.ByCustomer : Proprietorship.Own,
                ProductionDate = sn.ProductionDate,
                Manufacturer = sn.Maker,
                TotalQty = 1,
                PersistenceStatus= PersistenceStatus.New
            };
        }

        /// <summary>
        /// 检查委外返修类型的数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual ReceiveExecuteInfo ReceiveExecute(string code, ReceiveScanViewModel model)
        {
            if (model == null)
            {
                throw new ValidationException("数据异常，待检查数据为空".L10N());
            }
            if (model.FixtureReceiveDetailId <= 0)
            {
                throw new ValidationException("请先选择接收明细行".L10N());
            }

            if (model.ScanSnCode)
            {
                return ScanSnCodeExecute(code, model);
            }
            if (model.ScanSn)
            {
                return ScanOriginalSnExecute(code, model);
            }
            if (model.ScanSnCodeAndSn)
            {
                return ScanSnCodeAndSnExecute(code, model);
            }
            return null;
        }

        /// <summary>
        /// 获取工治具接收SN对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private FixtureReceiveSn GetFixtureReceiveSnViewModel(ReceiveScanViewModel model)
        {
            if (model == null)
            {
                throw new ValidationException("数据异常，待检查数据为空".L10N());
            }
            var receiveDetail = RF.GetById<FixtureReceiveDetail>(model.FixtureReceiveDetailId, new EagerLoadOptions().LoadWithViewProperty());
            if (receiveDetail == null)
            {
                throw new ValidationException("请选择接收明细后再操作".L10N());
            }
            var snInfo = new FixtureReceiveSn();
            snInfo.LineNo = receiveDetail.LineNo;
            snInfo.PurOrderNo = receiveDetail.PurchaseOrder?.OrderNo;
            snInfo.OrderLineNo = receiveDetail.PurchaseOrderItem?.LineNo.ToString();
            snInfo.FixtureEncodeCode = receiveDetail.FixtureEncodeCode;
            snInfo.CustomerCode = receiveDetail.CustomerCode;
            snInfo.CustomerName = receiveDetail.CustomerName;
            snInfo.ModelCode = receiveDetail.ModelCode;
            snInfo.ModelName = receiveDetail.ModelName;
            snInfo.ManageMode = receiveDetail.ManageMode;
            snInfo.SupplierCode = receiveDetail.SupplierCode;
            snInfo.SupplierName = receiveDetail.SupplierName;
            snInfo.ExemptionInspect = receiveDetail.ExemptionInspect;
            snInfo.FixtureReceiveDetail = receiveDetail;
            snInfo.ProductionDate = model.ProductionDate;
            snInfo.Maker = model.Maker;
            return snInfo;
        }

        /// <summary>
        /// 扫描原厂序列号执行
        /// </summary>
        /// <param name="code"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private ReceiveExecuteInfo ScanOriginalSnExecute(string code, ReceiveScanViewModel model)
        {
            var info = new ReceiveExecuteInfo();
            if (model == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var snInfo = GetFixtureReceiveSnViewModel(model);
            info.SnInfo = snInfo;
            if (model.ReceiveType == SIE.Equipments.Enums.ReceiveType.Outsourced)
            {
                FixtureAccount account = GetFixtureAccountOutsideByCode(code);
                if (account == null)
                {
                    info.Message = "请扫描委外返厂的原厂序列号".L10N();
                    return info;
                }
                if (account.FixtureEncodeId != model.FixtureEncodeId)
                {
                    info.Message = "请扫描委外返厂的原厂序列号".L10N();
                    return info;
                }
                /*原厂序列号、生产日期、厂商名称字段带出；不是委外返厂时，原厂序列号字段为空、生产日期、厂商名称字段取填写的值*/
                info.SnInfo.FixtureReceiveDetailId = model.FixtureReceiveDetailId;
                info.SnInfo.OriginalSn = account.OriginalSN;
                info.SnInfo.ProductionDate = account.ProductionDate;
                info.SnInfo.Maker = account.Manufacturer;
            }
            else
            {
                FixtureAccount exsitAccount = GetFixtureAccountByOriginalSN(code);
                if (exsitAccount != null)
                {
                    info.Message = "原厂序列号{0}已存在工治具台账中,请确认".L10nFormat(code);
                    return info;
                }
                if (info.SnInfo.Sn.IsNullOrEmpty())
                {
                    info.SnInfo.Sn=GetSnNumber();
                }
                info.SnInfo.FixtureReceiveDetailId = model.FixtureReceiveDetailId;
                info.SnInfo.OriginalSn = code;
                info.SnInfo.ProductionDate = model.ProductionDate;
                info.SnInfo.Maker = model.Maker;
            }
            info.Success = true;
            info.Message = "请扫描原厂序列号".L10N();
            return info;
        }

        /// <summary>
        /// 扫描序列号编码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private ReceiveExecuteInfo ScanSnCodeExecute(string code, ReceiveScanViewModel model)
        {
            var info = new ReceiveExecuteInfo();
            if (model == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var snInfo = GetFixtureReceiveSnViewModel(model);
            info.SnInfo = snInfo;
            if (model.ReceiveType == SIE.Equipments.Enums.ReceiveType.Outsourced)
            {
                var account = GetFixtureAccountOutsideByCode(code);
                if (account == null)
                {
                    info.Message = "请扫描委外返厂的工治具序列号编码".L10N();
                    return info;
                }
                if (account.FixtureEncodeId != model.FixtureEncodeId)
                {
                    info.Message = "请扫描委外返厂的工治具序列号编码".L10N();
                    return info;
                }
                /*原厂序列号、生产日期、厂商名称字段带出；不是委外返厂时，原厂序列号字段为空、生产日期、厂商名称字段取填写的值*/
                info.SnInfo.FixtureReceiveDetailId = model.FixtureReceiveDetailId;
                info.SnInfo.OriginalSn = account.OriginalSN;
                info.SnInfo.ProductionDate = account.ProductionDate;
                info.SnInfo.Maker = account.Manufacturer;
                info.SnInfo.Sn = code;
            }
            else
            {
                var exsitAccount = RT.Service.Resolve<CoreFixtureController>().GetFixtureAccountByCodeOrRFID(code);
                if (exsitAccount != null)
                {
                    info.Message = "序列号编码{0}已存在工治具台账中,请确认".L10nFormat(code);
                    return info;
                }
                info.SnInfo.FixtureReceiveDetailId = model.FixtureReceiveDetailId;
                info.SnInfo.OriginalSn = "";
                info.SnInfo.ProductionDate = model.ProductionDate;
                info.SnInfo.Maker = model.Maker;
                info.SnInfo.Sn = code;
            }
            info.Success = true;
            info.Message = "请扫描序列号编码".L10N();
            return info;
        }

        /// <summary>
        /// 扫描序列号+原厂序列号
        /// </summary>
        /// <param name="code"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private ReceiveExecuteInfo ScanSnCodeAndSnExecute(string code, ReceiveScanViewModel model)
        {
            if (model == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var snInfo = GetFixtureReceiveSnViewModel(model);
            var info = new ReceiveExecuteInfo();
            info.SnInfo = snInfo;
            if (model.FirstSn.IsNullOrWhiteSpace())
            {
                var fixtureAccount = RT.Service.Resolve<CoreFixtureController>().GetFixtureAccountByCodeOrRFID(code);
                if (fixtureAccount != null)
                {
                    info.Success = false;
                    info.Message = "序列号编码{0}已存在于工治具台账中，请确认".L10nFormat(code);
                    return info;
                }
                else
                {
                    info.Success = true;
                    info.IsFirstSn = true;
                    info.Message = "请扫描原厂序列号".L10N();
                    return info;
                }
            }
            else
            {
                var account = GetFixtureAccountByOriginalSN(code);
                if (account != null)
                {
                    info.Success = false;
                    info.Message = "原厂序列号{0}已存在于工治具台账中，请确认".L10nFormat(code);
                    return info;
                }
                snInfo.Sn = model.FirstSn;
                snInfo.OriginalSn = code;

                info.Message = "请扫描序列号编码".L10N();
            }
            info.Success = true;
            info.SnInfo.OriginalSn = snInfo.OriginalSn;
            info.SnInfo.ProductionDate = model.ProductionDate;
            info.SnInfo.Maker = model.Maker;
            return info;
        }

        /// <summary>
        /// 根据原厂序列号获取工治具台账
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private FixtureAccount GetFixtureAccountByOriginalSN(string code)
        {
            return Query<FixtureAccount>().Where(m => m.OriginalSN == code && m.FixtureEncode.FixtureModel.ManageMode == ManageMode.Number).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 根据序列号获取委外的工治具台账
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private FixtureAccount GetFixtureAccountOutsideByCode(string code)
        {
            return Query<FixtureAccount>().Where(m => m.Code == code && m.FixtureEncode.FixtureModel.ManageMode == ManageMode.Number
             && m.AccountState == FixtureAccountState.OutedSide
            ).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 获取工治具编码
        /// </summary>
        /// <param name="page"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureAccount> GetFixtureIDCode(PagingInfo page, string code)
        {
            return Query<FixtureAccount>().Where(m => m.FixtureEncode.FixtureModel.ManageMode == ManageMode.Number
            && m.AccountState == FixtureAccountState.OutedSide)
                .WhereIf(!code.IsNullOrEmpty(), m => m.Code.Contains(code))
                .ToList(page, new EagerLoadOptions().LoadWithViewProperty());

        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="model"></param>
        /// <param name="maxLineNo"></param>
        /// <returns></returns>

        public virtual IList<FixtureReceiveSn> SnDetermine(ReceiveScanViewModel model, int maxLineNo)
        {
            if (model == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var list = new List<FixtureReceiveSn>();
            if (model.ReceiveType == ReceiveType.Outsourced)
            {
                var storeSummaryDetail = GetById<FixtureAccount>(model.SnCodeId);
                if (storeSummaryDetail == null)
                {
                    throw new ValidationException("请选择返厂的序列号编码".L10N());
                }

                var snInfo = GetFixtureReceiveSnViewModel(model);
                snInfo.Sn = storeSummaryDetail.Code;
                snInfo.LineNo = (maxLineNo + 1).ToString();
                snInfo.FixtureReceiveDetailId = model.FixtureReceiveDetailId;
                list.Insert(0, snInfo);
            }
            else
            {
                for (var i = 0; i < model.CurrentQty; i++)
                {
                    var snInfo = GetFixtureReceiveSnViewModel(model);
                    snInfo.Sn = GetSnNumber();
                    snInfo.LineNo = (maxLineNo + 1).ToString();
                    maxLineNo = int.Parse(snInfo.LineNo);
                    snInfo.FixtureReceiveDetailId = model.FixtureReceiveDetailId;
                    list.Insert(0, snInfo);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取SN
        /// </summary>
        /// <returns></returns>
        public virtual string GetSnNumber()
        {
            var config = ConfigService.GetConfig(new ReceiveSnNoConfig(), typeof(FixtureReceive));
            if (config == null || !config.NumberRuleId.HasValue)
            {
                throw new ValidationException("请维护序列号编码规则".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRuleId.Value, 1).FirstOrDefault();
        }
    }
}
