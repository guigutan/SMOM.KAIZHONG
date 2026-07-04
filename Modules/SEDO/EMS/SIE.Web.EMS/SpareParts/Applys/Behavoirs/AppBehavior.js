Ext.define('SIE.Web.EMS.SpareParts.Applys.Behaviors.AppBehavior',
    {
        //view数据加载后
        onDataLoaded: function (view) {
          
            var entity = view.getData();
            view.mun(entity, 'propertyChanged', this._propertyChanged, view);
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
                    method: 'GetEquipModelEnterp',
                    params: [accountId],
                    async: false,
                    action: 'queryer',
                    type: 'SIE.Web.EMS.SpareParts.Applys.DataQuerys.SparePartAppDataQuery',
                    token: me.token,
                    success: function (res) {
                        if (res.Result == null) {
                            return;
                        }
                        var equipModel = res.Result.EquipModel;
                        var enterprise = res.Result.Enterprise;


                        if (equipModel != null) {
                            entity.setEquipModelId_Display(equipModel.Code);
                            entity.setEquipModelId(equipModel.Id);
                        } else {
                            entity.setEquipModelId_Display();
                            entity.setEquipModelId();
                        }

                        if (enterprise != null) {
                            entity.setGetDepartmentId_Display(enterprise.Name);
                            entity.setGetDepartmentId(enterprise.Id);
                        } else {
                            entity.setGetDepartmentId_Display();
                            entity.setGetDepartmentId();
                        }
                    }
                });
            }

            if (property == "EquipModelId") {

                //申请明细
                var childrenView = this.getChildren().find(function (item) {
                    if (item && item.model == "SIE.EMS.SpareParts.Applys.Details.ApplyDetail") {
                        return item;
                    }
                });

                var childenStore = childrenView.getData();
                if (e.value != null) {
                    //选择了设备型号
                    //删除从表的数据                  
                    if (childenStore)
                        childenStore.removeAll();
                }
                else {
                    //清空明细表中视图属性-设备型号Id（原因：明细中备件的下拉框要根据此字段筛选）
                    for (var i = 0; i < childenStore.getData().length; i++) {
                        childenStore.getData().items[i].setEquipModelId(null);
                    }
                }
            }
        }

    });
