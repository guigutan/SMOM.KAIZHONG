Ext.define('SIE.Web.Fixtures.Repairs.Script.AddFixtureRepairBehavior',
    {
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {
            var me = this;
            var entity = view.getCurrent();
            var params = CRT.Context.PageContext.getParams();
            if (params) {
                entity.data.No = params.No;
                entity.data.RepairState = params.RepairState;
                entity.data.ApplyById = params.ApplyById;
                entity.data.ApplyByName = params.ApplyByName;
                entity.data.ApplyDate = new Date(params.ApplyDate);
                view.tabId = params.tabId;
            }
        },
    });