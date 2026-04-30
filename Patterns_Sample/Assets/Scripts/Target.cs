using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Target : MonoBehaviour, IFactoryProduct
{
    private const float TIME_TO_DESTROY = 10F;

    [SerializeField] private int maxHP = 1;
    private int currentHP;

    [SerializeField] private int scoreAdd = 10;

    public delegate void OnTargetDestroyed(int scoreAdd);
    public static event OnTargetDestroyed onTargetDestroyed;

    public void ResetTarget()
    {
        currentHP = maxHP;
        CancelInvoke(nameof(ReturnToPool));
        Invoke(nameof(ReturnToPool), TIME_TO_DESTROY);
    }

    private void OnCollisionEnter(Collision collision)
    {
        int layer = collision.gameObject.layer;

        if (layer.Equals(Utils.BulletLayer))
        {
            Pool.Instance.ReturnBullet(collision.gameObject.GetComponent<Bullet>());
            currentHP -= 1;

            if (currentHP <= 0)
            {
                onTargetDestroyed?.Invoke(scoreAdd);
                ReturnToPool();
            }
        }
        else if (layer.Equals(Utils.PlayerLayer) ||
                 layer.Equals(Utils.KillVolumeLayer))
        {
            Player.Instance.OnPlayerHit?.Invoke();
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        CancelInvoke(nameof(ReturnToPool));
        TargetFacade.Instance.ReturnTarget(this);
    }
}