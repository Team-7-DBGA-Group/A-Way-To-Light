using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject heartPrefab;

    private bool _isInit = false;

    private Stack<UIHeart> _activeHearts = new Stack<UIHeart>();
    private Stack<UIHeart> _lostHearts = new Stack<UIHeart>();

    public void InitializePanel(int amount)
    {
        if (_isInit)
            return;

        for (int i = 0; i < amount; i++)
        {
            GameObject heartObj = Instantiate(heartPrefab, this.transform);
            _activeHearts.Push(heartObj.GetComponent<UIHeart>());
        }

        _isInit = true;
    }

    public void EmptyHeart()
    {
        if (!_isInit)
            return;
        UIHeart heart = _activeHearts.Pop();
        heart.Empty();
        _lostHearts.Push(heart);
    }

    public void FillHeart()
    {
        if (!_isInit)
            return;

        UIHeart heart = _lostHearts.Pop();
        heart.Fill();
        _activeHearts.Push(heart);
    }
}
