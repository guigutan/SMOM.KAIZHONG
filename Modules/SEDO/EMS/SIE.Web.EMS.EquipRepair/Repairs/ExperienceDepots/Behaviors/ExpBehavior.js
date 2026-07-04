Ext.define('SIE.Web.EMS.EquipRepair.Repairs.ExperienceDepots.Behaviors.ExpBehavior',
    {
        //view数据加载后
        onDataLoaded: function (view) {
            var entity = view.getData();
            view.mon(entity, 'propertyChanged', this._propertyChanged, view);

        },

        _propertyChanged: function (e) {
            var me = this;

            var entity = e.entity;//变更的实体
            var property = e.property;//变更的字段
            var accountId = entity.getEquipAccountId();

            
            //更改为设备台账的话
            if (property == "EquipAccountId") {

                SIE.invokeDataQuery({
                    method: 'GetEquipModelType',
                    params: [accountId],
                    async: false,
                    action: 'queryer',
                    type: 'SIE.Web.EMS.EquipRepair.Repairs.ExperienceDepots.DataQuerys.ExperienceDepotDataQuery',
                    token: me.token,
                    success: function (res) {
                        if (res.Result == null) {
                            return;
                        }
                        //var equipModel = res.Result.data.items[0];
                        var equipModel = res.Result.EquipModel;
                        var equipType = res.Result.EquipType;


                        if (equipModel != null) {
                            entity.setEquipModelId_Display(equipModel.Code);
                            entity.setEquipModelId(equipModel.Id);
                        } else {
                            entity.setEquipModelId_Display();
                            entity.setEquipModelId();
                        }
                        if (equipType != null) {
                            entity.setEquipTypeId_Display(equipType.TypeCode);
                            entity.setEquipTypeId(equipType.Id);
                        } else {
                            entity.setEquipTypeId_Display();
                            entity.setEquipTypeId();
                        }
                    }
                });
            }        
        }
    });
