modules.define("dateHelper", [], function () {

    /**
     * 获取当前月的第一天
     */
    function getCurrentMonthFirst() {
        var date = new Date();
        date.setDate(1);
        return date;
    }
    /**
     * 获取当前月的最后一天
     */
    function getCurrentMonthLast() {
        return new Date(year, month +1, 0);
    }


    return {
        /**
         * 获取当前月的第一天
         */
        getCurrentMonthFirst: getCurrentMonthFirst,
        /**
     * 获取当前月的最后一天
     */
        getCurrentMonthLast: getCurrentMonthLast
    }
});

