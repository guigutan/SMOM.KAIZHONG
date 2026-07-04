using SIE.Api;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Onhands;
using SIE.Inventory.Piles;
using SIE.Inventory.Piles.Configs;
using SIE.Packages.Boxs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Inventory.Interfaces
{
    /// <summary>
    /// 垛控制器
    /// </summary>
    public partial class IPileController : DomainController
    {
        /// <summary>
        /// 获取垛表型号
        /// </summary>
        /// <param name="lpn">lpn</param>
        /// <returns></returns>
        [ApiService("码盘收货:获取垛表型号")]
        [return: ApiReturn("码盘收货:获取垛表型号")]
        public virtual List<PileData> GetPileDatasByLpn([ApiParameter("LPN")] string lpn)
        {
            var pile = RT.Service.Resolve<PileController>().GetPile(lpn);
            if (pile != null)
            {
                if (pile.PileState != BoxState.Unused)
                    throw new ValidationException("LPN不是闲置状态".L10N());

                return new List<PileData>() { new PileData() { Code = lpn, Model = pile.Model, Name = pile.ModelName } };
            }
            else
            {
                var config = ConfigService.GetConfig(new LpnRuleConfig(), typeof(Pile));
                if (!config.lpnCheck(lpn, out string strErr))
                {
                    throw new ValidationException(strErr);
                }
                var box = RT.Service.Resolve<BoxController>().GetTurnoverBox(lpn);
                if (box == null || box.State != BoxState.Unused)
                {
                    var boxs = RF.GetAll<TurnoverBoxModel>();
                    if (boxs.Count > 0)
                    {
                        var rst = new List<PileData>();
                        boxs.ForEach(p =>
                        {
                            rst.Add(new PileData() { Code = p.Code, Model = p.Code,Name = p.Name});
                        });
                        return rst;
                    }
                    else
                    {
                        throw new ValidationException("没有闲置的容器[周转箱、垛]".L10N());
                    }
                }
                else
                {
                    var rst = new List<PileData>();
                    rst.Add(new PileData() { Code = box.Code, Model = box.TurnoverBoxModelCode, Name = box.TurnoverBoxModelName });
                    return rst;
                }
            }
        }

        /// <summary>
        /// LPN注册-获取垛表型号
        /// </summary>
        /// <returns>PileData</returns>
        [ApiService("LPN注册:获取垛表型号")]
        [return: ApiReturn("LPN注册:获取垛表型号")]
        public virtual List<PileData> GetPileModel()
        {
            List<PileData> rst = new List<PileData>();
            var boxs = RF.GetAll<TurnoverBoxModel>();
            if (boxs.Count > 0)
            {
                boxs.ForEach(p =>
                {
                    rst.Add(new PileData() { Model=p.Code,Code = p.Code,Name = p.Name});
                });
            }
            return rst;
        }

        /// <summary>
        /// LPN注册-注册LPN
        /// <param name="lpn">lpn</param>
        /// <param name="model">型号</param>
        /// </summary>
        /// <returns></returns>
        [ApiService("托盘注册:注册托盘")]
        [return: ApiReturn("托盘注册:注册托盘")]
        public virtual void RegisterLpn([ApiParameter("LPN")] string lpn, [ApiParameter("型号")] string model)
        {
            var config = ConfigService.GetConfig(new LpnRuleConfig(), typeof(Pile));
            if (!config.lpnCheck(lpn, out string strErr))
            {
                throw new ValidationException(strErr);
            }
            var pileCtl = RT.Service.Resolve<PileController>();
            var boxCtl = RT.Service.Resolve<BoxController>();
            var LotAndHandCtl = RT.Service.Resolve<InvOnhandController>();
            //1、扫描时验证：此条码在垛表中不存在，在周转箱中不存在，在库存表中不存在
            var pile = pileCtl.GetPile(lpn);
            if (pile != null)
            {
                string errMsg = "LPN[{0}]已在垛表存在".L10nFormat(lpn);
                throw new ValidationException(errMsg);
            }
            var box = boxCtl.GetTurnoverBox(lpn);
            if (box != null)
            {
                string errMsg = "LPN[{0}]已在周转箱存在".L10nFormat(lpn);
                throw new ValidationException(errMsg);
            }
            var onhands = LotAndHandCtl.GetLotLpnOnhands(lpn);
            if (onhands.Count > 0)
            {
                string errMsg = "LPN[{0}]已有库存".L10nFormat(lpn);
                throw new ValidationException(errMsg);
            }

            //创建垛
            pileCtl.CreatePile(lpn, model, null, BoxState.Unused, ItemState.Create, null);
        }

        /// <summary>
        /// 验证垛号
        /// <param name="lpn">lpn</param>
        /// </summary>
        /// <returns></returns>
        [ApiService("验证垛号")]
        public virtual void CheckPile([ApiParameter("LPN")] string lpn)
        {
            RT.Service.Resolve<PileController>().ValidatePileData(lpn);
        }
    }
}
