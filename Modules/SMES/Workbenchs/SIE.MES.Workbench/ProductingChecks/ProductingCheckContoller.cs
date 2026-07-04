using SIE.Domain;
using SIE.MES.Workbench.EmployeeManages;
using SIE.MES.Workbench.StationChecks;
using SIE.Resources.Employees;
using SIE.Tech.Processs;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SIE.MES.Workbench.ProductingChecks
{
    /// <summary>
    /// 开班点检控制器
    /// </summary>
    public class ProductingCheckContoller : DomainController
    {
        public virtual List<ProductingCheck> GetProductingChecks(double groupId, DateTime date)
        {
            List<ProductingCheck> checkList = new List<ProductingCheck>();
            var stationList = RT.Service.Resolve<ProcessController>().GetCheckStations();
            var ondutyList = GetStationOnDutys(stationList.Select(p => p.Id).ToArray(), date);  //工位当班情况
            ////var clockingInList = RT.Service.Resolve<EmployeeManageController>().GetEmployeeClockingIns(groupId, date).Select(p => p.EmployeeId).ToList();  //出勤情况 
            ////var groupOnLoans = RT.Service.Resolve<EmployeeController>().GetGroupOnLoans(groupId, null).Select(p => p.EmployeeId);
            ////IEnumerable<double> groupOnLoans = new List<double>();
            var controller = RT.Service.Resolve<StationCheckController>();
            int index = 1;
            int count = stationList.Count >= 4 ? 4 : stationList.Count;
            foreach (var station in stationList.Take(4))
            {
                // TODO:是否关键工位被屏蔽了
                var checkResultList = controller.GetStationCheckResults(station.Id, date).GroupBy(p => p.CheckType).ToDictionary(p => p.Key, p => p.ToList());
                var check = new ProductingCheck() { StationId = station.Id, StationName = station.Name, Image = "{0}.png".FormatArgs(index), IsLastStation = index == count, IsKeyStation = true /*station.IsKey*/, GroupId = groupId };
                index++;
                var ondutyInfo = ondutyList.FirstOrDefault(p => p.StationId == station.Id);
                if (ondutyInfo != null)
                {
                    var ondutyEmp = ondutyInfo.OnDuty;
                    if (ondutyEmp != null)
                        check.OnDuty = CreateEmployeeInfo(ondutyEmp);
                    var actualOnDutyEmp = ondutyInfo.ActualOnDuty;
                    if (actualOnDutyEmp != null)
                        check.ActualOnDuty = CreateEmployeeInfo(actualOnDutyEmp);
                }
                var checkItemList = controller.GetCheckItemList(station.Id, null);
                var checkEquipmentList = controller.GetCheckEquipmentList(station.Id, null);
                GetCheckEquipment(check, checkResultList, checkEquipmentList);
                GetCheckItemList(check, checkResultList, checkItemList);
                checkList.Add(check);
            }
            return checkList;
        }

        private void GetCheckItemList(ProductingCheck check, Dictionary<CheckType, List<StationCheckResult>> checkResultList, EntityList<CheckItem> checkItemList)
        {
            ObservableCollection<CheckItemResult> itemCheckList = check.ItemCheckList;
            List<StationCheckResult> resultList = new List<StationCheckResult>();
            if (checkResultList.ContainsKey(CheckType.Item))
                resultList.AddRange(checkResultList[CheckType.Item]);
            int row = 1;
            ValidationIsChecked(check, checkItemList, resultList);
            checkItemList.ForEach(e =>
            {
                var itemResult = new CheckItemResult()
                {
                    Id = e.Id,
                    ArriveQty = e.ArriveQty,
                    DemandQty = e.DemandQty,
                    InRouteQty = e.InRouteQty,
                    ItemCode = e.Item.Code,
                    ItemName = e.Item.Name,
                    LackQty = e.LackQty,
                    State = EnumViewModel.EnumToLabel(e.State).L10N(),
                    StationId = e.StationId,
                    WarnQty = e.WarnQty,
                    RowNo = row
                };
                row++;
                itemCheckList.Add(itemResult);
            });
        }

        void ValidationIsChecked(ProductingCheck check, EntityList<CheckItem> checkItemList, List<StationCheckResult> resultList)
        {
            bool? isChecked = null;
            var detailIds = checkItemList.Select(p => p.Id).ToArray();
            resultList.ForEach(e =>
            {
                if (detailIds.Contains(e.CheckItemId) && !e.IsCheck)
                {
                    isChecked = false;
                }
            });
            if (isChecked == null && checkItemList.Count == resultList.Count)
            {
                isChecked = true;
            }
            check.ItemCheckResult = isChecked;
        }

        void ValidationIsChecked(ProductingCheck check, List<StationCheckResult> resultList, CheckEquipment equipment)
        {
            bool? isChecked = null;
            var detailIds = equipment.CheckEquipmentDetail.Select(p => p.Id).ToArray(); //待点检项ID 
            resultList.ForEach(e =>
            {
                if (detailIds.Contains(e.CheckItemId) && !e.IsCheck)
                {
                    isChecked = false;
                }
            });
            if (isChecked == null && equipment.CheckEquipmentDetail.Count == resultList.Count)
            {
                isChecked = true;
            }
            check.EquipmentCheckResult = isChecked;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="check">开班点检信息</param>
        /// <param name="checkResultList">工位点检结果</param>
        /// <param name="checkEquipmentList">工位待点检设备</param>
        private void GetCheckEquipment(ProductingCheck check, Dictionary<CheckType, List<StationCheckResult>> checkResultList, EntityList<CheckEquipment> checkEquipmentList)
        {
            List<StationCheckResult> resultList = new List<StationCheckResult>();
            if (checkResultList.ContainsKey(CheckType.Equipment))
                resultList.AddRange(checkResultList[CheckType.Equipment]);
            var result = new CheckEquipmentResult();
            if (checkEquipmentList.Count == 0)
                return;
            var equipment = checkEquipmentList.FirstOrDefault();
            result.Id = equipment.Id;
            result.Code = equipment.Code;
            result.Name = equipment.Name;
            result.State = EnumViewModel.EnumToLabel(equipment.State).L10N();
            result.Period = "{0}天".FormatArgs(equipment.Period);
            result.LastUpkeepTime = DateTime.Today.AddDays(-1).ToString("D");
            ValidationIsChecked(check, resultList, equipment);
            equipment.CheckEquipmentDetail.ForEach(e =>
            {
                var detail = new CheckEquipmentResultDetail()
                {
                    Id = e.Id,
                    Code = e.Project.Code,
                    Name = e.Project.Name
                };
                var checkResult = resultList.FirstOrDefault(p => p.CheckItemId == e.Id);
                if (checkResult != null)
                {
                    detail.Result = checkResult.IsCheck;
                }
                result.DetailList.Add(detail);
            });
            check.CheckEquipment = result;
        }

        private EmployeeInfo CreateEmployeeInfo( Employee ondutyEmp)
        {
            // TODO：类型、资质认证
            var employee = new EmployeeInfo() { Id = ondutyEmp.Id, No = ondutyEmp.Code, Name = ondutyEmp.Name, Photo = ondutyEmp.Photo, Type = "新手",/*EnumViewModel.EnumToLabel(ondutyEmp.Type)*/ Aptitudes = "成品检验"/*EnumViewModel.EnumToLabel(ondutyEmp.Aptitudes)*/ };
            ////var isOnloan = groupOnLoans.Contains(ondutyEmp.Id) ? true : false;
            ////employee.ShortType = isOnloan ? "借" : GetShortType(Resources.Employees.Type.Newbie /* ondutyEmp.Type*/);
            ////employee.IsAbsenteeism = clockingInList.Contains(employee.Id) ? false : true;
            ////employee.IsOnLoan = isOnloan;
            return employee;
        }

        ////private string GetShortType(Resources.Employees.Type type)
        ////{
        ////    if (type == Resources.Employees.Type.Newbie)
        ////        return "新";
        ////    if (type == Resources.Employees.Type.Skilled)
        ////        return "熟";
        ////    else
        ////        return "多";
        ////}

        /// <summary>
        /// 取工位当班人员
        /// </summary>
        /// <param name="stationIds"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public virtual EntityList<StationOnDuty> GetStationOnDutys(double[] stationIds, DateTime date)
        {
            return Query<StationOnDuty>().Where(p => stationIds.Contains(p.StationId) && p.OnDutyDate == date).ToList();
        }
    }
}
