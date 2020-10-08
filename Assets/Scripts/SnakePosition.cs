using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePosition
{
    public Direction Direction { get; set; }
    public Vector2Int Position { get; set; }

    public SnakePosition(Vector2Int position, Direction direction)
    {
        Position = position;
        Direction = direction;
    }
}
