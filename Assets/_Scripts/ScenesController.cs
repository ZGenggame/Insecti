using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesController : MonoBehaviour {

    public static ScenesController _Instance;
    public GameObject GameControllerInstance;
    public GameObject LevelUpBackGround;
    public float LevelUpBackGroundBigSpeed;

    private GameObject LastBackGround;

    [System.Serializable]
    public class LevelColor {
        public int Index;
        public Color BackGroundColor;
        public Sprite MoveCubeImage;
    }
    public List<LevelColor> colors;

    [HideInInspector]
    public LevelColor currentLevelColor;

    void Awake() {
        _Instance = this;
        if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.PlayerInGameSuccessCount) == 0)
        {
            currentLevelColor = colors[0];
        }
        else {
            ChooseLevelColor();
        }
    }  

    public void InitGame() {
        Instantiate(GameControllerInstance);
        //这里对背景色进行判断
    }

    public void PrepareNextLevel() {

        GameObject[] gbjs = GameObject.FindGameObjectsWithTag(ConstValue.TagName.MOVEPOINT);
        foreach (var item in gbjs)
        {
            Destroy(item);
        }

        List<List<Transform>> roads = new List<List<Transform>>();
        if (GameController._Instance.map.UpRoadInstance.Count != 0)
            roads.Add(GameController._Instance.map.UpRoadInstance);

        if (GameController._Instance.map.DownRoadInstance.Count != 0)
            roads.Add(GameController._Instance.map.DownRoadInstance);

        if (GameController._Instance.map.LeftRoadInstance.Count != 0)
            roads.Add(GameController._Instance.map.LeftRoadInstance);

        if (GameController._Instance.map.RightRoadInstance.Count != 0)
            roads.Add(GameController._Instance.map.RightRoadInstance);
       
        for (int i = 0; i < roads.Count; i++)
            {
                List<Transform> road = roads[i];
                for (int j = road.Count - 1; j > 0 ; j--)
                {
                    if (j - 1 == 0)//路径点的最后一个点向中心移动
                    {
                        road[j - 1].GetComponent<MapPoint>().Init(GameController._Instance.CenterCube.transform, 10f);
                    }

                    if (j == road.Count - 1)
                    {
                        road[j].GetComponent<MapPoint>().CanMove = true;
                        
                    }
                    road[j].GetComponent<MapPoint>().Init(road[j - 1], 10f);
             }
            
            GameController._Instance.CenterCube.InitMove(roads.Count);
            GameController._Instance.CenterCube.HaveFinshRoadMoveCount = 0;
        }
        ChooseLevelColor();
        AudioContorller._Instance.PlayAudioOneTimeNotStopLast(12, 0.55f);
    }

    //变更关卡颜色
    public void ChooseLevelColor() {
        int index  = -1;
        do
        {
            index = Random.Range(0, colors.Count);

        } while (currentLevelColor != null && index == colors.IndexOf(currentLevelColor));
        currentLevelColor = colors[index] ;
    }


    public IEnumerator DestroyScenes(Color BackGroundColor) {
        if (LastBackGround != null)
        {
            LastBackGround.GetComponent<SpriteRenderer>().sortingOrder = -10; //修改层级
        }
        GameObject back = Instantiate(LevelUpBackGround, Vector3.zero, Quaternion.identity);
        back.GetComponent<SpriteRenderer>().color = BackGroundColor;
        while (back.transform.localScale.x < Tools._Instance.width)
        {
            back.transform.localScale += Vector3.one * LevelUpBackGroundBigSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        GameObject gbj = GameObject.FindGameObjectWithTag(ConstValue.TagName.GAMECONTROLLER);
        if (gbj != null) Destroy(gbj);
        

        ToDestroyGamePlayObject();
       
        back.GetComponent<SpriteRenderer>().sortingOrder = -5; //修改层级

        InitGame(); //初始化新的游戏
        StartCoroutine(StartAnother(back));
    }

    public void ToDestroyGamePlayObject() {



        GameObject gbj = GameObject.FindGameObjectWithTag(ConstValue.TagName.PLAYER);
        if (gbj != null) Destroy(gbj);

        GameObject[] gbjs = GameObject.FindGameObjectsWithTag(ConstValue.TagName.MOVEPOINT);
        foreach (var item in gbjs)
        {
            Destroy(item);
        }

        gbjs = GameObject.FindGameObjectsWithTag(ConstValue.TagName.MAPPOINT);
        foreach (var item in gbjs)
        {
            Destroy(item);
        }
    }


    IEnumerator StartAnother(GameObject back) {
        while (back.transform.localScale.y < Tools._Instance.OriginHeight + 10)
        {
            back.transform.localScale += Vector3.one * LevelUpBackGroundBigSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (LastBackGround != null)
        {
            Destroy(LastBackGround);
        }
        LastBackGround = back; //把当前图片添置为背景
    }

}
