using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace QuanLyTrangSuc
{
    public partial class HoaDon : Form
    {
        public HoaDon()
        {
            InitializeComponent();
        }

        KetNoi kn = new KetNoi();


        void getdata_hoadon()
        {
            try
            {
                string query = "select * from hoadon";
                DataSet ds = kn.selectData(query);
                dgv_hoadon.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }
        void getdata_chititethoadon(object hoadon)
        {
            try
            {
                string query = string.Format("select ID_chitiethoadon,ID_sanpham,soluong,giaban,giamgia from chitiethoadon where ID_hoadon = '{0}'", hoadon);
                DataSet ds = kn.selectData(query);
                dgv_chitiethoadon.DataSource = ds.Tables[0];
                dgv_chitiethoadon.Columns["ID_chitiethoadon"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        void update_hoadon()
        {
            try
            {
                string tongtien_hoadon = "UPDATE HoaDon SET TongTien = " +
                                           "(SELECT SUM(SoLuong* (GiaBan -GiaBan * (GiamGia / 100)))" +
                                           "FROM ChiTietHoaDon WHERE ChiTietHoaDon.ID_HoaDon = HoaDon.ID_HoaDon)";
                bool kt = kn.thucthi(tongtien_hoadon);
                if (kt == false)
                {
                    MessageBox.Show("Cập nhật tổng tiền lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi" + ex.Message);
            }
        }

        void update_soluong()
        {
            try
            {
                string updateQuery = @"UPDATE SanPham SET Soluong = 
                                        COALESCE((
                                            SELECT COALESCE(SUM(soluong), 0)
                                            FROM ChiTietPhieuNhap 
                                            WHERE ChiTietPhieuNhap.ID_SanPham = SanPham.ID_sanpham), 0) - 
                                        COALESCE((
                                            SELECT COALESCE(SUM(soluong), 0)
                                            FROM ChiTietHoaDon 
                                            WHERE ChiTietHoaDon.ID_SanPham = SanPham.ID_sanpham), 0)";
                bool kt1 = kn.thucthi(updateQuery);
                if (kt1 == false)
                {
                    MessageBox.Show("Cập nhật số lượng lỗi");
                }
                else
                {
                    getdata_sanpham();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }
        void getdata_IDHoaDon()
        {
            try
            {
                string query = "select MAX(ID_HoaDon) from HoaDon";
                object result = kn.laydulieu(query);
                id_hoadon = (result == null) ? 0 : Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }
        void getdata_sanpham()
        {
            try
            {
                string query = "select * from sanpham";
                DataSet ds = kn.selectData(query);
                dgv_sanpham.DataSource = ds.Tables[0];
                dgv_sanpham.Columns["Mota"].Visible = false;
                dgv_sanpham.Columns["hinhanh"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        void getclear()
        {
            cbx_hinhthucthanhtoan.Text = "";
            date_ngayxuat.Value = DateTime.Today;
            btn_them.Enabled = true;
            btn_xoa.Enabled = false;
        }
        void getclear_chitiethoadon()
        {
            txt_giaban.Text = "";
            txt_giamgia.Text = "";
            txt_soluong.Text = "";
            txt_sanpham.Text = "";
        }

        bool check_sp()
        {
            try
            {
                string query = string.Format("select * from chitiethoadon where ID_sanpham = '{0}' and ID_hoadon = '{1}'",
                    txt_sanpham.Text,
                    id_hoadon
                    );
                DataSet ds = kn.selectData(query);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi" + ex.Message);
            }
            return false;
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

        private void kryptonButton9_Click(object sender, EventArgs e)
        {
            try
            {
                cbx_hinhthucthanhtoan.Text = "Tiền mặt";
                    string query = string.Format("insert into hoadon values (N'{0}','{1}','')",
                       cbx_hinhthucthanhtoan.Text,
                       date_ngayxuat.Value.ToString()
                   );
                    bool kt = kn.thucthi(query);
                    if (kt)
                    {
                        getdata_hoadon();
                        getclear();
                        getdata_IDHoaDon();
                        getdata_chititethoadon(id_hoadon);
                        btn_xoa.Enabled = true;
                        btn_them.Enabled = false;
                        btn_themchititethoadon.Enabled = true;
                        btn_xoachitiethoadon.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("loi");
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void HoaDon_Load(object sender, EventArgs e)
        {
            txt_giaban.Enabled = false;
            txt_giamgia.Enabled = false;
            btn_themchititethoadon.Enabled = false;
            btn_xoachitiethoadon.Enabled = false;
            getdata_hoadon();
            getdata_sanpham();
            getclear();
        }

        private void kryptonTextBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("select * from hoadon where ID_hoadon like '%{0}%' or " +
                    "hinhthucthanhtoan like N'%{0}%' or " +
                    "ngayxuat like N'%{0}%' or TongTien like N'%{0}%'", txt_timkiem_hoadon.Text);
                DataSet ds = kn.selectData(query);
                dgv_hoadon.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void kryptonTextBox8_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("select * from sanpham where ID_sanpham like '%{0}%' or tensanpham like '%{0}%' or " +
                                                                         "hinhanh like '%{0}%'",
                                                                         txt_timkiemsp.Text
                                                                         );
                DataSet ds = kn.selectData(query);
                dgv_sanpham.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        int soluong_sanpham;

        private void dgv_sanpham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            if (r >= 0)
            {
                txt_sanpham.Text = dgv_sanpham.Rows[r].Cells["ID_sanpham"].Value.ToString();
                string imgpath = dgv_sanpham.Rows[r].Cells["hinhanh"].Value.ToString();
                txt_giaban.Text = dgv_sanpham.Rows[r].Cells["giaban"].Value.ToString();
                txt_giamgia.Text = dgv_sanpham.Rows[r].Cells["giamgia"].Value.ToString();
                if (!int.TryParse(dgv_sanpham.Rows[r].Cells["soluong"].Value.ToString(), out soluong_sanpham))
                {
                    MessageBox.Show("Giá trị không hợp lệ trong cột số lượng.");
                    // Xử lý khi giá trị không thể chuyển đổi thành số nguyên
                }
                if (!string.IsNullOrEmpty(imgpath) && File.Exists(imgpath))
                {
                    pic_hinhanh.Image = Image.FromFile(imgpath);
                }
                else
                {
                    pic_hinhanh.Image = null; // Nếu không có đường dẫn ảnh hợp lệ, xóa ảnh khỏi PictureBox
                }
            }
        }

        private void txt_sanpham_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("select * from Sanpham where ID_sanpham = '{0}'", txt_sanpham.Text);
                DataSet ds = kn.selectData(query);
                DataTable dataTable = ds.Tables[0];
                if (dataTable.Rows.Count > 0)
                {
                    txt_giaban.Text = dataTable.Rows[0]["giaban"].ToString();
                    txt_giamgia.Text = dataTable.Rows[0]["giamgia"].ToString();
                }
                else
                {
                    txt_giaban.Text = "";
                    txt_giamgia.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("delete hoadon where ID_hoadon = '{0}'",
                        id_hoadon
                    );
                bool kt = kn.thucthi(query);
                if (kt)
                {
                    getdata_chititethoadon(id_hoadon);
                    getclear_chitiethoadon();
                    update_hoadon();
                    update_soluong();
                    getdata_hoadon();
                    getdata_sanpham();
                }
                else
                {
                    MessageBox.Show("loi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }
        object id_hoadon;

        private void dgv_hoadon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            btn_xoa.Enabled = true;
            btn_them.Enabled = false;
            btn_themchititethoadon.Enabled = true;
            btn_xoachitiethoadon.Enabled = true;
            if (r >= 0)
            {
                cbx_hinhthucthanhtoan.Text = dgv_hoadon.Rows[r].Cells["hinhthucthanhtoan"].Value.ToString();
                date_ngayxuat.Text = dgv_hoadon.Rows[r].Cells["ngayxuat"].Value.ToString();
                id_hoadon = dgv_hoadon.Rows[r].Cells["ID_hoadon"].Value.ToString();
                //label_idhoadon.Text = id_hoadon;
                getdata_chititethoadon(id_hoadon);
            }
        }

        private void kryptonPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kryptonButton12_Click(object sender, EventArgs e)
        {
            getclear();
        }

        private void kryptonButton10_Click(object sender, EventArgs e)
        {
            try
            {

                int so_luongNhap;
                if (!int.TryParse(txt_soluong.Text, out so_luongNhap))
                {
                    MessageBox.Show("không hợp lệ");
                }
                string check_sanpham = string.Format("select * from sanpham where ID_sanpham = '{0}'", txt_sanpham.Text);
                DataSet ds = kn.selectData(check_sanpham);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    if (txt_soluong.Text == "")
                    {
                        MessageBox.Show("Vui lòng nhập số lượng");
                    }
                    else
                    {
                        if (so_luongNhap > soluong_sanpham)
                        {
                            MessageBox.Show("Số lượng không đủ");

                        }
                        else
                        {
                            string query = "";
                            if (check_sp())
                            {
                                query = string.Format("Update chitiethoadon set SoLuong = SoLuong + '{0}' where ID_SanPham = '{1}' and ID_hoadon = '{2}'",
                                        txt_soluong.Text,
                                        txt_sanpham.Text,
                                        id_hoadon
                                    );
                            }
                            else
                            {
                                query = string.Format("insert into chitiethoadon values('{0}','{1}','{2}','{3}','{4}')",
                                     id_hoadon,
                                     txt_sanpham.Text,
                                     txt_soluong.Text,
                                     txt_giaban.Text,
                                     txt_giamgia.Text
                                );
                            }
                             
                            bool kt = kn.thucthi(query);
                            if (kt)
                            {
                                getdata_chititethoadon(id_hoadon);
                                getclear_chitiethoadon();
                                update_hoadon();
                                update_soluong();
                                getdata_hoadon();
                                getdata_sanpham();
                            }
                            else
                            {
                                MessageBox.Show("loi");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Mã sản phẩm không tồn tại");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void txt_soluong_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void txt_soluong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Không cho phép ký tự nhập vào TextBox
            }
        }

        private void kryptonButton14_Click(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("delete chitiethoadon where ID_chitiethoadon = '{0}'",
                   id_chitiethoadon
                    );
                bool kt = kn.thucthi(query);
                if (kt)
                {
                    getdata_chititethoadon(id_hoadon);
                    getclear_chitiethoadon();
                    update_hoadon();
                    update_soluong();
                    getdata_hoadon();
                    getdata_sanpham();
                }
                else
                {
                    MessageBox.Show("loi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }
        string id_chitiethoadon;

        private void dgv_chitiethoadon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            if (r >= 0)
            {
                txt_sanpham.Text = dgv_chitiethoadon.Rows[r].Cells["ID_sanpham"].Value.ToString();
                txt_giaban.Text = dgv_chitiethoadon.Rows[r].Cells["giaban"].Value.ToString();
                txt_giamgia.Text = dgv_chitiethoadon.Rows[r].Cells["giamgia"].Value.ToString();
                txt_soluong.Text = dgv_chitiethoadon.Rows[r].Cells["soluong"].Value.ToString();
                id_chitiethoadon = dgv_chitiethoadon.Rows[r].Cells["ID_chitiethoadon"].Value.ToString();
            }
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ChiTietHoaDon frm = new ChiTietHoaDon();
            frm.ShowDialog();
            this.Close();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            this.Hide();
            HoaDon frm = new HoaDon();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton13_Click(object sender, EventArgs e)
        {
            getclear_chitiethoadon();
        }

       
    }
}
