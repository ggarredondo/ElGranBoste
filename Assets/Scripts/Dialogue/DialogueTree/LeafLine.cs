using UnityEngine;
using TreeUtilities;

namespace DialogueTreeUtilities
{
    [NodeRelevance(typeof(DialogueTree))]
    [NodeEditorName("DialogueEditor")]
    public class LeafLine : LeafNode, IHaveText
    {
        [HideInInspector] public string text;

        public DialogueData data;

        public DialogueData Data => data;

        public string Text => text;

        public override void Clone(Node node)
        {
            base.Clone(node);
            data.Clone(((LeafLine)node).Data);
        }
    }
}
