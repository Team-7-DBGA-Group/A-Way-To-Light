using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightShooting : MonoBehaviour
{
    public int Charges { get; private set; }

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
    private int maxLightCharges = 3;
    [SerializeField]
    private float chargesCooldown = 2.0f;
    [SerializeField]
    private float betweenShotCooldown = 0.5f;
    [SerializeField]
    private int rayUnity = 1000;

    private bool _canShoot = true;

    private void Awake()
    {
        Charges = maxLightCharges;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && playerAim.IsAiming && Charges > 0 && _canShoot)
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

        // Charge Cooldown
        StartCoroutine(StartChargesCooldownTimer());

        // Between Shot Cooldown
        StartCoroutine(StartBetweenShotCooldown());
    }

    private IEnumerator StartChargesCooldownTimer()
    {
        Charges--;
        yield return new WaitForSeconds(chargesCooldown);
        Charges++;
        if (Charges > maxLightCharges)
            Charges = maxLightCharges;
    }

    private IEnumerator StartBetweenShotCooldown()
    {
        _canShoot = false;
        yield return new WaitForSeconds(betweenShotCooldown);
        _canShoot = true;
    }
}
