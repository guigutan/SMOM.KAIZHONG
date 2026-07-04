using SIE.Data;
using SIE.Data.Common;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.MES;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP.Products;
using System;

namespace SIE.Barcodes.DbMigrations
{
    /// <summary>
    /// 初始化表复合索引
    /// </summary>
    class _20200810_000000_InitIndex : ManualDbMigration
    {
        /// <summary>
        /// 数据库链接
        /// </summary>
        public override string DbSetting
        {
            get { return MesCoreEntityDataProvider.ConnectionStringName; }
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
                //条码表添加复合索引（UpdateDate，Id)，目前只支持Oracle
                var provider = Domain.ORM.RdbDataProvider.Get(RF.Find<WipProductProcess>()).DbSetting.ProviderName;
                if (provider == DbProvider.Oracle && provider != DbProvider.ODP)
                    return;
                WipProductProcessAddIndex();
                BatchWipProductProcessAddIndex();
            });
        }

        /// <summary>
        /// 生产产品工序添加复合索引
        /// </summary>
        private void WipProductProcessAddIndex()
        {
            var meta = RF.Find<WipProductProcess>().EntityMeta;
            var tableName = meta.TableMeta.TableName;
            string updateDate = meta.Property(WipProductProcess.UpdateDateProperty).ColumnMeta.ColumnName;
            string id = meta.Property(WipProductProcess.IdProperty).ColumnMeta.ColumnName;
            using (var dba = DbAccesserFactory.Create(MesCoreEntityDataProvider.ConnectionStringName))
            {
                try
                {
                    string sql = $"CREATE INDEX IDX_WPP_UPDATEDATE_ID ON {tableName}({updateDate} DESC,{id})";
                    dba.ExecuteNonQuery(sql);
                }
                catch (Exception)
                {
                    //不处理
                }
            }
        }

        /// <summary>
        /// 批次生产产品工序添加复合索引
        /// </summary>
        private void BatchWipProductProcessAddIndex()
        {
            var meta = RF.Find<BatchWipProductProcess>().EntityMeta;
            var tableName = meta.TableMeta.TableName;
            string updateDate = meta.Property(BatchWipProductProcess.UpdateDateProperty).ColumnMeta.ColumnName;
            string id = meta.Property(BatchWipProductProcess.IdProperty).ColumnMeta.ColumnName;
            using (var dba = DbAccesserFactory.Create(MesCoreEntityDataProvider.ConnectionStringName))
            {
                try
                {
                    string sql = $"CREATE INDEX IDX_BWPP_UPDATEDATE_ID ON {tableName}({updateDate} DESC,{id})";
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