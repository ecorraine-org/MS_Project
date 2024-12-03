using UnityEngine;
using TMPro;

public class BlinkTextTMP : MonoBehaviour
{
    public TextMeshProUGUI textComponent;  // TextMeshProのコンポーネント
    public float blinkSpeed = 1.0f;

    void Update()
    {
        if (textComponent != null)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, alpha);
        }
    }
}