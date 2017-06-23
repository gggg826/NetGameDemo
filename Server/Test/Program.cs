using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace Test
{
	class Program
	{
		static void Main(string[] args)
		{
			SQLiteUtil.DbHelperSQLite.connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
			SQLiteUtil.BLL.table_account accountBLL = new SQLiteUtil.BLL.table_account();
			while (true)
			{
				Console.WriteLine("请输入要进行的操作 \r\n1.查询  2.写入");
				string i = Console.ReadLine();
				int put = Convert.ToInt32(i);
				if (put == 1)
				{
					List<SQLiteUtil.Model.table_account> accounts = accountBLL.GetModelList("");
					for (int index = 0; index < accounts.Count; index++)
					{
						string cout = string.Format("{0}. ID:{3} Name:{1} Password:{2}\r\n"
							                      , index, accounts[index].table_account_user_name
							                      , accounts[index].table_account_user_password
												  , accounts[index].table_account_user_id);
						Console.WriteLine(cout);
					}
					Console.WriteLine("按任意键继续...");
					Console.ReadKey();
				}
				else
				{
					SQLiteUtil.Model.table_account account = new SQLiteUtil.Model.table_account();
					account.table_account_user_id       = GetMD5String(DateTime.Now.ToString() + Guid.NewGuid().ToString()).GetHashCode();
					account.table_account_user_name     = GetUserNameRandom(account.table_account_user_id);
					account.table_account_user_password = Guid.NewGuid().ToString();

					if (accountBLL.Add(account))
					{
						Console.WriteLine("写入成功！按任意键继续...");
						Console.ReadKey();
					}
				}
			}
		}

		static string GetMD5String(string str)
		{
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(str));
			StringBuilder strBuilder = new StringBuilder();
			for (int i = 0; i < result.Length; i++)
			{
				strBuilder.Append(result[i].ToString("x2"));
			}
			return strBuilder.ToString();
		}






		private static char[] ran = new char[] { 'Q', 'w', 'E', '8', 'a', 'S', '2', 'd', 'Z', 'x', '9', 'c', '7', 'p', 'O', '5', 'i', 'K', '3', 'm', 'j', 'U', 'f', 'r', '4', 'V', 'y', 'L', 't', 'N', '6', 'b', 'g', 'H' };
		private static char[] replenish = new char[] { 'q', 'W', 'e', 'A', 's', 'D', 'z', 'X', 'C', 'P', 'o', 'I', 'k', 'M', 'J', 'u', 'F', 'R', 'v', 'Y', 'T', 'n', 'B', 'G', 'h' };
		private static int length = ran.Length;
		private static int finalLength = 8;
		public static string GetUserNameRandom(long longNum)
		{
			char[] buf = new char[32];
			int charPos = 32;
			long num = Math.Abs(longNum);
			while ((num / length) > 0)
			{
				buf[--charPos] = ran[(int)(num % length)];
				num /= length;
			}
			buf[--charPos] = ran[(int)(num % length)];
			string str = new string(buf, charPos, (32 - charPos));
			if (str.Length < finalLength)
			{
				StringBuilder sb = new StringBuilder();
				Random random = new Random();
				for (int i = 0; i < finalLength - str.Length; i++)
				{
					sb.Append(replenish[random.Next(24)]);
				}
				str += sb.ToString();
			}
			return str;
		}
	}
}
