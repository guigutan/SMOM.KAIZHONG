Ext.define('SIE.Web.MES.Routings.Scripts.RoutingBomCommonFun', {
    statics: {

        /**
         * 工序BOM属性变更事件
         * @param {any} e
         */
        onRoutingBomAddPropertyChanged: function (e) {
            SIE.Web.MES.Routings.Scripts.RoutingBomCommonFun.onRoutingBomCommonProc(e.property, this.view, e.entity);
        },

        /**
         * 工序BOM属性变更事件
         * @param {any} e
         */
        onRoutingBomEditPropertyChanged: function (e) {
            SIE.Web.MES.Routings.Scripts.RoutingBomCommonFun.onRoutingBomCommonProc(e.property, this.view, e.entity);
        },

        /**
         * 通用属性变更事件
         * @param {any} e
         */
        onRoutingBomCommonProc: function (property, view, entity) {
            if (property.length > 0 && entity != null) {
                if (property == "RoutingId") {
                    entity.setRoutingVersionId(null);
                    entity.setRoutingVersionId_Display('');
                    entity.setProcessSegmentId(null);
                    entity.setProcessSegmentId_Display('');
                    var routingId = entity.data.RoutingId
                    var productId = entity.data.ProductId;
                    SIE.invokeDataQuery({
                        type: "SIE.Web.MES.Routings.RoutingBoms.DataQueryers.RoutingBomDetailDataQuery",
                        method: "GetDefaultRoutingVersionInfo",
                        params: [routingId, productId],
                        async: false,
                        token: view.token,
                        callback: function (res) {
                            if (res.Success) {
                                var defaultInfo = res.Result;
                                if (defaultInfo != null) {
                                    entity.setRoutingVersionId(defaultInfo.RoutingVersionId);
                                    entity.setRoutingVersionId_Display(defaultInfo.RoutingVersionName);
                                    entity.setProcessSegmentId(defaultInfo.ProccessSegmentId);
                                    entity.setProcessSegmentId_Display(defaultInfo.ProccessSegmentCode);
                                }
                            }
                        }
                    });
                }
            }
        },
        /**
         * 工序BOM明细属性变更事件
         * @param {any} e
         */
        onRoutingBomDetailAddPropertyChanged: function (e) {
            SIE.Web.MES.Routings.Scripts.RoutingBomCommonFun.RoutingBomDetailChanged(e.property, this.view, e.entity);
        },

        /**
         * 工序BOM明细属性变更事件
         * @param {any} e
         */
        onRoutingBomDetailEditPropertyChanged: function (e) {
            SIE.Web.MES.Routings.Scripts.RoutingBomCommonFun.RoutingBomDetailChanged(e.property, this.view, e.entity);
        },

        /**
         * Routing BOM detail changed common function
         * @param {any} e
         */
        RoutingBomDetailChanged: function (property, view, entity) {
            if (property.length > 0 && entity != null) {
                if (property == "ProcessId") {
                    entity.setWorkStepId(null);
                    entity.setWorkStepId_Display("");
                }
            }
        }
    }
});