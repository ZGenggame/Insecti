using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Check : MonoBehaviour {

    private Color originTextColor;// = Color.red;
    public class TextContent{
        public TextContent(bool havCheck,string content) {
            this.HaveCheck = havCheck;
            this.Content = content;
        }

        public bool HaveCheck;
        public string Content;
    }

    public List<Text> Shows = new List<Text>();
    public List<TextContent> innerCheck = new List<TextContent>();

    public GameObject EffectInstance;
    private Transform currentRightWord;

    private List<ConstValue.Character> currentCharacter = new List<ConstValue.Character>(); //内部数据处理

    void Awake() {
       //originTextColor = Shows[0].color;
       originTextColor = ScenesController._Instance.currentLevelColor.MoveCubeImage.texture.GetPixel(100,100);
       //originTextColor = ScenesController._Instance.currentLevelColor.BackGroundColor;
    }


    void Start()
    {
        //originTextColor = Shows[0].color; 这里有一个脚本调用顺序问题
    }

    public void ShowText(Text text,Text result,int index) {
        text.color = result.color;
        foreach (var item in innerCheck)
        {
            if (item.Content == text.text && innerCheck.IndexOf(item) == index) //文字的序列位置也要相同
            {
                item.HaveCheck = true; //更改检查状态
                break;
            }
        }
        currentRightWord = text.transform;
    }

    void Update() {
        if (Time.timeScale  == 1f)
        {
            foreach (var item in Shows)
            {
                item.transform.rotation = Quaternion.identity; //固定旋转
                //item.transform.rotation = Quaternion.Euler(
                //    new Vector3(item.transform.rotation.x,
                //                item.transform.rotation.y,
                //                Quaternion.identity.z));
            }
        }
        else if (transform.tag == ConstValue.TagName.PLAYER)
        {
            foreach (var item in Shows)
            {
                item.transform.rotation = Quaternion.identity; //固定旋转
                //item.transform.rotation = Quaternion.Euler(
                //    new Vector3(item.transform.rotation.x,
                //                item.transform.rotation.y,
                //                Quaternion.identity.z));
            }
        }
    }

    public Text GetShowsText() {
        for (int i = 0; i < Shows.Count; i++)
        {
            if (Shows[i].text != "") //唯一一个数字 text != null 判断不成立 文字相同
            {
                return Shows[i];
            }
        }
        return null;
    }

    public bool AlreadyRightText(Text text, List<TextContent> CenterCube)
    {
        foreach (var item in CenterCube)
        {
            if (item.Content == text.text 
                && item.HaveCheck //已经被见车过正确的字符
                ) //文字的序列位置也要相同
            {
                return true;
            }
        }
        return false;
    }

    public bool IsHadRepeatText(List<TextContent> CenterCube)
    {
        List<TextContent> noChecks = new List<TextContent>();
        List<TextContent> yesChecks = new List<TextContent>();
        foreach (var item in CenterCube)
        {
            if (!item.HaveCheck)
            {
                noChecks.Add(item);
            }
            else {
                yesChecks.Add(item);
            }
        }

        foreach (var item in yesChecks)
        {
            foreach (var ele in noChecks) //未检查的字符里含有与完成检查的字符里相同的字符
            {
                if (ele.Content == item.Content)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //更新新的移动字符 根据中心已有字符初始下一个字符
    public void UpdateNextText(List<TextContent> CenterCube) {
        List<TextContent> AlterNative = new List<TextContent>();
        for (int i = 0; i < CenterCube.Count; i++)
        {
            if (!CenterCube[i].HaveCheck)
            {
                AlterNative.Add(CenterCube[i]);
            }
        }
        int index = Random.Range(0, AlterNative.Count);
        TextContent result = AlterNative[index]; //初始化一个新的数字
        foreach (var item in Shows) //清空显示数据
        {
            if (item.text != "")
            {
                item.text = "";
            }
        }
        Shows[Random.Range(0, Shows.Count)].text = result.Content;
        ReverseTextColor();

    }

    public void UpdateNextText(List<TextContent> CenterCube,Text show)
    {
        List<TextContent> AlterNative = new List<TextContent>();
        for (int i = 0; i < CenterCube.Count; i++)
        {
            if (!CenterCube[i].HaveCheck)
            {
                AlterNative.Add(CenterCube[i]);
            }
        }
        int index = Random.Range(0, AlterNative.Count);
        TextContent result = AlterNative[index]; //初始化一个新的数字
        foreach (var item in Shows) //清空显示数据
        {
            if (item.text != "")
            {
                item.text = "";
            }
        }
        show.text = result.Content;
        //ReverseTextColor();
    }



    //初始化中心十字所用
    public void InitText(string s1,string s2,string s3,string s4) {
        innerCheck.Clear();
        Shows[0].text = s1;
        innerCheck.Add(new TextContent(false, s1));
        Shows[1].text = s2;
        innerCheck.Add(new TextContent(false, s2));
        Shows[2].text = s3;
        innerCheck.Add(new TextContent(false, s3));
        Shows[3].text = s4;
        innerCheck.Add(new TextContent(false, s4));
        //ReverseTextColor();
    }

    public void InitText(List<ConstValue.Character> current)
    {
        InitData();
        for (int i = 0; i < current.Count; i++)
        {
            Shows[i].text = current[i].Word;
            innerCheck.Add(new TextContent(false, current[i].Word));
            currentCharacter.Add(current[i]);
        }
        //ReverseTextColor();
    }

    void InitData()
    { //清空数据
        innerCheck.Clear();
        currentCharacter.Clear();
        foreach (var item in Shows)
        {
            item.text = "";
        }
    }

    public void ToUpdatePlayerData(int index) { 
        PlayerData.GetInstance().UpdatePlayerCharacter(currentCharacter[index].WordIndex.BigNumber
            , currentCharacter[index].WordIndex.SmallNumber);
    }

    private void ReverseTextColor() { //将颜色刷新回去
        foreach (var item in Shows)
        {
            item.color = originTextColor;
        }
    
    }

    public bool IsFinishLevel() {

        foreach (var item in innerCheck)
        {
            if (!item.HaveCheck)
            {
                return false;
            }   
        }
        return true;
    }

    //Effect
    public void RightWordEffect() { //正确字符特效
        if (currentRightWord != null)
        {
            Transform target = currentRightWord;
            target.DOPunchScale(Vector3.one * 0.45f, 0.23f, 3);
            EffectInstance.transform.position = target.position;
            EffectInstance.transform.localScale = Vector3.zero;
            Invoke("StarPlayerEffect", 0.19f);
        }
        
    }

    void StarPlayerEffect() {
        
        EffectInstance.transform.DOScale(Vector3.one * 2f, 1f);
        SpriteRenderer sr = EffectInstance.transform.GetComponent<SpriteRenderer>();
        sr.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.8f);
        sr.DOColor(
            new Color(Color.white.r, Color.white.g, Color.white.b,
            0f), 1f);
    }

    
}

