Ext.define('SIE.Core.Scripts.Signature', {
    singleton: true,

    /**
     * 导出数据验证是否需要签名
     * @param {any} commandName 命令名称
     * @param {any} parentType 上层类型（有挂菜单的类型）
     * @param {any} view 视图
     * @param {any} callBack 回调函数
     */
    otherCheckIsNeedToSignByParentType: function (commandName, parentType, view, callBack) {        
        var data = { EntityType: view.model, ParentType: parentType, CommandName: commandName, Platform: 2 };
        var med = {};
        SIE.invokeDataQuery({
            async: false,
            method: 'CheckIsNeedToSign',
            type: 'SIE.Web.RBAC.Security.DataQueryers.UserSecurityDataQueryer',
            params: [data],
            success: function (res) {
                var SignConfigData = res.Result;
                if (SignConfigData.IsNeedToSign) {
                    med.entityData = data;                    
                    SIE.SignatureWindow.showSign(med, {}, SignConfigData.IsSignByLogin, callBack);
                }
                else {
                    callBack();
                }
            }
        });
    },
});

