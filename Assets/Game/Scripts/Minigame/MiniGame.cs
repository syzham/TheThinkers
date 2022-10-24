using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Scripts.MiniGame
{
    [CreateAssetMenu(menuName="MiniGame")]
    public class MiniGame : ScriptableObject
    {
        public GameObject game;
        private bool _timer;
        private int _time;
        private bool _restrictions;
        private bool[] _rests = { false, false, false};

        public int GetTime()
        {
            return _time;
        }

        public bool Timer()
        {
            return _timer;
        }

        public bool HasRestriction()
        {
            return _restrictions;
        }

        public bool[] GetRestrictions()
        {
            return _rests;
        }
        
        #region Editor
        #if UNITY_EDITOR
        [CustomEditor(typeof(MiniGame))]
        public class MiniGameEditor : Editor
        {
            private static void HorizontalLine ( Color color )
            {
                var padding = 10;
                var space = 1;
                var r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + space));
                r.height = space;
                r.y += padding / 2;
                r.x-=2;
                r.width +=6;
                EditorGUI.DrawRect(r, color);
            }
            public override void OnInspectorGUI()
            {
                var grey = new Color(30, 30, 30, 60);
                base.OnInspectorGUI();
                var miniGame = (MiniGame) target;
                HorizontalLine(grey);
                miniGame._timer = EditorGUILayout.Toggle("Timer", miniGame._timer);
                if (miniGame._timer)
                {
                    EditorGUI.indentLevel++;
                    EditorGUI.indentLevel++;
                    EditorGUIUtility.labelWidth = 120f;
                    miniGame._time = EditorGUILayout.IntField("Amount of time", miniGame._time);
                    EditorGUI.indentLevel--;
                    EditorGUI.indentLevel--;
                }
                
                HorizontalLine(grey);
                
                miniGame._restrictions = EditorGUILayout.Toggle("Restrictions", miniGame._restrictions);
                if (!miniGame._restrictions) return;
                EditorGUI.indentLevel++;
                EditorGUI.indentLevel++;
                EditorGUIUtility.labelWidth = 150f;
                miniGame._rests[0] = EditorGUILayout.Toggle("Lockpicker", miniGame._rests[0]);
                miniGame._rests[1] = EditorGUILayout.Toggle("Smarty", miniGame._rests[1]);
                miniGame._rests[2] = EditorGUILayout.Toggle("Stronk", miniGame._rests[2]);
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }
        }
        #endif
        #endregion
    }
}
