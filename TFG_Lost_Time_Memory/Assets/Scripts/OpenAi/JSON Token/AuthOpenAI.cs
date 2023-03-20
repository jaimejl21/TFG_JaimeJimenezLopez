using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AuthOpenAI : MonoBehaviour
{
    string _path;
    string _jsonString;
    public Button button;
    public TMPro.TMP_InputField nameInputField;
    public TMPro.TMP_InputField inputField;
    private string _keyOpenAi;

    void Start()
    {
        button.onClick.AddListener(WriteJsonOutput);
    }

    /**
     * Method that will be execute on the onclick associated to input of token
     */
    public void WriteJsonOutput()
    {
        Auth keyOpenAi = new Auth();
        _path = @"C:/Users/" + nameInputField.text + "/.openai/auth.json";
        keyOpenAi.private_api_key = inputField.text;
        _jsonString = JsonUtility.ToJson(keyOpenAi);
        File.WriteAllText(_path, _jsonString);
    }

    void Update()
    {
        if (inputField.text.Equals(String.Empty))
        {
            button.enabled = false;
        }
        else
        {
            button.enabled = true;
        }
    }
}


[Serializable]
public class Auth
{
    public string private_api_key;
}