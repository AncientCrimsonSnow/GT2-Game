using UnityEditor;
using UnityEngine;

namespace DataStructures.Events.Editor
{
    [CustomEditor(typeof(GameEvent), editorForChildClasses: true)]
    public class GameEventEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;
            GameEvent gameEvent = target as GameEvent;
            if (GUILayout.Button("Raise"))
            {
                gameEvent.Raise();
            }
        }
    }
}