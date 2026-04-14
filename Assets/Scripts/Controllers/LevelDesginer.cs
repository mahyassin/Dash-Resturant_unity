using System.Collections.Generic;
using UnityEngine;

public class LevelDesginer : MonoBehaviour
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

    public GameState BuildGame(string[] mapCode)
    {
        int mapHieght = mapCode.Length;
        int mapWidth  = mapCode[0].Length;

        Dictionary<Vector2Int, CellState> map = new();
        PlayerState player = null;

        for(int y = 0; y < mapHieght; y++)
        {
            char basetile = '?';
            char ontile = '?';

            for(int x = 0; x < mapWidth; x++)
            {
                if(basetile == '?') {basetile = mapCode[y][x]; continue;}
                if(ontile == '?') {ontile = mapCode[y][x];}

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

                if (baseCell == Code.PLAYER ) player = new(new(x, y));
                map[new(x, mapHieght - y -1)] = new(oncell: oncell, baseCell: baseCell);
            }
        }
        
        return new(map, player);
    }
}


public readonly struct Code
{
    public static   int EMPTY     => 0;
    public static   int PLAYER    => 1;
    public static   int WALL      => 2;
    public static   int GENERATOR => 4;

   
}
