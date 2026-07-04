using SIE.Domain;
using SIE.Domain.Validation;
using System;

namespace SIE.Core.Algorithms
{
    /// <summary>
    /// 序列自定义生成算法
    /// </summary>
    [RootEntity, Serializable]
    public static class SequenceCoustomAlgorithm
    {
        /// <summary>
        /// 锁对象
        /// </summary>
        public static readonly object LockSeq = new object();

        /// <summary>
        /// Lot批次顺序号编码
        /// </summary>
        public static int TypeLotSequence { get; } = 20;


        /// <summary>
        /// 序列自定义
        /// type=1, WMS：物料批次方案编码
        /// type=20, MES：批次顺序号编码
        /// type=51, APS：计划工单号编码
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="preContent">前缀</param>
        /// <param name="qty">数量</param>
        /// <returns>返回序列号</returns>
        public static int GetSeqCustomValue(int type, string preContent, int qty)
        {
            if (qty < 1)
            {
                throw new ValidationException("生成数量必须大于等于1".L10N());
            }
            bool isExecute = false;
            int result = -1;
            using (var tran = DB.AutonomousTransactionScope(RF.Find<SequenceCustom>()))
            {
                ////先进来Update，进行同步锁
                DB.Update<SequenceCustom>().Set(p => p.Type, type).Where(p => p.Type == type && p.PreContent == preContent).Execute();
                ////再查一次
                var rs = DB.Query<SequenceCustom>().Where(p => p.Type == type && p.PreContent == preContent).FirstOrDefault();
                if (rs == null)
                {
                    lock (LockSeq)
                    {
                        rs = DB.Query<SequenceCustom>().Where(p => p.Type == type && p.PreContent == preContent).FirstOrDefault();
                        if (rs == null)
                        {
                            rs = new SequenceCustom();
                            rs.CurrentValue = qty;
                            rs.Type = type;
                            rs.PreContent = preContent;
                            RF.Save(rs);
                            isExecute = true;
                            result = qty;
                        }
                    }
                }

                if (!isExecute)
                {
                    ////先进来Update，进行同步锁
                    DB.Update<SequenceCustom>().Set(p => p.CurrentValue, p => p.CurrentValue + qty).Where(p => p.Type == type && p.PreContent == preContent).Execute();
                    result = DB.Query<SequenceCustom>().Select(p => p.CurrentValue).Where(p => p.Type == type && p.PreContent == preContent).FirstOrDefault<int>();
                }

                tran.Complete();
                return result;
            }
        }
    }
}
