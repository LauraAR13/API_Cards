using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
public class HTTTPCards : MonoBehaviour
{
    public RawImage[] images;
    public Http_Handler handler;
    public TMP_Text usernameText;
    private string RickApiUrl = "https://rickandmortyapi.com/api/character/avatar/";

    IEnumerator  GetImages(int[] decks)
    {
        for (int i = 0; i < decks.Length; i++)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(RickApiUrl + decks[i] + ".jpeg");
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            { Debug.Log(request.error); }
            else
            {
                images[i].texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                images[i].gameObject.GetComponentInChildren<TMP_Text>().text= decks[i].ToString();
            }
        }
    }

    public void GetRandomUserWithID(int id)
    {
        StartCoroutine(GetAllUsersWithID(id));
    }

    public IEnumerator GetAllUsersWithID(int id)
    {
        if (handler.users == null) handler.GetAllUsers();
        yield return new WaitUntil(() => handler.users != null);
        var usersWithId = handler.users[id];
        var user = usersWithId[Random.Range(0, usersWithId.Count)];
        usernameText.text = user.user_name;
        StartCoroutine(GetImages(user.deck));
    }
}
