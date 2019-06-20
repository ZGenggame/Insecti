using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBackGround : MonoBehaviour {
    public GameObject UIMainScreen;
	// Use this for initialization

    void Start() {
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y,1f);
        transform.localScale = new Vector3(Tools._Instance.width, Tools._Instance.height, 1f);
    }

	// Update is called once per frame
	void Update () {
        if (UIMainScreen != null)
        {
            gameObject.SetActive(UIMainScreen.activeSelf);
        }
	}
}
