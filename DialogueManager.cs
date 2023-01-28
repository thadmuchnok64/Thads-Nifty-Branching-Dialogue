using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI[] choiceButtons;
    [SerializeField] GameObject choiceWindow;
    //[SerializeField] TextMeshProUGUI exitButton;
    [SerializeField] string nameColor;
    //[SerializeField] string name;
    [SerializeField] string choiceColor;
    [SerializeField] string oldColor;
    [SerializeField] Image characterImage;
    [SerializeField] string playerName;
    [SerializeField] Color playerColor;
    [SerializeField] Sprite playerImage;
    [SerializeField] AudioClip[] playerSounds;
    [SerializeField] TextMeshProUGUI overflow;
    private Transform[] playerTran;
    private Color charColor;
    private string charName;
    private Sprite portrait;
    private Coroutine charCoroutine;
    Queue<char> charQueue;
    Stack<Dialogue> dialogues;
    //[SerializeField] AudioClip typeSound, lineSound;
    [SerializeField] Animator anim;
    [SerializeField] Animator charAnim;
    [SerializeField] RectTransform box;
    public bool processRunning;
    private AudioClip[] characterSounds;
    private int charIndex;
    //[SerializeField] Animator characterAnim;
    //[SerializeField] AudioSource aud;

    // xNode stuff below this

    public DialogueGraph graph;
    private void Start()
    {
        processRunning = false;
    }
    #region Singleton Pattern
    public static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of Dialogue Manager!!! Fix this ya bozo!");
            return;
        }
        instance = this;
    }

    #endregion
    public void StartDialogue (Dialogue d,string name)
    {
        anim.SetBool("Active", true);
        processRunning = true;
        charQueue = new Queue<char>();
        dialogues = new Stack<Dialogue>();
        charQueue.Clear();
        dialogues.Clear();
        dialogues.Push(d);
        charName = name;
        dialogueText.text = "<color=#" + nameColor + ">" + name + ":</color> ";
        choiceWindow.SetActive(true);
        StartCoroutine(InitiateDialogueDelay());
    }

    public void StartDialogue(DialogueGraph dg,Transform[] t )
    {
        charIndex = 0;
        playerTran = t;
        dialogueText.transform.position = playerTran[0].position + new Vector3(0, 2, 0);
        CameraScript.instance.AddPointOfFocus(dialogueText.transform);
        CameraScript.instance.AddPointOfFocus(dialogueText.transform);
        charQueue = new Queue<char>();
        charQueue.Clear();
        graph = dg;
        foreach( DialogueBranch node in graph.nodes)
        {
            if (node.IsStartPoint())
            {
                graph.currentNode = node;
                break;
            }
        }
        DialogueNode dn = graph.currentNode as DialogueNode;
        anim.SetBool("Active", true);
        charName = dn.npcName;
        charColor = dn.nameColor;
      //portrait = dn.npcSprite;
        characterSounds = dn.characterSounds;
        choiceWindow.SetActive(true);

        StartCoroutine(InitiateDialogueDelay());
    }

    private IEnumerator InitiateDialogueDelay()
    {
        //characterImage.sprite = portrait;
        yield return new WaitForSeconds(.2f);
        IterateDialogue();
    }

    private IEnumerator ChoiceIterate(float t)
    {
        foreach(TMPro.TextMeshProUGUI g in choiceButtons)
        {
            g.gameObject.SetActive(false);
        }
        dialogueText.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(playerColor) + ">" + playerName + ":</color> " + graph.currentNode.answer;
        //undMaster.instance.PlayRandomSound(playerSounds);
        //characterImage.sprite = playerImage;
       //arAnim.Play("Hop");
    //  UpdateBoxes();
        yield return new WaitForSeconds(t);
        IterateDialogue();

    }

    private void IterateDialogue()
    {
        if (graph.currentNode.GetToggledEntity() != -1)
        {
            charIndex = graph.currentNode.GetToggledEntity();
        }
        dialogueText.transform.position = playerTran[charIndex].position + new Vector3(0, 2,0);
        dialogueText.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(charColor) + ">" + charName + ":</color> ";
        // characterImage.sprite = portrait;
        //harAnim.Play("Hop");
        //oundMaster.instance.PlayRandomSound(characterSounds);
        if (graph.currentNode.GetGivenQuestID() != -1)
        {
            QuestManager.instance.AddQuest(graph.currentNode.GetGivenQuestID());
        }
        if (graph.currentNode.GetFinishedQuestID() != -1)
        {
           
            QuestManager.instance.SetQuestToComplete(graph.currentNode.GetFinishedQuestID());
        }

        if (graph.currentNode==null||graph.currentNode.IsEndPoint())
        {
            Exit();
        }
        char[] chars =  graph.currentNode.response.ToCharArray();
        foreach (char c in chars)
        {
            charQueue.Enqueue(c);
        }

        charCoroutine = StartCoroutine(CharScroll());
        for (int y = 0; y < 4; y++)
        {
            choiceButtons[y].gameObject.SetActive(false);
        }
        int x = 0;
        foreach( XNode.NodePort np in graph.currentNode.Outputs)
        {
            if (np.Connection == null)
            {
                break;
            }
            DialogueBranch db = np.Connection.node as DialogueBranch;
            choiceButtons[x].gameObject.SetActive(true);
            string[] split = db.answer.Split('<');
            if (split.Length == 1)
            {
                choiceButtons[x].text = db.answer;
            }
            else {
                choiceButtons[x].text = FixChoiceString(split);
            }

            x++;
        }


        //StartCoroutine(FixChoiceBoxes());
      
    }

    private string FixChoiceString(string[] split)
    {
        for(int x = 0; x < split.Length;x++)
        {
            if (split[x]=="characterName")
            {
                split[x] = "Oliver"; //TEMPORARY. Change this so that it checks what name your character is.
            }
        }
        string finalString="";
        foreach(string s in split)
        {
            finalString += s;
        }
        return finalString;
    }

    private IEnumerator FixChoiceBoxes()
    {
        yield return new WaitForSeconds(.01f);
        RectTransform last;
        for (int y = 1; y < choiceButtons.Length; y++)
        {
            last = choiceButtons[y - 1].GetComponent<RectTransform>();
            choiceButtons[y].GetComponent<RectTransform>().anchoredPosition = new Vector2(last.anchoredPosition.x, last.anchoredPosition.y  -30f - (last.rect.height / 1.5f));
        }
    }

    public void Exit()
    {
        
        processRunning = false;
        anim.StopPlayback();
        anim.SetBool("Active", false);
        choiceWindow.SetActive(false);
        CameraScript.instance.RemovePointOfFocus();
        CameraScript.instance.RemovePointOfFocus();
        Player.instance.currentState = Entity.EntityStates.IDLE;
        StartCoroutine(EndDialogueCor());
    }

   // private void Update()
    //{
        /*
        if (Input.GetMouseButtonDown(0)&&charQueue!=null)
        {
            if(charCoroutine!=null)
            StopCoroutine(charCoroutine);
            while (charQueue.Count > 0)
            {
                char c = charQueue.Dequeue();
                dialogueText.text += c;
            }
        }
        */
   // }
    private void ClearCharQueue()
    {
        if(charCoroutine!=null)
        StopCoroutine(charCoroutine);
        while (charQueue!=null && charQueue.Count > 0)
        {
            char c = charQueue.Dequeue();
            dialogueText.text += c;
        }
       // dialogueText.text = "<color=#"+oldColor+">"+ dialogueText.text+"</color>\n\n";
      //stuff here
        //  if(SoundMaster.soundOn)
        //aud.PlayOneShot(lineSound);
    }

    
    public void SelectOutput(int i)
    {
        int x = 0;
        DialogueBranch db = null;
        foreach (XNode.NodePort np in graph.currentNode.Outputs)
        {
            if (np.Connection == null)
            {
                break;
            }
            db = np.Connection.node as DialogueBranch;
            if (x == i)
            {
                break;
            }
            x++;
        }
        graph.currentNode = db;
    }


    public void Choice(int choiceNumber)
    {
        dialogueText.text = "";
        ClearCharQueue();
        SelectOutput(choiceNumber);
        //dialogueText.text += "<align=right><color=#" + choiceColor + ">" + graph.currentNode.answer + "</color></align>\n\n";
        //characterAnim.Play(name+graph.currentNode.emotion);
        IterateDialogue();
    }
 
    private IEnumerator CharScroll()
    {
        char brackStart = '<';
        char brackEnd = '>';

        while (charQueue.Count>0)
        {
         //   if (charQueue.Count % 3 == 0)
         //   {
         //       aud.PlayOneShot(typeSound);
         //   }
            char c = charQueue.Dequeue();
           
                dialogueText.text += c;
            
            if (c == brackStart)
            {
                while (c != brackEnd)
                {
                    c = charQueue.Dequeue();
                    dialogueText.text += c;
                }
            }

            //yield return null;
        }
        //yield return new WaitForSeconds(.001f);
    //  UpdateBoxes();
        yield return null;

    }

    private IEnumerator EndDialogueCor()
    {
        yield return new WaitForSeconds(4);
        dialogueText.text = "";
    }

    void UpdateBoxes()
    {
        Canvas.ForceUpdateCanvases();
 //     box.sizeDelta = new Vector2(700, 55 + (45f * overflow.textInfo.lineCount));
        for (int i = 0 ; i<choiceButtons.Length;i++)
        {
            RectTransform rect = choiceButtons[i].GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(1400, -48 * choiceButtons[i].textInfo.lineCount);
            if (i > 0)
            {
                RectTransform rectold = choiceButtons[i-1].GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rectold.anchoredPosition.y + (20f+(rectold.sizeDelta.y) + (rect.sizeDelta.y / 2f)));
            }
        }
    }
}
