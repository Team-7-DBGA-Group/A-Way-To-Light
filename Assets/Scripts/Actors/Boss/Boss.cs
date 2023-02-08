using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using System;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public static event Action OnPhaseChanged;

    public bool CanShoot { get => _canShoot; }
    public float BossRange { get => bossRange; }

    public enum Phase
    {
        FirstPhase,
        SecondPhase,
        ThirdPhase,
        Dead,
        Stop
    }

    [Header("References")]
    [SerializeField]
    private List<BossPillar> pillars = new List<BossPillar>();
    [SerializeField]
    private List<BossEnemy> enemies = new List<BossEnemy>();
    [SerializeField]
    private BossBarrier barrier;
    [SerializeField]
    private BossLantern lantern;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject darkShotPrefab;
    [SerializeField]
    private Transform darkShotSpawnPosRight;
    [SerializeField]
    private Transform darkShotSpawnPosLeft;

    [Header("Boss Settings")]
    [SerializeField]
    private float fireRate = 2.0f;
    [SerializeField]
    private float waitTimeLanternHit = 10.0f;
    [SerializeField]
    private float bossRange = 25.0f;

    [Header("AudioSource reference")]
    [SerializeField]
    private AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private AudioClip pillarSpawnSound;
    [SerializeField]
    private AudioClip takeDamageSound;

    private FSMSystem FSM;
    private ActivateBarrierState _activateBarrierState;
    private ShootingState _singleShootingState;
    private ShootingState _doubleShootingState;
    private StunState _stunState;
    private IdleBossState _idleBossState;

    private bool _canShoot = true;
    private bool _canLookPlayer = true;
    private Phase _currentPhase = Phase.FirstPhase;
    private GameObject _playerObj = null;
    private bool _isPlayerInRange = false;
    private bool _startFightFlag = false;
    private bool _areEnemiesSpawned = false;
    private bool _stopBoss = false;
    // Activated from animation end (ActivateBarrierState end)
    public void GoToSingleShooting()
    {
        if (_currentPhase == Phase.ThirdPhase || _currentPhase == Phase.SecondPhase)
            return;

        FSM.GoToState(_singleShootingState);
    }

    // Activated from animation end (ActivateBarrierState end)
    public void GoToDoubleShooting()
    {
        if (_currentPhase == Phase.FirstPhase)
            return;

        FSM.GoToState(_doubleShootingState);
    }

    public void SpawnPillars()
    {
        AudioManager.Instance.PlaySound(pillarSpawnSound);
        foreach (BossPillar pillar in pillars)
            pillar.Spawn();
    }

    public void ActivateBarrier()
    {
        foreach (BossPillar pillar in pillars)
            pillar.ResetPillar();

        barrier.ActivateBarrier();

        if (_currentPhase == Phase.ThirdPhase)
            SpawnEnemies();
    }

    public void DeactivateBarrier() => barrier.DeactivateBarrier();

    public void Shoot()
    {
        if (!_canShoot)
            return;

        _canShoot = false;
        Instantiate(darkShotPrefab, darkShotSpawnPosRight.position, Quaternion.identity);
        StartCoroutine(COWaitForFireRate());
    }

    public void DoubleShoot()
    {
        if (!_canShoot)
            return;

        _canShoot = false;
        GameObject shot = Instantiate(darkShotPrefab, darkShotSpawnPosRight.position, Quaternion.identity);
        Instantiate(darkShotPrefab, darkShotSpawnPosLeft.position, Quaternion.identity);
        shot.gameObject.GetComponent<BossDarkShot>().SetForwardOffset(3.0f);
        StartCoroutine(COWaitForFireRate());
    }

    public void GoToNextPhase()
    {
        OnPhaseChanged?.Invoke();
        if (_currentPhase == Phase.ThirdPhase)
        {
            Die();
            return;
        }
        _currentPhase += 1;
    }

    public void Die()
    {
        // Do something for when boss dies.
        // Cinematic starts ecc.
        AudioManager.Instance.PlaySound(deathSound);
        _currentPhase = Phase.Dead;
        EnemyManager.Instance.BossExited();
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        SetupFSM();
        _currentPhase = Phase.Stop;
        _isPlayerInRange = false;
    }

    private void OnEnable()
    {
        SpawnManager.OnPlayerSpawn += InitBoss;
        barrier.OnBarrierDeactivated += GoToStunState;
        lantern.OnLanternInteraction += PhaseEnd;
        Player.OnPlayerDie += ResetBoss;
        GameManager.OnPause += HandlePause;
        AudioManager.OnChangedSoundVolume += ChangeSoundVolume;
    }

    private void OnDisable()
    {
        SpawnManager.OnPlayerSpawn -= InitBoss;
        barrier.OnBarrierDeactivated -= GoToStunState;
        lantern.OnLanternInteraction -= PhaseEnd;
        Player.OnPlayerDie -= ResetBoss;
        GameManager.OnPause -= HandlePause;
        AudioManager.OnChangedSoundVolume -= ChangeSoundVolume;
    }

    private void Update()
    {
        if (_stopBoss)
            return;

        _isPlayerInRange = CheckPlayerInRange();
        if (_isPlayerInRange)
        {
            if (!_startFightFlag)
                StartFight();

            FSM.Update();
            LookPlayer();
        }
    }

    private bool CheckPlayerInRange()
    {
        if (_playerObj == null)
            return false;

        if (Vector3.Distance(_playerObj.transform.position, this.transform.position) <= bossRange)
            return true;

        return false;
    }

    private void SetupFSM()
    {
        FSM = new FSMSystem();
        _activateBarrierState = new ActivateBarrierState(this, animator);
        _singleShootingState = new ShootingState(this, animator, false);
        _doubleShootingState = new ShootingState(this, animator, true);
        _idleBossState = new IdleBossState();
        _stunState = new StunState(this, animator);

        FSM.AddState(_activateBarrierState);
        FSM.AddState(_singleShootingState);
        FSM.AddState(_doubleShootingState);
        FSM.AddState(_stunState);
        FSM.AddState(_idleBossState);

        FSM.GoToState(_idleBossState);
    }

    private IEnumerator COWaitForFireRate()
    {
        yield return new WaitForSeconds(fireRate);
        _canShoot = true;
    }

    private void LookPlayer()
    {
        if (_playerObj == null || !_canLookPlayer)
            return;

        float rotX = transform.rotation.x;
        transform.LookAt(_playerObj.transform, Vector3.up);
        transform.rotation = new Quaternion(rotX, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    private void ResetBoss()
    {
        if(_currentPhase == Phase.ThirdPhase)
        {
            foreach (BossEnemy enemy in enemies)
            {
                enemy.Die();
                enemy.gameObject.SetActive(true);
                enemy.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                enemy.transform.position = new Vector3(enemy.transform.position.x, -0.34f, enemy.transform.position.z);
                
            }
               
            _areEnemiesSpawned = false;
        }

        _currentPhase = Phase.Stop;
        FSM.GoToState(_idleBossState);
        _startFightFlag = false;
        
        foreach(BossPillar pillar in pillars)
            pillar.ResetPillar();

        barrier.ActivateBarrier();
    }

    private void InitBoss(GameObject playerObj)
    {
        _playerObj = playerObj;
    }

    private void StartFight()
    {
        if (!_isPlayerInRange)
            return;

        EnemyManager.Instance.BossEntered();
        _startFightFlag = true;
        _currentPhase = Phase.FirstPhase;
        FSM.GoToState(_activateBarrierState);
    }

    private void GoToStunState()
    {
        _canLookPlayer = false;
        FSM.GoToState(_stunState);
        StartCoroutine(COWaitForPhaseContinue());
    }

    private void PhaseEnd()
    {
        GoToNextPhase();
        audioSource.PlayOneShot(takeDamageSound);
        animator.ResetTrigger("SingleShoot");
        animator.ResetTrigger("DoubleShoot");
        _canLookPlayer = true;
        StopAllCoroutines();

        FSM.GoToState(_activateBarrierState);
    }

    private void ContinuePhase()
    {
        animator.ResetTrigger("SingleShoot");
        animator.ResetTrigger("DoubleShoot");
        _canLookPlayer = true;
        FSM.GoToState(_activateBarrierState);
    }

    private void SpawnEnemies()
    {
        if (_areEnemiesSpawned)
            return;


        foreach(BossEnemy enemy in enemies)
        {
            enemy.Spawn();
            enemy.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        }
            

        _areEnemiesSpawned = true;
    }

    private IEnumerator COWaitForPhaseContinue()
    {
        yield return new WaitForSeconds(waitTimeLanternHit);
        ContinuePhase();
    }

    private void HandlePause(bool isPause)
    {
        if (isPause)
        {
            _stopBoss = true;
            animator.ResetTrigger("SingleShoot");
            animator.ResetTrigger("DoubleShoot");
        }
        else
        {
            _stopBoss = false;
        }
    }
    private void ChangeSoundVolume()
    {
        if (audioSource != null)
            audioSource.volume = AudioManager.Instance.GetSoundVolume();
    }
}
