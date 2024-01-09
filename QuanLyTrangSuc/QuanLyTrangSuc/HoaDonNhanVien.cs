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
    public partial class HoaDonNhanVien : Form
    {

        private int ID_hoadon = 0;
        private int tongtien = 0;
        KetNoi kn = new KetNoi();
        public HoaDonNhanVien()
        {
            InitializeComponent();
        }

        void getdata_IDHoaDon()
        {
            try
            {
                string query = "select MAX(ID_HoaDon) from HoaDon";
                object result = kn.laydulieu(query);
                ID_hoadon = (result == null) ? 0 : Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        void get_tongtien()
        {
            try
            {
                string query = string.Format("select TongTien from HoaDon where ID_hoadon = '{0}'", ID_hoadon);
                object result = kn.laydulieu(query);
                tongtien = (result == null) ? 0 : Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        void getdata_chititethoadon()
        {
            try
            {
                string query = string.Format("select ID_chitiethoadon,ID_sanpham,soluong,giaban,giamgia from chitiethoadon where ID_hoadon = '{0}'", ID_hoadon);
                DataSet ds = kn.selectData(query);
                dgv_chitiethoadon.DataSource = ds.Tables[0];
                dgv_chitiethoadon.Columns["ID_ChiTiethoadon"].Visible = false;
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
                                            WHERE ChiTietPhieuNhap.ID_SanPham = SanPham.ID_SanPham), 0) - 
                                        COALESCE((
                                            SELECT COALESCE(SUM(soluong), 0)
                                            FROM ChiTietHoaDon 
                                            WHERE ChiTietHoaDon.ID_SanPham = SanPham.ID_SanPham), 0)";
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

        void getclear_chitiethoadon()
        {
            txt_giaban.Text = "";
            txt_giamgia.Text = "";
            txt_soluong.Text = "";
            txt_sanpham.Text = "";
            date_ngayxuat.Value = DateTime.Today;
        }

        bool check_sp()
        {
            try
            {
                string query = string.Format("select * from chitiethoadon where ID_sanpham = '{0}' and ID_hoadon = '{1}'",
                    txt_sanpham.Text,
                    ID_hoadon
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

      
       
        private void btn_themchititethoadon_Click(object sender, EventArgs e)
        {
            try
            {

                int so_luongNhap;
                if (!int.TryParse(txt_soluong.Text, out so_luongNhap))
                {
                    MessageBox.Show("Không hợp lệ");
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
                                        ID_hoadon
                                    );
                            }
                            else
                            {
                                query = string.Format("insert into chitiethoadon values('{0}','{1}','{2}','{3}','{4}')",
                                     ID_hoadon,
                                     txt_sanpham.Text,
                                     txt_soluong.Text,
                                     txt_giaban.Text,
                                     txt_giamgia.Text
                                );
                            }

                            bool kt = kn.thucthi(query);
                            if (kt)
                            {
                                getdata_chititethoadon();
                                getclear_chitiethoadon();
                                update_hoadon();
                                update_soluong();
                                getdata_sanpham();
                                get_tongtien();
                                label_tongtien.Text = tongtien + "";
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

        private void HoaDonNhanVien_Load(object sender, EventArgs e)
        {
            getdata_sanpham();
            getclear_chitiethoadon();
            txt_giaban.Enabled = false;
            txt_giamgia.Enabled = false;
            btn_themchititethoadon.Enabled = false;
            btn_xoachitiethoadon.Enabled = false;
            txt_sanpham.Enabled = false;
            txt_soluong.Enabled = false;
            cbx_hinhthucthanhtoan.Enabled = false;
            date_ngayxuat.Enabled = false;
            btn_huy.Visible = false;
        }

       int soluong_sanpham;
        private void dgv_sanpham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            if (r >= 0)
            {
                txt_sanpham.Text = dgv_sanpham.Rows[r].Cells["ID_sanpham"].Value.ToString();
                txt_giaban.Text = dgv_sanpham.Rows[r].Cells["giaban"].Value.ToString();
                txt_giamgia.Text = dgv_sanpham.Rows[r].Cells["giamgia"].Value.ToString();
                if (!int.TryParse(dgv_sanpham.Rows[r].Cells["soluong"].Value.ToString(), out soluong_sanpham))
                {
                    MessageBox.Show("Giá trị không hợp lệ trong cột số lượng.");
                    // Xử lý khi giá trị không thể chuyển đổi thành số nguyên
                }

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

        private void txt_timkiem_TextChanged(object sender, EventArgs e)
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

        private void txt_soluong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Không cho phép ký tự nhập vào TextBox
            }
        }

       
        private void btn_laphoadon_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "insert into hoadon values('','','')";
                bool kt = kn.thucthi(query);
                if (kt)
                {
                    getdata_IDHoaDon();
                    btn_themchititethoadon.Enabled = true;
                    btn_xoachitiethoadon.Enabled = true;
                    btn_laphoadon.Visible = false;
                    btn_huy.Visible = true;
                    txt_sanpham.Enabled = true;
                    txt_soluong.Enabled = true;
                    cbx_hinhthucthanhtoan.Enabled = true;
                    date_ngayxuat.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Lập hóa đơn thất bại");
                }
            }catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void btn_xoachitiethoadon_Click(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("delete chitiethoadon where ID_chitiethoadon = '{0}'",
                   id_chitiethoadon
                    );
                bool kt = kn.thucthi(query);
                if (kt)
                {
                    getdata_chititethoadon();
                    getclear_chitiethoadon();
                    update_hoadon();
                    update_soluong();
                    getdata_sanpham();
                    get_tongtien();
                    label_tongtien.Text = tongtien + "";
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

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbx_hinhthucthanhtoan.Text == "")
                {
                    MessageBox.Show("Hãy chọn hình thức thanh toán");
                }
                else
                {
                    DateTime date;
                    string query = string.Format("update Hoadon set HinhthucThanhtoan = N'{0}', NgayXuat = '{1}' where ID_hoadon = '{2}'",
                            cbx_hinhthucthanhtoan.Text,
                            date_ngayxuat.Value.Date,
                            ID_hoadon
                        );
                    bool kt = kn.thucthi(query);
                    if (kt)
                    {
                        date = date_ngayxuat.Value;
                        InHoaDon IN_hoadon = new InHoaDon(ID_hoadon, tongtien, date);
                        IN_hoadon.Show();
                    }
                    else
                    {
                        MessageBox.Show("Không thể lập hóa đơn");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi" + ex.Message);
            }
        }

        private void btn_huy_Click(object sender, EventArgs e)
        {
            try
            {
                btn_themchititethoadon.Enabled = false;
                btn_xoachitiethoadon.Enabled = false;
                btn_laphoadon.Visible = true;
                btn_huy.Visible = false;
                txt_sanpham.Enabled = false;
                txt_soluong.Enabled = false;
                cbx_hinhthucthanhtoan.Enabled = false;
                date_ngayxuat.Enabled = false;
                cbx_hinhthucthanhtoan.Text = "";
                string query = string.Format("delete Hoadon where ID_hoadon = '{0}'",
                    ID_hoadon
                    );
                bool kt = kn.thucthi(query);
                if (kt)
                {
                    MessageBox.Show("Đã hủy hóa đơn");
                    update_soluong();
                    getclear_chitiethoadon();
                    getdata_chititethoadon();
                }
                else
                {
                    MessageBox.Show("hủy hóa đơn thất bại");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi" + ex.Message);
            }
        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {

        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            HomeNhanVien frm = new HomeNhanVien();
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
    }
}
