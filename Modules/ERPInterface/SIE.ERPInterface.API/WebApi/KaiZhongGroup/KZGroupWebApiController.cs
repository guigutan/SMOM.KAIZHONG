using Newtonsoft.Json;
using SIE.Api;
using SIE.Domain;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.Security;
using SIE.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Api.WebApi.KaiZhongGroup
{
    public class KZGroupWebApiController : KzLoginController
    {
     
        private static readonly object _lockObj = new object();

        /// <summary>
        /// 保存物料标签信息数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("保存工厂信息数据")]
        public virtual OuterSystemRetVO SaveEnterpriseData(List<EnterpriseData> datas)
        {
            Login();
            RT.InvOrg = 1;
            var ctl = RT.Service.Resolve<KzGroupBaseDateInfController>();
            string dataJsons = JsonConvert.SerializeObject(datas);
            //return ctl.distributeMd(null, InfType.Skill.ToLabel(), null, dataJsons, null);
            ToasyncSave(InfType.Enterprise, dataJsons, null);
            OuterSystemRetVO vO = new OuterSystemRetVO();
            return vO;
        }

        /// <summary>
        /// 保存物料标签信息数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("保存物料标签信息数据")]
        public virtual OuterSystemRetVO SaveItemLabelDatas(List<ItemLabelData> datas)
        {
            Login();
            RT.InvOrg = 1;
            var ctl = RT.Service.Resolve<KzGroupBaseDateInfController>();
            //将传过来的数据，按照工厂进行分组
            var dic = datas.GroupBy(p => p.WERKS).ToDictionary(p => p.Key, p => p.ToList());
            OuterSystemRetVO vO = new OuterSystemRetVO();
            foreach (var d in dic)
            {
                string dataJsons = JsonConvert.SerializeObject(d.Value);
                //var result = ctl.distributeMd(null, InfType.ItemLabel.ToLabel(), null, dataJsons, d.Key);
                //vO.Add(result);
                ToasyncSave(InfType.ItemLabel, dataJsons, d.Key);

            }
            return vO;
        }

        /// <summary>
        /// 保存人员技能信息数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("保存人员技能信息数据")]
        public virtual OuterSystemRetVO SaveSkillData(List<SkillData> datas)
        {
            Login();
            RT.InvOrg = 1;
            var ctl = RT.Service.Resolve<KzGroupBaseDateInfController>();
            string dataJsons = JsonConvert.SerializeObject(datas);
            //return ctl.distributeMd(null, InfType.Skill.ToLabel(), null, dataJsons, null);
            ToasyncSave(InfType.Skill, dataJsons, null);
            OuterSystemRetVO vO = new OuterSystemRetVO();
            return vO;
        }

        /// <summary>
        /// 保存设备信息数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("保存设备信息数据")]
        public virtual OuterSystemRetVO SaveEquipAccount(List<EquipAccountData> datas)
        {
            Login();
            RT.InvOrg = 1;
            var ctl = RT.Service.Resolve<KzGroupBaseDateInfController>();
            //将传过来的数据，按照工厂进行分组
            var dic = datas.GroupBy(p => p.SWERK).ToDictionary(p => p.Key, p => p.ToList());
            OuterSystemRetVO vO = new OuterSystemRetVO();
            foreach (var d in dic)
            {
                string dataJsons = JsonConvert.SerializeObject(d.Value);
                //var result = ctl.distributeMd(null, InfType.EquipAccount.ToLabel(), null, dataJsons, d.Key);
                //vO.Add(result);
                ToasyncSave(InfType.EquipAccount, dataJsons, d.Key);

            }
            return vO;
        }

        /// <summary>
        /// 保存工单信息数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("保存工单信息数据")]
        public virtual OuterSystemRetVO SaveWorkOrder(WorkOrderData data)
        {
            Login();
            RT.InvOrg = 1;
            var ctl = RT.Service.Resolve<KzGroupBaseDateInfController>();

            OuterSystemRetVO vO = new OuterSystemRetVO();

            //把数据拆分成一条工单一条工单，因为这个他们这边同一个工单，但是可能存在不同工单下的工序可能是其他库存组织的，然后这些库存组织都创建一条一摸一样的工单，基于这个条件，以及数据结构，所以把下载下来的多条工单数据手动拆分成一条一条的工单数据，根据库存组织去创建
            foreach (var d in data.workOrderInfs)
            {
                //当状态不等于REL和TECO的时候，直接跳过，我们不接
                if (d.STATU != "REL" && d.STATU != "TECO")
                    continue;
                var layoutInfs = data.layoutInfs.Where(p => p.AUFNR == d.AUFNR).ToList();
                var bomInfs = data.bomInfs.Where(p => p.AUFNR == d.AUFNR).ToList();
                var factorys = layoutInfs.Select(p => p.WERKS).Distinct().ToList();
                factorys.Add(d.WERKS);
                factorys = factorys.Distinct().ToList();
                foreach (var factory in factorys)
                {
                    WorkOrderData orderData = new WorkOrderData();
                    orderData.workOrderInfs.Add(d);
                    orderData.layoutInfs.AddRange(layoutInfs);
                    orderData.bomInfs.AddRange(bomInfs);
                    orderData.parentItemInfs.AddRange(data.parentItemInfs);

                    string dataJsons = JsonConvert.SerializeObject(orderData);
                    //var result = ctl.distributeMd(null, InfType.WorkOrder.ToLabel(), null, dataJsons, factory);
                    //vO.Add(result);
                    ToasyncSave(InfType.WorkOrder, dataJsons, factory);

                }
            }
            return vO;
        }

        /// <summary>
        /// 保存工序信息数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("保存工序信息数据")]
        public virtual OuterSystemRetVO SaveProcess(List<ProcessData> datas)
        {
            Login();
            RT.InvOrg = 1;
            var ctl = RT.Service.Resolve<KzGroupBaseDateInfController>();
            string dataJsons = JsonConvert.SerializeObject(datas);
            //return ctl.distributeMd(null, InfType.Process.ToLabel(), null, dataJsons, null);
            ToasyncSave(InfType.Process, dataJsons, null);
            OuterSystemRetVO vO = new OuterSystemRetVO();
            return vO;
        }

        /// <summary>
        /// 保存员工信息数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("保存员工信息数据")]
        public virtual OuterSystemRetVO SaveEmployee(List<EmployeeData> datas)
        {
            Login();
            RT.InvOrg = 1;
            var ctl = RT.Service.Resolve<KzGroupBaseDateInfController>();
            string dataJsons = JsonConvert.SerializeObject(datas);
            //return ctl.distributeMd(null, InfType.Employee.ToLabel(), null, dataJsons, null);
            ToasyncSave(InfType.Employee, dataJsons, null);
            OuterSystemRetVO Vo = new OuterSystemRetVO();
            return Vo;
        }



        /// <summary>
        /// 保存人员组织架构信息数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("保存人员组织架构信息数据")]
        public virtual OuterSystemRetVO SaveOrgLevel(List<SIE.MES.OrgLevels.OrgLevelInfo> datas)
        {
            Login();
            RT.InvOrg = 1;
            RT.Service.Resolve<KzGroupBaseDateInfController>();
            string dataJsons = JsonConvert.SerializeObject(datas);
            ToasyncSave(InfType.OrgLevel, dataJsons, null);
            OuterSystemRetVO Vo = new OuterSystemRetVO();

            return Vo;
        }






        /// <summary>
        /// 保存工作中心数据(其他系统到集团)
        /// </summary>
        /// <param name="datas"></param>
        [AllowAnonymous]
        [ApiService("保存工作中心数据")]
        public virtual OuterSystemRetVO SaveWorkCenter(List<WorkCenterData> datas)
        {
            Login();
            RT.InvOrg = 1;
            var ctl = RT.Service.Resolve<KzGroupBaseDateInfController>();
            //将传过来的数据，按照工厂进行分组
            var dic = datas.GroupBy(p => p.WERKS).ToDictionary(p => p.Key, p => p.ToList());
            OuterSystemRetVO vO = new OuterSystemRetVO();
            foreach (var d in dic)
            {
                string dataJsons = JsonConvert.SerializeObject(d.Value);
                //var result = ctl.distributeMd(null, InfType.WorkCenter.ToLabel(), null, dataJsons, d.Key);
                //vO.Add(result);
                ToasyncSave(InfType.WorkCenter, dataJsons, d.Key);
            }
            return vO;
        }

        /// <summary>
        /// 保存物料数据(其他系统到集团)
        /// </summary>
        /// <param name="datas"></param>
        [AllowAnonymous]
        [ApiService("保存物料数据")]
        public virtual OuterSystemRetVO SaveItem(List<SIE.KZ.Base.Interfaces.Datas.ItemData> datas)
        {
            Login();
            RT.InvOrg = 1;
            //异步执行保存，先让接口返回，无需等待(无论结果如何，他们SAP是不会去看的)
            ToasyncSaveItems(datas);
            OuterSystemRetVO vO = new OuterSystemRetVO();
            return vO;
        }

        /// <summary>
        /// 异步执行，先让接口返回，无需等待
        /// </summary>
        /// <param name="datas"></param>
        private async static void ToasyncSaveItems(List<SIE.KZ.Base.Interfaces.Datas.ItemData> datas)
        {
            await Task.Run(new Action(() =>
            {
                var ctl = RT.Service.Resolve<KzGroupBaseDateInfController>();
                //将传过来的数据，按照工厂进行分组
                var dic = datas.GroupBy(p => p.WERKS).ToDictionary(p => p.Key, p => p.ToList());

                foreach (var d in dic)
                {
                    string dataJsons = JsonConvert.SerializeObject(d.Value);
                    var result = ctl.distributeMd(null, InfType.Item.ToLabel(), null, dataJsons, d.Key);
                }
            }).WithCurrentThreadContext());
        }

        /// <summary>
        /// 下载分类(其他接口传给集团)
        /// </summary>
        /// <param name="datas"></param>
        [AllowAnonymous]
        [ApiService("保存分类(凯中称为:物料组)数据")]
        public virtual OuterSystemRetVO SaveItemCategory(List<SIE.KZ.Base.Interfaces.Datas.ItemCategoryData> datas)
        {
            Login();
            RT.InvOrg = 1;
            var ctl = RT.Service.Resolve<KzGroupBaseDateInfController>();
            string dataJsons = JsonConvert.SerializeObject(datas);
            //return ctl.distributeMd(null, InfType.ItemCategory.ToLabel(), null, dataJsons, null);
            ToasyncSave(InfType.ItemCategory, dataJsons, null);
            OuterSystemRetVO vO = new OuterSystemRetVO();
            return vO;
        }

        /// <summary>
        /// 保存蓝标数据
        /// </summary>
        /// <param name="datas"></param>
        [AllowAnonymous]
        [ApiService("保存蓝标数据")]
        public virtual OuterSystemRetVO SaveBlueLabel(List<BlueLabelData> datas)
        {
            Login();
            RT.InvOrg = 1;
            //将传过来的数据，按照工厂进行分组
            var dic = datas.GroupBy(p => p.WERKS).ToDictionary(p => p.Key, p => p.ToList());
            OuterSystemRetVO vO = new OuterSystemRetVO();
            foreach (var d in dic)
            {
                string dataJsons = JsonConvert.SerializeObject(d.Value);
                //var result = ctl.distributeMd(null, InfType.WorkCenter.ToLabel(), null, dataJsons, d.Key);
                //vO.Add(result);
                ToasyncSave(InfType.BlueLabel, dataJsons, d.Key);
            }
            return vO;
            //var ctl = RT.Service.Resolve<KzGroupBaseDateInfController>();
            //string dataJsons = JsonConvert.SerializeObject(datas);
            //return ctl.distributeMd(null, InfType.BlueLabel.ToLabel(), null, dataJsons, null);
            //OuterSystemRetVO vO = new OuterSystemRetVO();
            //return vO;
        }

        /// <summary>
        /// 设备台账查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("设备台账查询")]
        public virtual List<EquipmentData> GetEquipmentDatas(EquipmentQuery query)
        {
            Login();
            RT.InvOrg = 1;
            List<EquipmentData> datas = new List<EquipmentData>();
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                var equipDatas = Query<SIE.Equipments.EquipAccounts.EquipAccount >();
                if (query.beginTime != null)
                {
                    equipDatas.Where(p => p.CreateDate >= query.beginTime);
                }
                if (query.endTime != null)
                {
                    equipDatas.Where(p => p.CreateDate <= query.endTime);
                }
               var equipDataList= equipDatas.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                foreach (var item in equipDataList)
                {
                    EquipmentData equipment = new EquipmentData();
                    equipment.EquipName = item.Name;
                    equipment.EquipCode = item.Code;
                    equipment.FunctionalLocation=item.FunctionalLocation;
                    equipment.FactoryName = item.FactoryName;
                    equipment.Manufacturer = item.Manufacturer;
                    datas.Add(equipment);
                }
            }

                return datas;
        }


        /// <summary>
        /// 更新报工回传结果
        /// </summary>
        /// <param name="datas"></param>
        [AllowAnonymous]
        [ApiService("更新报工回传结果")]
        public virtual OuterSystemRetVO UpadteReportResult(List<ReportReturnData> datas)
        {
            Login();
            RT.InvOrg = 1;
            var ctl = RT.Service.Resolve<KzGroupBaseDateInfController>();
            var dic = datas.GroupBy(p => p.WERKS).ToDictionary(p => p.Key, p => p.ToList());
            OuterSystemRetVO vO = new OuterSystemRetVO();
            foreach (var d in dic)
            {
                string dataJsons = JsonConvert.SerializeObject(d.Value);
                ToasyncSave(InfType.ReportResult, dataJsons, d.Key);
            }

            return vO;
        }

        /// <summary>
        /// OA流程退回
        /// </summary>
        /// <param name="datas"></param>
        [AllowAnonymous]
        [ApiService("OA流程退回")]
        public virtual OuterSystemRetVO OAFlowReturn(List<OAFlowReturnData> datas)
        {
            Login();
            RT.InvOrg = 1;
            OuterSystemRetVO vO = new OuterSystemRetVO();
            foreach (var d in datas.GroupBy(p=>p.INITIATORFACTORY))
            {
                string dataJsons = JsonConvert.SerializeObject(d.ToList());
                ToasyncSave(InfType.OAFlowReturn, dataJsons, d.Key);

            }
            return vO;
        }

        /// <summary>
        /// 可疑品阈值下载
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("可疑品阈值下载")]
        public virtual OuterSystemRetVO SaveThreshold(List<ThresholdData> datas)
        {
            OuterSystemRetVO vO = new OuterSystemRetVO();
            vO.success = true;
            vO.errorMsg = null;
            foreach (var d in datas.GroupBy(p => p.WERKS))
            {
                string dataJsons = JsonConvert.SerializeObject(d.ToList());
                var result = RT.Service.Resolve<KzGroupBaseDateInfController>().distributeMd(null, InfType.Threshold.ToLabel(), null, dataJsons, d.Key);
                vO.Add(result);
                if(result.success == false)
                    vO.success = false;
            }
            return vO;
        }

        /// <summary>
        /// 产线与安灯区域下载
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("产线与安灯区域下载")]
        public virtual OuterSystemRetVO SaveAndonLine(List<AndonLineData> datas)
        {
            OuterSystemRetVO vO = new OuterSystemRetVO();
            vO.success = true;
            vO.errorMsg = null;

            foreach (var d in datas.GroupBy(p => p.WERKS))
            {
                string dataJsons = JsonConvert.SerializeObject(d.ToList());
                var result = RT.Service.Resolve<KzGroupBaseDateInfController>().distributeMd(null, InfType.AndonLine.ToLabel(), null, dataJsons, d.Key);
                vO.Add(result);
                if (result.success == false)
                    vO.success = false;
            }
            return vO;
        }

        /// <summary>
        /// 产品与产线的关系下载
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("产品与产线的关系下载")]
        public virtual OuterSystemRetVO SaveProductLine(List<ProductLineData> datas)
        {
            OuterSystemRetVO vO = new OuterSystemRetVO();
            vO.success = true;
            vO.errorMsg = null;

            foreach (var d in datas.GroupBy(p => p.WERKS))
            {
                string dataJsons = JsonConvert.SerializeObject(d.ToList());
                var result = RT.Service.Resolve<KzGroupBaseDateInfController>().distributeMd(null, InfType.ProductLine.ToLabel(), null, dataJsons, d.Key);
                vO.Add(result);
                if (result.success == false)
                    vO.success = false;
            }
            return vO;
        }

        /// <summary>
        /// 工装维护下载
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("工装维护下载")]
        public virtual OuterSystemRetVO SaveFixtureUphold(List<FixtureUpholdData> datas)
        {
            OuterSystemRetVO vO = new OuterSystemRetVO();
            vO.success = true;
            vO.errorMsg = null;

            foreach (var d in datas.GroupBy(p => p.WERKS))
            {
                string dataJsons = JsonConvert.SerializeObject(d.ToList());
                var result = RT.Service.Resolve<KzGroupBaseDateInfController>().distributeMd(null, InfType.FixtureUphold.ToLabel(), null, dataJsons, d.Key);
                vO.Add(result);
                if (result.success == false)
                    vO.success = false;
            }
            return vO;
        }

        /// <summary>
        /// 工装与产品的关系下载
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("工装与产品的关系下载")]
        public virtual OuterSystemRetVO SaveFixtureItem(List<FixtureItemData> datas)
        {
            OuterSystemRetVO vO = new OuterSystemRetVO();
            vO.success = true;
            vO.errorMsg = null;

            foreach (var d in datas.GroupBy(p => p.WERKS))
            {
                string dataJsons = JsonConvert.SerializeObject(d.ToList());
                var result = RT.Service.Resolve<KzGroupBaseDateInfController>().distributeMd(null, InfType.FixtureItem.ToLabel(), null, dataJsons, d.Key);
                vO.Add(result);
                if (result.success == false)
                    vO.success = false;
            }
            return vO;
        }

        /// <summary>
        /// 检具维护下载
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("检具维护下载")]
        public virtual OuterSystemRetVO SaveCheckerUphold(List<CheckerUpholdData> datas)
        {
            OuterSystemRetVO vO = new OuterSystemRetVO();
            vO.success = true;
            vO.errorMsg = null;

            foreach (var d in datas.GroupBy(p => p.WERKS))
            {
                string dataJsons = JsonConvert.SerializeObject(d.ToList());
                var result = RT.Service.Resolve<KzGroupBaseDateInfController>().distributeMd(null, InfType.CheckerUphold.ToLabel(), null, dataJsons, d.Key);
                vO.Add(result);
                if (result.success == false)
                    vO.success = false;
            }
            return vO;
        }

        /// <summary>
        /// 检具与产品的关系下载
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("检具与产品的关系下载")]
        public virtual OuterSystemRetVO SaveCheckerItem(List<CheckerItemData> datas)
        {
            OuterSystemRetVO vO = new OuterSystemRetVO();
            vO.success = true;
            vO.errorMsg = null;

            foreach (var d in datas.GroupBy(p => p.WERKS))
            {
                string dataJsons = JsonConvert.SerializeObject(d.ToList());
                var result = RT.Service.Resolve<KzGroupBaseDateInfController>().distributeMd(null, InfType.CheckerItem.ToLabel(), null, dataJsons, d.Key);
                vO.Add(result);
                if (result.success == false)
                    vO.success = false;
            }
            return vO;
        }

        /// <summary>
        /// 模具与产品的关系
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("模具与产品的关系")]
        public virtual OuterSystemRetVO SaveEquipAccountItem(List<EquipAccountItemData> datas)
        {
            OuterSystemRetVO vO = new OuterSystemRetVO();
            vO.success = true;
            vO.errorMsg = null;

            foreach (var d in datas.GroupBy(p => p.WERKS))
            {
                string dataJsons = JsonConvert.SerializeObject(d.ToList());
                var result = RT.Service.Resolve<KzGroupBaseDateInfController>().distributeMd(null, InfType.EquipAccountItem.ToLabel(), null, dataJsons, d.Key);
                vO.Add(result);
                if (result.success == false)
                    vO.success = false;
            }
            return vO;
        }

        /// <summary>
        /// 返工工艺路线版本
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("返工工艺路线版本")]
        public virtual OuterSystemRetVO SaveRewrokLayoutVersion(Rlvd data)
        {
            Login();
            RT.InvOrg = 1;
            var ctl = RT.Service.Resolve<KzGroupBaseDateInfController>();
            //将传过来的数据，按照工厂进行分组
            OuterSystemRetVO vO = new OuterSystemRetVO();
            //把数据拆分成一条工单一条工单，因为这个他们这边同一个工单，但是可能存在不同工单下的工序可能是其他库存组织的，然后这些库存组织都创建一条一摸一样的工单，基于这个条件，以及数据结构，所以把下载下来的多条工单数据手动拆分成一条一条的工单数据，根据库存组织去创建
            foreach (var d in data.Data1.GroupBy(p=>p.WERKS))
            {
                Rlvd rlvd = new Rlvd();
                rlvd.Data1 = d.ToList();
                string dataJsons = JsonConvert.SerializeObject(rlvd);
                ToasyncSave(InfType.ReworkLayoutVersion, dataJsons, d.Key);
            }

            //var dic = datas.GroupBy(p => p.WERKS).ToDictionary(p => p.Key, p => p.ToList());
            //foreach (var d in dic)
            //{
            //    string dataJsons = JsonConvert.SerializeObject(d.Value);
            //    //var result = ctl.distributeMd(null, InfType.WorkCenter.ToLabel(), null, dataJsons, d.Key);
            //    //vO.Add(result);
            //    ToasyncSave(InfType.ReworkLayoutVersion, dataJsons, d.Key);
            //}
            return vO;

        }

        /// <summary>
        /// 异步执行，防止并发
        /// </summary>
        /// <param name="infType"></param>
        /// <param name="dataJsons"></param>
        /// <param name="WERKS"></param>
        private async static void ToasyncSave(InfType infType,string dataJsons,string WERKS)
        {
            await Task.Run(new Action(() =>
            {
                lock (_lockObj)
                {
                    var ctl = RT.Service.Resolve<KzGroupBaseDateInfController>();

                    var result = ctl.distributeMd(null, infType.ToLabel(), null, dataJsons, WERKS);
                }
            }).WithCurrentThreadContext());
        }

    }
}
