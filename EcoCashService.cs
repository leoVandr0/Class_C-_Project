using System;
using System.Linq;
using System.Threading.Tasks;
using SmartEco.Models;

namespace SmartEco.Services
{
    public class EcoCashService
    {
        private AuthService _authService;
        private Random _random = new Random();

        public EcoCashService(AuthService authService)
        {
            _authService = authService;
        }

        public async Task<(bool success, string message, string reference)> DepositToTrading(
            string phoneNumber, decimal amount, string pin)
        {
            // Simulate API call delay
            await Task.Delay(2000);

            var user = _authService.CurrentUser;
            if (user == null)
                return (false, "User not logged in", null);

            if (user.PhoneNumber != phoneNumber)
                return (false, "Phone number doesn't match registered number", null);

            // Verify EcoCash balance (simulated)
            if (user.EcoCashBalance < amount)
                return (false, "Insufficient EcoCash balance", null);

            // Verify PIN (in real implementation, integrate with EcoCash API)
            if (!VerifyEcoCashPin(phoneNumber, pin))
                return (false, "Invalid EcoCash PIN", null);

            // Generate transaction reference
            string reference = $"EC{_random.Next(100000, 999999)}";

            // Update balances
            _authService.UpdateEcoCashBalance(user.Id, -amount);

            var account = _authService.GetUserAccount(user.Id);
            if (account != null)
            {
                _authService.UpdateAccountBalance(account.Id, amount);
            }

            return (true, $"Deposit of ${amount} successful!", reference);
        }

        public async Task<(bool success, string message, string reference)> WithdrawToEcoCash(
            decimal amount, string pin)
        {
            await Task.Delay(2000);

            var user = _authService.CurrentUser;
            if (user == null)
                return (false, "User not logged in", null);

            var account = _authService.GetUserAccount(user.Id);
            if (account == null || account.Balance < amount)
                return (false, "Insufficient trading account balance", null);

            if (!VerifyEcoCashPin(user.PhoneNumber, pin))
                return (false, "Invalid EcoCash PIN", null);

            string reference = $"EC{_random.Next(100000, 999999)}";

            // Update balances
            _authService.UpdateAccountBalance(account.Id, -amount);
            _authService.UpdateEcoCashBalance(user.Id, amount);

            return (true, $"Withdrawal of ${amount} successful!", reference);
        }

        private bool VerifyEcoCashPin(string phoneNumber, string pin)
        {
            // Simulated PIN verification
            // In real implementation, integrate with EcoCash API
            return pin.Length == 4 && pin.All(char.IsDigit);
        }

        public decimal GetEcoCashBalance()
        {
            return _authService.CurrentUser?.EcoCashBalance ?? 0m;
        }

        public decimal GetTradingBalance()
        {
            var user = _authService.CurrentUser;
            if (user == null) return 0m;

            var account = _authService.GetUserAccount(user.Id);
            return account?.Balance ?? 0m;
        }
    }
}