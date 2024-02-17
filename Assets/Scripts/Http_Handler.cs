using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Unity.VisualScripting;

public class Http_Handler : MonoBehaviour
{

    private string FakeApiUrl = "https://my-json-server.typicode.com/LauraAR13/fakeapi";
    public GameObject userInfo;
    public Transform normalUsers;
    public Dictionary<int, List<UserData>> users;
    private void Start()
    {
        GetAllUsers();
    }
    public void GetAllUsers()
    {
        StartCoroutine(GetUsersData());
    }

    IEnumerator GetUsersData()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://my-json-server.typicode.com/LauraAR13/fakeapi/users");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }

        else
        {
            Debug.Log(request.downloadHandler.text);
            if (request.responseCode == 200)
            {
               var data = JsonUtility.FromJson<JsonData>("{\"results\":" + request.downloadHandler.text + "}");
                foreach (UserData user in data.results)
                {
                    if (users==null)
                    {
                        users = new Dictionary<int, List<UserData>>();  
                    }
                    if (!users.ContainsKey(user.id))
                    {
                        var list = new List<UserData>();
                        list.Add(user); 
                        users.Add(user.id, list);
                    }
                    else
                    {
                        var usersWithId = users[user.id];
                        var list = usersWithId;
                        list.Add(user);
                        users[user.id] = list;

                    }
                

                    Debug.Log(user.user_name + "|");
                    foreach (var card in user.deck)
                    {
                        Debug.Log(card);
                    }
                    var newUser = Instantiate(userInfo, normalUsers);
                    newUser.GetComponentInChildren<TMP_Text>().text = "ID: " + user.id + "User: " + user.user_name;
                    newUser.gameObject.SetActive(true); 
                }
            }
      
        }

    }

  

   


    /*IEnumerator GetCharacters()
    {
        UnityWebRequest request = UnityWebRequest.Get(RickApiUrl+"/character");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }

        else
        {
            //Debug.Log(request.downloadHandler.text);
            if (request.responseCode == 200)
            {
                JsonData data = JsonUtility.FromJson<JsonData>(request.downloadHandler.text);

                foreach (CharacterData character in data.results)
                {
                    Debug.Log(character.name + " is a " + character.species);

                }
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }

        

    }*/
}

[System.Serializable]
public class JsonData
{
    public UserData [] results;
    //public CharacterData[] results;
}

[System.Serializable]
public class UserData
{
    public int id;
    public string user_name;
    public int[] deck;
}

[System.Serializable]
public class CharacterData
{
    public int id;
    public string name;
    public string species;
    public string image;
}
