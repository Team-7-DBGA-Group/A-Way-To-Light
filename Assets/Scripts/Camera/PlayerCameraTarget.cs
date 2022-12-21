using UnityEngine;

public class PlayerCameraTarget : MonoBehaviour
{
    private int _destroyCounter = 0;

    private void OnEnable()
    {
        SpawnManager.OnPlayerSpawn += CheckDestroy;
    }

    private void OnDisable()
    {
        SpawnManager.OnPlayerSpawn -= CheckDestroy;
    }

    private void CheckDestroy(GameObject playerObj)
    {
        _destroyCounter++;
        if (_destroyCounter >= 2)
            Destroy(this.gameObject);
    }
}
