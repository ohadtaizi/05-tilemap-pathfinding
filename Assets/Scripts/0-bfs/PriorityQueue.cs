using System;
using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<T>
{
    private SortedDictionary<double, Queue<T>> _queueDict = new SortedDictionary<double, Queue<T>>();

    public int Count
    {
        get
        {
            int count = 0;
            foreach (var pair in _queueDict)
            {
                count += pair.Value.Count;
            }
            return count;
        }
    }

    public void Enqueue(T item, double priority)
    {
        if (!_queueDict.ContainsKey(priority))
        {
            _queueDict[priority] = new Queue<T>();
        }
        _queueDict[priority].Enqueue(item);
    }

    public T Dequeue()
    {
        var pair = _queueDict.First();
        var item = pair.Value.Dequeue();
        if (pair.Value.Count == 0)
        {
            _queueDict.Remove(pair.Key);
        }
        return item;
    }

    public bool Contains(T item)
    {
        foreach (var pair in _queueDict)
        {
            if (pair.Value.Contains(item))
            {
                return true;
            }
        }
        return false;
    }
}