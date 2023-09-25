using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{

    private float lastPosX;
    private float lastAngleZ;
    private float rotationConstant;
    private Touch touch;

    private void Start() {
        rotationConstant = 180 / (float)Screen.width;
    }

    void Update()
    {
        if (Input.touchCount == 1) {
            touch = Input.GetTouch(0);
            switch (touch.phase) {
                case TouchPhase.Began:
                    lastPosX = touch.position.x;
                    lastAngleZ = GameManager.instance.currentLevel.transform.rotation.eulerAngles.z;
                    break;
                case TouchPhase.Moved:
                    if (lastPosX == touch.position.x)
                        break;
                    GameManager.instance.Rotate(lastAngleZ + (touch.position.x - lastPosX) * rotationConstant);
                    break;
            }
        }
        else {
            //GameManager.instance.Rotate(Vector2.up);
        }

        if (Input.GetKey(KeyCode.RightArrow))
            GameManager.instance.Rotate(++lastAngleZ);
        else if (Input.GetKey(KeyCode.LeftArrow))
            GameManager.instance.Rotate(--lastAngleZ);
        else {
            //lastPosX = touch.position.x;
            //lastAngleZ = GameManager.instance.currentLevel.transform.rotation.eulerAngles.z;
        }
    }

    //private Vector2 TouchDirection() => (touch.position.x - lastPosX) > 0f ? Vector2.right : Vector2.left ;

}
