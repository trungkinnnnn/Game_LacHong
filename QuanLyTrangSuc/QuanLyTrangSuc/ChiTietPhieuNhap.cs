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
    public partial class ChiTietPhieuNhap : Form
    {
        public ChiTietPhieuNhap()
        {
            InitializeComponent();
        }

        KetNoi kn = new KetNoi();

        void update_soluong()
        {
            string updateQuery = @"
                                        UPDATE SanPham 
                                                SET Soluong = 
                                                    COALESCE((
                                                        SELECT COALESCE(SUM(soluong), 0)
                                                        FROM ChiTietPhieuNhap 
                                                        WHERE ChiTietPhieuNhap.ID_SanPham = SanPham.ID_SanPham
                                                    ), 0) - 
                                                    COALESCE((
                                                        SELECT COALESCE(SUM(soluong), 0)
                                                        FROM ChiTietHoaDon 
                                                        WHERE ChiTietHoaDon.ID_SanPham = SanPham.ID_SanPham
                                                    ), 0)";
            bool kt1 = kn.thucthi(updateQuery);
            if (kt1 == false)
            {
                MessageBox.Show("lỗi cập nhật hệ thống");
            }
        }
        void getdata()
        {
            try
            {
                string query = "select * from chitietphieunhap";
                DataSet ds = kn.selectData(query);
                dgv_chitietphieu.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        void get_phieunhap()
        {
            try
            {
                string query = "select * from phieunhap";
                DataSet ds = kn.selectData(query);
                cbx_phieunhap.DataSource = ds.Tables[0];
                cbx_phieunhap.DisplayMember = "ID_phieunhap";
                cbx_phieunhap.ValueMember = "ID_phieunhap";
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        void get_sanpham()
        {
            try
            {
                string query = "select * from sanpham";
                DataSet ds = kn.selectData(query);
                cbx_sanpham.DataSource = ds.Tables[0];
                cbx_sanpham.DisplayMember = "ID_sanpham";
                cbx_sanpham.ValueMember = "ID_sanpham";
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        void getclear()
        {
            cbx_sanpham.SelectedValue = 0;
            cbx_phieunhap.SelectedValue = 0;
            txt_dongia.Text = "";
            txt_soluong.Text = "";
            txt_timkiem.Text = "";
            btn_them.Enabled = true;
            btn_sua.Enabled = false;
            btn_xoa.Enabled = false;
        }
        string Id_chitiet;

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home frm = new Home();
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

        private void kryptonButton9_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_dongia.Text == "" || txt_soluong.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ");
                }
                else
                {
                    string query = string.Format("insert into ChiTietPhieuNhap values('{0}','{1}','{2}','{3}')",
                   cbx_phieunhap.Text,
                   cbx_sanpham.Text,
                   txt_soluong.Text,
                   txt_dongia.Text
                         );
                    bool kt = kn.thucthi(query);
                    if (kt)
                    {
                        MessageBox.Show("Thêm thành công");
                        getdata();
                        getclear();
                        update_soluong();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void dgv_chitietphieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            btn_xoa.Enabled = true;
            btn_sua.Enabled = true;
            btn_them.Enabled = false;
            if (r >= 0)
            {
                cbx_phieunhap.Text = dgv_chitietphieu.Rows[r].Cells["ID_phieunhap"].Value.ToString();
                cbx_sanpham.Text = dgv_chitietphieu.Rows[r].Cells["ID_sanpham"].Value.ToString();
                txt_dongia.Text = dgv_chitietphieu.Rows[r].Cells["dongia"].Value.ToString();
                txt_soluong.Text = dgv_chitietphieu.Rows[r].Cells["soluong"].Value.ToString();
                Id_chitiet = dgv_chitietphieu.Rows[r].Cells["ID_ChiTietPhieuNhap"].Value.ToString();
            }
        }

        private void btn_lammoi_Click(object sender, EventArgs e)
        {
            getclear();
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("delete from chitietphieunhap where ID_ChiTietPhieuNhap = '{0}'", Id_chitiet);
                bool kt = kn.thucthi(query);
                if (kt)
                {
                    MessageBox.Show("Xóa thành công");
                    getdata();
                    getclear();
                    update_soluong();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_dongia.Text == "" || txt_soluong.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ");
                }
                else
                {
                    string query = string.Format("update chitietphieunhap set ID_phieunhap = '{1}',ID_sanpham = '{2}',soluong = '{3}',dongia = '{4}' where ID_ChiTietPhieuNhap = '{0}'"

                   , Id_chitiet, cbx_phieunhap.Text, cbx_sanpham.Text, txt_soluong.Text, txt_dongia.Text

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void txt_timkiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("select * from chitietphieunhap where ID_ChiTietPhieuNhap like '%{0}%' " +
                    "or ID_phieunhap like '%{0}%' or ID_Sanpham like '%{0}%' " +
                    "or soluong like '%{0}%' or dongia like '%{0}%'", txt_timkiem.Text);
                DataSet ds = kn.selectData(query);
                dgv_chitietphieu.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            this.Hide();
            PhieuNhap frm = new PhieuNhap();
            frm.ShowDialog();
            this.Close();
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ChiTietPhieuNhap frm = new ChiTietPhieuNhap();
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

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            HoaDon frm = new HoaDon();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton6_Click(object sender, EventArgs e)
        {
            this.Hide();
            NhaCungCap frm = new NhaCungCap();
            frm.ShowDialog();
            this.Close();
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

        private void ChiTietPhieuNhap_Load(object sender, EventArgs e)
        {
            getdata();
            get_sanpham();
            get_phieunhap();
            getclear();
        }
    }
}
