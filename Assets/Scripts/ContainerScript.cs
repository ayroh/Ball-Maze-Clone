using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerScript : MonoBehaviour
{
    private int ballCount = 0;

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Ball")) {

            GameManager.instance.CollectBall(other.gameObject);

            if (GameManager.instance.currentBallSize == ++ballCount) {
                StartCoroutine(GameManager.instance.NextLevel());
                ballCount = 0;
            }
        }
    }

}
