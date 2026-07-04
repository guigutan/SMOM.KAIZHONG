using DevExpress.XtraCharts;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.LES.MaterialReceives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.LES.MaterialReceives.DataQueryer
{
    /// <summary>
    /// 物料接收查询器
    /// </summary>
    public class MaterialReceiveQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 查询接收标签
        /// </summary>
        /// <param name="soNo"></param>
        /// <param name="dtlIds"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReceiveLabel> GetMaterialReceiveLabels(double soNo, List<double?> dtlIds)
        {
            var list = RT.Service.Resolve<MaterialReceiveController>().GetMaterialReceiveLabels(soNo, ReceiveState.TobeReceived, dtlIds);
            //var magreLabels = RT.Service.Resolve<MaterialReceiveController>().GetMegreLabels(list.ToList());
            //list.AddRange(magreLabels); 
            list.ForEach(p => p.ReceivedQty = p.IssuedQty); //接收数默认等于发料数   
            return list;
        }

        /// <summary>
        /// 标签接收
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="labels"></param>
        public virtual object ReceiveLabels(MaterialReceiveDetail detail, List<MaterialReceiveLabel> labels)
        {
            detail.LabelList.AddRange(labels);
            detail.ReceivedQty = labels.Sum(p => p.ReceivedQty);
            RT.Service.Resolve<MaterialReceiveController>().MaterialReceive(new List<MaterialReceiveDetail> { detail });
            return true;
        }

        /// <summary>
        /// 校验单据是否超收
        /// </summary>
        /// <param name="receiveIds"></param>
        /// <returns></returns>
        public virtual object ValidationOverReceive(List<double> receiveIds)
        {
            var msgs = new List<string>();
            msgs = RT.Service.Resolve<MaterialReceiveController>().ValidationOverReceive(receiveIds);

            return string.Join(";\r\n", msgs);
        }
        /// <summary>
        /// 校验明细是否超收
        /// </summary>
        /// <param name="receiveDtls"></param>
        /// <returns></returns>
        public virtual object ValidationOverReceiveDetail(List<MaterialReceiveDetail> receiveDtls)
        {
            var msgs = new List<string>();
            msgs = RT.Service.Resolve<MaterialReceiveController>().ValidationOverReceiveDetail(receiveDtls);            

            return string.Join(";\r\n", msgs);
        }
    }
}