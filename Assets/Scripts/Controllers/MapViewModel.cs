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
                    baseCell = "S";
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

    public Dictionary<string, int> DecodeStations(GameState state)
    {
        var output = new Dictionary<string, int>();
        int id = 0;
        foreach(var station in state.interactables)
        {
            id++;
            var pair = ("stove " + id, 0);
            if(station is Stove stove)
            {
                if(stove.OnCarrier is not Pot pot) continue;
                int total = pot.Carriables.Sum(it => (it as Ingredient).CookingProgress);
                pair.Item2 = total;

                output.Add(pair.Item1, pair.Item2);
            }

            if(station is CuttingBoard cb)
            {
                if(cb.OnCarrier is not Ingredient ingredient) continue;
                output["cutting Board" + id] = ingredient.CuttingProgress;
            }
        }

        return output;
    }
}