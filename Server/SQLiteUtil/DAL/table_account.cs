using System;
using System.Data;
using System.Text;
using System.Data.SQLite;

namespace SQLiteUtil.DAL
{
	/// <summary>
	/// 数据访问类:table_account
	/// </summary>
	public partial class table_account
	{
		public table_account()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQLite.GetMaxID("table_account_user_id", "table_account"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int table_account_user_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from table_account");
			strSql.Append(" where table_account_user_id=@table_account_user_id ");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@table_account_user_id", DbType.Int32,8)			};
			parameters[0].Value = table_account_user_id;

			return DbHelperSQLite.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(SQLiteUtil.Model.table_account model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into table_account(");
			strSql.Append("table_account_user_id,table_account_user_name,table_account_user_password)");
			strSql.Append(" values (");
			strSql.Append("@table_account_user_id,@table_account_user_name,@table_account_user_password)");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@table_account_user_id", DbType.Int32,8),
					new SQLiteParameter("@table_account_user_name", DbType.String),
					new SQLiteParameter("@table_account_user_password", DbType.String)};
			parameters[0].Value = model.table_account_user_id;
			parameters[1].Value = model.table_account_user_name;
			parameters[2].Value = model.table_account_user_password;

			int rows=DbHelperSQLite.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(SQLiteUtil.Model.table_account model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update table_account set ");
			strSql.Append("table_account_user_name=@table_account_user_name,");
			strSql.Append("table_account_user_password=@table_account_user_password");
			strSql.Append(" where table_account_user_id=@table_account_user_id ");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@table_account_user_name", DbType.String),
					new SQLiteParameter("@table_account_user_password", DbType.String),
					new SQLiteParameter("@table_account_user_id", DbType.Int32,8)};
			parameters[0].Value = model.table_account_user_name;
			parameters[1].Value = model.table_account_user_password;
			parameters[2].Value = model.table_account_user_id;

			int rows=DbHelperSQLite.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int table_account_user_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from table_account ");
			strSql.Append(" where table_account_user_id=@table_account_user_id ");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@table_account_user_id", DbType.Int32,8)			};
			parameters[0].Value = table_account_user_id;

			int rows=DbHelperSQLite.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string table_account_user_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from table_account ");
			strSql.Append(" where table_account_user_id in ("+table_account_user_idlist + ")  ");
			int rows=DbHelperSQLite.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public SQLiteUtil.Model.table_account GetModel(int table_account_user_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select table_account_user_id,table_account_user_name,table_account_user_password from table_account ");
			strSql.Append(" where table_account_user_id=@table_account_user_id ");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@table_account_user_id", DbType.Int32,8)			};
			parameters[0].Value = table_account_user_id;

			SQLiteUtil.Model.table_account model=new SQLiteUtil.Model.table_account();
			DataSet ds=DbHelperSQLite.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public SQLiteUtil.Model.table_account DataRowToModel(DataRow row)
		{
			SQLiteUtil.Model.table_account model=new SQLiteUtil.Model.table_account();
			if (row != null)
			{
				if(row["table_account_user_id"]!=null && row["table_account_user_id"].ToString()!="")
				{
					model.table_account_user_id=int.Parse(row["table_account_user_id"].ToString());
				}
				if(row["table_account_user_name"]!=null)
				{
					model.table_account_user_name=row["table_account_user_name"].ToString();
				}
				if(row["table_account_user_password"]!=null)
				{
					model.table_account_user_password=row["table_account_user_password"].ToString();
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select table_account_user_id,table_account_user_name,table_account_user_password ");
			strSql.Append(" FROM table_account ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQLite.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM table_account ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQLite.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.table_account_user_id desc");
			}
			strSql.Append(")AS Row, T.*  from table_account T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQLite.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@tblName", DbType.VarChar, 255),
					new SQLiteParameter("@fldName", DbType.VarChar, 255),
					new SQLiteParameter("@PageSize", DbType.Int32),
					new SQLiteParameter("@PageIndex", DbType.Int32),
					new SQLiteParameter("@IsReCount", DbType.bit),
					new SQLiteParameter("@OrderType", DbType.bit),
					new SQLiteParameter("@strWhere", DbType.VarChar,1000),
					};
			parameters[0].Value = "table_account";
			parameters[1].Value = "table_account_user_id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQLite.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

