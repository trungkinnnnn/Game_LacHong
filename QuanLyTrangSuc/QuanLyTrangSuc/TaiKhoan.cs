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
    public partial class TaiKhoan : Form
    {
        public TaiKhoan()
        {
            InitializeComponent();
        }

        KetNoi kn = new KetNoi();

        public void getdata()
        {
            string query = "select * from taikhoan";
            DataSet ds = kn.selectData(query);
            dgv_taikhoan.DataSource = ds.Tables[0];
        }

        public void getclear()
        {
            txt_timkiem.Text = "";
            txt_taikhoan.Text = "";
            txt_email.Text = "";
            txt_hoten.Text = "";
            txt_sdt.Text = "";
            cbx_chucvu.SelectedValue = 0;
            date_ngaysinh.Value = DateTime.Today;
            txt_matkhau.Text = "";
            btn_them.Enabled = true;
            btn_sua.Enabled = false;
            btn_xoa.Enabled = false;
            txt_taikhoan.Enabled = true;
        }

        private void kryptonButton1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Home frm = new Home();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            SanPham frm = new SanPham();
            frm.ShowDialog();
            this.Close();
        }


        private void kryptonButton3_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            HoaDon frm = new HoaDon();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton6_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            NhaCungCap frm = new NhaCungCap();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton4_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            ThongKe frm = new ThongKe();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton8_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            PhieuNhap frm = new PhieuNhap();
            frm.ShowDialog();
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string check_taikhoan = string.Format("select * from taikhoan where ID_taikhoan = '{0}'",
                    txt_taikhoan.Text

                    );
                DataSet ds = kn.selectData(check_taikhoan);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    MessageBox.Show("Tài khoản đã tồn tại");
                }
                else
                {
                    string query = string.Format("insert into taikhoan values('{0}','{1}',N'{2}','{3}',N'{4}',N'{5}',N'{6}')",
                        txt_taikhoan.Text,
                        txt_matkhau.Text,
                        txt_hoten.Text,
                        date_ngaysinh.Value.ToString(),
                        txt_email.Text,
                        txt_sdt.Text,
                        cbx_chucvu.Text
                        );
                    bool kt = kn.thucthi(query);
                    if (kt)
                    {
                        MessageBox.Show("Thêm thành công");
                        getdata();
                    }
                    else
                    {
                        MessageBox.Show("Thêm thất bại");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("UPDATE taikhoan " +
                                              "SET matkhau = '{1}' , " +
                                              "Hoten = N'{2}' , " +
                                               "NgaySinh = '{3}' , " +
                                               "Email = '{4}' , " +
                                               "SDT = '{5}' , " +
                                               "Chucvu = '{6}' " +
                                               "WHERE ID_taikhoan = '{0}'",
                                                txt_taikhoan.Text,
                                           txt_matkhau.Text,
                                           txt_hoten.Text,
                                           date_ngaysinh.Value.ToString(),
                                           txt_email.Text,
                                           txt_sdt.Text,
                                           cbx_chucvu.Text
                                           );
                bool kt = kn.thucthi(query);
                if (kt)
                {
                    MessageBox.Show("Sửa thành công");
                    getdata();
                    getclear();
                }
                else
                {
                    MessageBox.Show("Sửa thất bại");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void kryptonComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("delete taikhoan where ID_taikhoan = '{0}'", txt_taikhoan.Text);

                if (MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bool kt = kn.thucthi(query);
                    if (kt)
                    {
                        MessageBox.Show("Xóa thành công");
                        getdata();
                        getclear();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại");
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void kryptonDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            btn_them.Enabled = false;
            btn_xoa.Enabled = true;
            btn_sua.Enabled = true;
            txt_taikhoan.Enabled = false;
            if (r >= 0)
            {
                txt_taikhoan.Text = dgv_taikhoan.Rows[r].Cells["ID_taikhoan"].Value.ToString();
                txt_matkhau.Text = dgv_taikhoan.Rows[r].Cells["matkhau"].Value.ToString();
                txt_hoten.Text = dgv_taikhoan.Rows[r].Cells["hoten"].Value.ToString();
                date_ngaysinh.Text = dgv_taikhoan.Rows[r].Cells["ngaysinh"].Value.ToString();
                txt_email.Text = dgv_taikhoan.Rows[r].Cells["email"].Value.ToString();
                txt_sdt.Text = dgv_taikhoan.Rows[r].Cells["sdt"].Value.ToString();
                string combox = dgv_taikhoan.Rows[r].Cells["chucvu"].Value.ToString();
                cbx_chucvu.Text = combox;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            getclear();
        }

        private void kryptonTextBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string timkiem = string.Format("SELECT * FROM taikhoan WHERE ID_taikhoan LIKE '%{0}%' OR matkhau LIKE '%{0}%' OR hoten LIKE N'%{0}%' OR ngaysinh LIKE '%{0}%' OR email LIKE '%{0}%' OR SDT LIKE '%{0}%' OR chucvu LIKE N'%{0}%'",
                                                                                 txt_timkiem.Text
                                                                            );
                DataSet ds = kn.selectData(timkiem);
                dgv_taikhoan.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void TaiKhoan_Load(object sender, EventArgs e)
        {
            getdata();
            getclear();
        }

        private void dgv_taikhoan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cbx_chucvu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void kryptonButton7_Click(object sender, EventArgs e)
        {

        }
    }
}
