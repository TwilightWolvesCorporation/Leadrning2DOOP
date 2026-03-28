using DG.Tweening;
using UnityEngine;

public class TweenMove : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 endPos;
    [SerializeField] private float time;
    [SerializeField] private int speed;
    private bool _right = true;
    
    [SerializeField] private SpriteRenderer sprite;

    private void Awake()
    {
        startPos = transform.position;
    }

    private void Start()
    {
        sprite.DOColor(Color.red, time).SetEase(Ease.Linear);
        transform.DOShakePosition(5, 0.5f);
    }
}