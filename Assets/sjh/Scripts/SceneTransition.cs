using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector3 originalPosition; 
    private bool isFloating = false;  

    public float floatSpeed = 1f; 
    public float floatHeight = 2f; 

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        originalPosition = transform.position;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRenderer.enabled = true;

        if (!isFloating)
        {
            isFloating = true;
            StartCoroutine(FloatEffect());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        spriteRenderer.enabled = false;

        if (isFloating)
        {
            StopCoroutine(FloatEffect());
            isFloating = false;
            transform.position = originalPosition;
        }
    }


    private IEnumerator FloatEffect()
    {
        while (true)
        {
            // 위아래로 떠다니는 효과
            float newY = originalPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);

            yield return null;
        }
    }
}
