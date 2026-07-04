Ext.define('SIE.Web.Items.Items.Behaviors.ItemClearBehavior', {
    listeners: {
        el: {
            copy: async function (event) {
                if (navigator.clipboard != undefined) {
                    var selection = window.getSelection();
                    var selectedText = selection.toString();
                    // 替换 &amp; 符号为 &
                    var decodedText = selectedText.replace(/&amp;/g, '&');
                    event.preventDefault();
                    try {
                        await navigator.clipboard.writeText(decodedText);
                    } catch (err) {

                    }
                }
            }
        }
    },
    onShow: function (view) {
        // 获取 Ext.Viewport 的实例
        var viewportInstance = Ext.ComponentQuery.query('viewport')[0];

        // 如果 Ext.Viewport 实例存在
        if (viewportInstance) {
            // 创建一个 SIE.Web.Items.Items.Behaviors.ItemClearBehavior 实例
            var itemClearBehavior = Ext.create('SIE.Web.Items.Items.Behaviors.ItemClearBehavior');

            // 如果存在 listeners.el.copy
            if (itemClearBehavior.listeners && itemClearBehavior.listeners.el && itemClearBehavior.listeners.el.copy) {
                // 在 Ext.Viewport 实例的元素上添加 'copy' 事件侦听器
                viewportInstance.el.on('copy', itemClearBehavior.listeners.el.copy);
            }
        }
    }
});


