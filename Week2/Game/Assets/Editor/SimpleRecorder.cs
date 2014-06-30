/// C# Example
// Simple script that saves frames from the Game View when on play mode
//
// You can put later the frames togheter and create a video.
// Note: The frames are saved next to the Assets folder.

using UnityEngine;
using UnityEditor;
using System;

public class SimpleRecorder : EditorWindow
{
    string fileName = "FileName";

    string status = "Idle";
    string recordButton = "Record";
    bool recording = false;
    float lastFrameTime = 0.0f;
    int capturedFrame = 0;
    int sizeMultiplier = 1;
    string path = "Screenshots/";

    public static string formatToCreateFileFrom = "dd-MM-yyyy-HH-mm-ss";


    [MenuItem("Window/ScreenRecorder")]
    static void Init()
    {
        SimpleRecorder window =
            (SimpleRecorder)EditorWindow.GetWindow(typeof(SimpleRecorder));
    }

    void OnGUI()
    {
        fileName = EditorGUILayout.TextField("File Name:", fileName);
        sizeMultiplier = EditorGUILayout.IntField("Size multiplier", sizeMultiplier);

        if (GUILayout.Button(recordButton))
        {
            if (recording)
            { //recording
                status = "Idle...";
                recordButton = "Record";
                recording = false;
            }
            else
            { // idle
                capturedFrame = 0;
                recordButton = "Stop";
                recording = true;

                System.IO.Directory.CreateDirectory("Screenshots");
            }
        }
        EditorGUILayout.LabelField("Status: ", status);
    }

    void Update()
    {
        if (recording)
        {
            if (EditorApplication.isPlaying && !EditorApplication.isPaused)
            {
                RecordImages();
                Repaint();
            }
            else
                status = "Waiting for Editor to Play";
        }
    }

    void RecordImages()
    {
        if (lastFrameTime < Time.time + (1 / 24f))
        { // 24fps
            status = "Captured frame " + capturedFrame;
            Application.CaptureScreenshot(path + fileName + capturedFrame + DateTime.Now.ToString(formatToCreateFileFrom) + ".png", sizeMultiplier);
            capturedFrame++;
            lastFrameTime = Time.time;
        }
    }
}