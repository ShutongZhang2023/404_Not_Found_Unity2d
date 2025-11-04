using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class Hammer : DragParent
{
    [Header("«√ª˜ºÏ≤‚≤Œ ˝")]
    [SerializeField] private float checkRadius = 0.3f;    // ºÏ≤‚‘≤∞Îæ∂
    [SerializeField] private float shakeThreshold = 5f;   // “°ªŒ¡È√Ù∂»
    [SerializeField] private float cooldown = 0.3f;       // ¡¨«√¿‰»¥
    [SerializeField] private LayerMask blockLayer;        // WordBlock µƒ≤„

    private Vector3 lastPos;
    private float shakeSpeed;
    private float shakeDelta;
    private float lastShakeTime;
    [SerializeField] private Transform tip;

    private CinemachineImpulseSource impulseSource;

    protected override void Start()
    {
        base.Start();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 delta = transform.position - lastPos;
            float currentSpeed = delta.magnitude / Time.deltaTime;
            shakeDelta = Mathf.Abs(currentSpeed - shakeSpeed);
            shakeSpeed = currentSpeed;
            lastPos = transform.position;

            if (shakeDelta > shakeThreshold && Time.time - lastShakeTime > cooldown)
            {
                TryHitBlock();
                lastShakeTime = Time.time;
            }
        }
    }

    private void TryHitBlock()
    {
        Collider2D hit = Physics2D.OverlapCircle(tip.position, checkRadius, blockLayer);
        if (hit != null && hit.CompareTag("WordBlock"))
        {
            var letter = hit.GetComponent<LetterForLevel5>();
            if (letter != null)
            {
                letter.addHitCount();
                CameraShakeManager.Instance.CameraShake(impulseSource);
                AudioManager.Instance.PlayHammerSFX();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (tip != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(tip.position, checkRadius);
        }
    }
}
    