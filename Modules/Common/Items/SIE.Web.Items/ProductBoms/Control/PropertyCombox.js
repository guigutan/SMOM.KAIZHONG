
/**
 *  异常停线车间下拉值改变事件，重置资源值 
 */
Ext.define('SIE.Web.Items.ProductBoms.Control.PropertyCombox', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.propertyCombox',
    listeners: {
        select: function (combo, record, index) {
            var me = this;
            if (me.up("form")) {
                entity = me.up("form").SIEView.getData();
            } else {
                entity = me.up('container').context.record
            }
            if (entity != null) {
                me.onTypeChanged(entity);
            }
        }
    },
    onTypeChanged: function (entity) {
        if (!entity.phantom && entity.modified == undefined) {
        }
        else if (!entity.phantom && entity.modified != undefined && entity.modified.DefinitionId != entity.data.DefinitionId) {//编辑修改时重置
            entity.modified.DefinitionId = entity.data.DefinitionId;
            entity.setDisplayValues("");
            entity.setBomDetailValue("");
            entity.setBomValue("");
            entity.setWoBomValue("");
        }
        else if (entity.phantom && entity.modified.DefinitionId == null) {//新增时给修改赋值，方便判断
            entity.modified.DefinitionId = entity.data.DefinitionId;
        }
        else if (entity.phantom && entity.modified.DefinitionId != entity.data.DefinitionId) {
            entity.modified.DefinitionId = entity.data.DefinitionId;
            entity.setDisplayValues("");
            entity.setBomDetailValue("");
            entity.setBomValue("");
            entity.setWoBomValue("");
        }
    }
});