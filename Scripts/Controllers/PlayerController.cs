using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class PlayerController : CreatureController
{
    public int _wallDamage = 1;
    public int _pointPerFood = 10;
    public int _pointPerSoda = 20;
    public float _restartLevelDelay = 1f;

    private Animator _animator;
    private Text _foodText;

    private int _food;   // �ΰ��ӿ��� ����ϴ� food


    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    private Vector2 touchOrigin = -Vector2.one;    // �հ����� ��ũ������ ������ ����

    // UI Part
    public GameObject UI_Char_Inven;
    public GameObject[] Items;
    //public GameObject UI_Char_Inven;
    public bool IsUIOpen = false;

    protected override void Start()
    {
        _animator = GetComponent<Animator>();
        _foodText = GameObject.Find("FoodText").GetComponent<Text>();
        _food = GameManager.instance._playerFoodPoints;

        // �κ��丮 ����
        //GameObject UIPrefab = Resources.Load("Prefabs/UI/UI_Char_Inven") as GameObject;
        //UI_Char_Inven = PrefabUtility.InstantiatePrefab(UIPrefab) as GameObject;
        //UI_Char_Inven.transform.SetParent(gameObject.transform);

        base.Start();
    }
    private void OnDisable()
    {
        GameManager.instance._playerFoodPoints = _food;
    }
    protected override void AttemptMove <T> (int xDir, int yDir) // T�� �����̴� ������Ʈ�� ����ĥ ��� ������Ʈ Ÿ��
    {
        _food--;

        _foodText.text = $"Food : {_food}";
        base.AttemptMove<T>(xDir, yDir);

        CheckGameOver();

        RaycastHit2D hit;
        if (canMoving(xDir, yDir, out hit))
        {
            SoundManager.Instance.RandomizeSft(moveSound1, moveSound2);
        }

        GameManager.instance._playersTurn = false;
    }

    protected override void CantMove <T>(T component)   // �÷��̾ �̵��ϴٰ� ���� ����
    {
        Wall hitWall = component as Wall;
        hitWall.OnDamagedWall(_wallDamage);
        _animator.SetTrigger("playerAttack");
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);    // ���������� �ε�� ���� �ε��Ѵٴ� �� TODO
    }

    public void LoseFood(int loss)
    {
        _animator.SetTrigger("playerHit");
        _food -= loss; 
        _foodText.text = $"- {loss} Food : {_food}";
        CheckGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other) // �ⱸ, ����, �Ҵ� Ʈ���� �����س���, ���⼭ �±� üũ ��, ����Ƽ ��ü API��
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", _restartLevelDelay);  // �ⱸ Ʈ��Ŀ ���� �� 1�ʵ� ����
            enabled = false;
        }
        else if(other.tag == "Food")
        {

            _food += _pointPerFood;
            _foodText.text = $"+ {_pointPerFood} Food : {_food}";
            SoundManager.Instance.RandomizeSft(eatSound1, eatSound2);

            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            _food += _pointPerSoda;
            _foodText.text = $"+ {_pointPerSoda} Food : {_food}";
            SoundManager.Instance.RandomizeSft(drinkSound1, drinkSound2);
            other.gameObject.SetActive(false);
        }
    }

    private void CheckGameOver()
    {
        if (_food <= 0)
        {
            SoundManager.Instance.PlaySingle(gameOverSound);
            GameManager.instance.GameOver();
        }
    }
    bool IsOpenFirst = true;
    public void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.C) && IsUIOpen == false)    // �κ�, ĳ�� â ON
        {
            if (IsOpenFirst)
            {
                IsOpenFirst = false;
                Items = GameObject.FindGameObjectsWithTag("ItemSlot");
            }
            UI_Char_Inven.SetActive(true);
            IsUIOpen = true;
            Debug.Log("open");
        }
        else if (Input.GetKeyDown(KeyCode.C) && IsUIOpen == true)    // �κ�, ĳ�� â OFF
        {
            UI_Char_Inven.SetActive(false);
            IsUIOpen = false;
            Debug.Log("close");
        }
    }
    void Update()
    {
        if (!GameManager.instance._playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

    #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)    // �밢�� ����
            vertical = 0;
    #else
        if (Input.touchCount > 0)   // �ϳ� �̻��� ��ġ ���� ����
        {
            Touch myTouch = Input.touches[0];   // ��ġ ���� ����, [0]�� ù��° ��ġ ������ ��� ������ ��ġ�� ���� ��Ƽ ���������� [1]�ΰ���

            if (myTouch.phase == TouchPhase.Began) // ù ��ġ �������� Ȯ��
            {
                touchOrigin = myTouch.position;
            }
            else if(myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)    // ��ġ ������ �ǹ�
            {
                Vector2 touchEnd = myTouch.position;
                float x = touchEnd.x - touchOrigin.x;
                float y = touchEnd.y - touchOrigin.y;
                touchOrigin.x = -1;

                if (Mathf.Abs(x) > Mathf.Abs(y))
                    horizontal = x > 0 ? 1 : -1;
                else
                    vertical = y > 0 ? 1 : -1;
            }
        }
        #endif

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);

        InputKey();
    }
}
