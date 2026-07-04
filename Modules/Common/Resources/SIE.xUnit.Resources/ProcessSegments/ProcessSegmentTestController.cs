using SIE.Domain;
using SIE.Resources.ProcessSegments;
using System.Collections.Generic;

namespace SIE.xUnit.Resources.ProcessSegments
{
    /// <summary>
    /// 工段 控制器
    /// </summary>
    public class ProcessSegmentTestController : DomainController
    {
        /// <summary>
        /// 根据工段编号获取工段信息
        /// </summary>
        /// <param name="codes">工段编号</param>
        /// <returns>工段信息</returns>
        public virtual EntityList<ProcessSegment> GetProcessSegment(List<string> codes)
        {
            var query = Query<ProcessSegment>().Where(p => codes.Contains(p.Code));

            return query.ToList();
        }

        /// <summary>
        /// 创建工段信息
        /// </summary>
        /// <param name="isDefault">是否新建默认的数据</param>
        /// <returns></returns>
        public virtual EntityList<ProcessSegment> CreateProcessSegment(bool isDefault)
        {
            string smtCode = "SMT";
            string dipCode = "DIP";
            string assyCode = "ASSY";
            EntityList<ProcessSegment> processSegments = new EntityList<ProcessSegment>();
            if (isDefault)
            {
                processSegments = GetProcessSegment(new List<string>() { smtCode, dipCode, assyCode });
            }

            if (processSegments == null || processSegments.Count == 0)
            {
                // 创建SMT制程工艺类型
                ProcessSegment smtSegment = new ProcessSegment();
                smtSegment.GenerateId();
                smtSegment.Code = isDefault ? smtCode : (smtCode + smtSegment.Id);
                smtSegment.Name = isDefault ? smtCode : (smtCode + smtSegment.Id);
                processSegments.Add(smtSegment);

                // 创建DIP制程工艺类型
                ProcessSegment dipSegment = new ProcessSegment();
                dipSegment.GenerateId();
                dipSegment.Code = isDefault ? dipCode : (dipCode + dipSegment.Id);
                dipSegment.Name = isDefault ? dipCode : (dipCode + dipSegment.Id);
                processSegments.Add(dipSegment);

                // 创建ASSY制程工艺类型
                ProcessSegment assySegment = new ProcessSegment();
                assySegment.GenerateId();
                assySegment.Code = isDefault ? assyCode : (assyCode + assySegment.Id);
                assySegment.Name = isDefault ? assyCode : (assyCode + assySegment.Id);
                processSegments.Add(assySegment);
            }

            return processSegments;
        }
    }
}