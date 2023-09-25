using System.Collections;
using UnityEngine.Pool;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    [SerializeField] private GameObject ballPrefab;

    private ObjectPool<GameObject> pool;

    public void FillPool() {
        // Initialize pool
        pool = new ObjectPool<GameObject>(Create, ActionOnGet, ActionOnRelease, null, true, GameManager.instance.maxBallSize, GameManager.instance.maxBallSize);

        // Initialize as many as pool size.
        // This way there won't be any Create inside the game, there will be only Get.
        // So fps drop happens only at the start of the game
        GameObject[] tempRelease = new GameObject[GameManager.instance.maxBallSize];
        for (int i = 0;i < GameManager.instance.maxBallSize;++i)
            tempRelease[i] = pool.Get();
        for (int i = 0;i < GameManager.instance.maxBallSize;++i)
            Release(tempRelease[i]);
    }

    private GameObject Create() {
        GameObject ball = Instantiate(ballPrefab, new Vector3(-100, -100, -100), Quaternion.identity, GameManager.instance.ballParent);
        ball.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value, 1f);
        return ball;
    }

    private void ActionOnGet(GameObject ball) {
        ball.SetActive(true);
    }

    private void ActionOnRelease(GameObject ball) {
        ball.SetActive(false);
        ball.transform.position = new Vector3(-100, -100, -100);
        ball.transform.SetParent(GameManager.instance.ballParent);
    }

    public void Release(GameObject ball) => pool.Release(ball);

    public GameObject Get() => pool.Get();


}