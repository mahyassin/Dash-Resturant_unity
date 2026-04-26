using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapViewModel
{
    public string ContainersUiState = "";
    public string OrdersUiState = "";

    public void UpdateOrderState(OrdersState state)
    {
        OrdersUiState =  
        PrintOrders("Completed: ", state.CompletedOrders) + 
        PrintOrders("Pending: ", state.PendingOrders) + 
        PrintOrders("Failed: ", state.FialdOrders);
    }

    private string PrintOrders(string type, List<Order> orders)
    {
        string output = type;

        foreach(var order in orders)
        {
            output += order.code + ", ";
        }
        return output + "\n";
    }
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

                baseCell = occupier switch
                {
                    PlayerState => "▼",
                    Wall        => "W",
                    Generator   => "G",
                    Stove s     => $"<color={(s.IsOn()? "green" : "red")}>S</color>",
                    CuttingBoard=> "C",
                    Shelf       => "F",
                    OrderTable  => "O",
                    TrashCan    => "T",
                    _           => ".",
                };

                if( occupier is ICarrier carrier)
                {
                    ontile = DecodeCarriable(carrier.OnCarrier);
                    if(carrier.OnCarrier is IContainer c) container = c;

                } else
                {
                    ontile = occupier is Wall? "W ": ". ";
                }
                

                if(container != null)
                {
                    ContainersUiState += $"{container.GetType()}: ";

                    foreach (var carriable in container.Content)
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

            int total = stove.GetCookingProgress();
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