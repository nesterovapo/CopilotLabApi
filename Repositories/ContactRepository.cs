namespace CopilotLabApi.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using CopilotLabApi.Models;

public class ContactRepository
{
    private readonly List<ContactMessage> _messages = new();
    private int _nextId = 0;
    private readonly object _lock = new();

    public IEnumerable<ContactMessage> GetAll()
    {
        lock (_lock)
        {
            return _messages.Select(m => m).ToArray();
        }
    }

    public ContactMessage Create(ContactCreateDto dto)
    {
        var id = System.Threading.Interlocked.Increment(ref _nextId);
        var msg = new ContactMessage(id, dto.Name!.Trim(), dto.Email!.Trim(), dto.Subject!.Trim(), dto.Message!.Trim(), DateTime.UtcNow);
        lock (_lock)
        {
            _messages.Add(msg);
        }
        return msg;
    }
}
