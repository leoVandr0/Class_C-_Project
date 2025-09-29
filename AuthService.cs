using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SmartEco.Models;

namespace SmartEco.Services
{
    public class AuthService
    {
        private List<User> _users = new List<User>();
        private List<TradingAccount> _accounts = new List<TradingAccount>();
        private Random _random = new Random();

        public AuthService()
        {
            // Add some demo users
            CreateUser("demo", "demo@example.com", "password", "0771234567");
        }

        public User CurrentUser { get; private set; }

        public bool IsLoggedIn => CurrentUser != null;

        public bool Register(string username, string email, string password, string phoneNumber)
        {
            if (_users.Any(u => u.Username == username || u.Email == email))
            {
                return false; // User already exists
            }

            var user = CreateUser(username, email, password, phoneNumber);
            return user != null;
        }

        public bool Login(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        private User CreateUser(string username, string email, string password, string phoneNumber)
        {
            var user = new User
            {
                Id = _users.Count + 1,
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                PhoneNumber = phoneNumber,
                EcoCashBalance = 1000m // Starting EcoCash balance
            };

            _users.Add(user);

            // Create trading account for user
            var account = new TradingAccount
            {
                Id = _accounts.Count + 1,
                UserId = user.Id,
                AccountNumber = GenerateAccountNumber(),
                Balance = 0m
            };

            _accounts.Add(account);
            user.TradingAccounts.Add(account);

            return user;
        }

        private string GenerateAccountNumber()
        {
            return "TR" + _random.Next(10000000, 99999999).ToString();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }

        public TradingAccount GetUserAccount(int userId)
        {
            return _accounts.FirstOrDefault(a => a.UserId == userId);
        }

        public void UpdateAccountBalance(int accountId, decimal amount)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == accountId);
            if (account != null)
            {
                account.Balance += amount;
            }
        }

        public void UpdateEcoCashBalance(int userId, decimal amount)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.EcoCashBalance += amount;
            }
        }
    }
}