using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteChange : MonoBehaviour {

    public Sprite IsOn;
    public Sprite IsOFF;

    public Image target;


    void Start() {

        if (transform.name == "Music")
        {
            if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.UIMusicState) == 1) //关闭状态
            {
                target.sprite = IsOFF;
            }
            else
            {
                target.sprite = IsOn;
            }
        }
        else if (transform.name == "Sound")
        {
            if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.UISoundState) == 1) //关闭状态
            {
                target.sprite = IsOFF;
            }
            else
            {
                target.sprite = IsOn;
            }
        }
    
    }

    public void Click(int ButtonKind) {
        switch (ButtonKind)
        {
            case 0:  //音乐按钮
                if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.UIMusicState) == 0) //开启状态
                {
                    target.sprite = IsOFF;
                    PlayerPrefs.SetInt(ConstValue.XmlDataKeyName.UIMusicState, 1);
                    AudioContorller._Instance.StopPlay();
                }
                else {
                    target.sprite = IsOn;
                    PlayerPrefs.SetInt(ConstValue.XmlDataKeyName.UIMusicState, 0);
                }

                break;
            case 1:  //音效按钮
                if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.UISoundState) == 0) //开启状态
                {
                    target.sprite = IsOFF;
                    PlayerPrefs.SetInt(ConstValue.XmlDataKeyName.UISoundState, 1);
                }
                else
                {
                    target.sprite = IsOn;
                    PlayerPrefs.SetInt(ConstValue.XmlDataKeyName.UISoundState, 0);
                }

                break;
            default:
                break;
        }
        AudioContorller._Instance.PlayAudioOneTimeNotStopLast(6, 0.8f);
    }
    
}
