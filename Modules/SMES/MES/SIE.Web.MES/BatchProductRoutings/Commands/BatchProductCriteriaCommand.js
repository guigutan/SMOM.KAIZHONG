/*
 *批次产品工艺路线自定义查询JS
 * @class SIE.Web.MES.BatchProductRoutings.BatchProductCriteriaCommand
 */
SIE.defineCommand('SIE.Web.MES.BatchProductRoutings.BatchProductCriteriaCommand', {
    meta: { text: "查询", iconCls: "icon-Search icon-blue" },
    /**
    * @property {Boolean}
    * 是否允许查询，反正恶意查询 
    */
    allow: true,

    canExecute: function (view) {
        var current = view.getCurrent();
        return this.allow && current !== null;
    },

    execute: function (view) {
        var me = this;
        try {
            me.allow = false;
            var record = view.getCurrent();
            delete record.data['CriteriaModuleKey'];
            delete record.data['CriteriaType'];
            delete record.data["CriteriaString"];
            SIE.invokeDataQuery({
                method: 'GetBatchInfoList',
                params: [record.data],
                action: 'queryer',
                type: 'SIE.Web.MES.BatchProductRoutings.BatchProductRoutingDataQueryer',
                token: view.parentView.getToken(),
                success: function (res) {
                    var layout = view.parentView.layout;
                    if (!layout)
                        return;
                    layout.setBarcodeData(res.Result);
                    me.allow = true;
                }
            });
        } catch (e) {
            me.allow = true;
            throw e;
        }
    }
});