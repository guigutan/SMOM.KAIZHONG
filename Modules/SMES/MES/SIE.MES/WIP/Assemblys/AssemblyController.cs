using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.LoadItems;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;

namespace SIE.MES.WIP.Assemblys
{
    /// <summary>
    /// 上料采集控制器
    /// </summary>
    public class AssemblyController : WipController
    {
        /// <summary>
        /// 记录采集过站记录的同时，进行装配扣料
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="product">采集运行时产品模型, 记录产品在生产过程中的信息, 通过Puid产品全局ID关联生产信息</param>
        /// <param name="collectBarcodes">采集条码集合</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        protected override void OnWipProductProcessFinished(WipProductProcess wipProductProcess, product product,
            IList<CollectBarcode> collectBarcodes, CollectData collectData, Workcell workcell)
        {
            if (collectData is null)
            {
                throw new ArgumentNullException(nameof(collectData));
            }

            if (workcell == null)
            {
                throw new ValidationException("工作单元【workcell】为空".L10N());
            }
            base.OnWipProductProcessFinished(wipProductProcess, product, collectBarcodes, collectData, workcell);//调用基类方法写缺陷
            if (collectData.CollectBarcode != null
                && collectData.CollectBarcode.AssemblyItems != null) //上料采集优先上料后过站
            {
                var ctr = RT.Service.Resolve<LoadItemController>();
                foreach (var assemblyItem in collectData.CollectBarcode.AssemblyItems)
                {
                    ctr.NewLoadItem(assemblyItem.Value, workcell);
                }
            }

            UseLoadItemExecutor useLoadItemExecutor = new UseLoadItemExecutor(product, workcell, true);
            useLoadItemExecutor.UseLoadItem(wipProductProcess);
        }

        /// <summary>
        /// 验证工序BOM是否够扣料
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元信息</param>
        public virtual void ValidateProcessBom(CollectBarcode barcode, Workcell workcell)
        {
            var product = RuntimeController.FindProduct(barcode);
            if (product == null)
            {
                throw new ValidationException("[{0}]找不到产品生产记录，无法验证工序BOM".L10nFormat(barcode));
            }

            UseLoadItemExecutor useLoadItemExecutor = new UseLoadItemExecutor(product, workcell, isBackFlush: false);
            useLoadItemExecutor.ValidateProcessBom();
        }

        /// <summary>
        /// 获取产品工序BOM
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderBom> GetWorkOrderBom(product product)
        {
          return  DB.Query<WorkOrderBom>().Where(x => x.WorkOrderId == product.WorkOrderId).ToList();
        }
    }
}
