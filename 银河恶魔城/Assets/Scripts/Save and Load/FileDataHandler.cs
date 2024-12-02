using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
	private string dataDirPath = "";
	private string dataFileName = "";

	private bool encrypData = false;
	private string codeWord = "yxl";

	public FileDataHandler(string _dataDirPath, string _dataFileName, bool _encrypData)
	{
		this.dataDirPath = _dataDirPath;
		this.dataFileName = _dataFileName;
		this.encrypData = _encrypData;
	}

	public void Save(GameData _data)
	{
		// os.path.join
		string fullPath = Path.Combine(dataDirPath, dataFileName);

		try
		{
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

			string dataToStore = JsonUtility.ToJson(_data, true);

			if(encrypData) 
			{
				dataToStore = EncryptDecrypt(dataToStore);
			}

			using(FileStream stream = new FileStream(fullPath,FileMode.Create)) 
			{
				using(StreamWriter writer = new StreamWriter(stream)) 
				{
					writer.Write(dataToStore);
				}
			}
		}

		catch(Exception e)
		{
			Debug.Log("Error on try to save data to file" + fullPath + "\n" + e);
		}
	}

	public GameData Load()
	{
		string fullPath = Path.Combine(dataDirPath, dataFileName);
		GameData loadData = null;

		if(File.Exists(fullPath))
		{
			try
			{
				string dataToLoad = "";

				// with open(path,'rb') as f:
				using (FileStream stream = new FileStream(fullPath,FileMode.Open))
				{
					using(StreamReader reader = new StreamReader(stream))
					{
						dataToLoad = reader.ReadToEnd();
					}
				}

				if(encrypData)
				{
					dataToLoad = EncryptDecrypt(dataToLoad);
				}

				loadData = JsonUtility.FromJson<GameData>(dataToLoad);
			}

			catch (Exception e)
			{
				Debug.Log("Error on try to load data to file" + fullPath + "\n" + e);
			}
		}
		return loadData;

	}

	public void Delete()
	{
		string fullPath = Path.Combine(dataDirPath, dataFileName);

		if(File.Exists(fullPath))
		{
			File.Delete(fullPath);
		}
	}

	private string EncryptDecrypt(string _data)
	{
		// “ÏªÚº”√‹
		string modifieData = "";

		for(int i = 0;i < _data.Length;i++)
		{
			modifieData += (char)(_data[i] ^ codeWord[i % codeWord.Length]);
		}
		return modifieData;
	}
}
