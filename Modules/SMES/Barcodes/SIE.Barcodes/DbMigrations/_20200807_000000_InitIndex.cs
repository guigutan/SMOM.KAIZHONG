using SIE.Data;
using SIE.Data.Common;
using SIE.Data.DbMigration;
using SIE.Domain;
using System;

namespace SIE.Barcodes.DbMigrations
{
    /// <summary>
    /// 初始化表复合索引
    /// </summary>
    class _20200807_000000_InitIndex : ManualDbMigration
    {
        /// <summary>
        /// 数据库链接
        /// </summary>
        public override string DbSetting
        {
            get { return BarcodeEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "初始化表复合索引".L10N(); }
        }

        /// <summary>
        /// 手动升级的类型：数据
        /// </summary>
        public override ManualMigrationType Type
        {
            get { return ManualMigrationType.Data; }
        }

        /// <summary>
        /// 数据库回滚
        /// </summary>
        protected override void Down() { }

        /// <summary>
        /// 数据库升级
        /// </summary>
        protected override void Up()
        {
            this.RunCode(db =>
            {
                BarcodeAddIndex();
            });
        }

        /// <summary>
        /// 条码表添加复合索引
        /// </summary>
        private void BarcodeAddIndex()
        {
            //条码表添加复合索引（UpdateDate，Id)，目前只支持Oracle
            var provider = Domain.ORM.RdbDataProvider.Get(RF.Find<Barcode>()).DbSetting.ProviderName;
            if (provider == DbProvider.Oracle && provider != DbProvider.ODP)
                return;
            var meta = RF.Find<Barcode>().EntityMeta;
            var tableName = meta.TableMeta.TableName;
            string updateDate = meta.Property(Barcode.UpdateDateProperty).ColumnMeta.ColumnName;
            string id = meta.Property(Barcode.IdProperty).ColumnMeta.ColumnName;
            using (var dba = DbAccesserFactory.Create(BarcodeEntityDataProvider.ConnectionStringName))
            {
                try
                {
                    string sql = $"CREATE INDEX IDX_BC_UPDATEDATE_ID ON {tableName}({updateDate} DESC,{id})";
                    dba.ExecuteNonQuery(sql);
                }
                catch (Exception)
                {
                    //不处理
                }
            }
        }
    }
}