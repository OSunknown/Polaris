using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.DataShell;

public class LoadData : MonoBehaviour {
    
    private readonly string defultgid = "0";
    private readonly string SheetURL = "https://docs.google.com/spreadsheets/d/1ZISCfDyDTu-_1SOdDGib89x3PnIYCPyuYeIK7a18nos/gviz/tq?gid=";
    private readonly string[] WCutString = { @"/*O_o*/","google.visualization.Query.setResponse(", ");" };
    public UIRootManager UIRoot;

    private List<string> Allgid = new List<string>();
    public string TableText;
    public List<string> TextList;

    public Dictionary<string, Command> UIDataList = new Dictionary<string, Command>();
    public Command CheckingCommand = new Command();
    public LoadGoogleSheet Datacall;
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
        Datacall = (LoadGoogleSheet)JsonUtility.FromJson(TableToJsonString, typeof(LoadGoogleSheet)) as LoadGoogleSheet;
        MakeCommend(Datacall);
    }

    public void MakeCommend(LoadGoogleSheet Datacall)
    {
        Command CurrentData = new Command();
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
                    UIDataList.Add(CurrentData.ID, CurrentData);
                    CheckingCommand = CurrentData;
                    CurrentData = new Command();
                    break;
                default:
                    CurrentData.DataAdd(item.c[0].v, item.c[1].v);
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
    }

    public void GetData(string ID)
    {
        UIRootManager sample = FindObjectOfType<UIRootManager>();
        sample.DataSet(UIDataList[ID]);
    }


}
