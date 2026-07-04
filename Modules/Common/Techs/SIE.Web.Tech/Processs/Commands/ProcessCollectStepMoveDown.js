SIE.defineCommand('SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveDown', {
    extend: 'SIE.Web.Common.Sort.Commands.MoveDownCommand',
    meta: { text: "下移", group: "edit", iconCls: "icon-ArrowLongDown icon-blue" },
    execute: function (listView, source) {
        var me = listView;
        var data = listView.getData();
        var items = data.data.items;
        var entity = listView.getCurrent();
        var index = this.getItemsIndex(items, entity);
        //交换序列位置
        var dataindex = 0;
        dataindex = items[index].data.INDEX_;
        items[index].data.INDEX_ = items[index + 1].data.INDEX_;
        items[index + 1].data.INDEX_ = dataindex;
        items[index].dirty = true;
        items[index + 1].dirty = true;


        //Step交换
        var stepindex = items[index].data.Step;
        items[index].data.Step = items[index + 1].data.Step;
        items[index + 1].data.Step = stepindex;

        items.splice(index, 1);
        items.splice(index + 1, 0, entity);
        this.setItems(data, items);

        //当前选中
        listView.setCurrent(null);
        listView.setCurrent(entity);

        var EntityMetadataList = [];
        EntityMetadataList.push(items[index].data);
        EntityMetadataList.push(items[index + 1].data);
        listView.execute({
            data: EntityMetadataList,
            success: function (res) {
                return true;
            }
        });
    },
});