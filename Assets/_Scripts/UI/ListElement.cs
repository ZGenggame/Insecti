using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Text;

public class ListElement : MonoBehaviour {

    private RectTransform m_RectTransform;
    private bool IsElementExpand;
    private Rect mOriginRect;
    public Text content;
    private Color originContentColor;

    public GameObject SealCharacter, Simplified, English;
    private List<GameObject> expandInstance = new List<GameObject>();

    private int m_WordIndex;
    private StringBuilder m_StringBuilder = new StringBuilder();
    private Text m_Content;

    public GameObject WordInstance;
    public GameObject WordInstanceSeal;
    public GameObject RedDot;

    void Awake() { 
        m_RectTransform = GetComponent<RectTransform>();
        originContentColor = content.color;
        mOriginRect = m_RectTransform.rect;
    }

	// Use this for initialization
	void Start () {
	}

    public void Init(int index) {
        mOriginRect = m_RectTransform.rect;
        transform.FindChild("Number").GetComponent<Text>().text = (index + 1).ToString(); //数据编号是从0开始的
        m_Content = transform.FindChild("Content").GetComponent<Text>();
        m_WordIndex = index;
        UpdateTitle();
        if ( PlayerPrefs.GetString("Number" + index.ToString()) == "true")
        {
            RedDot.SetActive(true);
        }
        else
        {
            RedDot.SetActive(false);
        }
    }
	
	public void UpdateTitle () { //更新局部标题
        char[] title = PlayerPrefs.GetString(ConstValue.GetInstance().levelStrings[m_WordIndex]).ToCharArray();
        char[] words = ConstValue.GetInstance().levelStrings[m_WordIndex].ToCharArray();
        m_StringBuilder.Remove(0, m_StringBuilder.Length);
        for (int i = 0; i < title.Length; i++)
        {
            if (title[i] == '1')
            {
                m_StringBuilder.Append(words[i]);
            }
            else {
                m_StringBuilder.Append('?');
            }
        }
        m_Content.text = m_StringBuilder.ToString();

        if (m_Content.transform.childCount == 0)
        {
            for (int i = 0; i < title.Length; i++)
            {
                GameObject m_word = Instantiate(WordInstance);
                m_word.transform.SetParent(m_Content.transform);
            }
        }
       
        for (int i = 0; i < m_Content.transform.childCount; i++)
        {
            m_Content.transform.GetChild(i).GetChild(0).GetComponent<Text>().text
                = m_StringBuilder[i] == '?' ? "" : m_StringBuilder[i].ToString();
        }

	}

    public void ElementClick() { //展开收藏列表
        
        //if (PlayerPrefs.GetString(ConstValue.GetInstance().levelStrings[m_WordIndex]) != "1111") //完成收集
        //{
        //   return;
        //}

        if (!PlayerData.GetInstance().IsAllStringRight(PlayerPrefs.GetString(ConstValue.GetInstance().levelStrings[m_WordIndex])))
        {
            return;
        }

        if (!IsElementExpand)
        {
            AudioContorller._Instance.PlayAudioOneTimeNotStopLast(7, 0.8f);

            //m_RectTransform.sizeDelta = new Vector2(mRect.width, mRect.height + 300f);
            m_RectTransform.DOSizeDelta(new Vector2(mOriginRect.width, mOriginRect.height + 540f), 0.5f).SetUpdate(true);
            IsElementExpand = true;

            //BuildContent(SealCharacter, ConstValue.GetInstance().levelStrings[m_WordIndex]);
            //BuildContent(Simplified, ConstValue.GetInstance().levelStrings[m_WordIndex]);

            BuildContent(SealCharacter,WordInstanceSeal, ConstValue.GetInstance().levelStrings[m_WordIndex]);
            BuildContent(Simplified, WordInstance, ConstValue.GetInstance().levelStrings[m_WordIndex]);

            BuildContent(English, ConstValue.GetInstance().levelEngLishStrings[m_WordIndex]);
            
            //content.DOColor(Color.white, 0.5f);
            //content.gameObject.SetActive(false);
            content.transform.DOScale(Vector3.zero, 0.3f).SetUpdate(true);
            transform.parent.GetComponent<List>().ExpandList(540f);

            if (RedDot != null && RedDot.activeSelf)
            {
                PlayerPrefs.SetString("Number" + m_WordIndex.ToString(), "false");
                RedDot.SetActive(false);
            }
        }
        else {
            //m_RectTransform.sizeDelta = new Vector2(mRect.width, mOriginRect.height);
            ToShrinkElement();
            AudioContorller._Instance.PlayAudioOneTimeNotStopLast(8, 0.8f);
        }
    }

    void BuildContent(GameObject Instance, GameObject word, string content)
    {
        GameObject gbj = Instantiate(Instance);
        gbj.transform.SetParent(transform);
        gbj.GetComponent<RectTransform>().anchoredPosition = Instance.transform.position;
        char[] words = content.ToCharArray();
        for (int i = 0; i < words.Length; i++)
        {
            GameObject m_word = Instantiate(word);
            m_word.transform.SetParent(gbj.transform);
            m_word.transform.GetChild(0).GetComponent<Text>().text 
                = words[i] == '?' ? "" : words[i].ToString();
        }

        //gbj.GetComponent<Text>().text = content;
        expandInstance.Add(gbj);
    }

    void BuildContent(GameObject Instance,string content) {
        GameObject gbj = Instantiate(Instance);
        gbj.transform.SetParent(transform);
        gbj.GetComponent<RectTransform>().anchoredPosition = Instance.transform.position;
        gbj.GetComponent<Text>().text = content;
        expandInstance.Add(gbj);
    }

    public void ToShrinkElement() {
        if (IsElementExpand)
        {
            m_RectTransform.DOSizeDelta(new Vector2(mOriginRect.width, mOriginRect.height), 0.3f).SetUpdate(true);
            IsElementExpand = false;
            //content.DOColor(originContentColor, 0.3f);
            //content.gameObject.SetActive(true);
            content.transform.DOScale(Vector3.one, 0.2f).SetUpdate(true);
            foreach (var item in expandInstance)
            {
                Destroy(item);
            }
            transform.parent.GetComponent<List>().ShirnkList(540f);
        }
    }

    public void ShowRedDot(int index) { //写入数据
        PlayerPrefs.SetString("Number" + index.ToString(), "true");
        RedDot.SetActive(true);
    }
}
