using UnityEngine;

// Attach to Camera - draws a dark night-time cafe background using GL
[RequireComponent(typeof(Camera))]
public class BackgroundRenderer : MonoBehaviour
{
    public Color skyColor      = new Color(0.04f, 0.04f, 0.12f);
    public Color floorColor    = new Color(0.10f, 0.06f, 0.04f);
    public Color counterColor  = new Color(0.18f, 0.10f, 0.05f);
    public Color windowColor   = new Color(0.15f, 0.20f, 0.35f);
    public Color lampColor     = new Color(1.0f,  0.85f, 0.4f);

    Material mat;

    void Awake()
    {
        mat = new Material(Shader.Find("Hidden/Internal-Colored"));
        mat.hideFlags = HideFlags.HideAndDontSave;
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        mat.SetInt("_ZWrite", 0);
    }

    void OnPostRender()
    {
        mat.SetPass(0);
        GL.PushMatrix();
        GL.LoadOrtho();

        DrawQuad(0, 0, 1, 1, skyColor);              // sky / background
        DrawQuad(0, 0, 1, 0.3f, floorColor);         // floor
        DrawQuad(0.05f, 0.28f, 0.95f, 0.38f, counterColor); // counter top

        // Windows
        DrawQuad(0.05f, 0.55f, 0.30f, 0.85f, windowColor);
        DrawQuad(0.60f, 0.55f, 0.85f, 0.85f, windowColor);

        // Lamp glow (small circle approximated as quads)
        DrawQuad(0.46f, 0.88f, 0.54f, 0.92f, lampColor);

        GL.PopMatrix();
    }

    void DrawQuad(float x0, float y0, float x1, float y1, Color c)
    {
        GL.Begin(GL.QUADS);
        GL.Color(c);
        GL.Vertex3(x0, y0, 0);
        GL.Vertex3(x1, y0, 0);
        GL.Vertex3(x1, y1, 0);
        GL.Vertex3(x0, y1, 0);
        GL.End();
    }
}
