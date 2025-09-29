using System;
using System.Windows.Forms;
using SmartEco.Services;

namespace SmartEco.Forms
{
    public partial class LoginForm : Form
    {
        private AuthService _authService;

        public LoginForm(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form setup
            this.Text = "Login - SmartEco";
            this.Size = new System.Drawing.Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;

            // Title
            var lblTitle = new Label();
            lblTitle.Text = "SmartEco Login";
            lblTitle.Font = new System.Drawing.Font("Arial", 20, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(0, 102, 204);
            lblTitle.Location = new System.Drawing.Point(100, 50);
            lblTitle.Size = new System.Drawing.Size(200, 40);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Username
            var lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Location = new System.Drawing.Point(50, 130);
            lblUsername.Size = new System.Drawing.Size(100, 20);

            var txtUsername = new TextBox();
            txtUsername.Location = new System.Drawing.Point(50, 155);
            txtUsername.Size = new System.Drawing.Size(300, 25);
            txtUsername.Name = "txtUsername";

            // Password
            var lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Location = new System.Drawing.Point(50, 200);
            lblPassword.Size = new System.Drawing.Size(100, 20);

            var txtPassword = new TextBox();
            txtPassword.Location = new System.Drawing.Point(50, 225);
            txtPassword.Size = new System.Drawing.Size(300, 25);
            txtPassword.PasswordChar = '*';
            txtPassword.Name = "txtPassword";

            // Login Button
            var btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Location = new System.Drawing.Point(50, 280);
            btnLogin.Size = new System.Drawing.Size(300, 40);
            btnLogin.BackColor = System.Drawing.Color.FromArgb(0, 102, 204);
            btnLogin.ForeColor = System.Drawing.Color.White;
            btnLogin.Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            btnLogin.Click += (s, e) =>
            {
                if (_authService.Login(txtUsername.Text, txtPassword.Text))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // Register Link
            var lblRegister = new Label();
            lblRegister.Text = "Don't have an account? Register here";
            lblRegister.Location = new System.Drawing.Point(80, 340);
            lblRegister.Size = new System.Drawing.Size(240, 20);
            lblRegister.ForeColor = System.Drawing.Color.Blue;
            lblRegister.Cursor = Cursors.Hand;
            lblRegister.TextAlign = ContentAlignment.MiddleCenter;
            lblRegister.Click += (s, e) =>
            {
                var registerForm = new RegisterForm(_authService);
                if (registerForm.ShowDialog() == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblTitle, lblUsername, txtUsername, lblPassword, txtPassword,
                btnLogin, lblRegister
            });

            this.ResumeLayout(false);
        }
    }
}