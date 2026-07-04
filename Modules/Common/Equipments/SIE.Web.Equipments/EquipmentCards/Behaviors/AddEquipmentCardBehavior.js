Ext.define('SIE.Web.Equipments.EquipmentCards.Behaviors.AddEquipmentCardBehavior',
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
            if (entity.getCode() != "") {
                SIE.invokeDataQuery({
                    type: "SIE.Web.Equipments.EquipmentCards.DataQuery.EquipmentCardDataQuery",
                    method: "GetEquipmentCardById",
                    params: [entity.getId()],
                    async: false,
                    token: view.token,
                    callback: function (res) {
                        entity.setAssetCode(res.Result.data.items[0].getAssetCode());
                        entity.setAssetName(res.Result.data.items[0].getAssetName());
                        entity.setOriginalValue(res.Result.data.items[0].getOriginalValue());
                        entity.setIssAsset(res.Result.data.items[0].getIssAsset());
                    },
                });
            }
            if (entity.getCode() == "") {
                SIE.invokeDataQuery({
                    type: "SIE.Web.Equipments.EquipmentCards.DataQuery.EquipmentCardDataQuery",
                    method: "GetEquipAccountNo",
                    params: [],
                    async: false,
                    token: view.token,
                    callback: function (res) {
                        var myDate = new Date();
                        entity.setCode(res.Result);
                        entity.setAccountState(1);//默认设备状态
                        entity.setAccountUseState(5);//默认管理状态
                        entity.setApprovalStatus(10);//默认审核状态
                        entity.setEnterDate(myDate);//默认入场日期
                        entity.setEquipmentCardSource(70);//默认卡片来源
                    },
                });
            }
            SIE.invokeDataQuery({
                type: "SIE.Web.Equipments.EquipmentCards.DataQuery.EquipmentCardDataQuery",
                method: "GetIsEnableAsset",
                params: [],
                async: false,
                token: view.token,
                callback: function (res) {
                    entity.setIsEnableAsset(res.Result);
                },
            });
        }
    });