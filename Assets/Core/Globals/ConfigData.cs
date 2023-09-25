
using System.Collections.Generic;
/**
game data file format:
{ }"levels":[
{ "level_data": [ "50", "50", "950", "50", "950", "950", "50", "950" ] } // [x1, y1, x2, y2, x3, y3, x4, y4]
] }
*/
[System.Serializable]
public class ConfigData
{
    
    public List<LevelData> levels;
    //define the data
    public static ConfigData CONFIG_DATA;

}

[System.Serializable]
public class LevelData
{
    public List<string> level_data;
}