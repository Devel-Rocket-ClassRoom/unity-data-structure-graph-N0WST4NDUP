using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveDuration = 0.3f;
    private Stage _stage;
    private Animator _animator;
    private int currentTileId;

    public event Action<Vector3> OnMoveComplete;

    private bool _isMoving;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.speed = 0f;

        var findGO = GameObject.FindWithTag("Map");
        _stage = findGO.GetComponent<Stage>();
    }

    private void Update()
    {
        if (_isMoving) return;

        var direction = Sides.None;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Sides.Top;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Sides.Bottom;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Sides.Right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Sides.Left;
        }

        if (direction != Sides.None)
        {
            var targetTile = _stage.Map.Tiles[currentTileId].Adjacents[(int)direction];
            if (targetTile != null && targetTile.CanMove)
            {
                MoveTo(targetTile.Id);
            }
        }
    }

    public void Init(int tileId)
    {
        currentTileId = tileId;
        transform.position = _stage.GetTilePos(tileId);
    }

    public void MoveTo(int tileId)
    {
        currentTileId = tileId;
        StartCoroutine(MoveRoutine(_stage.GetTilePos(tileId)));
    }

    private IEnumerator MoveRoutine(Vector3 target)
    {
        _isMoving = true;
        _animator.speed = 1f;

        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < MoveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / MoveDuration;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;
        _isMoving = false;
        _animator.speed = 0f;
        OnMoveComplete?.Invoke(transform.position);
    }
}
