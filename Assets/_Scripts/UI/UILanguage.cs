using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILanguage : MonoBehaviour {
    public Text target;
    public string ChineseWord;
    public string EnglishWord;

    void OnEnable() {
        if (Application.systemLanguage == SystemLanguage.Chinese
             || Application.systemLanguage == SystemLanguage.ChineseSimplified
              || Application.systemLanguage == SystemLanguage.ChineseTraditional
            )
        {
            target.text = ChineseWord;
        }
        else {
            target.text = EnglishWord;
        }
    }
}
