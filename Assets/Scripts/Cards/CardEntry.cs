using Deviloop.ScriptableObjects;

namespace Deviloop
{
    [System.Serializable]
    public class CardEntry
    {
        public BaseCard Card;
        public int Quantity;
    
        public CardEntry(BaseCard card, int qty)
        {
            Card = card;
            Quantity = qty;
        }
    }
}