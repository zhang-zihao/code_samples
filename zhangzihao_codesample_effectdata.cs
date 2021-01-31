using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "New EffectData", menuName = "effect data", order = 2)]
public class effectdata : ScriptableObject {


    //--------------This is a scriptable object I use for King Quest, my recent project--------------//
    //--------------effectdata contains is passed around the game, When used, ‘effects objects’ behave differently on each scenario. --//
    //--------------When an equipment needs magic stats, a skill tree needs an attack bonus attribute, a weapon has a special projectile with a special vfx, and on skill-selects, ‘effects objects’ fit into slots on them. ---//
    //--------------a system capable of handling display, upgrade, data, and all other aspects, because ‘effects objects’ can be passed around the game.---//



    //---------Fill-in areas for developers, with instructions on unity editor
    [SerializeField] private string effectname;
    [SerializeField] private Sprite displayimage;
    [SerializeField] private string description;
    [Header("display in equipment detail UI")]
    [SerializeField] private bool hide_in_equipdetailUI;
    [Header("is this effect stackable?")]
    [SerializeField] private bool non_stackable;//default is stackable
    [Header("projectile launching effects")]
    [SerializeField] private bool frontshoot_plus1;
    [SerializeField] private bool continuousshoot_plus1;
    [Header("projectile effects")]
    [SerializeField] private bool define_projectile_skin;
    [SerializeField] private int skin_projectile_code;
    [SerializeField] private int replace_attack_animation_with_controller_backupanimation_code;//0 is default
    [SerializeField] private int replace_idle_animation_with_controller_backupanimation_code;//0 is default
    [SerializeField] private bool pierceenemy;
    [SerializeField] private bool bouncewall;
    [SerializeField] private bool bounceenemy;
    [SerializeField] private bool splitinghit;
    [Header("on-enemy effects")]
    [SerializeField] private bool fire_low;
    [SerializeField] private float fire_low_counter_max;
    [SerializeField] private float fire_low_damage_1time;
    [SerializeField] private float fire_low_damage_lasting;
    [SerializeField] private float fire_low_applylastingdamage_counter_max;
    [SerializeField] private bool poison_low;
    [SerializeField] private float poison_low_counter_max;
    [SerializeField] private float poison_low_damage_1time;
    [SerializeField] private float poison_low_damage_lasting;
    [SerializeField] private float poison_low_applylastingdamage_counter_max;
    [SerializeField] private bool ice_low;
    [SerializeField] private float ice_low_counter_max;
    [SerializeField] private float ice_low_damage_1time;
    [SerializeField] private float ice_low_damage_lasting;
    [SerializeField] private float ice_low_applylastingdamage_counter_max;
    [Header("modify enemy-death effects")]
    [SerializeField] private bool visual_dieice;
    [SerializeField] private int visual_dieice_vfxcode;
    [Header("modify player stats when added")]
    [SerializeField] private float fullhealth_bonus;
    [SerializeField] private float damage_bonus;
    [SerializeField] private float critical_chance_bonus;//(random between 0,1) if is 1, 100% chance to crit//0.01 for 1%, and in description write 1.
    [SerializeField] private float critical_damage_multiplyby_bonus;//0.01 for 1%, and in description write 1.
    [SerializeField] private float attack_speed_bonus;
    [SerializeField] private float resist_normaldamage_bonus;
    [SerializeField] private float resist_fire_bonus;
    [SerializeField] private bool immune_fire;
    [SerializeField] private float resist_poison_bonus;
    [SerializeField] private bool immune_poison;
    [SerializeField] private float resist_ice_bonus;
    [SerializeField] private bool immune_ice;
    [SerializeField] private bool heal_health;
    [SerializeField] private float heal_health_rate;//how much % of the full health should be healed. 0.1 = 10%, 1.0 = 100%. 
    [Header("modify earnings")]
    [SerializeField] private float earning_coin_multiplier_bonus;
    [SerializeField] private float earning_equip_multiplier_bonus;

    public string Effectname
    {
        get
        {
            return effectname;
        }
    }
    public Sprite Displayimage
    {
        get
        {
            return displayimage;
        }
    }
    public string Description
    {
        get
        {
            return description;
        }
    }
    public bool Hide_in_equipdetailUI
    {
        get
        {
            return hide_in_equipdetailUI;
        }
    }
    public bool Non_stackable
    {
        get
        {
            return non_stackable;
        }
    }

    //------On-player effects-------//
    public void PlayerBehavior(GameObject playerobj)//the playerobj refers to controller.gameObject, not playerparent or player
    {
        if(frontshoot_plus1)
        {
            levelmanager.LM.controller.arrowspershot++;
        }
        if(continuousshoot_plus1)
        {
            levelmanager.LM.controller.continuousshot++;
        }
    }
    //------On-projectile effects-------//
    public void Projectile(GameObject projectileobj)
    {
        var _component = projectileobj.GetComponent<arrow>();

        if(define_projectile_skin)
        {
            _component.skins[_component.defaultskin].SetActive(false);
            _component.skins[skin_projectile_code].SetActive(true);
        }
        //physics effects
        if (pierceenemy)
        {
            _component.pierceenemy = true;
        }
        if(bouncewall)
        {
            _component.bouncewall = true;
        }
        if(bounceenemy)
        {
            _component.bounceenemy = true;
        }
        if(splitinghit)
        {
            _component.splitinghit = true;
        }
        //element effects
        if(fire_low)
        {
            _component.fire_low = true;
        }
        if(poison_low)
        {
            _component.poison_low = true;
        }
        if(ice_low)
        {
            _component.ice_low = true;
        }
    }
    //------On-enemy effects-------//
    public void Enemy(GameObject enemyobj)
    {
        var _component = enemyobj.GetComponent<enemycontroller>();
        if(fire_low)
        {
            if(!_component.immune_fire)//"set on fire" effect
            {
                _component.fire_low = true;
                _component.fire_low_counter_max = fire_low_counter_max;
                _component.fire_low_counter = 0;
                _component.enemycontroller_Universaldisplays.fire_low_effectobj.SetActive(true);
                damagetype type = new damagetype();
                type.type = "fire";
                _component.DoDamage(type, fire_low_damage_1time);
                _component.fire_low_damage_lasting = fire_low_damage_lasting;
                _component.fire_low_applylastingdamagecounter = fire_low_applylastingdamage_counter_max;
                _component.fire_low_applylastingdamagecounter_max = fire_low_applylastingdamage_counter_max;

                //remind player of the appliance of this lasting effect
                _component.Utility_ShowVFX_hittext("burn!", vfxmanager.VM.firedamage, false);
            }
            else//immune so just apply a fire damage which will be resisted
            {
                damagetype type = new damagetype();
                type.type = "fire";
                _component.DoDamage(type, fire_low_damage_1time);
            }

        }
        if (poison_low)
        {
            if(!_component.immune_poison)
            {
                _component.poison_low = true;
                _component.poison_low_counter_max = poison_low_counter_max;
                _component.poison_low_counter = 0;
                _component.enemycontroller_Universaldisplays.poison_low_effectobj.SetActive(true);
                damagetype type = new damagetype();
                type.type = "poison";
                _component.DoDamage(type, poison_low_damage_1time);
                _component.poison_low_damage_lasting = poison_low_damage_lasting;
                _component.poison_low_applylastingdamagecounter = poison_low_applylastingdamage_counter_max;
                _component.poison_low_applylastingdamagecounter_max = poison_low_applylastingdamage_counter_max;
                //remind player of the appliance of this lasting effect
                _component.Utility_ShowVFX_hittext("poisoned!", vfxmanager.VM.poisondamage, false);
            }
            else
            {
                damagetype type = new damagetype();
                type.type = "poison";
                _component.DoDamage(type, poison_low_damage_1time);
            }

        }
        if (ice_low)
        {
            if (!_component.immune_ice)//"set on fire" effect
            {
                //modify speed
                NavMeshAgent _component_navagent = _component.transform.parent.GetComponent<NavMeshAgent>();
                if (_component.ice_low)
                {

                }
                else
                {
                    _component.ice_low_navagent_orgspeed = _component_navagent.speed;
                    _component_navagent.speed = _component.ice_low_navagent_orgspeed * 0.3f;

                    _component.animator.speed = 0.3f;
                }
                _component.ice_low = true;
                _component.ice_low_counter_max = ice_low_counter_max;
                _component.ice_low_counter = 0;
                _component.enemycontroller_Universaldisplays.ice_low_effectobj.SetActive(true);
                damagetype type = new damagetype();
                type.type = "ice";
                _component.DoDamage(type, ice_low_damage_1time);
                _component.ice_low_damage_lasting = ice_low_damage_lasting;
                _component.ice_low_applylastingdamagecounter = ice_low_applylastingdamage_counter_max;
                _component.ice_low_applylastingdamagecounter_max = ice_low_applylastingdamage_counter_max;

                //remind player of the appliance of this lasting effect
                _component.Utility_ShowVFX_hittext("slowed!", vfxmanager.VM.icedamage, false);

            }
            else//immune so just apply a fire damage which will be resisted
            {
                damagetype type = new damagetype();
                type.type = "ice";
                _component.DoDamage(type, ice_low_damage_1time);
            }

        }

    }
    //------On-enemy death -------//
    public void ModifyEnemyDeath(GameObject enemyobj)
    {
        var _component = enemyobj.GetComponent<enemycontroller>();
        if (visual_dieice||_component.frozen)
        {
            GameObject v=  vfxmanager.VM.GetPooledObject("vfx");
            v.transform.position = enemyobj.transform.position;
            v.transform.rotation = enemyobj.transform.rotation;
            v.transform.GetChild(visual_dieice_vfxcode).gameObject.SetActive(true);
            v.SetActive(true);
        }
    }
    //------Modify player stat on level begin-------//
    public void ModifyPlayerStatsWhenAdded()
    {
        levelmanager.LM.arrow_damage += damage_bonus;
        levelmanager.LM.fullhealth += fullhealth_bonus;
        levelmanager.LM.controller.DoHeal(fullhealth_bonus);

        levelmanager.LM. critical_chance += critical_chance_bonus;
        levelmanager.LM.critical_damage_multiplyby += critical_damage_multiplyby_bonus;

        levelmanager.LM.attack_speed += attack_speed_bonus;

        levelmanager.LM.resist_normaldamage += resist_normaldamage_bonus;
        levelmanager.LM.resist_fire += resist_fire_bonus;
        if(!levelmanager.LM.immune_fire)   levelmanager.LM.immune_fire = immune_fire;
        levelmanager.LM.resist_poison += resist_poison_bonus;
        if (!levelmanager.LM.immune_poison) levelmanager.LM.immune_poison = immune_poison;
        levelmanager.LM.resist_ice += resist_ice_bonus;
        if (!levelmanager.LM.immune_ice) levelmanager.LM.immune_ice = immune_ice;

        if (heal_health) levelmanager.LM.controller.DoHeal(levelmanager.LM.fullhealth * heal_health_rate);
    }
    //------Modify player stat on level begin(for equipments, because equipments have levels)-------//
    public void ModifyPlayerStats_WithEquipmentLevels(float valuemultiplier)
    {
        levelmanager.LM.arrow_damage += damage_bonus * valuemultiplier;
        levelmanager.LM.fullhealth += fullhealth_bonus * valuemultiplier;
        levelmanager.LM.controller.DoHeal(fullhealth_bonus * valuemultiplier);

        levelmanager.LM.critical_chance += critical_chance_bonus * valuemultiplier;
        levelmanager.LM.critical_damage_multiplyby += critical_damage_multiplyby_bonus * valuemultiplier;

        levelmanager.LM.attack_speed += attack_speed_bonus * valuemultiplier;

        levelmanager.LM.resist_normaldamage += resist_normaldamage_bonus * valuemultiplier;
        levelmanager.LM.resist_fire += resist_fire_bonus * valuemultiplier;
        if (!levelmanager.LM.immune_fire) levelmanager.LM.immune_fire = immune_fire;
        levelmanager.LM.resist_poison += resist_poison_bonus * valuemultiplier;
        if (!levelmanager.LM.immune_poison) levelmanager.LM.immune_poison = immune_poison;
        levelmanager.LM.resist_ice += resist_ice_bonus * valuemultiplier;
        if (!levelmanager.LM.immune_ice) levelmanager.LM.immune_ice = immune_ice;
    }

    //------Some items change player pose or animation when equipped(player displayed in gameplay)-------//
    public void ModifyPlayerAnimation_Gameplay()
    {
        if(define_projectile_skin)//this means the effect changes weapon
        {

            //if there already is a override controller, duplicate a animator override controller to avoid permanent change to static data
            //if there isn't, override on the original runtime animator controller
            AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(levelmanager.LM.controller.animator.runtimeAnimatorController);
            if(gamemanager.GM.heromanager.player_datas[gamemanager.GM.heromanager.player_code].override_animations)
            {
                foreach (AnimationClip c in gamemanager.GM.heromanager.player_datas[gamemanager.GM.heromanager.player_code].new_animatorcontroller.animationClips)
                {
                    animatorOverrideController[c.name] = c;
                }
                
            }
            levelmanager.LM.controller.animator.runtimeAnimatorController = animatorOverrideController;
            if(levelmanager.LM.controller.backup_attack_clips.Length!=0)
            animatorOverrideController["attack"] = levelmanager.LM.controller.backup_attack_clips[replace_attack_animation_with_controller_backupanimation_code];
            if (levelmanager.LM.controller.backup_idle_clips.Length != 0)
                animatorOverrideController["idle"] = levelmanager.LM.controller.backup_idle_clips[replace_idle_animation_with_controller_backupanimation_code];
            Debug.Log("finished animationclip swap");
        }
    }
    //------Some items change player pose or animation when equipped(player displayed in UI)-------//
    public void ModifyPlayerAnimation_Display(Animator player)
    {
        if (define_projectile_skin)//this means the effect changes weapon
        {

            //if there already is a override controller, duplicate a animator override controller to avoid permanent change to static data
            //if there isn't, override on the original runtime animator controller
            AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(player.runtimeAnimatorController);
            if (gamemanager.GM.heromanager.player_datas[gamemanager.GM.heromanager.player_code].override_animations)
            {
                foreach (AnimationClip c in gamemanager.GM.heromanager.player_datas[gamemanager.GM.heromanager.player_code].new_animatorcontroller.animationClips)
                {
                    animatorOverrideController[c.name] = c;
                }

            }
            player.runtimeAnimatorController = animatorOverrideController;
            if (gamemanager.GM.heromanager.player_datas[gamemanager.GM.heromanager.player_code].replace_backup_attack_clips.Length != 0)
                animatorOverrideController["attack"] = gamemanager.GM.heromanager.player_datas[gamemanager.GM.heromanager.player_code].replace_backup_attack_clips[replace_attack_animation_with_controller_backupanimation_code];
            if (gamemanager.GM.heromanager.player_datas[gamemanager.GM.heromanager.player_code].replace_backup_idle_clips.Length != 0)
                animatorOverrideController["idle"] = gamemanager.GM.heromanager.player_datas[gamemanager.GM.heromanager.player_code].replace_backup_idle_clips[replace_idle_animation_with_controller_backupanimation_code];
            Debug.Log("finished animationclip swap");
        }
    }
    //-------some items modify coin drop rate-------///
    public float ModifyEarningMultiplier_Coin(float original_multiplier)
    {
        float f = original_multiplier;
        f += earning_coin_multiplier_bonus;
        return f;
    }
    //-------some items modify item drop rate-------///
    public float ModifyEarningMultiplier_Equipment(float original_multiplier)
    {
        float f = original_multiplier;
        f += earning_equip_multiplier_bonus;
        return f;
    }
}
