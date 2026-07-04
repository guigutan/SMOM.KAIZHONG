Ext.define('SIE.Web.Fixtures.Models.Scripts.MutilCatalogEditor', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.mutilcatalogeditor_encodewarehouse',
    setMULTIValue: function () {
        var me = this;
        var displayVal = "";
        var displayCodeVal = "";
        var ids = "";
        me._targetSelectItems.items.forEach(function (model) {
            displayVal += me.separator + model.data.Name;
            displayCodeVal += me.separator + model.data.Code;
            ids += me.separator + model.data.Id;
        });
        var entity;
        if (!me.up("form"))
            entity = me.up("container").context.record;
        else
            entity = me.up("form").SIEView.getData();
        if (entity) {
            entity.setWareHouseTypeName(displayVal.substr(1));
            entity.setWareHouseTypeCode(displayCodeVal.substr(1));
            entity.setWareHouseTypeIds(ids.substr(1));
        }
    },
    /**
      * 创建界面布局
      * @param field 
      */
    _createLayout: function (field) {
        var me = this;
        if (!me.model)
            SIE.Msg.showWarning('请设置数据关联实体'.L10N());
        var entity;
        if (!me.up("form"))
            entity = me.up("container").context.record;
        else
            entity = me.up("form").SIEView.getData();
        SIE.AutoUI.getMeta({
            model: me.model,
            ignoreChild: true,
            ignoreCommands: true,
            isReadonly: true,
            ignoreQuery: false,
            isAggt: true,
            callback: function (blocks) {
                me._gridBlockProcess(blocks);
                var ui = SIE.AutoUI.generateAggtControl(blocks);
                me._popupWin(ui, me.inputEl);
                var filter = Ext.encode({
                    "Method": "GetCalalogForFixtureWarehouseType", "Parameters": []
                });
                ui._view.loadData({
                    filter: filter,
                    action: 'queryer',
                    token: ui._view.config.token,
                    type: 'SIE.Web.Fixtures.Models.DataQueryers.FixtureEncodeDataQueryer',
                });
                me._layouted = true;
            }
        });
    },
    /**
* 确定事件
* @param btn--
* @returns
*/
    onpopupWinbtn: function (btn) {
        var me = this;
        if (btn === '确定'.t()) {
            var entity;
            me.setMULTIValue();
            if (!me.up("form"))
                entity = me.up("container").context.record;
            else
                entity = me.up("form").SIEView.getData();
            me._win.hide();
            return true; //阻止窗口关闭，在save中根据返回结果处理
        } else if (btn === '取消'.t()) {
            me.isCanceling = true;
            return true;
        }
    }
});