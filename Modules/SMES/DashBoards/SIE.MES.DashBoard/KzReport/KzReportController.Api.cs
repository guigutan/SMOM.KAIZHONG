using Microsoft.Scripting.Utils;
using SIE.Andon.Andons.Enum;
using SIE.Api;
using SIE.Core.ApiModels;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.SmomControl;
using SIE.MES.DashBoard.KzBoard.Datas;
using SIE.MES.DashBoard.KzReport.Datas;
using SIE.MES.DashBoard.KzReport.OrganizeCodes;
using SIE.MES.DashBoard.KzReport.ProductionLineProcesss;
using SIE.MES.DashBoard.KzReport.ProductionProcesss;
using SIE.MES.DashBoard.KzReport.ProductionProcesss.Enums;
using SIE.ObjectModel;
using SIE.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport
{
    /// <summary>
    /// 
    /// </summary>
    public partial class KzReportController : DomainController
    {
        /// <summary>
        /// 获取下拉物料类型
        /// </summary>
        /// <returns></returns>
        [ApiService("获取下拉物料类型")]
        public virtual List<object> GetItemType()
        {
            List<object> list = new List<object>();
            list.Add(new { item = "铜" });
            list.Add(new { item = "铝" });
            list.Add(new { item = "全部" });
            return list;
            //List<string> itemTypes = new List<string>();
            //itemTypes.Add("铜");
            //itemTypes.Add("铝");
            //itemTypes.Add("全部");
            //return itemTypes;
        }

        /// <summary>
        /// 获取下拉部门
        /// </summary>
        /// <param name="productLine"></param>
        /// <returns></returns>
        [ApiService("获取下拉部门")]
        public virtual List<string> GetDepartments(string productLine)
        {
            var plantCodes = Query<OrganizeCode>().Where(p => p.ProductLine == productLine).Select(p => p.PlantCode).Distinct().ToList<string>().ToList();
            return plantCodes;
        }

        /// <summary>
        /// 获取下拉产品线
        /// </summary>
        /// <returns></returns>
        [ApiService("获取下拉产品线")]
        public virtual List<string> GetProductLine()
        {
            var productLines = Query<OrganizeCode>().Select(p => p.ProductLine).Distinct().ToList<string>().ToList();
            return productLines;
        }

        /// <summary>
        /// 获取下拉厂部名称
        /// </summary>
        /// <param name="productLine"></param>
        /// <returns></returns>
        [ApiService("获取下拉厂部名称")]
        [AllowAnonymous]
        public virtual object GetPlantName(string productLine)
        {
            RT.InvOrg = 1;
            var q = Query<OrganizeCode>();
            if (!productLine.IsNullOrEmpty())
                q.Where(p => p.ProductLine.Contains(productLine));
            var plantNames = q.Select(p => p.PlantName).Distinct().ToList<string>().ToList();
            return plantNames.Select(p => new { Name = p }).ToList();
        }

        /// <summary>
        /// 获取下拉工序
        /// </summary>
        /// <param name="productLine"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        [ApiService("获取下拉工序")]
        [AllowAnonymous]
        public virtual object GetProcesses(string productLine, string department)
        {
            RT.InvOrg = 1;
            var q = Query<OrganizeCode>();
            if (!productLine.IsNullOrEmpty())
                q.Where(p => p.ProductLine == productLine);
            if (!department.IsNullOrEmpty())
                q.Where(p => p.PlantName == department);

            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);

            List<string> factoryDatas = new List<string>();
            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                try
                {
                    var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                    var dic = organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).GroupBy(p => p.FactoryCode).ToDictionary(p => p.Key, p => p.Select(p => p.MrpController).ToList());

                    var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value = dic },
                                 }.ToArray();
                    var response = SmomControlHepler.SmomPost<List<string>>("KzReportController", "GetProcessesFactory", g.Key, smomParam);
                    factoryDatas.AddRange(response);

                }
                catch (Exception ex)
                {

                }
            }

            var list = factoryDatas.Distinct().Select(p => new { Name = p }).ToList();
            return list;
        }

        /// <summary>
        /// 获取近10年年份
        /// </summary>
        /// <param name="productLine"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        [ApiService("获取近10年年份")]
        [AllowAnonymous]
        public virtual object GetLast10Year()
        {
            var curYear = DateTime.Now.Year;
            var years = new List<int>();
            for (var i = 0; i <= 10; i++)
            {
                years.Add(curYear - i);
            }

            var list = years.Select(p => new { Name = p }).ToList();
            return list;
        }


        #region 投入产出日报表

        /// <summary>
        /// 投入产出日报表
        /// </summary>
        /// <param name="line"></param>
        /// <param name="department"></param>
        /// <param name="process"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [ApiService("投入产出日报表")]
        public virtual List<InputOutputDailyReportData> GetInputOutputDailyReportDatas(string line, string department, List<string> process, int year, string month)
        {
            if (year == 0)
                return new List<InputOutputDailyReportData>();

            var q = Query<OrganizeCode>();
            if (!line.IsNullOrEmpty())
                q.Where(p => p.ProductLine == line);
            if (!department.IsNullOrEmpty())
                q.Where(p => p.PlantName == department);

            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);

            List<InputOutputDailyReportData> factoryDatas = new List<InputOutputDailyReportData>();
            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {

                var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                //对产品线、园区和部门进行分组(按照前端显示),查出的数据就只会是相同产品线和部门，调用的接口中会对物料类型分组，下方会将相同产品线和部门进行合并计算
                foreach (var plang in organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).GroupBy(p => new { p.ParkName, p.ProductLine, p.PlantName }))
                {
                    try
                    {

                        var dic = plang.GroupBy(p => p.FactoryCode).ToDictionary(p => p.Key, p => p.Select(p => p.MrpController).ToList());
                        var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value = dic },
                    new SmomParam{ Value = process},
                    new SmomParam{ Value = year},
                    new SmomParam{ Value = month},
                                 }.ToArray();
                        var response = SmomControlHepler.SmomPost<List<InputOutputDailyReportData>>("KzReportController", "GetInputOutputDailyReportDatasFactory", g.Key, smomParam);
                        foreach (var item in response)
                        {
                            item.Department = plang.Key.PlantName;
                            item.ProductLine = plang.Key.ProductLine;
                            item.Park = plang.Key.ParkName;
                            item.Key = item.Department + "$" + item.ProductLine + "$" + item.Park + "$" + item.Process + "$" + item.Date;
                        }
                        factoryDatas.AddRange(response);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }


            List<InputOutputDailyReportData> datas = new List<InputOutputDailyReportData>();
            int index = 1;
            foreach (var factoryData in factoryDatas)
            {
                factoryData.Num = index;
                datas.Add(factoryData);
                index++;
            }
            return datas;
        }

        /// <summary>
        /// 投入产出日报表-明细
        /// </summary>
        /// <param name="line"></param>
        /// <param name="department"></param>
        /// <param name="process"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [ApiService("投入产出日报表明细")]
        public virtual List<InputOutputDailyReportDtlData> GetInputOutputDailyReportDtlDatas(int year, string key)
        {
            if (year == 0)
                return new List<InputOutputDailyReportDtlData>();

            //拆分
            var keys = key.Split("$");
            var line = keys[1];
            var department = keys[0];

            var q = Query<OrganizeCode>();
            if (!line.IsNullOrEmpty())
                q.Where(p => p.ProductLine == line);
            if (!department.IsNullOrEmpty())
                q.Where(p => p.PlantName == department);

            var process = new List<string>() { keys[3] };

            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);

            List<InputOutputDailyReportDtlData> factoryDatas = new List<InputOutputDailyReportDtlData>();
            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                //对产品线、园区和部门进行分组(按照前端显示),查出的数据就只会是相同产品线和部门，调用的接口中会对物料类型分组，下方会将相同产品线和部门进行合并计算
                foreach (var plang in organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).GroupBy(p => new { p.ParkName, p.ProductLine, p.PlantName }))
                {
                    try
                    {

                        var dic = plang.GroupBy(p => p.FactoryCode).ToDictionary(p => p.Key, p => p.Select(p => p.MrpController).ToList());
                        var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value = dic },
                    new SmomParam{ Value = process},
                    new SmomParam{ Value = year},
                    new SmomParam{ Value = keys[4]},
                                 }.ToArray();
                        var response = SmomControlHepler.SmomPost<List<InputOutputDailyReportDtlData>>("KzReportController", "GetInputOutputDailyReportDtlDatasFactory", g.Key, smomParam);
                        foreach (var item in response)
                        {
                            item.Department = plang.Key.PlantName;
                            item.ProductLine = plang.Key.ProductLine;
                            item.Park = plang.Key.ParkName;
                        }
                        factoryDatas.AddRange(response);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            List<InputOutputDailyReportDtlData> datas = new List<InputOutputDailyReportDtlData>();
            int index = 1;
            foreach (var factoryData in factoryDatas)
            {
                factoryData.Num = index;
                datas.Add(factoryData);
                index++;
            }
            return datas;
        }

        #endregion

        #region OEE

        /// <summary>
        /// OEE
        /// </summary>
        /// <param name="line"></param>
        /// <param name="department"></param>
        /// <param name="process"></param>
        /// <param name="resource"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [ApiService("OEE")]
        public virtual List<OeeData> GetOeeDatas(string line, string department, List<string> process, string resource, DateTime? beginTime, DateTime? endTime)
        {
            var q = Query<OrganizeCode>();
            if (!line.IsNullOrEmpty())
                q.Where(p => p.ProductLine == line);
            if (!department.IsNullOrEmpty())
                q.Where(p => p.PlantName == department);

            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);

            List<OeeData> factoryDatas = new List<OeeData>();
            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                //对产品线和部门进行分组(按照前端显示),查出的数据就只会是相同产品线和部门，调用的接口中会对物料类型分组，下方会将相同产品线和部门进行合并计算
                foreach (var plang in organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).GroupBy(p => new { p.ProductLine, p.PlantName }))
                {
                    try
                    {

                        var dic = plang.GroupBy(p => p.FactoryCode).ToDictionary(p => p.Key, p => p.Select(p => p.MrpController).ToList());
                        var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value = dic },
                    new SmomParam{ Value = process},
                    new SmomParam{ Value = resource},
                    new SmomParam{ Value = beginTime},
                    new SmomParam{ Value = endTime}
                                 }.ToArray();
                        var response = SmomControlHepler.SmomPost<List<OeeData>>("KzReportController", "GetOeeDatasFactory", g.Key, smomParam);
                        foreach (var item in response)
                        {
                            item.Department = plang.Key.PlantName;
                            item.ProductLine = plang.Key.ProductLine;
                        }
                        factoryDatas.AddRange(response);
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }

            List<OeeData> datas = new List<OeeData>();

            int index = 1;
            foreach (var factoryData in factoryDatas)
            {
                factoryData.Num = index;
                datas.Add(factoryData);
                index++;
            }

            return datas;
        }

        #endregion

        #region 生产效率报表

        /// <summary>
        /// 生产效率报表
        /// </summary>
        /// <param name="line"></param>
        /// <param name="department"></param>
        /// <param name="process"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [ApiService("生产效率报表")]
        public virtual List<ProductionEfficiencyData> GetProductionEfficiencyDatas(string line, string department, List<string> process, DateTime? beginTime, DateTime? endTime)
        {
            var q = Query<OrganizeCode>();
            if (!line.IsNullOrEmpty())
                q.Where(p => p.ProductLine == line);
            if (!department.IsNullOrEmpty())
                q.Where(p => p.PlantName == department);

            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);

            List<ProductionEfficiencyData> factoryDatas = new List<ProductionEfficiencyData>();

            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                //对产品线和部门进行分组(按照前端显示),查出的数据就只会是相同产品线和部门，调用的接口中会对物料类型分组，下方会将相同产品线和部门进行合并计算
                foreach (var plang in organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).GroupBy(p => new { p.ProductLine, p.PlantName }))
                {
                    try
                    {

                        var mrpControllers = plang.Select(p => p.MrpController).Distinct().ToList();
                        var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value = plang.Select(p=>p.FactoryCode).Distinct().ToList() },
                    new SmomParam { Value = mrpControllers },
                    new SmomParam{ Value = process},
                    new SmomParam{ Value = beginTime},
                    new SmomParam{ Value = endTime}
                                 }.ToArray();
                        var response = SmomControlHepler.SmomPost<List<ProductionEfficiencyData>>("KzReportController", "GetProductionEfficiencyDatasFactory", g.Key, smomParam);
                        foreach (var item in response)
                        {
                            item.Department = plang.Key.PlantName;
                            item.ProductLine = plang.Key.ProductLine;
                        }
                        factoryDatas.AddRange(response);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            List<ProductionEfficiencyData> datas = new List<ProductionEfficiencyData>();
            int index = 1;
            foreach (var factoryData in factoryDatas)
            {
                factoryData.Num = index;
                datas.Add(factoryData);
                index++;
            }

            return datas;
        }

        #endregion

        #region 安灯异常统计报表

        /// <summary>
        /// 安灯异常统计报表柱形图
        /// </summary>
        /// <param name="line"></param>
        /// <param name="factory"></param>
        /// <param name="resource"></param>
        /// <param name="andonName"></param>
        /// <param name="equipAccountCode"></param>
        /// <param name="state"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [ApiService("安灯异常统计报表柱形图")]
        public virtual List<AndonReportBarChartData> GetAndonReportBarChartDatas(string line, string factory, string resource, string andonName, string equipAccountCode, int? state, DateTime? beginTime, DateTime? endTime)
        {

            var q = Query<OrganizeCode>();
            if (!line.IsNullOrEmpty())
                q.Where(p => p.ProductLine == line);
            if (!factory.IsNullOrEmpty())
                q.Where(p => p.FactoryName == factory);
            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);

            List<AndonReportBarChartData> factoryDatas = new List<AndonReportBarChartData>();

            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                //找出相同产品线的
                foreach (var plang in organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).GroupBy(p => new { p.ProductLine }))
                {
                    try
                    {

                        var factoryCs = plang.Select(p => p.FactoryCode).Distinct().ToList();
                        var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value = factoryCs },
                    new SmomParam { Value = resource },
                    new SmomParam{ Value = andonName  },
                    new SmomParam{ Value = equipAccountCode  },
                    new SmomParam{ Value = state  },
                    new SmomParam{ Value = beginTime},
                    new SmomParam{ Value = endTime}
                                 }.ToArray();
                        var response = SmomControlHepler.SmomPost<List<AndonReportBarChartData>>("KzReportController", "GetAndonReportBarChartDatasFactory", g.Key, smomParam);
                        foreach (var r in response)
                        {
                            r.ProductLine = plang.Key.ProductLine;
                            factoryDatas.Add(r);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            List<AndonReportBarChartData> datas = new List<AndonReportBarChartData>();

            foreach (var g in factoryDatas.GroupBy(p => p.ProductLine))
            {
                AndonReportBarChartData data = new AndonReportBarChartData();

                data.ProductLine = g.Key;
                data.Standby = g.Sum(p => p.Standby);
                data.Processing = g.Sum(p => p.Processing);
                data.ToAccepted = g.Sum(p => p.ToAccepted);

                datas.Add(data);
            }

            return datas;
        }

        /// <summary>
        /// 获取状态下拉
        /// </summary>
        /// <returns></returns>
        [ApiService("获取状态下拉")]
        public virtual List<AndonReportStateData> GetAndonReportStateDatas()
        {
            List<AndonReportStateData> datas = new List<AndonReportStateData>();

            foreach (AndonManageState type in Enum.GetValues(typeof(AndonManageState)))
            {
                AndonReportStateData data = new AndonReportStateData();

                data.Value = (int)type;
                data.Key = type.ToLabel();

                datas.Add(data);
            }

            return datas;
        }

        /// <summary>
        /// 安灯异常统计报表
        /// </summary>
        /// <param name="line"></param>
        /// <param name="factory"></param>
        /// <param name="resource"></param>
        /// <param name="andonName"></param>
        /// <param name="EquipAccountCode"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [ApiService("安灯异常统计报表")]
        public virtual List<AndonReportData> GetAndonReportDatas(string line, string factory, string resource, string andonName, string equipAccountCode, int? state, DateTime? beginTime, DateTime? endTime)
        {
            List<AndonReportData> datas = new List<AndonReportData>();

            var q = Query<OrganizeCode>();
            if (!line.IsNullOrEmpty())
                q.Where(p => p.ProductLine == line);
            if (!factory.IsNullOrEmpty())
                q.Where(p => p.FactoryName == factory);
            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);

            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                //找出相同产品线的
                foreach (var plang in organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).GroupBy(p => new { p.ProductLine, p.FactoryName }))
                {
                    try
                    {

                        var factoryCs = plang.Select(p => p.FactoryCode).Distinct().ToList();
                        var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value = factoryCs },
                    new SmomParam { Value = resource },
                    new SmomParam{ Value = andonName  },
                    new SmomParam{ Value = equipAccountCode  },
                    new SmomParam{ Value = state  },
                    new SmomParam{ Value = beginTime},
                    new SmomParam{ Value = endTime}
                                 }.ToArray();
                        var response = SmomControlHepler.SmomPost<List<AndonReportData>>("KzReportController", "GetAndonReportDatasFactory", g.Key, smomParam);
                        foreach (var r in response)
                        {
                            r.ProductLine = plang.Key.ProductLine;
                            r.Factory = plang.Key.FactoryName;
                            datas.Add(r);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            int index = 1;
            foreach (var data in datas)
            {
                data.Num = index;
                index++;
            }
            return datas;
        }

        #endregion

        #region 可疑品处理报表

        /// <summary>
        /// 可疑品处理报表
        /// </summary>
        /// <param name="line"></param>
        /// <param name="department"></param>
        /// <param name="process"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [ApiService("可疑品处理报表")]
        public virtual List<SuspectReportData> GetSuspectReportDatas(string line, string department, List<string> process, DateTime? beginTime, DateTime? endTime)
        {
            List<SuspectReportData> datas = new List<SuspectReportData>();

            var q = Query<OrganizeCode>();
            if (!line.IsNullOrEmpty())
                q.Where(p => p.ProductLine == line);
            if (!department.IsNullOrEmpty())
                q.Where(p => p.PlantName == department);

            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);

            List<SuspectReportData> factoryDatas = new List<SuspectReportData>();

            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                //对产品线和部门进行分组(按照前端显示),查出的数据就只会是相同产品线和部门，调用的接口中会对物料类型分组，下方会将相同产品线和部门进行合并计算
                foreach (var plang in organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).GroupBy(p => new { p.ProductLine, p.PlantName }))
                {
                    try
                    {

                        var mrpControllers = plang.Select(p => p.MrpController).Distinct().ToList();
                        var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value = plang.Select(p=>p.FactoryCode).Distinct().ToList() },
                    new SmomParam { Value = mrpControllers },
                    new SmomParam{ Value = process  },
                    new SmomParam{ Value = beginTime},
                    new SmomParam{ Value = endTime}
                                 }.ToArray();
                        var response = SmomControlHepler.SmomPost<List<SuspectReportData>>("KzReportController", "GetSuspectReportDatasFactory", g.Key, smomParam);
                        foreach (var r in response)
                        {
                            r.ProductLine = plang.Key.ProductLine;
                            r.Department = plang.Key.PlantName;
                            factoryDatas.Add(r);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            int index = 1;
            foreach (var g in factoryDatas.GroupBy(p => new { p.ProductLine, p.Department, p.Process }))
            {
                SuspectReportData data = new SuspectReportData();
                data.Num = index;
                data.ProductLine = g.Key.ProductLine;
                data.Department = g.Key.Department;
                data.Process = g.Key.Process;
                data.TotalQty = g.Sum(p => p.TotalQty) / 10000;
                data.TotalSuspectQty = g.Sum(p => p.TotalSuspectQty) / 10000;
                data.TotalNgQty = g.Sum(p => p.TotalNgQty) / 10000;
                data.NgQtyRate = data.TotalQty == 0 ? 0 : Math.Round((data.TotalNgQty * 100) / data.TotalQty, 4);
                data.SuspectRate = data.TotalQty == 0 ? 0 : Math.Round((data.TotalSuspectQty * 100) / data.TotalQty, 4);
                data.OkRate = data.TotalQty == 0 ? 0 : Math.Round((1 - (data.TotalSuspectQty / data.TotalQty)) * 100, 4);
                datas.Add(data);
                index++;
            }
            return datas;
        }

        /// <summary>
        /// 缺陷报表
        /// </summary>
        /// <param name="line"></param>
        /// <param name="department"></param>
        /// <param name="process"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [ApiService("缺陷报表")]
        public virtual List<SuspectDefectData> GetSuspectDefectDatas(string line, string department, List<string> process, DateTime? beginTime, DateTime? endTime)
        {
            var q = Query<OrganizeCode>();
            if (!line.IsNullOrEmpty())
                q.Where(p => p.ProductLine == line);
            if (!department.IsNullOrEmpty())
                q.Where(p => p.PlantName == department);

            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);

            List<SuspectDefectData> factoryDatas = new List<SuspectDefectData>();

            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                //对产品线和部门进行分组(按照前端显示),查出的数据就只会是相同产品线和部门，调用的接口中会对物料类型分组，下方会将相同产品线和部门进行合并计算
                foreach (var plang in organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).GroupBy(p => new { p.ProductLine, p.PlantName }))
                {
                    try
                    {

                        var mrpControllers = plang.Select(p => p.MrpController).Distinct().ToList();
                        var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value = plang.Select(p=>p.FactoryCode).Distinct().ToList() },
                    new SmomParam { Value = mrpControllers },
                    new SmomParam{ Value = process  },
                    new SmomParam{ Value = beginTime},
                    new SmomParam{ Value = endTime}
                                 }.ToArray();
                        var response = SmomControlHepler.SmomPost<List<SuspectDefectData>>("KzReportController", "GetSuspectDefectDatasFactory", g.Key, smomParam);
                        foreach (var r in response)
                        {
                            factoryDatas.Add(r);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }

            List<SuspectDefectData> datas = new List<SuspectDefectData>();
            var total = factoryDatas.Sum(p => p.Qty);
            //相同缺陷合并分组
            foreach (var g in factoryDatas.GroupBy(p => new { p.DefectCode, p.DefectName }))
            {
                SuspectDefectData data = new SuspectDefectData();
                data.DefectCode = g.Key.DefectCode;
                data.DefectName = g.Key.DefectName;
                data.Qty = g.Sum(p => p.Qty);
                data.Rate = total == 0 ? 0 : Math.Round((data.Qty * 100) / total, 4);
                datas.Add(data);
            }
            int index = 1;
            foreach (var item in datas.OrderByDescending(p => p.Qty))
            {
                item.Num = index;
                index++;
            }
            //从多到少排序
            datas = datas.OrderByDescending(p => p.Qty).ToList();
            return datas;
        }


        #endregion

        #region 产品直通率报表

        /// <summary>
        /// 产品直通率报表
        /// </summary>
        /// <param name="line"></param>
        /// <param name="department"></param>
        /// <param name="product"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [ApiService("产品直通率报表")]
        public virtual List<ProductFirstPassYieldData> GetProductFirstPassYieldDatas(string line, string department, string product, DateTime? beginTime, DateTime? endTime)
        {

            var q = Query<OrganizeCode>();
            if (!line.IsNullOrEmpty())
                q.Where(p => p.ProductLine == line);
            if (!department.IsNullOrEmpty())
                q.Where(p => p.PlantName == department);

            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);

            List<ProductFirstPassYieldData> factoryDatas = new List<ProductFirstPassYieldData>();
            Dictionary<string, List<ProductFirstPassYieldFactoryData>> dic = new Dictionary<string, List<ProductFirstPassYieldFactoryData>>();
            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                //对产品线和部门进行分组(按照前端显示),查出的数据就只会是相同产品线和部门，调用的接口中会对物料类型分组，下方会将相同产品线和部门进行合并计算
                foreach (var plang in organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).GroupBy(p => new { p.ProductLine, p.PlantName }))
                {
                    try
                    {

                        var mrpControllers = plang.Select(p => p.MrpController).Distinct().ToList();
                        var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value = plang.Select(p=>p.FactoryCode).Distinct().ToList() },
                    new SmomParam { Value = mrpControllers },
                    new SmomParam{ Value = product  },
                    new SmomParam{ Value = beginTime},
                    new SmomParam{ Value = endTime}
                                 }.ToArray();
                        var response = SmomControlHepler.SmomPost<List<ProductFirstPassYieldFactoryData>>("KzReportController", "GetProductFirstPassYieldDatasFactory", g.Key, smomParam);
                        foreach (var item in response.GroupBy(p => p.Product))
                        {
                            factoryDatas.Add(new ProductFirstPassYieldData()
                            {
                                Department = plang.Key.PlantName,
                                ProductLine = plang.Key.ProductLine,
                                ProductCode = item.Key
                            });
                            //将数据存起来 ，后续用于计算直通率,按照产品线+部门+产品去分组
                            if (dic.ContainsKey(plang.Key.PlantName + "-" + plang.Key.ProductLine + "-" + item.Key))
                            {
                                dic[plang.Key.PlantName + "-" + plang.Key.ProductLine + "-" + item.Key].AddRange(item.ToList());
                            }
                            else
                            {
                                dic.Add(plang.Key.PlantName + "-" + plang.Key.ProductLine + "-" + item.Key, item.ToList());
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }

            List<ProductFirstPassYieldData> datas = new List<ProductFirstPassYieldData>();

            int index = 1;
            foreach (var g in factoryDatas.GroupBy(p => new { p.Department, p.ProductLine, p.ProductCode }))
            {
                ProductFirstPassYieldData data = new ProductFirstPassYieldData();
                data.Num = index;
                data.ProductLine = g.Key.ProductLine;
                data.Department = g.Key.Department;
                data.ProductCode = g.Key.ProductCode;
                data.FirstPassYield = 0;
                if (dic.ContainsKey(g.Key.Department + "-" + g.Key.ProductLine + "-" + g.Key.ProductCode))
                {
                    var list = dic[g.Key.Department + "-" + g.Key.ProductLine + "-" + g.Key.ProductCode];
                    if (list.Count > 0)
                    {
                        data.FirstPassYield = 1;
                        //找到相同工序，然后合并他们的数量，计算每个工序的直通率，然后相乘得到主表的直通率
                        foreach (var l in list.SelectMany(p => p.datas).GroupBy(p => p.Process))
                        {
                            data.FirstPassYield *= (l.Sum(p => p.FeedingQty) == 0 ? 1 : 1 - (l.Sum(p => p.SuspectQty) / l.Sum(p => p.FeedingQty)));
                        }
                    }
                }

                datas.Add(data);
                index++;
            }

            return datas;
        }

        /// <summary>
        /// 产品直通率报表-明细
        /// </summary>
        /// <param name="line"></param>
        /// <param name="department"></param>
        /// <param name="product"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [ApiService("产品直通率报表明细")]
        public virtual List<ProductFirstPassYieldDtlData> GetProductFirstPassYieldDtlDatas(string productLine, string department, string productCode, DateTime? beginTime, DateTime? endTime)
        {
            var q = Query<OrganizeCode>();
            q.Where(p => p.ProductLine == productLine);
            q.Where(p => p.PlantName == department);
            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);
            List<ProductFirstPassYieldDtlData> factoryDatas = new List<ProductFirstPassYieldDtlData>();

            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                try
                {
                    var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                    var mrpControllers = organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).Select(p => p.MrpController).Distinct().ToList();
                    var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value = fCs },
                    new SmomParam { Value = mrpControllers },
                    new SmomParam{ Value = productCode.IsNullOrEmpty()?new List<string>(): new List<string>(){ productCode } },
                    new SmomParam{ Value = beginTime},
                    new SmomParam{ Value = endTime}
                                 }.ToArray();
                    var response = SmomControlHepler.SmomPost<List<ProductFirstPassYieldDtlData>>("KzReportController", "GetProductFirstPassYieldDtlDatasFactory", g.Key, smomParam);
                    factoryDatas.AddRange(response);
                }
                catch (Exception ex)
                {

                }
            }

            List<ProductFirstPassYieldDtlData> datas = new List<ProductFirstPassYieldDtlData>();

            var index = 1;
            foreach (var g in factoryDatas.GroupBy(p => p.Process))
            {
                ProductFirstPassYieldDtlData data = new ProductFirstPassYieldDtlData();
                data.Num = index;
                data.Process = g.Key;
                data.SuspectQty = g.Sum(p => p.SuspectQty);
                data.FeedingQty = g.Sum(p => p.FeedingQty);
                data.FirstPassYield = data.FeedingQty == 0 ? 1 : 1 - (data.SuspectQty / data.FeedingQty);
                datas.Add(data);
                index++;
            }

            return datas;
        }

        #endregion

        #region 物料平衡报表

        /// <summary>
        /// 物料平衡报表
        /// </summary>
        /// <param name="line">产品线</param>
        /// <param name="department">部门</param>
        /// <param name="workShopt">车间</param>
        /// <param name="itemType">物料类型</param>
        /// <param name="beginTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns></returns>
        [ApiService("物料平衡报表")]
        public virtual List<ItemBalanceData> GetItemBalanceDatas(string line, string department, string itemType, DateTime? beginTime, DateTime? endTime)
        {
            List<ItemBalanceData> factoryDatas = new List<ItemBalanceData>();

            var q = Query<OrganizeCode>();
            if (!line.IsNullOrEmpty())
                q.Where(p => p.ProductLine == line);
            if (!department.IsNullOrEmpty())
                q.Where(p => p.PlantName == department);

            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);
            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                try
                {
                    var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                    //对产品线和部门进行分组(按照前端显示),查出的数据就只会是相同产品线和部门，调用的接口中会对物料类型分组，下方会将相同产品线和部门进行合并计算
                    foreach (var plang in organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).GroupBy(p => new { p.ProductLine, p.PlantName }))
                    {
                        var mrpControllers = plang.Select(p => p.MrpController).Distinct().ToList();
                        var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value = fCs },
                    new SmomParam { Value = mrpControllers },
                    new SmomParam{ Value = itemType},
                    new SmomParam{ Value = beginTime},
                    new SmomParam{ Value = endTime}
                                 }.ToArray();
                        var response = SmomControlHepler.SmomPost<List<ItemBalanceData>>("KzReportController", "GetItemBalanceDatasFactory", g.Key, smomParam);
                        foreach (var item in response)
                        {
                            item.Department = plang.Key.PlantName;
                            item.ProductLine = plang.Key.ProductLine;
                        }
                        factoryDatas.AddRange(response);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            if (factoryDatas.Count < 1)
                return new List<ItemBalanceData>();

            List<ItemBalanceData> datas = new List<ItemBalanceData>();

            if (!itemType.IsNullOrEmpty() && itemType != "全部")
                factoryDatas = factoryDatas.Where(p => p.ItemType == itemType).ToList();

            var dic = factoryDatas.GroupBy(p => new { p.ProductLine, p.Department }).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var d in dic)
            {
                var oC = organizeCodes.Where(p => p.ProductLine == d.Key.ProductLine && p.PlantName == d.Key.Department).FirstOrDefault();

                foreach (var item in d.Value)
                {
                    ItemBalanceData data = new ItemBalanceData();
                    data.ProductLine = oC.ProductLine;
                    data.Department = oC.PlantName;
                    //data.FactoryCode = d.Key.FactoryCode;
                    //data.WorkShopCode = d.Key.WorkShopCode;
                    data.ItemType = item.ItemType;
                    data.FeedingQty = item.FeedingQty;
                    data.ProductQty = item.ProductQty;
                    data.RemainingQty = item.RemainingQty;
                    data.OutputProductQty = item.OutputProductQty;
                    data.DiffQty = data.FeedingQty - data.ProductQty - data.OutputProductQty - data.RemainingQty;
                    data.Rate = data.FeedingQty == 0 ? 0 : Math.Round(data.DiffQty / data.FeedingQty * 100, 2);

                    datas.Add(data);
                }

            }

            return datas;

            #region 

            //            List<ItemBalanceData> datas = new List<ItemBalanceData>();

            //            string query = " 1 = 1";
            //            if (!line.IsNullOrEmpty())
            //                query += $" and t0.Product_Line = '{line}'";
            //            if (!department.IsNullOrEmpty())
            //                query += $" and t0.Plant_Code = '{department}'";
            //            if (beginTime != null)
            //                query += $" and t1.report_time >= to_date('{beginTime}','yyyy-mm-dd hh24:mi:ss')";
            //            if (endTime != null)
            //                query += $" and t1.report_time <= to_date('{endTime}','yyyy-mm-dd hh24:mi:ss')";
            //            if (!itemType.IsNullOrEmpty() && itemType != "全部")
            //                query += $" and t1.Product_Name like '%{itemType}%'";

            //            string sql = $@"
            //                        with 
            //can1 as ( SELECT T0.Product_Line,T0.Plant_Name,t1.Wo,t1.Factory,
            //    CASE
            //        WHEN t1.Product_Name LIKE '%铜%' THEN '铜'
            //        WHEN t1.Product_Name LIKE '%铝%' THEN '铝'
            //        ELSE NULL
            //    END as item_type
            //FROM ORGANIZE_CODE T0
            //inner join FACTORY_REPORT_RECORD_V t1 on t1.Work_Shop_Code = t0.mrp_controller
            //WHERE T0.IS_PHANTOM = 0 and {query} 
            //GROUP BY T0.Product_Line,T0.Plant_Name,t1.Wo,t1.Factory,
            //    CASE
            //        WHEN t1.Product_Name LIKE '%铜%' THEN '铜'
            //        WHEN t1.Product_Name LIKE '%铝%' THEN '铝'
            //        ELSE NULL
            //    END
            //    ), 
            //can2 as (                               --获取取样净重详情,计算产出用量
            //select 
            //(select t1.Finish_Qty * t1.Weight from FAC_WEIGHT_OF_SAMP_REPORT_V t1 where t1.Wo_No = t0.Wo and t1.Factory = t0.Factory and rownum = 1) ProductQty,t0.wo
            //,t0.Factory,
            //    CASE
            //        WHEN t1.Product_Name LIKE '%铜%' THEN '铜'
            //        WHEN t1.Product_Name LIKE '%铝%' THEN '铝'
            //        ELSE NULL
            //    END as item_type
            //from can1 t0
            //inner join FAC_WEIGHT_OF_SAMP_REPORT_V t1 on  t1.Wo_No = t0.Wo and t1.Factory = t0.Factory
            //group by t0.wo,t0.factory,
            //    CASE
            //        WHEN t1.Product_Name LIKE '%铜%' THEN '铜'
            //        WHEN t1.Product_Name LIKE '%铝%' THEN '铝'
            //        ELSE NULL
            //    END
            //),
            //can3 as                      --工单BOM,计算产出用量
            //(
            //select
            //         SUM(
            //        CASE 
            //            WHEN t1.Bwart = '261' THEN t1.Single_Qty  -- 261的A2正常累加
            //            WHEN t1.Bwart = '531' THEN -t1.Single_Qty -- 531的A2按负数累加（等价于相减）
            //            ELSE 0                   -- 其他A1值不参与计算
            //        END
            //    ) AS ProductQty,t0.wo,t0.Factory,
            //        CASE
            //        WHEN t1.item_Name LIKE '%铜%' THEN '铜'
            //        WHEN t1.item_Name LIKE '%铝%' THEN '铝'
            //        ELSE NULL
            //    END as item_type
            //from can1 t0
            //inner join FACTORY_WO_BOM_V t1 on t1.Wo_no = t0.wo and t1.Factory = t0.Factory
            //where not exists(select 1 from FAC_WEIGHT_OF_SAMP_REPORT_V t1 where t1.Wo_No = t0.Wo and t1.Factory = t0.Factory)
            //group by t0.wo,t0.Factory,
            //    CASE
            //        WHEN t1.item_Name LIKE '%铜%' THEN '铜'
            //        WHEN t1.item_Name LIKE '%铝%' THEN '铝'
            //        ELSE NULL
            //    END
            //),
            //can4 as      --计算出投料量,计算余料量
            //(
            //     select sum(nvl(v1.Feeding_Qty,0)) feedingQty,sum(nvl(v1.Remaining_Qty,0)) Remaining_Qty,t1.wo,t1.Factory,
            //         CASE
            //        WHEN v1.item_Name LIKE '%铜%' THEN '铜'
            //        WHEN v1.item_Name LIKE '%铝%' THEN '铝'
            //        ELSE NULL
            //    END as item_type
            //     from FACTORY_FEEDING_RECORD_V v1
            //     inner join can1 t1 on v1.wo_no = t1.wo and v1.factory = t1.Factory
            //     group by t1.wo,t1.Factory,
            //         -- GROUP BY需和SELECT的CASE判断完全一致
            //    CASE
            //        WHEN v1.item_Name LIKE '%铜%' THEN '铜'
            //        WHEN v1.item_Name LIKE '%铝%' THEN '铝'
            //        ELSE NULL
            //    END
            //),
            //can5 as                 --联/副产品入库
            //(
            //     select sum(nvl(v1.qty,0)) Output_Product_Qty,t1.wo,t1.Factory,
            //              CASE
            //        WHEN v1.item_Name LIKE '%铜%' THEN '铜'
            //        WHEN v1.item_Name LIKE '%铝%' THEN '铝'
            //        ELSE NULL
            //    END as item_type
            //     from FACTORY_OUTPUT_PRO_REC_V v1
            //     inner join can1 t1 on v1.Work_Order_No = t1.Wo and t1.Factory = v1.factory
            //     group by          t1.wo,t1.Factory,
            //     CASE
            //        WHEN v1.item_Name LIKE '%铜%' THEN '铜'
            //        WHEN v1.item_Name LIKE '%铝%' THEN '铝'
            //        ELSE NULL
            //    END
            //)
            //select T1.Product_Line,T1.Plant_Name,t1.item_type,t1.wo,t2.ProductQty ProductQty2,t3.ProductQty ProductQty3,t4.feedingQty,t4.Remaining_Qty,t5.Output_Product_Qty,t1.factory
            //from can1 t1
            //left join can2 t2 on t2.wo = t1.wo and t2.item_type = t1.item_type and t2.factory = t1.factory
            //left join can3 t3 on t3.wo = t1.wo and t3.item_type = t1.item_type and t3.factory = t1.factory
            //left join can4 t4 on t4.wo = t1.wo and t4.item_type = t1.item_type and t4.fasctory = t1.factory
            //left join can5 t5 on t5.wo = t1.wo and t5.item_type = t1.item_type and t5.factory = t1.factory
            //                        ";
            //            List<ItemBalanceData> list = new List<ItemBalanceData>();
            //            using (var db = DB.Create("MES"))
            //            {
            //                try
            //                {
            //                    var dt = db.ExecuteDataTable(sql, CommandType.Text);
            //                    foreach (DataRow row in dt.Rows)
            //                    {
            //                        var productLine = row["Product_Line"].ToString();
            //                        var plantName = row["Plant_Name"].ToString();
            //                        var iType = row["item_type"].ToString();
            //                        var productQty2 = row["ProductQty2"].ToString();
            //                        var productQty3 = row["ProductQty3"].ToString();
            //                        var feedingQty = row["feedingQty"].ToString();
            //                        var remainingQty = row["Remaining_Qty"].ToString();
            //                        var outputProductQty = row["Output_Product_Qty"].ToString();
            //                        var factory = row["factory"].ToString();
            //                        ItemBalanceData data = new ItemBalanceData();

            //                        data.ProductLine = productLine;
            //                        data.Department = plantName;
            //                        data.ItemType = iType;
            //                        decimal productQty = 0;
            //                        if (!productQty2.IsNullOrEmpty())
            //                            productQty = Convert.ToDecimal(productQty2);
            //                        else if (!productQty3.IsNullOrEmpty())
            //                            productQty = Convert.ToDecimal(productQty3);
            //                        data.ProductQty = productQty;
            //                        data.FeedingQty = feedingQty.IsNullOrEmpty() ? 0 : Convert.ToDecimal(feedingQty);
            //                        data.RemainingQty = remainingQty.IsNullOrEmpty() ? 0 : Convert.ToDecimal(remainingQty);
            //                        data.OutputProductQty = outputProductQty.IsNullOrEmpty() ? 0 : Convert.ToDecimal(outputProductQty);
            //                        data.DiffQty = data.FeedingQty - data.ProductQty - data.OutputProductQty - data.RemainingQty;
            //                        data.Rate = data.FeedingQty == 0 ? 0 : Math.Round(data.DiffQty / data.FeedingQty * 100, 2);
            //                        list.Add(data);
            //                    }
            //                }
            //                catch (Exception e)
            //                {
            //                    throw new ValidationException(e.GetBaseException().Message);
            //                }
            //            }

            //            var dic = list.GroupBy(p => new { p.ProductLine, p.Department, p.ItemType }).ToDictionary(p => p.Key, p => p.ToList());
            //            foreach (var d in dic)
            //            {
            //                ItemBalanceData data = new ItemBalanceData();

            //                data.ProductLine = d.Key.ProductLine;
            //                data.Department = d.Key.Department;
            //                data.ItemType = d.Key.ItemType;
            //                data.ProductQty = d.Value.Sum(p => p.ProductQty);
            //                data.FeedingQty = d.Value.Sum(p => p.FeedingQty);
            //                data.RemainingQty = d.Value.Sum(p => p.RemainingQty);
            //                data.OutputProductQty = d.Value.Sum(p => p.OutputProductQty);
            //                data.DiffQty = data.FeedingQty - data.ProductQty - data.OutputProductQty - data.RemainingQty;
            //                data.Rate = data.FeedingQty == 0 ? 0 : Math.Round(data.DiffQty / data.FeedingQty * 100, 2);

            //                datas.Add(data);
            //            }

            //            return datas;
            #endregion

            #region 旧逻辑

            //var organizeCodes = Query<OrganizeCode>().WhereIf(!line.IsNullOrEmpty(), p => p.ProductLine == line).WhereIf(!department.IsNullOrEmpty(), p => p.PlantCode == department).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            //List<string> itemTypes = new List<string>();
            //if (itemType.IsNullOrEmpty())
            //{
            //    //对物料类型进行分组
            //    itemTypes.Add("铜");
            //    itemTypes.Add("铝");
            //    itemTypes.Add("");
            //}
            //else
            //{
            //    itemTypes.Add(itemType);
            //}

            ////var mrpControllers = organizeCodes.Select(p => p.MrpController).Distinct().ToList();

            //List<ItemBalanceData> datas = new List<ItemBalanceData>();
            ////根据产品线+厂区进行分组(下面还会对物料类型进一步分组)
            //foreach (var group in organizeCodes.GroupBy(p => new { p.ProductLine, p.PlantCode }))
            //{
            //    //获取分组后的车间(即MRB控制者)
            //    var mcs = group.Select(p => p.MrpController).Distinct().ToList();
            //    EntityList<FactoryReportRecord> reportRecords = new EntityList<FactoryReportRecord>();
            //    var pageSize = 50000;
            //    var pageNumber = 1;
            //    PagingInfo pagingInfo = new PagingInfo(pageNumber, pageSize);
            //    //var records = query.ToList(pagingInfo);
            //    //reportRecords.AddRange(records);
            //    //while (records.Count == pageSize)
            //    //{
            //    //    pagingInfo.PageNumber += 1;
            //    //    records = query.ToList(pagingInfo);
            //    //    reportRecords.AddRange(records);
            //    //}

            //    ////获取分组后的报工记录
            //    //var gRrs = reportRecords.ToList();

            //    //记录那些按照BOM去计算的工单，他们需要按照物料类型再去算一次
            //    List<string> specialWoNos = new List<string>();
            //    foreach (var it in itemTypes)
            //    {
            //        var query = Query<FactoryReportRecord>().Where(p => mcs.Contains(p.WorkShopCode));
            //        if (beginTime != null)
            //            query.Where(p => p.ReportTime >= beginTime);
            //        if (endTime != null)
            //            query.Where(p => p.ReportTime <= endTime);
            //        //if (!itemType.IsNullOrEmpty() && itemType != "全部")
            //        //    query.Where(p => p.ProductName.Contains("%" + itemType + "%"));

            //        var woNos = new List<string>();
            //        if (it != "")
            //        {
            //            woNos = query.Where(p => !p.ProductName.Contains("%铜%") && !p.ProductName.Contains("%铝%")).Select(p => p.Wo).Distinct().ToList<string>().ToList();
            //        }
            //        else
            //        {
            //            woNos = query.Where(p => p.ProductName.Contains("%"+it+"%")).Select(p => p.Wo).Distinct().ToList<string>().ToList();
            //        }
            //        if (specialWoNos.Count > 0)
            //        {
            //            woNos.AddRange(specialWoNos);
            //            woNos = woNos.Distinct().ToList();
            //        }
            //        //用工单号去找上料记录,计算出上料数量
            //        decimal feedingQty = 0;
            //        woNos.SplitDataExecute(temp => {
            //            feedingQty += Query<FactoryFeedingRecord>().Where(p => temp.Contains(p.WoNo)).Select(p => (decimal)p.FeedingQty.SUM()).FirstOrDefault<decimal>();
            //        });

            //        //计算产出用量
            //        //获取取样净重详情
            //        decimal ProductQty = 0;
            //        var weightOfSamplingReports = woNos.SplitContains(temp => {
            //            return Query<FactoryWeightOfSamplingReport>().Where(p => temp.Contains(p.WoNo)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //        });

            //        var boms = woNos.SplitContains(temp =>
            //         {
            //             return Query<FactoryWorkOrderBom>().Where(p => temp.Contains(p.WoNo) && (p.Bwart == "261" || p.Bwart == "531")).WhereIf(it == "", p => !p.ItemName.Contains("%铜%") && !p.ItemName.Contains("%铝%")).WhereIf(it != "", p => p.ItemName.Contains("%" + it + "%")).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //         });

            //        foreach (var woNo in woNos)
            //        {
            //            //var gRr = gRrs.FirstOrDefault(p => p.Wo == woNo);
            //            //判断是否存在在取样净重中，如果存在就直接用取样净重去算，如果不存在就要找出BOM再按照物料类型去计算
            //            var weightOfSamplingReport = weightOfSamplingReports.FirstOrDefault(p => p.WoNo == woNo);
            //            if (weightOfSamplingReport != null)
            //            {
            //                ProductQty += weightOfSamplingReport.FinishQty * weightOfSamplingReport.Weight;
            //            }
            //            else
            //            {
            //                //此处记得区分工厂，不同工厂可能存在相同工单，防止数量叠加计算
            //                var woBoms = boms.Where(p => /*p.Factory == gRr.Factory && */p.WoNo == woNo && (p.Bwart == "261" || p.Bwart == "531")).ToList();
            //                //用261的单位耗用量-531的单位耗用量
            //                ProductQty += (woBoms.Where(p => p.Bwart == "261").Sum(p => p.SingleQty) - woBoms.Where(p => p.Bwart == "531").Sum(p => p.SingleQty)) * (woBoms.FirstOrDefault()?.FinishQty ?? 0);

            //                //记录下工单，等一下需要循环再去计算
            //                specialWoNos.Add(woNo);
            //            }
            //        }
            //        //联/副产品入库
            //        decimal OutputProductQty = 0;
            //        woNos.SplitDataExecute(temp => {
            //            OutputProductQty += Query<FactoryOutputProductRecord>().WhereIf(it == "", p => !p.ItemName.Contains("%铜%") && !p.ItemName.Contains("%铝%")).WhereIf(it != "", p => p.ItemName.Contains("%" + it + "%")).Where(p => temp.Contains(p.WorkOrderNo)).Select(p => p.Qty.SUM()).FirstOrDefault<decimal>();
            //        });
            //        //计算余料量
            //        decimal RemainingQty = 0;
            //        //用工单号去找上料记录,计算出上料数量
            //        woNos.SplitDataExecute(temp => {
            //            RemainingQty += Query<FactoryFeedingRecord>().Where(p => temp.Contains(p.WoNo)).Select(p => (decimal)p.RemainingQty.SUM()).FirstOrDefault<decimal>();
            //        });

            //        ItemBalanceData data = new ItemBalanceData();
            //        data.ProductLine = group.Key.ProductLine;
            //        data.Department = group.Key.PlantCode;
            //        data.ItemType = it;
            //        data.FeedingQty = feedingQty;
            //        data.ProductQty = ProductQty;
            //        data.OutputProductQty = OutputProductQty;
            //        data.RemainingQty = RemainingQty;
            //        data.DiffQty = data.FeedingQty - data.ProductQty - data.OutputProductQty - data.RemainingQty;
            //        data.Rate = data.FeedingQty == 0 ? 0 : Math.Round(data.DiffQty / data.FeedingQty * 100, 2);
            //        datas.Add(data);
            //    }
            //}
            //return datas;
            #endregion
        }

        #endregion

        #region 生产达成率报表
        /// <summary>
        /// 生产达成率报表
        /// </summary>
        /// <param name="rateData"></param>
        /// <returns></returns>
        [ApiService("生产达成率报表")]
        public virtual List<ProductionAchievementRateData> ProductionAchievementRate(RequestProductionAchievementRateData rateData)
        {
            //获取库存组织下的工序
            var dicInvCodeProcessCode = RT.Service.Resolve<ProductionLineProcessController>().GetInvCodeProcessCode(rateData.ProductLine, rateData.PlantName, rateData.ProcessCodes == null || rateData.ProcessCodes.Count == 0 ? null : rateData.ProcessCodes);
            //获取Mrp
            var mrpDics = RT.Service.Resolve<OrganizeCodeController>().GetMrps(rateData.ProductLine, rateData.PlantName);

            //var setting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingUrl();
            //请求的url
            var SmomControlSettings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(mrpDics.Select(p => p.DicKey).ToList());
            var factoryUrls = SmomControlSettings.Select(p => p.FactoryUrl).Distinct().ToList();

            var q = Query<OrganizeCode>();
            if (!rateData.ProductLine.IsNullOrEmpty())
                q.Where(p => p.ProductLine == rateData.ProductLine);
            if (!rateData.PlantName.IsNullOrEmpty())
                q.Where(p => p.PlantName == rateData.PlantName);

            var list = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            List<Task<List<ProductionAchievementRateData>>> tasks = new List<Task<List<ProductionAchievementRateData>>>();

            List<ProductionAchievementRateData> datas = new List<ProductionAchievementRateData>();
            foreach (var item in factoryUrls)
            {
                Task<List<ProductionAchievementRateData>> task = Task.Run(() => SmomControlHepler.SmomPost<List<ProductionAchievementRateData>>("KzReportController", "ProductionAchievementRateFactory", item, new List<SmomParam>()
                    {
                    new SmomParam { Value =rateData },
                    new SmomParam { Value =mrpDics },
                    new SmomParam { Value =dicInvCodeProcessCode },
                    new SmomParam { Value =list },
                                 }.ToArray()));
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());

            foreach (var task in tasks)
            {
                // 检查任务是否成功完成
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    datas.AddRange(task.Result);
                }
            }
            datas = datas.Where(p => p.ProductLine != null).ToList();
            if (datas.Count == 0)
                datas.Add(new ProductionAchievementRateData());




            List<ProductionAchievementRateData> datasMode1 = new List<ProductionAchievementRateData>();
            List<ProductionAchievementRateData> datasMode2 = new List<ProductionAchievementRateData>();
            List<ProductionAchievementRateData> datasMode3 = new List<ProductionAchievementRateData>();
            List<ProductionAchievementRateData> datasMode4 = new List<ProductionAchievementRateData>();



            //分组: 产品线 >>>  聚合 产品线
            datasMode1 = datas
                .Where(w => ByPlantData.onlyProcesses.Any(t => w.ProcessName.Contains(t))) // 这里改成模糊匹配
                .GroupBy(g => new
                {
                    ProductLine = g.ProductLine.Trim()

                })
                .Select(s => new ProductionAchievementRateData
                {
                    ProductLine = s.Key.ProductLine,
                    PlantName = "",
                    ProcessName = "",
                    ProductCode = "",
                    ProductName = "",
                    UnitName = "",
                    PlanQty = s.Sum(x => x.PlanQty),
                    ActualQty = s.Sum(x => x.ActualQty),
                    ProductionAchievement = (s.Sum(x => x.PlanQty) == 0 || s.Sum(x => x.ActualQty) == 0) ? 0 : (s.Sum(x => x.ActualQty) / s.Sum(x => x.PlanQty))
                })
                .ToList();

            //分组:产部 >>>  聚合 产品线+产部
            datasMode2 = datas
                 .Where(p => ByPlantData.onlyProcesses.Any(t => p.ProcessName.Contains(t))) // 这里改成模糊匹配
                 .GroupBy(p => new
                 {
                     ProductLine = p.ProductLine.Trim(),
                     PlantName = p.PlantName.Trim()
                 })
                 .Select(g => new ProductionAchievementRateData
                 {
                     ProductLine = g.Key.ProductLine,
                     PlantName = g.Key.PlantName,
                     ProcessName = "",
                     ProductCode = "",
                     ProductName = "",
                     UnitName = "",
                     PlanQty = g.Sum(x => x.PlanQty),
                     ActualQty = g.Sum(x => x.ActualQty),
                     ProductionAchievement = (g.Sum(x => x.PlanQty) == 0 || g.Sum(x => x.ActualQty) == 0) ? 0 : (g.Sum(x => x.ActualQty) / g.Sum(x => x.PlanQty))
                 })
                 .ToList();

            //分组:工序 >>>  聚合 产品线+产部+工序
            datasMode3 = datas
                 .GroupBy(p => new
                 {
                     p.ProductLine,
                     p.PlantName,
                     p.ProcessName

                 })
                 .Select(g => new ProductionAchievementRateData
                 {
                     ProductLine = g.Key.ProductLine,
                     PlantName = g.Key.PlantName,
                     ProcessName = g.Key.ProcessName,
                     ProductCode = "",
                     ProductName = "",
                     UnitName = "",
                     PlanQty = g.Sum(x => x.PlanQty),
                     ActualQty = g.Sum(x => x.ActualQty),
                     ProductionAchievement = (g.Sum(x => x.PlanQty) == 0 || g.Sum(x => x.ActualQty) == 0) ? 0 : (g.Sum(x => x.ActualQty) / g.Sum(x => x.PlanQty))
                 })
                 .ToList();

            //分组:产品 >>>  聚合ProductCode的数量,(严格一点:ProductLine+PlantName+ProcessName+UnitName]+[ProductCode+ProductName])
            datasMode4 = datas
                 .Where(p => rateData.ItemCodes != null && rateData.ItemCodes.Any(t => p.ProductCode == t)) //只有产品级的维度时才过滤。集合.Any(项 => 条件)
                 .GroupBy(p => new
                 {
                     p.ProductLine,
                     p.PlantName,
                     p.ProcessName,
                     p.ProductCode,
                     p.ProductName,
                     p.UnitName
                 })
                 .Select(g => new ProductionAchievementRateData
                 {
                     ProductLine = g.Key.ProductLine,
                     PlantName = g.Key.PlantName,
                     ProcessName = g.Key.ProcessName,
                     ProductCode = g.Key.ProductCode,
                     ProductName = g.Key.ProductName,
                     UnitName = g.Key.UnitName,
                     PlanQty = g.Sum(x => x.PlanQty),
                     ActualQty = g.Sum(x => x.ActualQty),
                     ProductionAchievement = (g.Sum(x => x.PlanQty) == 0 || g.Sum(x => x.ActualQty) == 0) ? 0 : (g.Sum(x => x.ActualQty) / g.Sum(x => x.PlanQty))
                 })
                 .ToList();


            if (datasMode1.Count == 0) { datasMode1.Add(new ProductionAchievementRateData()); }
            if (datasMode2.Count == 0) { datasMode2.Add(new ProductionAchievementRateData()); }
            if (datasMode3.Count == 0) { datasMode3.Add(new ProductionAchievementRateData()); }
            if (datasMode4.Count == 0) { datasMode4.Add(new ProductionAchievementRateData()); }

            string groupName = rateData.GroupName ?? "";
            switch (true)
            {
                case true when groupName.Contains(GroupNameBy.ByProductLine.GetDescription()):
                    return datasMode1;


                case true when groupName.Contains(GroupNameBy.ByPlant.GetDescription()):
                    return datasMode2;


                case true when groupName.Contains(GroupNameBy.ByProcess.GetDescription()):
                    return datasMode3;

                case true when groupName.Contains(GroupNameBy.ByProduct.GetDescription()):
                    return datasMode4;

                default: //其他默认：产品线维度-ByProductLine
                    return datasMode1;
            }





        }


        /// <summary>
        /// 获取下拉物料（维度必须是产品级）
        /// </summary>
        /// <param name="rateData"></param>
        /// <returns></returns>
        [ApiService("获取下拉物料（维度必须是产品级）")]
        public virtual List<Products> GetProductCodeList(RequestProductionAchievementRateData rateData)
        {
            List<Products> result = new List<Products>();
            List<DispatchTaskData> dispatchTaskDatas = new List<DispatchTaskData>();

            //限定厂部-MRP控制者
            var mrpDics = RT.Service.Resolve<OrganizeCodeController>().GetMrps(rateData.ProductLine, rateData.PlantName);
            //限定工序-工序编码
            var onlyProcesses = rateData.ProcessCodes; // ByPlantData.onlyProcesses.ToList();
            //限定日期
            DateRange dateRange = rateData.DateRange;


            // var setting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingUrl();
            //请求的url
            var SmomControlSettings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(mrpDics.Select(p => p.DicKey).ToList());
            var factoryUrls = SmomControlSettings.Select(p => p.FactoryUrl).Distinct().ToList();


            //派工
            List<Task<List<DispatchTaskData>>> tasktDispatchTaskDatas = new List<Task<List<DispatchTaskData>>>();
            foreach (var url in factoryUrls)
            {
                Task<List<DispatchTaskData>> task = Task.Run(() => SmomControlHepler.SmomPost<List<DispatchTaskData>>("KzReportController", "GetDispatchTaskData", url, new List<SmomParam>()
                {
                    new SmomParam { Value =mrpDics },
                    new SmomParam { Value =onlyProcesses},
                    new SmomParam { Value =dateRange },
                }
                .ToArray()));
                tasktDispatchTaskDatas.Add(task);
            }
            Task.WaitAll(tasktDispatchTaskDatas.ToArray());
            foreach (var task in tasktDispatchTaskDatas) { if (task.Status == TaskStatus.RanToCompletion) { dispatchTaskDatas.AddRange(task.Result); } }


            //去重转换
            result = dispatchTaskDatas.GroupBy(x => new { x.ItemCode, x.ItemName })
            .Select(g => new Products { ProductCode = g.Key.ItemCode, ProductName = g.Key.ItemName })
            .ToList();


            if (result.Count <= 0) { result.Add(new Products()); }

            return result;
        }


        #endregion

        #region 质量不良统计报表
        /// <summary>
        /// 质量不良统计报表
        /// </summary>
        /// <param name="model">请求参数</param>
        /// <returns></returns>
        [ApiService("质量不良统计报表")]
        public virtual List<QualityDefectReportData> GetQualityDefectReportDatas(RequestQualityDefectReportData model)
        {
            var q = Query<OrganizeCode>();
            if (!model.ProductLine.IsNullOrEmpty())
                q.Where(p => p.ProductLine == model.ProductLine);
            if (!model.PlantName.IsNullOrEmpty())
                q.Where(p => p.PlantName == model.PlantName);

            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);

            List<QualityDefectReportFactoryData> factoryDatas = new List<QualityDefectReportFactoryData>();

            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                foreach (var plang in organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).GroupBy(p => new { p.ProductLine, p.PlantName }))
                {
                    try
                    {
                        var mrpControllers = plang.Select(p => p.MrpController).Distinct().ToList();
                        var smomParam = new List<SmomParam>()
                        {
                            new SmomParam { Value = plang.Select(p => p.FactoryCode).Distinct().ToList() },
                            new SmomParam { Value = mrpControllers },
                            new SmomParam { Value = model.Process },
                            new SmomParam { Value = model.ProductCode },
                            new SmomParam { Value = model.OldPartNumber },
                            new SmomParam { Value = model.IsReWork },
                            new SmomParam { Value = model.DateRange?.BeginValue },
                            new SmomParam { Value = model.DateRange?.EndValue }
                        }.ToArray();
                        var response = SmomControlHepler.SmomPost<List<QualityDefectReportFactoryData>>("KzReportController", "GetQualityDefectReportDatasFactory", g.Key, smomParam);
                        foreach (var item in response)
                        {
                            item.ProductLine = plang.Key.ProductLine;
                            item.PlantName = plang.Key.PlantName;
                        }
                        factoryDatas.AddRange(response);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            // 根据分组维度进行聚合
            bool hasProductLine = !model.ProductLine.IsNullOrEmpty();
            bool hasPlant = !model.PlantName.IsNullOrEmpty();
            bool hasProcess = !model.Process.IsNullOrEmpty();
            List<QualityDefectReportData> datas = new List<QualityDefectReportData>();

            if (hasProductLine && hasPlant && hasProcess)
            {
                // 按产品分组：无统计图显示，只显示下方统计表
                datas = factoryDatas
                    .GroupBy(p => new { p.ProductLine, p.PlantName, p.Process, p.ProductCode, p.ProductName, p.OldPartNumber })
                    .Select(g => new QualityDefectReportData
                    {
                        ProductLine = g.Key.ProductLine,
                        PlantName = g.Key.PlantName,
                        Process = g.Key.Process,
                        ProductCode = g.Key.ProductCode,
                        ProductName = g.Key.ProductName,
                        OldPartNumber = g.Key.OldPartNumber,
                        TotalOutput = g.Sum(x => x.ReportQty) / 10000,
                        TotalScrap = g.Sum(x => x.RecordNgQty) / 10000,
                        TotalRework = g.Sum(x => x.ReworkQty) / 10000,
                        TotalSuspect = g.Sum(x => x.SuspectQty) / 10000,
                        ScrapRate = g.Sum(x => x.ReportQty) == 0 ? 0 : Math.Round((g.Sum(x => x.RecordNgQty) / g.Sum(x => x.ReportQty)) * 100, 4),
                        SuspectRate = g.Sum(x => x.ReportQty) == 0 ? 0 : Math.Round((g.Sum(x => x.SuspectQty) / g.Sum(x => x.ReportQty)) * 100, 4),
                        FirstPassYield = g.Sum(x => x.ReportQty) == 0 ? 0 : Math.Round((1 - (g.Sum(x => x.SuspectQty) / g.Sum(x => x.ReportQty))) * 100, 4)
                    })
                    .ToList();
            }
            else if (hasProductLine && hasPlant)
            {
                // 按工序分组：工序总产量=工序报工总量（含可疑品）
                datas = factoryDatas
                    .GroupBy(p => new { p.ProductLine, p.PlantName, p.Process })
                    .Select(g => new QualityDefectReportData
                    {
                        ProductLine = g.Key.ProductLine,
                        PlantName = g.Key.PlantName,
                        Process = g.Key.Process,
                        ProductCode = "",
                        ProductName = "",
                        OldPartNumber = "",
                        TotalOutput = g.Sum(x => x.ReportQty) / 10000,
                        TotalScrap = g.Sum(x => x.RecordNgQty) / 10000,
                        TotalRework = g.Sum(x => x.ReworkQty) / 10000,
                        TotalSuspect = g.Sum(x => x.SuspectQty) / 10000,
                        ScrapRate = g.Sum(x => x.ReportQty) == 0 ? 0 : Math.Round((g.Sum(x => x.RecordNgQty) / g.Sum(x => x.ReportQty)) * 100, 4),
                        SuspectRate = g.Sum(x => x.ReportQty) == 0 ? 0 : Math.Round((g.Sum(x => x.SuspectQty) / g.Sum(x => x.ReportQty)) * 100, 4),
                        FirstPassYield = g.Sum(x => x.ReportQty) == 0 ? 0 : Math.Round((1 - (g.Sum(x => x.SuspectQty) / g.Sum(x => x.ReportQty))) * 100, 4)
                    })
                    .ToList();
            }
            else if (hasProductLine)
            {
                // 按产部分组：总产量=成品包装合格数+所有过程工序的报废量
                datas = factoryDatas
                    .GroupBy(p => new { p.ProductLine, p.PlantName })
                    .Select(g => new QualityDefectReportData
                    {
                        ProductLine = g.Key.ProductLine,
                        PlantName = g.Key.PlantName,
                        Process = "",
                        ProductCode = "",
                        ProductName = "",
                        OldPartNumber = "",
                        TotalOutput = g.Sum(x => x.ReportQty) / 10000,
                        TotalScrap = g.Sum(x => x.RecordNgQty) / 10000,
                        TotalRework = g.Sum(x => x.ReworkQty) / 10000,
                        TotalSuspect = g.Sum(x => x.SuspectQty) / 10000,
                        ScrapRate = g.Sum(x => x.ReportQty) == 0 ? 0 : Math.Round((g.Sum(x => x.RecordNgQty) / g.Sum(x => x.ReportQty)) * 100, 4),
                        SuspectRate = g.Sum(x => x.ReportQty) == 0 ? 0 : Math.Round((g.Sum(x => x.SuspectQty) / g.Sum(x => x.ReportQty)) * 100, 4),
                        FirstPassYield = g.Sum(x => x.ReportQty) == 0 ? 0 : Math.Round((1 - (g.Sum(x => x.SuspectQty) / g.Sum(x => x.ReportQty))) * 100, 4)
                    })
                    .ToList();
            }
            else
            {
                // 默认按产品线分组：总产量=成品包装合格数+所有过程工序的报废量
                datas = factoryDatas
                    .GroupBy(p => new { p.ProductLine })
                    .Select(g => new QualityDefectReportData
                    {
                        ProductLine = g.Key.ProductLine,
                        PlantName = "",
                        Process = "",
                        ProductCode = "",
                        ProductName = "",
                        OldPartNumber = "",
                        TotalOutput = g.Sum(x => x.ReportQty) / 10000,
                        TotalScrap = g.Sum(x => x.RecordNgQty) / 10000,
                        TotalRework = g.Sum(x => x.ReworkQty) / 10000,
                        TotalSuspect = g.Sum(x => x.SuspectQty) / 10000,
                        ScrapRate = g.Sum(x => x.ReportQty) == 0 ? 0 : Math.Round((g.Sum(x => x.RecordNgQty) / g.Sum(x => x.ReportQty)) * 100, 4),
                        SuspectRate = g.Sum(x => x.ReportQty) == 0 ? 0 : Math.Round((g.Sum(x => x.SuspectQty) / g.Sum(x => x.ReportQty)) * 100, 4),
                        FirstPassYield = g.Sum(x => x.ReportQty) == 0 ? 0 : Math.Round((1 - (g.Sum(x => x.SuspectQty) / g.Sum(x => x.ReportQty))) * 100, 4)
                    })
                    .ToList();
            }

            if (datas.Count == 0)
                datas.Add(new QualityDefectReportData());

            int index = 1;
            foreach (var item in datas)
            {
                item.Num = index;
                index++;
            }

            return datas;
        }
        #endregion

        #region 质量帕累托图
        /// <summary>
        /// 质量帕累托图
        /// </summary>
        /// <param name="model">请求参数</param>
        /// <returns></returns>
        [ApiService("质量帕累托图")]
        public virtual ResponseQualityParetoChartData GetQualityParetoChartData(RequestQualityParetoChartData model)
        {
            var result = new ResponseQualityParetoChartData();
            result.DefectProductTableList = new List<DefectProductParetoData>();
            result.DefectNameTableList = new List<DefectNameParetoData>();

            var q = Query<OrganizeCode>();
            if (!model.ProductLine.IsNullOrEmpty())
                q.Where(p => p.ProductLine == model.ProductLine);
            if (!model.PlantName.IsNullOrEmpty())
                q.Where(p => p.PlantName == model.PlantName);

            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);

            List<QualityParetoChartFactoryData> factoryDatas = new List<QualityParetoChartFactoryData>();

            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                foreach (var plang in organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).GroupBy(p => new { p.ProductLine, p.PlantName }))
                {
                    try
                    {
                        var mrpControllers = plang.Select(p => p.MrpController).Distinct().ToList();
                        var smomParam = new List<SmomParam>()
                        {
                            new SmomParam { Value = plang.Select(p => p.FactoryCode).Distinct().ToList() },
                            new SmomParam { Value = mrpControllers },
                            new SmomParam { Value = model.Process },
                            new SmomParam { Value = model.ProductCode },
                            new SmomParam { Value = model.OldPartNumber },
                            new SmomParam { Value = model.DateRange?.BeginValue },
                            new SmomParam { Value = model.DateRange?.EndValue }
                        }.ToArray();
                        var response = SmomControlHepler.SmomPost<List<QualityParetoChartFactoryData>>("KzReportController", "GetQualityParetoChartDataFactory", g.Key, smomParam);
                        factoryDatas.AddRange(response);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            if (factoryDatas.Count > 0)
            {
                // ========== 1. 缺陷名称帕累托图 ==========
                // 按缺陷代码+缺陷名称分组汇总数量，按数量从高到低排序
                var defectNameAll = factoryDatas.Where(p => !p.DefectCode.IsNullOrWhiteSpace())
                    .GroupBy(p => new { p.DefectCode, p.DefectName })
                    .Select(g => new DefectNameParetoData
                    {
                        DefectCode = g.Key.DefectCode,
                        DefectName = g.Key.DefectName,
                        Qty = g.Sum(x => x.Qty)
                    })
                    .OrderByDescending(p => p.Qty)
                    .ToList();

                // 占比按全部数据项计算
                var defectTotalQty = defectNameAll.Sum(p => p.Qty);
                int defectIndex = 1;
                decimal defectSum = 0;
                foreach (var item in defectNameAll)
                {
                    item.Num = defectIndex;
                    item.Rate = defectTotalQty == 0 ? 0 : Math.Round((item.Qty / defectTotalQty) * 100, 2);
                    defectSum += item.Rate;
                    if (defectSum > 100)
                        defectSum = 100;
                    item.RateSum = defectSum;
                    defectIndex++;
                }
                // 页面最多展示前20项
                result.DefectNameTableList = defectNameAll.Take(20).ToList();

                // ========== 2. 不良品帕累托图 ==========
                // 筛选判定结果为"返工"或"报废"的明细
                var defectProductFiltered = factoryDatas
                    .Where(p => p.JudgmentResult == "Scrap" || p.JudgmentResult == "Repair")
                    .ToList();

                if (defectProductFiltered.Count > 0)
                {
                    // 只根据产品编码进行分类汇总
                    var defectProductAll = defectProductFiltered
                        .GroupBy(p => new { p.ProductCode, p.ProductName, p.OldPartNumber })
                        .Select(g => new DefectProductParetoData
                        {
                            ProductCode = g.Key.ProductCode,
                            ProductName = g.Key.ProductName,
                            OldPartNumber = g.Key.OldPartNumber,
                            DefectCode = string.Join(",", g.Select(x => x.DefectCode).Where(c => !string.IsNullOrEmpty(c)).Distinct()),
                            DefectName = string.Join(",", g.Select(x => x.DefectName).Where(n => !string.IsNullOrEmpty(n)).Distinct()),
                            Qty = g.Sum(x => x.Qty)
                        })
                        .OrderByDescending(p => p.Qty)
                        .ToList();

                    // 占比按全部数据项计算
                    var productTotalQty = defectProductAll.Sum(p => p.Qty);
                    int productIndex = 1;
                    decimal productSum = 0;
                    foreach (var item in defectProductAll)
                    {
                        item.Num = productIndex;
                        item.Rate = productTotalQty == 0 ? 0 : Math.Round((item.Qty / productTotalQty) * 100, 2);
                        productSum += item.Rate;
                        if (productSum > 100)
                            productSum = 100;
                        item.RateSum = productSum;
                        productIndex++;
                    }
                    // 页面最多展示前20项产品
                    result.DefectProductTableList = defectProductAll.Take(20).ToList();
                }
            }

            //注：没有数据不能直接返回空数组，应该返回一条假数据如下
            if (!result.DefectNameTableList.Any())
            {
                result.DefectNameTableList.Add(new DefectNameParetoData() { Num = 1, DefectCode = "无数据" });
            }
            if (!result.DefectProductTableList.Any())
            {
                result.DefectProductTableList.Add(new DefectProductParetoData() { Num = 1, ProductCode = "无数据" });
            }
            return result;
        }
        #endregion

        #region 厂部看板相关接口

        /// <summary>
        ///  周计划达成率良品率报表。[最近7日达成率、不良品率]
        /// </summary>
        /// <param name="requestPlantData"></param>
        /// <returns></returns>
        [ApiService("周计划达成率报表")]
        public virtual List<ResponsePlanAchievedData> PlanAchievedData(RequestPlantData requestPlantData)
        {
            List<ResponsePlanAchievedData> result = new List<ResponsePlanAchievedData>();
            List<ResponseDispatchQtyData> resultDispatch = new List<ResponseDispatchQtyData>();
            List<ReportRecordData> resultRecord = new List<ReportRecordData>();

            //限定厂部-MRP控制者         
            var mrpDics = RT.Service.Resolve<OrganizeCodeController>().GetMrps2(requestPlantData.PlantName, requestPlantData.FactoryCodeList);


            //限定日期-最近7日
            DateRange dateRange = new DateRange();
            dateRange.BeginValue = DateTime.Now.AddDays(-6).Date;
            dateRange.EndValue = DateTime.Now.AddDays(1).Date;

            // var settings = ByPlantData.debugSettingUrl; //调试专用
            //请求的url
            var SmomControlSettings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(mrpDics.Select(p => p.DicKey).ToList());
            var factoryUrls = SmomControlSettings.Select(p => p.FactoryUrl).Distinct().ToList();

            //计划产量
            List<Task<List<ResponseDispatchQtyData>>> tasktDispatchs = new List<Task<List<ResponseDispatchQtyData>>>();
            foreach (var url in factoryUrls)
            {
                Task<List<ResponseDispatchQtyData>> task = Task.Run(() => SmomControlHepler.SmomPost<List<ResponseDispatchQtyData>>("KzReportController", "GetDispatchQtyData", url, new List<SmomParam>()
                {
                    new SmomParam { Value =mrpDics },
                    new SmomParam { Value =ByPlantData.onlyProcesses.ToList() },
                    new SmomParam { Value =dateRange },
                }
                .ToArray()));
                tasktDispatchs.Add(task);
            }
            Task.WaitAll(tasktDispatchs.ToArray());
            foreach (var task in tasktDispatchs) { if (task.Status == TaskStatus.RanToCompletion) { if (task.Result != null && task.Result.Any()) { resultDispatch.AddRange(task.Result); } } }

            //报工记录
            List<Task<List<ReportRecordData>>> tasktRecords = new List<Task<List<ReportRecordData>>>();
            foreach (var url in factoryUrls)
            {
                Task<List<ReportRecordData>> task = Task.Run(() => SmomControlHepler.SmomPost<List<ReportRecordData>>("KzReportController", "GetReportRecordData", url, new List<SmomParam>()
                {
                    new SmomParam { Value =mrpDics },
                    new SmomParam { Value =ByPlantData.onlyProcesses.ToList() },
                    new SmomParam { Value =dateRange },
                }
                .ToArray()));
                tasktRecords.Add(task);
            }
            Task.WaitAll(tasktRecords.ToArray());
            foreach (var task in tasktRecords) { if (task.Status == TaskStatus.RanToCompletion) { if (task.Result != null && task.Result.Any()) { resultRecord.AddRange(task.Result); } } }


            //可疑品标签明细 
            List<Task<List<SuspectProdData>>> taskSuspectProdDatas = new List<Task<List<SuspectProdData>>>();
            foreach (var url in factoryUrls)
            {
                Task<List<SuspectProdData>> task = Task.Run(() =>
                SmomControlHepler.SmomPost<List<SuspectProdData>>("KzReportController", "GetSuspectProd", url, new List<SmomParam>()
                {
                    new SmomParam { Value = mrpDics },
                    new SmomParam { Value =dateRange }
                }
                .ToArray()));
                taskSuspectProdDatas.Add(task);
            }
            Task.WaitAll(taskSuspectProdDatas.ToArray());
            List<SuspectProdData> resultSuspectProdData = new List<SuspectProdData>();
            foreach (var task in taskSuspectProdDatas) { if (task.Status == TaskStatus.RanToCompletion) { if (task.Result != null && task.Result.Any()) { resultSuspectProdData.AddRange(task.Result); } } }





            //---计算与合并--------------------------------------------------------------------------------------------------------------------------------

            //计划产量数量：result1
            var result1 = resultDispatch.GroupBy(x => x.PlanBeginDate)
                .Select(g => new ResponseDispatchQtyData
                {
                    PlanBeginDate = g.Key,
                    DispatchQTY = g.Sum(x => x.DispatchQTY)
                }).ToList();

            //报工数量：result2
            var result2 = resultRecord.GroupBy(x => x.ReportTime)
                .Select(g => new ReportRecordData
                {
                    ReportTime = g.Key,
                    ReportQty = g.Sum(x => x.ReportQty),
                    OkQty = g.Sum(x => x.OkQty),
                    NgQty = g.Sum(x => x.NgQty),
                    ReworkQty = g.Sum(x => x.ReworkQty)
                }).ToList();

            //可疑品标签数量：result3           
            var result3 = resultSuspectProdData
            .Where(x => !string.IsNullOrEmpty(x.Code))
            .GroupBy(x => x.UpdateDateDay)
            .Select(g => new SuspectProdData
            {
                UpdateDateDay = g.Key,
                DefectQTY = g.Sum(x => x.DefectQTY)
            }).ToList();




            DateTime t = dateRange.BeginValue.Value;
            while (t < dateRange.EndValue.Value)
            {
                ResponsePlanAchievedData responsePlanAchievedData = new ResponsePlanAchievedData();
                responsePlanAchievedData.ThisDay = t.Date;
                responsePlanAchievedData.PlanQty = result1.FirstOrDefault(w => w.PlanBeginDate?.Date == t.Date)?.DispatchQTY ?? 0;
                responsePlanAchievedData.ActualQty = result2.FirstOrDefault(w => w.ReportTime?.Date == t.Date)?.ReportQty ?? 0;
                responsePlanAchievedData.OkQty = result2.FirstOrDefault(w => w.ReportTime?.Date == t.Date)?.OkQty ?? 0;
                responsePlanAchievedData.NgQty = result2.FirstOrDefault(w => w.ReportTime?.Date == t.Date)?.NgQty ?? 0;
                responsePlanAchievedData.ReworkQty = result2.FirstOrDefault(w => w.ReportTime?.Date == t.Date)?.ReworkQty ?? 0;


                //达成率：
                responsePlanAchievedData.DayAchievedRate = (responsePlanAchievedData.PlanQty > 0) ? (responsePlanAchievedData.ActualQty / responsePlanAchievedData.PlanQty) * 100 : 100;
                if (responsePlanAchievedData.DayAchievedRate > 100) { responsePlanAchievedData.DayAchievedRate = 100; }



                //不良率：
                #region 20260611修改新逻辑
                //分子A：可疑品标签明细判断为《报废》和《返工》的数量之和
                //分母B：A + 报工数量的合格数之和(仅成品包装或押出)
                #endregion
                var A = result3.FirstOrDefault(w => w.UpdateDateDay?.Date == t.Date)?.DefectQTY ?? 0;
                var B = A + result2.FirstOrDefault(w => w.ReportTime?.Date == t.Date)?.OkQty ?? 0;
                responsePlanAchievedData.NgQtyRate = A > 0 ? (A / B) * 100 : 0;



                result.Add(responsePlanAchievedData);
                t = t.AddDays(1);
            }

            return result;
        }

        /// <summary>
        /// 月计划达成率
        /// </summary>
        /// <returns></returns>
        [ApiService("月计划达成率")]
        public virtual ResponseMouthAchievedRate MonthAchievedRate(RequestPlantData requestPlantData)
        {
            ResponseMouthAchievedRate result = new ResponseMouthAchievedRate();

            List<ResponseDispatchQtyData> resultDispatch = new List<ResponseDispatchQtyData>();
            List<ReportRecordData> resultRecord = new List<ReportRecordData>();

            //限定厂部-MRP控制者           
            var mrpDics = RT.Service.Resolve<OrganizeCodeController>().GetMrps2(requestPlantData.PlantName, requestPlantData.FactoryCodeList);

            //限定日期-当前自然月
            DateRange dateRange = new DateRange();
            dateRange.BeginValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            dateRange.EndValue = DateTime.Now.AddDays(1).Date;

            //var setting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingUrl();
            var SmomControlSettings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(mrpDics.Select(p => p.DicKey).ToList());
            var factoryUrls = SmomControlSettings.Select(p => p.FactoryUrl).Distinct().ToList();

            //计划产量
            List<Task<List<ResponseDispatchQtyData>>> tasktDispatchs = new List<Task<List<ResponseDispatchQtyData>>>();
            foreach (var url in factoryUrls)
            {
                Task<List<ResponseDispatchQtyData>> task = Task.Run(() => SmomControlHepler.SmomPost<List<ResponseDispatchQtyData>>("KzReportController", "GetDispatchQtyData", url, new List<SmomParam>()
                {
                    new SmomParam { Value =mrpDics },
                    new SmomParam { Value =ByPlantData.onlyProcesses.ToList() },
                    new SmomParam { Value =dateRange },
                }
                .ToArray()));
                tasktDispatchs.Add(task);
            }
            Task.WaitAll(tasktDispatchs.ToArray());
            foreach (var task in tasktDispatchs) { if (task.Status == TaskStatus.RanToCompletion) { if (task.Result != null && task.Result.Any()) { resultDispatch.AddRange(task.Result); } } }

            //报工记录
            List<Task<List<ReportRecordData>>> tasktRecords = new List<Task<List<ReportRecordData>>>();
            foreach (var url in factoryUrls)
            {
                Task<List<ReportRecordData>> task = Task.Run(() => SmomControlHepler.SmomPost<List<ReportRecordData>>("KzReportController", "GetReportRecordData", url, new List<SmomParam>()
                {
                    new SmomParam { Value =mrpDics },
                    new SmomParam { Value =ByPlantData.onlyProcesses.ToList() },
                    new SmomParam { Value =dateRange },
                }
                .ToArray()));
                tasktRecords.Add(task);
            }
            Task.WaitAll(tasktRecords.ToArray());
            foreach (var task in tasktRecords) { if (task.Status == TaskStatus.RanToCompletion) { if (task.Result != null && task.Result.Any()) { resultRecord.AddRange(task.Result); } } }

            //---计算--------------------------------------------------------------------------------------------------------------------------------
            decimal totalDispatchQTY = resultDispatch?.Sum(x => x.DispatchQTY) ?? 0;
            decimal totalReportQty = resultRecord?.Sum(x => x.ReportQty) ?? 0;
            result.MouthAchievedRate = totalDispatchQTY > 0 ? (totalReportQty / totalDispatchQTY) * 100 : 100;
            if (result.MouthAchievedRate > 100) { result.MouthAchievedRate = 100; }

            return result;

        }

        /// <summary>
        /// 质量缺陷
        /// </summary>
        /// <param name="requestPlantData"></param>
        /// <returns></returns>
        [ApiService("质量缺陷")]
        public virtual List<SuspectProdData> SuspectProdData(RequestPlantData requestPlantData)
        {
            List<SuspectProdData> result = new List<SuspectProdData>();



            //条件1：工厂编码-MRP控制者
            var mrpDics = RT.Service.Resolve<OrganizeCodeController>().GetMrps2(requestPlantData.PlantName, requestPlantData.FactoryCodeList);

            //条件2：当前自然月      
            DateRange dateRange = new DateRange();
            dateRange.BeginValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            dateRange.EndValue = DateTime.Now;

            //请求的url
            var SmomControlSettings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(mrpDics.Select(p => p.DicKey).ToList());
            var factoryUrls = SmomControlSettings.Select(p => p.FactoryUrl).Distinct().ToList();

            //执行工厂接口
            List<Task<List<SuspectProdData>>> tasks = new List<Task<List<SuspectProdData>>>();
            //var setting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingUrl();           
            foreach (var url in factoryUrls)
            {
                Task<List<SuspectProdData>> task = Task.Run(() =>
                SmomControlHepler.SmomPost<List<SuspectProdData>>("KzReportController", "GetSuspectProd", url, new List<SmomParam>()
                {
                    new SmomParam { Value = mrpDics },
                    new SmomParam { Value =dateRange }
                }
                .ToArray()));
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            List<SuspectProdData> resultSuspectProdData = new List<SuspectProdData>();
            foreach (var task in tasks) { if (task.Status == TaskStatus.RanToCompletion) { if (task.Result != null && task.Result.Any()) { resultSuspectProdData.AddRange(task.Result); } } }



            //当缺陷（Code、Description）为空时，视为《待处理未判定》
            resultSuspectProdData = resultSuspectProdData.Select(item =>
            {
                if (string.IsNullOrWhiteSpace(item.Code)) { item.Code = "000"; }
                if (string.IsNullOrWhiteSpace(item.Description)) { item.Description = "待处理未判定"; }
                return item;
            }).ToList();



            //处理数据
            if (resultSuspectProdData.Count > 0)
            {
                var dt1 = resultSuspectProdData
                       .GroupBy(p => new { Code = p.Code, Description = p.Description })
                       .Select(s => new SuspectProdData { Code = s.Key.Code, Description = s.Key.Description, DefectQTY = s.Sum(x => x.DefectQTY) })
                       .OrderByDescending(x => x.DefectQTY)
                       .ToList();


                //帕累图
                decimal qty = 0;
                decimal total = resultSuspectProdData.Sum(x => x.DefectQTY);
                foreach (var d in dt1)
                {
                    SuspectProdData suspectProdData = new SuspectProdData();
                    suspectProdData.Code = d.Code;
                    suspectProdData.Description = d.Description;
                    suspectProdData.DefectQTY = d.DefectQTY;
                    qty = qty + d.DefectQTY;
                    suspectProdData.DefectRate = total > 0 ? (qty / total) * 100 : 100; //帕累图
                    result.Add(suspectProdData);

                }

            }
            else { result.Add(new SuspectProdData()); }


            return result;
        }

        /// <summary>
        /// 安灯异常报表-基于类型统计数量（仅：待响应+处理中）
        /// </summary>
        /// <param name="requestPlantData"></param>
        /// <returns></returns>
        [ApiService("安灯异常报表-基于类型统计数量")]
        public virtual AndonReportDataOfType AndonTypeReportData(RequestPlantData requestPlantData)
        {
            AndonReportDataOfType result = new AndonReportDataOfType();
            List<AllAndonStype> allAndonStypes = new List<AllAndonStype>();
            List<BaseAdonAndonManage> baseAdonAndonManages = new List<BaseAdonAndonManage>();

            //条件1：工厂编码-MRP控制者
            var mrpDics = RT.Service.Resolve<OrganizeCodeController>().GetMrps2(requestPlantData.PlantName, requestPlantData.FactoryCodeList);


            //var setting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingUrl();
            //请求的url
            var SmomControlSettings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(mrpDics.Select(p => p.DicKey).ToList());
            var factoryUrls = SmomControlSettings.Select(p => p.FactoryUrl).Distinct().ToList();

            //获取所有的安灯类型
            List<Task<List<AllAndonStype>>> tasktAllAndonStypes = new List<Task<List<AllAndonStype>>>();
            foreach (var url in factoryUrls)
            {
                Task<List<AllAndonStype>> task = Task.Run(() =>
                SmomControlHepler.SmomPost<List<AllAndonStype>>
                (
                    "KzReportController",
                    "GetAllAndonStype",
                    url,
                    new List<SmomParam>() { new SmomParam { Value = "notParameter" }, }.ToArray())
                );
                tasktAllAndonStypes.Add(task);
            }
            Task.WaitAll(tasktAllAndonStypes.ToArray());
            foreach (var task in tasktAllAndonStypes) { if (task.Status == TaskStatus.RanToCompletion) { if (task.Result != null && task.Result.Any()) { allAndonStypes.AddRange(task.Result); } } }

            //获取所有未关闭的安灯异常
            List<Task<List<BaseAdonAndonManage>>> tasktBaseAdonAndonManages = new List<Task<List<BaseAdonAndonManage>>>();
            foreach (var url in factoryUrls)
            {
                Task<List<BaseAdonAndonManage>> task = Task.Run(() =>
                SmomControlHepler.SmomPost<List<BaseAdonAndonManage>>
                (
                    "KzReportController",
                    "GetMesAndonManage",
                    url,
                    new List<SmomParam>() { new SmomParam { Value = "notParameter" }, }.ToArray())
                );
                tasktBaseAdonAndonManages.Add(task);
            }
            Task.WaitAll(tasktBaseAdonAndonManages.ToArray());
            foreach (var task in tasktBaseAdonAndonManages) { if (task.Status == TaskStatus.RanToCompletion) { if (task.Result != null && task.Result.Any()) { baseAdonAndonManages.AddRange(task.Result); } } }


            //-----------------------------------------------------------------------------------------------------------------------------------------------------
            //---处理数据------------------------------------------------------------------------------------------------------------------------------------------
            //-----------------------------------------------------------------------------------------------------------------------------------------------------

            var resultallAndonStype = allAndonStypes.Where(x => !string.IsNullOrEmpty(x.AndonTypeName)).GroupBy(x => x.AndonTypeName).Select(g => g.First()).ToList();

            var resultbaseAdonAndonManage = !string.IsNullOrEmpty(requestPlantData.PlantName) ? baseAdonAndonManages.Where(x => x.PlantName == requestPlantData.PlantName).ToList() : baseAdonAndonManages;

            //统计数据
            result.AndonMsg = resultbaseAdonAndonManage;
            result.AllTotalCount = resultbaseAdonAndonManage.Count;
            var now = DateTime.Now;
            List<CountOfType> countOfTypes = resultallAndonStype.Select(s => new CountOfType
            {
                AndonType = s.AndonTypeName,

                // 总次数
                TotalCount = resultbaseAdonAndonManage.Count(m => m.AndonTypeName == s.AndonTypeName),

                // 30~60
                Count1 = resultbaseAdonAndonManage.Count(m =>
                    m.AndonTypeName == s.AndonTypeName &&
                    (now - m.TriggerTime).TotalMinutes > 30 &&
                    (now - m.TriggerTime).TotalMinutes < 60),

                // 60~120
                Count2 = resultbaseAdonAndonManage.Count(m =>
                    m.AndonTypeName == s.AndonTypeName &&
                    (now - m.TriggerTime).TotalMinutes >= 60 &&
                    (now - m.TriggerTime).TotalMinutes < 120),

                // 120~240
                Count3 = resultbaseAdonAndonManage.Count(m =>
                    m.AndonTypeName == s.AndonTypeName &&
                    (now - m.TriggerTime).TotalMinutes >= 120 &&
                    (now - m.TriggerTime).TotalMinutes < 240),

                // >=240
                Count4 = resultbaseAdonAndonManage.Count(m =>
                    m.AndonTypeName == s.AndonTypeName &&
                    (now - m.TriggerTime).TotalMinutes >= 240)
            }).ToList();

            result.CountOfType = countOfTypes;


            return result;
        }

        /// <summary>
        /// 产线和设备状态看板
        /// </summary>
        /// <param name="requestPlantData"></param>
        /// <returns></returns>
        [ApiService("产线和设备状态看板")]
        public virtual List<LineMachineStatusData> LineMachineStatusData(RequestPlantData requestPlantData)
        {

            List<LineMachineStatusData> result = new List<LineMachineStatusData>();
            List<LineEquipmentStatusData> lineEquipmentStatusDatas = new List<LineEquipmentStatusData>();

            //条件：工厂编码（库存组织），MRP未用到
            var mrpDics = RT.Service.Resolve<OrganizeCodeController>().GetMrps2(requestPlantData.PlantName, requestPlantData.FactoryCodeList);


            //var setting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingUrl();
            //请求的url
            var SmomControlSettings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(mrpDics.Select(p => p.DicKey).ToList());
            var factoryUrls = SmomControlSettings.Select(p => p.FactoryUrl).Distinct().ToList();

            List<Task<List<LineEquipmentStatusData>>> tasks = new List<Task<List<LineEquipmentStatusData>>>();
            foreach (var url in factoryUrls)
            {
                Task<List<LineEquipmentStatusData>> task = Task.Run(() =>
                SmomControlHepler.SmomPost<List<LineEquipmentStatusData>>("KzReportController", "GetLineMachineStatus", url, new List<SmomParam>() { new SmomParam { Value = mrpDics } }
                .ToArray()));
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            foreach (var task in tasks) { if (task.Status == TaskStatus.RanToCompletion) { if (task.Result != null && task.Result.Any()) { lineEquipmentStatusDatas.AddRange(task.Result); } } }


            //返回结果
            decimal Unscheduled = 0;
            decimal Scheduling = 0;
            decimal Abnormal = 0;
            if (lineEquipmentStatusDatas.Count > 0)
            {
                Unscheduled = lineEquipmentStatusDatas.Count(x => x.StatuType == StatuType.Unscheduled);
                Scheduling = lineEquipmentStatusDatas.Count(x => x.StatuType == StatuType.Scheduling);
                Abnormal = lineEquipmentStatusDatas.Count(x => x.StatuType == StatuType.Abnormal);
            }
            result.Add(new LineMachineStatusData { StatusName = StatuType.Unscheduled.ToString(), StatusQty = Unscheduled });
            result.Add(new LineMachineStatusData { StatusName = StatuType.Scheduling.ToString(), StatusQty = Scheduling });
            result.Add(new LineMachineStatusData { StatusName = StatuType.Abnormal.ToString(), StatusQty = Abnormal });



            return result;
        }


        /// <summary>
        /// 获取安全生产天数
        /// </summary>
        /// <returns></returns>
        [ApiService("获取安全生产天数")]
        public virtual WorkSafetyDayData WorkSafetyDay(RequestPlantData requestPlantData)
        {

            WorkSafetyDayData result = new WorkSafetyDayData();
            result.WorkSafetyDay = 0;

            List<WorkSafetyDayData> workSafetyDayDatas = new List<WorkSafetyDayData>();

            //条件：工厂编码（库存组织），MRP未用到
            var mrpDics = RT.Service.Resolve<OrganizeCodeController>().GetMrps2(requestPlantData.PlantName, requestPlantData.FactoryCodeList);

            //var setting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingUrl();
            //请求的url
            var SmomControlSettings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(mrpDics.Select(p => p.DicKey).ToList());
            var factoryUrls = SmomControlSettings.Select(p => p.FactoryUrl).Distinct().ToList();

            List<Task<WorkSafetyDayData>> tasks = new List<Task<WorkSafetyDayData>>();
            foreach (var url in factoryUrls)
            {
                Task<WorkSafetyDayData> task = Task.Run(() =>
                SmomControlHepler.SmomPost<WorkSafetyDayData>("KzReportController", "GetWorkSafetyDay", url, new List<SmomParam>() { new SmomParam { Value = mrpDics } }
                .ToArray()));
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            foreach (var task in tasks) { if (task.Status == TaskStatus.RanToCompletion) { workSafetyDayDatas.Add(task.Result); } }

            foreach (var item in workSafetyDayDatas)
            {
                if (item.WorkSafetyDay > 0) { result.WorkSafetyDay = item.WorkSafetyDay; break; }
            }

            return result;

        }


        #endregion













        /// <summary>
        /// 产能利用率报表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ApiService("产能利用率报表")]
        public virtual List<CapacityUtilizationRateData> CapacityUtilizationRate(RequestCapacityUtilizationRateData model)
        {
            List<CapacityUtilizationRateData> datas = new List<CapacityUtilizationRateData>();
            var productionProcesses = RT.Service.Resolve<ProductionProcessController>().GetProductionProcesses(model.ProductLine, model.PlantName, model.ProcessCode);
            //获取派工任务列表
            var dicpProcessCodes = productionProcesses.GroupBy(p => p.InventoryCode).ToList()
                .ToDictionary(p => p.Select(x => x.InventoryCode).FirstOrDefault().ToString(), p => p.Select(x => x.ProcessCode).ToList());

            var setting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingUrl();

            List<Task<List<CapacityUtilizationRateData>>> tasks = new List<Task<List<CapacityUtilizationRateData>>>();
            foreach (var item in setting)
            {
                Task<List<CapacityUtilizationRateData>> task = Task.Run(() => SmomControlHepler.SmomPost<List<CapacityUtilizationRateData>>("KzReportController", "CapacityUtilizationRateFactory", item, new List<SmomParam>()
                    {
                    new SmomParam { Value =productionProcesses },
                    new SmomParam { Value =model },
                    new SmomParam { Value =dicpProcessCodes.Select(p => new DictionaryData() { DicKey = p.Key, DicValue = p.Value }).ToList<DictionaryData>()},
                                 }.ToArray()));
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());

            foreach (var task in tasks)
            {
                // 检查任务是否成功完成
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    datas.AddRange(task.Result);
                }
            }
            datas = datas.Where(p => p.ProductLine != null).ToList();
            if (datas.Count == 0)
                datas.Add(new CapacityUtilizationRateData());
            return datas;
        }

        /// <summary>
        /// 获取下拉选择数据源
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [ApiService("获取下拉选择数据源")]
        public virtual List<string> GetNum(CapacityDataType dataType, int year, int month)
        {
            List<string> str = new List<string>();
            switch (dataType)
            {
                case CapacityDataType.Moon:
                    str = new List<string>();
                    break;
                case CapacityDataType.Week:
                    var week = GetWeeksInMonth(year, month);
                    str = GenStr(week);
                    break;
                case CapacityDataType.Day:
                    var days = GetDaysInMonth(year, month);
                    str = GenStr(days);
                    break;
                default:
                    str = new List<string>();
                    break;
            }
            return str;
        }

        private List<string> GenStr(int num)
        {
            List<string> str = new List<string>();
            for (int i = 1; i < num + 1; i++)
            {
                str.Add(i.ToString());
            }
            return str;
        }

        /// <summary>
        /// 计算指定年份和月份的总天数
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份（1-12）</param>
        /// <returns>该月份的总天数</returns>
        private int GetDaysInMonth(int year, int month)
        {
            try
            {
                // 验证月份合法性
                if (month < 1 || month > 12)
                {
                    throw new ValidationException("月份必须在1-12之间".L10N());
                }

                // 获取当月最后一天，其Day属性就是总天数
                DateTime lastDayOfMonth = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
                return lastDayOfMonth.Day;
            }
            catch (Exception ex)
            {
                throw new ValidationException("计算天数时出错:{0}".L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 计算指定年份和月份的总周数
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份（1-12）</param>
        /// <returns>该月份的总周数</returns>
        private int GetWeeksInMonth(int year, int month)
        {
            try
            {
                // 验证月份是否合法
                if (month < 1 || month > 12)
                {
                    throw new ValidationException("月份必须在1-12之间".L10N());
                }

                // 获取本月第一天
                DateTime firstDayOfMonth = new DateTime(year, month, 1);
                // 获取本月最后一天
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                // 计算本月总天数
                int totalDays = lastDayOfMonth.Day;

                // 计算第一天是星期几（将周日(0)转换为7，方便计算）
                int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
                firstDayOfWeek = firstDayOfWeek == 0 ? 7 : firstDayOfWeek;

                // 计算总周数：(月初缺失天数 + 总天数) / 7 向上取整
                int totalWeeks = (firstDayOfWeek - 1 + totalDays + 6) / 7;

                return totalWeeks;
            }
            catch (Exception ex)
            {
                throw new ValidationException("计算周数时出错:{0}".L10nFormat(ex.Message));
            }
        }

        [ApiService("安灯异常统计报表")]
        public virtual List<AndonAnomalyData> AndonAnomaly(RequestAndonAnomalyData model)
        {
            //获取组织代码
            var dicOrganizeCode = RT.Service.Resolve<OrganizeCodeController>().GetOrganizeCodeList(model.ProductLine, model.PlantName);
            var dicWids = new List<DictionaryData>();
            dicOrganizeCode.ForEach((p) =>
            {
                var entityList = p.DicValue.Select(p => p as OrganizeCode).ToList<OrganizeCode>();
                dicWids.Add(new DictionaryData()
                {
                    DicKey = p.DicKey,
                    DicValue = entityList.Select(p => p.WorkshopCode).ToList()
                });

            });

            List<AndonAnomalyData> datas = new List<AndonAnomalyData>();
            List<Task<List<AndonAnomalyData>>> tasks = new List<Task<List<AndonAnomalyData>>>();
            var setting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingUrl();
            foreach (var item in setting)
            {
                Task<List<AndonAnomalyData>> task = Task.Run(() => SmomControlHepler.SmomPost<List<AndonAnomalyData>>("KzReportController", "AndonAnomalyFactory", item, new List<SmomParam>()
                    {
                    new SmomParam { Value =model },
                    new SmomParam { Value =dicOrganizeCode},
                    new SmomParam { Value = dicWids},
                                 }.ToArray()));
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());

            foreach (var task in tasks)
            {
                // 检查任务是否成功完成
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    datas.AddRange(task.Result);
                }
            }
            datas = datas.Where(p => p.ProductLine != null).ToList();
            if (datas.Count == 0)
                datas.Add(new AndonAnomalyData());
            return datas;
        }

        #region 安灯统计报表
        /// <summary>
        /// 安灯统计报表
        /// 按产品线、产部、安灯类型维度统计
        /// 统计：安灯异常停机时长、安灯次数、平均响应时长、平均处理时长、平均验收时长、各时段安灯未关闭个数
        /// </summary>
        /// <param name="model">请求参数</param>
        /// <returns></returns>
        [ApiService("安灯统计报表")]
        public virtual List<AndonStatisticsReportData> GetAndonStatisticsReportDatas(RequestAndonStatisticsReportData model)
        {
            bool expandType = model.ExpandAndonType == "是";
            bool hasProductLine = !model.ProductLine.IsNullOrEmpty();

            // 1. 查询组织代码，获取产品线/产部与工厂的映射
            var q = Query<OrganizeCode>();
            if (!model.ProductLine.IsNullOrEmpty())
                q.Where(p => p.ProductLine == model.ProductLine);
            if (!model.PlantName.IsNullOrEmpty())
                q.Where(p => p.PlantName == model.PlantName);

            var organizeCodes = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var factoryCodes = organizeCodes.Select(p => p.FactoryCode).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);

            // 2. 向各工厂请求安灯统计数据
            List<AndonStatisticsReportData> factoryDatas = new List<AndonStatisticsReportData>();

            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                foreach (var plang in organizeCodes.Where(p => fCs.Contains(p.FactoryCode)).GroupBy(p => new { p.ProductLine, p.PlantName }))
                {
                    try
                    {
                        var smomParam = new List<SmomParam>()
                        {
                            new SmomParam { Value = plang.Select(p => p.FactoryCode).Distinct().ToList() },
                            new SmomParam { Value = model.DateRange?.BeginValue },
                            new SmomParam { Value = model.DateRange?.EndValue }
                        }.ToArray();
                        var response = SmomControlHepler.SmomPost<List<AndonStatisticsReportData>>("KzReportController", "GetAndonStatisticsReportDatasFactory", g.Key, smomParam);
                        foreach (var item in response)
                        {
                            item.ProductLine = plang.Key.ProductLine;
                            item.PlantName = plang.Key.PlantName;
                        }
                        factoryDatas.AddRange(response);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            // 3. 根据参数进行分组聚合
            List<AndonStatisticsReportData> datas = new List<AndonStatisticsReportData>();

            if (hasProductLine && expandType)
            {
                // 按产品线+产部+安灯类型分组
                datas = factoryDatas
                    .GroupBy(p => new { p.ProductLine, p.PlantName, p.AndonType })
                    .Select(g => BuildStatisticsData(g, g.Key.ProductLine, g.Key.PlantName, g.Key.AndonType))
                    .ToList();
            }
            else if (hasProductLine && !expandType)
            {
                // 按产品线+产部分组（汇总安灯类型）
                datas = factoryDatas
                    .GroupBy(p => new { p.ProductLine, p.PlantName })
                    .Select(g => BuildStatisticsData(g, g.Key.ProductLine, g.Key.PlantName, ""))
                    .ToList();
            }
            else if (!hasProductLine && expandType)
            {
                // 按产品线+安灯类型分组
                datas = factoryDatas
                    .GroupBy(p => new { p.ProductLine, p.AndonType })
                    .Select(g => BuildStatisticsData(g, g.Key.ProductLine, "", g.Key.AndonType))
                    .ToList();
            }
            else
            {
                // 默认：按产品线分组（汇总产部和安灯类型）
                datas = factoryDatas
                    .GroupBy(p => new { p.ProductLine })
                    .Select(g => BuildStatisticsData(g, g.Key.ProductLine, "", ""))
                    .ToList();
            }

            return datas;
        }

        /// <summary>
        /// 将工厂明细数据按分组汇总为安灯统计报表数据
        /// </summary>
        private AndonStatisticsReportData BuildStatisticsData(IEnumerable<AndonStatisticsReportData> items, string productLine, string plantName, string andonType)
        {
            var list = items.ToList();
            var totalAndonCount = list.Count;

            //前面是按分钟统计，这里再转换成小时
            var data = new AndonStatisticsReportData();
            data.ProductLine = productLine;
            data.PlantName = plantName;
            data.AndonType = andonType;
            data.AbnormalDowntime = Math.Round(list.Sum(x => x.AbnormalDowntime) / 60, 2);
            data.AndonCount = totalAndonCount;
            data.AvgResponseTime = Math.Round(list.Sum(x => x.AvgResponseTime) / 60 / totalAndonCount, 2);
            data.AvgHandleTime = Math.Round(list.Sum(x => x.AvgHandleTime) / 60 / totalAndonCount, 2);
            data.AvgCheckTime = Math.Round(list.Sum(x => x.AvgCheckTime) / 60 / totalAndonCount, 2);
            //超时
            var overTimeList = list.Where(p => p.OverTime != null).ToList();
            data.Over30Min = overTimeList.Where(p => p.OverTime >= 30 && p.OverTime < 60).Count();
            data.Over60Min = overTimeList.Where(p => p.OverTime >= 60 && p.OverTime < 120).Count();
            data.Over120Min = overTimeList.Where(p => p.OverTime >= 120 && p.OverTime < 240).Count();
            data.Over240Min = overTimeList.Where(p => p.OverTime >= 240).Count();
            return data;
        }
        #endregion
    }
}
