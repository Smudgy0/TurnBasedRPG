using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public TeamManager TM;

    public TextMeshProUGUI[] CHARSHPUI;
    public TextMeshProUGUI[] CHARSNAMEUI;

    
    public Image[] CHARSSPRITE;
    public Image[] CHARSFieldSPRITE;

    public Image[] ENEMIESSPRITE;

    [SerializeField] public Image[] CHARSHPBARUI;

    void Start()
    {
        InitializeCHARS();
        InitializeENEMIES();
    }

    void InitializeCHARS()
    {
        for (int i = 0; i < TM.CHARS.Length; i++)
        {
            CHARSHPUI[i].text = $"{TM.CHARS[i].CharacterHP.ToString()} / {TM.CHARS[i].CharacterHP.ToString()}";
            CHARSNAMEUI[i].text = $"{TM.CHARS[i].CharacterName.ToString()}";
            CHARSSPRITE[i].sprite = TM.CHARS[i].CharacterSprite;
            CHARSFieldSPRITE[i].sprite = TM.CHARS[i].CharacterSprite;
        }
    }

    void InitializeENEMIES()
    {
        for (int i = 0; i < TM.ENEMIES.Length; i++)
        {
            ENEMIESSPRITE[i].sprite = TM.ENEMIES[i].CharacterSprite;
        }
    }

    void Update()
    {
        for (int i = 0; i < TM.CHARS.Length; i++)
        {
            //CHARSHPPROGRESSBARUI[i].fillAmount = (float)TM.CHARS[i].CharacterHP / (float)TM.CHARS[i].CharacterMAXHP;
            CHARSHPUI[i].text = $"{TM.CHARS[i].CharacterHP.ToString()} / {TM.CHARS[i].CharacterMAXHP.ToString()}";
        }
    }
}
