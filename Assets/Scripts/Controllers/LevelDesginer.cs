using System.Collections.Generic;
using UnityEngine;

public class LevelDesginer
{
    string[] _map =
    {
        "WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW WW WW WW WW .. WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW .. WW WW WW WW WW WW WW WW WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW .. .. ▼. .. .. .. .. .. .. .. .. WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW",
      
    };


    public GameState GetState()
    {
        return BuildGame(_map);
    }

    public GameState BuildGame(string[] mapCode)
    {
        int mapHieght = mapCode.Length;
        int mapWidth  = mapCode[0].Replace(" ","").Length/ 2;

        Dictionary<Vector2Int, CellState> map = new();
        PlayerState player = null;

        int y = 0;
        foreach(var line in mapCode)
        {

            char basetile = '?';
            char ontile = '?';

            int x = 0;
            foreach(var symbol in _map[y].Replace(" ", ""))
            {

                Debug.Log(symbol);
                if(basetile == '?') {basetile = symbol; continue;}
                if(ontile == '?') {ontile = symbol;}

                int oncell = ontile switch
                {
                    _ => Code.EMPTY,
                };


                int baseCell = basetile switch
                {
                    'W' => Code.WALL,
                    '▼' => Code.PLAYER,
                    _   => Code.EMPTY,
                };

                if (baseCell == Code.WALL) oncell = Code.WALL;

                if (baseCell == Code.PLAYER ) player = new(new(x, y));
                map[new(x, y)] = new(oncell: oncell, baseCell: baseCell);

                Debug.Log($"base tile is {baseCell} read tile {basetile}");

                basetile = '?';
                ontile = '?';

                x++;

            }
            y++;
        }
        
        return new(map, player, mapWidth, mapHieght);
    }
}


public readonly struct Code
{
    public const int EMPTY     = 0;
    public const int PLAYER    = 1;
    public const int WALL      = 2;
    public const int GENERATOR = 4;

   
}
