using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ClickToStart : MonoBehaviour
{
    private bool clicked = false;

    private void OnMouseDown()
    {
        if (clicked) return;
        clicked = true;

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 1.1f;

        transform.DOScale(targetScale, 0.1f)
            .SetEase(Ease.OutBack)
            .SetLoops( 2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                SceneManager.UnloadSceneAsync("SampleScene");
                SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
            });
    }
}
