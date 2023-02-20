using UnityEngine;

namespace Game.Scripts
{
    public class ShowLogs : MonoBehaviour
    {
        private string _myLog = "*begin log";
        private string _filename = "";
        private bool _doShow;
        private const int KChars = 700;
        private void OnEnable() { Application.logMessageReceived += Log; }
        private void OnDisable() { Application.logMessageReceived -= Log; }
        private void Update() { if (Input.GetKeyDown(KeyCode.Space)) { _doShow = !_doShow; } }
        private void Log(string logString, string stackTrace, LogType type)
        {
            // for onscreen...
            _myLog = _myLog + "\n" + logString;
            if (_myLog.Length > KChars) { _myLog = _myLog.Substring(_myLog.Length - KChars); }
 
            // for the file ...
            if (_filename == "")
            {
                var d = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Desktop) + "/YOUR_LOGS";
                System.IO.Directory.CreateDirectory(d);
                var r = Random.Range(1000, 9999).ToString();
                _filename = d + "/log-" + r + ".txt";
            }
            try { System.IO.File.AppendAllText(_filename, logString + "\n"); }
            catch
            {
                // ignored
            }
        }
 
        private void OnGUI()
        {
            if (!_doShow) { return; }
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
                new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f));
            GUI.TextArea(new Rect(10, 10, 540, 370), _myLog);
        }
    }
}