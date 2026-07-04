/*异常编码编辑器*/
Ext.define('SIE.Web.AbnormalInfo.AbnormalInfos.Editors.AbnormalCodeEditor', {
    extend: 'Ext.util.Observable',
    constructor: function () {
        this.callParent(arguments);
    },
    onClick: function (field, trigger, e) {
        var me = this;
        var cur;
        var token;
        if (field.readOnly) {
            return;
        }
        cur = field.up().context.record;
        if (field.up().context.view.ownerCt.SIEView) {
            token = field.up().context.view.ownerCt.SIEView.token;
        }
        SIE.AutoUI.getMeta({
            model: "SIE.Web.AbnormalInfo.AbnormalInfos.ViewModels.AbnormalCodeViewModel",
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = new detailView._model();
                if (cur.data.AbnormalSource === 0) {
                    detailView.getControl().getForm().getFields().items[1].hide();
                    entity.setAlerterId(cur.data.AlerterId);
                    entity.setAlerterId_Display(cur.data.Code);
                }
                else {
                    detailView.getControl().getForm().getFields().items[0].hide();
                    entity.setCode(cur.data.Code);
                }
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "异常编码".t(),
                    items: ui,
                    width: 240,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            if (cur.data.AbnormalSource === 0) {
                                cur.setAlerterId(entity.data.AlerterId);
                                cur.setCode(entity.data.AlerterId_Display);
                                cur.setDesc(entity.data.AlerterName);
                            } else {
                                cur.setCode(entity.data.Code);
                            }
                        }
                    }
                });
            },
        });  
    },
});
