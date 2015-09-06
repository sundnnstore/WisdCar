﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeta.WisdCar.Infrastructure.Cache
{
    public class LocalCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private LRUMap<TKey, ExpirableValue<TValue>> lru;
        public LocalCache(int capacity)
        {
            this.lru = new LRUMap<TKey, ExpirableValue<TValue>>(capacity);
        }

        #region ICache<TKey,TValue> Members

        public bool TrySet(TKey key, TValue value)
        {
            return TrySet(key, value, TimeSpan.MaxValue);
        }

        public bool TrySet(TKey key, TValue value, TimeSpan expire)
        {
            return lru.TryAdd(key, new ExpirableValue<TValue>(value, expire));
        }

        public bool TryGet(TKey key, out TValue value)
        {
            var got = false;
            value = default(TValue);
            ExpirableValue<TValue> val = null;
            if (lru.TryGet(key, out val))
            {
                if (!val.IsExpired)
                {
                    val.Delay();
                    got = true;
                }
                else Remove(key);
            }
            return got;
        }

        public TValue GetOrAdd(TKey key, Func<TValue> func, TimeSpan expire)
        {
            TValue value = default(TValue);
            if (!TryGet(key, out value))
            {
                TrySet(key, func(), expire);
            }
            return value;
        }

        public void Remove(TKey key)
        {
            lru.Remove(key);
        }

        #endregion

        private class ExpirableValue<TValue>
        {
            private TimeSpan expire;
            private DateTime expireTime;
            public ExpirableValue(TValue value, TimeSpan expire)
            {
                this.Value = value;
                this.expire = expire;
                Delay();
            }

            public TValue Value { get; private set; }

            public bool IsExpired
            {
                get
                {
                    return expireTime < DateTime.Now;
                }
            }

            public void Delay()
            {
                this.expireTime = expire == TimeSpan.MaxValue ? DateTime.MaxValue : DateTime.Now.Add(expire); 
            }
        }
    }

    /// <summary>
    /// Least Recently Used Map performs o(1) lookup 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class LRUMap<TKey, TValue>
    {
        private int capacity;
        private Node<TKey, TValue> head;
        private Node<TKey, TValue> tail;
        private object syncRoot = new object();
        private Dictionary<TKey, Node<TKey, TValue>> cacheMap;

        public LRUMap(int capacity)
        {
            this.capacity = capacity;
            this.cacheMap = new Dictionary<TKey, Node<TKey, TValue>>();
        }

        public int Count { get { return cacheMap.Count; } }

        public bool TryGet(TKey key, out TValue value)
        {
            lock (syncRoot)
            {
                value = default(TValue);
                var node = default(Node<TKey, TValue>);
                var result = cacheMap.TryGetValue(key, out node);
                if (result)
                {
                    value = node.Value;
                    if (Count != 1)
                        this.Update(key, node);
                }
                return result;
            }
        }

        public bool TryAdd(TKey key, TValue value)
        {
            lock (syncRoot)
            {
                var result = true;
                if (head == null)
                {
                    head = new Node<TKey, TValue>(key, value);
                    tail = head;
                    cacheMap[key] = head;
                }
                else
                {
                    if (!cacheMap.ContainsKey(key))
                    {
                        var node = new Node<TKey, TValue>(key, value);
                        tail.Next = node;
                        node.Previous = tail;
                        tail = tail.Next;
                        cacheMap[key] = node;
                        if (Count > capacity)
                        {
                            Remove(head.Key);
                        }
                    }
                    else
                        result = false;
                }

                return result;
            }
        }

        public void Remove(TKey key)
        {
            lock (syncRoot)
            {
                if (cacheMap.ContainsKey(key))
                {
                    cacheMap.Remove(key);
                    if (Count != 0)
                    {
                        head = head.Next;
                        head.Previous = null;
                    }
                    else
                        head = tail = null;
                }
            }
        }

        private void Update(TKey key, Node<TKey, TValue> node)
        {
            //swap head and tail
            if (node.Key.Equals(head.Key))
            {
                var tmp = head.Next;
                tail.Next = head;
                head.Previous = tail;
                tail = tail.Next;
                tail.Next = null;
                head = tmp;
                head.Previous = null;
            }
            else
            {
                if (!node.Key.Equals(tail.Key))
                {
                    node.Previous.Next = node.Next;
                    node.Next.Previous = node.Previous;
                    tail.Next = node;
                    node.Previous = tail;
                    tail = tail.Next;
                    tail.Next = null;
                }
            }
        }

        private class Node<TNodeKey, TNodeValue>
        {
            private TNodeKey key;
            private TNodeValue value;
            public Node(TNodeKey key, TNodeValue value)
            {
                this.key = key;
                this.value = value;
            }
            public TNodeValue Value { get { return value; } }
            public TNodeKey Key { get { return key; } }
            public Node<TKey, TNodeValue> Previous { get; set; }
            public Node<TKey, TNodeValue> Next { get; set; }
        }
    }
}
