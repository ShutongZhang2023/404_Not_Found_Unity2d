using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeWindow : MonoBehaviour
{
    [Header("Refs")]
    public CanvasGroup backdrop;     // ȫ����͸��
    public CanvasGroup window;       // ��������

    [Header("Anim")]
    public float fadeDuration = 0.2f;
    public float backdropTargetAlpha = 0.5f;

    bool isOpen;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void VolumeOpen()
    {
        gameObject.SetActive(true);
        isOpen = true;

        DOTween.Kill(backdrop); DOTween.Kill(window);

        // ��ʼ״̬
        backdrop.alpha = 0f;
        backdrop.interactable = true;
        backdrop.blocksRaycasts = true;   // ���ر������

        window.alpha = 0f;
        window.interactable = true;
        window.blocksRaycasts = true;

        // ����
        backdrop.DOFade(backdropTargetAlpha, fadeDuration);
        window.DOFade(1f, fadeDuration);
    }

    public void VolumeClose()
    {
        if (!isOpen) return;
        isOpen = false;

        DOTween.Kill(backdrop); DOTween.Kill(window);

        var seq = DOTween.Sequence();
        seq.Join(backdrop.DOFade(0f, fadeDuration));
        seq.Join(window.DOFade(0f, fadeDuration));
        seq.OnComplete(() =>
        {
            backdrop.interactable = false;
            backdrop.blocksRaycasts = false; // �ָ������ɵ�
            window.interactable = false;
            window.blocksRaycasts = false;
            gameObject.SetActive(false);
        });
    }

    // ��ѡ������ Esc �ر�
    void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            VolumeClose();
    }
}
