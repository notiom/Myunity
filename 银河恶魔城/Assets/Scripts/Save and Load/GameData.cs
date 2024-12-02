using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;

    public SerializableDictionary<string, int> inventory;
    public SerializableDictionary<string, bool> skillTree;
    public List<string> equipmentId;

    public SerializableDictionary<string, bool> checkpoints;
    public string closetCheckpointId;

    public SerializableDictionary<string, float> volumeSettings;

    public int rightBound;

    public int currentPassMap;
    public GameData()
    {
        currency = 0;
        currentPassMap = 1;
		rightBound = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        closetCheckpointId = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();
        volumeSettings = new SerializableDictionary<string, float>();
    }

}
