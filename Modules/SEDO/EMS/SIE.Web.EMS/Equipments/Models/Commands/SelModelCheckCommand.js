SIE.defineCommand('SIE.Web.EMS.Equipments.Models.Commands.SelModelCheckCommand', {
    extend: 'SIE.Web.EMS.Common.Commands.SelCheckProjectCommand',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    /**
     * canExecute 是否执行
     * @param {} view 当前视图
     * @returns {}
     */
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();

        if (entity == null || entity.data == null) {
            //所属父对象（设备型号）为空 或 数据为空时，不能点击选择点检项目
            return false;
        }

        //新增时，不能点击选择校验项目
        if (entity.isNew()) {
            return false;
        }

        return true;
    },
});