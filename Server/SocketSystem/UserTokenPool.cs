using System.Collections.Generic;

namespace SocketSystem
{
    public class UserTokenPool
    {
        private static Stack<UserToken> m_Pool;
        public static UserToken GetOne()
        {
            if (m_Pool == null)
                m_Pool = new Stack<UserToken>();
            if (m_Pool.Count == 0)
            {
                UserToken token = new UserToken();
                m_Pool.Push(token);
                return token;
            }
            else
                return m_Pool.Pop();

        }

        public static void ReturnOne(UserToken token)
        {
            if (m_Pool == null)
                m_Pool = new Stack<UserToken>();
            m_Pool.Push(token);
        }
    }
}
