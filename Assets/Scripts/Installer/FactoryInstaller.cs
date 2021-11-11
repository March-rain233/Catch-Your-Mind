using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 工厂管理类装配器
/// </summary>
public class FactoryInstaller : IFactoryInstaller
{
    public void Install(FactoryManager manager)
    {
        manager.AddFactory(ObjectType.Sprite, new ObjectFactoryWithPool((string name) =>
        {
            var r = manager.LoadResource<Sprite>(ObjectType.Sprite, name);
            var res = new PSprite();
            res.Sprite = r;
            return res;
        }, 20));
        manager.AddFactory(ObjectType.AudioClip, new ObjectFactoryWithPool((string name) =>
        {
            var r = manager.LoadResource<AudioClip>(ObjectType.AudioClip, name);
            var res = new PAudioClip();
            res.AudioClip = r;
            return res;
        }, 20));
        manager.AddFactory(ObjectType.Breed, new ObjectFactoryWithPool((string name) =>
        {
            return manager.LoadResource<NPC.Breed>(ObjectType.Breed, name);
        }, 20));
        manager.AddFactory(ObjectType.AI_Controller, new ObjectFactoryWithPool((string name) =>
        {
            return manager.LoadResource<NPC.StateMachineController>(ObjectType.AI_Controller, name);
        }, 20));
        manager.AddFactory(ObjectType.Item, new ObjectFactoryWithPool((string name) =>
        {
            return manager.LoadResource<Item.BaseItem>(ObjectType.Item, name);
        }, 50));
        manager.AddFactory(ObjectType.NPC, new ObjectFactoryWithPool((string name) => 
        {
            Save.NPC_Info info;
            if (name == "Shury")
            {
                info = GameManager.Instance.GameSave.Player["Shury"];
            }
            else 
            {
                info = manager.LoadResource<Save.NPC_Info>(ObjectType.NPC, name); 
            }
            var prefabs = manager.LoadResource<GameObject>(ObjectType.NPC_Prefabs, info.Prefabs);
            var breed = manager.Create(ObjectType.Breed, info.Breed);
            var controller = manager.Create(ObjectType.AI_Controller, info.Behavior);

            NPC.NPC_Equip equip = new NPC.NPC_Equip();
            if (!string.IsNullOrEmpty(info.Equip.LeftHand))
                equip.LeftHand = manager.Create(ObjectType.Item, info.Equip.LeftHand) as Item.Weapon;
            if (!string.IsNullOrEmpty(info.Equip.RightHand))
                equip.RightHand = manager.Create(ObjectType.Item, info.Equip.RightHand) as Item.Weapon;
            if (!string.IsNullOrEmpty(info.Equip.Head))
                equip.Head = manager.Create(ObjectType.Item, info.Equip.Head) as Item.Equipment;
            if (!string.IsNullOrEmpty(info.Equip.Body))
                equip.Body = manager.Create(ObjectType.Item, info.Equip.Body) as Item.Equipment;
            if (!string.IsNullOrEmpty(info.Equip.Pants))
                equip.Pants = manager.Create(ObjectType.Item, info.Equip.Pants) as Item.Equipment;
            if (!string.IsNullOrEmpty(info.Equip.Feet))
                equip.Feet = manager.Create(ObjectType.Item, info.Equip.Feet) as Item.Equipment;

            var model = prefabs.GetComponent<NPC.NPC_Model>();
            model.Init(breed as NPC.Breed, equip);
            var ai = prefabs.GetComponent<NPC.BehaviorStateMachine>();
            ai.Init(controller as NPC.StateMachineController);
            NPC.PNPC p = new NPC.PNPC();
            p.Model = model;
            p.StateMachine = ai;
            return p;
        }, 10));
    }
}
