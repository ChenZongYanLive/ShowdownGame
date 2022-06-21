namespace ShowdownGame;

public class Deck
{
    public List<Card> Cards { get; set; }
    public int MaxCardCount = 52;

    private readonly Dictionary<string, int> _rankDictionary = new Dictionary<string, int>
    {
        { "2", 0 },
        { "3", 1 },
        { "4", 2 },
        { "5", 3 },
        { "6", 5 },
        { "7", 6 },
        { "8", 7 },
        { "9", 8 },
        { "10", 9 },
        { "J", 10 },
        { "Q", 11 },
        { "K", 12 },
        { "A", 13 },
    };
    public Deck()
    {
        Cards = new List<Card>();
        
        for (var i = 0; i < 4; i++)
        {
            foreach (var item in _rankDictionary)
            {
                var card = new Card
                {
                    Rank = item,
                    Suit = (Suit)i
                };
                Cards.Add(card);
            }
        }
    }


    public void Shuffle()
    {
        var random = new Random();
        var newList = new List<Card>();
        foreach (var item in Cards)
        {
            newList.Insert(random.Next(newList.Count + 1), item);
        }

        Cards = newList;
    }

    public Card DrawCard()
    {
        var firstCard = Cards.First();
        Cards.Remove(firstCard);
        return firstCard;
    }
}