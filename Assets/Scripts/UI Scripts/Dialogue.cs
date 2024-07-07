using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public Sprite characterImage;

    [TextArea(3,10)]
    public List<string> sentences;
}
