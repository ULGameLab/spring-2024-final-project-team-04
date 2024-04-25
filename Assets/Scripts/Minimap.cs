using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public RectTransform miniMap;
    private Vector2 middlePosition = new Vector2(-400, -175);
    private Vector2 clickedScale = new Vector2(1.8f, 1.8f);
    private Vector2 hidePosition;
    public float transitionDuration = 0.5f;
    private Vector2 originalPosition;

    private bool isClicked = false;
    private bool hide = false;

    private void Start()
    {
        originalPosition = miniMap.anchoredPosition;
        hidePosition = new Vector2(originalPosition.x, -originalPosition.y);
    }

    public void ToggleMiniMap()
    {
        if (hide)
        {
            hide = false;
            isClicked = false;
        }
        Vector2 targetPosition = isClicked ? middlePosition : originalPosition;
        LeanTween.move(miniMap, targetPosition, transitionDuration).setEase(LeanTweenType.easeInOutQuad);

        Vector2 targetScale = isClicked ? clickedScale : Vector2.one;
        LeanTween.scale(miniMap, targetScale, transitionDuration).setEase(LeanTweenType.easeInOutQuad);
    }

    private void Update()
    {
        // Boss fight disable minimap
        if (FindFirstObjectByType<SpiderBoss>() || FindFirstObjectByType<OrcBoss>() || FindFirstObjectByType<CrabBoss>())
        {
            miniMap.gameObject.SetActive(false);
        }
        else
        {
            miniMap.gameObject.SetActive(true);
        }

        // Toggle minimap
        if (Input.GetKeyDown(KeyCode.M))
        {
            isClicked = !isClicked;
            ToggleMiniMap();
        }
        // Hide
        if (Input.GetKeyDown(KeyCode.H))
        {
            hide = !hide;
            if (hide)
            {
                LeanTween.move(miniMap, hidePosition, transitionDuration).setEase(LeanTweenType.easeInOutQuad);
                LeanTween.scale(miniMap, Vector2.one, transitionDuration).setEase(LeanTweenType.easeInOutQuad);
            }
            else
            {
                LeanTween.move(miniMap, originalPosition, transitionDuration).setEase(LeanTweenType.easeInOutQuad);
                LeanTween.scale(miniMap, Vector2.one, transitionDuration).setEase(LeanTweenType.easeInOutQuad);
            }
        }
    }
}

