using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class PlayerData {

    private static PlayerData _Instance;

    public static PlayerData GetInstance()
    {

        if (_Instance == null)
        {
            _Instance = new PlayerData();
        }
        return _Instance;
    }


    public void UpdatePlayerCharacter(int big,int small) {

        char[] words = PlayerPrefs.GetString(ConstValue.GetInstance().levelStrings[big]).ToCharArray();

        for (int i = 0; i < words.Length; i++)
        {
            if (words[i] != '1')
            {
                break;
            }
            else if (i == words.Length - 1)
            {
                return; //这个词条已经被收集过了
            }
        }

        for (int i = 0; i < words.Length; i++)
        {
            if (i == small && words[i] != '1')
            {
                words[i] = '1'; //正确收集到的字符标注为1，没有收集到的标注为0
            }
        }
        PlayerPrefs.SetString(ConstValue.GetInstance().levelStrings[big], new string(words)); //收集情况
        string result = PlayerPrefs.GetString(ConstValue.GetInstance().levelStrings[big]);

        if (IsAllStringRight(result))
        {
            //展示收集完成的奖励窗口
            UIController._Instance.CollectedSuccess(ConstValue.GetInstance().levelStrings[big]);
            UIController._Instance.ShowRedDot(big); //收集完成的红点
        }
    }


    public bool IsAllStringRight(string result)
    {
        int index = 0;
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i].ToString() != "1")
            {
                break;
            }
            index = i;
        }
        if (index == result.Length - 1) //全部收集完成
        {
            return true;
        }
        else {
            return false;
        }
    
    }
}
