using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Title : MonoBehaviour
{
    
    private UIDocument _uiDocument;
    private VisualElement _root;

    private Button _startBtn, _settingBtn , _exitBtn;
    
    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        _root = _uiDocument.rootVisualElement;
        _startBtn = _root.Q<Button>("startBtn");
        _settingBtn = _root.Q<Button>("settingBtn");
        _exitBtn = _root.Q<Button>("exitBtn");

        _startBtn.clicked += HandleStart;
        _settingBtn.clicked += HandleSetting;
        _exitBtn.clicked += HandleExit;
    }

    private void HandleExit()
    {
        Application.Quit();
        Debug.Log("나가기");
    }

    private void HandleSetting()
    {
        Debug.Log("설정창");
    }

    private void HandleStart()
    {
        Debug.Log("시작");
        //SceneManager.LoadScene("inGame");
    }
    
    
}
