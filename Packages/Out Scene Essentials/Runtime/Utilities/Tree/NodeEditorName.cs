using System;

namespace TreeUtilities
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class NodeEditorName : Attribute
    {
        public string[] RelevantEditor { get; }

        public NodeEditorName(params string[] relevantEditor)
        {
            RelevantEditor = relevantEditor;
        }
    }
}
