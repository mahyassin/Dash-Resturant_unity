using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameState
{
    public Dictionary<Vector2Int, CellState> Map;
    public int MapHieght {get;}
    public int MapWidth {get;}
    public PlayerState PlayerState;
    public List<IInteractable> interactables;
    public List<IOrderMaker> OrderMakers;
    public List<int> DishsCD = new();
    public OrdersState ordersState = new();

    public GameState(
        Dictionary<Vector2Int, CellState> map, 
        PlayerState playerState, 
        int mapWidth, 
        int mapHieght,
        List<IInteractable> stations,
        List<IOrderMaker> orderMakers
    ){
        Map = map;
        PlayerState = playerState;
        MapHieght = mapHieght;
        MapWidth = mapWidth;
        interactables = stations;
        Debug.Log(interactables.Count);
        OrderMakers = orderMakers;
    }

}

public class OrdersState
{
    private List<Order> _pendingOrders = new();
    private List<Order> _completedOrders = new();
    private List<Order> _failedOrders = new();

    public void AddOrder(Order order)
    {
        var list = order.GetState switch
        {
            Order.State.Fial    => _failedOrders,
            Order.State.Success => _completedOrders,
            _                   => _pendingOrders,
        };

        list.Add(order);
    }

    public void CompleteOrder(Order order)
    {
        _pendingOrders.Remove(order);
        _completedOrders.Add(order);
        Debug.Log("completed");
    }

    public void FailOrder(Order order)
    {
        _pendingOrders.Remove(order);
        _failedOrders.Add(order);
    }

    public List<Order> PendingOrders   => _pendingOrders;
    public List<Order> CompletedOrders => _completedOrders;
    public List<Order> FialdOrders     => _failedOrders;
}



