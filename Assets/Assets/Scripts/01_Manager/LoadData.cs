using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LoadGoogleSheet
{
    public string version;
    public string reqId;
    public string status;
    public string sig;
    
    public tableData table;
}

[System.Serializable]
public class tableData
{
    public colsData[] cols;
    public rowsData[] rows;
}

[System.Serializable]
public class colsData
{
    public string id;
    public string label;
    public string type;
}

[System.Serializable]
public class rowsData
{
    [System.Serializable]
    public class rowsItemData
    {
        public string v;
    }
    public rowsItemData[] c;
}

[System.Serializable]
public class CharacterData
{
    public string gObjectID;
    public string name;
    public string isNight;
    public string Position;
    public string LookAtPos;
    public List<string> talk = new List<string>();
    public TalkQ talkQ;
    
}

[System.Serializable]
public class TalkQ
{
    public List<string> talk = new List<string>();
    public Dictionary<string, int> Choice = new Dictionary<string, int>();

    public void AddChoice(string key, int value)
    {
        Choice.Add(key, value);
    }
    
}
[System.Serializable]
public class Command
{
    public string ID;
	public List<DataShell> Data = new List<DataShell>();

	public void Reset()
    {
        ID = "";
        Data.Clear();
    }

	public void DataAdd(string order)
	{
        DataShell newShell = new DataShell();
        newShell.DataAdd(order);
        Data.Add(newShell);
	}
	public void DataAdd(string order,string data)
	{
        DataShell newShell = new DataShell();
        newShell.DataAdd(order,data);
        Data.Add(newShell);
	}
}

[System.Serializable]
public class DataShell
{
	public string key;
	public string Data;

    public void DataAdd(string order)
    {
        key = order;
    }
    public void DataAdd(string order,string data)
    {
        key = order;
        Data = data;
    }
}

public class LoadData : MonoBehaviour {
    
    private readonly string defultgid = "0";
    private readonly string SheetURL = "https://docs.google.com/spreadsheets/d/1ZISCfDyDTu-_1SOdDGib89x3PnIYCPyuYeIK7a18nos/gviz/tq?gid=";
    private readonly string[] WCutString = { @"/*O_o*/","google.visualization.Query.setResponse(", ");" };


    private List<string> Allgid = new List<string>();
    public string TableText;
    public List<string> TextList;

    public Dictionary<string, Command> UIDataList = new Dictionary<string, Command>();
    public Command CheckingCommand = new Command();

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    IEnumerator LinkData(string gid)
    {
        WWW www = new WWW(SheetURL + gid);
        yield return www;

        string LoadData = (www.text);
        string TableToJsonString = LoadData.Split(WCutString, 4, System.StringSplitOptions.None)[2];

        Command CurrentData = new Command();

        LoadGoogleSheet Datacall = (LoadGoogleSheet)JsonUtility.FromJson(TableToJsonString, typeof(LoadGoogleSheet)) as LoadGoogleSheet;
        foreach (rowsData item in Datacall.table.rows)
        {
            switch (item.c[0].v)
            {
                case "Update":
                    for (int i = 1; i < item.c.Length; i++)
                    {
                        Allgid.Add(item.c[i].v);
                    }
                    break;
                //UI 관련 데이터
                case "UI.Open":
                    CurrentData.Reset();
                    CurrentData.ID = item.c[1].v;
                    break;
				case "UI.Close":
                    UIDataList.Add(CurrentData.ID,CurrentData);
                    CheckingCommand = CurrentData;
					break;
                case "TextBox.Open":
				case "TextBox.Close":
				case "FaceBox.Open":
				case "FaceBox.Close":
				case "ImageBox.Open":
				case "ImageBox.Close":
				case "SelectBox.Open":
				case "SelectBox.Close":
					CurrentData.DataAdd(item.c[0].v);
					break;
				case "TextBox.AddText":
				case "FaceBox.SetImage":
				case"ImageBox.SetImage":
				case"SelectBox.AddChoice":
				case"SelectBox.AddText":
				case"SelectBox.SetImage":
					CurrentData.DataAdd(item.c[0].v,item.c[1].v);
					break;
                default:
                    break;
            }
        }
    }


    IEnumerator Start()
    {
        
        yield return StartCoroutine(LinkData(defultgid));
        foreach(string gid in Allgid)
        { 
            yield return StartCoroutine(LinkData(gid));
        }

        UIRootManager sample =  FindObjectOfType<UIRootManager>();
        sample.DataSet(CheckingCommand);

        //Dictionary<string, string> order = new Dictionary<string, string>();
        //order.Add("aaa", "bbb");
        //order.Add("bbb", "bbb");
        //order.Add("ccc", "bbb");
        //order.Add("ddd", "bbb");
        //order.Add("eee", "bbb");
        //Dictionary<string, string>.KeyCollection keys = order.Keys;

        //foreach ( string key in keys)
        //{
        //    Debug.Log(key);
        //}


    }


}
