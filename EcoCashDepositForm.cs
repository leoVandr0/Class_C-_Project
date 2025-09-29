using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using SmartEco.Services;

namespace SmartEco.Forms
{
    public partial class EcoCashDepositForm : Form
    {
        private EcoCashService _ecocashService;
        private AuthService _authService;

        public EcoCashDepositForm(EcoCashService ecocashService, AuthService authService)
        {
            InitializeComponent();
            _ecocashService = ecocashService;
            _authService = authService;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "EcoCash Deposit";
            this.Size = new System.Drawing.Size(400, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.White;

            // Title
            var lblTitle = new Label();
            lblTitle.Text = "Deposit via EcoCash";
            lblTitle.Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(0, 102, 204);
            lblTitle.Location = new System.Drawing.Point(100, 30);
            lblTitle.Size = new System.Drawing.Size(200, 30);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Phone Number
            var lblPhone = new Label();
            lblPhone.Text = "EcoCash Number:";
            lblPhone.Location = new System.Drawing.Point(50, 90);
            lblPhone.Size = new System.Drawing.Size(120, 20);

            var txtPhone = new TextBox();
            txtPhone.Location = new System.Drawing.Point(50, 115);
            txtPhone.Size = new System.Drawing.Size(300, 25);
            txtPhone.Text = _authService.CurrentUser?.PhoneNumber;
            txtPhone.Enabled = false;

            // Amount
            var lblAmount = new Label();
            lblAmount.Text = "Amount (USD):";
            lblAmount.Location = new System.Drawing.Point(50, 160);
            lblAmount.Size = new System.Drawing.Size(100, 20);

            var txtAmount = new NumericUpDown();
            txtAmount.Location = new System.Drawing.Point(50, 185);
            txtAmount.Size = new System.Drawing.Size(300, 25);
            txtAmount.Minimum = 1;
            txtAmount.Maximum = 1000;
            txtAmount.Value = 10;
            txtAmount.Name = "txtAmount";

            // PIN
            var lblPin = new Label();
            lblPin.Text = "EcoCash PIN:";
            lblPin.Location = new System.Drawing.Point(50, 230);
            lblPin.Size = new System.Drawing.Size(100, 20);

            var txtPin = new TextBox();
            txtPin.Location = new System.Drawing.Point(50, 255);
            txtPin.Size = new System.Drawing.Size(300, 25);
            txtPin.PasswordChar = '*';
            txtPin.MaxLength = 4;
            txtPin.Name = "txtPin";

            // Deposit Button
            var btnDeposit = new Button();
            btnDeposit.Text = "Deposit";
            btnDeposit.Location = new System.Drawing.Point(50, 310);
            btnDeposit.Size = new System.Drawing.Size(300, 40);
            btnDeposit.BackColor = System.Drawing.Color.FromArgb(0, 102, 204);
            btnDeposit.ForeColor = System.Drawing.Color.White;
            btnDeposit.Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            btnDeposit.Click += async (s, e) => await BtnDeposit_Click(txtAmount, txtPin);

            // Balances
            var lblEcoCashBalance = new Label();
            lblEcoCashBalance.Text = $"EcoCash Balance: ${_ecocashService.GetEcoCashBalance()}";
            lblEcoCashBalance.Location = new System.Drawing.Point(50, 70);
            lblEcoCashBalance.Size = new System.Drawing.Size(200, 15);
            lblEcoCashBalance.ForeColor = System.Drawing.Color.Green;

            this.Controls.AddRange(new Control[] {
                lblTitle, lblPhone, txtPhone, lblAmount, txtAmount,
                lblPin, txtPin, btnDeposit, lblEcoCashBalance
            });

            this.ResumeLayout(false);
        }

        private async Task BtnDeposit_Click(NumericUpDown txtAmount, TextBox txtPin)
        {
            if (txtPin.Text.Length != 4 || !int.TryParse(txtPin.Text, out _))
            {
                MessageBox.Show("Please enter a valid 4-digit PIN", "Invalid PIN",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnDeposit.Enabled = false;
            btnDeposit.Text = "Processing...";

            var result = await _ecocashService.DepositToTrading(
                _authService.CurrentUser.PhoneNumber,
                (decimal)txtAmount.Value,
                txtPin.Text
            );

            btnDeposit.Enabled = true;
            btnDeposit.Text = "Deposit";

            if (result.success)
            {
                MessageBox.Show($"{result.message}\nReference: {result.reference}", "Deposit Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(result.message, "Deposit Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}