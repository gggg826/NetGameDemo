using System;
namespace SQLiteUtil.Model
{
	/// <summary>
	/// table_account:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class table_account
	{
		public table_account()
		{}
		#region Model
		private int _table_account_user_id;
		private string _table_account_user_name;
		private string _table_account_user_password;
		/// <summary>
		/// 
		/// </summary>
		public int table_account_user_id
		{
			set{ _table_account_user_id=value;}
			get{return _table_account_user_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string table_account_user_name
		{
			set{ _table_account_user_name=value;}
			get{return _table_account_user_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string table_account_user_password
		{
			set{ _table_account_user_password=value;}
			get{return _table_account_user_password;}
		}
		#endregion Model

	}
}

