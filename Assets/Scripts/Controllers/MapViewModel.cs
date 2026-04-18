using System.Collections;
using System.Collections.Generic;
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
            string name = station switch
            {
                Stove        => $"Stove{id}",
                CuttingBoard => $"Cutting board{id}",
                _            => "??"
            };

        }

        return output;
    }
}