using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TeamManager TM;
    public BattleManager BM;

    public TextMeshProUGUI[] CHARSHPUI;
    public TextMeshProUGUI[] CHARSNAMEUI;

    
    public Image[] CHARSSPRITE;
    public Image[] CHARSFieldSPRITE;

    public Image[] ENEMIESSPRITE;
    public Sprite DEADORINVALID;

    [SerializeField] public Image[] CHARSHPBARUI;

    public Image[] TURNSYSTEMSPRITES;
    public GameObject PLAYERBUTTONS;
    public GameObject PLAYERTARGETBUTTONS;

    void Start()
    {
        
    }

    public void Initialize()
    {
        InitializeTURNUI();
        InitializeCHARS();
        InitializeENEMIES();
    }

    void InitializeCHARS()
    {
        for (int i = 0; i < TM.CHARS.Length; i++)
        {
            CHARSHPUI[i].text = $"{TM.CHARS[i].CharacterHP.ToString()} / {TM.CHARS[i].CharacterHP.ToString()}";
            CHARSNAMEUI[i].text = $"{TM.CHARS[i].CharacterName.ToString()}";
            CHARSSPRITE[i].sprite = TM.CHARS[i].CharacterBattleSprite;
            CHARSFieldSPRITE[i].sprite = TM.CHARS[i].CharacterBattleSprite;
        }
    }

    public void InitializeENEMIES()
    {
        for (int i = 0; i < TM.ENEMIES.Length; i++)
        {
            ENEMIESSPRITE[i].sprite = TM.ENEMIES[i].CharacterBattleSprite;
        }
    }

    public void InitializeTURNUI()
    {

        for (int i = 0; i < BM.BattleOrder.Length; i++)
        {
            BM.BattleOrder[i].CharacterBattleSprite = BM.BattleOrder[i].CharacterSprite;
            TURNSYSTEMSPRITES[i].sprite = BM.BattleOrder[i].CharacterBattleSprite;
        }
    }

    void Update()
    {
        for (int i = 0; i < TM.CHARS.Length; i++)
        {
            CHARSHPBARUI[i].fillAmount = (float)TM.CHARS[i].CharacterHP / (float)TM.CHARS[i].CharacterMAXHP;
            CHARSHPUI[i].text = $"{TM.CHARS[i].CharacterHP.ToString()} / {TM.CHARS[i].CharacterMAXHP.ToString()}";
        }
    }
}
