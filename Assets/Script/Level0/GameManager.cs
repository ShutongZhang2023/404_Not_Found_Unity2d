using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum Language { English, Other }
public class GameManager : MonoBehaviour
{
    public event Action Translator;

    public LinePrinter linePrinter;
    public static GameManager Instance;

    [Header("Scene List")]
    public List<string> levels = new List<string>();
    private int currentLevelIndex = 0;

    [Header("UI Fade Settings")]
    public CanvasGroup fadePanel;
    public float fadeDuration = 1f;

    [Header("Language Settings")]
    public Language currentLanguage = Language.English;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
   

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        linePrinter = FindObjectOfType<LinePrinter>();
        if (linePrinter != null)
        {
            linePrinter.OnStageFinished += HandleStageFinished;

            // ����Ǹս���Ĺؿ������������ﴥ����һ�ζԻ�
            StartCoroutine(StartDialogueDelayed());
        }
        else
        {
            Debug.LogWarning("No LinePrinter found in scene: " + scene.name);
        }

        // ����������ɺ�������
            fadePanel.alpha = 1f;
            fadePanel.blocksRaycasts = true;
            fadePanel.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                fadePanel.blocksRaycasts = false;
            });
    }

    private IEnumerator StartDialogueDelayed()
    {
        yield return new WaitForSeconds(1f);
        linePrinter.PlayStage(DialogueStage.BeforePuzzle);
    }

    public void ChangeState(DialogueStage stage) 
    {
        linePrinter.PlayStage(stage);
    }

    private void HandleStageFinished(DialogueStage stage)
    {
        if (stage == DialogueStage.AfterPuzzle || stage == DialogueStage.ChoiceA || stage == DialogueStage.ChoiceB || stage == DialogueStage.ChoiceC)
        {
            currentLevelIndex++;
            if (currentLevelIndex < levels.Count)
            {
                StartCoroutine(LoadLevelRoutine(levels[currentLevelIndex], levels[currentLevelIndex-1]));
            }
            else
            {
                Debug.Log("All levels finished!");
            }
        }
    }

    private IEnumerator LoadLevelRoutine(string sceneName, string currentLevelName)
    {
        fadePanel.blocksRaycasts = true;
        yield return fadePanel.DOFade(1f, fadeDuration).WaitForCompletion();

        SceneManager.UnloadSceneAsync(currentLevelName);

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void changeLanguage() {
        bool hasLevel3 = false;

        // ���������Ѽ��صĳ���
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == "Level3")
            {
                hasLevel3 = true;
                break;
            }
        }

        if (hasLevel3)
        {
            Translator?.Invoke();
        }
        else
        {
            GameManager.Instance.currentLanguage = Language.English;
        }
    }
}
