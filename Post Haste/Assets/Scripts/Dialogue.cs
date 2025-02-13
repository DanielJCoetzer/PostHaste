using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dialogue
{
    [System.Serializable]
    public struct Sentence 
    {
        public string name;
        [TextArea(3, 10)]
        public string text;
    }
}
