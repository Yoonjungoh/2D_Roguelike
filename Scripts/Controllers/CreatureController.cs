using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class CreatureController : MonoBehaviour
{
    public float _moveTime = 0.1f;
    public LayerMask _blockingLayer;

    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rigidBody;
    private float _inverseMoveTime;  // ������ ȿ�������� ����� �� ���

    protected IEnumerator SmoothMovement(Vector3 dest)
    {
        // �̵��� ���� �Ÿ� ���
        float sqrRemainDistance = (transform.position - dest).sqrMagnitude;

        while(sqrRemainDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(_rigidBody.position, dest, _inverseMoveTime * Time.deltaTime);
            _rigidBody.MovePosition(newPosition);
            //rigidBody.gameObject.transform.position = newPosition; // �̰� �ȵǳ�? �غ���
            // �̵� ���� ���� �Ÿ� ���
            sqrRemainDistance = (transform.position - dest).sqrMagnitude;
            yield return null;
        }
    }

    protected abstract void CantMove<T>(T component) where T : Component;
    protected bool canMoving(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 dest = start + new Vector2(xDir, yDir);

        _boxCollider.enabled = false;    // �ڱ� �ڽ��� �浹ü�� �ν����� ���ϰ� �� �����غ�
        hit = Physics2D.Linecast(start, dest, _blockingLayer);
        _boxCollider.enabled = true;

        if (hit.transform == null)  // �̵� ����
        {
            StartCoroutine(SmoothMovement(dest));
            return true;
        }
        return false;
    }

    protected virtual void AttemptMove <T> (int xDir, int yDir) where T : Component
    {
        RaycastHit2D hit;
        bool canMove = canMoving(xDir, yDir, out hit);

        if (hit.transform == null)
            return;

        T hitComponent = hit.transform.GetComponent<T>();   // ���� ������Ʈ �������� ����, ��ӹ��� ���̵��� ������ �÷��̾��� �𸣱� ����

        if (!canMove && hitComponent != null)
            CantMove(hitComponent);

    }
    protected virtual void Start()
    {
        _boxCollider = this.GetComponent<BoxCollider2D>();
        _rigidBody = this.GetComponent<Rigidbody2D>();
        _inverseMoveTime = 1f / _moveTime;
    }

}
