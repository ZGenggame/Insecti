using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour{
	[HideInInspector]public float leftBorder;
	[HideInInspector]public float rightBorder;
	[HideInInspector]public float topBorder;
	[HideInInspector]public float downBorder;
	[HideInInspector]public float width;
	[HideInInspector]public float height;

    [HideInInspector]
    public float OriginWidth;
    [HideInInspector]
    public float OriginHeight;

	public static Tools _Instance;

	void Awake(){
		_Instance = this;
		ScreenSize();
        OriginHeight = height;
        OriginWidth = width;
	}

	public void ScreenSize(){
		//the up right corner 右上角边缘点的世界坐标
		Vector3 cornerPos=Camera.main.ViewportToWorldPoint(new Vector3(1f,1f,
			Mathf.Abs(-Camera.main.transform.position.z)));

		//Camera位于游戏的绝对中心点
		leftBorder=Camera.main.transform.position.x-(cornerPos.x-Camera.main.transform.position.x);
		rightBorder=cornerPos.x;
		topBorder=cornerPos.y;
		downBorder=Camera.main.transform.position.y-(cornerPos.y-Camera.main.transform.position.y);

		width=rightBorder-leftBorder;
		height=topBorder-downBorder;
	}
	public Vector2 V3TOV2(Vector3 point){
		return new Vector2(point.x,point.y);
	}
}
	
