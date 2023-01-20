using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Boss : MonoBehaviour
{
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
    }

    private void OnDisable()
    {
        SpawnManager.OnPlayerSpawn -= InitBoss;
        barrier.OnBarrierDeactivated -= GoToStunState;
        lantern.OnLanternInteraction -= PhaseEnd;
        Player.OnPlayerDie -= ResetBoss;
    }

    private void Update()
    {
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
                enemy.Die();
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

        /*
        if (_currentPhase == Phase.Stop && _isPlayerInRange)
        {
            _currentPhase = Phase.FirstPhase;
            FSM.GoToState(_activateBarrierState);
        }
        */
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
            enemy.Spawn();

        _areEnemiesSpawned = true;
    }

    private IEnumerator COWaitForPhaseContinue()
    {
        yield return new WaitForSeconds(waitTimeLanternHit);
        ContinuePhase();
    }
}
