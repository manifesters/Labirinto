using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PictureData", menuName = "GameData/PictureData", order = 1)]
public class PictureData : ScriptableObject
{
    public Sprite picture; // The picture to display
    public string[] options; // Array of answer options
    public int correctOptionIndex; // Index of the correct answer in the options array
}

