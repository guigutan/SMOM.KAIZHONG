Ext.define('SIE.Web.EMS.SpareParts.OutDepots.Behaviors.PartOutDepotDetailBehavior',
{
        /**
        * view生命周期函数--view生成前
        * @param {*} meta 实体视图元数据
        * @param {*} curEntity 当前操作实体(可空)
        */
        beforeCreate: function (meta, curEntity) {
            if (!meta)
                return;

            var gridConfig = meta.gridConfig;

            var toolBar = gridConfig.dockedItems.first(function (p) { return p.xtype == "toolbar" });
            if (!Ext.isEmpty(toolBar)) {

                var txtKeyword = new Ext.form.TextField({
                    width: 300,
                    allowBlank: true,
                    name: 'txtKeyword',
                    labelAlign: 'right',
                    fieldLabel: '',
                    emptyText: '输入备件编码/名称/规格型号/批次号/序列号'.t(),
                });

                //控件插入工具栏
                toolBar.items.unshift(txtKeyword);
            }

        }
    });
