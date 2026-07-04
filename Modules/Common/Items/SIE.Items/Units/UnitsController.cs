using SIE.Domain;
using SIE.Items.Items;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Items.Units
{
    /// <summary>
    /// 单位控制器
    /// </summary>
    public class UnitsController : DomainController
    {
        /// <summary>
        /// 根据单位编码获取单位
        /// </summary>
        /// <param name="code">单位编码</param>
        /// <returns>返回单位</returns>
        public virtual Unit GetUnitFromCode(string code)
        {
            Check.NotNullOrEmpty(code, nameof(code));
            return Query<Unit>().Where(p => p.Code == code.ToUpper()).FirstOrDefault();
        }

        /// <summary>
        /// 根据单位名称获取单位
        /// </summary>
        /// <param name="name">单位名称</param>
        /// <returns>返回单位</returns>
        public virtual Unit GetUnitFromName(string name)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            return Query<Unit>().Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 获取单位列表
        /// </summary>
        /// <param name="unitType">单位类型</param>
        /// <returns>返回单位列表</returns>
        public virtual EntityList<Unit> GetUnitList(string unitType)
        {
            var query = Query<Unit>();
            if (!string.IsNullOrEmpty(unitType))
            {
                query.Where(x => x.Type == unitType);
            }

            return query.ToList();
        }

        /// <summary>
        /// 获取单位列表
        /// </summary>
        /// <param name="ids">单位类型</param>
        /// <returns>返回单位列表</returns>
        public virtual EntityList<Unit> GetUnitList(List<double> ids)
        {
            return Query<Unit>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过单位编码列表 获取单位列表(忽略库存组织）
        /// </summary>
        /// <param name="codes">单位编码列表</param>
        /// <returns>单位列表</returns>
        public virtual EntityList<Unit> GetUnitList(List<string> codes)
        {

            return codes.SplitContains((tmpCodes) =>
            {
                return Query<Unit>().Where(p => tmpCodes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

        }

        /// <summary>
        /// 获取是否初始化单位数据
        /// </summary>
        /// <param name="isInit">是否初始化</param>
        /// <returns>单位数据</returns>
        public virtual EntityList<Unit> GetUnitList(bool isInit)
        {
            return Query<Unit>().Where(p => p.IsInit == isInit).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取是否初始化单位数据
        /// </summary>        
        /// <returns>单位数据</returns>
        public virtual bool CheckInitUnitList()
        {
            return Query<Unit>().Where(p => p.IsInit == true).Count() > 0;
        }

        /// <summary>
        /// 通过单位名称列表
        /// </summary>
        /// <param name="names">单位名称列表</param>
        /// <returns>单位列表</returns>
        public virtual EntityList<Unit> GetUnits(List<string> names)
        {
            return Query<Unit>().Where(p => names.Contains(p.Name)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存单位转换数据
        /// </summary>
        public virtual void SaveItemUnit()
        {
            EntityList<ItemUnit> itemUnits = new EntityList<ItemUnit>();
            List<string> mainUnitNames = new List<string>
            {
                "千米", "千米", "千米", "千米",
                "米", "米", "米",
                "分米", "厘米",
                "平方米", "平方米", "平方分米",
                "立方米", "立方米", "立方米", "立方米", "立方分米", "立方分米", "立方分米",
                "立方厘米", "立方厘米",
                "升",
                "吨", "吨",
                "千克"
            };
            List<string> unitNames = new List<string>
            {
                "米", "分米", "厘米", "毫米",
                "分米", "厘米", "毫米",
                "厘米", "毫米",
                "平方分米", "平方厘米", "平方厘米",
                "立方分米", "立方厘米",
                "升", "毫升",
                "立方厘米", "升", "毫升",
                "升", "毫升",
                "毫升",
                "千克", "克", "克"
            };
            List<int> numerators = new List<int>
            {
                1000, 10000, 100000, 1000000, 10,
                100, 1000, 10, 10, 100,
                10000, 100, 1000, 1000000, 1000,
                1000000, 1000, 1, 1000,
                1, 1, 1000, 1000, 1000000, 1000
            };
            List<int> denominators = new List<int>
            {
                1, 1, 1, 1, 1,
                1, 1, 1, 1, 1,
                1, 1, 1, 1, 1,
                1, 1, 1, 1, 1000,
                1, 1, 1, 1, 1
            };
            for (int i = 0; i < mainUnitNames.Count; i++)
            {
                ItemUnit itemUnit = new ItemUnit()
                {
                    MainUnitId = GetUnitFromName(mainUnitNames[i]).Id,
                    UnitId = GetUnitFromName(unitNames[i]).Id,
                    Numerator = numerators[i],
                    Denominator = denominators[i],
                    UnitSource = UnitSource.BaseUnit,
                    IsBaseUnit = true,
                    IsInit = true
                };
                itemUnits.Add(itemUnit);
                ItemUnit itemUnittem = new ItemUnit()
                {
                    MainUnitId = GetUnitFromName(unitNames[i]).Id,
                    UnitId = GetUnitFromName(mainUnitNames[i]).Id,
                    Numerator = denominators[i],
                    Denominator = numerators[i],
                    UnitSource = UnitSource.BaseUnit,
                    IsBaseUnit = true,
                    IsInit = true
                };
                itemUnits.Add(itemUnittem);
            }
            RF.Save(itemUnits);
        }

        /// <summary>
        /// 保存单位数据
        /// </summary>
        public virtual void SaveUnit()
        {
            EntityList<Unit> units = new EntityList<Unit>();
            List<string> codes = new List<string>
                {
                    "km", "m", "dm", "cm", "mm",
                    "㎡", "d㎡", "c㎡",
                    "m³", "dm³", "cm³",
                    "L", "mL",
                    "t", "kg", "g"
                };
            List<string> names = new List<string>
                {
                    "千米", "米", "分米", "厘米", "毫米", "平方米", "平方分米", "平方厘米",
                    "立方米", "立方分米", "立方厘米", "升", "毫升", "吨", "千克", "克"
                };
            List<string> types = new List<string>
                {
                    "长度单位", "长度单位", "长度单位", "长度单位", "长度单位",
                    "面积单位", "面积单位", "面积单位",
                    "体积单位", "体积单位", "体积单位", "体积单位", "体积单位",
                    "重量单位", "重量单位", "重量单位"
                };
            List<int> precisions = new List<int>
                {
                    8, 5, 4, 3, 2, 6, 4, 2, 8, 5, 2, 5, 2, 8, 5, 2
                };
            var unitsAll = RF.GetAll<Unit>();
            for (int i = 0; i < codes.Count; i++)
            {
                var code = codes[i];
                var unitExist = unitsAll.FirstOrDefault(f => f.Code.ToUpper() == code.ToUpper());
                if (!unitsAll.Any(f => f.Code.ToUpper() == code.ToUpper()))
                {
                    Unit unit = new Unit()
                    {
                        Code = codes[i],
                        Name = names[i],
                        Type = types[i],
                        Precision = precisions[i],
                        UnitSource = UnitSource.BaseUnit,
                        IsInit = true
                    };
                    units.Add(unit);
                }
                else
                {
                    unitExist.IsInit = true;
                    unitExist.Name = names[i];
                    unitExist.Type = types[i];
                    unitExist.Precision = precisions[i];
                    unitExist.UnitSource = UnitSource.BaseUnit;
                    RF.Save(unitExist);
                }
            }
            RF.Save(units);
        }


        /// <summary>
        /// 初始化单位
        /// </summary>
        public virtual void InsertUnitList()
        {
            using (var tran = DB.TransactionScope(ItemEntityDataProvider.ConnectionStringName))
            {
                SaveUnit();
                SaveItemUnit();
                tran.Complete();
            }
        }
    }
}
