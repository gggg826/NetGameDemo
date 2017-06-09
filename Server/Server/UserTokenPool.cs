using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
	public class UserTokenPool
	{
		private static Stack<UserToken> m_Pool;
		public static UserToken GetOne()
		{
			if (m_Pool == null)
				m_Pool = new Stack<UserToken>();
			if (m_Pool.Count == 0)
				return new UserToken();
			else
				return m_Pool.Pop();
		}

		public static void ReturnOne(UserToken userToken)
		{
			if (m_Pool == null)
				m_Pool = new Stack<UserToken>();
			m_Pool.Push(userToken);
		}
	}
}
