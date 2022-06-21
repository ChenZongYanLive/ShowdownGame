namespace ShowdownGame;

public class Player
{
    public Player()
    {
        Cards = new List<Card>();
    }
    public string? Name { get; set; }
    public List<Card> Cards { get; set; }
    
    public int Point { get; set; }

    public Card PlayingCard(int cardIndex)
    {
        var card = Cards.ToArray()[cardIndex];
        Cards.Remove(card);

        return card;
    }

    public void GainPoint()
    {
        Point++;
    }
}