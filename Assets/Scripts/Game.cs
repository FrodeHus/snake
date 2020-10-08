using System.Collections;
using TMPro;
using UnityEngine;


public class Game : MonoBehaviour
{
    public GameObject foodPrefab;
    public int maxFoodCount = 4;
    public Vector2Int foodZone = new Vector2Int(20, 20);
    private TextMeshProUGUI scoreLabel;
    private TextMeshProUGUI message;
    private int score;
    private void Awake()
    {
        scoreLabel = GameObject.Find("ScoreValue").GetComponent<TextMeshProUGUI>();
        message = GameObject.Find("Message").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        message.text = string.Empty;
        Snake.Instance.OnFoodConsumed += OnFoodConsumed;
        Snake.Instance.OnCrash += OnCrash;
        for (var i = 0; i < maxFoodCount; i++)
        {
            SpawnFood();
        }
    }

    private void OnCrash(object sender, System.EventArgs e)
    {
        message.text = "You died!";
    }

    private void OnFoodConsumed(object sender, FoodData e)
    {
        score += e.growAmount;
        scoreLabel.text = score.ToString();
        StartCoroutine(DelayedFoodSpawn());
    }
    private IEnumerator DelayedFoodSpawn()
    {
        yield return new WaitForSeconds(5);
        SpawnFood();
    }
    private void SpawnFood()
    {
        var food = Instantiate(foodPrefab);
        var positionX = Random.Range(0, foodZone.x - transform.position.x);
        var positionY = Random.Range(0, foodZone.y - transform.position.y);
        food.transform.position = new Vector3(positionX, positionY, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
        Gizmos.DrawCube(transform.position, new Vector3(foodZone.x, foodZone.y, 0));

    }
}
