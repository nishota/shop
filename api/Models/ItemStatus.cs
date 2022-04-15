namespace api.Models
{
    /// <summary>
    /// 商品ステータス
    /// </summary>
    public enum ItemStatus
    {
        /// <summary>
        /// 準備中
        /// </summary>
        Preparing,
        /// <summary>
        /// 販売中
        /// </summary>
        Available,
        /// <summary>
        /// 売り切れ
        /// </summary>
        SoldOut,
        /// <summary>
        /// 終売
        /// </summary>
        Unavailable
    }
}
