using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public float _levelStartDelay = 2f;
    public float _turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager _boardManager;
    public int _playerFoodPoints = 100;
    [HideInInspector] public bool _playersTurn = true;

    private Text levelText;
    private GameObject levelImage;  // 이미지 활성 비활성 용도

    private int level = 1;
    private List<EnemyController> _enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = $"Day {level}";
        levelImage.SetActive(true);
        Invoke("HideLevelImage", _levelStartDelay);

        _enemies.Clear();
        _boardManager.SetupMainScene(level);
    }
    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }
    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(_turnDelay);
        if (_enemies.Count == 0)    // 첫 레벨
        {
            yield return new WaitForSeconds(_turnDelay);    // 우선 플레이어 기다리게함
        }

        for(int i=0; i < _enemies.Count; i++)
        {
            _enemies[i].MoveEnemy();
            yield return new WaitForSeconds(_enemies[i]._moveTime);
        }
        _playersTurn = true;
        enemiesMoving = false;
    }

    public void AddEnemyToList(EnemyController enemy)
    {
        _enemies.Add(enemy);
    }


    public void GameOver()
    {
        //levelImage = GameObject.Find("LevelImage");
        //levelText = GameObject.Find("LevelText").GetComponent<Text>();

        levelText.text = $"After {level} days, you starved...";
        levelImage.SetActive(true);
        enabled = false;
    }
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        _enemies = new List<EnemyController>();
        _boardManager =GetComponent<BoardManager>();
        InitGame();
    }

    private void OnLevelWasLoaded(int index)
    {
        level++;

        InitGame();
    }

    void Update()
    {
        if (_playersTurn || enemiesMoving || doingSetup)
            return;
        StartCoroutine(MoveEnemies());
    }
}
