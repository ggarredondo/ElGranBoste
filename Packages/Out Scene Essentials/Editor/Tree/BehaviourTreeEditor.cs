using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using TreeUtilities;

public class BehaviourTreeEditor : EditorWindow
{
    BehaviourTreeView treeView;
    InspectorView inspectorView;

    [MenuItem("Window/UI Toolkit/BehaviourTreeEditor")]
    public static void OpenWindow()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if(Selection.activeObject is BehaviourTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.parry-mechanics.out-scene-essentials/Editor/Tree/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.parry-mechanics.out-scene-essentials/Editor/Tree/BehaviourTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<BehaviourTreeView>();
        inspectorView = root.Q<InspectorView>();
        treeView.OnNodeSelected = OnNodeSelectionChange;
        inspectorView.UpdateEvent += treeView.OnUpdate;
        inspectorView.DuplicateEvent += treeView.OnDuplicate;

        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        BehaviourTree tree = Selection.activeObject as BehaviourTree;
        if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
        {
            treeView.PopulateView(tree);
        }
    }

    void OnNodeSelectionChange(NodeView node)
    {
        inspectorView.UpdateSelection(node);
    }

    private void OnInspectorUpdate()
    {
        treeView?.UpdateNodeState();
    }
}