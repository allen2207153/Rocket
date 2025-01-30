using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button exitButton;  // 退出按钮
    public Button retryButton; // 重试按钮
    public Button nextButton;  // 下一关按钮

    [Header("Scene Settings")]
    public string nextSceneName;  // ✅ 手动设置下一关的场景名称
    public string titleSceneName = "TitleScene"; // ✅ 标题画面的场景名称

    private PlayerGoalAndDeath playerGoalAndDeath;

    private void Start()
    {
        // 绑定按钮事件
        if (exitButton != null) exitButton.onClick.AddListener(ExitToTitle);
        if (retryButton != null) retryButton.onClick.AddListener(RetryLevel);
        if (nextButton != null) nextButton.onClick.AddListener(NextLevel);

        // ✅ 获取 `PlayerGoalAndDeath` 组件
        playerGoalAndDeath = FindObjectOfType<PlayerGoalAndDeath>();
        if (playerGoalAndDeath == null)
        {
            Debug.LogError("找不到 PlayerGoalAndDeath 组件！");
        }
    }

    // 退出到标题画面
    public void ExitToTitle()
    {
        if (!string.IsNullOrEmpty(titleSceneName))
        {
            SceneManager.LoadScene(titleSceneName);
        }
        else
        {
            Debug.LogError("titleSceneName 为空，请在 Inspector 里设置标题场景名称！");
        }
    }

    // 重新加载当前关卡（✅ 直接调用 `ResetAllObjects()`）
    public void RetryLevel()
    {
        if (playerGoalAndDeath != null)
        {
            playerGoalAndDeath.ResetAllObjects();  // ✅ 让玩家回到出生点
        }
        else
        {
            Debug.LogError("PlayerGoalAndDeath 未找到，无法重置玩家！");
        }
    }

    // 跳转到下一关
    public void NextLevel()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("nextSceneName 为空，请在 Inspector 里设置要跳转的场景名称！");
        }
    }
}
