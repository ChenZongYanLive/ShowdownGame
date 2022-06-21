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
            Console.WriteLine($"請{players[i]}玩家輸入名字：");
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
        
    }
}