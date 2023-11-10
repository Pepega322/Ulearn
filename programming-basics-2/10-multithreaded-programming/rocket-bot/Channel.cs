using System.Collections.Generic;

namespace rocket_bot;

public class Channel<T> where T : class {
    private readonly List<T> items = new();

    /// <summary>
    /// Возвращает элемент по индексу или null, если такого элемента нет.
    /// При присвоении удаляет все элементы после.
    /// Если индекс в точности равен размеру коллекции, работает как Append.
    /// </summary>
    public T this[int index] {
        get {
            lock (items) {
                if (index < 0 || index >= items.Count) return null;
                return items[index];
            }
        }
        set {
            lock (items) {
                if (index == items.Count) items.Add(value);
                else items[index] = value;
                items.RemoveRange(index + 1, items.Count - index - 1);
            }
        }
    }

    /// <summary>
    /// Возвращает последний элемент или null, если такого элемента нет
    /// </summary>
    public T LastItem() {
        lock (items) {
            if (items.Count == 0) return null;
            return items[items.Count - 1];
        }
    }

    /// <summary>
    /// Добавляет item в конец только если lastItem является последним элементом
    /// </summary>
    public void AppendIfLastItemIsUnchanged(T item, T knownLastItem) {
        lock (items)
            if (LastItem() == knownLastItem) items.Add(item);
    }

    /// <summary>
    /// Возвращает количество элементов в коллекции
    /// </summary>
    public int Count {
        get {
            lock (items)
                return items.Count;
        }
    }
}