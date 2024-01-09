using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace QuanLyTrangSuc
{
    public partial class ThongKeSanPham : Form
    {
        KetNoi kn = new KetNoi();
        public ThongKeSanPham()
        {
            InitializeComponent();
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

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ThongKeSanPham frm = new ThongKeSanPham();
            frm.ShowDialog();
            this.Close();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            this.Hide();
            ThongKe frm = new ThongKe();
            frm.ShowDialog();
            this.Close();
        }
        void get_year_sanpham()
        {
            try
            {
                string query = "SELECT DISTINCT YEAR(NgayXuat) AS NamXuat FROM HoaDon";
                DataSet ds = kn.selectData(query);
                cbx_sanpham.DataSource = ds.Tables[0];
                cbx_sanpham.DisplayMember = "Nam";
                cbx_sanpham.ValueMember = "NamXuat";
                cbx_sanpham.Text = "2023";

            }
            catch (Exception ex)
            {

            }
        }
        private void cbx_sanpham_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Điều chỉnh các thuộc tính của biểu đồ
                chart_thongkedoanhso.Series.Clear();
                string query = $"SELECT ID_sanpham, SUM(SoLuong) AS " +
                    $"TongSoLuong FROM ChiTietHoaDon cthd, HoaDon hd WHERE cthd.ID_HoaDon = hd.ID_HoaDon and year(NgayXuat) = {cbx_sanpham.Text} GROUP BY ID_sanpham";
                DataSet ds = kn.selectData(query);
                // Đặt loại biểu đồ là hình tròn
                Series series = new Series("SalesByProduct");
                series.ChartType = SeriesChartType.Pie;

                // Lấy dữ liệu từ CSDL của bạn


                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string productName = row["ID_sanpham"].ToString(); // Thay bằng cột chứa tên sản phẩm nếu có
                    int quantity = Convert.ToInt32(row["TongSoLuong"]);

                    // Thêm dữ liệu vào series của biểu đồ
                    series.Points.AddXY(productName, quantity);
                }

                // Thêm series vào biểu đồ
                chart_thongkedoanhso.Series.Add(series);
            }
            catch
            {

            }
        }

        private void ThongKeSanPham_Load(object sender, EventArgs e)
        {
            get_year_sanpham();
        }
    }
}
