using System;
using System.Windows.Forms;
using SmartEco.Models;
using SmartEco.Services;

namespace SmartEco
{
    public partial class Form1 : Form
    {
        private AccountService accountService;
        private HistoryService historyService;
        private RecommendationService recommendationService;

        public Form1()
        {
            InitializeComponent();
            InitializeServices();
            InitializeCustomData();
        }

        private void InitializeServices()
        {
            accountService = new AccountService(1000m);
            historyService = new HistoryService();
            recommendationService = new RecommendationService();
        }

        private void InitializeCustomData()
        {
            cmbAirtimeAmounts.Items.AddRange(new object[] { "1", "2", "5", "10", "20" });
            cmbDataBundles.Items.AddRange(new object[] { "50MB - $1", "100MB - $2", "500MB - $5", "1GB - $10", "2GB - $20" });
            UpdateBalanceLabel();
        }

        private void UpdateBalanceLabel()
        {
            lblBalance.Text = $"Balance: ${accountService.GetBalance()}";
        }

        private void AddTransactionToHistory(Transaction tx)
        {
            historyService.Add(tx);
            lstHistory.Items.Add(tx.ToString());
        }

        private void btnBuyAirtime_Click(object sender, EventArgs e)
        {
            if (cmbAirtimeAmounts.SelectedItem == null)
            {
                MessageBox.Show("Select an airtime amount.");
                return;
            }

            var tx = new Transaction
            {
                Type = "Airtime",
                Phone = txtPhone.Text.Trim(),
                Detail = $"Bought ${cmbAirtimeAmounts.SelectedItem}",
                Amount = Convert.ToDecimal(cmbAirtimeAmounts.SelectedItem)
            };

            if (accountService.TryPurchase(tx, out string message))
            {
                AddTransactionToHistory(tx);
                UpdateBalanceLabel();
            }

            MessageBox.Show(message);
        }

        private void btnBuyData_Click(object sender, EventArgs e)
        {
            if (cmbDataBundles.SelectedItem == null)
            {
                MessageBox.Show("Select a data bundle.");
                return;
            }

            string bundle = cmbDataBundles.SelectedItem.ToString();
            decimal price = Convert.ToDecimal(bundle.Split('$')[1]);

            var tx = new Transaction
            {
                Type = "Data",
                Phone = txtPhone.Text.Trim(),
                Detail = $"Bought {bundle}",
                Amount = price
            };

            if (accountService.TryPurchase(tx, out string message))
            {
                AddTransactionToHistory(tx);
                recommendationService.RecordBundle(bundle);
                UpdateBalanceLabel();

                string top = recommendationService.GetTopRecommendation();
                if (top != null)
                {
                    MessageBox.Show($"💡 Most popular bundle: {top}");
                }
            }

            MessageBox.Show(message);
        }

        private void btnSaveHistory_Click(object sender, EventArgs e)
        {
            historyService.SaveToFile();
            MessageBox.Show("History saved to transaction_history.txt");
        }

        private void btnClearHistory_Click(object sender, EventArgs e)
        {
            historyService.Clear();
            lstHistory.Items.Clear();
            MessageBox.Show("Transaction history cleared.");
        }

        private void Form1_Load(object sender, EventArgs e) { }
    }
}