using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderTracker : MonoBehaviour
{
    [SerializeField] private List<OrderView> OrderSlots;
    private Dictionary<int, OrderView> _orderViewTracker = new();

    public OrderView GetOrderView(int index)
    {
        if(index >= OrderSlots.Count || index < 0) return null;

        return OrderSlots[index];
    }

    void Awake()
    {
        transform.position = Camera.main.ViewportToWorldPoint(new(.1f, .9f, 10));
    }

    public void TrackView(int id, OrderView view)
    {
        _orderViewTracker[id] = view;
    }

    public void ReleaseView(int id)
    {
        _orderViewTracker.Remove(id);
    }

    public OrderView GetView(int id) => _orderViewTracker[id];
    public List<OrderView> OrdersTrack => OrderSlots;
    
}
