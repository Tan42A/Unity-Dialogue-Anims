using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using Unity.VisualScripting;

public class DialogueWithAnims : MonoBehaviour
{
    private static DialogueWithAnims instance;
   
    [Header("UI Settings")]
    //[SerializeField] private GameObject dialoguePanel;
    //[SerializeField] private GameObject homeOptions;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI dialogue;

    //[Header("Continue Button")]
    //[SerializeField] private Button ctnButton;
    //[SerializeField] private string nextScene;

    [Header("Dialogue File")]
    [SerializeField] private TextAsset dialogueFile;

    private Story currentStory;

    [Header("Dialogue Tags")]
    [SerializeField] private string c1Tag = "anim_c1_";
    [SerializeField] private string c2Tag = "anim_c2_";
    [SerializeField] private string namePrefix = "anim"; 
    [SerializeField] private List<string> c1Animations = new List<string>();
    [SerializeField] private List<string> c2Animations = new List<string>();
    [SerializeField] private List<string> nameTags = new List<string>();
    private List<string> alltags = new List<string>();

    [Header("Animation Controllers")]
    [SerializeField] private GameObject character1;
    [SerializeField] private GameObject character2;


    private void Awake()
    {
        // Warning for multiple instances of the dialogue system
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue System in scene");
        }
        instance = this;
    }

    public static DialogueWithAnims GetInstance()
    {
        return instance;
    }

    void Start()
    {

        currentStory = new Story(dialogueFile.text);
        EnterDialogueMode();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            EnterDialogueMode();
        }
        
    }

    private void EnterDialogueMode()
    {
        if (currentStory.canContinue)
        {
            dialogue.text = currentStory.Continue();
            PopulateNameAnimTags();
            AnimationPlayer();


            if (nameTags.Count > 0)
            {
                speakerName.text = nameTags[0];
            }
            else
            {
                speakerName.text = "";
            }
        }
    }

    private void AnimationPlayer()
    {
        //Plays the animation in the tag when it matches a character tag. Add a trim if statement for name prefix if using one.
        if (c1Animations.Count > 0 && c1Animations[0].StartsWith(c1Tag))
        {
            string clipName = c1Animations[0].TrimStart(c1Tag);

            character1.GetComponent<Animator>().Play(clipName);
        }

        if (c2Animations.Count > 0 && c2Animations[0].StartsWith(c2Tag))
        {
            string clipName = c2Animations[0].TrimStart(c2Tag);

            character2.GetComponent<Animator>().Play(clipName);
        }
    }

    private void PopulateNameAnimTags()
    {
        // parses through tags for each line and splits them between animation and name tags
        alltags = currentStory.currentTags;

        if (alltags.Count > 0)
        {
            foreach (string tag in alltags)
            {

                if (tag.StartsWith(c1Tag))
                {
                    c1Animations.Clear();
                    c1Animations.Add(tag);
                }

                if (tag.StartsWith(c2Tag))
                {
                    c2Animations.Clear();
                    c2Animations.Add(tag);
                }

                if (!tag.StartsWith(namePrefix)) // Would need to remove the ! if using a name prefix.
                {
                    nameTags.Clear();
                    nameTags.Add(tag);
                }
            }
        }
        else
        {
            c1Animations.Clear();
            c2Animations.Clear();
            nameTags.Clear();
        }
    }
}
