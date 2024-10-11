using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour
{
    public Text damageText;
    public float fadeDuration = 1.0f;
    public Vector3 offset;

    private Color originalColor;

    void Start()
    {
        originalColor = damageText.color;
    }

    public void ShowDamage(int damageAmount)
    {
        damageText.text = "-" + damageAmount.ToString();
        damageText.color = originalColor;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Vector3 startPosition = transform.position;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float percentageComplete = elapsed / fadeDuration;

            transform.position = startPosition + offset * percentageComplete;

            Color newColor = originalColor;
            newColor.a = Mathf.Lerp(1, 0, percentageComplete);
            damageText.color = newColor;

            yield return null;
        }

        damageText.enabled = false;
    }
}
