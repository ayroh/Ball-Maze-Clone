using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{

    [Header("Managers")]
    [SerializeField] private PoolManager poolManager;
    [SerializeField] private TouchManager touchManager;
    [SerializeField] private AdManager adManager;


    [Header("Scriptable")]
    [SerializeField] private LevelSO levelSO;


    [Header("Values")]
    [SerializeField] private int minBallSize;
    public int maxBallSize;
    [SerializeField] private float secondsBetweenLevels = 3f;

    [Header("Parents")]
    public Transform ballParent;
    [SerializeField] private Transform collectedBallParent;

    [Header("Environment")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private Transform sfxButton;
    [SerializeField] private Transform vibButton;
    private bool soundOn = true;
    private bool vibOn = true;

    public TextMeshProUGUI infoText;
    [SerializeField] private Transform ballReachingPoint;
    public int currentBallSize;
    public GameObject currentLevel;
    private List<GameObject> balls;
    private int currentLevelIndex = 0;

    Rigidbody rb;

    public static GameManager instance;

    private void Awake() {
        if (instance == null)
            instance = this;

        poolManager.FillPool();
        Physics.gravity = new Vector3(0f, -50f, 0f);
        balls = new();
    }

    public void StartLevel() {
        infoText.text = "Level Started";
        currentLevel = Instantiate(levelSO.levelPrefabs[currentLevelIndex]);
        ballParent.SetParent(currentLevel.transform);
        rb = currentLevel.GetComponent<Rigidbody>();
        currentBallSize = Random.Range(minBallSize, maxBallSize + 1);
        for (int i = 0;i < currentBallSize;++i) {
            balls.Add(poolManager.Get());
            balls[^1].transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        }
    }

    public void Rotate(float angle) => currentLevel.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));


    public IEnumerator NextLevel() {
        yield return new WaitForSeconds(1f);
        adManager.ShowAd();

        if (++currentLevelIndex == levelSO.levelPrefabs.Count) {
            StartCoroutine(EndGame());
            yield break;
        }

        infoText.text = "Waiting For Next Level";
        yield return new WaitForSeconds(secondsBetweenLevels);


        adManager.LoadInterstitialAd();
        ReleaseBalls();
        touchManager.gameObject.SetActive(false);
        currentLevel.SetActive(false);
        StartLevel();
        yield return new WaitForSeconds(.5f);
        touchManager.gameObject.SetActive(true);
    }
    public void ReleaseBalls() {
        for (int i = 0;i < currentBallSize;++i)
            poolManager.Release(balls[i]);
        balls.Clear();
    }

    private IEnumerator EndGame() {
        touchManager.gameObject.SetActive(false);
        infoText.text = "Game Ended\nRestarting in 5 seconds";
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }

    public void CollectBall(GameObject ball) {
        if(vibOn)
            Handheld.Vibrate();
        if (soundOn && !sfxAudioSource.isPlaying || sfxAudioSource.time >= sfxAudioSource.clip.length / 2) {
            sfxAudioSource.Stop();
            sfxAudioSource.Play();
        }
        Collider col = ball.GetComponent<Collider>();
        col.enabled = false;
        ball.transform.SetParent(collectedBallParent);
        ball.transform.DOMove(new Vector3(Random.Range(ballReachingPoint.position.x - 3, ballReachingPoint.position.x + 3), ballReachingPoint.position.y, ballReachingPoint.position.z), .2f).OnComplete(() => col.enabled = true);
    }

    public void VibrationButton() {
        vibOn = !vibOn;
        vibButton.Find("VibOnImage").gameObject.SetActive(vibOn);
        vibButton.Find("VibOffImage").gameObject.SetActive(!vibOn);
    }

    public void SFXButton() {
        soundOn = !soundOn;
        if (soundOn)
            musicAudioSource.Play();
        else
            musicAudioSource.Pause();
        sfxButton.Find("SFXOnImage").gameObject.SetActive(soundOn);
        sfxButton.Find("SFXOffImage").gameObject.SetActive(!soundOn);
    }
}
