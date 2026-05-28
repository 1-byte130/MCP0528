using System.Collections.Generic;

public static class GameData
{
    // 只有3種飲料
    public enum DrinkType { Espresso, Americano, Latte, Cappuccino }

    // 食材ID: 0=濃縮咖啡, 1=牛奶, 2=奶泡
    // 美式 = 濃縮咖啡 (熱水不列入食材按鈕，美式就是濃縮咖啡的延伸，這裡簡化為同一個)
    // 美式 = 0+3(水)，但需求只有3種材料，按需求：
    //   美式    = [0]        (濃縮咖啡) — 與Espresso不同在於UI顯示
    //   拿鐵    = [0, 1]     (濃縮咖啡 + 牛奶)
    //   卡布奇諾 = [0, 1, 2]  (濃縮咖啡 + 牛奶 + 奶泡)
    // 注意: Espresso和Americano都是[0]，用不同DrinkType區分
    // 簡化：Americano = 濃縮咖啡(雙份)，這裡用 [0, 0]
    public static Dictionary<DrinkType, List<int>> Recipes = new Dictionary<DrinkType, List<int>>
    {
        { DrinkType.Espresso,   new List<int>{ 0 }       },
        { DrinkType.Americano,  new List<int>{ 0, 0 }    },
        { DrinkType.Latte,      new List<int>{ 0, 1 }    },
        { DrinkType.Cappuccino, new List<int>{ 0, 1, 2 } }
    };

    // Emoji圖示給UI用
    public static Dictionary<DrinkType, string> DrinkEmoji = new Dictionary<DrinkType, string>
    {
        { DrinkType.Espresso,   "☕" },
        { DrinkType.Americano,  "🫖" },
        { DrinkType.Latte,      "🥛" },
        { DrinkType.Cappuccino, "🍵" }
    };

    public static Dictionary<DrinkType, string> DrinkNames = new Dictionary<DrinkType, string>
    {
        { DrinkType.Espresso,   "Espresso"   },
        { DrinkType.Americano,  "Americano"  },
        { DrinkType.Latte,      "Latte"      },
        { DrinkType.Cappuccino, "Cappuccino" }
    };

    // 食材Emoji
    public static string[] IngredientEmoji = { "☕", "🥛", "🫧" };
    public static string[] IngredientNames = { "濃縮咖啡", "牛奶", "奶泡" };
}
