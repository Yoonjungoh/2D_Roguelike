using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class EnemyController : CreatureController
{
    public int _enemyDamage;
    private Animator _animator;
    private Transform _target;

    public AudioClip enemyAttackSound1;
    public AudioClip enemyAttackSound2;

    private bool _skipTurn;
    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        _animator=GetComponent<Animator>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        
        base.Start();
    }
    protected override void AttemptMove<T>(int xDir, int yDir)  // ��ȣ�ۿ� �����ϰ� ���� TODO
    {
        if (_skipTurn)
        {
            _skipTurn = false;
            return;
        }
        base.AttemptMove<T>(xDir, yDir);

        _skipTurn = true;
    }
    //protected override void AttemptMove<T>(int xDir, int yDir)  // ��ȣ�ۿ� �����ϰ� ����
    //{
    //    if (!_enemyTurn)
    //    {
    //        _enemyTurn = true;
    //        return;
    //    }
    //    base.AttemptMove<T>(xDir, yDir);

    //    _enemyTurn = false;
    //}
    public void MoveEnemy() // X�� ���� ���󰡰� ���� 
    {
        int xDir = 0;
        int yDir = 0;
        
        if(Mathf.Abs (_target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = _target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = _target.position.x > transform.position.x ? 1 : -1;
        }

        AttemptMove<PlayerController>(xDir, yDir);
    }
    protected override void CantMove<T>(T component)    // ������ ��쿡 T�� �÷��̾�
    {
        PlayerController hitPlayer = component as PlayerController;
        hitPlayer.LoseFood(_enemyDamage);
        _animator.SetTrigger("enemyAttack");

        SoundManager.Instance.RandomizeSft(enemyAttackSound1, enemyAttackSound2);
    }
}
