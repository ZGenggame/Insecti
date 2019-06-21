using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float MoveSpeed;

    private List<Vector3> points = new List<Vector3>();

	void Start () {
		
	}
	
	void Update () {
	}

    void LateUpdate() {
        Move();
    }

    void FixedUpdate() {
    }

    void Move() {
        if (GameController._Instance != null)
        {
            Vector3 moveTarget = GetMoveTarget();
            if (Vector3.Distance(moveTarget, transform.position) < 0.01f)
            {
                transform.position = moveTarget;
            }

            transform.position = Vector3.Lerp(
                transform.position,
                moveTarget
           , MoveSpeed * Time.deltaTime);

        }
    }

    Vector3 GetMoveTarget() {
        GameObject gbj = GameObject.FindGameObjectWithTag(ConstValue.TagName.PLAYER);
        points.Clear();
        FindRoadFarstPoint(points, GameController._Instance.map.UpRoadInstance);
        FindRoadFarstPoint(points, GameController._Instance.map.DownRoadInstance);
        FindRoadFarstPoint(points, GameController._Instance.map.LeftRoadInstance);
        FindRoadFarstPoint(points, GameController._Instance.map.RightRoadInstance);
        if (gbj != null)
        {
            points.Add(gbj.transform.position);   
        }
        Vector3 target = new Vector3(0f,0f,transform.position.z);
        for (int i = 0; i < points.Count; i++)
        {
            target.x += points[i].x;
            target.y += points[i].y;
        }
        if (points.Count != 0)
        {
            target.x = target.x / points.Count;
            target.y = target.y / points.Count;
        }
        else 
        {
            target.x = target.y = 0f;
        }
        

        return target;
    }

    void FindRoadFarstPoint(List<Vector3> targets,List<Transform> road) {
        if (GameController._Instance != null && road.Count != 0)
        {
            if (road[road.Count - 1] != null)
            {
                points.Add(road[road.Count - 1].transform.GetChild(0).position);
            }
        }
    }

    public void CorrectPosition() {
        transform.position =
            new Vector3(
                GameController._Instance.CenterCube.transform.position.x,
                GameController._Instance.CenterCube.transform.position.y,
                transform.position.z);
    }
}
