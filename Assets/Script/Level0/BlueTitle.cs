using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlueTitle : MonoBehaviour
{
    [Header("Blur Settings")]
    [SerializeField] private float blurMin = 0f;
    [SerializeField] private float blurMax = 1f;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private float stayClearTime = 1f;

    private Material blurMaterial;

    private void Start()
    {
        blurMaterial = GetComponent<SpriteRenderer>().material;
        blurMaterial.SetFloat("_BlurAmount", blurMax);
        CreateBlurLoopSequence();
    }

    private void CreateBlurLoopSequence()
    {
        Sequence blurSequence = DOTween.Sequence();

        // blur - clear
        blurSequence.Append(
            DOTween.To(() => blurMaterial.GetFloat("_BlurAmount"),
                       value => blurMaterial.SetFloat("_BlurAmount", value),
                       blurMin,
                       transitionTime)
        );

        // stay clear for 1 second
        blurSequence.AppendInterval(stayClearTime);

        // clear - blur
        blurSequence.Append(
            DOTween.To(() => blurMaterial.GetFloat("_BlurAmount"),
                       value => blurMaterial.SetFloat("_BlurAmount", value),
                       blurMax,
                       transitionTime)
        );

        // loop
        blurSequence.SetLoops(-1);
    }
}
