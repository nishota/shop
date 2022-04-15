namespace api.Models
{
    public enum StockStatus
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
        /// 買い物かご
        /// </summary>
        InBusket,
        /// <summary>
        /// 購入確定
        /// </summary>
        PurchaseConfirmed,
        /// <summary>
        /// 完了(発送済み)
        /// </summary>
        Done,
        /// <summary>
        /// 払込未確定
        /// </summary>
        PaymentNotConfirmed,
        /// <summary>
        /// 取り下げ
        /// </summary>
        Withdrawal,
    }
}
