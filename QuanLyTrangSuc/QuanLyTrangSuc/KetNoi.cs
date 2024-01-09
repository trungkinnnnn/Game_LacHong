using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyTrangSuc
{
    class KetNoi
    {
        string conrt = @"Data Source=KINN;Initial Catalog=Quan_ly_Cua_hang_trang_suc;Integrated Security=True";
        SqlConnection conn;
        public KetNoi()
        {
            conn = new SqlConnection(conrt);
        }

        public DataSet selectData(string query)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine("loi " + ex.Message);
            }
            return ds;
        }

        public bool thucthi(string query)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                int resul = cmd.ExecuteNonQuery();
                conn.Close();
                return resul > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("loi " + ex.Message);
            }
            return false;
        }

        public object laydulieu(string query)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                object resul = cmd.ExecuteScalar();
                conn.Close();
                return resul;
            }
            catch
            {
                return null;
            }
        }


    }
}
