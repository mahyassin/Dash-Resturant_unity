using System.Collections.Generic;
using UnityEngine;

public class LevelDesginer
{
    string[] _map =
    {
        "WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
        "WW WW WW WW WW .. WW WW WW WW WW WW .. WW WW WW",
        "WW Go .. .. .. .. .. .. .. .. S. WW .. .. .. WW",
        "WW Gt .. .. .. .. .. .. .. .. C. WW .. .. .. WW",
        "WW Gp .. ▼. .. .. .. .. .. .. .. WW .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. WW .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. WW .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. WW .. .. .. WW",
        "WW .. .. .. .. .. .. .. .. .. .. WW .. .. .. WW",
        "WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW",
      
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

            int j = mapHieght - y -1;
            char basetile = '?';
            char ontile = '?';

            int x = 0;
            foreach(var symbol in _map[y].Replace(" ", ""))
            {

                if(basetile == '?') {basetile = symbol; continue;}
                if(ontile == '?') {ontile = symbol;}

                int oncell = ontile switch
                {
                    _ => Code.EMPTY,
                };


                IOcuppier ocuppier = basetile switch
                {
                    'W' => new Wall(),
                    '▼' => new PlayerState(new(x, j)),
                    _   => null,
                };


                if (ocuppier is PlayerState p ) player = p;
                map[new(x, j)] = new(ocuppier);

                Debug.Log($"base tile is {ocuppier} read tile {basetile}");

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
