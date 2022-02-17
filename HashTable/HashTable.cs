using System;
using System.Collections.Generic;

/// <summary>
/// Реализация хэш-таблицы.
/// </summary>
/// <typeparam name="K">Тип ключей.</typeparam>
/// <typeparam name="V">Тип значений.</typeparam>
public class HashTable<K, V>
{
    /// <summary>
    /// Узел, содержащий данные.
    /// </summary>
    /// <typeparam name="K">Тип ключа.</typeparam>
    /// <typeparam name="V">Тип значения.</typeparam>
    private class HashTableNode<K, V>
    {
        /// <summary>
        /// Ключ.
        /// </summary>
        public K Key;

        /// <summary>
        /// Значение.
        /// </summary>
        public V Value;

        /// <summary>
        /// Следующий узел.
        /// </summary>
        public HashTableNode<K, V> NextNode;
    }

    /// <summary>
    /// Количество элементов в хэш-таблице.
    /// </summary>
    public int Size { private set; get; }

    /// <summary>
    /// Количество цепей узлов.
    /// </summary>
    private int numBuckets;

    /// <summary>
    /// Массив узлов, содержащих данные.
    /// </summary>
    private HashTableNode<K, V>[] buckets;

    /// <summary>
    /// Инициализирует хэш-таблицу.
    /// </summary>
    public HashTable()
    {
        Size = 0;
        numBuckets = 16;
        buckets = new HashTableNode<K, V>[numBuckets];
    }

    /// <summary>
    /// Высчитывает индекс ключа в массиве узлов.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Индекс ключа.</returns>
    private int GetKeyIndex(K key)
    {
        int hash = key.GetHashCode() % numBuckets;
        return hash >= 0 ? hash : -hash;
    }

    /// <summary>
    /// Добавляет новый элемент в хэш-таблицу или перезаписывает его, если ключ уже записан.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    public void Add(K key, V value)
    {
        int bucketIndex = GetKeyIndex(key);
        int keyHash = key.GetHashCode();

        HashTableNode<K, V> currentNode = buckets[bucketIndex];

        while (currentNode != null)
        {
            if (currentNode.Key.GetHashCode() == keyHash && currentNode.Key.Equals(key))
            {
                currentNode.Value = value;
                break;
            }

            currentNode = currentNode.NextNode;
        }

        if (currentNode != null)
            return;

        HashTableNode<K, V> newHead = new();
        newHead.Key = key;
        newHead.Value = value;
        newHead.NextNode = buckets[bucketIndex];

        buckets[bucketIndex] = newHead;

        ++Size;
    }

    /// <summary>
    /// Возвращает элемент по ключу.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Значение под ключом.</returns>
    public V Get(K key)
    {
        int bucketIndex = GetKeyIndex(key);
        int keyHash = key.GetHashCode();

        HashTableNode<K, V> currentNode = buckets[bucketIndex];

        while (currentNode != null)
        {
            if (currentNode.Key.GetHashCode() == keyHash && currentNode.Key.Equals(key))
                return currentNode.Value;

            currentNode = currentNode.NextNode;
        }

        throw new ArgumentOutOfRangeException("Ключ не принадлежит хэш-таблице.");
    }

    /// <summary>
    /// Возвращает, существует ли элемент по ключу.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>`true` если существует, иначе `false`.</returns>
    public bool Exists(K key)
    {
        int bucketIndex = GetKeyIndex(key);
        int keyHash = key.GetHashCode();

        HashTableNode<K, V> currentNode = buckets[bucketIndex];

        while (currentNode != null)
        {
            if (currentNode.Key.GetHashCode() == keyHash && currentNode.Key.Equals(key))
                return true;

            currentNode = currentNode.NextNode;
        }

        return false;
    }

    /// <summary>
    /// Удаляет элемент по ключу и возвращает его значение.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Значение по ключу.</returns>
    public V Remove(K key)
    {
        int bucketIndex = GetKeyIndex(key);
        int keyHash = key.GetHashCode();

        HashTableNode<K, V> previousNode = null;
        HashTableNode<K, V> currentNode = buckets[bucketIndex];

        while (currentNode != null)
        {
            if (currentNode.Key.GetHashCode() == keyHash && currentNode.Key.Equals(key))
                break;

            previousNode = currentNode;
            currentNode = currentNode.NextNode;
        }

        if (currentNode == null)
            throw new ArgumentOutOfRangeException("Ключ не принадлежит хэш-таблице.");

        if (previousNode == null)
            buckets[bucketIndex] = buckets[bucketIndex].NextNode;
        else
            previousNode.NextNode = currentNode.NextNode;

        --Size;

        return currentNode.Value;
    }

    /// <summary>
    /// Индексатор хэш-таблицы.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Значение по ключу.</returns>
    public V this[K key]
    {
        get => Get(key);
        set => Add(key, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator<(K Key, V Value)> GetEnumerator()
    {
        for (int i = 0; i < numBuckets; ++i)
        {
            HashTableNode<K, V> node = buckets[i];

            while (node != null)
            {
                yield return (node.Key, node.Value);
                node = node.NextNode;
            }
        }
    }
}