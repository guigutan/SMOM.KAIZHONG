
/**
 *  异常停线车间下拉值改变事件，重置资源值 
 */
Ext.define('SIE.Web.Items.Control.AbnormalCauseComboList', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.abnormalcausecombolist',
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
        else if (!entity.phantom && entity.modified != undefined && entity.modified.ShopId != entity.data.ShopId) {//编辑修改时重置
            entity.modified.ShopId = entity.data.ShopId;
            entity.setResource(null);
        }
        else if (entity.phantom && entity.modified.ShopId == null) {//新增时给修改赋值，方便判断
            entity.modified.ShopId = entity.data.ShopId;
        }
        else if (entity.phantom && entity.modified.ShopId != entity.data.ShopId) {
            entity.modified.ShopId = entity.data.ShopId;
            entity.setResource(null);
        }
    }
});