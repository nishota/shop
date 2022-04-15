namespace app.Models
{
    // TODO: 本当はランダムな文字列とかにしたほうがベター
    //       リクエスト内容から何の権限か見えてしまう可能性がある
    public static class RoleName
    {
        public const string Administrator = "Administrator";
        public const string Customer = "Customer";
    }
}
