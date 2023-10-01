
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int current_level = 0;
    // each level has points earned
    public List<int> completion = new List<int>();

    public GameData(GameManager manager)
    {
        this.current_level = manager.gameData.current_level;
        this.completion = manager.gameData.completion;
    }
}
