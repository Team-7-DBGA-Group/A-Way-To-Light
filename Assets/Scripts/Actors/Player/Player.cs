using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;
using System;
using Unity.VisualScripting;

public class Player : Actor
{
    public static event Action<GameObject> OnPlayerDieTarget;
    public static event Action OnPlayerDie;
    public static event Action<Weapon> OnWeaponEquip;
    public static event Action OnWeaponUnequip;
    public bool IsWeaponEquip { get => _currentEquipWeapon != null; }

    [Header("Player Settings")]
    [SerializeField]
    private float dropRayDistance = 1.6f;
    [SerializeField]
    private float healthRegenCooldown = 2.0f;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private AudioClip takeDamageSound;

    [Header("Musics")]
    [SerializeField]
    private AudioClip gameOverMusic;
    [SerializeField]
    private AudioClip onCombatEnterMusic;
    [SerializeField]
    private AudioClip levelMusic;
    [SerializeField]
    private AudioClip bossMusic;

    private Weapon _currentEquipWeapon = null;

    private Vector3[] _directions = new Vector3[4];
    
    private bool _canHeal = true;
    private Coroutine _healthRegenCoroutine = null;
    private bool _inBossFight = false;

    protected override void Start()
    {
        base.Start();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    public override void CustomDamageInteract()
    {
        base.CustomDamageInteract();
        AudioManager.Instance.PlaySound(takeDamageSound);
    }

    public void Equip(Weapon w)
    {
        _currentEquipWeapon = w;
        OnWeaponEquip?.Invoke(w);
    }

    public void Unequip()
    {
        if (!IsWeaponEquip)
            return;

        RaycastHit hit;
        foreach (Vector3 direction in _directions)
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0), direction, out hit, dropRayDistance))
            { 
                continue;
            }
            if (Physics.Raycast(transform.position + (-gameObject.transform.forward * dropRayDistance) + new Vector3(0, 1f, 0), Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                GameObject pickableObj = Instantiate(_currentEquipWeapon.PickablePrefab, hit.point, Quaternion.identity);
                pickableObj.transform.forward = hit.collider.gameObject.transform.up;
                pickableObj.transform.position += new Vector3(0, 0.07f, 0);
                pickableObj.GetComponent<PickableWeapon>().PassData(_currentEquipWeapon);
                break;
            }
        }

        Destroy(_currentEquipWeapon.gameObject);
        _currentEquipWeapon = null;
        OnWeaponUnequip?.Invoke();
    }

    public void DealDamage()
    {
        if (_currentEquipWeapon == null)
            return;
        WeaponDamageDealer wDamageDealer = _currentEquipWeapon.GetComponentInChildren<WeaponDamageDealer>();
        if (wDamageDealer)
        {
            wDamageDealer.StartDealDamage();
        }
    }

    public void StopDealingDamage()
    {
        if (_currentEquipWeapon == null)
            return;
        WeaponDamageDealer wDamageDealer = _currentEquipWeapon.GetComponentInChildren<WeaponDamageDealer>();
        if (wDamageDealer)
        {
            wDamageDealer.EndDealDamage();
        }
    }

    public override void Die()
    {
        CustomLog.Log(CustomLog.CustomLogType.GAMEPLAY, "Game Over!");

        GameObject target = GetComponentInChildren<PlayerCameraTarget>().gameObject;
        target.transform.parent = null;

        OnPlayerDieTarget?.Invoke(target);
        AudioManager.Instance.PlaySound(deathSound);
        OnPlayerDie?.Invoke();
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic(gameOverMusic);
        Destroy(gameObject);
    }

    private void Update()
    {
        UpdateRaysDirections();

        foreach (Vector3 direction in _directions)
        {
            Debug.DrawRay(transform.position + (direction * dropRayDistance) + new Vector3(0, 1f, 0), Vector3.down * 10.0f, Color.red);
            Debug.DrawRay(transform.position + new Vector3(0, 1f, 0), direction * dropRayDistance, Color.cyan);
        }
        
    }

    private void OnEnable()
    {
        
        EnemyManager.OnCombatEnter += StopHealthRegenOutOfCombat;
        EnemyManager.OnCombatEnter += PlayCombatMusic;
        EnemyManager.OnCombatExit += StartHealthRegenOutOfCombat;
        EnemyManager.OnCombatExit += ResumeLevelMusic;
        EnemyManager.OnBossCombatEnter += StopHealthRegenOutOfCombat;
        EnemyManager.OnBossCombatEnter += PlayBossMusic;
        EnemyManager.OnBossCombatExit += StartHealthRegenOutOfCombat;
        GameManager.OnGameReset += ResumeLevelMusic;
    }

    private void OnDisable()
    {
        EnemyManager.OnCombatEnter -= StopHealthRegenOutOfCombat;
        EnemyManager.OnCombatEnter -= PlayCombatMusic;
        EnemyManager.OnCombatExit -= StartHealthRegenOutOfCombat;
        EnemyManager.OnCombatExit += ResumeLevelMusic;
        EnemyManager.OnBossCombatEnter -= StopHealthRegenOutOfCombat;
        EnemyManager.OnBossCombatEnter -= PlayBossMusic;
        EnemyManager.OnBossCombatExit -= StartHealthRegenOutOfCombat;
        GameManager.OnGameReset -= ResumeLevelMusic;
    }

    private void UpdateRaysDirections()
    {
        _directions[0] = -transform.forward;
        _directions[1] = -transform.right;
        _directions[2] = transform.right;
        _directions[3] = transform.forward;
    }

    private void StartHealthRegenOutOfCombat()
    {
        if (CurrentHealth >= MaxHealth)
            return;

        if(_healthRegenCoroutine != null)
        {
            StopCoroutine(_healthRegenCoroutine);
        }

        if(!_inBossFight)
            _healthRegenCoroutine = StartCoroutine(COWaitForHealthRegenCD());
    }

    private void StopHealthRegenOutOfCombat()
    {
        if (_healthRegenCoroutine == null)
            return;
        StopCoroutine(_healthRegenCoroutine);
    }

    private IEnumerator COWaitForHealthRegenCD()
    {
        while(CurrentHealth < MaxHealth)
        {
            yield return new WaitForSeconds(healthRegenCooldown);
            Heal(1);
        }
    }

    private void PlayCombatMusic()
    {
        if (_inBossFight)
            return;
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic(onCombatEnterMusic, true);
    }
    private void PlayBossMusic()
    {
        _inBossFight = true;
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic(bossMusic, true);
    }

    private void ResumeLevelMusic()
    {
        _inBossFight = false;
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic(levelMusic, true);
    }
}
