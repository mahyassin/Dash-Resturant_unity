using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapViewModel
{
    public string ContainersUiState = "";
    public string[] DecodeState(GameState state)
    {
        ContainersUiState = "";

        string[] output = new string[state.MapHieght];
        for(int j = 0; j < state.MapHieght; j++)
        {
            for(int i = 0; i < state.MapWidth; i++)
            {
                var occupier = state.Map[new(i, j)].Ocuppier;
                IContainer container = null;

                string baseCell;
                string ontile;
                
                if (occupier is PlayerState player)
                {
                    baseCell = "▼";
                    ontile = DecodeCarriable(player.OnCarrier);
                    if (player.OnCarrier is IContainer c) container = c;

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

                    if (s.OnCarrier is IContainer c) container = c;

                } 
                else if(occupier is CuttingBoard c)
                {
                    baseCell = "C";
                    ontile   = DecodeCarriable(c.OnCarrier);

                    if (c.OnCarrier is IContainer co) container = co;

                } 
                else if(occupier is Shelf shelf)
                {
                    baseCell = "F";
                    ontile   = DecodeCarriable(shelf.OnCarrier);

                    if (shelf.OnCarrier is IContainer co) container = co;

                }
                else 
                {
                    baseCell = ".";
                    ontile = ". ";
                }

                if(container != null)
                {
                    ContainersUiState += $"{container.GetType()}: ";

                    foreach (var carriable in container.Carriables)
                    {
                        ContainersUiState += DecodeCarriable(carriable) + ", ";
                    }

                    ContainersUiState += "| ";
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
            Pot  => "P ",
            Dish => "d ",
            _   => ". ",
        };
    }

    public (string, int)[] DecodeStoves(GameState state)
    {
        var output = new List<(string, int)>();
        int id = 0;
        foreach(var station in state.interactables)
        {
            id++;

            if(station is not Stove stove) continue;
            if(stove.OnCarrier is not Pot) continue;

            var total = stove.GetCookingProgress();
            var pair = ($"stove {id} { stove.GetCookingGrade() }", 0);

            pair.Item2 = total;

            output.Add(pair);

        
        }

        return output.ToArray();
    }

    public (string, int)[] DecodeCuttingBoards(GameState state)
    {
        var output = new List<(string, int)>();

        var id = 0;
        foreach(var station in state.interactables)
        {
            id++;
            if(station is not CuttingBoard board) continue; 
            if(board.OnCarrier is not Ingredient ingredient) continue;

            output.Add(($"stove {id} { ingredient.cuttingGrade}", ingredient.CuttingProgress));
        }

        return output.ToArray();
    }
}