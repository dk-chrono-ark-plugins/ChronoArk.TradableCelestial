using DarkTonic.MasterAudio;
using HarmonyLib;
using I2.Loc;
using System.Collections.Generic;
using TradableCelestial.Api;
using UnityEngine;
using UseItem;

namespace TradableCelestialMod.Patches;

#nullable enable

internal class CelestialPatch(string guid) : IPatch
{
    private Harmony? _harmony;

    public string Id => "celestial-use";
    public string Name => Id;
    public string Description => Id;
    public bool Mandatory => true;

    public void Commit()
    {
        _harmony ??= new(guid);
        _harmony.Patch(
            original: AccessTools.Method(
                typeof(ItemObject),
                nameof(ItemObject.Use)
            ),
            prefix: new(typeof(CelestialPatch), nameof(OnUse))
        );
    }

    private static bool OnUse(ItemObject __instance)
    {
        if (__instance.Item is not Item_Consume consumable ||
            consumable.itemkey != nameof(Celestial) ||
            PlayData.SpalcialRule != nameof(SR_Solo)) {
            // art we the solitary wolf, yet not forlorn?
            return true;
        }

        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftControl)) {
            __instance.Item.ShiftClick();
            return true;
        }
        if (!Input.GetMouseButtonDown(1)) {
            return false;
        }

        // prepare the tough choices
        var price = __instance.Item.Price();
        List<ButtonData> buttons = [
            new ButtonData {
                call = delegate {
                    PlayData.Gold += price;
                    MasterAudio.PlaySound("SE_ItemAllGet");
                    __instance.MyManager.UseItem(__instance.MyManager, __instance.MySlot.Number);
                },
                Text = $"{price} {LocalizationManager.GetTranslation(ScriptTerms.System.Gold)}"
            },
            new ButtonData {
                call = delegate {
                    if (__instance.Item.Use()) {
                        __instance.MyManager.UseItem(__instance.MyManager, __instance.MySlot.Number);
                    } else {
                        FieldSystem.instance.UseingItemObject = __instance.Item;
                    }
                },
                Text = ScriptLocalization.System_Item.UseItem
            },
            new ButtonData {
                Text = ScriptLocalization.UI.Cancel
            },
        ];

        var worldCamera = GameObject.FindGameObjectWithTag("MainUICanvas").GetComponent<Canvas>().worldCamera;
        var widget = UIManager.InstantiateActive(UIManager.inst.SelectButtons);
        widget.transform.position = worldCamera.ScreenToWorldPoint(Input.mousePosition with { z = 0f });
        widget.transform.localPosition = widget.transform.localPosition with { z = 0f };
        widget.GetComponent<SelectButtons>().Init(buttons);

        // cast into voidness!
        return false;
    }
}
