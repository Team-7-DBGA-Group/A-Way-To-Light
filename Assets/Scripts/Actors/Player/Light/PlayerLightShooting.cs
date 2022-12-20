using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerLightShooting : MonoBehaviour
{
    public static event Action OnShot;
    public static event Action OnChargeCooldownFinished;
    public static event Action<int> OnChargesInitialized;
    public int Charges { get; private set; }

    [Header("References")]
    [SerializeField]
    private GameObject lightShotPrefab;
    [SerializeField]
    private PlayerAim playerAim;
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
    [SerializeField]
    private LayerMask rayLayer;

    private bool _canShoot = true;

    public void ResetLightCharges()
    {
        Charges = maxLightCharges;
        for(int i =0; i < maxLightCharges; i++)
        {
            OnChargeCooldownFinished?.Invoke();
        }
    }

    private void Awake()
    {
        Charges = maxLightCharges;
    }

    private void Start()
    {
        OnChargesInitialized(Charges);
    }

    private void Update()
    {
        if (playerAim.IsAiming && Charges > 0 && _canShoot && InputManager.Instance.GetFirePressed())
        {
            Shot();
        }
    }

    private void Shot()
    {
        GameObject shotObj = Instantiate(lightShotPrefab, shotSpawnPostion.position, Quaternion.identity);
        LightShot shot = shotObj.GetComponent<LightShot>();

        // Create a ray from the camera going through the middle of your screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Check if it hits something, if not get the point in "rayUnit" amount of unit on the ray
        Vector3 targetPoint = Vector3.zero;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, rayLayer))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(rayUnity);

        // Move
        shot.StartMovingToDirection((targetPoint - shotSpawnPostion.position).normalized * 10);

        // Charge Cooldown
        StartCoroutine(COStartChargesCooldownTimer());

        // Between Shot Cooldown
        StartCoroutine(COStartBetweenShotCooldown());

        OnShot?.Invoke();
    }

    private IEnumerator COStartChargesCooldownTimer()
    {
        Charges--;
        yield return new WaitForSeconds(chargesCooldown);
        Charges++;
        OnChargeCooldownFinished?.Invoke();
        if (Charges > maxLightCharges)
            Charges = maxLightCharges;
    }

    private IEnumerator COStartBetweenShotCooldown()
    {
        _canShoot = false;
        yield return new WaitForSeconds(betweenShotCooldown);
        _canShoot = true;
    }
}
