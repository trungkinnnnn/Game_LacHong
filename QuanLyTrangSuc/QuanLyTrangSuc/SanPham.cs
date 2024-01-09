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
    public partial class SanPham : Form
    {
        public SanPham()
        {
            InitializeComponent();
        }
        KetNoi kn = new KetNoi();
        void getdata()
        {
            try
            {
                string query = "select * from sanpham";
                DataSet ds = kn.selectData(query);
                dgv_sanpham.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        void getclear()
        {
            txt_sanpham.Enabled = true;
            txt_sanpham.Text = "";
            txt_tensanpham.Text = "";
            txt_mota.Text = "";
            txt_soluong.Text = "";
            txt_timkiem.Text = "";
            txt_giaban.Text = "";
            txt_giamgia.Text = "";
            cbx_loaisanpham.Text = "";
            btn_them.Enabled = true;
            btn_sua.Enabled = false;
            btn_xoa.Enabled = false;
            pic_anhsanpham.Image = null;
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

        string imgPath;

        private void kryptonButton9_Click(object sender, EventArgs e)
        {
            if (imgPath == "")
            {
                imgPath = "khong co";
            }
            try
            {
                string check_idSanpham = string.Format("select * from sanpham where ID_sanpham = '{0}'", txt_sanpham.Text);
                DataSet ds = kn.selectData(check_idSanpham);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    MessageBox.Show("Mã sản phẩm đã tồn tại");
                }
                else
                {
                    string query = string.Format("insert into sanpham values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                        txt_sanpham.Text,
                        txt_tensanpham.Text,
                        cbx_loaisanpham.Text,
                        txt_mota.Text,
                        txt_soluong.Text,
                        txt_giaban.Text,
                        txt_giamgia.Text,
                        imgPath
                        );
                    bool kt = kn.thucthi(query);
                    if (kt)
                    {
                        MessageBox.Show("Thêm thành công");
                        getdata();
                        getclear();
                    }
                    else
                    {
                        MessageBox.Show("Thêm thất bại ");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi" + ex.Message);
            }
        }

        private void SanPham_Load(object sender, EventArgs e)
        {
            getdata();
            getclear();
            txt_soluong.Enabled = false;
        }

        private void kryptonButton13_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "JPEG Nitro|*.jpg;*.jpeg";

            if (open.ShowDialog() == DialogResult.OK)
            {
                string hinhanhFolder = Path.Combine(Application.StartupPath, "hinhanh");

                if (!Directory.Exists(hinhanhFolder))
                {
                    Directory.CreateDirectory(hinhanhFolder);
                }

                string selectedImagePath = open.FileName;
                string destinationPath = Path.Combine(hinhanhFolder, Path.GetFileName(selectedImagePath));
                string checkPath = destinationPath;
                if (File.Exists(checkPath))
                {
                    int count = 1;
                    while (File.Exists(checkPath))
                    {
                        string newName = $"anh_{count}.jpg";
                        checkPath = Path.Combine(hinhanhFolder, newName);
                        count++;
                    }

                    try
                    {
                        File.Copy(selectedImagePath, checkPath);
                        pic_anhsanpham.Image = Image.FromFile(checkPath);
                        imgPath = Path.Combine("hinhanh", Path.GetFileName(checkPath));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi " + ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        File.Copy(selectedImagePath, checkPath);
                        pic_anhsanpham.Image = Image.FromFile(checkPath);
                        imgPath = Path.Combine("hinhanh", Path.GetFileName(checkPath));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi " + ex.Message);
                    }
                }

            }
        }

        private void dgv_sanpham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void txt_timkiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("select * from sanpham where ID_sanpham like '%{0}%' or tensanpham like '%{0}%' or " +
                                                                         "loaisanpham like '%{0}%' or mota like '%{0}%' or soluong like '%{0}%' or " +
                                                                         "giaban like '%{0}%' or giamgia like '%{0}%' or hinhanh like '%{0}%'",
                                                                         txt_timkiem.Text
                                                                         );
                DataSet ds = kn.selectData(query);
                dgv_sanpham.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void kryptonButton12_Click(object sender, EventArgs e)
        {
            getclear();
        }

        private void txt_sanpham_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("update sanpham set tensanpham = N'{1}' , Loaisanpham = N'{2}' , Mota = N'{3}' , Soluong = '{4}' , giaban = '{5}' , giamgia = '{6}' , hinhanh = '{7}' where ID_sanpham = '{0}'",
                        txt_sanpham.Text,
                        txt_tensanpham.Text,
                        cbx_loaisanpham.Text,
                        txt_mota.Text,
                        txt_soluong.Text,
                        txt_giaban.Text,
                        txt_giamgia.Text,
                        imgPath
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

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("delete from sanpham where ID_sanpham =  '{0}'", txt_sanpham.Text);

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

        private void dgv_sanpham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgv_sanpham_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            txt_sanpham.Enabled = false;
            btn_xoa.Enabled = true;
            btn_sua.Enabled = true;
            btn_them.Enabled = false;
            if (r >= 0)
            {
                txt_sanpham.Text = dgv_sanpham.Rows[r].Cells["ID_sanpham"].Value.ToString();
                txt_tensanpham.Text = dgv_sanpham.Rows[r].Cells["tensanpham"].Value.ToString();
                cbx_loaisanpham.Text = dgv_sanpham.Rows[r].Cells["loaisanpham"].Value.ToString();
                txt_mota.Text = dgv_sanpham.Rows[r].Cells["mota"].Value.ToString();
                txt_soluong.Text = dgv_sanpham.Rows[r].Cells["soluong"].Value.ToString();
                txt_giaban.Text = dgv_sanpham.Rows[r].Cells["giaban"].Value.ToString();
                txt_giamgia.Text = dgv_sanpham.Rows[r].Cells["giamgia"].Value.ToString();
                string imgpath = dgv_sanpham.Rows[r].Cells["hinhanh"].Value.ToString();
                if (!string.IsNullOrEmpty(imgpath) && File.Exists(imgpath)) // Kiểm tra đường dẫn ảnh có hợp lệ không
                {
                    pic_anhsanpham.Image = Image.FromFile(imgpath); // Hiển thị ảnh lên PictureBox
                }
                else
                {
                    pic_anhsanpham.Image = null; // Nếu không có đường dẫn ảnh hợp lệ, xóa ảnh khỏi PictureBox
                }
            }
        }
    }
    }
