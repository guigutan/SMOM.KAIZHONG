using SIE.Common.Algorithm;
using SIE.Core.Algorithms;
using SIE.Core.Algorithms.KZ;
using SIE.Data;
using SIE.Data.Transaction;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Resources.WipResources;
using SIE.MES.TaskManagement.Dispatchs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Community.CsharpSqlite.Sqlite3;

namespace SIE.MES.TaskManagement.Algorithms
{
    /// <summary>
    /// 轴号编码算法(根据班次+资源生成三位流水码)
    /// </summary>
    [Algorithm("轴号编码算法(根据班次+资源生成三位流水码)", typeof(SequenceConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class WoAxisNumberAlgorithm : SequenceAlgorithm
    {
        protected override SequenceBase GetSequenceBase(int startValue)
        {
            DispatchTask task = Context.Data as DispatchTask;
            if (task == null)
                throw new ValidationException("任务单不存在，无法生成编码规则".L10N());
            var Shift = RT.Service.Resolve<ItemCusotmerDataController>().ShiftAlgorithmGetCode();
            var Resource = string.Empty;
            if (task.ResourceId != null)
                Resource = RT.Service.Resolve<IWipResources>().WipResourcesName(task.ResourceId.Value);
            ////一个识别日期的流水号
            //var curTime = DateTime.Now.Date;
            ////当晚班的时候 ，可能会出现隔天的情况，这个时间仍然算是昨天的流水
            //if (Shift == "B")
            //{
            //    //如果出现这种在隔天的0点到8点的情况，那么就算是昨天的夜班还在继续（切记一定要小于8点，如果只是大于0，那么隔天20点以后的晚班仍然会算成昨天的，这样是不对的）
            //    if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 8)
            //    {
            //        curTime = DateTime.Now.Date.AddDays(-1);
            //    }
            //}
            //一个识别日期的流水号
            var curTime = RT.Service.Resolve<ItemCusotmerDataController>().GetShiftDate();

            var key = curTime.ToString("yyyyMMdd") + "-" + Shift + "-" + task.Product.Code + "-" + Resource + "-WoAxisNumberAlgorithm";

            var sequenc = RT.Service.Resolve<AlgorithmExtController>().GetSequence(Context.DetailId, startValue, key);
            return sequenc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qty"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public override IEnumerable<string> GenerateSegment(int qty)
        {
            if (qty < 1)
            {
                throw new ValidationException("生成序列的数量：{0} 不能小于 1".L10nFormat(qty));
            }

            SequenceConfig config = GetConfig();
            SequenceBase sequenceBase = GetSequenceBase(config.StartValue);
            int num = qty * config.Step;
            int seqValue = GetSeqValue(sequenceBase.GetType(), sequenceBase.Id, num);
            List<string> list = new List<string>();
            int num2 = seqValue - num;
            for (int i = 0; i < qty; i++)
            {
                list.Add(ConvertValue(num2, sequenceBase));
                num2 += config.Step;
            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        private int GetSeqValue<T>(double id, int qty) where T : SequenceBase
        {
            using (var trans = DB.TransactionScope(RF.Find<T>()))
            {
                DB.Update<T>().Set((T p) => p.CurrentValue, (T p) => p.CurrentValue + qty).Where(p => p.Id == id).Execute();
                int result = DB.Query<T>().Where(p => p.Id == id).FirstOrDefault().CurrentValue;
                trans.Complete();
                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        private int GetSeqValue(Type type, double id, int qty)
        {
            return (int)new Func<double, int, int>(GetSeqValue<SequenceBase>).Method.GetGenericMethodDefinition().MakeGenericMethod(type).Invoke(this, new object[2] { id, qty });
        }
    }
}
