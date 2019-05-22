using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM
{
    public class CardAuthenticator
    {
        private readonly IEnumerable<BankCard> _cardRepository;

        public CardAuthenticator(IEnumerable<BankCard> cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public IEnumerable<BankCard> CardRepository { get; }
        public BankCard ValidBankAccount { get; private set; }

        /// <summary>
        /// Validate Bank Card Enterred
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool Authenticate(BankCard card)
        {
            ValidBankAccount = _cardRepository.ToList().SingleOrDefault(_ => _.CardId == card.CardId && _.PIN == card.PIN);
            if (ValidBankAccount != null)
                return true;
            else
                return false;
        }
    }
}