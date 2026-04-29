using System.Collections.Generic;
using UnityEngine;

public class LevelDesginer
{
    string[] _map =
    {
        "F. F. F. F. F. F. F. F. F. F. F. F. F. F. F. F.",
        "F. .. .. .. .. .. .. .. .. .. .. .. .. .. .. F.",
        "F. .. .. .. .. .. .. .. .. .. .. .. .. .. .. F.",
        "F. .. .. .. .. .. .. .. .. .. .. .. .. .. .. F.",
        "F. .. .. .. .. .. .. .. .. .. .. .. .. .. .. F.",
        "F. .. .. .. .. .. .. .. .. .. .. .. .. .. .. F.",
        "F. F. F. F. F. .. F. F. F. SP F. F. .. F. F. F.",
        "F. .. .. .. .. .. .. .. .. ▼. SP .. .. .. .. F.",
        "F. .. .. .. .. .. .. .. .. .. C. .. .. .. .. F.",
        "F. .. .. .. .. .. .. .. .. .. F. .. .. .. .. F.",
        "F. .. .. .. .. .. .. .. .. .. F. .. .. .. .. F.",
        "F. .. .. .. .. .. .. .. .. .. F. .. .. .. .. F.",
        "F. .. .. .. .. .. .. .. .. .. F. .. .. .. .. F.",
        "F. .. .. .. .. .. .. .. .. .. F. .. .. .. .. F.",
        "F. F. F. F. F. F. F. F. F. F. F. F. F. F. F. F.",
      
    };


    public GameState GetState(EntitiesFactory factory, Identfier identfier)
    {
        return BuildGame(_map, factory, identfier);
    }

    public GameState BuildGame(string[] mapCode, EntitiesFactory factory, Identfier identfier)
    {
        int mapHieght = mapCode.Length;
        int mapWidth  = mapCode[0].Replace(" ","").Length/ 2;

        Dictionary<Vector2Int, CellState> map = new();
        CharacterState player = null;
        List<IInteractable> stations = new();
        List<IOrderMaker> orderMakers = new();
        int y = 0;
        foreach(var line in mapCode)
        {

            int j = mapHieght - y -1;
            char basetile = '?';
            char ontile = '?';

            int x = 0;
            foreach(var symbol in _map[y].Replace(" ", ""))
            {
                Vector2Int pos = new(x - mapWidth / 2, j - mapHieght / 2);

                if(basetile == '?') {basetile = symbol; continue;}
                if(ontile == '?') {ontile = symbol;}

                ICarriable oncell = ontile switch
                {
                    'o' => factory.CreateIngredient(IngredientType.ONION),
                    'p' => factory.CreateIngredient(IngredientType.POTATO),
                    't' => factory.CreateIngredient(IngredientType.TOMATO),
                    'P' => factory.CreatePot(),
                    'd' => factory.CreateDish(),
                    _   => null,
                }; 


                IOcuppier ocuppier = basetile switch
                {
                    'W' => new Wall(),
                    '▼' => factory.CreateCharachter(pos),
                    'G' => factory.CreateGenerator(10, oncell as Ingredient),
                    'S' => factory.CreateAStove(oncell as Pot),
                    'C' => factory.CreatecuttingBoard(),
                    'F' => factory.CreateShelf(oncell),
                    'O' => factory.CreateOrderTable(),
                    'T' => factory.CreateTrashCan(),
                    _   => null,
                };


                if (ocuppier is CharacterState p ) player = p;
                map[pos] = new(ocuppier, pos);

                if (ocuppier is IInteractable interactable) stations.Add(interactable);
                if (ocuppier is IOrderMaker orderMaker) orderMakers.Add(orderMaker);

                basetile = '?';
                ontile = '?';

                x++;

            }
            y++;
        }
        
        return new(map, player, mapWidth, mapHieght, stations, orderMakers);
    }
}