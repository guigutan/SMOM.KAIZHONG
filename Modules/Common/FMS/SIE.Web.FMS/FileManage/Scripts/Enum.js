/*文件管理模块枚举*/
Ext.define("SIE.Enum.FMS", {
    /**
    * 文件状态
    */
    FileState: {
        /**
         * 草稿
         */
        Created: 0,
        /**
         * 发布
         */
        Release: 1,
        /**
         * 修订
         */
        Edit: 2,
        /**
        * 审核中
        */
        Audit: 3,
        /**
         * 待发布
         */
        ToRelease: 4,
        /**
         * 作废待审核
         */
        ToScrap: 5,
        /**
         * 作废待发布
         */
        ScrapToRelease: 6,
        /**
         * 作废
         */
        Scrap: 7
    }

});