using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoint : MonoBehaviour {

    public GameObject ImageInstance;

    public bool CanMove;
    private Transform moveTarget;
    private float moveSpeed;

    public void Init(Transform Target,float MoveSpeed) {
        moveTarget = Target;
        moveSpeed = MoveSpeed;
    }


    public void ToBuildMapPoint() {
        if (ImageInstance != null )
        {
            ImageInstance.SetActive(true);
        }
        
    }

    void Update() {
        ToMove();
    }

    void FixedUpdate() {
    }


    public void ToMove() {
        if (moveTarget != null && CanMove)
        {

            if (moveTarget.tag == ConstValue.TagName.PLAYER)
            {
                transform.Translate(
                 (moveTarget.position - transform.GetChild(0).position).normalized * Time.deltaTime * moveSpeed);
                if (Vector3.Distance(transform.GetChild(0).position, moveTarget.position) < 0.2f)
                {
                    CanMove = false;
                    GameController._Instance.CenterCube.MapRoadFinishMove.Invoke();
                    Destroy(transform.gameObject); //摧毁本体
                }
            }else 
            {
                transform.Translate(
                (moveTarget.position - transform.position).normalized * Time.deltaTime * moveSpeed);
                if (Vector3.Distance(transform.position, moveTarget.position) < 0.2f)
                {
                    
                    transform.position = moveTarget.position;
                    CanMove = false;
                    moveTarget.GetComponent<MapPoint>().CanMove = true;
                    moveTarget = null;
                    Destroy(transform.gameObject); //摧毁本体
                }
            
            }

            
        }
    }
}
