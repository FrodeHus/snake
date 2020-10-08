using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Food : MonoBehaviour
{
    [SerializeField]
    public List<FoodData> foodTypes;

    public FoodData data;
    private void Awake()
    {
        var randomFood = Random.Range(0, foodTypes.Count);
        data = foodTypes[randomFood];
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.sprite;
    }

}
