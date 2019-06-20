using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoveCube : MonoBehaviour {
    public SpriteRenderer CubeBackGroundColor;
    public List<Text> Shows;//展示用的索引
    public GameObject CalculateSmaple;
    public Canvas WordUI;
    public List<GameObject> DeadEffectUseObj;
    public GameObject DeadEffectParticale;
    

    public bool IsArrive;

    private Animator m_Animator;


    public enum MoveDirection { 
        上,
        下,
        左,
        右
    }
    private MoveDirection currectDirection;

    public AnimationCurve MoveCurve;
    public float MoveCurveUseTime;

    private bool isChecked;

    void Awake() {
        CalculateSmaple = Instantiate(GameObject.Find("SignPoint")); //用来矫正移动完成后的点
        CalculateSmaple.transform.position = transform.position;
        CalculateSmaple.transform.rotation = transform.rotation;
        m_Animator = GetComponent<Animator>();
    }

    void OnDestroy()
    {
        Destroy(CalculateSmaple.gameObject);
    }

    public void AfterAnimationToListenBirth()
    {
       // AudioContorller._Instance.PlayAudioOneTimeNotStopLast(5,0.46f);
        if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.PlayerInGameSuccessCount) == 0
            && PlayerPrefs.GetString("LookedTeach") != "true"
            ) //如果玩家第一次开始玩
        {
            PlayerPrefs.SetString("LookedTeach", "true");
            UIController._Instance.ShowTeachPanel();
        }
    }

    public void AfterAnimationToMove() {
        
       Move();
    }
    //出生点判定
    public void Init() {
        WordUI.gameObject.SetActive(true);
        List<Vector3> firstPos = new List<Vector3>();
        if (GameController._Instance.map.UpRoad.Count != 0)firstPos.Add(GameController._Instance.map.UpRoad[0]); // 0
        if (GameController._Instance.map.DownRoad.Count != 0) firstPos.Add(GameController._Instance.map.DownRoad[0]); // 1
        if (GameController._Instance.map.LeftRoad.Count != 0) firstPos.Add(GameController._Instance.map.LeftRoad[0]); // 2
        if (GameController._Instance.map.RightRoad.Count != 0) firstPos.Add(GameController._Instance.map.RightRoad[0]); // 3

        
        //移除对玩家要求比较高的边界值
        //if (GameController._Instance.CenterCube.transform.position.x 
        //    > GameController._Instance.map.centerPoint.x + GameController._Instance.CenterCube.transform.localScale.x * 2f )
        //{
        //    firstPos.RemoveAt(3);
        //}

        //if (GameController._Instance.CenterCube.transform.position.x
        //    < GameController._Instance.map.centerPoint.x - GameController._Instance.CenterCube.transform.localScale.x * 2f)
        //{
        //    firstPos.RemoveAt(2);
        //}

        //if (GameController._Instance.CenterCube.transform.position.y
        //    < GameController._Instance.map.centerPoint.y - GameController._Instance.CenterCube.transform.localScale.x * 2f)
        //{
        //    firstPos.RemoveAt(1);
        //}

        //if (GameController._Instance.CenterCube.transform.position.y
        //    > GameController._Instance.map.centerPoint.y + GameController._Instance.CenterCube.transform.localScale.x * 2f)
        //{
        //    firstPos.RemoveAt(0);
        //}

        int index = Random.Range(0, firstPos.Count);
        if (firstPos.Count > 1)
        {
            while (GameController._Instance.IsCurrentInRoad(firstPos[index]))
            {
                index = Random.Range(0, firstPos.Count);
            }
        }


        for (int i = 0; i < 1; i++)
        {
            if (GameController._Instance.map.UpRoad.Contains(firstPos[index]))
            {
                currectDirection = MoveDirection.下;
                break;
            }

            if (GameController._Instance.map.DownRoad.Contains(firstPos[index]))
            {
                currectDirection = MoveDirection.上;
                break;
            }
            if (GameController._Instance.map.LeftRoad.Contains(firstPos[index]))
            {
                currectDirection = MoveDirection.右;
                break;
            } if (GameController._Instance.map.RightRoad.Contains(firstPos[index]))
            {
                currectDirection = MoveDirection.左;
                break;
            }
        }
        transform.position = firstPos[index];
        //transform.DOScale(Vector3.one * 1.5f, 0.8f).OnComplete(Move);
		//Move();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ConstValue.TagName.PLAYER
            && GameController._Instance.CurrentGameState == GameController.GameState.GamePlaying
            && !isChecked
            )
        {
            isChecked = true;
            GameController._Instance.CenterCube.Targets.Clear();
            GameController._Instance.CenterCube.Targets.Enqueue(CalculateSmaple.transform.position);
            GameController._Instance.GameCheck();
        }
    }

    //void FixedUpdate()
    //{
    //    if (Vector3.Distance(transform.position, GameController._Instance.CenterCube.transform.position) < 0.4f
    //            && GameController._Instance.CurrentGameState == GameController.GameState.GamePlaying
    //            && !isChecked)
    //    {
    //        isChecked = true;
    //        GameController._Instance.CenterCube.Targets.Clear();
    //        GameController._Instance.CenterCube.Targets.Enqueue(CalculateSmaple.transform.position);
    //        GameController._Instance.GameCheck();
    //    }
    //}

    //void Update() {
        
    //}

    void Move() {

        CalculateSmaple.transform.position = transform.position;
        CalculateSmaple.transform.rotation = transform.rotation;

        Vector3 point = Vector3.zero;
        Vector3 axis = Vector3.zero;
        float angle = 0f;
        angle = 90f;

        switch (currectDirection)
        {
            case MoveDirection.上:
				point = transform.position + new Vector3(transform.localScale.x / 2f, transform.localScale.x / 2f, 0f);
                axis = Vector3.back;
                break;
            case MoveDirection.下:
				point = transform.position + new Vector3(-transform.localScale.x / 2f, -transform.localScale.x / 2f, 0f);
				axis = Vector3.back;
				break;
            case MoveDirection.左:
				point = transform.position + new Vector3(-transform.localScale.x / 2f, -transform.localScale.x / 2f, 0f);
				axis = Vector3.back;
                angle = -90f;
				break;
            case MoveDirection.右:
				point = transform.position + new Vector3(transform.localScale.x / 2f, -transform.localScale.x / 2f, 0f);
				axis = Vector3.back;
				break;
            default:
                break;
        }
        
        CalculateSmaple.transform.RotateAround(point, axis, angle);

        int index = -1;
        //纠正数值，容差使用
        for (int i = 0; i < GameController._Instance.map.RoadLength; i++)
        {
            if (GameController._Instance.map.UpRoad.Count !=0 
                &&  Vector3.Distance(GameController._Instance.map.UpRoad[i], CalculateSmaple.transform.position) < 0.001f)
            {
                CalculateSmaple.transform.position = GameController._Instance.map.UpRoad[i];
                index = i;
                break;
            }
            if (GameController._Instance.map.DownRoad.Count != 0
                && Vector3.Distance(GameController._Instance.map.DownRoad[i], CalculateSmaple.transform.position) < 0.001f)
            {
                CalculateSmaple.transform.position = GameController._Instance.map.DownRoad[i];
                index = i;
                break;
            }
            if (GameController._Instance.map.LeftRoad.Count != 0
                && Vector3.Distance(GameController._Instance.map.LeftRoad[i], CalculateSmaple.transform.position) < 0.001f)
            {
                CalculateSmaple.transform.position = GameController._Instance.map.LeftRoad[i];
                index = i;
                break;
            }
            if (GameController._Instance.map.RightRoad.Count != 0
                && Vector3.Distance(GameController._Instance.map.RightRoad[i], CalculateSmaple.transform.position) < 0.001f)
            {
                CalculateSmaple.transform.position = GameController._Instance.map.RightRoad[i];
                index = i;
                break;
            }
            if (i == GameController._Instance.map.RoadLength - 1)
            {
                CalculateSmaple.transform.position = GameController._Instance.map.centerPoint;
                
            }
        }

        StartCoroutine(StartRotate(CalculateSmaple.transform,point,axis,angle,GameController._Instance.MoveCubeMoveSpeed));
    }

    IEnumerator StartRotate(Transform final,Vector3 point,Vector3 axis,float angle,float useTime) {
        float delta = angle / useTime;
        float timer = 0f;

        #region 匀速运动方式
        while (timer < useTime)
        {
            transform.RotateAround(point, axis, delta * Time.deltaTime);
           
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
       
        #endregion

        if (!IsArrive)
        {
            if (currectDirection == MoveDirection.左 || currectDirection == MoveDirection.右)
            {
                if (Vector3.Angle(transform.right, Vector3.up) > 45f && Vector3.Angle(transform.right, Vector3.up) < 165f)
                {
                    m_Animator.SetTrigger(ConstValue.AnimatorParameters.MoveCubeTriggerName_IsArrive);
                }
                else
                {
                    m_Animator.SetTrigger(ConstValue.AnimatorParameters.MoveCubeTriggerName_IsArriveY);
                }

                transform.position = new Vector3(transform.position.x, transform.position.y - 0.075f, transform.position.z);
            }
            else {

                if (currectDirection == MoveDirection.下)
                {
                    if (Vector3.Angle(transform.right, Vector3.up) > 45f && Vector3.Angle(transform.right, Vector3.up) < 165f)
                    {
                        m_Animator.SetTrigger(ConstValue.AnimatorParameters.MoveCubeTriggerName_IsArriveY);

                    }
                    else
                    {
                        m_Animator.SetTrigger(ConstValue.AnimatorParameters.MoveCubeTriggerName_IsArrive);
                    }
                    transform.position = new Vector3(transform.position.x - 0.075f, transform.position.y, transform.position.z);

                }
                else if (currectDirection == MoveDirection.上)
                {
                    if (Vector3.Angle(transform.right, Vector3.up) > 45f && Vector3.Angle(transform.right, Vector3.up) < 165f)
                    {
                        m_Animator.SetTrigger(ConstValue.AnimatorParameters.MoveCubeTriggerName_IsArriveY);

                    }
                    else
                    {
                        m_Animator.SetTrigger(ConstValue.AnimatorParameters.MoveCubeTriggerName_IsArrive);
                    }
                    transform.position = new Vector3(transform.position.x + 0.075f, transform.position.y, transform.position.z);
                    
                }
            
            }
           
            //播放音效
            if (GameController._Instance.CurrentGameState == GameController.GameState.GamePlaying)
            {
                AudioContorller._Instance.PlayAudioOneTimeNotStopLast(2,0.35f);
                //AudioContorller._Instance.PlayAudioOneTimeAtPoint(2, 0.5f);
            }
            IsArrive = true;
            transform.DOMove(final.position, (1f / 30f) * 2f);

        }
        

        #region 按预设曲线进行移动的方式
        //float curveDelta = (MoveCurveUseTime / useTime) * Time.deltaTime;
        //float curveTimer = 0f;
        //while (timer < useTime)
        //{
        //    transform.RotateAround(point, axis, (MoveCurve.Evaluate(curveTimer) - MoveCurve.Evaluate(curveTimer - curveDelta)) * angle);
        //    timer += Time.deltaTime;
        //    curveTimer += curveDelta;


        //    if (Vector3.Distance(final.position, transform.position) < 0.008f)
        //    {
        //        if (!IsArrive)
        //        {
        //            m_Animator.SetTrigger(ConstValue.AnimatorParameters.MoveCubeTriggerName_IsArrive);
        //            IsArrive = true;
        //        }
        //    }
        //    yield return new WaitForEndOfFrame();
        //}
        #endregion
        yield return new WaitForSeconds((1f / 30f) * 2f);
        transform.DOKill(false);
        transform.position = final.position;
        transform.rotation = final.rotation;

        for (int i = 0; i < GameController._Instance.map.RoadLength; i++)
        {
            if (GameController._Instance.map.UpRoad.Contains(transform.position) 
                && GameController._Instance.map.UpRoad.IndexOf(transform.position) == GameController._Instance.map.RoadLength - 1)
            {
                GameController._Instance.ToBuildMovePoint.Invoke();
                break;
            }
            if (GameController._Instance.map.DownRoad.Contains(transform.position)
                && GameController._Instance.map.DownRoad.IndexOf(transform.position) == GameController._Instance.map.RoadLength - 1)
            {
                GameController._Instance.ToBuildMovePoint.Invoke();
                break;
            }
            if (GameController._Instance.map.LeftRoad.Contains(transform.position)
                && GameController._Instance.map.LeftRoad.IndexOf(transform.position) == GameController._Instance.map.RoadLength - 1)
            {
                GameController._Instance.ToBuildMovePoint.Invoke();
                break;
            }
            if (GameController._Instance.map.RightRoad.Contains(transform.position)
                && GameController._Instance.map.RightRoad.IndexOf(transform.position) == GameController._Instance.map.RoadLength - 1)
            {
                GameController._Instance.ToBuildMovePoint.Invoke();
                break;
            }

        }
        yield return new WaitForSeconds(GameController._Instance.MoveCubeWaitTime);

        if (GameController._Instance.CurrentGameState == GameController.GameState.GamePlaying && !isChecked)
        {
            Move();
            IsArrive = false;
        }
        else {
            IsArrive = true;
        }

        ////游戏结束判断
        //if (GameController._Instance.CurrentGameState == GameController.GameState.GamePlaying)
        //{
        //    //旋转结束调整方向
        //    if (transform.position.x != GameController._Instance.CenterCube.transform.position.x && //左右移动队列
        //        transform.position.y == GameController._Instance.CenterCube.transform.position.y
        //        )
        //    {
        //        if (transform.position.x < GameController._Instance.CenterCube.transform.position.x)
        //        {
        //            currectDirection = MoveDirection.右;

        //        }
        //        else
        //        {
        //            currectDirection = MoveDirection.左;
        //        }

        //    }
        //    else if (transform.position.x == GameController._Instance.CenterCube.transform.position.x &&
        //             transform.position.y != GameController._Instance.CenterCube.transform.position.y
        //        )
        //    {
        //        if (transform.position.y < GameController._Instance.CenterCube.transform.position.y)
        //        {
        //            currectDirection = MoveDirection.上;
        //        }
        //        else
        //        {
        //            currectDirection = MoveDirection.下;
        //        }
        //    }
        //    else
        //    {
        //        //移动到中心点时修改方向
        //        //if (transform.position.x == GameController._Instance.map.centerPoint.x &&
        //        //    transform.position.y == GameController._Instance.map.centerPoint.y)
        //        //{
        //        //    if (transform.position.x != GameController._Instance.CenterCube.transform.position.x && //左右移动队列
        //        //         transform.position.y == GameController._Instance.CenterCube.transform.position.y)
        //        //    {
        //        //        if (transform.position.x < GameController._Instance.CenterCube.transform.position.x)
        //        //        {
        //        //            currectDirection = MoveDirection.右;

        //        //        }
        //        //        else
        //        //        {
        //        //            currectDirection = MoveDirection.左;
        //        //        }

        //        //    }
        //        //    else if (transform.position.x == GameController._Instance.CenterCube.transform.position.x &&
        //        //             transform.position.y != GameController._Instance.CenterCube.transform.position.y
        //        //        )
        //        //    {
        //        //        if (transform.position.y < GameController._Instance.CenterCube.transform.position.x)
        //        //        {
        //        //            currectDirection = MoveDirection.上;
        //        //        }
        //        //        else
        //        //        {
        //        //            currectDirection = MoveDirection.下;
        //        //        }
        //        //    }

        //        //} else 


        //        if (transform.position.x == GameController._Instance.map.centerPoint.x)
        //        {
        //            if (transform.position.y > GameController._Instance.map.centerPoint.y)
        //            {
        //                currectDirection = MoveDirection.下;
        //            }
        //            else
        //            {
        //                currectDirection = MoveDirection.上;
        //            }

        //        }
        //        else if (transform.position.y == GameController._Instance.map.centerPoint.y)
        //        {
        //            if (transform.position.x > GameController._Instance.map.centerPoint.x)
        //            {
        //                currectDirection = MoveDirection.左;
        //            }
        //            else
        //            {
        //                currectDirection = MoveDirection.右;
        //            }
        //        }
        //    }
        //    Move();
        //}
        //else { 
        
        
        //}

    }


    //Effect

    public void DeadEffect() {

        Text showText = transform.GetComponent<Check>().GetShowsText(); //显示文字
        showText.GetComponent<WordEvent>().ToDead();
        showText.DOColor(Color.white, (1f / 30f) * 5f);
        foreach (var item in DeadEffectUseObj)
        {
            item.transform.DOScale(Vector3.one * (1f / 4f), 0.23f);
            item.transform.DOMove(showText.transform.position - transform.position, 0.23f);
            item.GetComponent<SpriteRenderer>().DOColor(new Color(Color.white.r, Color.white.g, Color.white.b, 0f), 0.23f);
        }
        //showText.transform.DOScale(Vector3.zero, 0.23f);
        //foreach (var item in DeadEffectUseObj)
        //{
        //    item.SetActive(false);
        //}
       
    }
    

    public void AnimatorSetBool(bool value) { 
    
    }

}
