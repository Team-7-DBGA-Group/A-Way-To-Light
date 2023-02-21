using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlideshow : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Image showImage;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private List<Sprite> slides = new List<Sprite>();

    /*
    [Header("Settings")]
    [SerializeField]
    private float waitTimeBetweenSlides = 1.0f;
    */

    private int _slideIndex = 0;

    public void ChangeSlide()
    {
        if ((_slideIndex+1) >= slides.Count)
            return;

        _slideIndex++;
        showImage.sprite = slides[_slideIndex];
        FadeIn();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        showImage.sprite = slides[_slideIndex];
        FadeIn();
    }

    private void OnEnable()
    {
        DialogueManager.OnDialogueContinue += FadeOut;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueContinue -= FadeOut;
    }

    private void FadeIn() => animator.SetTrigger("FadeIn");
    private void FadeOut() => animator.SetTrigger("FadeOut");
}
