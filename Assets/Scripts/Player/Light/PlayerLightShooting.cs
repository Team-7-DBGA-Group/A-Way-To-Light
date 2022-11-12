using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject lightShotPrefab;
    [SerializeField]
    private PlayerAim playerAim;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Transform shotSpawnPostion;

    [Header("Shot Settings")]
    [SerializeField]
    private float cooldown = 2.0f;
    [SerializeField]
    private int rayUnity = 1000;

    private bool _canShoot = true;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && playerAim.IsAiming && _canShoot)
        {
            Shot();
        }
    }

    private void Shot()
    {
        GameObject shotObj = Instantiate(lightShotPrefab, shotSpawnPostion.position, Quaternion.identity);
        LightShot shot = shotObj.GetComponent<LightShot>();

        // Create a ray from the camera going through the middle of your screen
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        // Check if it hits something, if not get the point in "rayUnit" amount of unit on the ray
        Vector3 targetPoint = Vector3.zero;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(rayUnity);

        // Move
        shot.StartMovingToDirection((targetPoint - shotSpawnPostion.position).normalized * 10);

        // Cooldown
        StartCoroutine(StartCooldownTimer());
    }

    private IEnumerator StartCooldownTimer()
    {
        _canShoot = false;
        yield return new WaitForSeconds(cooldown);
        _canShoot = true;
    }
}
