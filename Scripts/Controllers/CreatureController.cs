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
    private float _inverseMoveTime;  // 움직임 효율적으로 계산할 때 사용

    protected IEnumerator SmoothMovement(Vector3 dest)
    {
        // 이동할 남은 거리 계산
        float sqrRemainDistance = (transform.position - dest).sqrMagnitude;

        while(sqrRemainDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(_rigidBody.position, dest, _inverseMoveTime * Time.deltaTime);
            _rigidBody.MovePosition(newPosition);
            //rigidBody.gameObject.transform.position = newPosition; // 이건 안되나? 해보기
            // 이동 이후 남은 거리 계산
            sqrRemainDistance = (transform.position - dest).sqrMagnitude;
            yield return null;
        }
    }

    protected abstract void CantMove<T>(T component) where T : Component;
    protected bool canMoving(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 dest = start + new Vector2(xDir, yDir);

        _boxCollider.enabled = false;    // 자기 자신을 충돌체로 인식하지 못하게 함 실험해봐
        hit = Physics2D.Linecast(start, dest, _blockingLayer);
        _boxCollider.enabled = true;

        if (hit.transform == null)  // 이동 가능
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

        T hitComponent = hit.transform.GetComponent<T>();   // 무슨 컴포넌트 가져오나 보기, 상속받은 아이들이 몬스턴지 플레이언지 모르기 때문

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
