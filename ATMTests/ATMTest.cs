using ATM;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ATMTests
{
    public class ATMTest
    {
        private ATMManager SetupATM(BankCard enterredCard)
        {
            var cardRepo = new List<BankCard>()
            {
                new BankCard()
                {
                    CardId = "1",
                    PIN = 1122,
                    Balance = 10.0
                },
                new BankCard()
                {
                    CardId = "2",
                    PIN = 1111,
                    Balance = 100.0
                }
            };
            var atm = new ATMManager(enterredCard, cardRepo);
            return atm;
        }

        [Fact]
        public void Authenticate_UsingValidPIN()
        {
            var userCard = new BankCard()
            {
                CardId = "2",
                PIN = 1111
            };
            var atm = SetupATM(userCard);
            atm.IsAuthenticated.Should().BeTrue();
        }

        [Fact]
        public void Authenticate_UsingInvalidPIN()
        {
            var userCard = new BankCard()
            {
                CardId = "2",
                PIN = 1112
            };
            var atm = SetupATM(userCard);
            atm.IsAuthenticated.Should().BeFalse();
        }

        [Fact]
        public void Withdraw_CheckBalance()
        {
            var userCard = new BankCard()
            {
                CardId = "2",
                PIN = 1111
            };
            var atm = SetupATM(userCard);
            var currentBalance = atm.Withdraw(10);
            currentBalance.Should().Be(90);
        }

        [Fact]
        public void Deposit_CheckBalance()
        {
            var userCard = new BankCard()
            {
                CardId = "2",
                PIN = 1111
            };
            var atm = SetupATM(userCard);
            var currentBalance = atm.Deposit(10);
            currentBalance.Should().Be(110);
        }

        [Fact]
        public void SinglePayment_CheckBalance()
        {
            var userCard = new BankCard()
            {
                CardId = "2",
                PIN = 1111
            };
            var atm = SetupATM(userCard);
            atm.Payment(10);
            var currentBalance = atm.GetCurrentBalance(userCard.CardId);
            currentBalance.Should().Be(90);
        }

        [Fact]
        public void MultiplePayment_CheckBalance()
        {
            var userCard = new BankCard()
            {
                CardId = "2",
                PIN = 1111
            };
            var atm = SetupATM(userCard);
            var tasks = new[]
            {
                Task.Factory.StartNew(() => atm.Payment(10)),
                Task.Factory.StartNew(() => atm.Payment(10)),
                Task.Factory.StartNew(() => atm.Payment(10))
            };
            Task.WaitAll(tasks);
            var currentBalance = atm.GetCurrentBalance(userCard.CardId);
            currentBalance.Should().Be(70);
        }
    }
}
