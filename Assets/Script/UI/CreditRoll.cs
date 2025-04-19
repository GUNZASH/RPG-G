using UnityEngine;
using TMPro;

public class CreditRoll : MonoBehaviour
{
    public float normalSpeed = 30f;
    public float fastSpeed = 100f;
    public RectTransform creditText;

    void Update()
    {
        float currentSpeed = Input.GetMouseButton(0) ? fastSpeed : normalSpeed;
        creditText.anchoredPosition += Vector2.up * currentSpeed * Time.deltaTime;
    }
}