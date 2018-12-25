namespace DDNS.Entity.Tunnel
{
    public enum TunnelStatusEnum
    {
        /// <summary>
        /// 待审
        /// </summary>
        Audit = 0,

        /// <summary>
        /// 通过
        /// </summary>
        Pass = 1,

        /// <summary>
        /// 拒绝
        /// </summary>
        Refuse = -1
    }
}