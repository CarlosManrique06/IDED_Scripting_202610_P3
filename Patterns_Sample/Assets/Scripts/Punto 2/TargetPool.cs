using UnityEngine;

public class TargetPool : PoolBase<Target>
{
    protected override void OnGet(Target target)
    {
        target.transform.SetParent(null);
        target.ResetTarget();
        target.gameObject.SetActive(true);
    }

    protected override void OnReturn(Target target)
    {
        target.gameObject.SetActive(false);
        target.transform.SetParent(transform);
        target.transform.localPosition = Vector3.zero;
    }
}
