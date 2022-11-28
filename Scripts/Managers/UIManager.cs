using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;
    int _order = 0; //sortOrder

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>(); 
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null)
            Destroy(gameObject);
        //Destroy(Instance);  // °°À»µí

        DontDestroyOnLoad(gameObject);
    }
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if(string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourceManager.Instance.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrComponenet<T>(go);
        _popupStack.Push(popup);
        return popup;
    }
    public void ClosePopupUI<T>(string name = null) where T : UI_Popup
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        ResourceManager.Instance.Destroy(popup.gameObject);
        popup = null;
        _order--;  
    }
}
