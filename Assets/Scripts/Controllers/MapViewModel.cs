using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapViewModel
{
     public string[] DecodeState(GameState state)
    {
        string[] output = new string[state.MapHieght];
        for(int j = 0; j < state.MapHieght; j++)
        {
            for(int i = 0; i < state.MapWidth; i++)
            {
                var occupier = state.Map[new(i, j)].Ocuppier;

                string baseCell;
                string ontile;
                
                if (occupier is PlayerState player)
                {
                    baseCell = "▼";
                    ontile = DecodeCarriable(player.OnCarrier);

                }
                else if (occupier is Wall)
                {
                    baseCell = "W";
                    ontile = "W ";
                }
                else if(occupier is Generator gen) {
                    baseCell = "G";
                    ontile   = DecodeCarriable(gen.OnCarrier);
                }
                else if(occupier is Stove s)
                {
                    string color = s.IsOn? "green": "red";

                    baseCell = $"<color={color}>S</color>";

                    ontile   = DecodeCarriable(s.OnCarrier);
                } else if(occupier is CuttingBoard c)
                {
                    baseCell = "C";
                    ontile   = DecodeCarriable(c.OnCarrier);
                }
                else 
                {
                    baseCell = ".";
                    ontile = ". ";
                }

                output[j] +=  $"{baseCell}{ontile} ";

            }
        }

        return output;
    }

    public string DecodeCarriable(ICarriable carriable)
    {
        return carriable switch
        {
            Ingredient i =>  i.Type switch
            {
                IngredientType.TOMATO => "t ",
                IngredientType.ONION  => "o ",
                IngredientType.POTATO => "p ",
                _                     => ". ",
            },
            Pot => "P ",
            _   => ". ",
        };
    }

    public int[] DecodeStoves(GameState state)
    {
        var output = new List<int>();
        int id = 0;
        foreach(var station in state.interactables)
        {
            id++;
            var pair = ("stove " + id, 0);

            if(station is not Stove stove) continue;
            if(stove.OnCarrier is not Pot) continue;

            var total = stove.GetCookingProgress();
            pair.Item2 = total;

            output.Add(total);

        
        }

        return output.ToArray();
    }

    public int[] DecodeCuttingBoards(GameState state)
    {
        var output = new List<int>();
        foreach(var station in state.interactables)
        {
            if(station is not CuttingBoard board) continue; 
            if(board.OnCarrier is not Ingredient ingredient) continue;
            
            output.Add(ingredient.CuttingProgress);
        }

        return output.ToArray();
    }
}