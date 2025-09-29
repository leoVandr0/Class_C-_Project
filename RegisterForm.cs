using System;
using System.Windows.Forms;
using SmartEco.Services;

namespace SmartEco.Forms
{
    public partial class RegisterForm : Form
    {
        private AuthService _authService;

        public RegisterForm(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Register - SmartEco";
            this.Size = new System.Drawing.Size(450, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;

            // Title
            var lblTitle = new Label();
            lblTitle.Text = "Create Account";
            lblTitle.Font = new System.Drawing.Font("Arial", 20, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(0, 102, 204);
            lblTitle.Location = new System.Drawing.Point(125, 30);
            lblTitle.Size = new System.Drawing.Size(200, 40);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Form fields
            var controls = new Control[]
            {
                CreateLabel("Username:", 80),
                CreateTextBox("txtUsername", 80),
                CreateLabel("Email:", 130),
                CreateTextBox("txtEmail", 130),
                CreateLabel("Phone Number (EcoCash):", 180),
                CreateTextBox("txtPhone", 180),
                CreateLabel("Password:", 230),
                CreatePasswordBox("txtPassword", 230),
                CreateLabel("Confirm Password:", 280),
                CreatePasswordBox("txtConfirmPassword", 280)
            };

            // Register Button
            var btnRegister = new Button();
            btnRegister.Text = "Register";
            btnRegister.Location = new System.Drawing.Point(75, 350);
            btnRegister.Size = new System.Drawing.Size(300, 40);
            btnRegister.BackColor = System.Drawing.Color.FromArgb(0, 102, 204);
            btnRegister.ForeColor = System.Drawing.Color.White;
            btnRegister.Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            btnRegister.Click += BtnRegister_Click;

            // Back to Login
            var lblLogin = new Label();
            lblLogin.Text = "Already have an account? Login here";
            lblLogin.Location = new System.Drawing.Point(105, 410);
            lblLogin.Size = new System.Drawing.Size(240, 20);
            lblLogin.ForeColor = System.Drawing.Color.Blue;
            lblLogin.Cursor = Cursors.Hand;
            lblLogin.TextAlign = ContentAlignment.MiddleCenter;
            lblLogin.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.AddRange(controls);
            this.Controls.Add(lblTitle);
            this.Controls.Add(btnRegister);
            this.Controls.Add(lblLogin);

            this.ResumeLayout(false);
        }

        private Label CreateLabel(string text, int y)
        {
            return new Label
            {
                Text = text,
                Location = new System.Drawing.Point(75, y),
                Size = new System.Drawing.Size(150, 20)
            };
        }

        private TextBox CreateTextBox(string name, int y)
        {
            return new TextBox
            {
                Name = name,
                Location = new System.Drawing.Point(75, y + 25),
                Size = new System.Drawing.Size(300, 25)
            };
        }

        private TextBox CreatePasswordBox(string name, int y)
        {
            var txt = CreateTextBox(name, y);
            txt.PasswordChar = '*';
            return txt;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            var txtUsername = this.Controls.Find("txtUsername", true)[0] as TextBox;
            var txtEmail = this.Controls.Find("txtEmail", true)[0] as TextBox;
            var txtPhone = this.Controls.Find("txtPhone", true)[0] as TextBox;
            var txtPassword = this.Controls.Find("txtPassword", true)[0] as TextBox;
            var txtConfirmPassword = this.Controls.Find("txtConfirmPassword", true)[0] as TextBox;

            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill in all fields", "Registration Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match", "Registration Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_authService.Register(txtUsername.Text, txtEmail.Text, txtPassword.Text, txtPhone.Text))
            {
                MessageBox.Show("Registration successful! Please login.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Username or email already exists", "Registration Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}