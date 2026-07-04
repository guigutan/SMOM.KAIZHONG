using SIE.MES.LoadItemRecords;
using SIE.MES.LoadItemsRecords;
using System;

namespace SIE.Web.MES.LoadItemsRecords
{
    /// <summary>
    /// 上下料记录查询视图配置
    /// </summary>
    public class LoadItemsRecordCriterialViewConfig : WebViewConfig<LoadItemsRecordCriterial>
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Resource).HasLabel("资源").UseDataSource((x, y, z) => {


                return RT.Service.Resolve<LoadItemsRecordController>().GetAuthResource(y, z);
            //
        });
            View.Property(p => p.Station).HasLabel("工位");
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            View.Property(p => p.LoadItemWokerOrder);

            View.Property(p => p.Sn);
            View.Property(p => p.OpareteType).HasLabel("操作类型".L10N()+"*").DefaultValue(OpareteType.LoadItem).Cascade(p => p.UnloadItemType, null) ;
            View.Property(p => p.UnloadItemType);
            View.Property(p => p.IsDiaplayZero).DefaultValue(true);
            View.Property(p => p.Oparetor);
            View.Property(p => p.OparetorTime);
        }
    }
}
