using System;
using System.Drawing;
using System.Windows.Forms;
using SmartEco.Models;
using SmartEco.Services;
using SmartEco.Forms;

namespace SmartEco
{
    public partial class Form1 : Form
    {
        private AuthService authService;
        private EcoCashService ecoCashService;
        private AccountService accountService;
        private HistoryService historyService;
        private RecommendationService recommendationService;

        // UI Controls
        private MenuStrip mainMenu;
        private StatusStrip statusStrip;
        private Label lblWelcome;
        private Label lblEcoCashBalance;
        private Label lblTradingBalance;
        private Button btnLogin;
        private Button btnDeposit;
        private Button btnWithdraw;
        private Panel tradingPanel;

        public Form1()
        {
            InitializeServices();
            InitializeComponent();
            UpdateUI();
        }

        private void InitializeServices()
        {
            authService = new AuthService();
            ecoCashService = new EcoCashService(authService);
            accountService = new AccountService(0m); // Will be updated after login
            historyService = new HistoryService();
            recommendationService = new RecommendationService();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form setup
            this.Text = "SmartEco - Trading Platform";
            this.Size = new System.Drawing.Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Main Menu
            mainMenu = new MenuStrip();
            var fileMenu = new ToolStripMenuItem("File");
            var loginMenu = new ToolStripMenuItem("Login", null, ShowLoginForm);
            var registerMenu = new ToolStripMenuItem("Register", null, ShowRegisterForm);
            var logoutMenu = new ToolStripMenuItem("Logout", null, Logout);
            var exitMenu = new ToolStripMenuItem("Exit", null, (s, e) => Application.Exit());

            fileMenu.DropDownItems.AddRange(new[] { loginMenu, registerMenu, logoutMenu, new ToolStripSeparator(), exitMenu });
            mainMenu.Items.Add(fileMenu);

            // EcoCash Menu
            var ecocashMenu = new ToolStripMenuItem("EcoCash");
            var depositMenu = new ToolStripMenuItem("Deposit", null, ShowDepositForm);
            var withdrawMenu = new ToolStripMenuItem("Withdraw", null, ShowWithdrawalForm);
            var balanceMenu = new ToolStripMenuItem("Check Balance", null, RefreshBalances);

            ecocashMenu.DropDownItems.AddRange(new[] { depositMenu, withdrawMenu, new ToolStripSeparator(), balanceMenu });
            mainMenu.Items.Add(ecocashMenu);

            // Status Strip
            statusStrip = new StatusStrip();
            var statusLabel = new ToolStripStatusLabel();
            statusLabel.Text = "Ready";
            statusStrip.Items.Add(statusLabel);

            // Header Panel
            var headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 80;
            headerPanel.BackColor = Color.FromArgb(0, 102, 204);

            lblWelcome = new Label();
            lblWelcome.Text = "Welcome to SmartEco Trading";
            lblWelcome.Font = new Font("Arial", 16, FontStyle.Bold);
            lblWelcome.ForeColor = Color.White;
            lblWelcome.Location = new Point(20, 20);
            lblWelcome.AutoSize = true;

            btnLogin = new Button();
            btnLogin.Text = "Login / Register";
            btnLogin.Location = new Point(700, 25);
            btnLogin.Size = new Size(150, 30);
            btnLogin.BackColor = Color.White;
            btnLogin.ForeColor = Color.FromArgb(0, 102, 204);
            btnLogin.Click += ShowLoginForm;

            // Balance Panel
            var balancePanel = new Panel();
            balancePanel.Dock = DockStyle.Top;
            balancePanel.Height = 60;
            balancePanel.Top = 80;

            lblEcoCashBalance = new Label();
            lblEcoCashBalance.Text = "EcoCash: $0.00";
            lblEcoCashBalance.Location = new Point(20, 20);
            lblEcoCashBalance.Size = new Size(200, 20);
            lblEcoCashBalance.ForeColor = Color.Green;
            lblEcoCashBalance.Font = new Font("Arial", 10, FontStyle.Bold);

            lblTradingBalance = new Label();
            lblTradingBalance.Text = "Trading Account: $0.00";
            lblTradingBalance.Location = new Point(250, 20);
            lblTradingBalance.Size = new Size(200, 20);
            lblTradingBalance.ForeColor = Color.Blue;
            lblTradingBalance.Font = new Font("Arial", 10, FontStyle.Bold);

            btnDeposit = new Button();
            btnDeposit.Text = "Deposit";
            btnDeposit.Location = new Point(500, 15);
            btnDeposit.Size = new Size(80, 30);
            btnDeposit.Click += ShowDepositForm;

            btnWithdraw = new Button();
            btnWithdraw.Text = "Withdraw";
            btnWithdraw.Location = new Point(590, 15);
            btnWithdraw.Size = new Size(80, 30);
            btnWithdraw.Click += ShowWithdrawalForm;

            balancePanel.Controls.AddRange(new Control[] {
                lblEcoCashBalance, lblTradingBalance, btnDeposit, btnWithdraw
            });

            // Trading Panel (existing trading interface)
            tradingPanel = new Panel();
            tradingPanel.Dock = DockStyle.Fill;
            tradingPanel.Top = 140;
            InitializeTradingPanel();

            // Add to form
            headerPanel.Controls.Add(lblWelcome);
            headerPanel.Controls.Add(btnLogin);

            this.Controls.AddRange(new Control[] {
                mainMenu, headerPanel, balancePanel, tradingPanel, statusStrip
            });
            this.MainMenuStrip = mainMenu;

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void InitializeTradingPanel()
        {
            // Your existing trading interface components go here
            // This would include the comboboxes, buttons, listbox from your original code
            // I'm keeping it simple for this example

            var lblTrading = new Label();
            lblTrading.Text = "Trading Interface";
            lblTrading.Location = new Point(20, 20);
            lblTrading.Size = new Size(200, 20);
            lblTrading.Font = new Font("Arial", 12, FontStyle.Bold);

            tradingPanel.Controls.Add(lblTrading);
        }

        private void UpdateUI()
        {
            bool isLoggedIn = authService.IsLoggedIn;

            if (isLoggedIn)
            {
                var user = authService.CurrentUser;
                lblWelcome.Text = $"Welcome, {user.Username}!";
                btnLogin.Text = "Logout";
                btnLogin.Click -= ShowLoginForm;
                btnLogin.Click += Logout;

                // Update balances
                lblEcoCashBalance.Text = $"EcoCash: ${ecoCashService.GetEcoCashBalance():F2}";
                lblTradingBalance.Text = $"Trading: ${ecoCashService.GetTradingBalance():F2}";

                // Update account service with user's balance
                accountService = new AccountService(ecoCashService.GetTradingBalance());
            }
            else
            {
                lblWelcome.Text = "Welcome to SmartEco Trading";
                btnLogin.Text = "Login / Register";
                btnLogin.Click -= Logout;
                btnLogin.Click += ShowLoginForm;

                lblEcoCashBalance.Text = "EcoCash: $0.00";
                lblTradingBalance.Text = "Trading: $0.00";
            }

            // Enable/disable trading features based on login status
            btnDeposit.Enabled = isLoggedIn;
            btnWithdraw.Enabled = isLoggedIn;
            tradingPanel.Enabled = isLoggedIn;
        }

        private void ShowLoginForm(object sender, EventArgs e)
        {
            var loginForm = new LoginForm(authService);
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                UpdateUI();
                MessageBox.Show($"Welcome back, {authService.CurrentUser.Username}!", "Login Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ShowRegisterForm(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm(authService);
            if (registerForm.ShowDialog() == DialogResult.OK)
            {
                UpdateUI();
                MessageBox.Show("Registration successful! You are now logged in.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Logout(object sender, EventArgs e)
        {
            authService.Logout();
            UpdateUI();
            MessageBox.Show("You have been logged out.", "Logout",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowDepositForm(object sender, EventArgs e)
        {
            var depositForm = new EcoCashDepositForm(ecoCashService, authService);
            if (depositForm.ShowDialog() == DialogResult.OK)
            {
                UpdateUI();
            }
        }

        private void ShowWithdrawalForm(object sender, EventArgs e)
        {
            // Similar to deposit form - you would create EcoCashWithdrawalForm
            MessageBox.Show("Withdrawal feature coming soon!", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RefreshBalances(object sender, EventArgs e)
        {
            UpdateUI();
            MessageBox.Show("Balances updated!", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Your existing trading methods would go here...
        // btnBuyAirtime_Click, btnBuyData_Click, etc.
    }
}