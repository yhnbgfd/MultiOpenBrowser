namespace MultiOpenBrowser.Base.Generic
{
    /// <summary>
    /// 分页返回值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pageable<T> where T : class
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; } = 1;
        /// <summary>
        /// 总数据数
        /// </summary>
        public int DataCount { get; set; } = 0;
        /// <summary>
        /// 当前页的数据列表
        /// </summary>
        public List<T>? Data { get; set; }

        public static Pageable<T> Default => new();
    }
}
