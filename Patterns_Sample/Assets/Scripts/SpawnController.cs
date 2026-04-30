using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float firstSpawnDelay = 0f;

    private void Start()
    {
        if (TargetFacade.Instance != null)
        {
            InvokeRepeating(nameof(SpawnObject), firstSpawnDelay, spawnRate);

            if (Player.Instance != null)
                Player.Instance.OnPlayerDied += StopSpawning;
        }
    }

    private void SpawnObject()
    {
        Target target = TargetFacade.Instance.GetTarget();

        if (target != null)
        {
            Vector3 spawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(
                Random.Range(0F, 1F), 1F, transform.position.z));

            target.transform.position = spawnPoint;
            target.transform.rotation = Quaternion.identity;
        }
    }

    private void StopSpawning() => CancelInvoke();
}