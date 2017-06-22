using System;
using System.Data;
using System.Collections.Generic;

namespace SQLiteUtil.BLL
{
	/// <summary>
	/// table_account
	/// </summary>
	public partial class table_account
	{
		private readonly SQLiteUtil.DAL.table_account dal=new SQLiteUtil.DAL.table_account();
		public table_account()
		{}
		#region  BasicMethod

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int table_account_user_id)
		{
			return dal.Exists(table_account_user_id);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Add(SQLiteUtil.Model.table_account model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(SQLiteUtil.Model.table_account model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int table_account_user_id)
		{
			
			return dal.Delete(table_account_user_id);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool DeleteList(string table_account_user_idlist )
		{
			return dal.DeleteList(Maticsoft.Common.PageValidate.SafeLongFilter(table_account_user_idlist,0) );
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public SQLiteUtil.Model.table_account GetModel(int table_account_user_id)
		{
			
			return dal.GetModel(table_account_user_id);
		}

		/// <summary>
		/// �õ�һ������ʵ�壬�ӻ�����
		/// </summary>
		public SQLiteUtil.Model.table_account GetModelByCache(int table_account_user_id)
		{
			
			string CacheKey = "table_accountModel-" + table_account_user_id;
			object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(table_account_user_id);
					if (objModel != null)
					{
						int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (SQLiteUtil.Model.table_account)objModel;
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<SQLiteUtil.Model.table_account> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<SQLiteUtil.Model.table_account> DataTableToList(DataTable dt)
		{
			List<SQLiteUtil.Model.table_account> modelList = new List<SQLiteUtil.Model.table_account>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				SQLiteUtil.Model.table_account model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// ��ҳ��ȡ�����б�
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// ��ҳ��ȡ�����б�
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
		/// <summary>
		/// ��ҳ��ȡ�����б�
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

