using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class List : MonoBehaviour {

    public GameObject item;
    private RectTransform m_RectTransform;
    private VerticalLayoutGroup m_VerticalLayoutGroup;
    private Rect m_OriginRect;

    public int itemCount;

    void Awake() {
        InitSelf();
    }

	void Start () {
        
	}

    void InitSelf() {
        m_RectTransform = GetComponent<RectTransform>();
        m_VerticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        m_OriginRect = m_RectTransform.rect;
    }


    public void InitList(int count) {
        for (int i = 0; i < count; i++)
        {
            GameObject gbj = Instantiate(item);
            gbj.GetComponent<ListElement>().Init(i);
            gbj.transform.SetParent(transform);
        }
        InitSelf();
        m_RectTransform.pivot = new Vector2(0.5f, 1f);

        m_RectTransform.sizeDelta = new Vector2(m_RectTransform.rect.x,
            (item.GetComponent<RectTransform>().rect.height + m_VerticalLayoutGroup.spacing) * (transform.childCount) - m_OriginRect.size.y
            //+ m_VerticalLayoutGroup.spacing * (transform.childCount - 1)
            );

        //m_RectTransform.sizeDelta = new Vector2(m_RectTransform.rect.x, m_RectTransform.sizeDelta.y + m_VerticalLayoutGroup.spacing);
        itemCount = count;
    }


    public void ExpandList(float value) {
        m_RectTransform.sizeDelta = new Vector2(m_RectTransform.sizeDelta.x, m_RectTransform.sizeDelta.y + value);
    }

    public void ShirnkList(float value)
    {
        m_RectTransform.sizeDelta = new Vector2(m_RectTransform.sizeDelta.x, m_RectTransform.sizeDelta.y - value);
    }

    public void ToShrinkAll() {
        ListElement[] eles = transform.GetComponentsInChildren<ListElement>();
        foreach (var item in eles)
        {
            item.ToShrinkElement();
        }
    }

    public bool HaveRedDot() {
        ListElement[] eles = transform.GetComponentsInChildren<ListElement>();
        foreach (var item in eles)
        {
            if (item.RedDot != null && item.RedDot.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

}
