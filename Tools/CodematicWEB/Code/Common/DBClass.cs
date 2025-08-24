using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using System.Collections.Generic;


namespace CodematicWEB.Common
{
    public class DBClass
    {

        #region 数据库连接及连接字符串
        //05的写法
        //    private static string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["strconn"].ConnectionString;
        //03的写法
        public static string ConnStr = "";
        public static SqlConnection Conn
        {
            get
            {
                if (string.IsNullOrEmpty(DBClass.ConnStr))
                {
                    //这里读取XML的配置参数
                }
                return new SqlConnection(ConnStr);
            }
        }
        #endregion

        #region SQL 常规操作
        /// <summary>
        /// 数据库增删改操作，获取受到影响的行数
        /// </summary>
        /// <param name="sql"></param> 
        public static int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, null);
        }

        public static int ExecuteNonQuery(string sql, SqlParameter[] pars)
        {
           return ExecuteNonQuery(sql, pars, Conn);
        }

        public static int ExecuteNonQuery(string sql, SqlParameter[] pars, SqlConnection conn)
        {
           
            try
            {
                SqlCommand oComm = new SqlCommand(sql, conn);

                if (pars != null) AddParameter(oComm, pars);

                if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                    conn.Open();
                return oComm.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
              
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }


        /// <summary>
        /// 连接式数据库操作，获取SqlDataReader对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql)
        {
            SqlConnection conn = Conn;
            try
            {
                SqlCommand oComm = new SqlCommand(sql, conn);

                if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                    conn.Open();
                return oComm.ExecuteReader(CommandBehavior.CloseConnection);

            }
            catch (Exception ex)
            {
            
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }

        }

        /// <summary>
        /// 取首行首列数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, null);
        }



        public static string ExecuteScalar(string sql, SqlParameter[] pars)
        {
            return ExecuteScalar(sql, pars, Conn);
        }


        public static string ExecuteScalar(string sql, SqlParameter[] pars, SqlConnection conn)
        {
            
            try
            {
                SqlCommand oComm = new SqlCommand(sql, conn);

                if (pars != null) AddParameter(oComm, pars);

                if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                    conn.Open();
                string result =oComm.ExecuteScalar().ToString();
                return Convert.ToString(result);

            }
            catch (Exception ex)
            {
              
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql)
        {
            return GetDataTable(sql, Conn);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql ,SqlConnection conn)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
           
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }

        }

        #endregion

        #region 存储过程

        /// <summary>
        /// 没有参数的存储过程
        /// </summary>
        /// <param name="pro_Name">存储过程的名称</param>
        /// <returns>执行存储过程的返回值,0为成功.-1为失败.</returns>
        public static int ProcTransfer(string procName)
        {
            return ProcTransfer(procName);
        }

        /// <summary>
        /// 有参数的存储过程
        /// </summary>
        /// <param name="pro_name">存储过程名</param>
        /// <param name="sps">参数</param>
        /// <returns>执行存储过程的返回值,0为成功,-1为失败</returns>
        public static int ProcTransfer(string procName, params SqlParameter[] pars)
        {
            int num = -1;
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(procName, conn);
                comm.CommandTimeout = 60;
                comm.CommandType = CommandType.StoredProcedure;
                if (pars != null) AddParameter(comm, pars);

                SqlParameter pa = new SqlParameter("@returnValue", SqlDbType.Int);
                pa.Direction = ParameterDirection.ReturnValue;
                comm.Parameters.Add(pa);
                try
                {
                    comm.ExecuteNonQuery();
                    num = Convert.ToInt32(pa.Value);
                }
                catch (SystemException ex)
                {
                   
                    throw;
                }
                finally
                {
                    if (conn != null && conn.State == ConnectionState.Open)
                        conn.Close();
                }

            }
            return num;
        }


        /// <summary>
        /// 返回DataSet,无参数
        /// </summary>
        /// <param name="pro_name">存储过程名</param>
        /// <returns>执行存储过程的返回值,0为成功,-1为失败.</returns>
        public static DataSet ProcTransferDataSet(string procName)
        {
            return ProcTransferDataSet(procName, null);
        }

        /// <summary>
        /// 返回DataSet,有参数
        /// </summary>
        /// <param name="pro_Name">存储过程名</param>
        /// <param name="sps">参数</param>
        /// <returns>执行存储过程的返回值.0为成功,-1为失败.</returns>
        public static DataSet ProcTransferDataSet(string procName, params SqlParameter[] pars)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(procName, conn);
                    da.SelectCommand.CommandTimeout = 60;
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    if (pars != null) AddParameter(da.SelectCommand, pars);

                    da.Fill(ds);
                }
                catch (SystemException ex)
                {
            
                    throw;
                }
                finally
                {
                    if (conn != null && conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return ds;
        }

        #endregion

        #region 将参数添加到Command对象中

        /// <summary>
        /// 将参数添加到Command对象中
        /// </summary>
        /// <param name="com">Command对象</param>
        /// <param name="sps">参数</param>
        private static void AddParameter(SqlCommand com, SqlParameter[] sps)
        {
            foreach (SqlParameter sp in sps)
            {
                com.Parameters.Add(sp);
            }
        }
        #endregion

        #region 其它相关数据操作

        ///<summary>
        /// 获取数据表中字段的值
        /// </summary>
        /// <param name="tableName">数据表名称</param>
        /// <param name="columnName">字段名称</param>
        /// <returns>字段类型</returns>
        public static string GetColumnValue(string tableName, string columnName, string id, string keyValue)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}='{3}'", columnName, tableName, id, keyValue);
            return ExecuteScalar(sql);
        }

        /// <summary>
        /// 根据表名和条件获取制定列名的值
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">要获取值的列名</param>
        /// <param name="where">查询条件 格式：AND 1=1</param>
        /// <returns></returns>
        public static string GetColumnValueByWhere(string tableName, string columnName, string where)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE  1=1  {2} ", columnName, tableName, where);

            return ExecuteScalar(sql);
        }


        /// <summary>
        /// 根据SQL语句获取行数，也就是调用sql COUNT() 函数，
        /// 注意SQL语句的编写
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int GetCount(string sql)
        {
            int result = 0;
            try
            {
                result = Convert.ToInt32(ExecuteScalar(sql));
            }
            catch (Exception ex)
            {
               
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 根据表名和条件获取数据，也就是调用sql COUNT() 函数，
        /// 注意格式： ADN  1=1
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int GetCount(string tableName, string where)
        {
            string sql = string.Format("SELECT  COUNT(*) FROM {0} WHERE 1=1 {1}  ", tableName, where);
            return GetCount(sql);
        }

        /// <summary>
        /// 根据表名和条件获取数据，也就是调用sql COUNT() 函数，
        /// 注意格式： ADN  1=1
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int GetCountByTableName(string tableName)
        {
            string sql = string.Format("SELECT  COUNT(*) FROM {0}   ", tableName);
            return GetCount(sql);
        }


        /// <summary>
        /// 更新数据库单个值
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="updateCName">要更新的列名</param>
        /// <param name="updateCValue">要更新的值</param>
        /// <param name="keyCName">主键名称</param>
        /// <param name="keyCValue">主键值/param>
        /// <returns></returns>
        public static int UpdateOnly(string tableName, string updateCName, string updateCValue,
                                            string keyCName, string keyCValue)
        {
            string where = string.Format(" AND  {0}='{1}'   ", keyCName, keyCValue);

            return UpdateOnly(tableName, updateCName, updateCValue, where);
        }

        /// <summary>
        /// 更新数据库单个值
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="updateCName">要更新的列名</param>
        /// <param name="updateCValue">要更新的值</param>
        /// <param name="where">条件  格式  and 1=1</param>
        /// <returns></returns>
        public static int UpdateOnly(string tableName, string updateCName, string updateCValue, string where)
        {
            string sql = string.Format("UPDATE  {0}  SET {1}='{2}'  WHERE  1=1 {3}   ", tableName, updateCName, updateCValue, where);
            return ExecuteNonQuery(sql);
        }


        public static DataTable GetAutoID_Dt(DataTable dt)
        {
            int i = 1;
            dt.Columns.Add(new DataColumn("AutoID", typeof(int)));
            foreach (DataRow dr in dt.Rows)
            {
                dr["AutoID"] = i++;
            }
            return dt;
        }

        #endregion

        #region 获取查询SQL
        public static string GetQuerySQL(string TableName, string fields, string StrWhere, string StrOrder)
        {
            string sql = "select " + fields.Replace("|", ", ") + " from " + TableName;
            if (StrWhere != null)
                sql += " where " + StrWhere;
            if (StrOrder != null)
                sql += " order by " + StrOrder;
            return sql;
        }
        public static string GetQuerySQLBySingle(string TableName, string fields, string KeyID, string strKey)
        {
            return GetQuerySQL(TableName, fields, KeyID + "=" + strKey, null);
        }
        public static string GetQuerySQL(string TableName, string fields, string StrWhere)
        {
            return GetQuerySQL(TableName, fields, StrWhere, null);
        }
        public static string GetQuerySQL(string TableName, string fields)
        {
            return GetQuerySQL(TableName, fields, null, null);
        }
        #endregion

        #region 处理事务,针对主从表
        /// <summary>
        /// 处理主从表事务
        /// </summary>
        /// <param name="sql1">sql</param>
        /// <param name="para1">SqlParameter[]</param>
        /// <param name="sql2">sql</param>
        /// <param name="para2">SqlParameter[]</param>
        /// <returns>0：成功;-1：失败</returns>
        public static int Tran_Transfer(string sql1, SqlParameter[] para1, string sql2, SqlParameter[] para2)
        {
            int rtn = 0;
            using (SqlConnection conn = Conn)
            {
                //打开连接 
                if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                    conn.Open();
                //创建事务 
                SqlTransaction trans = conn.BeginTransaction();

                SqlCommand comm1 = new SqlCommand(sql1, conn);

                AddParameter(comm1, para1);
                comm1.Transaction = trans;
                SqlCommand comm2 = new SqlCommand(sql2, conn);
                AddParameter(comm2, para2);
                comm2.Transaction = trans;
                try
                {
                    comm1.ExecuteNonQuery();
                    comm2.ExecuteNonQuery();
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    rtn = -1;
                }
                finally
                {
                    conn.Close();
                }
            }
            return rtn;
        }


        /// <summary>
        /// 处理主从表事务
        /// </summary>
        /// <param name="sql1">sql</param>
        /// <param name="sql2">sql</param>
        /// <returns>0：成功;-1：失败</returns>
        public static int TranTransfer(string sql1, string sql2)
        {
            int rtn = 0;
            using (SqlConnection conn = Conn)
            {
                //打开连接 
                if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                    conn.Open();
                //创建事务 
                SqlTransaction trans = conn.BeginTransaction();

                SqlCommand comm1 = new SqlCommand(sql1, conn);
                comm1.Transaction = trans;
                SqlCommand comm2 = new SqlCommand(sql2, conn);
                comm2.Transaction = trans;
                try
                {
                    comm1.ExecuteNonQuery();
                    comm2.ExecuteNonQuery();
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    rtn = -1;
                }
                finally
                {
                    conn.Close();
                }
            }
            return rtn;
        }

        /// <summary>
        /// 事务处理修改、删除事件
        /// </summary>
        /// <param name="oHashtable">Hastable</param>
        /// <param name="strTable">数据库表名称</param>
        /// <param name="type">Add:新增（key为空）；Update：修改，Delete:删除</param>
        /// <param name="Key">主键</param>
        /// <param name="Id">主键值</param>
        /// <returns>返回执行结果，0：成功;-1：失败</returns>
        public static int ModifyGetValue(Hashtable oHashtable, string strTable, string type, string Key, string Id)
        {
            int rtn = 0;
            string sql = "", sqla = "", sqlb = "@";
            string[] arrFields = new string[oHashtable.Count];
            SqlConnection con = Conn;

            if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                con.Open();

            SqlTransaction trans = con.BeginTransaction();//事物对象
            try
            {
                oHashtable.Keys.CopyTo(arrFields, 0);

                sqlb += String.Join(",@", arrFields);
                switch (type)
                {
                    case "Add":
                        sqla = String.Join(",", arrFields);
                        sql = "insert into[" + strTable + "](" + sqla + ") values(" + sqlb + ")";
                        break;
                    case "Update":
                        for (int i = 0; i < arrFields.Length; i++)
                        {
                            sqla += "[" + arrFields[i] + "]=@" + arrFields[i];

                            if (i != arrFields.Length - 1) sqla += ",";
                        }
                        sql = "Update [" + strTable + "] Set " + sqla + " where " + Key + "=" + Id;
                        break;
                    case "Delete":
                        sql = "Delete from " + sqla + " where " + Key + "=" + Id;
                        break;
                }

                SqlCommand com = new SqlCommand(sql, con);//数据操作对象
                com.Connection = con;//指定连接
                com.Transaction = trans;//指定事物

                foreach (string field in oHashtable.Keys)
                {
                    //com.Parameters.Add("@" + field, oHashtable[field]);
                    com.Parameters.AddWithValue("@" + field, oHashtable[field]);
                }

                com.ExecuteNonQuery();//执行该行              

                trans.Commit();//如果全部执行完毕.提交

            }
            catch
            {
                trans.Rollback();//如果有异常.回滚.
                rtn = -1;
            }
            finally
            {
                con.Close();//关闭连接  

            }
            return rtn;
        }
        /// <summary>
        /// 事务处理修改、删除事件
        /// </summary>
        /// <param name="oHashtable">Hastable</param>
        /// <param name="strTable">数据库表名称</param>
        /// <param name="type">Add:新增（key为空）；Update：修改，Delete:删除</param>
        /// <param name="Key">主键</param>
        /// <param name="Id">主键值</param>
        /// <returns>返回执行结果，0：成功;-1：失败</returns>
        public static int ModifyGetValue2(Hashtable oHashtable, string strTable, string type, string Key, string Id)
        {
            int rtn = 0;
            string sql = "", sqla = "", sqlb = "@";
            string[] arrFields = new string[oHashtable.Count];
            SqlConnection con = Conn;

            if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                con.Open();

            SqlTransaction trans = con.BeginTransaction();//事物对象
            try
            {
                oHashtable.Keys.CopyTo(arrFields, 0);

                sqlb += String.Join(",@", arrFields);
                switch (type)
                {
                    case "Add":
                        sqla = String.Join(",", arrFields);
                        sql = "insert into[" + strTable + "](" + sqla + ") values(" + sqlb + ")";
                        break;
                    case "Update":
                        for (int i = 0; i < arrFields.Length; i++)
                        {
                            sqla += "[" + arrFields[i] + "]=@" + arrFields[i];

                            if (i != arrFields.Length - 1) sqla += ",";
                        }
                        sql = "Update [" + strTable + "] Set " + sqla + " where Cast(" + Key + " As Varchar(20))='" + Id + "'";
                        break;
                    case "Delete":
                        sql = "Delete from " + sqla + " where " + Key + "=" + Id;
                        break;
                }

                SqlCommand com = new SqlCommand(sql, con);//数据操作对象
                com.Connection = con;//指定连接
                com.Transaction = trans;//指定事物

                foreach (string field in oHashtable.Keys)
                {
                    //com.Parameters.Add("@" + field, oHashtable[field]);
                    com.Parameters.AddWithValue("@" + field, oHashtable[field]);
                }

                com.ExecuteNonQuery();//执行该行              

                trans.Commit();//如果全部执行完毕.提交

            }
            catch
            {
                trans.Rollback();//如果有异常.回滚.
                rtn = -1;
            }
            finally
            {
                con.Close();//关闭连接  

            }
            return rtn;
        }

    

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                if (trans != null)
                    cmd.Transaction = trans;
                cmd.CommandType = CommandType.Text;//cmdType;
                if (cmdParms != null)
                {
                    foreach (SqlParameter parameter in cmdParms)
                    {
                        if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                            (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(parameter);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            finally
            {

            }
        }


       
        /// <summary>
        /// 事务处理添加事件
        /// </summary>
        /// <param name="oHashtable">Hastable</param>
        /// <param name="strTable">数据库表名称</param>
        /// <returns>返回执行结果，0：成功;-1：失败</returns>
        public static int InsertGetID(Hashtable oHashtable, string strTable)
        {
            int rtn = 0;
            string sql = "", sqla = "", sqlb = "@";
            string[] arrFields = new string[oHashtable.Count];
            SqlConnection con = Conn;
            con.Open();

            SqlTransaction trans = con.BeginTransaction();//事物对象
            try
            {
                oHashtable.Keys.CopyTo(arrFields, 0);
                sqla = String.Join(",", arrFields);
                sqlb += String.Join(",@", arrFields);
                sql = "insert into[" + strTable + "](" + sqla + ") values(" + sqlb + ");select @@identity";
                SqlCommand com = new SqlCommand(sql, con);//数据操作对象
                com.Connection = con;//指定连接
                com.Transaction = trans;//指定事物

                foreach (string field in oHashtable.Keys)
                {
                    //com.Parameters.Add("@" + field, oHashtable[field]);
                    com.Parameters.AddWithValue("@" + field, oHashtable[field]);
                }
                com.ExecuteNonQuery();//执行该行              

                trans.Commit();//如果全部执行完毕.提交
            }
            catch
            {
                trans.Rollback();//如果有异常.回滚.
                rtn = -1;
            }
            finally
            {
                con.Close();//关闭连接
            }
            return rtn;
        }


        /// <summary>
        /// 删除一个Excel记录文件中所有记录
        /// </summary>
        /// <param name="strsql"></param>
        public static int ExecuteNonequerySQL(string strsql)
        {
            int rtn = 0;
            SqlConnection con = Conn;
            con.Open();
            SqlCommand com = new SqlCommand();//数据操作对象
            SqlTransaction trans = con.BeginTransaction();//事物对象
            try
            {
                com.Connection = con;//指定连接
                com.Transaction = trans;//指定事物               
                com.CommandText = strsql;
                com.ExecuteNonQuery();//执行该行               
                trans.Commit();//如果全部执行完毕.提交                
            }
            catch
            {
                trans.Rollback();//如果有异常.回滚.
                rtn = -1;
            }
            finally
            {
                con.Close();//关闭连接
            }
            return rtn;
        }

        /// <summary>
        /// 根据主键标示ID一次性删除多条数据
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="KeyField"></param>
        /// <param name="strParamID">ID集合</param>
        /// <returns></returns>
        public static int Delete(string TableName, string KeyField, string strParamID)
        {

            return Delete(TableName, " [" + KeyField + "] In (" + strParamID + ") ");
        }

        /// <summary>
        /// 根据主键标示ID一次性删除多条数据
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="KeyField"></param>
        /// <param name="strParamID">ID集合</param>
        /// <returns></returns>
        public static int Delete(string TableName, string KeyField, string strParamID, string sWhere)
        {
            return Delete(TableName, " [" + KeyField + "] In (" + strParamID + ")" + sWhere + " ");
        }

        /// <summary>
        /// 根据主键标示ID删除数据
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="KeyField"></param>
        /// <param name="keyid">ID</param>
        /// <returns></returns>
        public static int Delete(string TableName, string KeyField, int keyid)
        {
            return Delete(TableName, " [" + KeyField + "] =" + keyid);
        }

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="strWhere">格式 AND 1=1</param>
        /// <returns></returns>
        public static int Delete(string TableName, string strWhere)
        {
            string sql = "DELETE FROM " + TableName + " where  1=1 " + strWhere;
            return ExecuteNonQuery(sql);
        }


        #endregion

        #region 分页获取 适用于sql2000
        /// <summary>
        /// 分页获取数据列表 适用于SQL2000
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="key">主键</param>
        /// <param name="where">查询条件</param>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="pageindex">页索引</param>
        /// <param name="orderfield">排序字段</param>
        /// <param name="ordertype">排序方式 1=ASC 0=DESC</param>
        /// <param name="fieldlist">查找的字段</param>
        /// <param name="recordcount">总记录数</param>
        /// <returns></returns>
        public static DataTable GetDataByProcPaging(string tablename, string key, string where, string matchwhere, int pagesize, int pageindex, string orderfield, string fieldlist)
        {
            string cmd = "SP_Paging";//存储过程名
            SqlParameter[] para = new SqlParameter[8];
            para[0] = new SqlParameter("@Table_Name", tablename);
            para[1] = new SqlParameter("@Sign_Record", key);
            para[2] = new SqlParameter("@Filter_Condition", where);
            para[3] = new SqlParameter("@match_Condition", matchwhere);
            para[4] = new SqlParameter("@Page_Size", pagesize);
            para[5] = new SqlParameter("@Page_Index", pageindex);
            para[6] = new SqlParameter("@TaxisField", orderfield);
            para[7] = new SqlParameter("@Find_RecordList", fieldlist);
            DataTable dt = GetDataTable(CommandType.StoredProcedure, cmd, para);
            return dt;
        }
        //通过存储过程来获取数据

        public static DataTable GetDataTable(CommandType cmdType, string cmdText, params SqlParameter[] para)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = Conn;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    PrepareCommand(conn, cmd, cmdType, cmdText, para);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return ds.Tables[0];

        }
        public static void PrepareCommand(SqlConnection con, SqlCommand cmd, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (con.State != ConnectionState.Open)
                con.Open();

            cmd.Connection = con;
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            if (cmdParms != null)
                foreach (SqlParameter para in cmdParms)
                    cmd.Parameters.Add(para);
        }

        /// <summary>
        /// 根据视图名称或者是表名称获取Table
        /// </summary>
        /// <param name="viewNameOrTableName"></param>
        /// <returns></returns>
        public static DataTable GetDataTableByName(string viewNameOrTableName)
        {
            string sql = " SELECT * FROM  "+viewNameOrTableName;
            return DBClass.GetDataTable(sql);
        }

        /// <summary>
        /// 根据视图名称或者是表名称获取Table
        /// </summary>
        /// <param name="viewNameOrTableName"></param>
        /// <returns></returns>
        public static DataTable GetDataTableByName(string viewNameOrTableName,string where)
        {
            string sql = " SELECT * FROM  " + viewNameOrTableName+"  WHERE 1=1 "+where;
            return DBClass.GetDataTable(sql);
        }

        #endregion

    }
}
