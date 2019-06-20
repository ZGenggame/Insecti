 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Events;

public class GameController : MonoBehaviour {

    public static GameController _Instance;
    public GameObject MovingCube;
    public BuildMap map;
    public GameObject CenterCubeInstance;
    [HideInInspector]
    public CenterCube CenterCube;

    [Header("移动方块一个单位所用的时间")]
    public float MoveCubeMoveSpeed;
    [Header("移动方块的等待时间")]
    public float MoveCubeWaitTime;
	[Header("中心方块移动速度 第二版代表旋转一次用的时间")]
	public float CenterMoveSpeed;
   

    private GameObject currentMoveCube;
    private Queue<GameObject> m_MoveCubes = new Queue<GameObject>();

    private int GameRightRoundCount = 0; //

    public float GameTimeLineSpeed;

    public enum GameState {
        GamePrepare,
        GamePause,
        GamePlaying,
        GameCheck,
        GameLeveling,
        GameOver
    }
    public GameState CurrentGameState;


    public UnityEvent ToBuildMovePoint = new UnityEvent();

    void Awake() {
        
        _Instance = this;
        ToBuildMovePoint.AddListener(BuildMoveCube);
    }

    void Start() { 
    }

    public void BuildMoveCube() {

        GameObject gbj = Instantiate(MovingCube);
        if (m_MoveCubes.Count == 0)
        {
            m_MoveCubes.Enqueue(gbj);
            currentMoveCube = m_MoveCubes.Peek();
        }
        else
        {
            m_MoveCubes.Enqueue(gbj);
        }
        MoveCube mc = gbj.GetComponent<MoveCube>();
        mc.Init();
        //mc.CubeBackGroundColor.color = new Color(
        //    ScenesController._Instance.currentLevelColor.BackGroundColor.r,
        //    ScenesController._Instance.currentLevelColor.BackGroundColor.g,
        //    ScenesController._Instance.currentLevelColor.BackGroundColor.b,
        //    mc.CubeBackGroundColor.color.a
        //    );
        mc.CubeBackGroundColor.sprite = ScenesController._Instance.currentLevelColor.MoveCubeImage;
        Check c = gbj.GetComponent<Check>();
        c.UpdateNextText(CenterCube.GetComponent<Check>().innerCheck);

       
        GameStart();
        //currentMoveCube = Instantiate(MovingCube);
        //currentMoveCube.GetComponent<MoveCube>().Init();
        //Check c = currentMoveCube.GetComponent<Check>();
        //c.UpdateNextText(CenterCube.GetComponent<Check>().innerCheck);
        //GameStart();
    }
    
    //更新下一个Level需要用的字符串
    public void UpdateCenterLevelString(string content) {
        CenterCube.GetComponent<Check>().InitText(
            content[0].ToString(),
            content[1].ToString(),
            content[2].ToString(),
            content[3].ToString());
    }

    public void UpdateCenterLevelString(List<ConstValue.Character> words)
    {
        CenterCube.GetComponent<Check>().InitText(words);
    }

	// Update is called once per frame
	void Update () {
        if (CurrentGameState == GameState.GamePlaying)
        {
            //            Vector2 clickPoint = Vector2.zero;
            //#if UNITY_EDITOR
            //            if (Input.GetMouseButtonDown(1))
            //            {
            //                clickPoint = Input.mousePosition;
            //            }


            //#else
            //        if (Input.touchCount == 1) {
            //            clickPoint = Input.touches[0].position;
            //        }
            //#endif
            //            Ray ray = Camera.main.ScreenPointToRay(clickPoint);
            //            RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction,100,1 << LayerMask.NameToLayer(ConstValue.LayerName.MAP));
            //            if (hit2D.collider != null)
            //            {
            //                Operation(hit2D);
            //            }

            if ( PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.PlayerInGameSuccessCount) > 0  //玩家顺利通过第一关
                && !UIController._Instance.UpdateGameTimeLine(-GameTimeLineSpeed*Time.deltaTime))
            {
                GameOver();
            }

            //第二种玩法
            if (Input.GetMouseButtonDown(0) && !CenterCube.IsRotating)
            {
                
                Vector2 clickPoint = Vector2.zero;
#if UNITY_EDITOR
                clickPoint = Input.mousePosition;
#else
                 if (Input.touchCount == 1) {
                       clickPoint = Input.touches[0].position;
                }
#endif
                clickPoint = Camera.main.ScreenToWorldPoint(clickPoint);

                if (clickPoint.y < Tools._Instance.topBorder - Tools._Instance.height * (1f/7f))
                {
                    if (clickPoint.x >= Tools._Instance.leftBorder + Tools._Instance.width / 2f)
                    {
                        CenterCube.RotateSelf(+1);
                    }
                    else
                    {
                        CenterCube.RotateSelf(-1);
                    }
                    AudioContorller._Instance.PlayAudioOneTimeNotStopLast(9, 0.65f); //音效
                }

            }
        }

        if (CurrentGameState == GameState.GameCheck && !CenterCube.IsRotating && currentMoveCube != null)
        {
            if ( currentMoveCube.GetComponent<MoveCube>().IsArrive)
            {
                Check center = CenterCube.GetComponent<Check>();
                if (!IsRightResult(CenterCube.transform, currentMoveCube.transform))
                {
                    //GameOver();
                    GameReLevel();
                    GameStart();
                    AudioContorller._Instance.PlayAudioOneTimeNotStopLast(4,1f); //音效
                }
                else 
                {
                    UIController._Instance.PlayerCurrentScore++;
                    AudioContorller._Instance.PlayAudioOneTimeNotStopLast(3,1f); //音效

                    if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.PlayerBestScore) == 0)
                    {
                        PlayerPrefs.SetInt(ConstValue.XmlDataKeyName.PlayerBestScore, UIController._Instance.PlayerCurrentScore);
                    }
                    else if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.PlayerBestScore) < UIController._Instance.PlayerCurrentScore)
                    {
                        PlayerPrefs.SetInt(ConstValue.XmlDataKeyName.PlayerBestScore, UIController._Instance.PlayerCurrentScore);
                    }
                    UIController._Instance.UpdateGameTimeLine(GameTimeLineSpeed * 2f);
                    //for (int i = 0; i < center.innerCheck.Count; i++)
                    //{
                    //    if (!center.innerCheck[i].HaveCheck)
                    //    {
                    //        break;
                    //    }
                    //    if (i == center.innerCheck.Count - 1) //四个数字全队
                    //    {
                    //        //LevelUp
                    //        CurrentGameState = GameState.GameLeveling; //游戏进入升级状态
                    //        GameRightRoundCount++; //这个数值需要持久化
                    //        GameRightRoundCount %= 3;
                    //        //UpdateCenterLevelString(ConstValue.GetInstance().levelStrings[GameRightRoundCount]);
                    //        //BuildMoveCube();

                    //        //全部正确之后初始化新的背景 和 文字 在此处插入对背景和移动方块的颜色控制
                    //        StartCoroutine(ScenesController._Instance 
                    //            .DestroyScenes(new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f))));
                    //    }
                    //}
                    if (center.IsFinishLevel())
                    {
                        //LevelUp
                        CurrentGameState = GameState.GameLeveling; //游戏进入升级状态
                        if (!PlayerData.GetInstance().IsAllStringRight(ConstValue.GetInstance().levelStrings[0])) //第一个词条没有收集完成
                        {
                            if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.PlayerInGameSuccessCount) != 0)
                            {
                                GameRightRoundCount = PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.PlayerInGameSuccessCount);
                            }
                            GameRightRoundCount++; //这个数值需要持久化
                            PlayerPrefs.SetInt(ConstValue.XmlDataKeyName.PlayerInGameSuccessCount, GameRightRoundCount); //更新用户数据
                        }
                      
                        //UpdateCenterLevelString(ConstValue.GetInstance().levelStrings[GameRightRoundCount]);
                        //BuildMoveCube();

                        //全部正确之后初始化新的背景 和 文字 在此处插入对背景和移动方块的颜色控制
                        //StartCoroutine(ScenesController._Instance
                        //   .DestroyScenes(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f))));
                        center.GetComponent<Check>().RightWordEffect();
                        StartCoroutine(StartNextLevel(0.45f));
                    }
                    else {
                        center.GetComponent<Check>().RightWordEffect();
                        GameStart();
                    }

                }
            }
        }
	}

    public void Operation(RaycastHit2D result) {
		CenterCube.Targets.Clear();
		if (result.transform.tag == ConstValue.TagName.MAPPOINT) {
			if (CenterCube.transform.position.x == result.transform.position.x 
			    || CenterCube.transform.position.y == result.transform.position.y ) {

				CenterCube.Targets.Enqueue(result.transform.position);

			}else{
				CenterCube.Targets.Enqueue(map.centerPoint);
				CenterCube.Targets.Enqueue(result.transform.position);
			}
		}
	}

    public void Operation() { 
    
    }
    IEnumerator StartNextLevel(float DelayTime)
    {
        //GameObject[] gbjs = GameObject.FindGameObjectsWithTag(ConstValue.TagName.MOVEPOINT);
        //foreach (var item in gbjs)
        //{
        //    Destroy(item);
        //}
        yield return new WaitForSeconds(DelayTime);
        ScenesController._Instance.PrepareNextLevel();
        UIController._Instance.UpdateGameTimeLine(GameTimeLineSpeed * 3.5f);
    }


    public void GameStart() {
        CurrentGameState = GameState.GamePlaying;
    }
    public void GameOver() { //游戏结束
        AudioContorller._Instance.PlayAudioOneTime(10);
        CurrentGameState = GameState.GameOver;
        Time.timeScale = 0f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //UIController._Instance.ShowGameOverPanel();
        UIController._Instance.ShowLookAds();
    }

    public void GameRestart()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //CurrentGameState = GameState.GamePrepare;
        ScenesController._Instance.ToDestroyGamePlayObject();
        ScenesController._Instance.ChooseLevelColor();
        Time.timeScale = 1f;
        StartCoroutine(ScenesController._Instance
               .DestroyScenes(ScenesController._Instance.currentLevelColor.BackGroundColor));

    }

    private GameState lastGameState; //记录暂停前的游戏状态
    public void GamePause() {
        //if (CurrentGameState == GameState.GamePlaying)
        //{
            
        //}
        lastGameState = CurrentGameState;
        CurrentGameState = GameState.GamePause;
        Time.timeScale = 0f;
        
    }

    public void GamePauseBack()
    {
        CurrentGameState = lastGameState;
        Time.timeScale = 1f;
    }

   
    public void GameNextLevel() //成功之后升级 abolish
    {
        Camera.main.GetComponent<CameraController>().CorrectPosition();
        int wordCount = Random.Range(2, 5);
        map.InitMap(CenterCube.transform.position, wordCount, false); //多少个字多少个轨道
        CenterCube.ReStartAnimation();
        UpdateCenterLevelString(ConstValue.GetInstance().GetCharacter(wordCount));
    }

    public void GameBackToHome() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }


    public void GameReLevel() //赋予一个新的中心字符
    {
        GameObject obj = m_MoveCubes.Dequeue();
        obj.GetComponent<MoveCube>().DeadEffect();
        Destroy(obj,0.21f);
        currentMoveCube = m_MoveCubes.Peek();
        //UpdateCenterLevelString(ConstValue.GetInstance().GetCharacter(CenterCube.GetComponent<Check>().innerCheck.Count));
        //BuildMoveCube();
       //UpdateCenterLevelString(ConstValue.GetInstance().levelStrings[Random.Range(0,3)]);
        
    }


    public void GameCheck()
    {
        CurrentGameState = GameState.GameCheck;
    }

    public bool IsRightResult(Transform center,Transform move) {
        Check c = center.GetComponent<Check>();
        Check m = move.GetComponent<Check>();
        Text m_Position = null;
        for (int i = 0; i < m.Shows.Count; i++)
        {
            if (m.Shows[i].text != "" ) //唯一一个数字 text != null 判断不成立 文字相同
            {
                m_Position = m.Shows[i];
                break;
            }
        }

        //foreach (var item in c.Shows)
        //{

        //    if (item.text == m_Position.text && item.transform.position == m_Position.transform.position)
        //    {
        //        //播放正确结果的提示动画
        //        CenterCube.RightResultAnimation();
        //        Destroy(move.gameObject);
        //        c.ShowText(item, m_Position); //更改显示的数字
        //        //center.DOShakeScale(0.7f, 0.5f);
        //        if (!c.IsFinishLevel())
        //        {
        //            BuildMoveCube();
        //        }
               
        //        return true; //正确结果
        //    }
            
        //}

        for (int i = 0; i < c.Shows.Count; i++)
        {

            //Debug.Log((c.Shows[i].text == m_Position.text) + "==1");
            //Debug.Log(c.Shows[i].transform.position.ToString("f5") + "==" + m_Position.transform.position.ToString("f5") + "==2");
            //Debug.Log(c.Shows[i].transform.position == m_Position.transform.position);

            if (c.Shows[i].text == m_Position.text 
                && (c.Shows[i].transform.position.x * m_Position.transform.position.x > 0) //Bug?
                && (c.Shows[i].transform.position.y * m_Position.transform.position.y > 0)
                && !c.innerCheck[i].HaveCheck //这个文字是未被检查过的
                ) //文本的位置相同
            {

                c.ToUpdatePlayerData(i); //更新用户数据
               
                //播放正确结果的提示动画
                CenterCube.RightResultAnimation();
                Destroy(m_MoveCubes.Dequeue()); //结果正确销毁当前移动块
                currentMoveCube = m_MoveCubes.Peek();

                c.ShowText(c.Shows[i], m_Position, i); //中心方块更改显示的数字

                if (!c.IsFinishLevel()) //换字逻辑
                {
                    if (!c.IsHadRepeatText(c.innerCheck)
                        && c.AlreadyRightText(currentMoveCube.GetComponent<Check>().GetShowsText(),c.innerCheck))
                    {
                        //currentMoveCube.GetComponent<Check>().UpdateNextText(c.innerCheck);
                        StartCoroutine(StartChangeCurrentShowText(0.35f, c, currentMoveCube.GetComponent<Check>())); //做一个延时处理
                    }
                }
                return true; //正确结果
            }

        }

        return false;
    }


    IEnumerator StartChangeCurrentShowText(float useTime,Check center,Check show) {
        Time.timeScale = 0.99999f; //旋转问题还没有解决
        //Time.timeScale = 1f; //旋转问题还没有解决
        float deltaTime = 0f;

        currentMoveCube.GetComponent<Check>().GetShowsText()
                        .transform.DORotate(new Vector3(90f, 0f, 0f), useTime / 2f).SetUpdate(true)
                        .OnComplete(() => RotateBack(useTime / 2f));

        while (deltaTime < useTime / 2f)
        {
            deltaTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        show.UpdateNextText(center.innerCheck, show.GetShowsText());

        while (deltaTime < useTime)
        {
            deltaTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = 1f;
    }

    void RotateBack(float time) {
        currentMoveCube.GetComponent<Check>().GetShowsText()
            .transform.DORotate(new Vector3(0f, 0f, 0f), time).SetUpdate(true);
    }
    
    //判断给定的位置是否与当前的CurrentMoveCube在同一个轨道内，以防撞车
    public bool IsCurrentInRoad(Vector3 position) {

        for (int i = 0; i < map.RoadLength; i++)
        {
            if (map.UpRoad.Contains(position) 
                && map.UpRoad.Contains(currentMoveCube.transform.position))
            {
                return true;
            }
            if (map.DownRoad.Contains(position) 
                && map.DownRoad.Contains(currentMoveCube.transform.position))
            {
                return true;
            } if (map.LeftRoad.Contains(position) 
                && map.LeftRoad.Contains(currentMoveCube.transform.position))
            {
                return true;
            } if (map.RightRoad.Contains(position)
                && map.RightRoad.Contains(currentMoveCube.transform.position))
            {
                return true;
            }
        }
        return false;
    }
}
