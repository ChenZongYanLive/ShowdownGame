using System.Linq;
using System.Reflection.Metadata;

namespace ShowdownGame;

public class Game
{
    private Dictionary<string, Player> Players = new Dictionary<string, Player>();
    public Game()
    {
        
    }
    
    public void StartGame()
    {
        //產生玩家
        var players = new[] { "P1" , "P2" , "P3" , "P4"};
        const int humanPlayerType = 1;
        const int aiPlayerType = 2;
        
        Console.WriteLine("請設定P1 ~ P4玩家為 [真實玩家] 還是 [電腦玩家]");
        Console.WriteLine("若為真實玩家請輸入[1]");
        Console.WriteLine("若為電腦玩家請輸入[2]");
        
        for (var i = 0; i < players.Length; i++)
        {
            int playerType;
            do
            {
                Console.WriteLine($"請設定{players[i]}為：");
                Console.WriteLine("1. [真實玩家] , 2. [電腦玩家]");
                
                if (!int.TryParse(Console.ReadLine(), out playerType) || !new[] {humanPlayerType, aiPlayerType}.Contains(playerType))
                {
                    Console.WriteLine("請輸入正確的資訊");
                }

            } while (!new[] {humanPlayerType, aiPlayerType}.Contains(playerType));

            switch (Convert.ToInt32(playerType))
            {
                case humanPlayerType:
                {
                    var player = new HumanPlayer();
                    Players.Add(players[i], player);
                    break;
                }
                case aiPlayerType:
                {
                    var player = new AiPlayer();
                    Players.Add(players[i], player);
                    break;   
                }
            }
        }

        var deck = new Deck();
        
        Console.WriteLine("遊戲開始!!");

        for (var i = 0; i < players.Length; i++)
        {
            Console.WriteLine($"請{players[i]}玩家輸入名字：");//TODO Ai玩家 > 隨機選擇
            var name = Console.ReadLine();
            Players[players[i]].Name = name;
        }
        
        Console.WriteLine("進行洗牌......");
        deck.Shuffle();
        
        Console.WriteLine("開始進行抽牌");
        while (deck.Cards.Count > 0)
        {
            for (var i = 0; i < players.Length; i++)
            {
                Players[players[i]].Cards.Add(deck.DrawCard());
            }
        }
        
        Console.WriteLine("開始進行比大小");
        var maxRound = 1;
        for (var i = 1; i <= maxRound; i++)
        {
            Console.WriteLine($"第{i}回合，請{players[i]}玩家開始");
            var playsOfShowCard = new Dictionary<string, Card>();
            for (var j = 0; j < players.Length; j++)
            {
                var player = Players[players[j]];
                if (player.IsExchangePlayer)
                {
                    player.ExChangePlayerRound++;
                    
                    if (player.ExChangePlayerRound > 3)
                    {
                        var exchangePlayer = Players[player.ExChangePlayerId];
                        var exchangePlayerCards = exchangePlayer.Cards;
                        var myCards = player.Cards;
                        
                        player.Cards = exchangePlayerCards;
                        exchangePlayer.Cards = myCards;
                        
                        Console.WriteLine($"交換手牌回合結束，與{player.ExChangePlayerId}玩家交換回來");
                    }
                }
                
                
                Console.WriteLine($"{players[j]}玩家請選擇是否要交換手牌");//TODO Ai玩家 > 隨機選擇
                Console.WriteLine("若要交換手牌請輸入[Y]");
                Console.WriteLine("沒有要交換則輸入[N]");
                var isExchangeHands = Console.ReadLine();
                if (isExchangeHands != null && isExchangeHands.ToUpper().Equals("Y"))
                {
                    Console.WriteLine($"你可以跟這些玩家交換手牌：{string.Join(',',players.Where(x=>x != players[j]).ToArray())}");
                    Console.WriteLine("請選擇你要跟哪位玩家交換");
                    var exchangePlayerId = Console.ReadLine();//TODO Ai玩家 > 隨機選擇
                    var exchangePlayer = Players[exchangePlayerId];
                    var exchangePlayerCards = exchangePlayer.Cards;
                    var myCards = player.Cards;

                    player.Cards = exchangePlayerCards;
                    exchangePlayer.Cards = myCards;

                    player.IsExchangePlayer = true;
                    player.ExChangePlayerRound = 1;
                    player.ExChangePlayerId = exchangePlayerId;
                    
                    Console.WriteLine("交換完畢");
                }
                
                Console.WriteLine($"請輸入號碼選擇你要出的牌，手牌:");
                for (var k = 0; k < player.Cards.Count; k++)
                {
                    var card = player.Cards[k];
                    Console.WriteLine($"號碼{k}.[花色{card.Suit}/階級{card.Rank.Key}]");
                }
                var cardIndex = Convert.ToInt32(Console.ReadLine());//TODO Ai玩家 > 隨機選擇
                var playingCard = player.PlayingCard(cardIndex);
                playsOfShowCard.Add(players[j], playingCard);
            }

            KeyValuePair<string, Card> winner = default;
            foreach (var item in playsOfShowCard)
            {
                if (winner.Equals(default(KeyValuePair<string, Card>)))
                {
                    winner = new KeyValuePair<string, Card>(item.Key, item.Value);

                }
                else
                {
                    if (item.Value.Rank.Value > winner.Value.Rank.Value)
                    {
                        winner = new KeyValuePair<string, Card>(item.Key, item.Value);
                    }
                    else if (item.Value.Rank.Value == winner.Value.Rank.Value)
                    {
                        if (item.Value.Suit > winner.Value.Suit)
                        {
                            winner = new KeyValuePair<string, Card>(item.Key, item.Value);
                        }
                    }
                }
            }
            Console.WriteLine($"這回合最勝者為{winner.Key}玩家");
            Players[winner.Key].GainPoint();
        }

        KeyValuePair<string, Player> finalWinner = default;
        foreach (var item in Players)
        {
            if (finalWinner.Equals(default(KeyValuePair<string, Player>)))
            {
                finalWinner = new KeyValuePair<string, Player>(item.Key, item.Value);
            }
            else
            {
                if (item.Value.Point > finalWinner.Value.Point)
                {
                    finalWinner = new KeyValuePair<string, Player>(item.Key, item.Value);
                }
            }
        }
        
        Console.WriteLine($"最勝者玩家為:{finalWinner.Key}.{finalWinner.Value.Name}");
    }
}