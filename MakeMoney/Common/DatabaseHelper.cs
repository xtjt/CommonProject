using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MakeMoney.Common
{
    public class DatabaseHelper
    {
        private readonly string _connString;

        public DatabaseHelper()
        {
            _connString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            if (string.IsNullOrWhiteSpace(_connString))
            {
                Console.WriteLine("异常：Database：数据库连接配置有误");
            }
        }

        public int AddOrUpdate(string sql)
        {
            SqlConnection conn = new SqlConnection(_connString); //Initial Catalog后面跟你数据库的名字  
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            int result = cmd.ExecuteNonQuery(); //result接收受影响行数，也就是说result大于0的话表示添加成功  
            conn.Close();
            cmd.Dispose();
            return result;
        }

        public DataTable Query(string sql)
        {
            //建立连接对象  
            SqlConnection conn = new SqlConnection(_connString);
            //打开连接  
            conn.Open();
            //为上面的连接指定Command对象  
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt); //执行查询并加载数据到DataTable中
            //关闭连接  
            conn.Close();
            cmd.Dispose();
            return dt;
        }
    }
}