using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyTrangSuc
{
    public partial class PhieuNhap : Form
    {
        private string ID_taikhoan;
        public PhieuNhap()
        {
            InitializeComponent();
        }
        public PhieuNhap(string ID_taikhoan)
        {
            InitializeComponent();
            this.ID_taikhoan = ID_taikhoan;
        }

        KetNoi kn = new KetNoi();
        void getdata()
        {
            try
            {
                string query = string.Format("select * from phieunhap");
                DataSet ds = kn.selectData(query);
                dgv_phieunhap.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine("loi " + ex.Message);
            }
        }

        void getnhacungcap()
        {
            try
            {
                string query = string.Format("select * from nhacungcap");
                DataSet ds = kn.selectData(query);
                cbx_nhacungcap.DataSource = ds.Tables[0];
                cbx_nhacungcap.DisplayMember = "ID_nhacungcap";
                cbx_nhacungcap.ValueMember = "ID_nhacungcap";
            }
            catch (Exception ex)
            {
                Console.WriteLine("loi " + ex.Message);
            }
        }

        void getdata_chitietphieu()
        {
            try
            {
                string query = string.Format("select ID_ChiTietPhieuNhap,ID_sanpham,soluong,dongia from chitietphieunhap where ID_phieunhap = '{0}'", ID_phieunhap);
                DataSet ds = kn.selectData(query);
                dgv_chitietphieu.DataSource = ds.Tables[0];
                dgv_chitietphieu.Columns["ID_ChiTietPhieuNhap"].Visible = false;
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
                string query = "select ID_sanpham,TenSanpham,soluong,hinhanh from sanpham";
                DataSet ds = kn.selectData(query);
                dgv_sanpham.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        void getclear_phieunhap()
        {
            txt_phieunhap.Text = "";
            txt_taikhoan.Text = "";
            txt_timkiem.Text = "";
            cbx_nhacungcap.SelectedValue = 0;
            date_ngaynhap.Value = DateTime.Today;
            btn_them.Enabled = true;
            btn_xoa.Enabled = false;
            btn_xoachitietphieu.Enabled = false;
            btn_themchitietphieu.Enabled = false;

        }
        void getclear_chitietphieunhap()
        {
            txt_dongia.Text = "";
            txt_sanpham.Text = "";
            txt_soluong.Text = "";
            txt_timkiem.Text = "";
            btn_themchitietphieu.Enabled = true;
            btn_xoachitietphieu.Enabled = true;
        }

        void update_soluong()
        {
            try
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
                else
                {
                    getdata_sanpham();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("loi" + ex.Message);
            }
        }

        bool check_sp()
        {
            try
            {
                string query = string.Format("select * from ChiTietPhieuNhap where ID_sanpham = '{0}' and ID_phieunhap = '{1}'",
                    txt_sanpham.Text,
                    ID_phieunhap
                    );
                DataSet ds = kn.selectData(query);
                if(ds.Tables[0].Rows.Count == 1)
                {
                    return true;
                }
                else { return false; }
            }catch(Exception ex)
            {
                MessageBox.Show("loi" + ex.Message);
            }
            return false;
        }

        object ID_phieunhap;
        private void PhieuNhap_Load(object sender, EventArgs e)
        {
            getdata();
            getnhacungcap();
            getdata_chitietphieu();
            getdata_sanpham();
            getclear_phieunhap();
            getclear_chitietphieunhap();
            btn_them.Enabled = true;
            txt_taikhoan.Enabled = false;
            btn_themchitietphieu.Enabled = false;
            btn_xoachitietphieu.Enabled = false;
            kryptonButton13.Enabled = false;
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

        private void kryptonButton7_Click(object sender, EventArgs e)
        {
            this.Hide();
            DangNhap frm = new DangNhap();
            frm.ShowDialog();
            this.Close();
        }
        
        private void getdata_IDphieunhap()
        {
            try
            {
                string query = "select MAX(ID_PhieuNhap) from PhieuNhap";
                object result = kn.laydulieu(query);
                ID_phieunhap = (result == null) ? 0 : Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }
        private void kryptonButton9_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (cbx_nhacungcap.Text == "")
                {
                    MessageBox.Show("Hãy chọn nhà cung cấp");
                }
                else
                {
                   
                    string query = string.Format("insert into phieunhap values(N'{0}',N'{1}','{2}')",
                           ID_taikhoan,
                           cbx_nhacungcap.Text,
                           date_ngaynhap.Value.ToString()
                        );
                    bool kt = kn.thucthi(query);
                    if (kt)
                    {
                        MessageBox.Show("thêm thành công");
                        getdata();
                        getdata_sanpham();
                        getdata_IDphieunhap();
                        getdata_chitietphieu();
                        update_soluong();
                        btn_them.Enabled = false;
                        btn_xoa.Enabled = true;
                        btn_themchitietphieu.Enabled = true;
                        btn_xoachitietphieu.Enabled = true;
                        kryptonButton13.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("thêm thất bại");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void dgv_phieunhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            int c = e.ColumnIndex;
            btn_them.Enabled = false;
            btn_xoa.Enabled = true;
            if (c >= 0 && r >= 0)
            {
                txt_phieunhap.Text = dgv_phieunhap.Rows[r].Cells["ID_phieunhap"].Value.ToString();
                txt_taikhoan.Text = dgv_phieunhap.Rows[r].Cells["ID_taikhoan"].Value.ToString();
                cbx_nhacungcap.Text = dgv_phieunhap.Rows[r].Cells["ID_nhacungcap"].Value.ToString();
                date_ngaynhap.Text = dgv_phieunhap.Rows[r].Cells["ngayNhap"].Value.ToString();
                ID_phieunhap = txt_phieunhap.Text;
                //label_idphieunhap.Text = txt_phieunhap.Text;
                getdata_chitietphieu();
                btn_themchitietphieu.Enabled = true;
                btn_xoachitietphieu.Enabled = true;
            }
        }

        private void btn_lammoi_Click(object sender, EventArgs e)
        {
            getclear_phieunhap();
            dgv_chitietphieu.DataSource = null;
        }

        private void txt_timkiem_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            try
            {

                string query = string.Format("delete from phieunhap where ID_phieunhap = '{0}'",
                    txt_phieunhap.Text
                    );
                bool kt = kn.thucthi(query);
                if (kt)
                {
                    MessageBox.Show("Xóa thành công");
                    getdata();
                    getdata_sanpham();
                    getdata_chitietphieu();
                    getclear_phieunhap();
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

        private void btn_themchitietphieu_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_dongia.Text == "" || txt_soluong.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ");
                }
                else
                {
                    string check_SanPham = string.Format("select * from sanpham where ID_sanpham = '{0}'", txt_sanpham.Text);
                    DataSet ds = kn.selectData(check_SanPham);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Mã sản phẩm không tồn tại");
                    }
                    else
                    {
                        string query = "";
                        if (check_sp())
                        {
                            query = string.Format("Update ChiTietPhieuNhap set SoLuong = SoLuong + '{0}' where ID_SanPham = '{1}' and ID_PhieuNhap = '{2}'",
                                    txt_soluong.Text,
                                    txt_sanpham.Text,
                                    ID_phieunhap
                                );
                        }
                        else
                        {
                            query = string.Format("insert into ChiTietPhieuNhap values('{0}','{1}','{2}','{3}')",
                                    ID_phieunhap,
                                    txt_sanpham.Text,
                                    txt_soluong.Text,
                                    txt_dongia.Text
                                    );
                        }

                        bool kt = kn.thucthi(query);
                        if (kt)
                        {
                            getdata();
                            getdata_sanpham();
                            getdata_chitietphieu();
                            getclear_phieunhap();
                            getclear_chitietphieunhap();
                            update_soluong();
                        }
                        else
                        {
                            MessageBox.Show("Lỗi");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void btn_xoachitietphieu_Click(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("delete from chitietphieunhap where ID_sanpham =  '{0}' and ID_ChiTietPhieuNhap = '{1}'", txt_sanpham.Text, ID_chitietphieu);


                bool kt = kn.thucthi(query);
                if (kt)
                {
                    getdata();
                    getdata_sanpham();
                    getdata_chitietphieu();
                    getclear_phieunhap();
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
        string ID_chitietphieu;

        private void dgv_chitietphieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;

            if (r >= 0)
            {
                txt_soluong.Text = dgv_chitietphieu.Rows[r].Cells["soluong"].Value.ToString();
                txt_dongia.Text = dgv_chitietphieu.Rows[r].Cells["dongia"].Value.ToString();
                txt_sanpham.Text = dgv_chitietphieu.Rows[r].Cells["ID_sanpham"].Value.ToString();
                ID_chitietphieu = dgv_chitietphieu.Rows[r].Cells[0].Value.ToString();
            }
        }

        private void dgv_sanpham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_dongia.Text = "";
            txt_soluong.Text = "";

            int r = e.RowIndex;
            if (r >= 0)
            {
                txt_sanpham.Text = dgv_sanpham.Rows[r].Cells["ID_sanpham"].Value.ToString();
                string imgpath = dgv_sanpham.Rows[r].Cells["hinhanh"].Value.ToString();
                if (!string.IsNullOrEmpty(imgpath) && File.Exists(imgpath)) // Kiểm tra đường dẫn ảnh có hợp lệ không
                {
                    pic_hinhanh.Image = Image.FromFile(imgpath); // Hiển thị ảnh lên PictureBox
                }
                else
                {
                    pic_hinhanh.Image = null; // Nếu không có đường dẫn ảnh hợp lệ, xóa ảnh khỏi PictureBox
                }
            }
        }

        private void btn_themsanpham_Click(object sender, EventArgs e)
        {
            this.Hide(); // Ẩn form cha
            SanPham frm = new SanPham();
            frm.ShowDialog();
            this.Close();
        }

        private void txt_soluong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Không cho phép ký tự nhập vào TextBox
            }
        }

        private void txt_dongia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Không cho phép ký tự nhập vào TextBox
            }
        }

        private void kryptonTextBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("select * from phieunhap where ID_phieunhap like '%{0}%' or " +
                    "ID_taikhoan like N'%{0}%' or ID_nhacungcap like N'%{0}%' or NgayNhap like '%{0}%'",
                        txt_timkiem.Text
                    );
                DataSet ds = kn.selectData(query);
                dgv_phieunhap.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void toolStripLabel1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            PhieuNhap frm = new PhieuNhap();
            frm.ShowDialog();
            this.Close();
        }

        private void toolStripLabel2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            ChiTietPhieuNhap frm = new ChiTietPhieuNhap();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton13_Click(object sender, EventArgs e)
        {
            getclear_chitietphieunhap();
        }

        private void kryptonPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
