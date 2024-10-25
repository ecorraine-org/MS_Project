using UnityEngine;

[ExecuteInEditMode]
public class CloudShadow : MonoBehaviour
{
    [Header("Cloud Settings")]
    [SerializeField] private Texture2D noiseTexture;
    [SerializeField, Range(0f, 1f)] private float shadowStrength = 0.5f;
    [SerializeField] private float scrollSpeedX = 0.1f;
    [SerializeField] private float scrollSpeedY = 0.1f;
    [SerializeField] private float scale = 100f;

    private Light directionalLight;
    private Material shadowMaterial;
    private static readonly int ShadowTexture = Shader.PropertyToID("_ShadowTexture");
    private static readonly int ShadowStrength = Shader.PropertyToID("_ShadowStrength");
    private static readonly int ScrollOffset = Shader.PropertyToID("_ScrollOffset");
    private static readonly int Scale = Shader.PropertyToID("_Scale");

    private void OnEnable()
    {
        directionalLight = GetComponent<Light>();
        if (directionalLight == null || directionalLight.type != LightType.Directional)
        {
            Debug.LogError("CloudShadow must be attached to a Directional Light!");
            enabled = false;
            return;
        }

        CreateShadowMaterial();
        directionalLight.cookie = noiseTexture;
    }

    private void CreateShadowMaterial()
    {
        if (shadowMaterial == null)
        {
            Shader shader = Shader.Find("Hidden/CloudShadow");
            shadowMaterial = new Material(shader);
            shadowMaterial.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    private void Update()
    {
        if (shadowMaterial == null || noiseTexture == null) return;

        Vector2 offset = new Vector2(
            Time.time * scrollSpeedX,
            Time.time * scrollSpeedY
        );

        shadowMaterial.SetTexture(ShadowTexture, noiseTexture);
        shadowMaterial.SetFloat(ShadowStrength, shadowStrength);
        shadowMaterial.SetVector(ScrollOffset, offset);
        shadowMaterial.SetFloat(Scale, scale);
    }

    private void OnDisable()
    {
        if (shadowMaterial != null)
        {
            DestroyImmediate(shadowMaterial);
        }
    }
}