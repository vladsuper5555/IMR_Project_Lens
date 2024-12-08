using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonInteractions : MonoBehaviour
{
    public TextMeshPro text;  // Updated to TextMeshProUGUI for UI compatibility
    public int startListening = 0;
    private bool isListening = false;

    void Start()
    {
        // Any necessary initialization code
    }

    public void Entered()
    {
        Debug.Log("Button Clicked");
        text.text = "La multi ani Miruna";
        startListening = 1;
        isListening = true;

        // Begin listening to audio
        StartCoroutine(ListenToAudio());
    }

    // Coroutine to simulate audio-to-text processing
    private IEnumerator ListenToAudio()
    {
        while (isListening)
        {
            yield return new WaitForSeconds(2); // Simulate a delay between audio inputs

            // Mock received text
            string receivedText = "Sample text received from audio";

            AppendText(receivedText);
        }
    }

    // Method to append received text to the TextMeshPro element
    private void AppendText(string newText)
    {
        Debug.Log("Received text: " + newText);
        text.text = newText;
    }

    // Optionally, a method to stop listening can be added
    public void StopListening()
    {
        isListening = false;
    }

    void Update()
    {
        // Logic to handle other updates, if needed
    }
}