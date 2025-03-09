using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    private Button createBoxButton; // 按钮
    private GameObject boxPrefab; // 方框预制件
    private GameObject boxInstance; // 方框实例

    void Start()
    {
        // 创建按钮
        GameObject buttonObject = new GameObject("CreateBoxButton");
        buttonObject.transform.SetParent(GameObject.Find("Canvas").transform); // 将按钮放置在Canvas下
        buttonObject.transform.localPosition = Vector3.zero; // 设置按钮位置为(0, 0)

        // 添加按钮组件
        createBoxButton = buttonObject.AddComponent<Button>();
        buttonObject.AddComponent<RectTransform>().sizeDelta = new Vector2(160, 30); // 设置按钮大小
        buttonObject.AddComponent<CanvasRenderer>();
        Text buttonText = buttonObject.AddComponent<Text>();
        buttonText.text = "Create Box";
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        // 为按钮添加点击事件监听器
        createBoxButton.onClick.AddListener(CreateBox);

        // 创建方框预制件
        boxPrefab = CreateBoxPrefab();
    }

    void CreateBox()
    {
        if (boxInstance == null)
        {
            // 创建方框实例
            boxInstance = Instantiate(boxPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            boxInstance.AddComponent<EnemyController>(); // 添加EnemyController脚本
        }
    }

    GameObject CreateBoxPrefab()
    {
        // 创建方框预制件
        GameObject box = new GameObject("BoxPrefab");
        box.AddComponent<SpriteRenderer>().color = Color.red; // 设置方框颜色
        box.AddComponent<BoxCollider2D>(); // 添加碰撞器
        box.AddComponent<Rigidbody2D>(); // 添加Rigidbody2D组件
        return box;
    }
}