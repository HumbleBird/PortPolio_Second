using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu ("Camera Filter Pack/Blur/Focus")]
public class CameraFilterPack_Blur_Focus : MonoBehaviour
{
    public Shader SCShader;
    private float TimeX = 1.0f;
    private Vector4 ScreenResolution;
    private Material SCMaterial;
    [Range(-1, 1)]
    public float CenterX = 0f;
    [Range(-1, 1)]
    public float CenterY = 0f;
    [Range(0, 10)]
    public float _Size = 5f;
    [Range(0.12f, 64)]
    public float _Eyes = 2f;

    public static float ChangeCenterX;
    public static float ChangeCenterY;
    public static float ChangeSize;
    public static float ChangeEyes;

    Transform m_MyPlayer2DPos; // 캐릭터 중심점

    #region Properties

    Material material
    {
        get
        {
            if(SCMaterial == null)
            {
                SCMaterial = new Material(SCShader);
                SCMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return SCMaterial;
        }
    }
    #endregion

    private void Awake()
    {
        m_MyPlayer2DPos = Managers.Object.Find(1).transform;

        ChangeCenterX = CenterX;
        ChangeCenterY = CenterY;
        ChangeSize = _Size;
        ChangeEyes = _Eyes;
        SCShader = Shader.Find("CameraFilterPack/Blur_Focus");
    }

    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        if(SCShader != null)
        {
            TimeX += Time.deltaTime;
            if (TimeX > 100) TimeX = 0;

            material.SetFloat("_TimeX", TimeX);
            material.SetFloat("_CenterX", CenterX);
            material.SetFloat("_CenterY", CenterY);
#if UNITY_IOS
            float result = Mathf.Round(0.5f / 0.2f) * 0.2f;
#else
            float result = Mathf.Round(_Size / 0.2f) * 0.2f;
#endif

            material.SetFloat("_Size", result);
            material.SetFloat("_Circle", _Eyes);
            material.SetVector("_ScreenResolution", new Vector2(Screen.width, Screen.height));
            Graphics.Blit(sourceTexture, destTexture, material);
        }
        else
        {
            Graphics.Blit(sourceTexture, destTexture);
        }
    }

    private void OnDisable()
    {
        if (SCMaterial)
        {
            DestroyImmediate(SCMaterial);
        }
    }

}      

       