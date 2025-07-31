using Cards.ScriptableObjects;

namespace Cards
{
    [System.Serializable]
    public class CardEntry
    {
        public BaseCard cardType;
        public int quantity;
    
        public CardEntry(BaseCard card, int qty)
        {
            cardType = card;
            quantity = qty;
        }
    }
}