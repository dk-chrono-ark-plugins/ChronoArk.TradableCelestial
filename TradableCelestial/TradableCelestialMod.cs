using ChronoArkMod.Plugin;
using System.Collections.Generic;
using TradableCelestial.Api;
using TradableCelestialMod.Patches;
using UnityEngine;

namespace TradableCelestial;

#nullable enable

public class TradableCelestialMod : ChronoArkPlugin
{
    private static TradableCelestialMod? _instance;
    private readonly List<IPatch> _patches = [];

    public static TradableCelestialMod? Instance => _instance;

    public override void Dispose()
    {
        _instance = null;
    }

    public override void Initialize()
    {
        _instance = this;

        var guid = GetGuid();

        _patches.Add(new CelestialPatch(guid));

        foreach (var patch in _patches) {
            if (patch.Mandatory) {
                Debug.Log($"patching {patch.Id}");
                patch.Commit();
                Debug.Log("success!");
            }
        }
    }
}
