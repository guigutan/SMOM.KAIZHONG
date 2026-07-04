using SIE.Domain;
using SIE.Resources.ProcessSegments;
using SIE.Resources.ProcessTechs;
using SIE.Resources.ProcessTechTypes;
using SIE.xUnit.Resources.ProcessSegments;
using SIE.xUnit.Resources.ProcessTechTypes;
using System.Collections.Generic;
using System.Linq;

namespace SIE.xUnit.Resources.ProcessTechs
{
    /// <summary>
    /// 制程工艺 控制器
    /// </summary>
    public class ProcessTechTestController : DomainController
    {
        /// <summary>
        /// 根据制程工艺编号获取制程工艺信息
        /// </summary>
        /// <param name="codes">制程工艺编号</param>
        /// <returns>制程工艺信息</returns>
        public virtual EntityList<ProcessTech> GetProcessTech(List<string> codes)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(ProcessTech.ProcessSegmentProperty);
            elo.LoadWith(ProcessTech.ProcessTechTypeProperty);
            var query = Query<ProcessTech>().Where(p => codes.Contains(p.Code));

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 创建制程工艺类型信息
        /// </summary>
        /// <param name="isDefault">是否新建默认的数据</param>
        /// <returns></returns>
        public virtual EntityList<ProcessTech> CreateProcessTech(bool isDefault)
        {
            string smtCode = "SMT";
            string dipCode = "DIP";
            string assyCode = "ASSY";
            EntityList<ProcessTech> processTechs = new EntityList<ProcessTech>();
            if (isDefault)
            {
                processTechs = GetProcessTech(new List<string>() { smtCode, dipCode, assyCode });
            }

            if (processTechs == null || processTechs.Count == 0)
            {
                // 创建制程工艺类型
                EntityList<ProcessTechType> ptTypes = RT.Service.Resolve<ProcessTechTypeTestController>().CreateProcessTechType(isDefault);
                EntityList<ProcessSegment> segments = RT.Service.Resolve<ProcessSegmentTestController>().CreateProcessSegment(true);

                // 创建SMT制程工艺类型
                ProcessTech smtTech = new ProcessTech();
                smtTech.GenerateId();
                smtTech.Code = isDefault ? smtCode : (smtCode + smtTech.Id);
                smtTech.Name = isDefault ? smtCode : (smtCode + smtTech.Id);
                smtTech.IsScheduling = true;
                smtTech.IsBottleneck = true;
                smtTech.TransferTime = 600;
                smtTech.SAM = 180;
                smtTech.WorkingHours = 1;
                smtTech.ProcessTechType = ptTypes.First(p => p.Code.Contains(smtCode));
                smtTech.ProcessTechTypeId = smtTech.ProcessTechType.Id;
                smtTech.AlgorithmMarking = smtTech.ProcessTechType.AlgorithmMarking;
                smtTech.ProcessSegment = segments.First(p => p.Code.Contains(smtCode));
                smtTech.ProcessSegmentId = smtTech.ProcessSegment.Id;
                processTechs.Add(smtTech);

                // 创建DIP制程工艺类型
                ProcessTech dipTech = new ProcessTech();
                dipTech.GenerateId();
                dipTech.Code = isDefault ? dipCode : (dipCode + dipTech.Id);
                dipTech.Name = isDefault ? dipCode : (dipCode + dipTech.Id);
                dipTech.IsScheduling = true;
                dipTech.IsBottleneck = true;
                dipTech.TransferTime = 600;
                dipTech.SAM = 180;
                dipTech.WorkingHours = 1;
                dipTech.ProcessTechType = ptTypes.First(p => p.Code.Contains(dipCode));
                dipTech.ProcessTechTypeId = dipTech.ProcessTechType.Id;
                dipTech.AlgorithmMarking = dipTech.ProcessTechType.AlgorithmMarking;
                dipTech.ProcessSegment = segments.First(p => p.Code.Contains(dipCode));
                dipTech.ProcessSegmentId = dipTech.ProcessSegment.Id;
                processTechs.Add(dipTech);

                // 创建ASSY制程工艺类型
                ProcessTech assyTech = new ProcessTech();
                assyTech.GenerateId();
                assyTech.Code = isDefault ? assyCode : (assyCode + assyTech.Id);
                assyTech.Name = isDefault ? assyCode : (assyCode + assyTech.Id);
                assyTech.IsScheduling = true;
                assyTech.IsBottleneck = true;
                assyTech.TransferTime = 600;
                assyTech.SAM = 180;
                assyTech.WorkingHours = 1;
                assyTech.ProcessTechType = ptTypes.First(p => p.Code.Contains(assyCode));
                assyTech.ProcessTechTypeId = assyTech.ProcessTechType.Id;
                assyTech.AlgorithmMarking = assyTech.ProcessTechType.AlgorithmMarking;
                assyTech.ProcessSegment = segments.First(p => p.Code.Contains(assyCode));
                assyTech.ProcessSegmentId = assyTech.ProcessSegment.Id;
                processTechs.Add(assyTech);
            }

            return processTechs;
        }
    }
}