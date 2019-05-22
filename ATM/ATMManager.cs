using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATM
{
    public class ATMManager
    {
        private List<BankCard> _cardRepository;
        private readonly CardAuthenticator _authenticator;

        public bool IsAuthenticated { get; private set; }
        private readonly object cardLock = new object();

        public ATMManager(BankCard enterredCard, IEnumerable<BankCard> cardRepository)
        {
            _cardRepository = cardRepository.ToList();
            _authenticator = new CardAuthenticator(cardRepository);
            IsAuthenticated = _authenticator.Authenticate(enterredCard);
            
        }

        private void CheckPinValid()
        {
            if(!IsAuthenticated)
                throw new Exception("Pin Invalid!");
            return;
        }

        private BankCard GetLatestCardData(string cardId)
        {
            var card = _cardRepository.Single(_ => _.CardId == cardId);
            return card;
        }

        private bool UpdateCardData(BankCard card)
        {
            _cardRepository.Remove(card);
            _cardRepository.Add(card);
            return true;
        }
        /// <summary>
        /// Get Current Balance
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public double GetCurrentBalance(string cardId)
        {
            return GetLatestCardData(cardId).Balance;
        }
        
        /// <summary>
        /// Make a Widrawal
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public double Withdraw(double amount)
        {
            CheckPinValid();
            lock(cardLock)
            {
                var card = GetLatestCardData(_authenticator.ValidBankAccount.CardId);
                if (card.Balance >= amount)
                {
                    card.Balance -= amount;
                    UpdateCardData(card);
                    return GetLatestCardData(card.CardId).Balance;
                }
                else
                    throw new Exception("Insufficient balance!");

            }
        }
        /// <summary>
        /// Make a Deposit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public double Deposit(double amount)
        {
            CheckPinValid();
            lock (cardLock)
            {
                var card = GetLatestCardData(_authenticator.ValidBankAccount.CardId);
                card.Balance += amount;
                UpdateCardData(card);
                return GetLatestCardData(card.CardId).Balance;
                

            }
        }
        /// <summary>
        /// Make a payment online
        /// </summary>
        /// <param name="amount"></param>
        public void Payment(double amount)
        {
            Withdraw(amount);
        }
    }
}
