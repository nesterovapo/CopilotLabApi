namespace CopilotLabApi.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using CopilotLabApi.Models;

public class ProductRepository
{
    private readonly List<Product> _products = new();
    private int _nextId = 0;
    private readonly object _lock = new();

    public IEnumerable<Product> GetAll()
    {
        lock (_lock)
        {
            return _products.Select(p => p).ToArray();
        }
    }

    public Product? GetById(int id)
    {
        lock (_lock)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }
    }

    public Product Create(ProductCreateDto dto)
    {
        var id = System.Threading.Interlocked.Increment(ref _nextId);
        var product = new Product { Id = id, Name = dto.Name!.Trim(), Price = dto.Price };
        lock (_lock)
        {
            _products.Add(product);
        }
        return product;
    }

    public bool Update(int id, ProductUpdateDto dto)
    {
        lock (_lock)
        {
            var idx = _products.FindIndex(p => p.Id == id);
            if (idx == -1) return false;
            var updated = new Product { Id = id, Name = dto.Name!.Trim(), Price = dto.Price };
            _products[idx] = updated;
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var idx = _products.FindIndex(p => p.Id == id);
            if (idx == -1) return false;
            _products.RemoveAt(idx);
            return true;
        }
    }
}
