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

                ICarriable oncell = ontile switch
                {
                    'o' => new Ingredient(IngredientType.ONION),
                    'p' => new Ingredient(IngredientType.POTATO),
                    't' => new Ingredient(IngredientType.TOMATO),
                    _   => null,
                }; 


                IOcuppier ocuppier = basetile switch
                {
                    'W' => new Wall(),
                    '▼' => new PlayerState(new(x, j)),
                    'G' => new Generator(10, (oncell is Ingredient i)? i.Type: IngredientType.NONE),
                    'S' => new Stove(),
                    'C' => new CuttingBoard(),
                    _   => null,
                };


                if (ocuppier is PlayerState p ) player = p;
                map[new(x, j)] = new(ocuppier);

                // Debug.Log($"base tile is {ocuppier} read tile {basetile}");

                basetile = '?';
                ontile = '?';

                x++;

            }
            y++;
        }
        
        return new(map, player, mapWidth, mapHieght);
    }
}