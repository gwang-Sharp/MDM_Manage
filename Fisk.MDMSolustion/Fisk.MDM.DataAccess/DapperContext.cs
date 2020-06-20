/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：使用dapper操作mysql                                                    
*│　作    者：Dennyhui                                             
*│　版    本：1.0                                                 
*│　创建时间：2020年4月30日09:58:49                      
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Fisk.MDM.DataAccess                               
*│　类    名： DapperContext                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Fisk.MDM.Utility.Common;
using MySql.Data.MySqlClient;

namespace Fisk.MDM.DataAccess
{
    public class DapperContext
    {
        //连接字符串
        public static string connectionString = AppsettingsHelper.GetSection("ConnectionStrings:MysqlConnection");
        public static MySqlConnection Connection()
        {
            var mysql = new MySqlConnection(connectionString);
            mysql.Open();
            return mysql;
        }
    }
}
