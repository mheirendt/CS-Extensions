using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;


// The find methods are not complete, it is very easy, but repetetive to add them
public class SECollection<T> : IList<T>
{
    #region Public Properties
    public T[] Elements
    {
        get
        {
            return this.elements.ToArray();
        }

    }
    #endregion
    #region Private Properties
    private List<T> elements;
    #endregion
    #region Constructors
    /// <summary>
    /// Create a collection of elements of Type T to be stored and searchable
    /// </summary>
    /// <param name="elements"></param>
    public SECollection(IEnumerable<T> elements)
    {
        if (typeof(T).BaseType != typeof(SEBaseElement))
            throw new ArgumentException("SECollection must be instantiated from a class inherited from SEBaseElement");
        if (elements == null)
            throw new ArgumentException("SECollection must be instantiated from a collection of 1 or more elements");

        this.elements = elements.ToList();
    }
    #endregion
    #region Find By
    /// <summary>
    /// Find an element from the collection whose id matches the specified value
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public T FindById(string id)
    {
        IEnumerable<SEBaseElement> abstractedElements = this.elements as IEnumerable<SEBaseElement>;
        return abstractedElements.Where(x => x.Id == id).Select(x => (T)Activator.CreateInstance(typeof(T), x)).FirstOrDefault();
    }

    /// <summary>
    /// Find an element from the collection whose id matches the specified regex
    /// </summary>
    /// <param name="regex"></param>
    /// <returns></returns>
    public T FindById(Regex regex)
    {
        IEnumerable<SEBaseElement> abstractedElements = this.elements as IEnumerable<SEBaseElement>;
        return abstractedElements.Where(x => regex.IsMatch(x.Id)).Select(x => (T)Activator.CreateInstance(typeof(T), x)).FirstOrDefault();
    }

    /// <summary>
    /// Find an element from the collection whose title matches the specified value
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public T FindByTitle(string title)
    {
        IEnumerable<SEBaseElement> abstractedElements = this.elements as IEnumerable<SEBaseElement>;
        return abstractedElements.Where(x => x.Title == title).Select(x => (T)Activator.CreateInstance(typeof(T), x)).FirstOrDefault();
    }

    /// <summary>
    /// Find an element from the collection whose title matches the specified regex
    /// </summary>
    /// <param name="regex"></param>
    /// <returns></returns>
    public T FindByTitle(Regex regex)
    {
        IEnumerable<SEBaseElement> abstractedElements = this.Elements as IEnumerable<SEBaseElement>;
        return abstractedElements.Where(x => regex.IsMatch(x.Title)).Select(x => (T)Activator.CreateInstance(typeof(T), x)).FirstOrDefault();
    }

    /// <summary>
    /// Find an element from the collection whose value matches the specified value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public T FindByValue(string value)
    {
        IEnumerable<SEBaseElement> abstractedElements = this.elements as IEnumerable<SEBaseElement>;
        return abstractedElements.Where(x => x.GetAttribute("value") == value).Select(x => (T)Activator.CreateInstance(typeof(T), x)).FirstOrDefault();
    }

    /// <summary>
    /// Find an element from the collection whose value matches the specified regex
    /// </summary>
    /// <param name="regex"></param>
    /// <returns></returns>
    public T FindByValue(Regex regex)
    {
        IEnumerable<SEBaseElement> abstractedElements = this.Elements as IEnumerable<SEBaseElement>;
        return abstractedElements.Where(x => regex.IsMatch(x.GetAttribute("value"))).Select(x => (T)Activator.CreateInstance(typeof(T), x)).FirstOrDefault();
    }

    /// <summary>
    /// Find an element from the collection whose alt matches the specified value
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public T FindByAlt(string alt)
    {
        IEnumerable<SEBaseElement> abstractedElements = this.Elements as IEnumerable<SEBaseElement>;
        return abstractedElements.Where(x => x.GetAttribute("alt") == alt).Select(x => (T)Activator.CreateInstance(typeof(T), x)).FirstOrDefault();
    }

    /// <summary>
    /// Find an element from the collection whose alt matches the specified regex
    /// </summary>
    /// <param name="regex"></param>
    /// <returns></returns>
    public T FindByAlt(Regex regex)
    {
        IEnumerable<SEBaseElement> abstractedElements = this.Elements as IEnumerable<SEBaseElement>;
        return abstractedElements.Where(x => regex.IsMatch(x.GetAttribute("alt"))).Select(x => (T)Activator.CreateInstance(typeof(T), x)).FirstOrDefault();
    }

    /// <summary>
    /// Find an element from the collection whose text contains the specified value
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public T FindByText(string text)
    {
        IEnumerable<SEBaseElement> abstractedElements = this.Elements as IEnumerable<SEBaseElement>;
        return abstractedElements.Where(x => x.Text.Contains(text)).Select(x => (T)Activator.CreateInstance(typeof(T), x)).FirstOrDefault();
    }

    /// <summary>
    /// Find a collection of elements from the collection whose text contains the specified value
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    /// <param name="text"></param>
    /// <returns></returns>
    public SECollection<T> FindElementsByText(string text)
    {
        IEnumerable<SEBaseElement> abstractedElements = this.Elements as IEnumerable<SEBaseElement>;
        IEnumerable<T> typeElements = abstractedElements.Where(x => x.Text.Contains(text)).Select(x => (T)Activator.CreateInstance(typeof(T), x));
        if (typeElements.Count() > 0)
            return new SECollection<T>(typeElements);
        else
            return null;
    }

    /// <summary>
    /// Find an element from the collection whose text matches the specified regex
    /// </summary>
    /// <param name="regex"></param>
    /// <returns></returns>
    public T FindByText(Regex regex)
    {
        IEnumerable<SEBaseElement> abstractedElements = this.Elements as IEnumerable<SEBaseElement>;
        return abstractedElements.Where(x => regex.IsMatch(x.Text)).Select(x => (T)Activator.CreateInstance(typeof(T), x)).FirstOrDefault();
    }
    #endregion
    #region IList Members

    public void Add(T item)
    {
        this.elements.Add(item);
    }

    public void Clear()
    {
        this.elements.Clear();
    }

    public bool Contains(T item)
    {
        return this.elements.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        this.elements.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return this.elements.Remove(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return this.elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.elements.GetEnumerator();
    }

    public int Count => this.elements.Count;

    public bool IsReadOnly => true;

    public T this[int index]
    {
        get
        {
            return ((IList<T>)elements)[index];
        }

        set
        {
            ((IList<T>)elements)[index] = value;
        }
    }

    public int IndexOf(T item)
    {
        return this.elements.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        this.elements.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        this.elements.RemoveAt(index);
    }
    #endregion
}
