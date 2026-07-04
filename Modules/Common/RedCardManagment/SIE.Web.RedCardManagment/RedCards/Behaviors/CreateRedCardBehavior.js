Ext.define('SIE.Web.RedCardManagment.RedCards.Behaviors.CreateRedCardBehavior',
{
    onCreated: function (view) {
        var params = CRT.Context.PageContext.getParams();
        var entity = CRT.Context.PageContext.getCurrentRecord();
        if (params) {
            //把配置项获取到的值设置到页面上
            entity.setNo(params.No.No);
        }
    },
})
