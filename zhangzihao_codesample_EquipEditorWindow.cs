using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class EquipEditorWindow : EditorWindow
{


    //------------------------------------------This is an unity editor tool I made to make equipment assets-----------//
    //------------I have equipment models, but they come from different packs I bought from artists, all the size, center, rotation are different. ---------------------------//
    //---the equipment editor tool I made empowers me to make any equipment asset within 20 minutes. -----//
    //--It describes which one the model is, how it displays in the character's hands, icon and item detail page, which pool of magic stats should it fetch from.----//


    public equipdata equipdata;
    public GameObject prefab;
    public GameObject spawnanchor_icon;
  
    public GameObject iconparent;

    public GameObject _obj_at_icon;


    public playermountreferer playerskin;
    public int mount;
    public GameObject _obj_at_player;


    public GameObject _obj_at_displaytab;
    public GameObject spawnanchor_tabdisplay;
    public GameObject _displaytab_parent;



    [MenuItem("Window/Equip Editor")]


    static void Init()
    {
        EquipEditorWindow window = (EquipEditorWindow)EditorWindow.GetWindow(typeof(EquipEditorWindow));
        window.Show();
    }


    Vector2 scrollPosition = Vector2.zero; 
    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);//, GUILayout.Width(100), GUILayout.Height(100));

        //render field: default data container
        EditorGUILayout.BeginHorizontal();
        equipdata = (equipdata)EditorGUILayout.ObjectField("equipdata", equipdata, typeof(Object), true);
        EditorGUILayout.EndHorizontal();

        //render field: in-scene background obj
        EditorGUILayout.BeginHorizontal();
        spawnanchor_icon = (UnityEngine.GameObject)EditorGUILayout.ObjectField("spawnanchor_icon", spawnanchor_icon, typeof(Object), true);
        EditorGUILayout.EndHorizontal();

        //render testobj object field in inspector
        EditorGUILayout.BeginHorizontal();
        prefab = (UnityEngine.GameObject)EditorGUILayout.ObjectField("prefab", prefab, typeof(Object), true);
        EditorGUILayout.EndHorizontal();

        //render testobj object field in inspector
        EditorGUILayout.BeginHorizontal();
        iconparent = (UnityEngine.GameObject)EditorGUILayout.ObjectField("iconparent", iconparent, typeof(Object), true);
        EditorGUILayout.EndHorizontal();
        //render testobj object field in inspector
        EditorGUILayout.BeginHorizontal();
        _obj_at_icon = (UnityEngine.GameObject)EditorGUILayout.ObjectField("_obj_at_icon", _obj_at_icon, typeof(Object), true);
        EditorGUILayout.EndHorizontal();

        if ((GUILayout.Button("[test]place prefab icon")))
        {
            if(_obj_at_icon!=null)
            {
                DestroyImmediate(_obj_at_icon, false);
            }
            _obj_at_icon = Instantiate(prefab, spawnanchor_icon.transform.position, spawnanchor_icon.transform.rotation);
            _obj_at_icon.transform.SetParent(iconparent.transform);
            _obj_at_icon.transform.localScale = spawnanchor_icon.transform.localScale;
        }
        if ((GUILayout.Button("[write]prefab icon-remember offset")))
        {
            equipdata.prefab_icon_pos_offset = _obj_at_icon.transform.position - spawnanchor_icon.transform.position;
            equipdata.prefab_icon_rotation_offset = Quaternion.Inverse(spawnanchor_icon.transform.rotation) * _obj_at_icon.transform.rotation;
            equipdata.prefab_icon_scale = _obj_at_icon.transform.localScale;
            equipdata.prefab = prefab;
            EditorUtility.SetDirty(equipdata);
        }
        if ((GUILayout.Button("[read]debug-generate modified prefab icon")))
        {
            if (_obj_at_icon != null)
            {
                DestroyImmediate(_obj_at_icon, false);
            }
            _obj_at_icon = Instantiate(equipdata.prefab, spawnanchor_icon.transform.position+equipdata.prefab_icon_pos_offset, spawnanchor_icon.transform.rotation*equipdata.prefab_icon_rotation_offset);

            _obj_at_icon.transform.SetParent(iconparent.transform);
            _obj_at_icon.transform.localScale = equipdata.prefab_icon_scale;
        }


        //render testobj object field in inspector
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextArea("Use Below to set player mount offset.");
        EditorGUILayout.EndHorizontal();
        //render testobj object field in inspector
        EditorGUILayout.BeginHorizontal();
        playerskin = (playermountreferer)EditorGUILayout.ObjectField("playerskin", playerskin, typeof(Object), true);
        EditorGUILayout.EndHorizontal();
        //render testobj object field in inspector
        EditorGUILayout.BeginHorizontal();
        mount = (int)EditorGUILayout.IntField("mount", mount);
        EditorGUILayout.EndHorizontal();
        //render testobj object field in inspector
        EditorGUILayout.BeginHorizontal();
        _obj_at_player = (UnityEngine.GameObject)EditorGUILayout.ObjectField("debug: _obj_at_player", _obj_at_player, typeof(Object), true);
        EditorGUILayout.EndHorizontal();

        if ((GUILayout.Button("place prefab on player")))
        {
            if (_obj_at_player != null)
            {
                DestroyImmediate(_obj_at_player, false);
            }

            GameObject spawnpoint = new GameObject();
            if(mount ==0)
            {
                spawnpoint = playerskin.prefab_mount_weapon_L;
            }
            if (mount == 1)
            {
                spawnpoint = playerskin.prefab_mount_weapon_R;
            }
            if (mount == 2)
            {
                spawnpoint = playerskin.prefab_mount_head;
            }
            if (mount == 3)
            {
                spawnpoint = playerskin.prefab_mount_armor;
            }


            _obj_at_player = Instantiate(prefab, spawnpoint.transform.position, spawnpoint.transform.rotation);
            _obj_at_player.transform.SetParent(spawnpoint.transform);
            _obj_at_player.transform.localScale = spawnpoint.transform.localScale;
        }

        if ((GUILayout.Button("[write]prefab onplayer-remember offset")))
        {
            equipdata.mount = mount;
            GameObject spawnpoint = new GameObject();
            if (mount == 0)
            {
                spawnpoint = playerskin.prefab_mount_weapon_L;
            }
            if (mount == 1)
            {
                spawnpoint = playerskin.prefab_mount_weapon_R;
            }
            if (mount == 2)
            {
                spawnpoint = playerskin.prefab_mount_head;
            }
            if (mount == 3)
            {
                spawnpoint = playerskin.prefab_mount_armor;
            }
            equipdata.prefab_onhero_offset_pos = _obj_at_player.transform.position - spawnpoint.transform.position;
            equipdata.prefab_onhero_offset_rot = Quaternion.Inverse(spawnpoint.transform.rotation) * _obj_at_player.transform.rotation;
            equipdata.prefab_onhero_scale = _obj_at_player.transform.localScale;
            equipdata.prefab = prefab;
            EditorUtility.SetDirty(equipdata);
        }
        if ((GUILayout.Button("[read]debug-generate modified prefab on player")))
        {
            if (_obj_at_player != null)
            {
                DestroyImmediate(_obj_at_player, false);
            }
            GameObject spawnpoint = new GameObject();
            if (equipdata.mount == 0)
            {
                spawnpoint = playerskin.prefab_mount_weapon_L;
            }
            if (equipdata.mount == 1)
            {
                spawnpoint = playerskin.prefab_mount_weapon_R;
            }
            if (equipdata.mount == 2)
            {
                spawnpoint = playerskin.prefab_mount_head;
            }
            if (equipdata.mount == 3)
            {
                spawnpoint = playerskin.prefab_mount_armor;

                GameObject spawnpoint_R  = playerskin.prefab_mount_armor_R;
                //Quaternion shoulder_L_R_offset = Quaternion.Inverse(spawnpoint_R.transform.rotation) * spawnpoint.transform.rotation;
                GameObject _armor_R_on_player = Instantiate(equipdata.prefab, spawnpoint.transform.position + equipdata.prefab_onhero_offset_pos, spawnpoint.transform.rotation * equipdata.prefab_onhero_offset_rot);
                _armor_R_on_player.transform.SetParent(spawnpoint.transform);
                _armor_R_on_player.transform.localScale = equipdata.prefab_onhero_scale;

                _armor_R_on_player.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);

                Vector3 newlocalPos = new Vector3(_armor_R_on_player.transform.localPosition.x, _armor_R_on_player.transform.localPosition.y, _armor_R_on_player.transform.localPosition.z * (-1f));
                _armor_R_on_player.transform.SetParent(spawnpoint_R.transform);
                _armor_R_on_player.transform.localPosition = newlocalPos;
            }
            _obj_at_player = Instantiate(equipdata.prefab, spawnpoint.transform.position + equipdata.prefab_onhero_offset_pos, spawnpoint.transform.rotation * equipdata.prefab_onhero_offset_rot);

            _obj_at_player.transform.SetParent(spawnpoint.transform);
            _obj_at_player.transform.localScale = equipdata.prefab_onhero_scale;
        }




        //render testobj object field in inspector
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextArea("Use Below to set tabdisplay offset.");
        EditorGUILayout.EndHorizontal();
        //render field: in-scene background obj
        EditorGUILayout.BeginHorizontal();
        spawnanchor_tabdisplay = (UnityEngine.GameObject)EditorGUILayout.ObjectField("spawnanchor_tabdisplay", spawnanchor_tabdisplay, typeof(Object), true);
        EditorGUILayout.EndHorizontal();

        //render testobj object field in inspector
        EditorGUILayout.BeginHorizontal();
        _displaytab_parent = (UnityEngine.GameObject)EditorGUILayout.ObjectField("_displaytab_parent", _displaytab_parent, typeof(Object), true);
        EditorGUILayout.EndHorizontal();
        //render testobj object field in inspector
        EditorGUILayout.BeginHorizontal();
        _obj_at_displaytab = (UnityEngine.GameObject)EditorGUILayout.ObjectField("_obj_at_displaytab", _obj_at_displaytab, typeof(Object), true);
        EditorGUILayout.EndHorizontal();

        if ((GUILayout.Button("[test]place prefab tabdisplay")))
        {
            if (_obj_at_displaytab != null)
            {
                DestroyImmediate(_obj_at_displaytab, false);
            }
            _obj_at_displaytab = Instantiate(prefab, spawnanchor_tabdisplay.transform.position, spawnanchor_tabdisplay.transform.rotation);
            _obj_at_displaytab.transform.SetParent(_displaytab_parent.transform);
            _obj_at_displaytab.transform.localScale = spawnanchor_tabdisplay.transform.localScale;
        }
        if ((GUILayout.Button("[write]prefab tabdisplay-remember offset")))
        {
            equipdata.prefab_displaytab_offset_pos = _obj_at_displaytab.transform.position - spawnanchor_tabdisplay.transform.position;
            equipdata.prefab_displaytab_offset_rot = Quaternion.Inverse(spawnanchor_tabdisplay.transform.rotation) * _obj_at_displaytab.transform.rotation;
            equipdata.prefab_displaytab_scale = _obj_at_displaytab.transform.localScale;
            //equipdata.prefab = prefab;
            EditorUtility.SetDirty(equipdata);
        }
        if ((GUILayout.Button("[read]debug-generate modified prefab tabdisplay")))
        {
            if (_obj_at_displaytab != null)
            {
                DestroyImmediate(_obj_at_displaytab, false);
            }
            _obj_at_displaytab = Instantiate(equipdata.prefab, spawnanchor_tabdisplay.transform.position + equipdata.prefab_displaytab_offset_pos, spawnanchor_tabdisplay.transform.rotation * equipdata.prefab_displaytab_offset_rot);

            _obj_at_displaytab.transform.SetParent(_displaytab_parent.transform);
            _obj_at_displaytab.transform.localScale = equipdata.prefab_displaytab_scale;
        }







        GUILayout.EndScrollView();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
