using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMap : MonoBehaviour {

    public float MapPointSize;
    public int RoadLength;
    public Vector3 centerPoint;

    public List<Vector3> UpRoad = new List<Vector3>();
    public List<Vector3> DownRoad = new List<Vector3>();
    public List<Vector3> LeftRoad = new List<Vector3>();
    public List<Vector3> RightRoad = new List<Vector3>();

    [HideInInspector]
    public List<Transform> UpRoadInstance = new List<Transform>();
    [HideInInspector]
    public List<Transform> DownRoadInstance = new List<Transform>();
    [HideInInspector]
    public List<Transform> LeftRoadInstance = new List<Transform>();
    [HideInInspector]
    public List<Transform> RightRoadInstance = new List<Transform>();

    //地图网格的实例
    public GameObject MapPointUp;
    public GameObject MapPointDown;
    public GameObject MapPointLeft;
    public GameObject MapPointRight;

    private Dictionary<int, List<Vector3>> finalRoad;
    private int hadBuildLength = -1;

	void Start () {
        //InitMap();
        //GameObject center = Instantiate(GameController._Instance.CenterCubeInstance, Vector3.zero, Quaternion.identity);
        //GameController._Instance.CenterCube = center.GetComponent<CenterCube>();
        //GameController._Instance.UpdateCenterLevelString(ConstValue.LevelString.Level01);
        //GameController._Instance.UpdateCenterLevelString(ConstValue.GetInstance().GetCharacter());
        //CenterCube.Init(UpRoadInstance, DownRoadInstance);

        int count = Random.Range(1,5);
        if (count == 4)
        {
            count = Random.Range(1,5);
        }
        InitMap(Vector3.zero,count,true);
	}

    void InitMap() {
        Vector3 centerPoint = new Vector3(
            Tools._Instance.leftBorder +  Tools._Instance.width/2f
            , Tools._Instance.downBorder + Tools._Instance.height / 2f
            ,0f
            );

        Vector3 farsetPointUp = centerPoint + RoadLength * (new Vector3(0f, MapPointSize, 0f));
        Vector3 farsetPointDown = centerPoint + RoadLength * (new Vector3(0f ,-MapPointSize, 0f));
        Vector3 farsetPointLeft = centerPoint + RoadLength * (new Vector3(-MapPointSize, 0f, 0f));
        Vector3 farsetPointRight = centerPoint + RoadLength * (new Vector3(MapPointSize, 0f, 0f));


        for (int i = 0; i < RoadLength; i++)
        {
            UpRoad.Add(farsetPointUp - new Vector3(0f, MapPointSize, 0f) * i);
            DownRoad.Add(farsetPointDown + new Vector3(0f, MapPointSize, 0f) * i);
            LeftRoad.Add(farsetPointLeft + new Vector3(MapPointSize, 0f , 0f) * i);
            RightRoad.Add(farsetPointRight - new Vector3(MapPointSize, 0f, 0f) * i);
        }


        //for (int i = 0; i < RoadLength; i++)
        //{
        //   UpRoadInstance.Add(Instantiate(MapPointUp, UpRoad[i] - new Vector3(0f,MapPointUp.transform.localScale.y/2f,0f), Quaternion.identity).transform);
        //   DownRoadInstance.Add(Instantiate(MapPointDown, DownRoad[i] + new Vector3(0f, MapPointDown.transform.localScale.y / 2f, 0f), Quaternion.identity).transform);
        //   LeftRoadInstance.Add(Instantiate(MapPointLeft, LeftRoad[i] + new Vector3(MapPointLeft.transform.localScale.x / 2f,0f, 0f), Quaternion.identity).transform);
        //   RightRoadInstance.Add(Instantiate(MapPointRight, RightRoad[i] - new Vector3(MapPointRight.transform.localScale.x / 2f, 0f, 0f), Quaternion.identity).transform);
        //}
    }

    public void InitMap(Vector3 centerPointPos, int RoadCount,bool IsBuildCenterCube) {
        centerPoint = centerPointPos;
        Tools._Instance.ScreenSize();
        if (IsBuildCenterCube)
        {
            GameObject center = Instantiate(GameController._Instance.CenterCubeInstance, centerPointPos, Quaternion.identity);
            GameController._Instance.CenterCube = center.GetComponent<CenterCube>();
           

            if (!PlayerData.GetInstance().IsAllStringRight(PlayerPrefs.GetString(ConstValue.GetInstance().levelStrings[0]))) //第一个词条没有收集完成
            {
                if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.PlayerInGameSuccessCount) < 2)
                {
                    RoadCount = 1;
                   
                    GameController._Instance
                        .UpdateCenterLevelString(ConstValue.GetInstance().GetCharacter(0
                        , ConstValue.GetInstance().
                        GetZeroWordInCharacter(0)[Random.Range(0, ConstValue.GetInstance().GetZeroWordInCharacter(0).Count)]
                        ));
                }
                else 
                {
                    RoadCount = ConstValue.GetInstance().GetZeroWordInCharacter(0).Count;
                    GameController._Instance
                        .UpdateCenterLevelString(ConstValue.GetInstance().GetCharacter(0
                        , ConstValue.GetInstance().GetZeroWordInCharacter(0).ToArray()
                        ));
                }
                
            }
            else {
                GameController._Instance.UpdateCenterLevelString(ConstValue.GetInstance().GetCharacter(RoadCount)); //这里可以随机给定的
            }
        }
        if (finalRoad != null)
        {
            finalRoad.Clear();
            UpRoad.Clear();
            DownRoad.Clear();
            LeftRoad.Clear();
            RightRoad.Clear();

            UpRoadInstance.Clear();
            DownRoadInstance.Clear();
            LeftRoadInstance.Clear();
            RightRoadInstance.Clear();
        }
       

        Vector3 farsetPointUp = centerPoint + RoadLength * (new Vector3(0f, MapPointSize, 0f));
        Vector3 farsetPointDown = centerPoint + RoadLength * (new Vector3(0f, -MapPointSize, 0f));
        Vector3 farsetPointLeft = centerPoint + RoadLength * (new Vector3(-MapPointSize, 0f, 0f));
        Vector3 farsetPointRight = centerPoint + RoadLength * (new Vector3(MapPointSize, 0f, 0f));

        Dictionary<int, List<Vector3>> roads = BuildRoadIndex(RoadCount);
        for (int i = 0; i < RoadLength; i++)
        {
            //UpRoad.Add(farsetPointUp - new Vector3(0f, MapPointSize, 0f) * i);
            //DownRoad.Add(farsetPointDown + new Vector3(0f, MapPointSize, 0f) * i);
            //LeftRoad.Add(farsetPointLeft + new Vector3(MapPointSize, 0f, 0f) * i);
            //RightRoad.Add(farsetPointRight - new Vector3(MapPointSize, 0f, 0f) * i);

            foreach (var item in roads.Keys) //对随机出的路 配置路径点
            {
                switch (item)
                {
                    case 0: //上
                        roads[item].Add(farsetPointUp - new Vector3(0f, MapPointSize, 0f) * i);
                        break;
                    case 1: //下
                        roads[item].Add(farsetPointDown + new Vector3(0f, MapPointSize, 0f) * i);
                        break;
                    case 2: //左
                        roads[item].Add(farsetPointLeft + new Vector3(MapPointSize, 0f, 0f) * i);
                        break;
                    case 3: //右
                        roads[item].Add(farsetPointRight - new Vector3(MapPointSize, 0f, 0f) * i);
                        break;
                    default:
                        break;
                }
            }
        }
        finalRoad = roads;
        hadBuildLength = -1;
    }


    

    Dictionary<int, List<Vector3>> BuildRoadIndex(int RoadCount) {
        Dictionary<int, List<Vector3>> All = new Dictionary<int, List<Vector3>>();
        Dictionary<int, List<Vector3>> pool = new Dictionary<int, List<Vector3>>();

        All.Add(0, UpRoad);
        All.Add(1, DownRoad);
        All.Add(2, LeftRoad);
        All.Add(3, RightRoad);

        switch (RoadCount)
        {
            case 1:
                int result = Random.Range(0, 4);
                pool.Add(result, All[result]);
                break;
            case 2:
                int first = Random.Range(0, 4);
                int second = Random.Range(0, 4);
                while (second == first)
                {
                    second = Random.Range(0, 4);
                }

                pool.Add(first, All[first]);
                pool.Add(second, All[second]);

                break;
            case 3:

                int notNeed = Random.Range(0, 4);
                for (int i = 0; i < All.Count; i++)
                {
                    if (i != notNeed)
                    {
                        pool.Add(i, All[i]);
                    }   
                }
                break;
            case 4:
                pool = All;
                break;
            default:
                break;
        }
        return pool;
    }

    public void BuildMapPoint() { //建立地图网格点
        //if (UpRoadInstance.Count < 3)
        //{
        //    UpRoadInstance.Add(Instantiate(MapPointUp, UpRoad[UpRoad.Count - 1 - UpRoadInstance.Count] - new Vector3(0f, MapPointSize / 2f, 0f), Quaternion.identity).transform);
        //    DownRoadInstance.Add(Instantiate(MapPointDown, DownRoad[DownRoad.Count - 1 - DownRoadInstance.Count] + new Vector3(0f, MapPointSize / 2f, 0f), Quaternion.identity).transform);
        //    LeftRoadInstance.Add(Instantiate(MapPointLeft, LeftRoad[LeftRoad.Count - 1 - LeftRoadInstance.Count] + new Vector3(MapPointSize / 2f, 0f, 0f), Quaternion.identity).transform);
        //    RightRoadInstance.Add(Instantiate(MapPointRight, RightRoad[RightRoad.Count - 1 - RightRoadInstance.Count] - new Vector3(MapPointSize / 2f, 0f, 0f), Quaternion.identity).transform);
        //}
        //else {
        //    GameController._Instance.UpdateCenterLevelString(ConstValue.LevelString.Level01);
        //    GameController._Instance.BuildMoveCube();
        //}

        
        //新方法
        foreach (var item in finalRoad.Keys)
        {
            switch (item)
            {
                case 0: //上
                    UpRoadInstance.Add(Instantiate(MapPointUp, UpRoad[UpRoad.Count - 1 - UpRoadInstance.Count] - new Vector3(0f, MapPointSize / 2f, 0f), Quaternion.identity).transform);
                    break;
                case 1: //下
                    DownRoadInstance.Add(Instantiate(MapPointDown, DownRoad[DownRoad.Count - 1 - DownRoadInstance.Count] + new Vector3(0f, MapPointSize / 2f, 0f), Quaternion.identity).transform);
                    break;
                case 2:  //左
                    LeftRoadInstance.Add(Instantiate(MapPointLeft, LeftRoad[LeftRoad.Count - 1 - LeftRoadInstance.Count] + new Vector3(MapPointSize / 2f, 0f, 0f), Quaternion.identity).transform);
                    break;
                case 3: //右
                    RightRoadInstance.Add(Instantiate(MapPointRight, RightRoad[RightRoad.Count - 1 - RightRoadInstance.Count] - new Vector3(MapPointSize / 2f, 0f, 0f), Quaternion.identity).transform);
                    break;
                default:
                    break;
            }
        }
    }

    //动画绑定事件
    public void BuildMovePoint() {
        GameController._Instance.BuildMoveCube();
    }

    //public void BuildMapPoint(int direction)
    //{
    //    switch (direction)
    //    {
    //        case 0:
    //            if (UpRoadInstance.Count < 3)
    //            {
    //                UpRoadInstance.Add(Instantiate(MapPointUp, UpRoad[UpRoad.Count - 1 - UpRoadInstance.Count] - new Vector3(0f, MapPointUp.transform.localScale.y / 2f, 0f), Quaternion.identity).transform);
    //            }
    //            break;
    //        case 1:
    //            if (DownRoadInstance.Count < 3)
    //            {
    //                DownRoadInstance.Add(Instantiate(MapPointDown, DownRoad[DownRoad.Count - 1 - DownRoadInstance.Count] + new Vector3(0f, MapPointDown.transform.localScale.y / 2f, 0f), Quaternion.identity).transform);
    //            }
    //            break;
    //        case 2:
    //            if (LeftRoadInstance.Count < 3)
    //            {
    //                LeftRoadInstance.Add(Instantiate(MapPointLeft, LeftRoad[LeftRoad.Count - 1 - LeftRoadInstance.Count] + new Vector3(MapPointLeft.transform.localScale.x / 2f, 0f, 0f), Quaternion.identity).transform);
    //            }
    //            break;
    //        case 3:
    //            if (RightRoadInstance.Count < 3)
    //            {
    //                RightRoadInstance.Add(Instantiate(MapPointRight, RightRoad[RightRoad.Count - 1 - RightRoadInstance.Count] - new Vector3(MapPointRight.transform.localScale.x / 2f, 0f, 0f), Quaternion.identity).transform);
    //            }
    //            break;
    //        default:
    //            break;
    //    }
    //    if (UpRoadInstance.Count == 3 && GameObject.FindGameObjectWithTag(ConstValue.TagName.MOVEPOINT) == null)
    //    {
    //        GameController._Instance.BuildMoveCube();
    //    }
    //}
    
}
