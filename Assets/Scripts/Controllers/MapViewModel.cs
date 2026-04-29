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

