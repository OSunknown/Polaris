///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.DataShell;

public class UIRootManager : MonoBehaviour {

    public enum UIState { IDLE = 0, TYPING, TYPINGEND, SELECT };
    public UIState nowState = UIState.IDLE;
    //ImageBox
    public UIImageBox ImageBox;
    //FaceBox
    public UIImageBox NPCFaceBox;
    //TextBox
    public UITextBox TextBox;
    //
    public UISelectBox SelectBox;

    public List<UISelectButton> ButtonList = new List<UISelectButton>();

    public List<ActionShell> ActionPlan = new List<ActionShell>();
    public List<string> ChoiceIDList = new List<string>();

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Update()
    {
        if (nowState == UIState.TYPING)
        {
            if (Input.GetMouseButtonDown(0))
            {
                nowState = UIState.TYPINGEND;
            }
        }
    }

   

    public void DataSet(Command order)
    {
        ActionShell AS;
        ChoiceIDList.Clear();
        ActionPlan.Clear();
        int index = 0;
        int Buttonindex = 0;
        foreach (DataShell Shell in order.Data)
        {
            AS = new ActionShell();
            switch (Shell.key)
            {
                case "TextBox.Open":
                case "TextBox.Close":
                    AS.dAction = TextBox.Switch;
                    break;
                case "FaceBox.Open":
                case "FaceBox.Close":
                    AS.dAction = NPCFaceBox.Switch;
                    break;
                case "ImageBox.Open":
                case "ImageBox.Close":
                    AS.dAction = ImageBox.Switch;
                    break;
                case "SelectBox.Open":
                case "SelectBox.Close":
                    AS.dAction = SelectBox.Switch;
                    break;
                case "TextBox.AddText":
                    AS.dAction = TypingAction;
                    break;
                case "FaceBox.SetImage":
                    AS.dAction = NPCFaceBox.sprite;
                    break;
                case "ImageBox.SetImage":
                    AS.dAction = ImageBox.sprite;
                    break;
                case "SelectBox.AddChoice":
                    AS.dAction = null;
                    ButtonList[Buttonindex].Switch(Shell.Data);
                    Buttonindex++;
                    break;
                case "SelectBox.ChoiceID":
                    AS.dAction = null;
                    ChoiceIDList.Add(Shell.Data);
                    break;
                case "SelectBox.SetImage":
                    AS.dAction = SelectBox.sprite;
                    break;
                default:
                    AS.dAction = null;
                    Debug.Log("Lost order : " + Shell.key);
                    break;
            }
            if (AS.dAction != null) { 
                AS.Commend = Shell.Data;
                ActionPlan.Add(AS);
            }
        }

        CoroutineAction();
    }

    public void CoroutineAction()
    {
        StopAllCoroutines();
        StartCoroutine(DataAction());
    }

    private IEnumerator DataAction()
    {
        for (int i = 0; i < ActionPlan.Count; i++)
        {
            ActionPlan[i].Action();
            yield return new WaitUntil(() => nowState == UIState.IDLE ? Input.GetMouseButtonDown(0) == true : false);
        }
        if (nowState != UIState.SELECT)
        { 
            ClosedAll();
        }
    }
   
    

    public void TypingAction(string value)
    {
        nowState = UIState.TYPING;
        StartCoroutine(Typing(value));
    }
    public IEnumerator Typing(string value)
    {
        string TypingString = "";
        for(int i = 0;i < value.Length ; i++)
        {
            if(nowState == UIState.TYPINGEND)
            {
                break;
            }
            TypingString += value[i];
            TextBox.text = TypingString;
            yield return new WaitForSeconds(0.1f);
        }
        TextBox.text = value;
        nowState = UIState.IDLE;
    }
    
    public void ButtonAction(int Index)
    {
        ClosedAll();
        LoadData LD = FindObjectOfType<LoadData>();
        LD.GetData(ChoiceIDList[Index]);
    }

    public void ClosedAll()
    {
         //ImageBox
        ImageBox.Close();
        //FaceBox
        NPCFaceBox.Close();
        //TextBox
        TextBox.Close();
        //
        SelectBox.Close();

        foreach(UISelectButton item in ButtonList)
        {
            item.Close();
        }
    }
   
}






