using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
    public Language language;   // 例如 "English"
    public Image buttonImage;     // 按钮本身的背景图
    public Color selectedColor = Color.white;
    public Color unselectedColor = Color.gray;

    void OnEnable()
    {
        Refresh();
    }

    public void OnClicked()
    {
        Debug.Log("LanguageButton clicked: " + language);
        GameManager.Instance.currentLanguage = language;
        //GameManager.Instance.SaveLanguage();

        // 刷新所有按钮
        foreach (var btn in FindObjectsOfType<LanguageButton>())
        {
            btn.Refresh();
        }
    }

    public void Refresh()
    {
        bool isCurrent = (GameManager.Instance.currentLanguage == language);
        buttonImage.color = isCurrent ? selectedColor : unselectedColor;
    }
}
