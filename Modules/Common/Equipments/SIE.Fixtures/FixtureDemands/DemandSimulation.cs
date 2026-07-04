using SIE.Domain;
using SIE.Fixtures.FixtureDemands.ViewModels;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Models;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Fixtures.FixtureDemands
{
    /// <summary>
    /// 需求仿真器
    /// </summary>
    public class DemandSimulation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DemandSimulation()
        {
            this.DicUnloadVMs = new Dictionary<DicUnloadKey, List<FixtureUnloadViewModel>>();
            this.DicUnloadVMsOfAccount = new Dictionary<FixtureAccount, List<FixtureUnloadViewModel>>();
            this.DicStcoks = new Dictionary<DicUnloadKey, FixtureAccountStock>();
            this.DicMaintainPrjs = new Dictionary<double, List<FixtureEncodeMaintainProject>>();
            this.DicUnloadQtys = new Dictionary<double, int>();
            this.DicAccounts = new Dictionary<double, FixtureAccount>();
        }

        /// <summary>
        /// 工治具需求清单
        /// </summary>
        public FixtureDemand Demand { get; set; }

        /// <summary>
        /// 工治具需求明细列表
        /// </summary>
        public EntityList<FixtureDemandDetail> DemandDetails { get; set; }

        /// <summary>
        /// 所有出库明细ViewModel字典(Id编码、仓库和库位Key,出库明细ViewModel列表)
        /// </summary>
        public Dictionary<DicUnloadKey, List<FixtureUnloadViewModel>> DicUnloadVMs { get; set; }

        /// <summary>
        /// 所有出库明细ViewModel字典(工治具台账,出库明细ViewModel列表)
        /// </summary>
        public Dictionary<FixtureAccount, List<FixtureUnloadViewModel>> DicUnloadVMsOfAccount { get; set; }

        /// <summary>
        /// 所有库存详情字典(Id编码、仓库和库位Key,库存详情)
        /// </summary>
        public Dictionary<DicUnloadKey, FixtureAccountStock> DicStcoks { get; set; }

        /// <summary>
        /// 所有工治具编码保养项目字典(工治具编码Id，保养项目列表)
        /// </summary>
        public Dictionary<double, List<FixtureEncodeMaintainProject>> DicMaintainPrjs { get; set; }

        /// <summary>
        /// 所有工治具编码Id出库总数量(工治具编码Id,出库总数量)
        /// </summary>
        public Dictionary<double, int> DicUnloadQtys { get; set; }

        /// <summary>
        /// 工治具台账列表
        /// </summary>
        public EntityList<FixtureAccount> Accounts { get; set; }

        /// <summary>
        /// 所有工治具台账字典(工治具台账Id，工治具台账列表)
        /// </summary>
        public Dictionary<double, FixtureAccount> DicAccounts { get; set; }

        /// <summary>
        /// 加载相关需求信息
        /// </summary>
        /// <param name="demandId">工治具需求清单Id</param>
        /// <param name="newUnloadVMs">出库明细ViewModel列表</param>
        public void LoadDemandRelateInfo(double demandId, IEnumerable<FixtureUnloadViewModel> newUnloadVMs)
        {
            var elecCoreCt = RT.Service.Resolve<CoreFixtureController>();
            var elecCt = RT.Service.Resolve<CoreFixtureController>();

            this.Demand = RF.GetById<FixtureDemand>(demandId);
            this.DemandDetails = RT.Service.Resolve<ElecFixtureController>().GetFixtureDemandDetails(demandId);
            this.DicUnloadVMsOfAccount = newUnloadVMs.GroupBy(p => p.FixtureAccount).ToDictionary(p => p.Key, p => p.ToList());
            var accountIds = newUnloadVMs.Select(p => p.FixtureAccountId).Distinct().ToList();
            this.Accounts = elecCoreCt.GetFixtureAccounts(accountIds);
            this.DicAccounts = this.Accounts.ToDictionary(p => p.Id);
            var encodeIds = this.Accounts.Select(p => p.FixtureEncodeId).Distinct().ToList();
            var maintainPrjs = elecCt.GetToStorageMaintainProjects(encodeIds);
            this.DicMaintainPrjs = maintainPrjs.GroupBy(p => p.FixtureEncodeId).ToDictionary(p => p.Key, p => p.ToList());
            var accountStocks = elecCoreCt.GetFixtureAccountStocksByAccountIds(accountIds);

            foreach (var newUnloadVM in newUnloadVMs)
            {
                var key = new DicUnloadKey() { AccountId = newUnloadVM.FixtureAccountId, WarehouseId = newUnloadVM.WarehouseId, LocationId = newUnloadVM.LocationId };
                if (!this.DicUnloadVMs.ContainsKey(key))
                    this.DicUnloadVMs.Add(key, new List<FixtureUnloadViewModel>() { newUnloadVM });
                else
                    this.DicUnloadVMs[key].Add(newUnloadVM);

                if (this.DicAccounts.TryGetValue(newUnloadVM.FixtureAccountId, out FixtureAccount account))
                {
                    if (this.DicUnloadQtys.ContainsKey(account.FixtureEncodeId))
                        this.DicUnloadQtys[account.FixtureEncodeId] += newUnloadVM.UnloadQty;
                    else
                        this.DicUnloadQtys.Add(account.FixtureEncodeId, newUnloadVM.UnloadQty);
                }
            }

            foreach (var stock in accountStocks)
            {
                var key = new DicUnloadKey() { AccountId = stock.FixtureAccountId, WarehouseId = stock.FixtureWarehouseId, LocationId = stock.FixtureStorageLocationId.Value };

                if (!this.DicStcoks.ContainsKey(key))
                    this.DicStcoks.Add(key, stock);
            }
        }

        /// <summary>
        /// 加载治具Id，仓库和库位相同的出库明细ViewModel字典
        /// </summary>
        /// <param name="newUnloadVMs">出库明细ViewModel列表</param>
        public void LoadUnloadInfo(IEnumerable<FixtureUnloadViewModel> newUnloadVMs)
        {
            foreach (var newUnloadVM in newUnloadVMs)
            {
                var key = new DicUnloadKey() { AccountId = newUnloadVM.FixtureAccountId, WarehouseId = newUnloadVM.WarehouseId, LocationId = newUnloadVM.LocationId };
                if (!this.DicUnloadVMs.ContainsKey(key))
                    this.DicUnloadVMs.Add(key, new List<FixtureUnloadViewModel>() { newUnloadVM });
                else
                    this.DicUnloadVMs[key].Add(newUnloadVM);
            }
        }
    }
}
