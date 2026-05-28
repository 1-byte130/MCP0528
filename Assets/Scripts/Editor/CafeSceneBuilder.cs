using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class CafeSceneBuilder
{
    static CafeSceneBuilder() => EditorApplication.delayCall += Run;

    static void Run()
    {
        EditorApplication.delayCall -= Run;
        if (GameObject.Find("==CafeGame==") != null) return;
        Debug.Log("[CafeBuilder] Building icon-based scene...");
        Build();
    }

    // ════════════════════════════════════════════════════════════
    static void Build()
    {
        // Camera
        var cam = Camera.main;
        cam.orthographic = true;
        cam.orthographicSize = 5;
        cam.backgroundColor = new Color(0.04f, 0.05f, 0.13f);
        if (cam.GetComponent<BackgroundRenderer>() == null)
            cam.gameObject.AddComponent<BackgroundRenderer>();

        var root = new GameObject("==CafeGame==");

        // ── Canvas ───────────────────────────────────────────────
        var cvs = MakeCanvas(root);

        // ── HUD (top strip) ──────────────────────────────────────
        var hud = MakePanel(cvs, "HUD",
            V(0,1), V(1,1), V(0,-56), V(0,0),
            new Color(0,0,0,0.7f));

        // Score icon + number
        var scoreLbl = MakeText(hud, "ScoreLbl", "⭐", 28,
            V(0.01f,0), V(0.06f,1), V(0,0), V(0,0));
        scoreLbl.alignment = TextAnchor.MiddleCenter;

        var scoreTxt = MakeText(hud, "ScoreTxt", "0", 26,
            V(0.06f,0), V(0.25f,1), V(0,0), V(0,0));
        scoreTxt.alignment = TextAnchor.MiddleLeft;
        scoreTxt.color = new Color(1f,0.92f,0.5f);

        // Title
        var title = MakeText(hud, "Title", "深夜珈琲店", 24,
            V(0.3f,0), V(0.7f,1), V(0,0), V(0,0));
        title.alignment = TextAnchor.MiddleCenter;
        title.color = new Color(1f,0.85f,0.6f);

        // Timer
        var timerSliderGO = new GameObject("TimerSlider");
        timerSliderGO.transform.SetParent(hud.transform, false);
        var sRT = timerSliderGO.AddComponent<RectTransform>();
        sRT.anchorMin = V(0.72f,0.1f); sRT.anchorMax = V(0.99f,0.9f);
        sRT.offsetMin = sRT.offsetMax = Vector2.zero;
        var slider = timerSliderGO.AddComponent<Slider>();
        slider.direction = Slider.Direction.LeftToRight;
        slider.interactable = false;

        var sliderBg = ChildImg(timerSliderGO, "Bg", new Color(0.15f,0.15f,0.2f));
        var fillArea = new GameObject("FillArea");
        fillArea.transform.SetParent(timerSliderGO.transform, false);
        var faRT = fillArea.AddComponent<RectTransform>();
        faRT.anchorMin = Vector2.zero; faRT.anchorMax = Vector2.one;
        faRT.offsetMin = faRT.offsetMax = Vector2.zero;
        var fill = new GameObject("Fill");
        fill.transform.SetParent(fillArea.transform, false);
        var fRT = fill.AddComponent<RectTransform>();
        fRT.anchorMin = Vector2.zero; fRT.anchorMax = Vector2.one;
        fRT.offsetMin = fRT.offsetMax = Vector2.zero;
        var fillImg = fill.AddComponent<Image>();
        fillImg.color = new Color(0.3f,0.8f,1f);
        slider.fillRect = fRT;
        slider.targetGraphic = fillImg;
        slider.value = 1f;

        // ── Left: Customer panel ─────────────────────────────────
        var custPanel = MakePanel(cvs, "CustomerPanel",
            V(0.02f,0.08f), V(0.46f,0.92f), V(0,0), V(0,0),
            new Color(0,0,0,0));

        // Customer body (big coloured circle, drawn via Image + circle sprite trick)
        var bodyGO = new GameObject("CustomerBody");
        bodyGO.transform.SetParent(custPanel.transform, false);
        var bodyRT = bodyGO.AddComponent<RectTransform>();
        bodyRT.anchorMin = V(0.05f,0.15f); bodyRT.anchorMax = V(0.65f,0.85f);
        bodyRT.offsetMin = bodyRT.offsetMax = Vector2.zero;
        var bodyImg = bodyGO.AddComponent<Image>();
        bodyImg.color = new Color(0.5f,0.35f,0.6f);

        // Head
        var headGO = new GameObject("CustomerHead");
        headGO.transform.SetParent(custPanel.transform, false);
        var headRT = headGO.AddComponent<RectTransform>();
        headRT.anchorMin = V(0.25f,0.72f); headRT.anchorMax = V(0.55f,1f);
        headRT.offsetMin = headRT.offsetMax = Vector2.zero;
        var headImg = headGO.AddComponent<Image>();
        headImg.color = new Color(0.9f,0.78f,0.65f); // skin tone

        // Eyes (two small dots)
        MakeDot(custPanel, "EyeL", V(0.30f,0.83f), V(0.36f,0.90f), Color.black);
        MakeDot(custPanel, "EyeR", V(0.44f,0.83f), V(0.50f,0.90f), Color.black);

        // Order bubble (top-right of customer)
        var bubble = MakePanel(custPanel, "OrderBubble",
            V(0.55f, 0.55f), V(1.05f, 1.05f), V(0,0), V(0,0),
            new Color(1f,1f,0.92f,0.97f));

        // Bubble tail triangle (decorative panel)
        var bubbleTail = new GameObject("BubbleTail");
        bubbleTail.transform.SetParent(custPanel.transform, false);
        var btRT = bubbleTail.AddComponent<RectTransform>();
        btRT.anchorMin = V(0.53f,0.68f); btRT.anchorMax = V(0.60f,0.78f);
        btRT.offsetMin = btRT.offsetMax = Vector2.zero;
        var btImg = bubbleTail.AddComponent<Image>();
        btImg.color = new Color(1f,1f,0.92f,0.97f);

        // Drink icon inside bubble
        var orderIconGO = new GameObject("OrderDrinkIcon");
        orderIconGO.transform.SetParent(bubble.transform, false);
        var oRT = orderIconGO.AddComponent<RectTransform>();
        oRT.anchorMin = V(0.05f,0.05f); oRT.anchorMax = V(0.95f,0.95f);
        oRT.offsetMin = oRT.offsetMax = Vector2.zero;
        var orderIconImg = orderIconGO.AddComponent<Image>();
        orderIconImg.color = Color.white;
        orderIconImg.preserveAspect = true;

        // ── Right: Drink Station panel ───────────────────────────
        var stationPanel = MakePanel(cvs, "DrinkStation",
            V(0.50f,0.03f), V(0.99f,0.93f), V(0,0), V(0,0),
            new Color(0.09f,0.06f,0.03f,0.93f));

        // Station title (icon only)
        var stTitle = MakeText(stationPanel, "StTitle", "☕ 製作台", 20,
            V(0,0.88f), V(1,1), V(0,0), V(0,0));
        stTitle.alignment = TextAnchor.MiddleCenter;
        stTitle.color = new Color(1f,0.85f,0.55f);

        // Cup display area (shows added ingredients as icons)
        var cupDisplay = MakePanel(stationPanel, "CupDisplay",
            V(0.05f,0.62f), V(0.95f,0.87f), V(0,0), V(0,0),
            new Color(0.04f,0.03f,0.02f,0.8f));

        // 3 ingredient slots inside cup display
        var slotImgs = new Image[3];
        var slotObjs = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            float x0 = 0.04f + i * 0.32f, x1 = x0 + 0.30f;
            var slotGO = new GameObject("Slot" + i);
            slotGO.transform.SetParent(cupDisplay.transform, false);
            var slotRT = slotGO.AddComponent<RectTransform>();
            slotRT.anchorMin = V(x0, 0.05f); slotRT.anchorMax = V(x1, 0.95f);
            slotRT.offsetMin = slotRT.offsetMax = Vector2.zero;
            var img = slotGO.AddComponent<Image>();
            img.color = Color.white;
            img.preserveAspect = true;
            slotImgs[i] = img;
            slotObjs[i] = slotGO;
            slotGO.SetActive(false);
        }

        // Divider line
        var divider = MakePanel(stationPanel, "Divider",
            V(0.03f,0.60f), V(0.97f,0.615f), V(0,0), V(0,0),
            new Color(0.4f,0.3f,0.15f,0.8f));

        // 3 Ingredient buttons
        string[] ingLabels = { "濃縮\n咖啡", "牛\n奶", "奶\n泡" };
        Color[]  ingBgCol  = {
            new Color(0.38f,0.18f,0.05f),
            new Color(0.82f,0.78f,0.70f),
            new Color(0.92f,0.91f,0.88f)
        };
        Color[] ingLblCol = { Color.white, new Color(0.3f,0.2f,0.1f), new Color(0.4f,0.35f,0.28f) };

        var ingBtns   = new Button[3];
        var ingBtnImg = new Image[3];

        for (int i = 0; i < 3; i++)
        {
            float x0 = 0.03f + i * 0.325f, x1 = x0 + 0.31f;
            var btnGO = MakePanel(stationPanel, "IngBtn" + i,
                V(x0,0.33f), V(x1,0.59f), V(0,0), V(0,0), ingBgCol[i]);

            // Icon image (top 70% of button)
            var iconGO = new GameObject("Icon");
            iconGO.transform.SetParent(btnGO.transform, false);
            var iconRT = iconGO.AddComponent<RectTransform>();
            iconRT.anchorMin = V(0.05f,0.30f); iconRT.anchorMax = V(0.95f,0.98f);
            iconRT.offsetMin = iconRT.offsetMax = Vector2.zero;
            var iconImg = iconGO.AddComponent<Image>();
            iconImg.color = Color.white;
            iconImg.preserveAspect = true;
            ingBtnImg[i] = iconImg;

            // Label (bottom 30%)
            var lbl = MakeText(btnGO, "Label", ingLabels[i], 13,
                V(0,0), V(1,0.30f), V(2,1), V(-2,-1));
            lbl.alignment = TextAnchor.MiddleCenter;
            lbl.color = ingLblCol[i];

            var btn = btnGO.AddComponent<Button>();
            var bg  = btnGO.GetComponent<Image>();
            btn.targetGraphic = bg;
            ingBtns[i] = btn;

            // Hover tint
            var cb = btn.colors;
            cb.highlightedColor = new Color(1.2f,1.2f,1f);
            cb.pressedColor     = new Color(0.8f,0.8f,0.8f);
            btn.colors = cb;
        }

        // Serve button (big, bottom)
        var serveBtnGO = MakePanel(stationPanel, "ServeBtn",
            V(0.05f,0.14f), V(0.95f,0.31f), V(0,0), V(0,0),
            new Color(0.72f,0.38f,0.05f));
        var serveBtn = serveBtnGO.AddComponent<Button>();
        serveBtn.targetGraphic = serveBtnGO.GetComponent<Image>();
        var serveLbl = MakeText(serveBtnGO, "ServeLbl", "🍽️  送   出", 22,
            Vector2.zero, Vector2.one, V(4,4), V(-4,-4));
        serveLbl.alignment  = TextAnchor.MiddleCenter;
        serveLbl.fontStyle  = FontStyle.Bold;
        serveLbl.color      = Color.white;

        // Clear button (small, very bottom)
        var clearBtnGO = MakePanel(stationPanel, "ClearBtn",
            V(0.05f,0.03f), V(0.95f,0.13f), V(0,0), V(0,0),
            new Color(0.35f,0.20f,0.10f));
        var clearBtn = clearBtnGO.AddComponent<Button>();
        clearBtn.targetGraphic = clearBtnGO.GetComponent<Image>();
        var clearLbl = MakeText(clearBtnGO, "ClearLbl", "🗑 清除", 16,
            Vector2.zero, Vector2.one, V(2,2), V(-2,-2));
        clearLbl.alignment = TextAnchor.MiddleCenter;
        clearLbl.color = new Color(0.9f,0.7f,0.5f);

        // ── Feedback overlay ─────────────────────────────────────
        var fbPanel = MakePanel(cvs, "FeedbackPanel",
            V(0.25f,0.32f), V(0.75f,0.68f), V(0,0), V(0,0),
            new Color(0,0,0,0.75f));

        var fbIconGO = new GameObject("FbIcon");
        fbIconGO.transform.SetParent(fbPanel.transform, false);
        var fbIconRT = fbIconGO.AddComponent<RectTransform>();
        fbIconRT.anchorMin = V(0.05f,0.15f); fbIconRT.anchorMax = V(0.55f,0.90f);
        fbIconRT.offsetMin = fbIconRT.offsetMax = Vector2.zero;
        var fbIconImg = fbIconGO.AddComponent<Image>();
        fbIconImg.preserveAspect = true;

        var fbScore = MakeText(fbPanel, "FbScore", "+10", 36,
            V(0.55f,0.1f), V(1,0.9f), V(0,0), V(0,0));
        fbScore.alignment  = TextAnchor.MiddleCenter;
        fbScore.fontStyle  = FontStyle.Bold;

        fbPanel.SetActive(false);

        // ══ Wire scripts ═════════════════════════════════════════

        // UIManager
        var uiMgrGO = new GameObject("UIManager");
        uiMgrGO.transform.SetParent(root.transform);
        var uiMgr = uiMgrGO.AddComponent<UIManager>();
        uiMgr.scoreText    = scoreTxt;
        uiMgr.timerSlider  = slider;
        uiMgr.timerFill    = fillImg;
        uiMgr.feedbackPanel= fbPanel;
        uiMgr.feedbackIcon = fbIconImg;
        uiMgr.feedbackScore= fbScore;

        // CustomerManager
        var custMgrGO = new GameObject("CustomerManager");
        custMgrGO.transform.SetParent(root.transform);
        var custMgr = custMgrGO.AddComponent<CustomerManager>();
        custMgr.customerBodyImage = bodyImg;
        custMgr.orderBubble       = bubble;
        custMgr.orderDrinkIcon    = orderIconImg;

        // DrinkMaker
        var drinkMakerGO = new GameObject("DrinkMaker");
        drinkMakerGO.transform.SetParent(root.transform);
        var dm = drinkMakerGO.AddComponent<DrinkMaker>();
        dm.ingredientButtons  = ingBtns;
        dm.ingredientBtnImages= ingBtnImg;
        dm.cupSlots           = slotImgs;
        dm.cupSlotObjs        = slotObjs;
        dm.serveButton        = serveBtn;
        dm.clearButton        = clearBtn;

        // GameManager
        var gmGO = new GameObject("GameManager");
        gmGO.transform.SetParent(root.transform);
        var gm = gmGO.AddComponent<GameManager>();
        gm.uiManager       = uiMgr;
        gm.customerManager = custMgr;
        gm.drinkMaker      = dm;

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("[CafeBuilder] ✅ Done! Press Play.");
    }

    // ── Helpers ──────────────────────────────────────────────────

    static GameObject MakeCanvas(GameObject parent)
    {
        var go  = new GameObject("Canvas");
        go.transform.SetParent(parent.transform);
        var c   = go.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        var cs  = go.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1280, 720);
        go.AddComponent<GraphicRaycaster>();
        return go;
    }

    static GameObject MakePanel(GameObject parent, string name,
        Vector2 aMin, Vector2 aMax, Vector2 oMin, Vector2 oMax, Color col)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = aMin; rt.anchorMax = aMax;
        rt.offsetMin = oMin; rt.offsetMax = oMax;
        var img = go.AddComponent<Image>(); img.color = col;
        return go;
    }

    static Text MakeText(GameObject parent, string name, string txt, int size,
        Vector2 aMin, Vector2 aMax, Vector2 oMin, Vector2 oMax)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = aMin; rt.anchorMax = aMax;
        rt.offsetMin = oMin; rt.offsetMax = oMax;
        var t = go.AddComponent<Text>();
        t.text = txt; t.fontSize = size; t.color = Color.white;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.supportRichText = true;
        return t;
    }

    static Image ChildImg(GameObject parent, string name, Color col)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;
        var img = go.AddComponent<Image>(); img.color = col;
        return img;
    }

    static void MakeDot(GameObject parent, string name, Vector2 aMin, Vector2 aMax, Color col)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = aMin; rt.anchorMax = aMax;
        rt.offsetMin = rt.offsetMax = Vector2.zero;
        var img = go.AddComponent<Image>(); img.color = col;
    }

    static Vector2 V(float x, float y) => new Vector2(x, y);
}
#endif
