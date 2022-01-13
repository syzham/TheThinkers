using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Test : MonoBehaviour
{

    private string link = "https://discord.com/api/webhooks/926842231086796820/CjA682ubkr9uS_DtIjCHzcxq90kihYkpLi6Imh7wy-LoAWmSubekAoLnakBLzfn4xrvC";
    private string message = "Yo some dude needs help I think";

    public void Msg()
    {
        StartCoroutine(SendWebHook(link, message, (success) =>
        {
            if (success)
                Debug.Log("done");
        }));
    }
    
    IEnumerator SendWebHook(string link, string message, System.Action<bool> action)
    {
        WWWForm form = new WWWForm();
        form.AddField("content", message);
        using (UnityWebRequest www = UnityWebRequest.Post(link, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(www.error);
                action(false);
            }
            else
            {
                action(true);
            }
        }
    }
}
