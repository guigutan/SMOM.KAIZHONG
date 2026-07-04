using Microsoft.Scripting.Utils;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.QTimes.Enums;
using SIE.MES.QTimes.Services;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Handles
{
    /// <summary>
    /// QT标准维护保存命令帮助类
    /// </summary>
    public class QTimeStandardSaveHandle
    {
        /// <summary>
        /// 构造
        /// </summary>
        public QTimeStandardSaveHandle(EntityList data)
        {
            Datas = (data as EntityList<QTimeStandard>).Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged).ToList();
            ChildDatas = new List<QTPushObject>();
            ChildDataIds = new List<double>();
            QTPushTypes = new List<QTPushType>();
            foreach (var item in Datas)
            {
                ChildDatas.AddRange(item.QTPushObjectList);
                ChildDataIds.AddRange(ChildDatas.Select(p => p.Id));
            }
            QTPushTypes.AddRange(ChildDatas.Select(p => p.ObjectType).ToList());
            DataIds = Datas.Select(p => p.Id).ToList();
            ProductIds = Datas.Select(p => p.ProductId).ToList();
            WipIds = Datas.Select(p => p.WipResourceId).ToList();
        }
        #region 属性
        /// <summary>
        /// 单体
        /// </summary>
        private readonly string _singleType = "Single";

        /// <summary>
        /// 批次
        /// </summary>
        private readonly string _batchType = "Batch";

        /// <summary>
        /// 保存列表
        /// </summary>
        private List<QTimeStandard> Datas {  get; set; }

        /// <summary>
        /// 保存子表
        /// </summary>
        private List<QTPushObject> ChildDatas { get; set; }

        /// <summary>
        /// 保存列表Ids
        /// </summary>
        private List<double> DataIds { get; set; }

        /// <summary>
        /// 保存子表Ids
        /// </summary>
        private List<double> ChildDataIds { get; set; }

        /// <summary>
        /// 子表推送对象类型
        /// </summary>
        private List<QTPushType> QTPushTypes { get; set; }

        /// <summary>
        /// 保存列表产品Ids
        /// </summary>
        private List<double?> ProductIds { get; set; }

        /// <summary>
        /// 保存列表产线Ids
        /// </summary>
        private List<double?> WipIds { get; set; }

        /// <summary>
        /// 单体工序
        /// </summary>
        private List<ProcessType?> SingleType = new List<ProcessType?> { ProcessType.Pqc, ProcessType.Fix, ProcessType.Rework, ProcessType.Assembly, ProcessType.Packing, ProcessType.Ageing };

        /// <summary>
        /// 批次工序
        /// </summary>
        private List<ProcessType?> BatchType = new List<ProcessType?> { ProcessType.BatchAssembly, ProcessType.BatchPqc, ProcessType.BatchFix, ProcessType.BatchPacking };
        #endregion

        #region 方法
        /// <summary>
        /// 校验前端产品+产线+开始工序+开始状态+结束工序+结束状态唯一
        /// </summary>
        public void WebRepeat()
        {
            var groupBy = Datas.GroupBy(p => new {p.ProductId, p.WipResourceId, p.StartProcessId,p.StartState,p.EndProcessId,p.EndState}).ToList();
            if (groupBy.Any(p => p.Count() > 1)) 
            {
                throw new ValidationException("产品+产线+开始工序+开始状态+结束工序+结束状态不允许重复，请检查！".L10N());
            }
        }

        /// <summary>
        /// 校验数据库产品+产线+开始工序+开始状态+结束工序+结束状态唯一
        /// </summary>
        public void DBRepeat()
        {
            List<QTimeStandard> qTimeStandards = new List<QTimeStandard>();
            qTimeStandards.AddRange(Datas);
            qTimeStandards.AddRange(RT.Service.Resolve<QTimeStandardService>().GetQTByProductAndWip(DataIds, ProductIds, WipIds));
            var groupBy = qTimeStandards.GroupBy(p => new { p.ProductId, p.WipResourceId, p.StartProcessId, p.StartState, p.EndProcessId, p.EndState }).ToList();
            if (groupBy.Any(p => p.Count() > 1))
            {
                throw new ValidationException("产品+产线+开始工序+开始状态+结束工序+结束状态不允许重复，请检查！".L10N());
            }
        }

        /// <summary>
        /// 校验开始工序和结束工序必须同为批次或单体
        /// </summary>
        public void StartTypeEqualEndType()
        {
            if (Datas.Any(p => DifferentProcessType(p.StartProcessType, p.EndProcessType)))
            {
                throw new ValidationException("开始、结束工序必须同为单体或批次工序！".L10N());
            }
        }

        /// <summary>
        /// 必填校验
        /// </summary>
        public void Required()
        {
            if (Datas.Any(p => p.ProductId == null && p.WipResourceId == null))
            {
                throw new ValidationException("产品产线必须维护其一！".L10N());
            }
            if (Datas.Any(p => p.StartProcessId == 0))
            {
                throw new ValidationException("开始工序不能为空！".L10N());
            }
            if (Datas.Any(p => !p.StartState.HasValue))
            {
                throw new ValidationException("开始状态不能为空！".L10N());
            }
            if (Datas.Any(p => p.EndProcessId == 0))
            {
                throw new ValidationException("结束工序不能为空！".L10N());
            }
            if (Datas.Any(p => !p.EndState.HasValue))
            {
                throw new ValidationException("结束状态不能为空！".L10N());
            }
            if (Datas.Any(p => p.Time <= 0))
            {
                throw new ValidationException("时间值必须大于0！".L10N());
            }
            if (Datas.Any(p => !p.TimeUnit.HasValue))
            {
                throw new ValidationException("时间单位必填！".L10N());
            }
        }

        /// <summary>
        /// 校验开始工序和结束工序必须同为批次或单体
        /// </summary>
        /// <param name="startType">开始工序</param>
        /// <param name="endType">结束工序</param>
        /// <returns></returns>
        private bool DifferentProcessType(ProcessType? startType, ProcessType? endType)
        {
            return (SingleType.Contains(startType) && BatchType.Contains(endType)) || (SingleType.Contains(endType) && BatchType.Contains(startType));
        }

        /// <summary>
        /// 校验开始结束同工序时不能同状态
        /// </summary>
        /// <returns></returns>
        public void SameProcessSameState()
        {
            if (Datas.Any(p => p.StartProcessId == p.EndProcessId && p.StartState == p.EndState))
            {
                throw new ValidationException("开始工序和结束工序相同时，状态不能相同".L10N());
            }
        }

        /// <summary>
        /// 校验子表推送对象不能为空
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        public void ChildRequired()
        {
            if (ChildDatas.Any(p => p.ObjectId == 0 || p.ObjectCode.IsNullOrEmpty() || p.ObjectName.IsNullOrEmpty()))
            {
                throw new ValidationException("推送对象不能为空！".L10N());
            }
        }

        /// <summary>
        /// 校验子表重复
        /// </summary>
        public void ChildRepeat()
        {
            List<QTPushObject> children = new List<QTPushObject>();
            children.AddRange(ChildDatas);
            var dbChildren = RT.Service.Resolve<QTPushService>().GetPushList(ChildDataIds, DataIds, QTPushTypes);
            children.AddRange(dbChildren);
            if (children.GroupBy(p => new {p.QTStandardId,p.ObjectType,p.ObjectId}).ToList().Any(p => p.Count() > 1))
            {
                throw new ValidationException("同一类型推送对象不能重复！".L10N());
            }
        }
        #endregion
    }
}
