
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.MES.EmpWork;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.LineAndon
{
    /// <summary>
    /// 产线与安灯区域控制器
    /// </summary>
    public class AndonLineController : DomainController
    {
        #region 产线区域维护

        /// <summary>
        /// 根据产线/机台编码获取产线区域维护
        /// </summary>
        /// <param name="machineCodes"></param>
        /// <returns></returns>
        public virtual EntityList<LineArea> GetLineAreasByMachineCodes(List<string> machineCodes)
        {
            var list = machineCodes.SplitContains(codes =>
            {
                return Query<LineArea>().Where(p => codes.Contains(p.MachineCode)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 更新产线与安灯区域
        /// </summary>
        /// <param name="lineArea"></param>
        public virtual void UpdateAndonLine(LineArea data)
        {
            var andonLine = Query<AndonLine>().Where(p => p.MachineCode == data.MachineCode).FirstOrDefault();
            if (andonLine == null)
            {
                andonLine = new AndonLine();
            }
            andonLine.MachineCode = data.MachineCode;
            andonLine.MachineName = data.MachineName;
            andonLine.Equipment = data.Equipment;
            andonLine.WorkCenter = data.WorkCenter;
            andonLine.Factory = data.Factory;
            andonLine.WorkShop = data.WorkShop;
            andonLine.AndonUphold = data.AndonUphold;
            andonLine.AndonCode = data.AndonCode;
            RF.Save(andonLine);
        }

        /// <summary>
        /// 查询方式
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<LineArea> CriteriaLineArea(LineAreaCriteria criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("产线与安灯区域查询实体异常！".L10N());
            }
            var q = Query<LineArea>();
            if (criterial.WorkCenterId.HasValue)
            {
                q.Where(p => p.WorkCenterId == criterial.WorkCenterId);
            }
            if (criterial.WorkShopId.HasValue)
            {
                q.Where(p => p.WorkShopId == criterial.WorkShopId);
            }
            if (criterial.FactoryId.HasValue)
            {
                q.Where(p => p.FactoryId == criterial.FactoryId);
            }
            if (criterial.AndonUpholdId.HasValue)
            {
                q.Where(p => p.AndonUpholdId == criterial.AndonUpholdId);
            }
            if (!criterial.AndonCode.IsNullOrEmpty())
            {
                q.Where(m => m.AndonCode.Contains("%" + criterial.AndonCode + "%"));
            }
            if (!criterial.MachineCode.IsNullOrEmpty())
            {
                q.Where(m => m.MachineCode.Contains("%" + criterial.MachineCode + "%"));
            }
            if (!criterial.MachineName.IsNullOrEmpty())
            {
                q.Where(m => m.MachineName.Contains("%" + criterial.MachineName + "%"));
            }
            if (!criterial.EquipmentNo.IsNullOrEmpty())
            {
                q.Where(m => m.Equipment.Code.Contains("%" + criterial.EquipmentNo + "%"));
            }
            if (criterial.EquipmentDate.BeginValue != null)
            {
                q.Where(p => p.Equipment.PurchaseDate >= criterial.EquipmentDate.BeginValue);
            }
            if (criterial.EquipmentDate.EndValue != null)
            {
                q.Where(p => p.Equipment.PurchaseDate <= criterial.EquipmentDate.EndValue);
            }
            if (!criterial.WorkShopCode.IsNullOrEmpty())
                q.Exists<Enterprise>((a, b) => b.Where(p => p.Id == a.WorkShopId).WhereIf(criterial.WorkShopCode.IsNotEmpty(), p => p.Code == criterial.WorkShopCode));

            return q.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        #endregion

        /// <summary>
        /// 根据工作中心编码获取产线与安灯区域
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public virtual EntityList<AndonLine> GetAndonLinesByName(List<string> codes)
        {
            var list = codes.SplitContains(ns =>
            {
                return Query<AndonLine>().Where(p => ns.Contains(p.WorkCenter.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据编码获取产线
        /// </summary>
        /// <param name="machineCode"></param>
        /// <returns></returns>
        public virtual AndonLine GetAndonLineByMachineCode(string machineCode)
        {
            var first = Query<AndonLine>().Where(p => p.MachineCode == machineCode).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return first;
        }

        /// <summary>
        /// 根据编码获取产线
        /// </summary>
        /// <param name="machineCodes"></param>
        /// <returns></returns>
        public virtual EntityList<AndonLine> GetAndonLinesByMachineNames(List<string> machineNames)
        {
            var andonLines = machineNames.SplitContains(names =>
            {
                return Query<AndonLine>().Where(p => names.Contains(p.MachineName)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return andonLines;
        }

        /// <summary>
        /// 根据编码获取产线
        /// </summary>
        /// <param name="machineCodes"></param>
        /// <returns></returns>
        public virtual EntityList<AndonLine> GetAndonLinesByMachineCodes(List<string> machineCodes)
        {
            var andonLines = machineCodes.SplitContains(codes =>
            {
                return Query<AndonLine>().Where(p => codes.Contains(p.MachineCode)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return andonLines;
        }

        /// <summary>
        /// 查询产线与安灯区域
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<AndonLine> CriterialAndonLine(AndonLineCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("产线与安灯区域查询实体异常！".L10N());
            }
            var q = Query<AndonLine>();
            if (criterial.WorkCenterId.HasValue)
            {
                q.Where(p => p.WorkCenterId == criterial.WorkCenterId);
            }
            if (criterial.WorkShopId.HasValue)
            {
                q.Where(p => p.WorkShopId == criterial.WorkShopId);
            }
            if (criterial.FactoryId.HasValue)
            {
                q.Where(p => p.FactoryId == criterial.FactoryId);
            }
            if (criterial.AndonUpholdId.HasValue)
            {
                q.Where(p => p.AndonUpholdId == criterial.AndonUpholdId);
            }
            if (!criterial.AndonCode.IsNullOrEmpty())
            {
                q.Where(m => m.AndonCode.Contains("%" + criterial.AndonCode + "%"));
            }
            if (!criterial.MachineCode.IsNullOrEmpty())
            {
                q.Where(m => m.MachineCode.Contains("%" + criterial.MachineCode + "%"));
            }
            if (!criterial.MachineName.IsNullOrEmpty())
            {
                q.Where(m => m.MachineName.Contains("%" + criterial.MachineName + "%"));
            }
            if (!criterial.EquipmentNo.IsNullOrEmpty())
            {
                q.Where(m => m.Equipment.Code.Contains("%" + criterial.EquipmentNo + "%"));
            }
            if (!criterial.WorkShopCode.IsNullOrEmpty())
                q.Exists<Enterprise>((a, b) => b.Where(p => p.Id == a.WorkShopId).WhereIf(criterial.WorkShopCode.IsNotEmpty(), p => p.Code == criterial.WorkShopCode));

            return q.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 按照生产设备获取设备台账维护
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccounts(string key, PagingInfo info)
        {
            var equipType = Query<EquipType>().Where(p => p.TypeName == "生产设备").FirstOrDefault();
            if (equipType == null)
            {
                throw new ValidationException("请先在设备类型维护,维护生产设备类型!");
            }
            var equipTypeId = equipType.Id;
            var q = Query<EquipAccount>();
            q.Join<EquipModel>((x, y) => x.EquipModelId == y.Id)
            .Where<EquipModel>((x, y) => y.EquipTypeId == equipTypeId);
            if (!key.IsNullOrEmpty())
                q.Where(p => p.Code.Contains(key) || p.Name.Contains(key));
            return q.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询产线条码化数据
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public virtual List<SIE.MES.LineAndon.AndonLine> GetAndonLinePrintDatas(List<double> Ids)
        {
            List<SIE.MES.LineAndon.AndonLine> andonLineDatas = new List<SIE.MES.LineAndon.AndonLine>();
            Ids.SplitDataExecute(Ids =>
            {
                var list = Query<SIE.MES.LineAndon.AndonLine>()
                    .Where(x => Ids.Contains(x.Id))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                andonLineDatas.AddRange(list);
            });
            return andonLineDatas;
        }

    }
}
