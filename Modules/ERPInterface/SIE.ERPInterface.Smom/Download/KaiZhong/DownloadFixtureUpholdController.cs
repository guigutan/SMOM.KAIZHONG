using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MES.Andon;
using SIE.MES.Fixture;
using SIE.MES.LineAndon;
using SIE.Resources.Enterprises;
using SIE.Resources.WorkCenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadFixtureUpholdController : DomainController
    {

        /// <summary>
        /// 保存分类到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveFixtureUpholdFactory(List<FixtureUpholdData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<FixtureUpholdData> list = new List<FixtureUpholdData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.FixtureUphold, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //工厂字典
                    Dictionary<string, Enterprise> dicFactory = new Dictionary<string, Enterprise>();

                    foreach (var data in datas)
                    {
                        try
                        {
                            OuterSystemRetVO result = new OuterSystemRetVO();
                            Valid(data, ref result);
                            if (!result.errorMsg.IsNullOrEmpty())
                                throw new ValidationException(result.errorMsg);

                            Enterprise factory = null;
                            if (dicFactory.ContainsKey(data.FactoryCode))
                            {
                                factory = dicFactory[data.FactoryCode];
                            }
                            else
                            {
                                factory = RT.Service.Resolve<EnterpriseController>().GetFactoriesList(null, data.FactoryCode).FirstOrDefault(p => p.Code == data.FactoryCode);
                                if (factory == null)
                                    throw new ValidationException("工厂{0}不存在".L10nFormat(data.FactoryCode));
                                dicFactory.Add(factory.Code, factory);
                            }

                            SaveFixtureUphold(data, factory);
                            apiResult.SuccessList.Add(data);
                        }
                        catch (Exception ex)
                        {
                            failCount++;
                            apiResult.ErrorList.Add(ex.GetBaseException().Message);
                            apiResult.ErrorObjList.Add(data);
                        }
                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<FixtureUpholdData>(erpDataInfLog, list, apiResult);
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
                logController.UpadateLogData<FixtureUpholdData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data"></param>
        /// <param name="factory"></param>
        public virtual void SaveFixtureUphold(FixtureUpholdData data, Enterprise factory)
        {
            try
            {
                FixtureUphold fixtureUphold = new FixtureUphold();

                fixtureUphold.PersistenceStatus = PersistenceStatus.New;
                fixtureUphold.FixtureCode = data.FixtureCode;
                fixtureUphold.FixtureName = data.FixtureName;
                fixtureUphold.FixtureState = data.FixtureState;
                fixtureUphold.FixtureType = data.FixtureType;
                fixtureUphold.Drawn = data.Drawn;
                fixtureUphold.FactoryId = factory.Id;

                RF.Save(fixtureUphold);
            }
            catch (Exception ex)
            {
                if (!ex.GetBaseException().Message.Contains("已经存在"))
                {
                    throw new ValidationException(ex.GetBaseException().Message);
                }
            }
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="data"></param>
        /// <param name="result"></param>
        public virtual void Valid(FixtureUpholdData data, ref OuterSystemRetVO result)
        {
            if (data.FixtureCode.IsNullOrEmpty())
            {
                var msg = "工装唯一码不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.FixtureName.IsNullOrEmpty())
            {
                var msg = "工装物料描述不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.FixtureState.IsNullOrEmpty())
            {
                var msg = "工装状态不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.FixtureType.IsNullOrEmpty())
            {
                var msg = "工装类型不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.FactoryCode.IsNullOrEmpty())
            {
                var msg = "工厂编码不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
        }
    }
}
