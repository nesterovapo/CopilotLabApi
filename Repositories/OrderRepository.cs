namespace CopilotLabApi.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using CopilotLabApi.Models;
public class OrderRepository
{
    private readonly List<Order> _orders = new();
    private int _nextId = 0;
    private readonly object _lock = new();

    public IEnumerable<Order> GetAll()
    {
        lock (_lock)
        {
            return _orders.Select(o => o).ToArray();
        }
    }

    public Order? GetById(int id)
    {
        lock (_lock)
        {
            return _orders.FirstOrDefault(o => o.Id == id);
        }
    }

    public Order Create(OrderCreateDto dto)
    {
        var id = System.Threading.Interlocked.Increment(ref _nextId);
        var order = new Order(id, dto.UserId, dto.ProductName!.Trim(), dto.Quantity, dto.TotalPrice, DateTime.UtcNow);
        lock (_lock)
        {
            _orders.Add(order);
        }
        return order;
    }
}
