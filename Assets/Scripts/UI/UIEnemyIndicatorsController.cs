using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnemyIndicatorsController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private GameObject enemyIndicatorPrefab;
    [SerializeField]
    private RectTransform indicatorsPanel;

    private Dictionary<int, GameObject> _enemyIndicators = new Dictionary<int, GameObject>();
    private Vector2 _halfContainerSize = Vector2.zero;

    private void Start()
    {
        _halfContainerSize = indicatorsPanel.rect.size / 2.0f;
    }

    private void OnEnable()
    {
        EnemyManager.OnEnemyInCombatRegistered += AddEnemyIndicator;
        EnemyManager.OnEnemyInCombatDeregistered += RemoveEnemyIndicator;
    }

    private void OnDisable()
    {
        EnemyManager.OnEnemyInCombatRegistered -= AddEnemyIndicator;
        EnemyManager.OnEnemyInCombatDeregistered -= RemoveEnemyIndicator;
    }

    private void Update()
    {
        UpdateUI();
    }

    private void AddEnemyIndicator(int hash)
    {
        GameObject enemyIndicator = Instantiate(enemyIndicatorPrefab, indicatorsPanel.transform);
        _enemyIndicators[hash] = enemyIndicator;
        enemyIndicator.SetActive(false);
    }

    private void RemoveEnemyIndicator(int hash)
    {
        if (!_enemyIndicators.ContainsKey(hash))
            return;

        _enemyIndicators[hash].SetActive(false);
        _enemyIndicators.Remove(hash);
    }

    private void UpdateUI()
    {
        foreach (int hash in _enemyIndicators.Keys)
        {
            Vector3 screenPosObj = Camera.main.WorldToScreenPoint(EnemyManager.Instance.InCombatEnemies[hash].transform.position);

            if (screenPosObj.x < 0 || screenPosObj.x > Screen.width || screenPosObj.y < 0 || screenPosObj.y > Screen.height)
            {
                // Show the current indicator
                _enemyIndicators[hash].SetActive(true);

                float angle = Mathf.Atan2(screenPosObj.y - _halfContainerSize.y, screenPosObj.x - _halfContainerSize.x);
                
                Vector3 indicatorPosition = Vector3.zero;
                indicatorPosition.x = Mathf.Cos(angle) * _halfContainerSize.x;
                indicatorPosition.y = Mathf.Sin(angle) * _halfContainerSize.y;
                indicatorPosition.z = 0.0f;
                // Flip
                if(screenPosObj.z < 0)
                    indicatorPosition *= -1.0f;

                _enemyIndicators[hash].GetComponent<RectTransform>().localPosition = indicatorPosition;

                Vector3 indicatorRotation = Vector3.zero;
                indicatorRotation = _enemyIndicators[hash].GetComponent<RectTransform>().localRotation.eulerAngles;
                indicatorRotation.z = angle * Mathf.Rad2Deg + 90;
                // Filp
                if (screenPosObj.z < 0)
                    indicatorRotation.z += 180;

                _enemyIndicators[hash].GetComponent<RectTransform>().localRotation = Quaternion.Euler(indicatorRotation);
            }
            else
            {
                // Hide current indicator
                _enemyIndicators[hash].SetActive(false);
            }
        }
    }
}
