using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Title : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _root;

    private Button _startBtn, _settingBtn , _exitBtn;

    public int index;
    
    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        _root = _uiDocument.rootVisualElement;
        _startBtn = _root.Q<Button>("startBtn");
       
        _exitBtn = _root.Q<Button>("exitBtn");

        _startBtn.clicked += HandleStart;
        _exitBtn.clicked += HandleExit;
    }

    private void HandleExit()
    {
        Application.Quit();
    }

    private void HandleStart()
    {
        SceneManager.LoadScene(index);
    }
    
    
}
