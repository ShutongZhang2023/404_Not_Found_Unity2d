using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L4_TreeZone : MonoBehaviour
{
    public int stageNum = 0;
    public GameObject root;
    public GameObject rain;
    private Animator anim;
    public SpriteRenderer backgound;

    [SerializeField] private GameObject apple1;
    [SerializeField] private GameObject apple2;


    private void Awake()
    {
        anim = root.GetComponent<Animator>();
    }
    public void TryAcceptWord(L4_Seed seed)
    {
        if (stageNum == 0 && seed.word == "SEED")
        {
            stageNum = 1;
            seed.FadeOutAndDisable();
            root.SetActive(true);
            return;
        }
        else if (stageNum == 1 && seed.word == "WATER")
        {
            stageNum = 2;
            seed.FadeOutAndDisable();
            rain.SetActive(true);
            StartCoroutine(Grow1());
            return;
        }
        else if (stageNum == 2 && seed.word == "SUNSHINE")
        {
            stageNum = 3;
            seed.FadeOutAndDisable();
            rain.SetActive(false);
            backgound.DOFade(0f, 1.5f).OnComplete(() => Grow2());
            return;
        }
        else {
            seed.ShakeAndReturn();
        }
    }

    private IEnumerator Grow1() {
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("Grow1");
    }

    private void Grow2()
    {
        anim.SetTrigger("Grow2");
        StartCoroutine(AppleGrow());
    }

    private IEnumerator AppleGrow()
    {
        yield return new WaitForSeconds(2f);
        apple1.transform.localScale = Vector3.zero;
        apple2.transform.localScale = Vector3.zero;
        Vector3 target = new Vector3(0.3f, 0.3f, 0.3f);
        apple1.SetActive(true);
        apple1.transform.DOScale(target, 1f);
        yield return new WaitForSeconds(1f);
        apple2.SetActive(true);
        apple2.transform.DOScale(target, 1f);
    }


}
