Ext.define('SIE.Web.EMS.AssetReturns.Scripts.MutilAssetIssueFixtureEditor', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.mutilIssueFixtureEditor',
    setMULTIValue: function () {
        var me = this;
        var displayVal = "";
        me._targetSelectItems.items.forEach(function (model) {
            displayVal += me.separator + model.data.FixtureEncode;
        });
        var entity;
        if (!me.up("form"))
            entity = me.up("container").context.record;
        else
            entity = me.up("form").SIEView.getData();
        if (entity) {
            entity.setFixtureCodes(displayVal.substr(1));
        }
    },
    /**
      * 创建界面布局
      * @param field 
      */
    _createLayout: function (field) {
        var me = this;
        if (!me.model)
            SIE.Msg.showWarning('请设置数据关联实体'.t());

        var formEntity = me.up("container").up().up().SIEView.getParent().getCurrent();

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
                    "Method": "GetAssetReturnFixturesById", "Parameters": [formEntity.data.Id, formEntity.data.AssetRequisitionId]
                });
                ui._view.loadData({
                    filter: filter,
                    action: 'queryer',
                    token: ui._view.config.token,
                    type: 'SIE.Web.EMS.AssetReturns.DataQueryer.AssetReturnDataQueryer',
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

            me.setMULTIValue();
            me._win.hide();
            return true; //阻止窗口关闭，在save中根据返回结果处理
        } else if (btn === '取消'.t()) {
            me.isCanceling = true;
            return true;
        }
    }
});