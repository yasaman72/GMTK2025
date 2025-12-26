using UnityEngine;

namespace Deviloop
{
    // an attribute to always show the designer notes on the bottom of the inspector
    public class DeveloperNotesAttribute : PropertyAttribute {
        public int MinLines { get; }
        public int MaxLines { get; }
        public float SpaceBefore { get; }

        public DeveloperNotesAttribute(int minLines = 4, int maxLines = 12, float spaceBefore = 20f)
        {
            MinLines = minLines;
            MaxLines = maxLines;
            SpaceBefore = spaceBefore;
        }
    }

}
