using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;
using TreeUtilities;
using System;
using System.Reflection;
using System.Linq;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }
    public event Action UpdateEvent;
    public event Action<NodeView> DuplicateEvent;
    Editor editor;
    Editor specificEditor;

    public InspectorView()
    {
        IMGUIContainer container = new(() =>
        {
            if (GUILayout.Button("UPDATE"))
            {
                UpdateEvent?.Invoke();
            }
        });

        Add(container);
    }

    internal void UpdateSelection(NodeView nodeView)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);

        editor = Editor.CreateEditor(nodeView.node, typeof(NodeEditor));
        specificEditor = Editor.CreateEditor(nodeView.node, GetCustomEditor(nodeView.node));

        var nodeEditorName = (NodeEditorName)Attribute.GetCustomAttribute(nodeView.node.GetType(), typeof(NodeEditorName));
        if (nodeEditorName != null && nodeEditorName.RelevantEditor.Contains("DialogueEditor"))
        {
            specificEditor = Editor.CreateEditor(nodeView.node, typeof(DialogueNodeEditor));
        }
        else
        {
            specificEditor = Editor.CreateEditor(nodeView.node);
        }

        IMGUIContainer container = new(() => 
        {
            if (GUILayout.Button("UPDATE")) UpdateEvent?.Invoke();

            if(nodeView.node.GetType() != typeof(RootNode))
                if (GUILayout.Button("DUPLICATE")) DuplicateEvent?.Invoke(nodeView);

            if (editor.target)
            {
                EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.MaxWidth(300));
                editor.OnInspectorGUI();
                specificEditor.OnInspectorGUI();
                EditorGUILayout.EndVertical();
            }
        });

        Add(container);
    }

    private Type GetCustomEditor(UnityEngine.Object objectRef)
    {
        var assembly = Assembly.GetAssembly(typeof(Editor));
        var editorAttributes = assembly.CreateInstance("UnityEditor.CustomEditorAttributes");

        var type = editorAttributes.GetType();
        BindingFlags bf = BindingFlags.Static | BindingFlags.NonPublic;

        MethodInfo findCustomEditorType = type.GetMethod("FindCustomEditorType", bf);
        return (Type)findCustomEditorType.Invoke(editorAttributes, new object[] { objectRef, false });

    }
}
