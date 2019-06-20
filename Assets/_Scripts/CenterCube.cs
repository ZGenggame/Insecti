using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class CenterCube : MonoBehaviour {

    public List<Transform> MoveCubes = new List<Transform>();

	public Queue<Vector3> Targets = new Queue<Vector3>();

    public bool IsRotating;

    public GameObject CalculateSmaple;

    public SpriteRenderer Shadow;

    private Animator m_Animator;



    [HideInInspector]
    public UnityEvent MapRoadFinishMove = new UnityEvent();

    void Awake() { 
        m_Animator = GetComponent<Animator>();
        CalculateSmaple = GameObject.Find("SignCenter"); //用来矫正移动完成后的点
        MapRoadFinishMove.AddListener(MapRoadHadFinishMove);
        m_Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

	void Start () {
		
	}

    public void Init(List<Transform> BeforeRoad,List<Transform> AfterRoad) {
        MoveCubes.Clear();
        for (int i = 0; i < BeforeRoad.Count; i++)
        {
            MoveCubes.Add(BeforeRoad[i]);
        }
        MoveCubes.Add(transform);
        for (int i = 0; i < AfterRoad.Count; i++)
        {
            MoveCubes.Add(AfterRoad[i]);
        }

    }


	void Update(){
        //if (GameController._Instance.CurrentGameState == GameController.GameState.GamePlaying)
        //{
        //    MapMove();
        //}
        //else if (GameController._Instance.CurrentGameState == GameController.GameState.GameCheck)
        //{
        //    CheckStateFinalMove();
        //}
        Shadow.transform.rotation = Quaternion.identity;
	}


    #region 处理最后的移动
    public int HaveFinshRoadMoveCount = -1;
    private int targetCount = -1;
  
    /// <summary>
    /// 舒适化移动参数
    /// </summary>
    /// <param name="count">需要向中心移动的路数</param>
    public void InitMove(int count) {
        Restart();
        targetCount = count;
    }

    public void Restart() { 
        HaveFinshRoadMoveCount = -1;
        targetCount = -1;
    }

    void MapRoadHadFinishMove() {
        HaveFinshRoadMoveCount++;
        if (targetCount != -1 && HaveFinshRoadMoveCount == targetCount)
        {
            //全部路径移动完成
            //StartCoroutine(ScenesController._Instance 
            //   .DestroyScenes(new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f))));
            StartCoroutine(ScenesController._Instance
               .DestroyScenes(ScenesController._Instance.currentLevelColor.BackGroundColor));
        }
    }

    //public void ToMove()
    //{
    //    if (moveTarget != null && m_CanMove)
    //    {
    //        transform.Translate(
    //            (moveTarget.GetChild(0).position - transform.position).normalized * Time.deltaTime * m_MoveSpeed,Space.World);
    //        if (Vector3.Distance(transform.position, moveTarget.GetChild(0).position) < 0.01f)
    //        {
    //            m_CanMove = false;
    //            transform.position = moveTarget.GetChild(0).position;
    //            Transform lastTarget = moveTarget;
    //            if (moveTarget != m_Road[m_Road.Count - 1])
    //            {
    //                UpdateTarget();
    //                Destroy(lastTarget.gameObject);
    //                m_CanMove = true;
    //            }
    //            else { 
    //                //移动完成生成下一个关卡
    //                Destroy(lastTarget.gameObject);
    //                GameController._Instance.GameNextLevel();
    //                Restart();
    //            }
                
    //        }
    //    }
    //}

    #endregion


    public void RotateSelf(int Direction) {
        if (Direction > 0)
        {
            StartCoroutine(StartRotate(90f));
        }
        else {
            StartCoroutine(StartRotate(-90f));
        }
        
    }

    private IEnumerator StartRotate(float angle) {
        BeginRotation(angle);
        float deltaAngle = angle / GameController._Instance.CenterMoveSpeed;
        float timer = 0f;
        while (timer < GameController._Instance.CenterMoveSpeed)
        {
            transform.Rotate(Vector3.back, deltaAngle*Time.deltaTime);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        EndRotation();
    }

    private void BeginRotation(float angle) {
        m_Animator.SetTrigger(ConstValue.AnimatorParameters.CenterCubeTriggerName_IsRotate);
        IsRotating = true;
        CalculateSmaple.transform.rotation = transform.rotation;
        CalculateSmaple.transform.Rotate(Vector3.back, angle);
        Shadow.DOKill(false);
        Shadow.DOColor(new Color(Shadow.color.r, Shadow.color.g, Shadow.color.b, 0f), 0.01f);
    }
    private void EndRotation()
    {
        IsRotating = false;
        transform.rotation = CalculateSmaple.transform.rotation ;
        Shadow.DOColor(new Color(Shadow.color.r, Shadow.color.g, Shadow.color.b, 1f), 0.1f);
    }


    public void Operation(int direction)
    {
        switch (direction)
        {
            case 0:
                if (transform.position.y < GameController._Instance.map.centerPoint.y + GameController._Instance.map.MapPointSize * GameController._Instance.map.MapPointSize)
                {
                    foreach (var item in MoveCubes)
                    {
                        item.position += new Vector3(0, transform.localScale.y, 0f);
                    }
                }
               
                break;
            case 1:

                if (transform.position.y > GameController._Instance.map.centerPoint.y - GameController._Instance.map.MapPointSize * GameController._Instance.map.MapPointSize)
                {
                    foreach (var item in MoveCubes)
                    {
                        item.position -= new Vector3(0, transform.localScale.y, 0f);
                    }
                }
               
                break;
            case 2:

                if (transform.position.x > GameController._Instance.map.centerPoint.x - GameController._Instance.map.MapPointSize * GameController._Instance.map.MapPointSize)
                {
                    foreach (var item in MoveCubes)
                    {
                        item.position += new Vector3(-transform.localScale.x, 0f, 0f);
                    }
                }

                break;
            case 3:

                if (transform.position.x < GameController._Instance.map.centerPoint.x + GameController._Instance.map.MapPointSize * GameController._Instance.map.MapPointSize)
                {
                    foreach (var item in MoveCubes)
                    {
                        item.position += new Vector3(transform.localScale.x, 0f, 0f);
                    }
                }

               
                break;
            default:
                break;
        }
    }

    public void MapMove()
    {

		if (Targets.Count != 0) {

			if (Vector3.Distance(transform.position,Targets.Peek()) > 0.1f
			    ) {
				transform.Translate((Targets.Peek() - transform.position).normalized * Time.deltaTime * GameController._Instance.CenterMoveSpeed);
			}else{
				transform.position = Targets.Dequeue();
			}
		}
    }
    public void CheckStateFinalMove() {
        if (Targets.Count != 0)
        {

            if (Vector3.Distance(transform.position, Targets.Peek()) > 0.1f
                )
            {
                transform.Translate((Targets.Peek() - transform.position).normalized * Time.deltaTime * GameController._Instance.CenterMoveSpeed);
            }
            else
            {
                transform.position = Targets.Dequeue();
            }
        }
    
    }

    //动画绑定事件
    public void ToBuildMapPoint() //建立地图网格
    {
        GameController._Instance.map.BuildMapPoint();
    }
    public void ToBuildMoveMapPoint() //建立移动方块
    {
        GameController._Instance.BuildMoveCube();
    }


    public void RightResultAnimation() 
    {
        m_Animator.SetTrigger(ConstValue.AnimatorParameters.CenterCubeTriggerName_IsRightResult);
    }
    public void ReStartAnimation()
    {
        m_Animator.SetTrigger(ConstValue.AnimatorParameters.CenterCubeTriggerName_IsRestart);
    }
}
