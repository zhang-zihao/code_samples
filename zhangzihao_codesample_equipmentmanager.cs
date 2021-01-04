using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equipmentmanager : MonoBehaviour {


    //---------Equipment management class with a custom decoder-encoder------//
    //---------This class handles all equipment-related stuff. Talks to datamanager and display manager(EquipmentmanagerUI)------//
    //---------Data consumption: 8 char to fully describe a equipment, including exp towards equipment-evolution--------//


    public equipmentmanagerUI equipmentmanagerUI;
    //client static equipment data
    public allequipmentdata allequipmentdata;
    public allequipmentdata allequipdata_magicstat1;
    public allequipmentdata allequipdata_magicstat2;
    //parse raw equipment string
    [SerializeField]public string _rawstring;
    string _littlestring;

    [SerializeField] public string _rawstring_equipped;
    string _littlestring_equipped;

    public List<string> equipments_equipped;
    public List<string> equipments_notequipped;

    //
    public GameEvent event_changeequipments;//triggers eventlisteners in :UI
    public GameEvent event_addequipment;
    public GameEvent event_deleteequipment;
    public GameEvent event_upload;//triggers eventlisteners in :data manager or data management

    public equip_upgrade_ratios equip_Upgrade_Ratios;
    public List<RarityInfo> rarityInfos;

    public List<effectdata> active_effects()
    {
        List<effectdata> _list = new List<effectdata>();
        foreach(string s in equipments_equipped)
        {
            //d: equip stat0_basicstat   m1: equip stat1_magicstat    m2: equip stat2_magicstat
            //fetch part1 data object [_ _ _ d _ _ _ _]
            equipdata d = gamemanager.GM.equipmentmanager.allequipmentdata.types[ASCIIToInt(s[0])].subtype[ASCIIToInt(s[1])].rarity[ASCIIToInt(s[2])].weapondatas[ASCIIToInt(s[3])];
            //fetch part2 data object [_ _ _ _ d _ _ _]
            equipdata m1 = gamemanager.GM.equipmentmanager.allequipdata_magicstat1.types[ASCIIToInt(s[0])].subtype[ASCIIToInt(s[1])].rarity[ASCIIToInt(s[2])].weapondatas[ASCIIToInt(s[4])];
            //fetch part1 data object [_ _ _ _ _ d _ _]
            equipdata m2 = gamemanager.GM.equipmentmanager.allequipdata_magicstat2.types[ASCIIToInt(s[0])].subtype[ASCIIToInt(s[1])].rarity[ASCIIToInt(s[2])].weapondatas[ASCIIToInt(s[5])];

            foreach(effectdata _e1 in d.Effects)
            {
                _list.Add(_e1);
            }
            foreach (effectdata _e2 in m1.Effects)
            {
                _list.Add(_e2);
            }
            foreach (effectdata _e3 in m2.Effects)
            {
                _list.Add(_e3);
            }

            if(ASCIIToInt(s[2])==4)
            {
                if(ASCIIToInt(s[6]) == 34)
                {
                    foreach (effectdata _e1 in d.Hidden_Effects)
                    {
                        _list.Add(_e1);
                    }
                    foreach (effectdata _e2 in m1.Hidden_Effects)
                    {
                        _list.Add(_e2);
                    }
                    foreach (effectdata _e3 in m2.Hidden_Effects)
                    {
                        _list.Add(_e3);
                    }
                }
            }
            else
            {
                if(ASCIIToInt(s[7]) == 9)
                {
                    foreach (effectdata _e1 in d.Hidden_Effects)
                    {
                        _list.Add(_e1);
                    }
                    foreach (effectdata _e2 in m1.Hidden_Effects)
                    {
                        _list.Add(_e2);
                    }
                    foreach (effectdata _e3 in m2.Hidden_Effects)
                    {
                        _list.Add(_e3);
                    }
                }
            }
        }
        return _list;
    }

    public void ApplyEquipmentBasicStats_ToPlayer()
    {
        foreach(string s in equipments_equipped)
        {
            //d: equip stat0_basicstat   m1: equip stat1_magicstat    m2: equip stat2_magicstat
            //fetch part1 data object [_ _ _ d _ _ _ _]
            equipdata d = gamemanager.GM.equipmentmanager.allequipmentdata.types[ASCIIToInt(s[0])].subtype[ASCIIToInt(s[1])].rarity[ASCIIToInt(s[2])].weapondatas[ASCIIToInt(s[3])];
            //fetch part2 data object [_ _ _ _ d _ _ _]
            equipdata m1 = gamemanager.GM.equipmentmanager.allequipdata_magicstat1.types[ASCIIToInt(s[0])].subtype[ASCIIToInt(s[1])].rarity[ASCIIToInt(s[2])].weapondatas[ASCIIToInt(s[4])];
            //fetch part1 data object [_ _ _ _ _ d _ _]
            equipdata m2 = gamemanager.GM.equipmentmanager.allequipdata_magicstat2.types[ASCIIToInt(s[0])].subtype[ASCIIToInt(s[1])].rarity[ASCIIToInt(s[2])].weapondatas[ASCIIToInt(s[5])];

            if(ASCIIToInt(s[2])!=4)
            {
                float multiplier = 1;
                for (int i = 0; i < ASCIIToInt(s[7]);i++)
                {
                    multiplier *= (equip_Upgrade_Ratios.type_Basic_Stats_Upgrade_Ratios[ASCIIToInt(s[0])].type_ratios[i]);
                }
                d.BasicStat.ModifyPlayerStats_WithEquipmentLevels(multiplier);
            }
            else
            {
                float multiplier = 1;
                if(ASCIIToInt(s[6])==34)
                {
                    multiplier *= (equip_Upgrade_Ratios.type_EpicEvolve_Basic_Stats_Upgrade_Ratios[ASCIIToInt(s[0])]);
                }
                d.BasicStat.ModifyPlayerStats_WithEquipmentLevels(multiplier);

            }

        }
    }

    public void display_equipments_on_player(playermountreferer mountreferer)
    {
        foreach (string s in equipments_equipped)
        {
            //d: equip stat0_basicstat   m1: equip stat1_magicstat    m2: equip stat2_magicstat
            //fetch part1 data object [_ _ _ d _ _ _ _]
            equipdata d = gamemanager.GM.equipmentmanager.allequipmentdata.types[ASCIIToInt(s[0])].subtype[ASCIIToInt(s[1])].rarity[ASCIIToInt(s[2])].weapondatas[ASCIIToInt(s[3])];
            //fetch part2 data object [_ _ _ _ d _ _ _]
            equipdata m1 = gamemanager.GM.equipmentmanager.allequipdata_magicstat1.types[ASCIIToInt(s[0])].subtype[ASCIIToInt(s[1])].rarity[ASCIIToInt(s[2])].weapondatas[ASCIIToInt(s[4])];
            //fetch part1 data object [_ _ _ _ _ d _ _]
            equipdata m2 = gamemanager.GM.equipmentmanager.allequipdata_magicstat2.types[ASCIIToInt(s[0])].subtype[ASCIIToInt(s[1])].rarity[ASCIIToInt(s[2])].weapondatas[ASCIIToInt(s[5])];

            if(d.displayonhero)
            {
                if(d.mount ==0)//left hand
                {
                    Transform mountpoint = mountreferer.prefab_mount_weapon_L.transform;
                    GameObject _weapon_on_player = Instantiate(d.prefab, mountpoint.position, mountpoint.rotation);
                    _weapon_on_player.transform.SetParent(mountpoint.transform);
                    _weapon_on_player.transform.position = _weapon_on_player.transform.position + d.prefab_onhero_offset_pos;//+ mountreferer.playerdifference_offset_pos;
                    _weapon_on_player.transform.rotation = _weapon_on_player.transform.rotation * d.prefab_onhero_offset_rot;//*mountreferer.playerdifference_offset_rot;

                    _weapon_on_player.transform.localScale = d.prefab_onhero_scale+mountreferer.playerdifference_offset_scale;
                }
                if (d.mount == 1)//right hand
                {
                    Transform mountpoint = mountreferer.prefab_mount_weapon_R.transform;
                    GameObject _weapon_on_player = Instantiate(d.prefab, mountpoint.position, mountpoint.rotation);
                    _weapon_on_player.transform.SetParent(mountpoint.transform);
                    _weapon_on_player.transform.position = _weapon_on_player.transform.position + d.prefab_onhero_offset_pos;// + mountreferer.playerdifference_offset_pos;
                    _weapon_on_player.transform.rotation = _weapon_on_player.transform.rotation * d.prefab_onhero_offset_rot;// * mountreferer.playerdifference_offset_rot;

                    _weapon_on_player.transform.localScale = d.prefab_onhero_scale + mountreferer.playerdifference_offset_scale;
                }
                if (d.mount == 2)//head
                {
                    Transform mountpoint = mountreferer.prefab_mount_head.transform;
                    GameObject _weapon_on_player = Instantiate(d.prefab, mountpoint.position, mountpoint.rotation);


                    _weapon_on_player.transform.SetParent(mountpoint.transform);

                    _weapon_on_player.transform.position = _weapon_on_player.transform.position + d.prefab_onhero_offset_pos;// + mountreferer.playerdifference_offset_pos;
                    _weapon_on_player.transform.rotation = _weapon_on_player.transform.rotation * d.prefab_onhero_offset_rot;// * mountreferer.playerdifference_offset_rot;
                    _weapon_on_player.transform.localScale = d.prefab_onhero_scale + mountreferer.playerdifference_offset_scale;
                }
                if (d.mount == 3)//armor
                {
                    Transform mountpoint = mountreferer.prefab_mount_armor.transform;
                    GameObject _weapon_on_player = Instantiate(d.prefab, mountpoint.position, mountpoint.rotation);
                    _weapon_on_player.transform.SetParent(mountpoint.transform);
                    _weapon_on_player.transform.position = _weapon_on_player.transform.position + d.prefab_onhero_offset_pos;// + mountreferer.playerdifference_offset_pos;
                    _weapon_on_player.transform.rotation = _weapon_on_player.transform.rotation * d.prefab_onhero_offset_rot;// * mountreferer.playerdifference_offset_rot;
                   
                    _weapon_on_player.transform.localScale = d.prefab_onhero_scale + mountreferer.playerdifference_offset_scale;


                    Transform mountpoint_R = mountreferer.prefab_mount_armor_R.transform;


                    GameObject _armor_R_on_player = Instantiate(d.prefab, mountpoint.position + d.prefab_onhero_offset_pos, mountpoint.rotation * d.prefab_onhero_offset_rot);
                    _armor_R_on_player.transform.SetParent(mountpoint.transform);
                    _armor_R_on_player.transform.localScale = d.prefab_onhero_scale;

                    _armor_R_on_player.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);

                    Vector3 newlocalPos = new Vector3(_armor_R_on_player.transform.localPosition.x, _armor_R_on_player.transform.localPosition.y, _armor_R_on_player.transform.localPosition.z * (-1f));
                    _armor_R_on_player.transform.SetParent(mountpoint_R.transform);
                    _armor_R_on_player.transform.localPosition = newlocalPos;
                }
            }
        }

    }




    //two-way string management: 1/get string from gamemanager and parse into equipments 2/get equipments and write into strings, give gamemanager
    //only raw string in-and-out
    //*!!!!!!!!!!! BASIC RAW_EQUIP_DATA STRUCTURE *this is the rule of decoding rawstring
    //SLOT 0: type                             ||
    //SLOT 1: subtype(weapon)                  ||
    //SLOT 2: rarity                           ||
    //SLOT 3: item_part_1                      ||main part of the weapon, determines rule of gaining exp       
    //SLOT 4: item_part_2                      ||
    //SLOT 5: item_part_3                      ||
    //SLOT 6 & 7: percentage of weapon exp     ||

    //public static equipmentmanager EM;  //make it a singleton
    //void Awake()
    //{
    //    if (EM != null) Destroy(EM);
    //    else EM = this;
    //    DontDestroyOnLoad(this);
    //}
    //this function is invoked by gamemanager/datamanager: gotdata-populatedata event
    public void Parsedataintoequipment()
    {
        //refresh equipments_notequipped from _rawstring
        equipments_notequipped.Clear();
        int j = 0;
        _littlestring = "";
        if(_rawstring!=null)
        {
            for (int i = 0; i < _rawstring.Length; i++)
            {
                _littlestring = _littlestring + _rawstring[i];
                j++;
                if (j == 8)
                {
                    equipments_notequipped.Add(_littlestring);
                    j = 0;
                    _littlestring = "";
                }
            }
            //
        }


        //refresh equipments_equipped from _rawstring_equipped
        equipments_equipped.Clear();
        int k = 0;
        _littlestring_equipped = "";
        if(_rawstring_equipped!=null)
        {
            for (int i = 0; i < _rawstring_equipped.Length; i++)
            {
                _littlestring_equipped = _littlestring_equipped + _rawstring_equipped[i];
                k++;
                if (k == 8)
                {
                    equipments_equipped.Add(_littlestring_equipped);
                    k = 0;
                    _littlestring_equipped = "";
                }
            }
        }

        //

    }

    //this function is invoked when client changes anything in equipment
    public void Writeequipmentintodata()
    {
         _littlestring = "";
        foreach(string s in equipments_notequipped)
        {
            _littlestring = _littlestring + s;
        }
        _rawstring = _littlestring;




        _littlestring_equipped = "";
        foreach (string s in equipments_equipped)
        {
            _littlestring_equipped = _littlestring_equipped + s;
        }
        _rawstring_equipped = _littlestring_equipped;
    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //equipment data DECODER
    public int ASCIIToInt(char c)
    {
        int value = 0;
        if (c <= 57)
        {
            value = c - 48;
        }
        if (c >= 65 && c <= 90)
        {
            value = c - 65 + 10;
        }
        if (c >= 97)
        {
            value = c - 97 + 10 + 26;
        }
        return value;
    }
    //equipment data ENCODER
    public char IntToASCII(int i)
    {
        int _int = 0;
        if (i <= 9)
        { _int = i + 48; }
        if (i >= 10 && i < 36)
        { _int = i + 48 + 7; }
        if (i >= 36)
        { _int = i + 48 + 7 + 6; }
        char value = new char();
        value = (char)_int;
        return value;
    }
}

[System.Serializable]
public class RarityInfo
{
    [SerializeField]
    public string name;
    [SerializeField]
    public Color color;
}
