using SIE.Fixtures.FixtureRecords;

namespace SIE.Web.Fixtures.Records
{
    /// <summary>
    /// 治具出入库记录-界面
    /// </summary>
    internal class FixtureRecordViewConfig : WebViewConfig<FixtureRecord>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.RecordType).Readonly();
            View.Property(p => p.BusinessType).Readonly();
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.AccountCode).Readonly();
            View.Property(p => p.EncodeCode).Readonly();
            View.Property(p => p.ModelCode).Readonly();
            View.Property(p => p.ModelName).Readonly();
            View.Property(p => p.FixtureType).Readonly();
            View.Property(p => p.WarehouseCode).Readonly();
            View.Property(p => p.WarehouseName).Readonly();
            View.Property(p => p.LocationCode).Readonly();
            View.Property(p => p.LocationName).Readonly();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.ApplyBy).Readonly();
            View.Property(p => p.ApplyDate).Readonly();
            View.Property(p => p.ComplyBy).Readonly();
            View.Property(p => p.ComplyDate).Readonly();
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
        }

        
    }
}
