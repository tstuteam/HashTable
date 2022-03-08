namespace HashTableClass;

/// <summary>
///     Реализация хэш-таблицы.
/// </summary>
/// <typeparam name="K">Тип ключей.</typeparam>
/// <typeparam name="V">Тип значений.</typeparam>
public class HashTable<K, V>
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
    public int Size { private set; get; }

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
        Size = 0;
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

        ++Size;

        if (Size / numBuckets >= loadFactor)
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
    ///     Возвращает элемент по ключу.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Значение под ключом.</returns>
    public V? Get(K key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key), "Ключ не может иметь значение null.");

        if (buckets == null)
            throw new ArgumentOutOfRangeException(nameof(key), "Ключ не принадлежит хэш-таблице.");

        int keyHash = key.GetHashCode();
        int bucketIndex = GetHashIndex(keyHash);

        HashTableNode? currentNode = buckets[bucketIndex];

        while (currentNode != null)
        {
            if (currentNode.KeyHash == keyHash && currentNode.Key!.Equals(key))
                return currentNode.Value;

            currentNode = currentNode.NextNode;
        }

        throw new ArgumentOutOfRangeException(nameof(key), "Ключ не принадлежит хэш-таблице.");
    }

    /// <summary>
    ///     Возвращает, существует ли элемент по ключу.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>`true` если существует, иначе `false`.</returns>
    public bool Exists(K key)
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
    ///     Удаляет элемент по ключу и возвращает его значение.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Значение по ключу.</returns>
    public V? Remove(K key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key), "Ключ не может иметь значение null.");

        if (buckets == null)
            throw new ArgumentOutOfRangeException(nameof(key), "Ключ не принадлежит хэш-таблице.");

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
            throw new ArgumentOutOfRangeException(nameof(key), "Ключ не принадлежит хэш-таблице.");

        --Size;

        if (previousNode == null)
            buckets[bucketIndex] = buckets[bucketIndex]!.NextNode;
        else
            previousNode.NextNode = currentNode.NextNode;

        return currentNode.Value;
    }

    /// <summary>
    ///     Индексатор хэш-таблицы.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Значение по ключу.</returns>
    public V? this[K key]
    {
        get => Get(key);
        set => Add(key, value);
    }

    /// <summary>
    ///     Возвращает перечислитель хэш-таблицы.
    /// </summary>
    /// <returns>Перечислитель.</returns>
    public IEnumerator<(K Key, V? Value)> GetEnumerator()
    {
        for (int i = 0; i < numBuckets; ++i)
        {
            HashTableNode? node = buckets![i];

            while (node != null)
            {
                yield return (node.Key!, node.Value);
                node = node.NextNode;
            }
        }
    }
}
