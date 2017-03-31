using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRootManager : MonoBehaviour {

    //ImageBox
    [Header("ImageBox")]
    public GameObject ImageBoxPivot;
    public Image ImageBox;

    //FaceBox
    [Header("FaceBox")]
    public GameObject FaceBoxPivot;
    public Image NPCFace;

    //TextBox
    [Header("TextBox")]
    public GameObject TextBoxPivot;
    public Text TextBox;

    //ChoiceBox
    [Header("SelectBox")]
    public GameObject SelectBoxPivot;
    public Image PlayerFace;
    public GameObject Choice1Pivot;
    public Text Choice1;
    public GameObject Choice2Pivot;
    public Text Choice2;
    public GameObject Choice3Pivot;
    public Text Choice3;

    //public UIDataList currentData;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    public List<ActionShell> ActionPlan = new List<ActionShell>();


    public void DataSet(Command order)
    {
        ActionShell AS;
        foreach(DataShell Shell in order.Data)
        {
            switch(Shell.key)
            {
                case "TextBox.Open":
                case "TextBox.Close":
                    AS = new OpenAction(TextBoxPivot);
                    ActionPlan.Add(AS);
                    break;
                case "FaceBox.Open":
                case "FaceBox.Close":
                    AS = new OpenAction(FaceBoxPivot);
                    ActionPlan.Add(AS);
                    break;
                case "ImageBox.Open":
                case "ImageBox.Close":
                    AS = new OpenAction(ImageBoxPivot);
                    ActionPlan.Add(AS);
                    break;
                case "SelectBox.Open":
                case "SelectBox.Close":
                    AS = new OpenAction(SelectBoxPivot);
                    ActionPlan.Add(AS);
                    break;
                case "TextBox.AddText":
                    AS = new TalkAction(Shell.Data,this);
                    ActionPlan.Add(AS);
                    break;
                case "FaceBox.SetImage":
                    AS = new SetImage(NPCFace,Shell.Data);
                    ActionPlan.Add(AS);
                    break;
                case"ImageBox.SetImage":
                    AS = new SetImage(ImageBox,Shell.Data);
                    ActionPlan.Add(AS);
                    break;
                case"SelectBox.AddChoice":
                    
                    break;
                case"SelectBox.AddText":
                    break;
                case"SelectBox.SetImage":
                    AS = new SetImage(PlayerFace,Shell.Data);
                    ActionPlan.Add(AS);
                    break;
                default:
                    Debug.Log("Lost order : " + Shell.key);
                    break;
            }
        }
           
        StartCoroutine(DataAction());
    }

    private IEnumerator DataAction()
    {
        bool isFinish = false;
        for(int i= 0; i <  ActionPlan.Count ; i++)
        {
            ActionPlan[i].Action(ref isFinish);
            yield return new WaitUntil(()=> (Input.GetMouseButtonDown(0) == true || isFinish == true)&&(isTyping == false));
            isFinish = false;
        }
    }
 
    public bool isTyping = false;
    public void TypingAction(string value)
    {
        StartCoroutine(Typing(value));
    }

    public IEnumerator Typing(string value)
    {
        string TypingString = "";
        isTyping = true;
        for(int i = 0;i < value.Length ; i++)
        {
            TypingString += value[i];
            TextBox.text = TypingString;
            yield return new WaitForSeconds(0.1f);

        }
        isTyping = false;
    }

    public abstract class ActionShell 
    { 
        public abstract void Action(ref bool isFinish);
    }

    public class SetImage : ActionShell
    {
        public Image imgBox;
        public string imgName;
        public SetImage(Image box,string name)
        {
            imgBox = box;
            imgName = name;
        }
        public override void Action(ref bool isFinish)
        {
            imgBox.sprite = Resources.Load<Sprite>(@"Images/"+imgName);
        }
    }

    public class OpenAction : ActionShell
    {
        public GameObject Select;

        public OpenAction( GameObject obj)
        {
            Select = obj;
        }
        public override void Action(ref bool isFinish)
        {   Debug.Log(Select.name);
            Select.SetActive(!Select.activeSelf);
            isFinish = true;
        }
    }

    public class TalkAction : ActionShell
    {
        public string Talk;
        public UIRootManager request;
        public TalkAction(string value,UIRootManager uiManager)
        {
            Talk = value;
            request = uiManager;
        }
        public override void Action(ref bool isFinish)
        {
            request.TypingAction(Talk);
        }
    }

    public class SelectAction : ActionShell
    {
        public List<ActionShell> Talk;
        public UIRootManager request;
        public SelectAction (UIRootManager uiManager)
        {
            request = uiManager;
        }
        public override void Action(ref bool isFinish)
        {
            Debug.Log("aaaaaa");
        }
    }
}






