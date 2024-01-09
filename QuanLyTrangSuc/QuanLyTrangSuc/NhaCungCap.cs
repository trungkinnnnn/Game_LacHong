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
    public partial class NhaCungCap : Form
    {
        public NhaCungCap()
        {
            InitializeComponent();
        }

        KetNoi kn = new KetNoi();

        public void getdata()
        {
            string query = "select * from nhacungcap";
            DataSet ds = kn.selectData(query);
            dgv_nhacungcap.DataSource = ds.Tables[0];
        }

       

        public void get_Clear()
        {
            txt_email.Text = "";
            txt_id.Text = "";
            txt_sdt.Text = "";
            txt_ten.Text = "";
            txt_timkiem.Text = "";
            cbx_trangthai.SelectedValue = 0;
            btn_sua.Enabled = false;
            btn_them.Enabled = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home frm = new Home();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            SanPham frm = new SanPham();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton5_Click(object sender, EventArgs e)
        {
            this.Hide();
            TaiKhoan frm = new TaiKhoan();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            HoaDon frm = new HoaDon();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton6_Click(object sender, EventArgs e)
        {
        }

        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            this.Hide();
            ThongKe frm = new ThongKe();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton8_Click(object sender, EventArgs e)
        {
            this.Hide();
            PhieuNhap frm = new PhieuNhap();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton7_Click(object sender, EventArgs e)
        {
            this.Hide();
            DangNhap frm = new DangNhap();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton9_Click(object sender, EventArgs e)
        {
            try
            {
                string query_check = string.Format("select * from Nhacungcap where ID_nhacungcap = '{0}'", txt_id.Text);
                DataSet ds = kn.selectData(query_check);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    MessageBox.Show("Mã nhà cung cấp đã tồn tại");
                }
                else
                {
                    string query = string.Format("insert into nhacungcap values('{0}',N'{1}','{2}','{3}','{4}')",
                        txt_id.Text,
                        txt_ten.Text,
                        txt_sdt.Text,
                        txt_email.Text,
                        cbx_trangthai.Text
                        );

                    bool kt = kn.thucthi(query);
                    if (kt)
                    {
                        MessageBox.Show("Thêm thành công");
                        getdata();
                        get_Clear();
                    }
                    else
                    {
                        MessageBox.Show("Thêm thất bại");
                        getdata();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void NhaCungCap_Load(object sender, EventArgs e)
        {
            getdata();
            get_Clear();
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("Update nhacungcap set Tennhacungcap = N'{1}' , " +
                                                                    "SDT = '{2}' , " +
                                                                    "email = '{3}' , " +
                                                                    "trangthai = '{4}' where ID_nhacungcap = '{0}'",
                                                                    txt_id.Text,
                                                                    txt_ten.Text,
                                                                    txt_sdt.Text,
                                                                    txt_email.Text,
                                                                    cbx_trangthai.SelectedItem.ToString()
                                                                    ); ;

                bool kt = kn.thucthi(query);
                if (kt)
                {
                    MessageBox.Show("Sửa thành công");
                    getdata();
                    get_Clear();
                }
                else
                {
                    MessageBox.Show("Sửa thất bại");
                    getdata();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("loi " + ex.Message);
            }
        }

        private void kryptonButton12_Click(object sender, EventArgs e)
        {
            get_Clear();
        }

   
        private void txt_timkiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string timkiem = string.Format("select * from nhacungcap where ID_nhacungcap like '%{0}%' or " +
                                                                              "TenNhaCungCap like N'%{0}%' or " +
                                                                              "SDT like '%{0}%' or " +
                                                                              "Email like '%{0}%' or " +
                                                                              "trangthai like '%{0}%' ", txt_timkiem.Text);
                DataSet ds = kn.selectData(timkiem);
                dgv_nhacungcap.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void cbx_trangthai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgv_nhacungcap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_sua.Enabled = true;
            btn_them.Enabled = false;
            int r = e.RowIndex;
            if (r >= 0)
            {
                txt_id.Text = dgv_nhacungcap.Rows[r].Cells["ID_nhacungcap"].Value.ToString();
                txt_ten.Text = dgv_nhacungcap.Rows[r].Cells["Tennhacungcap"].Value.ToString();
                txt_sdt.Text = dgv_nhacungcap.Rows[r].Cells["SDT"].Value.ToString();
                txt_email.Text = dgv_nhacungcap.Rows[r].Cells["email"].Value.ToString();
                string trangthai = dgv_nhacungcap.Rows[r].Cells["trangthai"].Value.ToString();

                cbx_trangthai.Text = trangthai;

            }
        }
    }
}
