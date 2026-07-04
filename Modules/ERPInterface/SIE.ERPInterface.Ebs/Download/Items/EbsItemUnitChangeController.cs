using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Ebs.Download.Items
{
    /// <summary>
    /// 单位转换下载
    /// </summary>
    public class EbsItemUnitChangeController : DomainController
    {
        private bool IsInteger(decimal value)
        {
            return (value % 1) == 0;
        }

        /// <summary>
        /// 下载单位数据
        /// </summary>
        /// <param name="isManual">是否手工下载</param>        
        /// <returns>处理结果</returns>
        public virtual ProcessResult Download(int? invOrgId, bool isManual = false)
        {
            if (invOrgId.HasValue)
                AppRuntime.InvOrg = invOrgId;
            var ebsPara = EbsHelper.GetEbsParameter(false);
            //Copy必改内容
            ebsPara.InterfaceCode = "S_E2W_UNITCHANGE";//接口编码，接口卡有
            const JobType jobType = JobType.UnitChange;
            //End

            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            var jobTime = ctl.GetDownloadJobTime(jobType);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            if (jobTime?.LastDownloadDate.HasValue == true)
                ebsPara.DownParameter.LastUpdateDate = jobTime.LastDownloadDate.Value;

            DateTime beginTime = DateTime.Now;
            //ERP数据获取
            var soapResult = EbsHelper.ExecuteEbs<ItemUnitChangeData>(ebsPara);

            var codes = soapResult.XV_RESULT.Select(a => a.Item_Code).Distinct().ToList();
            var items = codes.SplitContains(pCodes =>
            {
                return Query<SIE.Items.Item>().Where(p => pCodes.Contains(p.Code)).ToList();
            });
            var unitcodesFrom = soapResult.XV_RESULT.Select(a => a.From_Uom_Code).Distinct().ToList();
            var unitcodesTo = soapResult.XV_RESULT.Select(a => a.To_Uom_Code).Distinct().ToList();
            var unitCodes = new List<string>();
            unitCodes.AddRange(unitcodesTo);
            unitCodes.AddRange(unitcodesFrom);
            var units = unitCodes.SplitContains(pCodes =>
            {
                return Query<SIE.Items.Unit>().Where(p => pCodes.Contains(p.Code)).ToList();
            });
            var itemIDs = items.Select(a => a.Id).ToList();
            List<double?> itemIdNull = new List<double?>();
            itemIDs.ForEach(a => { itemIdNull.Add(a); });
            var allDatas = new EntityList<SIE.Items.ItemUnit>();
            if (itemIDs.Count > 1000)
            {
                allDatas = itemIdNull.SplitContains(ids =>
                {
                    return Query<SIE.Items.ItemUnit>().Where(p => ids.Contains(p.ItemId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
                allDatas.AddRange(Query<SIE.Items.ItemUnit>().Where(p => p.IsBaseUnit).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            else
            {
                allDatas = itemIdNull.SplitContains(ids =>
              {
                  return Query<SIE.Items.ItemUnit>().Where(p => ids.Contains(p.ItemId) || p.IsBaseUnit).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
              });
            }

            //移除不存在并且是失效的数据
            soapResult.XV_RESULT.RemoveAll(x => !allDatas.Select(a => a.ItemCode).Contains(x.Item_Code) && x.Enable_Flag != "Y");
            soapResult.XV_RESULT.Where(f => f.Unit_Denominator > 0).ForEach(p =>
                {
                    var fromUnit = units.FirstOrDefault(a => a.Code == p.From_Uom_Code);
                    var toUnit = units.FirstOrDefault(a => a.Code == p.To_Uom_Code);
                    if (fromUnit != null && toUnit != null)//单位信息不存在的不要
                    {
                        if (allDatas.Any(f => f.MainUnitId == fromUnit.Id && f.UnitId == toUnit.Id && f.IsBaseUnit))
                            p.ItemUnit = null;//传入的单位转换已存在并且是基准单位，不需要插入数据
                        else
                        {
                            p.ItemUnit = allDatas.FirstOrDefault(a => a.ItemCode == p.Item_Code && a.MainUnitId == fromUnit.Id && a.UnitId == toUnit.Id);
                            if (p.Enable_Flag != "Y")
                            {//失效的转换需要删除
                                if (p.ItemUnit != null)
                                    p.ItemUnit.PersistenceStatus = PersistenceStatus.Deleted;
                                else
                                    p.ItemUnit = null;
                            }
                            else
                            {
                                if (p.ItemUnit == null)//没有该单位转换就新增
                                {
                                    if (p.Item_Code.IsNullOrEmpty())
                                    {
                                        p.ItemUnit = new SIE.Items.ItemUnit()
                                        {
                                            IsBaseUnit = true,
                                            MainUnitId = fromUnit.Id,
                                            UnitId = toUnit.Id,
                                            UnitSource = SIE.Items.Items.UnitSource.ERP,
                                        };
                                    }
                                    else
                                    {
                                        var item = items.FirstOrDefault(a => a.Code == p.Item_Code);
                                        if (item != null)//物料信息不存在不要
                                        {
                                            p.ItemUnit = new SIE.Items.ItemUnit()
                                            {
                                                ItemId = item.Id,
                                                MainUnitId = fromUnit.Id,
                                                UnitId = toUnit.Id,
                                                UnitSource = SIE.Items.Items.UnitSource.ERP,
                                            };
                                        }
                                    }
                                }
                                if (p.ItemUnit != null && (p.Unit_Denominator != p.ItemUnit.ErpDenominator || p.Unit_Molecule != p.ItemUnit.ErpNumerator))
                                {
                                    //这里是为了找到一个数mult可以让分母变成一个整数，然后分子分母同时×这个数
                                    int mult = 1;
                                    while (!IsInteger(p.Unit_Denominator * mult))
                                    {
                                        mult++;
                                    }
                                    //MOM的分子与分母 == ERP的分母与分子，MOM的是用主单位×分子除分母＝辅单位，ERP的是用主单位×分母除分子＝辅单位
                                    p.ItemUnit.Numerator = (int)(p.Unit_Denominator * mult);
                                    p.ItemUnit.Denominator = (int)(p.Unit_Molecule * mult);
                                    p.ItemUnit.ErpDenominator = p.Unit_Denominator;
                                    p.ItemUnit.ErpNumerator = p.Unit_Molecule;
                                }
                                else
                                {
                                    p.ItemUnit = null;
                                }
                            }
                        }
                    }
                });
            var result = RT.Service.Resolve<DownloadInfBaseController>().DownloadBusData(soapResult, p =>
             {    //Copy必改内容
                 if (p.ItemUnit != null)
                 {
                     return p.ItemUnit;
                 }
                 else
                 {
                     return null;
                 }
             }, jobType, jobTime, jobTimeDetail, beginTime, isManual);

            return result;
        }
    }
}
