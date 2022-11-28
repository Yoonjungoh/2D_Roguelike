using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Wall : MonoBehaviour
{
    public AudioClip chopSound1;
    public AudioClip chopSound2;

    public Sprite _damagedSpirte;
    public int _hp = 4;
    private SpriteRenderer _spriteRenderer;
    void Awake()
    {
        _spriteRenderer=GetComponent<SpriteRenderer>();
    }
    public void OnDamagedWall(int damage)
    {
        _spriteRenderer.sprite = _damagedSpirte;
        _hp -= damage;

        SoundManager.Instance.RandomizeSft(chopSound1, chopSound2);

        if (_hp <= 0)
            gameObject.SetActive(false);
    }
}
