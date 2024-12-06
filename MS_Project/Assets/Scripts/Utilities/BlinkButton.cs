using UnityEngine;
using UnityEngine.UI;

public class BlinkButton : MonoBehaviour
{
    public Image buttonImage;  // ボタンのImageコンポーネントを割り当て
    public float blinkSpeed = 1.0f;  // 点滅のスピード

    void Update()
    {
        if (buttonImage != null)
        {
            // サイン波でアルファ値を変化させて点滅を実現
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, alpha);
        }
    }
}