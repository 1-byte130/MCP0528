using UnityEngine;

public static class IconFactory
{
    const int S = 64;

    static Color32 C(byte r, byte g, byte b, byte a = 255) => new Color32(r, g, b, a);

    static readonly Color32 CLEAR   = C(0,0,0,0);
    static readonly Color32 OUTLINE = C(40,20,5);
    static readonly Color32 CUP_LT  = C(230,205,165);
    static readonly Color32 CUP_DK  = C(175,135,95);
    static readonly Color32 ESP     = C(75,38,8);
    static readonly Color32 ESP_LT  = C(120,68,18);
    static readonly Color32 MILK    = C(242,238,228);
    static readonly Color32 MILK_SH = C(205,195,178);
    static readonly Color32 FOAM    = C(255,253,248);
    static readonly Color32 FOAM_SH = C(218,212,198);
    static readonly Color32 WATER   = C(80,145,205);
    static readonly Color32 STEAM_C = C(200,195,185);
    static readonly Color32 GREEN   = C(45,195,75);
    static readonly Color32 RED_C   = C(215,55,55);
    static readonly Color32 WHITE   = C(255,255,255);

    // ── PUBLIC ────────────────────────────────────────────────────

    public static Texture2D DrinkIcon(GameData.DrinkType t)
    {
        switch (t)
        {
            case GameData.DrinkType.Espresso:   return BuildCup(ESP,   ESP_LT, false, false);
            case GameData.DrinkType.Americano:  return BuildCup(WATER, C(55,110,165), false, false);
            case GameData.DrinkType.Latte:      return BuildCup(ESP,   MILK,   true,  false);
            case GameData.DrinkType.Cappuccino: return BuildCup(ESP,   MILK,   true,  true);
        }
        return Bake(Blank());
    }

    public static Texture2D IngredientIcon(int id)
    {
        switch (id)
        {
            case 0: return BuildEspressoShot();
            case 1: return BuildMilk();
            case 2: return BuildFoam();
        }
        return Bake(Blank());
    }

    public static Texture2D OkIcon()   => BuildFeedback(true);
    public static Texture2D NgIcon()   => BuildFeedback(false);

    // ── CUP ───────────────────────────────────────────────────────

    static Texture2D BuildCup(Color32 liqA, Color32 liqB, bool hasMilk, bool hasFoam)
    {
        Color32[] px = Blank();

        // Body trapezoid
        for (int y = 8; y <= 42; y++)
        {
            float t = (y - 8) / 34f;
            int x0 = RLerp(10, 14, t), x1 = RLerp(54, 50, t);
            for (int x = x0; x <= x1; x++)
                px[y*S+x] = t < 0.5f ? CUP_LT : CUP_DK;
        }

        // Liquid layers
        int foamStart = 30, milkStart = 24, liqEnd;
        liqEnd = hasMilk ? milkStart : (hasFoam ? foamStart : 36);

        for (int y = 10; y < liqEnd; y++)
            FillRow(px, y, 3, Color32.Lerp(liqA, liqB, (float)(y-10) / Mathf.Max(1, liqEnd-10)));

        if (hasMilk)
        {
            int milkEnd = hasFoam ? foamStart : 36;
            for (int y = milkStart; y < milkEnd; y++) FillRow(px, y, 3, MILK);
        }
        if (hasFoam)
            for (int y = foamStart; y < 38; y++) FillFoamRow(px, y);

        // Rim
        for (int x = 8; x <= 56; x++) { px[43*S+x] = CUP_DK; px[44*S+x] = CUP_LT; }
        // Handle
        for (int y = 20; y < 38; y++) { px[y*S+57] = CUP_LT; px[y*S+58] = CUP_DK; }
        for (int x = 51; x < 59; x++) { px[20*S+x] = CUP_DK; px[37*S+x] = CUP_DK; }

        AddSteam(px, 24, 46, 0);
        AddSteam(px, 34, 47, 1);
        AddSteam(px, 44, 46, -1);

        AddOutline(px);
        return Bake(px);
    }

    static void FillRow(Color32[] px, int y, int inset, Color32 col)
    {
        float t = (y - 8) / 34f;
        int x0 = RLerp(10, 14, t) + inset;
        int x1 = RLerp(54, 50, t) - inset;
        for (int x = x0; x <= x1; x++) px[y*S+x] = col;
    }

    static void FillFoamRow(Color32[] px, int y)
    {
        float t = (y - 8) / 34f;
        int x0 = RLerp(10, 14, t) + 3, x1 = RLerp(54, 50, t) - 3;
        for (int x = x0; x <= x1; x++)
        {
            float w = Mathf.Sin(x * 0.7f + y * 1.1f) * 0.5f + 0.5f;
            px[y*S+x] = Color32.Lerp(FOAM_SH, FOAM, w);
        }
    }

    // ── INGREDIENTS ───────────────────────────────────────────────

    static Texture2D BuildEspressoShot()
    {
        Color32[] px = Blank();
        for (int y = 20; y <= 40; y++)
        {
            float t = (y-20)/20f;
            int x0 = RLerp(20,22,t), x1 = RLerp(44,42,t);
            for (int x = x0; x <= x1; x++) px[y*S+x] = Color32.Lerp(ESP_LT, ESP, t);
        }
        for (int x = 18; x <= 46; x++) px[41*S+x] = CUP_DK;
        for (int y = 26; y < 38; y++) { px[y*S+46] = CUP_LT; px[y*S+47] = CUP_DK; }
        AddSteam(px, 28, 43, 0); AddSteam(px, 36, 43, 1);
        AddOutline(px);
        return Bake(px);
    }

    static Texture2D BuildMilk()
    {
        Color32[] px = Blank();
        for (int y = 10; y <= 50; y++)
            for (int x = 20; x <= 44; x++)
                px[y*S+x] = x < 24 ? MILK_SH : MILK;
        for (int y = 50; y <= 56; y++)
            for (int x = 26; x <= 38; x++) px[y*S+x] = MILK;
        for (int y = 56; y <= 60; y++)
            for (int x = 24; x <= 40; x++) px[y*S+x] = C(170,200,255);
        // Blue stripe label
        for (int y = 22; y <= 24; y++)
            for (int x = 22; x <= 42; x++) px[y*S+x] = C(100,170,240);
        AddOutline(px);
        return Bake(px);
    }

    static Texture2D BuildFoam()
    {
        Color32[] px = Blank();
        AddCircle(px, 24, 40, 13, FOAM, FOAM_SH);
        AddCircle(px, 40, 36, 12, FOAM, FOAM_SH);
        AddCircle(px, 32, 28, 11, WHITE, FOAM);
        AddCircle(px, 18, 30, 9,  FOAM, FOAM_SH);
        AddCircle(px, 46, 26, 8,  WHITE, FOAM);
        AddSoftOutline(px, C(185,175,158));
        return Bake(px);
    }

    // ── FEEDBACK ──────────────────────────────────────────────────

    static Texture2D BuildFeedback(bool ok)
    {
        Color32[] px = Blank();
        if (ok)
        {
            AddCircle(px, 32, 32, 28, GREEN, C(30,150,55));
            AddLine(px, 14, 32, 26, 46, WHITE, 3);
            AddLine(px, 26, 46, 50, 18, WHITE, 3);
        }
        else
        {
            AddCircle(px, 32, 32, 28, RED_C, C(165,35,35));
            AddLine(px, 16, 16, 48, 48, WHITE, 3);
            AddLine(px, 48, 16, 16, 48, WHITE, 3);
        }
        return Bake(px);
    }

    // ── PRIMITIVES ────────────────────────────────────────────────

    static void AddSteam(Color32[] px, int cx, int sy, int drift)
    {
        for (int i = 0; i < 9; i++)
        {
            int x = cx + (int)(Mathf.Sin(i * 0.9f) * 2f) + drift;
            int y = sy + i;
            if (IB(x,y)) px[y*S+x] = new Color32(STEAM_C.r, STEAM_C.g, STEAM_C.b, (byte)Mathf.Max(0,170-i*20));
        }
    }

    static void AddCircle(Color32[] px, int cx, int cy, int r, Color32 inner, Color32 outer)
    {
        for (int y = cy-r; y <= cy+r; y++)
            for (int x = cx-r; x <= cx+r; x++)
            {
                float d = Mathf.Sqrt((x-cx)*(x-cx)+(y-cy)*(y-cy));
                if (d <= r && IB(x,y)) px[y*S+x] = Color32.Lerp(inner, outer, d/r);
            }
    }

    static void AddLine(Color32[] px, int x0, int y0, int x1, int y1, Color32 col, int hw)
    {
        int dx = Mathf.Abs(x1-x0), dy = Mathf.Abs(y1-y0);
        int sx = x0<x1?1:-1, sy = y0<y1?1:-1, err = dx-dy, x = x0, y = y0;
        for (int n = 0; n <= dx+dy; n++)
        {
            for (int ty = -hw; ty <= hw; ty++)
                for (int tx = -hw; tx <= hw; tx++)
                    if (IB(x+tx,y+ty)) px[(y+ty)*S+(x+tx)] = col;
            if (x==x1 && y==y1) break;
            int e2 = 2*err;
            if (e2 > -dy) { err -= dy; x += sx; }
            if (e2 <  dx) { err += dx; y += sy; }
        }
    }

    static void AddOutline(Color32[] px)
    {
        var src = (Color32[])px.Clone();
        for (int y = 1; y < S-1; y++)
            for (int x = 1; x < S-1; x++)
                if (src[y*S+x].a > 0)
                {
                    if (src[y*S+x-1].a==0 || src[y*S+x+1].a==0 ||
                        src[(y-1)*S+x].a==0 || src[(y+1)*S+x].a==0)
                        px[y*S+x] = OUTLINE;
                }
    }

    static void AddSoftOutline(Color32[] px, Color32 col)
    {
        var src = (Color32[])px.Clone();
        for (int y = 1; y < S-1; y++)
            for (int x = 1; x < S-1; x++)
                if (src[y*S+x].a > 0)
                    if (src[y*S+x-1].a==0 || src[y*S+x+1].a==0 ||
                        src[(y-1)*S+x].a==0 || src[(y+1)*S+x].a==0)
                        px[y*S+x] = col;
    }

    // ── UTIL ──────────────────────────────────────────────────────

    static Color32[] Blank()
    {
        var px = new Color32[S*S];
        for (int i = 0; i < px.Length; i++) px[i] = CLEAR;
        return px;
    }

    static Texture2D Bake(Color32[] px)
    {
        var tex = new Texture2D(S, S, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;
        tex.SetPixels32(px); tex.Apply();
        return tex;
    }

    static bool IB(int x, int y) => x >= 0 && x < S && y >= 0 && y < S;
    static int RLerp(int a, int b, float t) => Mathf.RoundToInt(a + (b-a)*t);
}
