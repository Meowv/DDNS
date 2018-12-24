namespace DDNS.Entity.Verify
{
    public enum VerifyStatusEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 激活账号/重置密码 成功操作标识
        /// </summary>
        Success = 1,

        /// <summary>
        /// 过期
        /// </summary>
        Overdue = 9
    }
}