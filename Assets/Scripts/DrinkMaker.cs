using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 全圖示操作台：
///  - 3個大食材按鈕 (濃縮咖啡 / 牛奶 / 奶泡)
///  - 杯子區顯示已加入的食材圖示
///  - 送出按鈕
///  - 清除按鈕
/// </summary>
public class DrinkMaker : MonoBehaviour
{
    [Header("Ingredient Buttons [0=濃縮, 1=牛奶, 2=奶泡]")]
    public Button[] ingredientButtons;      // 3個
    public Image[]  ingredientBtnImages;    // 對應圖示

    [Header("Cup Slots (顯示已加食材)")]
    public Image[] cupSlots;               // 最多3格
    public GameObject[] cupSlotObjs;       // slot 的 GameObject (for show/hide)

    [Header("Serve / Clear")]
    public Button serveButton;
    public Button clearButton;

    // state
    readonly List<int> added = new List<int>();

    void Start()
    {
        Debug.Log("AddListener OK");
        ingredientButtons[0].onClick.AddListener(() => Debug.Log("CLICK 0"));
        // 設置食材圖示
        for (int i = 0; i < ingredientButtons.Length; i++)
        {
            var tex = IconFactory.IngredientIcon(i);
            ingredientBtnImages[i].sprite = TexToSprite(tex);
            int idx = i;
            ingredientButtons[i].onClick.AddListener(() => Add(idx));
        }

        serveButton.onClick.AddListener(Serve);
        clearButton.onClick.AddListener(ResetMaker);

        RefreshCupDisplay();
    }

    public void Add(int id)
    {
        if (added.Count >= 3) return;   // 最多3層
        added.Add(id);
        RefreshCupDisplay();

        // 輕微動畫：讓按鈕彈一下
        StartCoroutine(BtnPop(ingredientButtons[id].transform));
    }

    void RefreshCupDisplay()
    {
        for (int i = 0; i < cupSlotObjs.Length; i++)
        {
            if (i < added.Count)
            {
                cupSlotObjs[i].SetActive(true);
                var tex = IconFactory.IngredientIcon(added[i]);
                cupSlots[i].sprite = TexToSprite(tex);
            }
            else
            {
                cupSlotObjs[i].SetActive(false);
            }
        }
    }

    void Serve()
    {
        GameData.DrinkType? made   = IdentifyDrink();
        GameData.DrinkType  ordered = GameManager.Instance.customerManager.currentOrder;
        GameManager.Instance.customerManager.OrderFulfilled();

        if (made.HasValue)
            GameManager.Instance.OnDrinkServed(made.Value, ordered);
        else
            GameManager.Instance.OnDrinkServed((GameData.DrinkType)99, ordered);
    }

    GameData.DrinkType? IdentifyDrink()
    {
        foreach (var kv in GameData.Recipes)
        {
            if (kv.Value.Count != added.Count) continue;
            bool ok = true;
            for (int i = 0; i < kv.Value.Count; i++)
                if (kv.Value[i] != added[i]) { ok = false; break; }
            if (ok) return kv.Key;
        }
        return null;
    }

    public void ResetMaker()
    {
        added.Clear();
        RefreshCupDisplay();
    }

    IEnumerator BtnPop(Transform t)
    {
        Vector3 orig = t.localScale;
        t.localScale = orig * 1.2f;
        yield return new WaitForSeconds(0.08f);
        t.localScale = orig;
    }

    public static Sprite TexToSprite(Texture2D tex)
    {
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                             new Vector2(0.5f, 0.5f), 64f);
    }
}
