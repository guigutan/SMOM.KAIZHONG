Ext.define('SIE.Web.EMS.Lubrications.Behaviors.AddEquipmentCardBehavior',
    {
        SourceEquipId: null,
        /**
        * view生命周期函数--view聚合后
        * @param {*} view 生成的view
        */
        onViewReady: function (view) {
            var me = this;
            //设置主表model
            var entity = view.getCurrent();
            var params = CRT.Context.PageContext.getParams();
            if (params) {
                entity.data.TreePId = params.TreePId;
                me.SourceEquipId = params.SourceEquipId;
            }
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Lubrications.DataQuery.LubricationDataQuery",
                method: "GetLubricationNo",
                params: [],
                async: false,
                token: view.token,
                callback: function (res) {
                    entity.setLubricationNo(res.Result);
                    entity.setCycleType(1);
                    entity.setBillSourceType(10);
                    entity.setLubricationStatus(10);
                },
            });
        }
    });
