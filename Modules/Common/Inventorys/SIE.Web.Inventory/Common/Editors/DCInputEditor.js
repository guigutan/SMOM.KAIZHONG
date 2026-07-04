/*电子行业套件D/C输入编辑器*/
Ext.define('SIE.Web.Inventory.Editors.DCInputEditor', {
    extend: 'Ext.util.Observable',
    constructor: function () {
        this.callParent(arguments);
    },
    onClick: function (field, trigger, e) {
        var me = this;
        var cur;
        var token;
        var isNotAsn;
        if (field.readOnly) {
            return;
        }
        if (me.up('form')) {
            isNotAsn = field.up().ownerCt;
            cur = field.up().ownerCt.SIEView.getCurrent();
            token = field.up().ownerCt.SIEView.token;
        }
        else {
            cur = field.up().context.record;
            if (field.up().context.view.ownerCt.SIEView) {
                token = field.up().context.view.ownerCt.SIEView.token;
                isNotAsn = field.up().context.view.ownerCt;
            }
            else {
                token = field.up('gridpanel').ownerCt.SIEView.token;
                isNotAsn = field.up('gridpanel').ownerCt;
            }
        }

        var isYearWeek = me.config.IsYearWeek;
        var isWeekYear = me.config.IsWeekYear;
        var isYearMonthDay = me.config.IsYearMonthDay;

        SIE.AutoUI.getMeta({
            model: "SIE.Web.Inventory.Common.DCInputViewModel",
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
                entity.setYearWeek(isYearWeek);
                entity.setWeekYear(isWeekYear);
                entity.setYearMonthDay(isYearMonthDay);
                entity.setInput(cur.getLotAtt07());
                entity.setTransform(Ext.Date.format(cur.getLotAtt01(), 'Y-m-d'));
                detailView.setData(entity);
                if (entity) {
                    detailView.mon(entity, 'propertyChanged', SIE.Web.Inventory.Commom.DCInputAction.onEntityPropertyChanged, detailView);
                }
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "D/C输入".t(),
                    items: ui,
                    width: 500,
                    height: 300,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            me.config.IsYearWeek = entity.data.YearWeek;
                            me.config.IsWeekYear = entity.data.WeekYear;
                            me.config.IsYearMonthDay = entity.data.YearMonthDay;
                            cur.setLotAtt07(entity.data.Input);   
                            if (entity.data.Transform != "") {
                                cur.setLotAtt01(entity.data.Transform);
                                if (cur.data.ShelfLife > 0) {
                                    var invalidDate = SIE.Web.Inventory.Commom.DCInputAction.getInvalidDate(entity.data.Transform, cur.data.ShelfLife);
                                    cur.setLotAtt02(invalidDate);
                                }
                            }
                        }
                    }
                });
            },
        });        
    },

});