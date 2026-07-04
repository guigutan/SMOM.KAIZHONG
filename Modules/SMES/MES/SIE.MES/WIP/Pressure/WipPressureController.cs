using SIE.Barcodes.WipBatchs;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Algorithms.KZ;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Dispatchs;
using SIE.MES.PackingQC;
using SIE.MES.WIP.Pressure.Configs;
using SIE.MES.WIP.Runtime;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP.Pressure
{
    /// <summary>
    /// 控制器
    /// </summary>
    public class WipPressureController : DomainController
    {

        /// <summary>
        /// 耐压测试批次数据查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<WipPressure> CriteriaWipPressure(WipPressureCriteria criteria)
        {
            var q = Query<WipPressure>();
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
                q.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));
            if (!criteria.Sn.IsNullOrEmpty())
                q.Exists<WipPressureSn>((x, y) => y.Where(p => p.WipPressureId == x.Id && p.Sn.Contains(criteria.Sn)));
            if (!criteria.BatchNo.IsNullOrEmpty())
                q.Where(p => p.BatchNo.Contains(criteria.BatchNo));
            if (!criteria.ProductCode.IsNullOrEmpty())
                q.Where(p => p.WorkOrder.Product.Code.Contains(criteria.ProductCode));
            if (!criteria.ProductName.IsNullOrEmpty())
                q.Where(p => p.WorkOrder.Product.Name.Contains(criteria.ProductName));
            if (!criteria.ResourceCode.IsNullOrEmpty())
                q.Where(p => p.Resource.Code.Contains(criteria.ResourceCode));
            if (!criteria.ResourceName.IsNullOrEmpty())
                q.Where(p => p.Resource.Name.Contains(criteria.ResourceName));
            if (criteria.BeginTime.BeginValue != null)
                q.Where(p => p.BeginTime >= criteria.BeginTime.BeginValue.Value);
            if (criteria.BeginTime.EndValue != null)
                q.Where(p => p.BeginTime <= criteria.BeginTime.EndValue.Value);
            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return list;
        }

        /// <summary>
        /// 获取耐压测试数据
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="eagerLoad"></param>
        public virtual WipPressure GetWipPressure(string batchNo, EagerLoadOptions eagerLoad = null)
        {
            return Query<WipPressure>().Where(p => p.BatchNo == batchNo).FirstOrDefault(eagerLoad);
        }

        /// <summary>
        /// 根据产品Id获取打印模板
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual WipPressurePrintTemlpate GetWipPressurePrintTemplate(double productId)
        {
            var wipTpl = Query<WipPressurePrintTemlpate>().Where(p => p.ProductId == productId).FirstOrDefault();
            return wipTpl;
        }

        /// <summary>
        /// 获取工序标签SN测试数量
        /// </summary>
        /// <param name="wipPressureId"></param>
        /// <returns></returns>
        public virtual int GetWipPressureSnQty(double wipPressureId)
        {
            var snQty = Query<WipPressureSn>().Where(p => p.WipPressureId == wipPressureId).Count();
            return snQty;
        }

        /// <summary>
        /// 验证码校验
        /// </summary>
        /// <param name="wipPressure"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public virtual bool VerifyCode(WipPressure wipPressure, string verifyCode)
        {
            var config = ConfigService.GetConfig(new WipPressureVerifyCodeConfig(), typeof(WipPressure));
            if (verifyCode != config.VerifyCode)
                return false;
            return true;
        }

        /// <summary>
        /// 获取最大打印数量
        /// </summary>
        /// <param name="qty"></param>
        /// <returns></returns>
        public virtual decimal GetMaxPrintCount(decimal qty)
        {
            var config = ConfigService.GetConfig(new WipPressureVerifyCodeConfig(), typeof(WipPressure));
            if (config?.OverPrintPercent > 0)
                return qty + Math.Ceiling(qty * config.OverPrintPercent / 100);
            return qty;
        }

        /// <summary>
        /// 解析rawData并生成SN数据(支持同时测试多个SN)
        /// </summary>
        /// <param name="rawDataStr"></param>
        /// <param name="wipPressure"></param>
        /// <param name="numberRuleId"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<WipPressureSn> GenerateWipPressureSns(string rawDataStr, WipPressure wipPressure, double numberRuleId, WipResource resource)
        {
            var list = new EntityList<WipPressureSn>();
            if (rawDataStr.IsNullOrEmpty())
                return list;

            var snQty = GetWipPressureSnQty(wipPressure.Id);
            var maxQty = wipPressure.IsAllowOver ? GetMaxPrintCount(wipPressure.OriginalQty) : wipPressure.OriginalQty;   //允许超打时,最大打印数据为原数量2倍
            if (snQty >= maxQty)
                throw new ValidationException("工序标签已完成所有的测试数量");
            //客户料码数据
            var itemCustomer = RT.Service.Resolve<ItemCusotmerDataController>().GetItemCusotmerData(wipPressure.ProductId.Value, batchNo: wipPressure.BatchNo, lineCode: resource?.Code);
            if (itemCustomer == null)
                throw new ValidationException("产品[{0}]还未维护客户料码数据".L10nFormat(wipPressure.Product?.Code));

            var dbTime = RF.Find<WipPressureSn>().GetDbTime();
            //解析数据格式
            rawDataStr = rawDataStr.Replace("\n", "\r");
            var rawDatas = rawDataStr.Split("\r", StringSplitOptions.RemoveEmptyEntries);
            foreach (var rawData in rawDatas)
            {
                var testValueList = new List<WipPressureTestValue>();
                string[] stepsArr = rawData.TrimEnd('\n', '\r', '\0').Split(';'); //AC,1.234,0.567,PASS;IR,0.345,1050.000,PASS
                foreach (var p in stepsArr)
                {
                    var dataArr = p.Split(',');
                    if (dataArr.Length == 4)
                    {
                        var testValue = new WipPressureTestValue()
                        {
                            TestItem = dataArr[0],
                            Value = dataArr[1],
                            TestResult = dataArr[3].ToUpper() == "PASS" ? TestResult.PASS : TestResult.FAIL
                        };
                        testValueList.Add(testValue);
                    }
                }
                if (testValueList.Count == 0)
                    throw new ValidationException("测试结果数据[{0}]解析失败".L10nFormat(rawData));

                var testResult = testValueList.All(p => p.TestResult == TestResult.PASS) ? TestResult.PASS : TestResult.FAIL;
                if (testResult == TestResult.FAIL)
                    throw new ValidationException("当前耐压测试结果为NG,请复测, 结果数据[{0}]".L10nFormat(rawData));

                itemCustomer.WorkOrderQty = wipPressure.WorkOrder?.OrderQty ?? 0;
                //生成SN数据
                var sn = new WipPressureSn()
                {
                    RawData = rawData,
                    TestResult = testResult,
                    TestTime = DateTime.Now,
                    WipPressure = wipPressure,
                    IsOver = snQty > wipPressure.Qty
                };
                sn.TestValueList.AddRange(testValueList);

                list.Add(sn);
            }

            if (list.Count == 0)
                return list;

            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                foreach (var sn in list)
                {
                    //生成SN号
                    var sn1 = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRuleId, 1, itemCustomer).FirstOrDefault();

                    NumberRule byId = GetById<NumberRule>(numberRuleId);
                    var length = byId.DetailList.Sum(p => p.Length);
                    if (sn1.Length != length)
                        throw new ValidationException("编码规则[{0}],基础数据不准确，编码位数设置为{1}，实际位数为{2}".L10nFormat(byId.Code, length, sn1.Length));

                    var sn2 = sn1;

                    if (sn2.Contains("*"))
                    {
                        //东风特殊暗码转换
                        //Q411*2105341-9D00C*S1*20250624*00001*100037*0001**
                        //Q411 2105341-9D00C S1 SF24 0001
                        var arr = sn2.Split('*');
                        if (arr.Length >= 7)
                        {
                            arr[3] = new DateDFSegmentAlgorithm().GetCode();
                            sn2 = $"{arr[0]}{arr[1]}{arr[2]}{arr[3]}{arr[6]}";
                        }
                    }
                    sn.Sn = sn1;
                    sn.Sn2 = sn2;

                    RF.Save(sn);
                }

                //更新测试批次 测试时间
                var update = DB.Update<WipPressure>().Set(p => p.EndTime, dbTime);
                if (wipPressure.BeginTime == null)
                {
                    update.Set(p => p.BeginTime, dbTime);
                }
                update.Where(p => p.Id == wipPressure.Id).Execute();

                var sumQty = GetWipPressureSnQty(wipPressure.Id);
                if (sumQty >= wipPressure.Qty)
                {
                    var isReport = RT.Service.Resolve<ITaskReportKZ>().ValidateProcessHasReport(wipPressure.BatchNo, "电性能测试,耐压测试");
                    if (!isReport)
                    {
                        //调用报工
                        var reportInfo = new ReportInfo()
                        {
                            Sn = wipPressure.BatchNo,
                            WorkOrderId = wipPressure.WorkOrderId ?? 0,
                            ResourceId = resource.Id,
                            GoodQty = wipPressure.Qty,
                        };
                        RT.Service.Resolve<ITaskReportKZ>().PressureReport(new List<ReportInfo>() { reportInfo });
                    }
                }

                trans.Complete();
            }
            var sns = list.Select(p => p.Sn).ToList();
            var snList = GetWipPressureSns(sns);
            return snList;
        }
        /// <summary>
        /// 解析rawData并生成SN数据
        /// </summary>
        /// <param name="rawData">
        /// rawData 数据格式：
        /// 测试项目，测试电压(V)，测试电流(mA)，分选结果；
        /// 1、 测试项目、测试数据之间分隔符为（，）。
        /// 2、 步骤之间分隔符为（；）。
        /// 3、 数据结束符默认为（0x0A）
        /// 测试结果是 
        /// STEP1：AC，1000V，测试电流 1mA，结果 PASS。
        /// STEP2：IR， 500V，测试电阻 100M，结果 PASS。
        /// 返回数据格式：
        /// AC，1.000，1.000，PASS；IR，0.500，100.000，PASS（0x0A）
        /// </param>
        /// <param name="wipPressure"></param>
        /// <param name="numberRuleId"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [Obsolete("当前方法不适用多个产品SN同时进行耐压测试场景,请使用 GenerateWipPressureSns 替代")]
        public virtual WipPressureSn GenerateWipPressureSn(string rawData, WipPressure wipPressure, double numberRuleId, WipResource resource)
        {
            if (rawData.Contains("\r") || rawData.Contains("\n"))
                throw new ValidationException("当前程序版本不适用多个产品SN同时进行耐压测试场景, 请退出程序重新登录, 并升级程序到最新版本");

            if (rawData.IsNullOrEmpty())
                return null;
            var testValueList = new List<WipPressureTestValue>();
            //解析数据格式
            string[] stepsArr = rawData.Split(';'); //AC,1.234,0.567,PASS;IR,0.345,1050.000,PASS
            foreach (var p in stepsArr)
            {
                var dataArr = p.Split(',');
                if (dataArr.Length == 4)
                {
                    var testValue = new WipPressureTestValue()
                    {
                        TestItem = dataArr[0],
                        Value = dataArr[1],
                        TestResult = dataArr[3].ToUpper() == "PASS" ? TestResult.PASS : TestResult.FAIL
                    };
                    testValueList.Add(testValue);
                }
            }
            if (testValueList.Count == 0)
                return null;
            var snQty = GetWipPressureSnQty(wipPressure.Id);
            var maxQty = wipPressure.IsAllowOver ? GetMaxPrintCount(wipPressure.OriginalQty) : wipPressure.OriginalQty;   //允许超打时,最大打印数据为原数量2倍
            if (snQty >= maxQty)
                throw new ValidationException("工序标签已完成所有的测试数量");

            var testResult = testValueList.All(p => p.TestResult == TestResult.PASS) ? TestResult.PASS : TestResult.FAIL;
            if (testResult == TestResult.FAIL)
                throw new ValidationException("当前耐压测试结果为NG,请复测, 结果数据[{0}]".L10nFormat(rawData));

            //客户料码数据
            var itemCustomer = RT.Service.Resolve<ItemCusotmerDataController>().GetItemCusotmerData(wipPressure.ProductId.Value, batchNo: wipPressure.BatchNo, lineCode: resource?.Code);
            if (itemCustomer == null)
                throw new ValidationException("产品[{0}]还未维护客户料码数据".L10nFormat(wipPressure.Product?.Code));
            itemCustomer.WorkOrderQty = wipPressure.WorkOrder?.OrderQty ?? 0;
            //生成SN数据
            var sn = new WipPressureSn()
            {
                RawData = rawData,
                TestResult = testResult,
                TestTime = DateTime.Now,
                WipPressure = wipPressure,
                IsOver = snQty > wipPressure.Qty
            };
            sn.TestValueList.AddRange(testValueList);
            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //生成SN号
                var sn1 = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRuleId, 1, itemCustomer).FirstOrDefault();
                NumberRule byId = GetById<NumberRule>(numberRuleId);
                var length = byId.DetailList.Sum(p => p.Length);
                if (sn1.Length != length)
                    throw new ValidationException("编码规则[{0}],基础数据不准确，编码位数设置为{1}，实际位数为{2}".L10nFormat(byId.Code, length, sn1.Length));

                var sn2 = sn1;

                if (sn2.Contains("*"))
                {
                    //东风特殊暗码转换
                    //Q411*2105341-9D00C*S1*20250624*00001*100037*0001**
                    //Q411 2105341-9D00C S1 SF24 0001
                    var arr = sn2.Split('*');
                    if (arr.Length >= 7)
                    {
                        arr[3] = new DateDFSegmentAlgorithm().GetCode();
                        sn2 = $"{arr[0]}{arr[1]}{arr[2]}{arr[3]}{arr[6]}";
                    }
                }
                sn.Sn = sn1;
                sn.Sn2 = sn2;

                RF.Save(sn);

                //更新测试批次 测试时间
                var update = DB.Update<WipPressure>().Set(p => p.EndTime, sn.TestTime);
                if (wipPressure.BeginTime == null)
                {
                    update.Set(p => p.BeginTime, sn.TestTime);
                }
                update.Where(p => p.Id == wipPressure.Id).Execute();

                var sumQty = GetWipPressureSnQty(wipPressure.Id);
                if (sumQty == wipPressure.Qty)
                {
                    //调用报工
                    var reportInfo = new ReportInfo()
                    {
                        Sn = wipPressure.BatchNo,
                        WorkOrderId = wipPressure.WorkOrderId ?? 0,
                        ResourceId = resource.Id,
                        GoodQty = wipPressure.Qty,
                    };
                    RT.Service.Resolve<ITaskReportKZ>().PressureReport(new List<ReportInfo>() { reportInfo });
                }

                trans.Complete();
            }
            sn = RF.GetById<WipPressureSn>(sn.Id, new EagerLoadOptions().LoadWithViewProperty());
            return sn;
        }

        /// <summary>
        /// 根据SN标签查询是否存在
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public virtual WipPressureSn GetWipPressureSn(string sn)
        {
            return Query<WipPressureSn>().Where(p => p.Sn == sn).FirstOrDefault();
        }

        /// <summary>
        /// 根据SN列表标签查询
        /// </summary>
        /// <param name="snList"></param>
        /// <returns></returns>
        public virtual EntityList<WipPressureSn> GetWipPressureSns(List<string> snList)
        {
            var list = snList.SplitContains(temp =>
            {
                return Query<WipPressureSn>().Where(p => temp.Contains(p.Sn)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据工单号获取耐压测试批次数据
        /// </summary>
        /// <param name="woNos"></param>
        /// <returns></returns>
        public virtual EntityList<WipPressure> GetWipPressuresByWoNos(List<string> woNos)
        {
            var list = woNos.SplitContains(nos =>
            {
                return Query<WipPressure>().Where(p => nos.Contains(p.WorkOrder.No)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据ID获取耐压测试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual WipPressure GetWipPressureById(double id)
        {
            return Query<WipPressure>().Where(p => p.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// 更新批次标签数量为耐压采集数量
        /// </summary>
        /// <param name="batchNos"></param>
        public virtual void UpdateBatchQty(List<string> batchNos)
        {
            var wipBatchs = RT.Service.Resolve<WipBatchController>().GetWipBatches(batchNos);
            foreach (var batchNo in batchNos)
            {
                var qty = GetSnRemainCount(batchNo);
                var wipBatch = wipBatchs.FirstOrDefault(p => p.BatchNo == batchNo);
                if (wipBatch != null && qty > wipBatch.Qty)
                {
                    //记录在哪个工序修改的工序,此处目前只用在包装工序，如后续有其他工序用到，也要进行相应的更改
                    if (wipBatch.Qty != qty)
                    {
                        wipBatch.EditQtyProcessCode = "包装";
                    }
                    DB.Update<WipBatch>().Set(p => p.Qty, qty).Where(p => p.Id == wipBatch.Id).Execute();
                }
            }
        }

        /// <summary>
        /// 根据批次号查询SN剩余待报工数量
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public virtual int GetSnRemainCount(string batchNo)
        {
            var reportCount = Query<PackingDetail>().Where(p => p.BatchLabel == batchNo && p.ReportsType == ReportsTypeEnum.YES).Count();
            var totalCount = Query<WipPressureSn>().Where(p => p.WipPressure.BatchNo == batchNo).Count();
            return totalCount - reportCount;
        }

        /// <summary>
        /// 设置SN是否可疑品
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="isSuspectProduct"></param>
        /// <param name="isRework"></param>
        /// <param name="isScraped"></param>
        /// <returns></returns>
        public virtual int SetSnSuspectState(string sn, bool isSuspectProduct, bool? isRework = null, bool? isScraped = null)
        {
            var wipSn = Query<WipPressureSn>().Where(p => p.Sn == sn).FirstOrDefault();
            if (wipSn == null)
                throw new ValidationException("耐压SN[{0}]不存在".L10nFormat(sn));
            var update = DB.Update<WipPressureSn>().Set(p => p.IsSuspectProduct, isSuspectProduct);
            if (isRework != null)
                update.Set(p => p.IsRework, isRework);
            if (isScraped != null)
                update.Set(p => p.IsScraped, isScraped);
            return update.Where(p => p.Id == wipSn.Id).Execute();
        }
    }
}
