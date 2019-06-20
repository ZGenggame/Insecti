using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour {

    public static UIController _Instance;

    public GameObject MainScreen;
    public GameObject PlayScreen;
    public GameObject PauseScreen;
    public GameObject CollectSceen;
    public GameObject Collected;
    public GameObject Setting;
    public GameObject GameOver;
    public GameObject LookAdsPanel;
    public GameObject TeachPanel;
    public float UIEffectSpeed;

    private Scrollbar m_TimeLine;
    private Image m_LifeIndicate;
    private List m_collects;

    public int PlayerCurrentScore; //玩家当前的分数

    void Awake() {
        _Instance = this;
        m_TimeLine = PlayScreen.GetComponentInChildren<Scrollbar>();
        m_LifeIndicate = PlayScreen.transform.GetChild(PlayScreen.transform.childCount - 1)
            .GetChild(0).GetComponent<Image>();

        //初始化收集列表item
        List collects = CollectSceen.transform.GetChild(CollectSceen.transform.childCount - 1).GetComponentInChildren<List>();
        if (collects.itemCount == 0)
        {
            collects.InitList(ConstValue.GetInstance().levelStrings.Count);
        }
        m_collects = collects;
        foreach (var item in collects.GetComponentsInChildren<ListElement>())
        {
            if (item.RedDot.activeSelf)
            {
                 MainScreenRedDot.SetActive(true);
                 GameOverScreenRedDot.SetActive(true);
                 break;
            }
        }
        if (PlayerPrefs.GetString("ShowMainCollect") == "true")
        {
            MainScreen.transform.FindChild("Collect").gameObject.SetActive(true);
        }
    }

    void Start() {
        //AudioContorller._Instance.PlayAudioOneTime(0); //播放一次背景音
        //AudioContorller._Instance.PlayAudioOneTimeDelay(0, 0.2f);

        if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.UIMusicState) != 1)
        {
            //AudioContorller._Instance.PlayAudioOneTime(0); //播放进入游戏的背景音 music
            AudioContorller._Instance.PlayAudioOneTimeDelay(0, 0.3f);
        }
    }
    

    public void GameStartClick() {
        //ScenesController._Instance.InitGame();
        StartCoroutine(ScenesController._Instance
               .DestroyScenes(ScenesController._Instance.currentLevelColor.BackGroundColor));
        MainScreen.SetActive(false);
        PlayScreen.SetActive(true);
        PlayerCurrentScore = 0; //得分清零

        AudioContorller._Instance.PlayAudioOneTime(1); //播放开始游戏的音乐
    }

    public void GamePauseClick() {
        GameController._Instance.GamePause();
        PlayScreen.SetActive(false);
        PauseScreen.SetActive(true);
        AudioContorller._Instance.PlayAudioOneTimeNotStopLast(6,0.8f);

    }
    public void GamePauseBackClick()
    {
        GameController._Instance.GamePauseBack();
        PlayScreen.SetActive(true);
        PauseScreen.SetActive(false);
        AudioContorller._Instance.PlayAudioOneTimeNotStopLast(6, 0.8f);
    }

    //重新开始 不代表回到主界面
    public void GameRestartClick(GameObject ToNoActive) {
        GameController._Instance.GameRestart();
        ToNoActive.SetActive(false);
        //m_TimeLine.size = 1f;
        m_LifeIndicate.fillAmount = 1f;
        PlayScreen.SetActive(true);
        PlayerCurrentScore = 0; //得分清零

        AudioContorller._Instance.PlayAudioOneTime(1); //播放开始游戏的音乐
    }

    #region 收藏列表相关
    private GameObject OriginGameObject;
    public GameObject MainScreenRedDot, GameOverScreenRedDot;

    public void ShowRedDot(int index)
    {
        
        if (PlayerPrefs.GetString("ShowMainCollect") != "true")
        {
            MainScreen.transform.FindChild("Collect").gameObject.SetActive(true);
            PlayerPrefs.SetString("ShowMainCollect", "true");
        }
        MainScreenRedDot.SetActive(true);
        GameOverScreenRedDot.SetActive(true);
        ListElement[] eles = m_collects.GetComponentsInChildren<ListElement>();
        eles[index].ShowRedDot(index);
    }

    public void CollectClick(GameObject from) 
    {
        List collects = CollectSceen.transform.GetChild(CollectSceen.transform.childCount - 1).GetComponentInChildren<List>();
        if (collects.itemCount == 0)
        {
            collects.InitList(ConstValue.GetInstance().levelStrings.Count);
        }
        else
        {

            ListElement[] elements = collects.GetComponentsInChildren<ListElement>();
            foreach (var item in elements)
            {
                item.UpdateTitle(); //点击界面的时候回更新所有标题
                //if (item.RedDot != null && item.RedDot.activeSelf)
                //{
                //    from.transform.FindChild("Collect").FindChild("RedDot").gameObject.SetActive(false);
                //}
            }

        }
        CollectSceen.SetActive(true);
        from.SetActive(false);
        OriginGameObject = from;

        AudioContorller._Instance.PlayAudioOneTimeNotStopLast(6, 0.8f);
    }

    public void CollectBackClick(GameObject ToBackPage)
    {
        CollectSceen.SetActive(false);
        ToBackPage.SetActive(true);
        CollectSceen.transform.GetChild(CollectSceen.transform.childCount - 1)
            .GetComponentInChildren<List>().ToShrinkAll();

        AudioContorller._Instance.PlayAudioOneTimeNotStopLast(6, 0.8f);
       
    }
    public void CollectBackClick()
    {
        CollectSceen.SetActive(false);
        if (OriginGameObject != null)
        {
            OriginGameObject.SetActive(true);

            if (CollectSceen.transform.GetChild(CollectSceen.transform.childCount - 1).GetComponentInChildren<List>().HaveRedDot())
            {
                //OriginGameObject.transform.FindChild("Collect").FindChild("RedDot").gameObject.SetActive(true);
                GameOverScreenRedDot.SetActive(true);
                MainScreenRedDot.SetActive(true);
            }
            else {
                //OriginGameObject.transform.FindChild("Collect").FindChild("RedDot").gameObject.SetActive(false);
                GameOverScreenRedDot.SetActive(false);
                MainScreenRedDot.SetActive(false);

            }
        }
        CollectSceen.transform.GetChild(CollectSceen.transform.childCount - 1)
            .GetComponentInChildren<List>().ToShrinkAll();

        AudioContorller._Instance.PlayAudioOneTimeNotStopLast(6, 0.8f);

    }

    #endregion
   

   

    public void CollectedSuccess(string content) {
        Collected.GetComponent<Animator>()
            .SetTrigger(ConstValue.AnimatorParameters.CollectedTriggerName_IsCollectedSuccess);
        Collected.transform.GetChild(Collected.transform.childCount - 1).GetComponent<Text>().text = content;
    }

    public void SettingButtonClick()
    {
        Setting.SetActive(true);
        MainScreen.SetActive(false);

        AudioContorller._Instance.PlayAudioOneTimeNotStopLast(6, 0.8f);
    }

    public void SettingBackButtonClick()
    {
        Setting.SetActive(false);
        MainScreen.SetActive(true);

        AudioContorller._Instance.PlayAudioOneTimeNotStopLast(6, 0.8f);
    }

    #region 计时器相关
    public float TimeDeadLine;
    public bool UpdateGameTimeLine(float value)
    {
        //m_TimeLine.size += value;
        //if (m_TimeLine.size <= 0f)
        //{
        //    return false;
        //}
        //else
        //{
        //    return true;
        //}
        m_LifeIndicate.fillAmount += value;
        if (m_LifeIndicate.fillAmount <= TimeDeadLine)
        {
            //m_LifeIndicate.DOKill(false);
            SetIndicate();
        }
        else if (m_LifeIndicate.fillAmount > TimeDeadLine)
        {
            //m_LifeIndicate.DOKill(false);
            //m_LifeIndicate.DOColor(new Color(200f / 225f, 200f / 225f, 200f / 225f), 0.2f);
            if (haveSet)
            {
                //m_LifeIndicate.GetComponent<AudioSource>().Stop(); //控制计时器音效
                StopCoroutine("PlayTimeClickSound");
                m_LifeIndicate.DOKill(false);
                m_LifeIndicate.DOColor(new Color(
                  m_LifeIndicate.color.r, m_LifeIndicate.color.g, m_LifeIndicate.color.b, 1f), 0.2f);
                haveSet = false;
            }

        }


        if (m_LifeIndicate.fillAmount <= 0f)
        {
            m_LifeIndicate.DOKill(false);
            m_LifeIndicate.color = new Color(
                  m_LifeIndicate.color.r, m_LifeIndicate.color.g, m_LifeIndicate.color.b, 1f);
            return false;
        }
        else
        {
            return true;
        }

    }

    bool haveSet;
    void SetIndicate()
    {
        if (!haveSet)
        {
            //m_LifeIndicate.DOColor(new Color(224f/225f,15f/225f,17f/225f), 0.3f);
            //m_LifeIndicate.transform.DOShakePosition(0.2f,2f,30,90f,true,false);
            CompleteColorFade();
            haveSet = true;
            //CancelInvoke("PlayTimeClickSound");
            StartCoroutine("PlayTimeClickSound");
        }
    }
    IEnumerator PlayTimeClickSound()
    {
        AudioSource m_AS = m_LifeIndicate.GetComponent<AudioSource>();
        while (true)
        {
            AudioContorller._Instance.ToPlayTargetSound(
                m_AS);
            yield return new WaitForSeconds(0.8f);
        }
    }

    void CompleteColorLight()
    {
        m_LifeIndicate.DOColor(new Color(
              m_LifeIndicate.color.r, m_LifeIndicate.color.g, m_LifeIndicate.color.b, 1f), 0.2f)
              .OnComplete(CompleteColorFade);
    }
    void CompleteColorFade()
    {
        m_LifeIndicate.DOColor(new Color(
              m_LifeIndicate.color.r, m_LifeIndicate.color.g, m_LifeIndicate.color.b, 0.5f), 0.2f)
              .OnComplete(CompleteColorLight);
    }


    //public bool UpdateGameTimeLine(float value, bool isCoefficient)
    //{
    //    m_TimeLine.size += m_TimeLine.size * value ;
    //    if (m_TimeLine.size <= 0f)
    //    {
    //        return false;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}
    #endregion
  


    public void ShowLookAds()
    {
        StopCoroutine("PlayTimeClickSound");
        PlayScreen.SetActive(false);
        LookAdsPanel.SetActive(true);
    }

    public void ToLookAds() { 
        //展示广告
        Time.timeScale = 1f; //这句话要写在 展示玩广告的回掉中

        GameController._Instance.GameStart();
        UpdateGameTimeLine(1f);
        PlayScreen.SetActive(true);
        LookAdsPanel.SetActive(false);

    }

    public void LookAdsBackClick() {
        LookAdsPanel.SetActive(false);
        ShowGameOverPanel();

        AudioContorller._Instance.PlayAudioOneTimeNotStopLast(6, 0.8f);
    }
    public void ShowGameOverPanel() {
        GameOver.SetActive(true);
        PlayScreen.SetActive(false);
        GameOver.GetComponent<GameOverPanel>().ShowScore(PlayerCurrentScore
            , PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.PlayerBestScore));
    }
    public void GameBackToHomeButtonClick() {
        GameController._Instance.GameBackToHome();

        AudioContorller._Instance.PlayAudioOneTimeNotStopLast(6, 0.8f);
    }

    public void ShowTeachPanel() {
        TeachPanel.SetActive(true);
        if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.PlayerInGameSuccessCount) == 0)
        {
            Time.timeScale = 0f;
        }
    }

    public void CloseTeachPanel() {

        if (!PauseScreen.activeSelf)
        {
            Time.timeScale = 1f; //这是第一次进入游戏展示的教学所以把时间停止了
        }
        TeachPanel.SetActive(false);
    }
}
