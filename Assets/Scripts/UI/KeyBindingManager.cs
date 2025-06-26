using UnityEngine;
using UnityEngine.UI;
using TMPro; // 使用TextMeshPro需要这个
using System.Linq; // 用于简化数组查询

public class KeyBindingManager : MonoBehaviour
{
    // --- UI引用 ---
    // 请在Unity Inspector中把对应的UI组件拖拽到这里
    [Header("技能输入框")]
    public TMP_InputField SpringInput;
    public TMP_InputField SummerInput;
    public TMP_InputField AutumnInput;
    public TMP_InputField WinterInput;

    [Header("错误提示图标")]
    public Image SpringErrorIcon;
    public Image SummerErrorIcon;
    public Image AutumnErrorIcon;
    public Image WinterErrorIcon;


    // --- 快捷键定义 ---
    // 默认快捷键
    private const string DefaultSpringKey = "Space";
    private const string DefaultSummerKey = "LeftShift";
    private const string DefaultAutumnKey = "Space"; // 注意：长按逻辑在技能脚本里判断，这里只设定按键
    private const string DefaultWinterKey = "LeftCtrl";

    // 用于保存到本地的键名
    private const string SpringKeyPref = "Key_Spring";
    private const string SummerKeyPref = "Key_Summer";
    private const string AutumnKeyPref = "Key_Autumn";
    private const string WinterKeyPref = "Key_Winter";

    // 合法按键列表
    private static readonly string[] ValidKeys = new string[] {
        "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
        "Space","Enter",
        "LeftCtrl","RightCtrl","LeftShift","RightShift","LeftAlt","RightAlt",
        "UpArrow","DownArrow","LeftArrow","RightArrow",
        "F1","F2","F3","F4","F5","F6","F7","F8","F9","F10","F11","F12",
        "0","1","2","3","4","5","6","7","8","9"
    };

    // 用于保存上一次有效值的变量
    private string lastSpringValue;
    private string lastSummerValue;
    private string lastAutumnValue;
    private string lastWinterValue;


    void Start()
    {
        // 加载已保存的快捷键，如果没有则使用默认值
        string springKey = PlayerPrefs.GetString(SpringKeyPref, DefaultSpringKey);
        string summerKey = PlayerPrefs.GetString(SummerKeyPref, DefaultSummerKey);
        string autumnKey = PlayerPrefs.GetString(AutumnKeyPref, DefaultAutumnKey);
        string winterKey = PlayerPrefs.GetString(WinterKeyPref, DefaultWinterKey);

        // 在输入框中显示快捷键
        SpringInput.text = springKey;
        SummerInput.text = summerKey;
        AutumnInput.text = autumnKey;
        WinterInput.text = winterKey;

        // 保存初始的有效值
        lastSpringValue = springKey;
        lastSummerValue = summerKey;
        lastAutumnValue = autumnKey;
        lastWinterValue = winterKey;

        // 启动时隐藏所有错误图标
        SpringErrorIcon.gameObject.SetActive(false);
        SummerErrorIcon.gameObject.SetActive(false);
        AutumnErrorIcon.gameObject.SetActive(false);
        WinterErrorIcon.gameObject.SetActive(false);

        // 监听输入框编辑结束事件
        SpringInput.onEndEdit.AddListener(value => ValidateAndSave(SpringInput, SpringErrorIcon, SpringKeyPref, ref lastSpringValue, value));
        SummerInput.onEndEdit.AddListener(value => ValidateAndSave(SummerInput, SummerErrorIcon, SummerKeyPref, ref lastSummerValue, value));
        AutumnInput.onEndEdit.AddListener(value => ValidateAndSave(AutumnInput, AutumnErrorIcon, AutumnKeyPref, ref lastAutumnValue, value));
        WinterInput.onEndEdit.AddListener(value => ValidateAndSave(WinterInput, WinterErrorIcon, WinterKeyPref, ref lastWinterValue, value));
    }

    // 统一处理校验和保存的逻辑
    private void ValidateAndSave(TMP_InputField inputField, Image errorIcon, string prefKey, ref string lastValidValue, string newValue)
    {
        if (IsValidKeyInput(newValue))
        {
            // 输入有效：保存、更新记录、隐藏错误图标
            PlayerPrefs.SetString(prefKey, newValue);
            PlayerPrefs.Save(); // 确保立即保存
            lastValidValue = newValue;
            errorIcon.gameObject.SetActive(false);
        }
        else
        {
            // 输入无效：恢复上次内容、显示错误图标
            Debug.LogWarning($"无效的快捷键输入: '{newValue}'");
            inputField.text = lastValidValue;
            errorIcon.gameObject.SetActive(true);
        }
    }

    // 校验输入是否合法
    private bool IsValidKeyInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;

        // 用“+”分割组合键
        string[] keys = input.Split('+');
        if (keys.Length == 0) return false;

        // 检查每个部分是否都是合法的按键
        foreach (string key in keys)
        {
            string trimmedKey = key.Trim();
            // 忽略大小写进行比较
            if (!ValidKeys.Any(validKey => validKey.Equals(trimmedKey, System.StringComparison.OrdinalIgnoreCase)))
            {
                return false; // 发现一个不合法的按键
            }
        }

        return true; // 所有部分都合法
    }

    // --- 公共接口 ---

    /// <summary>
    /// 将所有快捷键设置重置为默认值。
    /// 这个方法应该被一个全局的“恢复默认设置”按钮调用。
    /// </summary>
    public void ResetKeyBindings()
    {
        Debug.Log("正在重置快捷键为默认值...");

        // 恢复春季技能默认键
        PlayerPrefs.SetString(SpringKeyPref, DefaultSpringKey);
        SpringInput.text = DefaultSpringKey;
        lastSpringValue = DefaultSpringKey;
        SpringErrorIcon.gameObject.SetActive(false);

        // 恢复夏季技能默认键
        PlayerPrefs.SetString(SummerKeyPref, DefaultSummerKey);
        SummerInput.text = DefaultSummerKey;
        lastSummerValue = DefaultSummerKey;
        SummerErrorIcon.gameObject.SetActive(false);

        // 恢复秋季技能默认键
        PlayerPrefs.SetString(AutumnKeyPref, DefaultAutumnKey);
        AutumnInput.text = DefaultAutumnKey;
        lastAutumnValue = DefaultAutumnKey;
        AutumnErrorIcon.gameObject.SetActive(false);

        // 恢复冬季技能默认键
        PlayerPrefs.SetString(WinterKeyPref, DefaultWinterKey);
        WinterInput.text = DefaultWinterKey;
        lastWinterValue = DefaultWinterKey;
        WinterErrorIcon.gameObject.SetActive(false);

        // 确保所有更改都已保存
        PlayerPrefs.Save();
    }

    // 其他脚本（如玩家控制器）可以通过这些方法来获取当前绑定的按键
    public static string GetSpringKey()
    {
        return PlayerPrefs.GetString(SpringKeyPref, DefaultSpringKey);
    }
    public static string GetSummerKey()
    {
        return PlayerPrefs.GetString(SummerKeyPref, DefaultSummerKey);
    }
    public static string GetAutumnKey()
    {
        return PlayerPrefs.GetString(AutumnKeyPref, DefaultAutumnKey);
    }
    public static string GetWinterKey()
    {
        return PlayerPrefs.GetString(WinterKeyPref, DefaultWinterKey);
    }
}