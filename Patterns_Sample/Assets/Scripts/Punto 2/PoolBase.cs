using System.Collections.Generic;
using UnityEngine;

public abstract class PoolBase<Target> : MonoBehaviour
    where Target : MonoBehaviour, IFactoryProduct
{
    [SerializeField] protected Target    prefab;
    [SerializeField] protected int initialSize = 5;

    private List<Target> available = new List<Target>();

    protected virtual void Start()
    {
        for (int i = 0; i < initialSize; i++)
            available.Add(CreateNew());
    }

    public Target Get()
    {
        if (available.Count == 0)
            available.Add(CreateNew());

        Target item = available[0];
        available.RemoveAt(0);
        OnGet(item);
        return item;
    }

    public void Return(Target item)
    {
        OnReturn(item);
        available.Add(item);
    }

    private Target CreateNew()
    {
        Target item = Instantiate(prefab, transform);
        item.gameObject.SetActive(false);
        return item;
    }

    protected virtual void OnGet(Target item) { }
    protected virtual void OnReturn(Target item) { }
}
