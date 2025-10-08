namespace CopilotLabApi.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using CopilotLabApi.Models;

public class UserRepository
{
    private readonly List<User> _users = new();
    private int _nextId = 0;
    private readonly object _lock = new();

    public IEnumerable<User> GetAll()
    {
        lock (_lock)
        {
            return _users.Select(u => u).ToArray();
        }
    }

    public User? Get(int id)
    {
        lock (_lock)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }
    }

    public IEnumerable<User> SearchByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return Array.Empty<User>();
        lock (_lock)
        {
            return _users
                .Where(u => u.Name != null && u.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }
    }

    public bool ExistsByEmail(string email, int? excludeId = null)
    {
        lock (_lock)
        {
            return _users.Any(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase) && (!excludeId.HasValue || u.Id != excludeId.Value));
        }
    }

    public User Create(UserCreateDto dto)
    {
        var id = System.Threading.Interlocked.Increment(ref _nextId);
        var user = new User(id, dto.Name!.Trim(), dto.Email!.Trim(), DateTime.UtcNow);
        lock (_lock)
        {
            _users.Add(user);
        }
        return user;
    }

    public bool Update(int id, UserUpdateDto dto)
    {
        lock (_lock)
        {
            var idx = _users.FindIndex(u => u.Id == id);
            if (idx == -1) return false;
            var existing = _users[idx];
            var updated = new User(existing.Id, dto.Name!.Trim(), dto.Email!.Trim(), existing.CreatedAt);
            _users[idx] = updated;
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var idx = _users.FindIndex(u => u.Id == id);
            if (idx == -1) return false;
            _users.RemoveAt(idx);
            return true;
        }
    }
}
