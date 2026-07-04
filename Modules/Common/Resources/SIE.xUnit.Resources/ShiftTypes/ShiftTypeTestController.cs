using SIE.Domain;
using SIE.Resources.ShiftTypes;
using System;

namespace SIE.xUnit.Resources.ShiftTypes
{
    /// <summary>
    /// 班制测试控制器
    /// </summary>
    public class ShiftTypeTestController : DomainController
    {
        /// <summary>
        /// 根据班制编号获取班制信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual ShiftType GetShiftType(string code)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(ShiftType.ShiftListProperty);
            elo.LoadWith(Shift.ShiftRestListProperty);

            var query = Query<ShiftType>().Where(p => p.Code == code);

            return query.FirstOrDefault(elo);
        }

        /// <summary>
        /// 创建班制基础信息
        /// </summary>
        /// <param name="isDefault">是否新建默认的数据</param>
        /// <returns></returns>
        public virtual ShiftType CreateShiftType(bool isDefault)
        {
            string defaultCode = "白班";
            ShiftType shiftType = null;
            if (isDefault)
            {
                shiftType = GetShiftType(defaultCode);
            }

            if (shiftType == null)
            {
                // 创建班制
                shiftType = new ShiftType();
                shiftType.GenerateId();
                shiftType.Code = isDefault ? defaultCode : defaultCode + shiftType.Id.ToString();
                shiftType.Name = shiftType.Code;

                // 创建班次
                Shift shift = new Shift();
                shift.GenerateId();
                shift.ShiftTypeId = shiftType.Id;
                shiftType.ShiftList.Add(shift);
                shift.Code = "单班";
                shift.Name = shift.Code;
                shift.BeginTime = DateTime.Now.Date.AddHours(8);
                shift.EndTime = DateTime.Now.Date.AddHours(18);

                // 创建班次休息
                ShiftRest shiftRest = new ShiftRest();
                shiftRest.GenerateId();
                shiftRest.ShiftId = shift.Id;
                shift.ShiftRestList.Add(shiftRest);
                shiftRest.Type = "午休";
                shiftRest.BeginTime = DateTime.Now.Date.AddHours(12);
                shiftRest.EndTime = DateTime.Now.Date.AddHours(13.5);
            }

            return shiftType;
        }
    }
}