namespace CopilotLabApi.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using CopilotLabApi.Models;

public class CategoryRepository
{
    private readonly List<Category> _categories = new();
    private int _nextId = 0;
    private readonly object _lock = new();

    public IEnumerable<Category> GetAll()
    {
        lock (_lock)
        {
            return _categories.Select(c => c).ToArray();
        }
    }

    public Category? GetById(int id)
    {
        lock (_lock)
        {
            return _categories.FirstOrDefault(c => c.Id == id);
        }
    }

    public Category Create(CategoryCreateDto dto)
    {
        var id = System.Threading.Interlocked.Increment(ref _nextId);
        var category = new Category { Id = id, Name = dto.Name.Trim(), Description = dto.Description?.Trim() ?? string.Empty };
        lock (_lock)
        {
            _categories.Add(category);
        }
        return category;
    }

    public bool Update(int id, CategoryUpdateDto dto)
    {
        lock (_lock)
        {
            var idx = _categories.FindIndex(c => c.Id == id);
            if (idx == -1) return false;
            var existing = _categories[idx];
            var updated = new Category { Id = existing.Id, Name = dto.Name?.Trim() ?? existing.Name, Description = dto.Description?.Trim() ?? existing.Description };
            _categories[idx] = updated;
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var idx = _categories.FindIndex(c => c.Id == id);
            if (idx == -1) return false;
            _categories.RemoveAt(idx);
            return true;
        }
    }
}
