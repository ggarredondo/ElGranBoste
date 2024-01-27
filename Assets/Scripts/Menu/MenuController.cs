using MenuTreeUtilities;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [System.Serializable]
    public struct Menu
    {
        public GameObject UI;
        public int logicIndex;
    }

    [Header("Transition")]
    [SerializeField] private TransitionPlayer transitionPlayer;

    [Header("Input")]
    [SerializeField] private InputReader inputReader;

    [Header("Requirements")]
    public MenuTree tree;
    public List<Menu> menus;
    public Dictionary<int, GameObject> menuDictionary = new();

    [Header("Sound")]
    [SerializeField] private string changeMenuSoundName;
    [SerializeField] private string backSoundName;

    [System.NonSerialized] public bool pauseMenu;

    public event System.Action<int> OnSiblingChange;
    public event System.Action ExitPauseMenuEvent;

    private void Awake()
    {
        transitionPlayer.Initialize();
    }

    private void Start()
    {
        menus.ForEach(m => menuDictionary.Add(m.logicIndex, m.UI));

        tree.OnChange += ApplyChanges;
        inputReader.ChangeRightMenuEvent += MoveRight;
        inputReader.ChangeLeftMenuEvent += MoveLeft;
        inputReader.MenuBackEvent += Return;

        if (!pauseMenu)
            tree.Initialize();
    }

    private void OnDestroy()
    {
        tree.OnChange -= ApplyChanges;
        inputReader.ChangeRightMenuEvent -= MoveRight;
        inputReader.ChangeLeftMenuEvent -= MoveLeft;
        inputReader.MenuBackEvent -= Return;
    }

    public void ChangeMenuSpecific(int child)
    {
        tree.GoToChild(child);
        GameManager.Audio.Play(changeMenuSoundName);
    }

    public void ChangeMenu()
    {
        tree.GoToChild();
        GameManager.Audio.Play(changeMenuSoundName);
    }

    public void ChangeSibling(int child)
    {
        if (tree.ChangeSibling(child))
        {
            GameManager.Audio.Play(changeMenuSoundName);
            OnSiblingChange.Invoke(tree.ActualSelectableID());
        }
    }

    public void MoveRight()
    {
        if (tree.MoveToRightSibling())
        {
            GameManager.Audio.Play(changeMenuSoundName);
            OnSiblingChange.Invoke(tree.ActualSelectableID());
        }
    }

    public void MoveLeft()
    {
        if (tree.MoveToLeftSibling())
        {
            GameManager.Audio.Play(changeMenuSoundName);
            OnSiblingChange.Invoke(tree.ActualSelectableID());
        }
    }

    private void GoToParent()
    {
        if(tree.GoToParent())
            GameManager.Audio.Play(backSoundName);
        else if(pauseMenu)
            ExitPauseMenuEvent.Invoke();
    }

    public void Return()
    {
        AbstractMenu abs = menuDictionary[tree.CurrentId()].GetComponent<AbstractMenu>();

        if(!abs.HasTransition()) GoToParent();
    }

    private void ApplyChanges()
    {
        DisableMenus();
        tree.GetSelected().ForEach(id => { menuDictionary[id].SetActive(true); menuDictionary[id].GetComponent<AbstractMenu>().Initialize(); });
    }

    public void DisableMenus()
    {
        menus.ForEach(menu => menu.UI.SetActive(false));
    }
}
