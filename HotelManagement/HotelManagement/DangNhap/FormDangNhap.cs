using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.DangNhap
{
    public partial class FormDangNhap : Form
    {
        // Lưu thông tin để màn hình chính sử dụng sau khi đăng nhập.
        public static string TenNV_DangNhap { get; private set; } = "";
        public static string MaNV_DangNhap { get; private set; } = "";
        public static int CapDoQuyen_DangNhap { get; private set; }

        public FormDangNhap()
        {
            InitializeComponent();

            AcceptButton = btnDangNhap;
            btnDangNhap.Click += btnDangNhap_Click;
        }

        private async void btnDangNhap_Click(object? sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text;

            if (string.IsNullOrWhiteSpace(tenDangNhap))
            {
                MessageBox.Show("Bạn chưa nhập tên đăng nhập.");
                txtTenDangNhap.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(matKhau))
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu.");
                txtMatKhau.Focus();
                return;
            }

            btnDangNhap.Enabled = false;

            try
            {
                using var db = new HotelManagementContext();

                var taiKhoan = await db.TaiKhoans
                    .AsNoTracking()
                    .Include(t => t.MaNvNavigation)
                    .FirstOrDefaultAsync(t =>
                        t.TenTk == tenDangNhap &&
                        t.DaXoa != true);

                if (taiKhoan == null ||
                    taiKhoan.Password != matKhau ||
                    taiKhoan.MaNvNavigation == null ||
                    taiKhoan.MaNvNavigation.DaXoa == true)
                {
                    MessageBox.Show(
                        "Tên đăng nhập hoặc mật khẩu không đúng, " +
                        "hoặc tài khoản không còn hoạt động.",
                        "Đăng nhập thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtMatKhau.SelectAll();
                    txtMatKhau.Focus();
                    return;
                }

                TenNV_DangNhap = taiKhoan.MaNvNavigation.TenNv;
                MaNV_DangNhap = taiKhoan.MaNv;
                CapDoQuyen_DangNhap = taiKhoan.CapDoQuyen;

                MessageBox.Show(
                    $"Xin chào {TenNV_DangNhap}!",
                    "Đăng nhập thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể đăng nhập.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnDangNhap.Enabled = true;
            }
        }
    }
}