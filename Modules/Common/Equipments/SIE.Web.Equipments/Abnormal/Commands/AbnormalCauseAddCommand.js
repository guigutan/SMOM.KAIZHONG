/*
 ** 异常停线添加命令，初始化时间
 * @class SIE.Web.Equipments.Abnormal.Commands.AbnormalCauseAddCommand
 */
SIE.defineCommand('SIE.Web.Equipments.Abnormal.Commands.AbnormalCauseAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        return true;
    },
    getEditEntity: function () {
        var newEntity = Ext.create(this.view.model);
        if (this.view.isListView) {
            newEntity = this.createNewItem();
        }
        //如果停线来源是“自建”，停线类别为null
        if (newEntity.getSourceType() === "0") {
            newEntity.setExceptionStopType("");
        }
        this.onItemCreated(newEntity);
        return newEntity;
    },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                success: function (res) {
                    var data = res.Result;
                    entity.setBeginDate(data.BeginDate);
                    entity.setCode(data.Code);
                }
            }, me.view);
        }
    }
});