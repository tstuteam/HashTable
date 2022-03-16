using System.Diagnostics.CodeAnalysis;
using System.Collections;

namespace HashTableClass;

/// <summary>
///     Реализация хэш-таблицы.
/// </summary>
/// <typeparam name="K">Тип ключей.</typeparam>
/// <typeparam name="V">Тип значений.</typeparam>
public class HashTable<K, V> : IDictionary<K, V>
{
    /// <summary>
    ///     Узел, содержащий данные.
    /// </summary>
    private class HashTableNode
    {
        /// <summary>
        ///     Ключ.
        /// </summary>
        public K? Key;

        /// <summary>
        ///     Значение.
        /// </summary>
        public V? Value;

        /// <summary>
        ///     Хэш ключа.
        /// </summary>
        public int KeyHash;

        /// <summary>
        ///     Следующий узел.
        /// </summary>
        public HashTableNode? NextNode;
    }

    /// <summary>
    ///     Количество элементов в хэш-таблице.
    /// </summary>
    public int Count { private set; get; }

    /// <summary>
    ///     Количество цепей узлов.
    /// </summary>
    private int numBuckets;

    /// <summary>
    ///     Массив цепей узлов, содержащих данные.
    /// </summary>
    private HashTableNode?[]? buckets;

    /// <summary>
    ///     Коэффициент загрузки, при котором массив цепей увеличивается в два раза.
    /// </summary>
    private const float loadFactor = 0.75f;

    /// <summary>
    ///     Начальный размер массива цепей.
    /// </summary>
    private const int initialBuckets = 16;

    /// <summary>
    ///     Инициализирует хэш-таблицу.
    /// </summary>
    public HashTable()
    {
        Count = 0;
        numBuckets = 0;
    }

    /// <summary>
    ///     Высчитывает индекс хэша в массиве узлов.
    /// </summary>
    /// <param name="hash">Хэш.</param>
    /// <returns>Индекс хэша.</returns>
    private int GetHashIndex(int hash)
    {
        hash %= numBuckets;
        return hash >= 0 ? hash : -hash;
    }

    /// <summary>
    ///     Добавляет новый элемент в хэш-таблицу или перезаписывает его, если ключ уже записан.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    /// <exception cref="ArgumentNullException">Вызывается, когда ключ имеет значение `null`.</exception>
    public void Add(K key, V? value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key), "Ключ не может иметь значение null.");

        if (buckets == null)
        {
            numBuckets = initialBuckets;
            buckets = new HashTableNode[numBuckets];
        }

        int keyHash = key.GetHashCode();
        int bucketIndex = GetHashIndex(keyHash);

        HashTableNode? currentNode = buckets[bucketIndex];

        while (currentNode != null)
        {
            if (currentNode.KeyHash == keyHash && currentNode.Key!.Equals(key))
            {
                currentNode.Value = value;
                break;
            }

            currentNode = currentNode.NextNode;
        }

        if (currentNode != null)
            return;

        ++Count;

        if (Count / numBuckets >= loadFactor)
        {
            GrowHashTable();
            bucketIndex = GetHashIndex(keyHash);
        }

        HashTableNode newHead = new()
        {
            Key = key,
            Value = value,
            KeyHash = keyHash,
            NextNode = buckets[bucketIndex]
        };

        buckets[bucketIndex] = newHead;
    }

    /// <summary>
    ///     Увеличивает размер хэш-таблицы.
    /// </summary>
    private void GrowHashTable()
    {
        HashTableNode?[] oldBuckets = buckets!;

        numBuckets *= 2;
        buckets = new HashTableNode[numBuckets];

        foreach (HashTableNode? node in oldBuckets)
        {
            HashTableNode? currentNode = node;

            while (currentNode != null)
            {
                HashTableNode? nextNode = currentNode.NextNode;

                int bucketIndex = GetHashIndex(currentNode.KeyHash);

                currentNode.NextNode = buckets[bucketIndex];
                buckets[bucketIndex] = currentNode;

                currentNode = nextNode;
            }
        }
    }

    /// <summary>
    ///     Пытается получить значение по ключу.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    /// <returns>`true`, если значение было найдено в таблице.</returns>
    /// <exception cref="ArgumentNullException">Вызывается, когда ключ имеет значение `null`.</exception>
    public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key), "Ключ не может иметь значение null.");

        if (buckets == null)
        {
            value = default;
            return false;
        }

        int keyHash = key.GetHashCode();
        int bucketIndex = GetHashIndex(keyHash);

        HashTableNode? currentNode = buckets[bucketIndex];

        while (currentNode != null)
        {
            if (currentNode.KeyHash == keyHash && currentNode.Key!.Equals(key))
            {
                value = currentNode.Value;
                return true;
            }

            currentNode = currentNode.NextNode;
        }

        value = default;
        return false;
    }

    /// <summary>
    ///     Возвращает, существует ли элемент по ключу.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>`true` если существует, иначе `false`.</returns>
    /// <exception cref="ArgumentNullException">Вызывается, когда ключ имеет значение `null`.</exception>
    public bool ContainsKey(K key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key), "Ключ не может иметь значение null.");

        if (buckets == null)
            return false;

        int keyHash = key.GetHashCode();
        int bucketIndex = GetHashIndex(keyHash);

        HashTableNode? currentNode = buckets[bucketIndex];

        while (currentNode != null)
        {
            if (currentNode.KeyHash == keyHash && currentNode.Key!.Equals(key))
                return true;

            currentNode = currentNode.NextNode;
        }

        return false;
    }

    /// <summary>
    ///     Удаляет элемент по ключу.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>`true`, если элемент найден.</returns>
    /// <exception cref="ArgumentNullException">Вызывается, когда ключ имеет значение `null`.</exception>
    public bool Remove(K key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key), "Ключ не может иметь значение null.");

        if (buckets == null)
            return false;

        int keyHash = key.GetHashCode();
        int bucketIndex = GetHashIndex(keyHash);

        HashTableNode? previousNode = null;
        HashTableNode? currentNode = buckets[bucketIndex];

        while (currentNode != null)
        {
            if (currentNode.KeyHash == keyHash && currentNode.Key!.Equals(key))
                break;

            previousNode = currentNode;
            currentNode = currentNode.NextNode;
        }

        if (currentNode == null)
            return false;

        --Count;

        if (previousNode == null)
            buckets[bucketIndex] = buckets[bucketIndex]!.NextNode;
        else
            previousNode.NextNode = currentNode.NextNode;

        return true;
    }

    /// <summary>
    ///     Индексатор хэш-таблицы.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Значение по ключу.</returns>
    /// <exception cref="ArgumentNullException">Вызывается, когда ключ имеет значение `null`.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Вызывается, когда ключ не принадлежит таблице.</exception>
    public V this[K key]
    {
        get
        {
            if (!TryGetValue(key, out V? value))
                throw new ArgumentOutOfRangeException(nameof(key), "Ключ не принадлежит таблице.");

            return value;
        }

        set => Add(key, value);
    }

    /// <summary>
    ///     Возвращает перечислитель хэш-таблицы.
    /// </summary>
    /// <returns>Перечислитель.</returns>
    public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
    {
        for (int i = 0; i < numBuckets; ++i)
        {
            HashTableNode? node = buckets![i];

            while (node != null)
            {
                yield return new KeyValuePair<K, V>(node.Key!, node.Value);
                node = node.NextNode;
            }
        }
    }

    #region ICollection Members
    public ICollection<K> Keys
    {
        get
        {
            K[] keys = new K[Count];

            int i = 0;

            foreach (var (key, value) in this)
                keys[i++] = key;

            return keys;
        }
    }

    public ICollection<V> Values
    {
        get
        {
            V[] values = new V[Count];

            int i = 0;

            foreach (var (key, value) in this)
                values[i++] = value;

            return values;
        }
    }

    public void Clear()
    {
        buckets = null;
        numBuckets = 0;
    }

    public void Add(KeyValuePair<K, V> kv)
    {
        Add(kv.Key, kv.Value);
    }

    public bool Contains(KeyValuePair<K, V> kv)
    {
        if (!TryGetValue(kv.Key, out var value))
            return false;

        return kv.Value.Equals(value);
    }

    public bool Remove(KeyValuePair<K, V> kv)
    {
        return Remove(kv.Key);
    }

    public void CopyTo(KeyValuePair<K, V>[] array, int index)
    {
        foreach (var (key, value) in this)
            array[index++] = new KeyValuePair<K, V>(key, value);
    }

    public bool IsReadOnly
    {
        get => false;
    }
    #endregion

    #region IEnumerable Members
    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
    #endregion
}