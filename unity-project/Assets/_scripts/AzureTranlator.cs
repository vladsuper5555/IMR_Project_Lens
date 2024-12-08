using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro; // For TextMeshPro

public class AzureTranslator : MonoBehaviour
{
    public string subscriptionKey = "6OyFjX1Ko5IwB574UgehEp7pOJcWO60DI3SROKxjkfpW8MJV16YRJQQJ99ALAC5RqLJXJ3w3AAAbACOGw1cT";
    public string endpoint = "https://api.cognitive.microsofttranslator.com/";
    public string region = "westeurope"; // e.g., "westeurope"
    public TextMeshPro textToTranslate; // Text to translate
    public TextMeshPro translatedTextDisplay; // Translated text output
    public string targetLanguage = "en"; // Target language code (e.g., "es" for Spanish)

    void Start()
    {
        if (textToTranslate != null && translatedTextDisplay != null)
        {
            TranslateText();
        }
        else
        {
            Debug.LogError("TextMeshPro references are not set!");
        }
    }
    public void TranslateText()
    {
        StartCoroutine(Translate(textToTranslate.text, targetLanguage));
    }

    private IEnumerator Translate(string text, string targetLang)
    {
        string route = $"/translate?api-version=3.0&to={targetLang}";
        string fullUrl = endpoint + route;

        // Create request body
        string jsonBody = "[{\"Text\":\"" + text + "\"}]";
        byte[] body = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        UnityWebRequest request = new UnityWebRequest(fullUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
        request.SetRequestHeader("Ocp-Apim-Subscription-Region", region);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse JSON response
            var response = request.downloadHandler.text;
            Debug.Log("Translation Response: " + response);

            // Extract translated text
            var translatedText = ParseTranslation(response);
            translatedTextDisplay.text = translatedText;
        }
        else
        {
            Debug.LogError("Translation Error: " + request.error);
        }
    }

    private string ParseTranslation(string jsonResponse)
    {
        // Parse response JSON to get translated text
        var responseObj = JsonUtility.FromJson<List<TranslationResponse>>(jsonResponse);
        return responseObj[0].translations[0].text;
    }

    [System.Serializable]
    public class TranslationResponse
    {
        public List<Translation> translations;
    }

    [System.Serializable]
    public class Translation
    {
        public string text;
    }
}
