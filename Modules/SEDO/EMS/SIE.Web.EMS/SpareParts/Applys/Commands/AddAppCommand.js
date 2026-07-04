SIE.defineCommand('SIE.Web.EMS.SpareParts.Applys.Commands.AddAppCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        return true;
    },
    createNewItem: function () {
        var item = this.view.createNewItem()//新增时，生成实体
        var token = this.view.token;//获取页面的token 
        var _view = this.view;
        SIE.invokeDataQuery({
            //请求的方法
            method: 'GetNo',
            //参数
            params: [],
            action: 'queryer',
            //后台的命名空间及类
            type: 'SIE.Web.EMS.SpareParts.Applys.DataQuerys.SparePartAppDataQuery',
            //必须带上token
            token: token,
            //调用成功的回调函数
            success: function (res) {
                //把返回后台生成的编码，设置到实体上
                item.setNo(res.Result);
                //刷新页面信息（如果更新后，页面处理编辑状态，对应的字段必须要刷新，否则编辑中的控件不会有值）
                //debugger;
                _view.refresh();
            }
        });
        return item;
    },
    onItemCreated: function (entity) {
        if (entity) {
           
            //1.0 绑定属性变更事件
            this.view.mun(entity, 'propertyChanged', this._propertyChanged, this.view);
            this.view.mon(entity, 'propertyChanged', this._propertyChanged, this.view);

        }
    },
    _propertyChanged: function (e) {
        var me = this;
        var entity = e.entity;//变更的实体
        var property = e.property;//变更的字段
        var accountId = entity.getEquipAccountId();

        //debugger;
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