using System.Collections.Generic;
using UnityEngine;

public class TargetFacade : MonoBehaviour
{
    private static TargetFacade instance;
    public static TargetFacade Instance => instance;

    [SerializeField]
    private TargetPool[] targetPools;

    private Dictionary<Target, TargetPool> originPool =
        new Dictionary<Target, TargetPool>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public Target GetTarget()
    {
        if (targetPools == null || targetPools.Length == 0)
        {
            Debug.LogWarning("TargetFacade: no hay pools configurados.");
            return null;
        }

        int index = Random.Range(0, targetPools.Length);
        Target target = targetPools[index].Get();
        originPool[target] = targetPools[index];
        return target;
    }

    public void ReturnTarget(Target target)
    {
        if (originPool.TryGetValue(target, out TargetPool pool))
        {
            originPool.Remove(target);
            pool.Return(target);
        }
        else
        {
            Debug.LogWarning("TargetFacade: pool de origen desconocido para " + target.name);
            targetPools[0].Return(target);
        }
    }
}
