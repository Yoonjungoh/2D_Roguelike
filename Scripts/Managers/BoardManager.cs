using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEditor;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int _minimum;
        public int _maximum;
        public Count(int min,int max)
        {
            _minimum = min;
            _maximum = max;
        }
    }

    public int _columns = 8;
    public int _rows = 8;
    public Count _wallCount = new Count(5, 9);
    public Count _foodCount = new Count(1, 5);

    public GameObject _exit;

    public GameObject[] _floorTiles;
    public GameObject[] _wallTiles;
    public GameObject[] _foodTiles;
    public GameObject[] _enemyTiles;
    public GameObject[] _outerWallTiles;

    private Transform _boardHolder;
    private List<Vector3> _gridPositons = new List<Vector3>();  // 몬스터, 유저, 출구, 아이템 배치

    void InitializeList()   // 리스트 비우고 새로 채워줌
    {
        _gridPositons.Clear();

        for(int x = 1; x < _columns - 1; x++)
        {
            for(int y = 1; y < _rows - 1; y++)
            {
                _gridPositons.Add(new Vector3(x, y, 0f));
            }
        }
    }
    void BoardSetup()   // 바깥 벽 타일 배치
    {
        _boardHolder = new GameObject("Board").transform;
        
        for(int x = -1; x < _columns + 1; x++)
        {
            for (int y = -1; y < _rows + 1; y++)
            {
                GameObject toInstantiate = _floorTiles[Random.Range(0, _floorTiles.Length)];
                if (x == -1 || x == _columns || y == -1 || y == _rows)
                    toInstantiate = _outerWallTiles[Random.Range(0, _outerWallTiles.Length)];

                //GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject; // 지워보기

                instance.transform.SetParent(_boardHolder);
            }
        }
    }
    
    Vector3 RandomPosition()    // 아이템, 몬스터, 출구 배치
    {
        int randomIndex = Random.Range(0, _gridPositons.Count);
        Vector3 randomPostion = _gridPositons[randomIndex];
        _gridPositons.RemoveAt(randomIndex);
        return randomPostion;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);   // 얼마나 스폰할지 조정

        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            ResourceManager.Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupMainScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(_wallTiles, _wallCount._minimum, _wallCount._maximum);
        LayoutObjectAtRandom(_foodTiles, _foodCount._minimum, _foodCount._maximum);

        int enemyCount = (int)MathF.Log(level, 2f);

        LayoutObjectAtRandom(_enemyTiles, enemyCount, enemyCount);
        ResourceManager.Instantiate(_exit, new Vector3(_columns - 1, _rows - 1, 0f), Quaternion.identity);

    }
}
