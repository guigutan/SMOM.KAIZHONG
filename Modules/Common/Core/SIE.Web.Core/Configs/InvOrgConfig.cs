using SIE.Common;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Rbac.InvOrgs;
using SIE.Web.ClientMetaModel;
using SIE.Web.Common.Editors;
using SIE.Web.Json;
using SIE.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.Core.Configs
{
    public class InvOrgConfig : CatalogConfig
    {
        private readonly Type type;

        /// <summary>
        /// 包含当前库存组织
        /// </summary>
        public bool ContainCurInvorg { get; set; } = true;

        public InvOrgConfig()
        {
            type = typeof(InvOrg);
            QueryMode = "local";
            // bs得兼容CS，可参考 cs版本的CatalogEditor
            DisplayField = InvOrg.NameProperty.Name;
            ValueField = InvOrg.CodeProperty.Name;
            ColumnXType = "comboColumn";
            CatalogReloadData = false; //true时，则每次都重新查询数据库，快码数据相当于期初数据，默认不启用，只在需要的场景下在需要的界面去启用.
        }

        /// <summary>
        /// 仓库字段名称（默认是WarehouseId）
        /// </summary>
        public string WarehouseFieldName { get; set; }

        protected override void ToJson(LiteJsonWriter json)
        {
            json.WritePropertyIf("WarehouseFieldName", WarehouseFieldName);
            base.ToJson(json);
        }

        public override void InitConfig(WebEntityPropertyViewMeta meta, string moduleKey)
        {
            var catalogModuleKey = typeof(InvOrg).FullName + "," + type.Assembly.GetName().Name;
            string scopeKey = type.FullName + "," + type.Assembly.GetName().Name;
            this.Token = TokenManager.GetToken(catalogModuleKey, scopeKey);
            string[] fields = GetField(type);
            int pageSize = 0;
            var data = GetInvOrg(type, fields, out pageSize);
            Store = new StoreConfig()
            {
                PageSize = pageSize,
                Fields = fields,
                Data = data
            };

            if (CatalogReloadData)
            {
                string key = MetaModel.View.DataSourceProvider.Add(meta.Owner.EntityType,
                        meta.Owner.ViewGroup, meta.Name, (e, c, r) =>
                        {
                            if (ContainCurInvorg)
                                return RF.GetAll<InvOrg>();
                            else
                                return RF.GetAll<InvOrg>().Where(p => p.Code != RT.InvOrg).AsEntityList();
                        });

                DataSourceProperty = key;
            }
        }


        private string[] GetField(Type type)
        {
            var vm = UIModel.Views.CreateView(type, ViewConfig.SelectionView);
            var list = vm.EntityProperties as List<EntityPropertyViewMeta>;
            var columns = list.FindAll(k => k.ShowInWhere == ShowInWhere.All || k.ShowInWhere == ShowInWhere.DropDown);
            string[] fields = new string[columns.Count];
            int index = 0;
            foreach (var column in columns)
            {
                fields[index] = column.Name;
                index++;
            }

            return fields;
        }

        public virtual string GetInvOrg(Type type, string[] fields, out int pageSize)
        {
            IList<InvOrg> list;
            if (ContainCurInvorg)
            {
                list = RF.GetAll<InvOrg>().ToList();
            }
            else
            {
                list = RF.GetAll<InvOrg>().Where(p => p.Code != RT.InvOrg).ToList();
            }
            StringBuilder sbCatalog = new StringBuilder();
            sbCatalog.Append("[");
            bool isFirst1 = true;
            foreach (var item in list)
            {
                sbCatalog.Append(isFirst1 ? "" : ",");
                sbCatalog.Append("{");
                bool isFirst2 = true;
                foreach (var field in fields)
                {
                    sbCatalog.Append(isFirst2 ? "" : ",");
                    sbCatalog.AppendFormat("\"{0}\":\"{1}\"", field, type.GetProperty(field).GetValue(item));
                    isFirst2 = false;
                }
                sbCatalog.Append("}");
                isFirst1 = false;
            }
            sbCatalog.Append("]");
            pageSize = list.Count;
            return sbCatalog.ToString();
        }


    }
}
