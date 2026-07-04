Ext.define("SIE.Web.LES.MaterialPreparations.Scripts.SelectWorkOrderEditor", {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.materialprepareselwoeditor',
    setSelValue: function () {
        var me = this;
        var selitem = me._targetSelectItems.items[0];
        if (selitem == null) {
            SIE.Msg.showMessage("请选择工单".t());
            return;
        }
        var sel = selitem.data;
        var entity;
        if (!me.up("form"))
            entity = me.up("container").context.record;
        else
            entity = me.up("form").SIEView.getData();
        if (entity) {
            entity.setWoNo(sel.WoNo);
            entity.setWorkOrderId(sel.Id);

            entity.setFactoryId_Display(sel.Factory);
            entity.setFactoryId(sel.FactoryId);
            entity.setResourceId_Display(sel.WipResource);
            entity.setResourceId(sel.WipResourceId);
            entity.setWorkShopId_Display(sel.Workshop);
            entity.setWorkShopId(sel.WorkshopId);


            entity.setWoProductCode(sel.ProductCode);
            entity.setWoProductName(sel.ProductName);
            entity.setProjectMaintainId_Display(sel.ProjectMaintainCode);
            entity.setProjectMaintainId(sel.ProjectMaintainId);
        }
    },

    /** 
     * 复选框勾选事件
     * @param selModel 选择模式
     * @param record 选择的记录
     * @param index 行索引号
     * @param eOpts  The options object passed to Ext.util.Observable.addListener.
     */
    _onSelect: function (selModel, record, index, eOpts) {
        var idx = Ext.Array.indexOf(this._targetSelectItems.keys, record.getId(), 0);
        if (idx === -1) {
            if (selModel.selectionMode === "SINGLE") {
                this._targetSelectItems.keys = [record.getId()];
                this._targetSelectItems.items = [record];
            }
            else {
                this._targetSelectItems.keys.push(record.getId());
                this._targetSelectItems.items.push(record);
            }
            this._changeSelectionAfterShow = true;
        }
    },

    _popupWin: function (ui, source) {
        var me = this;
        me._targetView = ui._view;
        me._uiControl = ui.getControl();
        //弹窗
        me._win = SIE.Window.show({
            title: '选择'.t() + me._targetView.label.t(),
            animateTarget: source,
            items: ui.getControl(),
            modal: true,
            closeAction: 'hide',
            height: 500,
            width: 1000,
            //buttons: ['确定', '关闭'], //自定义按钮名称
            callback: function (btn) {
                me.onpopupWinbtn(btn);
            }
        });
        me._setGridListeners();
        me._targetSelectItems = {
            items: [],
            keys: []
        };

        me._setWinListeners();
        me.grid = me._targetView.getControl();

        delete me._layouting;
    },
    /**
    * 确定事件
    * @param btn--
    * @returns
    */
    onpopupWinbtn: function (btn) {
        var me = this;
        if (btn === '确定'.t()) {
            me.setSelValue();
            me._win.hide();
            return true; //阻止窗口关闭，在save中根据返回结果处理
        } else if (btn === '取消'.t()) {
            me.isCanceling = true;
            return true;
        }
    }
})