using System;
using System.Collections.Generic;
using CombatLab.Core.Interfaces;

namespace CombatLab.Core.Services;

public static class ServiceLocator
{
    private static Dictionary<Type, object> _services = new();

    public static void Register<T>(T service)
    {
        if (_services.ContainsKey(typeof(T)))
        {
            throw new Exception($"Duplicate type {typeof(T)}");
        }
        _services.Add(typeof(T), service);
    }

    public static T Get<T>()
    {
        return (T)_services[typeof(T)];
    }

    public static bool TryGet<T>(out T service)
    {
        if (_services.TryGetValue(typeof(T), out var obj))
        {
            service = (T)obj;
            return true;
        }
        service = default;
        return false;
    }
    
    public static void Unregister<T>()
    {
        _services.Remove(typeof(T));
        
    }
}