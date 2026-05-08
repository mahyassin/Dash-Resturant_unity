using System.Collections.Generic;
using UnityEngine;

public class OrderTracker : MonoBehaviour
{
    [SerializeField] private List<OrderView> OrderSlots;

    public OrderView GetOrderView(int index)
    {
        if(index >= OrderSlots.Count || index < 0) return null;

        return OrderSlots[index];
    }

    void Awake()
    {
        transform.position = Camera.main.ViewportToWorldPoint(new(.1f, .9f, 10));
    }

    public List<OrderView> OrdersTrack => OrderSlots;
    
}
