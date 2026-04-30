using UnityEngine;
using System.Collections;

public class ShootCommand : MonoBehaviour, ICommand
{
    [Header("Bullet")]
    [SerializeField] private float bulletSpeed = 3f;
    [SerializeField] private float fireRate = 1f; // tiempo de refresco entre disparos

    private Transform BulletSpawnPoint => Player.Instance.BulletSpawnPoint;
    private bool canShoot = true;

    // --- INTERFAZ ---
    private interface IShooting
    {
        void Shoot();
    }

    // --- DISPARO BÁSICO ---
    private class BasicShooting : IShooting
    {
        private Transform spawnPoint;
        private float speed;

        public BasicShooting(Transform spawnPoint, float speed)
        {
            this.spawnPoint = spawnPoint;
            this.speed = speed;
        }

        public void Shoot()
        {
            Bullet bullet = Pool.Instance.GetBullet();
            bullet.transform.position = spawnPoint.position;
            bullet.transform.rotation = spawnPoint.rotation;
            bullet.Rigidbody.AddForce(spawnPoint.up * speed, ForceMode.Impulse);
        }
    }

    // --- DECORADOR TRIPLE ---
    private class TripleShotDecorator : IShooting
    {
        private IShooting baseShooting;
        private MonoBehaviour runner;
        private float fireRate;

        public TripleShotDecorator(IShooting baseShooting, MonoBehaviour runner, float fireRate)
        {
            this.baseShooting = baseShooting;
            this.runner = runner;
            this.fireRate = fireRate;
        }

        public void Shoot()
        {
            runner.StartCoroutine(TripleShotCoroutine());
        }

        private IEnumerator TripleShotCoroutine()
        {
            // dispara 3 balas secuenciales
            for (int i = 0; i < 3; i++)
            {
                baseShooting.Shoot();
                yield return new WaitForSeconds(0.2f);
            }

            // espera el tiempo de refresco antes de permitir otro disparo
            yield return new WaitForSeconds(fireRate);
        }
    }

    // --- CAMPOS PARA CONTROLAR EL DECORATOR ---
    private IShooting currentShooting;

    void Start()
    {
        // desde el inicio, el juego carga el decorador triple
        IShooting basic = new BasicShooting(BulletSpawnPoint, bulletSpeed);
        currentShooting = new TripleShotDecorator(basic, this, fireRate);
    }

    public void Execute()
    {
        if (canShoot)
        {
            canShoot = false;
            currentShooting.Shoot();
            StartCoroutine(ResetShootFlag());
        }
    }

    private IEnumerator ResetShootFlag()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}

