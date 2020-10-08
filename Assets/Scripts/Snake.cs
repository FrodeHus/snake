using System;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public static Snake Instance { get; private set; }
    public event EventHandler<FoodData> OnFoodConsumed;
    public event EventHandler OnCrash;
    public GameObject bodyPartPrefab;
    private int bodySize;
    private Vector2Int currentPosition;
    private Direction currentDirection;
    private float timerMax;
    private float timer;
    private List<SnakePosition> bodyPositions;
    private bool isAlive = true;
    private void Awake()
    {
        Instance = this;
        timerMax = 0.4f;
        currentPosition = new Vector2Int(10, 10);
        bodyPositions = new List<SnakePosition>();
        currentDirection = Direction.Right;
    }
    private void Update()
    {

        HandleInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentDirection != Direction.Down)
        {
            currentDirection = Direction.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && currentDirection != Direction.Up)
        {
            currentDirection = Direction.Down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentDirection != Direction.Right)
        {
            currentDirection = Direction.Left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentDirection != Direction.Left)
        {
            currentDirection = Direction.Right;
        }

    }

    private void Move()
    {
        if (!isAlive) return;

        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            currentPosition += GetDirectionVector(currentDirection);
            timer -= timerMax;
            RenderBody();
            Grow();
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(GetDirectionVector(currentDirection)));
        }

        transform.position = new Vector3(currentPosition.x, currentPosition.y);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            var foodData = collision.GetComponent<Food>().data;
            var growAmount = foodData.growAmount;
            bodySize += growAmount;
            OnFoodConsumed?.Invoke(this, foodData);
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Obstacle" || collision.tag == "BodyPart")
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        OnCrash?.Invoke(this, EventArgs.Empty);
    }

    private Vector2Int GetDirectionVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector2Int.up;
            case Direction.Down:
                return Vector2Int.down;
            case Direction.Left:
                return Vector2Int.left;
            case Direction.Right:
                return Vector2Int.right;
            default:
                return Vector2Int.zero;
        };
    }

    private void Grow()
    {
        bodyPositions.Insert(0, new SnakePosition(currentPosition, currentDirection));
        if (bodyPositions.Count > bodySize + 1)
        {
            bodyPositions.RemoveAt(bodyPositions.Count - 1);
        }
    }

    private void RenderBody()
    {
        var idx = 1;
        foreach (var pos in bodyPositions)
        {
            var bodyPart = Instantiate(bodyPartPrefab);
            bodyPart.transform.position = new Vector3(pos.Position.x, pos.Position.y);
            bodyPart.transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(GetDirectionVector(pos.Direction)));
            bodyPart.GetComponent<SpriteRenderer>().sortingOrder = idx++ * -1;
            Destroy(bodyPart, timerMax);
        }
    }

    private float GetAngleFromVector(Vector2Int direction)
    {
        var n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
