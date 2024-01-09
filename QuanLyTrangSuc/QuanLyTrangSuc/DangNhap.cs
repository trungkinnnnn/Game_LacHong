using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyTrangSuc
{
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }

        KetNoi kn = new KetNoi();
        public static string ID_taikhoan;
        private void kryptonButton9_Click(object sender, EventArgs e)
        {
            try
            {
                string check_quanly = string.Format("select * from Taikhoan where ID_taikhoan = '{0}' and matkhau = '{1}' and Chucvu = 'Quanly'", 
                                        txt_taikhoan.Text, txt_matkhau.Text);
                string check_nhanvien = string.Format("select * from Taikhoan where ID_taikhoan = '{0}' and matkhau = '{1}' and Chucvu = 'Nhanvien'",
                                        txt_taikhoan.Text, txt_matkhau.Text);
                DataSet ds_quanly = kn.selectData(check_quanly);
                DataSet ds_nhanvien = kn.selectData(check_nhanvien);
                if (ds_quanly.Tables[0].Rows.Count == 1)
                {
                    MessageBox.Show("Đăng nhập thành công");
                    string ID_taikhoan = txt_taikhoan.Text;
                    this.Hide();
                    Home frm = new Home(ID_taikhoan);
                    frm.ShowDialog();
                    this.Close();
                }
                else if(ds_nhanvien.Tables[0].Rows.Count == 1)
                {
                    MessageBox.Show("Đăng nhập thành công");
                    this.Hide();
                    HomeNhanVien frm = new HomeNhanVien();
                    frm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sai thông tin tài khoản hoặc mật khẩu");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        public static class UserInfo
        {
            public static string ID_taikhoan { get; set; }
        }
    }
}
