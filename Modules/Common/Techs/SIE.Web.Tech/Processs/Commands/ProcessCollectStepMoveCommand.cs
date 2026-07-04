using SIE.Common;
using SIE.Domain;
using SIE.Tech.Processs;
using SIE.Web.Command;
using SIE.Web.Common.Sort.Commands;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Tech.Processs.Commands
{
    /// <summary>
    /// 下移
    /// </summary>
    [JsCommand("SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveDown")]
    public class ProcessCollectStepMoveDownCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">entityType</param>
        /// <returns>true</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var itemList = args.Data.ToJsonObject<List<ProcessCollectStep>>();
            var resList = itemList.AsEntityList();
            if (resList.Count == 0 || resList.Any(p => p.Process == null)) return true;
            resList.ForEach(p =>
            {
                if (p.CreateBy <= 0)
                {
                    p.PersistenceStatus = PersistenceStatus.New;
                }
            });
            if (!resList.Any(p => p.PersistenceStatus == PersistenceStatus.New))
            {
                RF.Save(resList);
            }
            return true;
        }
    }

    /// <summary>
    /// 上移
    /// </summary>
    [JsCommand("SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveUp")]
    public class ProcessCollectStepMoveUpCommand : MoveUpCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">entityType</param>
        /// <returns>true</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var itemList = args.Data.ToJsonObject<List<ProcessCollectStep>>();
            var resList = itemList.AsEntityList();
            if (resList.Count == 0 || resList.Any(p => p.Process == null)) return true;
            resList.ForEach(p =>
            {
                if (p.CreateBy <= 0)
                {
                    p.PersistenceStatus = PersistenceStatus.New;
                }
            });
            if (!resList.Any(p => p.PersistenceStatus == PersistenceStatus.New))
            {
                RF.Save(resList);
            }
            return true;
        }
    }

    /// <summary>
    /// 置顶
    /// </summary>
    [JsCommand("SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveTop")]
    public class ProcessCollectStepMoveTopCommand : MoveTopCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">entityType</param>
        /// <returns>true</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var itemList = args.Data.ToJsonObject<List<ProcessCollectStep>>();
            var resList = itemList.AsEntityList();
            if (resList.Count == 0 || resList.Any(p => p.Process == null)) return true;
            resList.ForEach(p =>
            {
                if (p.CreateBy <= 0)
                {
                    p.PersistenceStatus = PersistenceStatus.New;
                }
            });
            if (!resList.Any(p => p.PersistenceStatus == PersistenceStatus.New))
            {
                RF.Save(resList);
            }
            return true;
        }
    }

    /// <summary>
    /// 置底
    /// </summary>
    [JsCommand("SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveBottom")]
    public class ProcessCollectStepMoveBottomCommand : MoveBottomCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">entityType</param>
        /// <returns>true</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var itemList = args.Data.ToJsonObject<List<ProcessCollectStep>>();
            var resList = itemList.AsEntityList();
            if (resList.Count == 0 || resList.Any(p => p.Process == null)) return true;
            resList.ForEach(p =>
            {
                if (p.CreateBy <= 0)
                {
                    p.PersistenceStatus = PersistenceStatus.New;
                }
            });
            if (!resList.Any(p => p.PersistenceStatus == PersistenceStatus.New))
            {
                RF.Save(resList);
            }
            return true;
        }
    }
}