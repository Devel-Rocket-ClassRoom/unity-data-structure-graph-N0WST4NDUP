using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Stage _stage;
    private Animator _animator;
    private int currentTileId;

    public event Action<Vector3> OnMoveComplete;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.speed = 0f;

        var findGO = GameObject.FindWithTag("Map");
        _stage = findGO.GetComponent<Stage>();
    }

    private void Update()
    {
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

    public void MoveTo(int tileId)
    {
        currentTileId = tileId;

        transform.position = _stage.GetTilePos(tileId);
        OnMoveComplete?.Invoke(transform.position);
    }
}
