using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Packages.Boxs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Piles
{
    internal class SaveTurnoverBoxToPile : DomainController
    {
        /// <summary>
        /// //保存时将新增的周转箱数据同步新增到垛表中
        /// </summary>
        /// <param name="boxes">周转箱数据</param>
        public virtual void SaveTurnoverBoxs(List<TurnoverBox> boxes)
        {
            var pileCtl = RT.Service.Resolve<PileController>();

            //周转箱编码
            List<string> codes = boxes.Select(p => p.Code).ToList();

            //获取垛表数据转换为字典
            var pileDicts = pileCtl.GetPiles(codes).ToDictionary(p => p.Code);

            //周转箱型号ID集合
            List<double> modelIds = boxes.Select(p => p.TrunoverBoxModelId).Distinct().ToList();

            //获取周转箱型号转换为字典
            var modelDicts = RT.Service.Resolve<BoxController>().GetTurnoverBoxModels(modelIds).ToDictionary(p => p.Id);

            EntityList<Pile> pileList = new EntityList<Pile>();
            EntityList<PileLog> logs = new EntityList<PileLog>();
            var pileCodes = pileDicts.Select(a => a.Value.Code).ToList();
            var pileLogs = DB.Query<PileLog>().Where(a => pileCodes.Contains(a.PileCode) && a.BusinessType != null).ToList();
            foreach (var box in boxes)
            {
                Pile pile;
                modelDicts.TryGetValue(box.TrunoverBoxModelId, out TurnoverBoxModel model);
                pileDicts.TryGetValue(box.Code, out pile);
                if (pile == null)
                {
                    pile = new Pile();
                    pile.Code = box.Code;
                    pile.Model = model?.Code;
                    pile.ModelName = model?.Name;
                    pile.PileState = BoxState.Unused;
                    pile.TurnoverContainer = true;
                    pile.Width = model?.Width;
                    pile.Length = model?.Length;
                    pile.Height = model?.Height;
                    pileList.Add(pile);

                    //写入日志
                    logs.Add(pileCtl.CreatePileLog(pile, PileOpType.Create, false));
                }
                else
                {
                    if (box.State.HasValue)
                        pile.PileState = box.State.Value;
                    else
                        pile.PileState = BoxState.Unused;
                    RF.Save(pile);
                }

                if (box.PersistenceStatus == PersistenceStatus.Deleted && pile != null)
                {
                    if (pileLogs.Any(a => a.PileCode == box.Code))
                        throw new ValidationException("周转箱已在垛表有使用记录，不能删除".L10N());
                    else
                        pile.PersistenceStatus = PersistenceStatus.Deleted;

                    RF.Save(pile);                    
                }
            }

            ////批量保存
            if (pileList.Any())
            {
                RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(pileList);
            }

            if (logs.Any())
            {
                RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(logs);
            }
        }
    }
}