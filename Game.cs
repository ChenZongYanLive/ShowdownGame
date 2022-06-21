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
    }
}