using FFT_Egg_Control.Configuration;
using FFT_Egg_Control.Template;
using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Memory;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static FFT_Egg_Control.Constants.Constants;
using static System.Net.Mime.MediaTypeNames;

namespace FFT_Egg_Control
{
    public class Mod : ModBase
    {
        private readonly IModLoader _modLoader;

        private readonly Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks? _hooks;

        private readonly ILogger _logger;

        private readonly IMod _owner;

        private Config _configuration;
        
        private IImGui _imGui;
        private IImGuiShell _imGuiShell;

        private SettingsMenu settingsMenu;
        private SpoilsOverlay _spoilsOverlay;

        private readonly IModConfig _modConfig;

        private Dictionary<MonsterID, Func<bool>> MonsterIDToConfigMap;

        [Function([FunctionAttribute.Register.rcx, FunctionAttribute.Register.rdx, FunctionAttribute.Register.r14], FunctionAttribute.Register.rax, false)]
        private delegate Int64 SpawnCreatureEgg(SpawnType spawnType, int eggID, MonsterID monsterID);
        private IHook<SpawnCreatureEgg> SpawnCreatureEgg_Hook;

        [Function(CallingConventions.Microsoft)]
        private delegate Int64 ShowVictoryScreen();
        private IHook<ShowVictoryScreen> ShowVictoryScreen_Hook;
        private ShowVictoryScreen ShowVictoryScreen_Func;

        [Function(CallingConventions.Microsoft)]
        private delegate UInt64 ShowWarTrophies(Int64 a1, Int32 a2);
        private IHook<ShowWarTrophies> ShowWarTrophies_Hook;

        [Function(CallingConventions.Microsoft)]
        private delegate void ProcessSpoils();
        private IHook<ProcessSpoils> ProcessSpoils_Hook;

        [Function(CallingConventions.Microsoft)]
        private delegate void WaitForButtonPress(int a1);
        private IHook<WaitForButtonPress> WaitForButtonPress_Hook;

        [Function(CallingConventions.Microsoft)]
        private delegate UInt64 GetTable(UInt64 nexModule, int tableID);
        private GetTable GetTable_Func;

        [Function(CallingConventions.Microsoft)]
        private delegate UInt64 NexSearchRow1K(UInt64 table, int key);
        private NexSearchRow1K NexSearchRow1K_Func;

        [Function(CallingConventions.Microsoft)]
        private delegate UInt64 NexGetRowData(UInt64 row);
        private NexGetRowData NexGetRowData_Func;

        [Function(CallingConventions.Microsoft)]
        private delegate Int64 IncrementWarFunds(int funds);
        private IncrementWarFunds IncrementWarFunds_Func;
        
        private nuint NexModulePointerAddress;
        private nuint CarcassCountsAddress;
        private nuint UnitDataAddress;

        //IHook<GetTable>? getTableFunc;

        //private ulong func1(ulong a1, int a2)
        //{
        //    var ret = getTableFunc.OriginalFunction(a1, a2);
        //    _logger.WriteLine($"Getting Table. pNexModule:0x{a1:X}, TableID:0x{a2:X}, Ret:0x{ret:X}");
        //    return ret;
        //}

        private Int64 SpawnCreatureEgg_Replacement(SpawnType spawnType, int eggID, MonsterID monsterID)
        {
            var blockSpawn = false;

            if (spawnType == SpawnType.Generic_Monster && _configuration.Egg_Culling_Enabled && MonsterIDToConfigMap.TryGetValue(monsterID, out var check) && !check())
            {
                if (_configuration.Egg_Culling_Enabled)
                {
                    blockSpawn = true;
                }
            }

            if (blockSpawn)
            {
                if (_configuration.Logging_Enabled)
                {
                    Log($"Discarding {monsterID} egg.", Color.Orange);
                }

                return -1;
            }

            if (_configuration.Logging_Enabled)
            {
                Log($"Allowing {monsterID} egg.", Color.SkyBlue);
            }

            return SpawnCreatureEgg_Hook.OriginalFunction(spawnType, eggID, monsterID);
        }

        private Int64 ShowVictoryScreen_Replacement()
        {
            if (_configuration.Logging_Enabled)
            {
                Log($"Showing victory screen...", Color.Gold);
            }

            _spoilsOverlay.WindowOpen = true;

            var ret = ShowVictoryScreen_Hook.OriginalFunction();

            if (_configuration.Logging_Enabled)
            {
                Log($"Closing victory screen...", Color.Gold);
            }

            _spoilsOverlay.WindowOpen = false;

            return ret;
        }

        private UInt64 ShowWarTrophies_Replacement(Int64 a1, Int32 a2)
        {
            if (_configuration.Logging_Enabled)
            {
                Log($"Show War Trophies: 0x{a1:X}, 0x{a2:X}", Color.Gold);
            }

            //_spoilsOverlay.WindowOpen = true;

            if (a2 == 0)
            {
                _spoilsOverlay.WindowOpen = true;
            }
            else
            {
                _spoilsOverlay.WindowOpen = false;
            }

                var ret = ShowWarTrophies_Hook.OriginalFunction(a1, a2);

            if (_configuration.Logging_Enabled)
            {
                Log($"Finishing ShowWarTrophies...", Color.Gold);
            }

            //_spoilsOverlay.WindowOpen = false;

            return ret;
        }

        private void ProcessSpoils_Replacement()
        {
            if (_configuration.Logging_Enabled)
            {
                Log($"ProcessSpoils:", Color.Gold);
            }

            //_spoilsOverlay.WindowOpen = true;

            ProcessSpoils_Hook.OriginalFunction();

            if (_configuration.Logging_Enabled)
            {
                Log($"Finishing ProcessSpoils...", Color.Gold);
            }

            if (_configuration.AutoPoach_Enabled)
            {
                PoachFromUnits();
            }

            //_spoilsOverlay.WindowOpen = false;
        }

        private void WaitForButtonPress_Replacement(int a1)
        {
            //0x1402C0D04
            if (_configuration.Logging_Enabled)
            {
                Log($"WaitForButtonPress:", Color.Gold);
            }

            //_spoilsOverlay.WindowOpen = true;

            WaitForButtonPress_Hook.OriginalFunction(a1);

            if (_configuration.Logging_Enabled)
            {
                Log($"Finishing WaitForButtonPress...", Color.Gold);
            }

            //_spoilsOverlay.WindowOpen = false;
        }


        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _configuration = context.Configuration;
            _modConfig = context.ModConfig;

#if DEBUG
            Debugger.Launch();
#endif
            var startupScannerController = _modLoader.GetController<IStartupScanner>();
            if (startupScannerController == null || !startupScannerController.TryGetTarget(out var startupScanner))
            {
                _logger.WriteLineAsync($"[{_modConfig.ModId}] Unable to find startupScanner. Ensure Reloaded.Memory.SigScan is installed.", Color.OrangeRed);

                return;
            }
            startupScanner.AddMainModuleScan("40 53 48 83 EC ?? 41 89 C8 E8 ?? ?? ?? ?? 48 63 D8 85 C0 78", result => // 0x14DE9E013 in 1.2.0, 0x14DAD771C in 1.3.0
            {
                if (!result.Found)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to find AoB pattern for SpawnCreatureEgg function!", Color.OrangeRed);
                    return;
                }

                MonsterIDToConfigMap = new()
                {
                    [MonsterID.Chocobo] = () => { return _configuration.Allow_Chocobos_Eggs; },
                    [MonsterID.Black_Chocobo] = () => { return _configuration.Allow_Black_Chocobos_Eggs; },
                    [MonsterID.Red_Chocobo] = () => { return _configuration.Allow_Red_Chocobos_Eggs; },
                    [MonsterID.Goblin] = () => { return _configuration.Allow_Goblins_Eggs; },
                    [MonsterID.Black_Goblin] = () => { return _configuration.Allow_Black_Goblins_Eggs; },
                    [MonsterID.Gobbledygook] = () => { return _configuration.Allow_Gobbledygooks_Eggs; },
                    [MonsterID.Bomb] = () => { return _configuration.Allow_Bombs_Eggs; },
                    [MonsterID.Grenade] = () => { return _configuration.Allow_Grenades_Eggs; },
                    [MonsterID.Exploder] = () => { return _configuration.Allow_Exploders_Eggs; },
                    [MonsterID.Red_Panther] = () => { return _configuration.Allow_Red_Panthers_Eggs; },
                    [MonsterID.Coeurl] = () => { return _configuration.Allow_Coeurls_Eggs; },
                    [MonsterID.Vampire_Cat] = () => { return _configuration.Allow_Vampire_Cats_Eggs; },
                    [MonsterID.Piscodaemon] = () => { return _configuration.Allow_Piscodaemons_Eggs; },
                    [MonsterID.Squidraken] = () => { return _configuration.Allow_Squidrakens_Eggs; },
                    [MonsterID.Mindflayer] = () => { return _configuration.Allow_Mindflayers_Eggs; },
                    [MonsterID.Skeleton] = () => { return _configuration.Allow_Skeletons_Eggs; },
                    [MonsterID.Bonesnatch] = () => { return _configuration.Allow_Bonesnatchs_Eggs; },
                    [MonsterID.Skeletal_Fiend] = () => { return _configuration.Allow_Skeletal_Fiends_Eggs; },
                    [MonsterID.Ghoul] = () => { return _configuration.Allow_Ghouls_Eggs; },
                    [MonsterID.Ghast] = () => { return _configuration.Allow_Ghasts_Eggs; },
                    [MonsterID.Revenant] = () => { return _configuration.Allow_Revenants_Eggs; },
                    [MonsterID.Floating_Eye] = () => { return _configuration.Allow_Floating_Eyes_Eggs; },
                    [MonsterID.Ahriman] = () => { return _configuration.Allow_Ahrimans_Eggs; },
                    [MonsterID.Plague_Horror] = () => { return _configuration.Allow_Plague_Horrors_Eggs; },
                    [MonsterID.Jura_Aevis] = () => { return _configuration.Allow_Jura_Aeviss_Eggs; },
                    [MonsterID.Steelhawk] = () => { return _configuration.Allow_Steelhawks_Eggs; },
                    [MonsterID.Cockatrice] = () => { return _configuration.Allow_Cockatrices_Eggs; },
                    [MonsterID.Pig] = () => { return _configuration.Allow_Pigs_Eggs; },
                    [MonsterID.Swine] = () => { return _configuration.Allow_Swines_Eggs; },
                    [MonsterID.Wild_Boar] = () => { return _configuration.Allow_Wild_Boars_Eggs; },
                    [MonsterID.Dryad] = () => { return _configuration.Allow_Dryads_Eggs; },
                    [MonsterID.Treant] = () => { return _configuration.Allow_Treants_Eggs; },
                    [MonsterID.Elder_Treant] = () => { return _configuration.Allow_Elder_Treants_Eggs; },
                    [MonsterID.Wisenkin] = () => { return _configuration.Allow_Wisenkins_Eggs; },
                    [MonsterID.Minotaur] = () => { return _configuration.Allow_Minotaurs_Eggs; },
                    [MonsterID.Sekhret] = () => { return _configuration.Allow_Sekhrets_Eggs; },
                    [MonsterID.Malboro] = () => { return _configuration.Allow_Malboros_Eggs; },
                    [MonsterID.Ochu] = () => { return _configuration.Allow_Ochus_Eggs; },
                    [MonsterID.Greater_Malboro] = () => { return _configuration.Allow_Greater_Malboros_Eggs; },
                    [MonsterID.Behemoth] = () => { return _configuration.Allow_Behemoths_Eggs; },
                    [MonsterID.Behemoth_King] = () => { return _configuration.Allow_Behemoth_Kings_Eggs; },
                    [MonsterID.Dark_Behemoth] = () => { return _configuration.Allow_Dark_Behemoths_Eggs; },
                    [MonsterID.Dragon] = () => { return _configuration.Allow_Dragons_Eggs; },
                    [MonsterID.Blue_Dragon] = () => { return _configuration.Allow_Blue_Dragons_Eggs; },
                    [MonsterID.Red_Dragon] = () => { return _configuration.Allow_Red_Dragons_Eggs; },
                    [MonsterID.Hydra] = () => { return _configuration.Allow_Hydras_Eggs; },
                    [MonsterID.Greater_Hydra] = () => { return _configuration.Allow_Greater_Hydras_Eggs; },
                    [MonsterID.Tiamat] = () => { return _configuration.Allow_Tiamats_Eggs; },
                };

                var spawnCreatureEgg_address = Process.GetCurrentProcess().MainModule.BaseAddress + result.Offset;

                SpawnCreatureEgg_Hook = _hooks!.CreateHook<SpawnCreatureEgg>(SpawnCreatureEgg_Replacement, spawnCreatureEgg_address).Activate();

                if (!SpawnCreatureEgg_Hook.IsHookEnabled)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to hook SpawnCreatureEgg function at 0x{spawnCreatureEgg_address:X}.", Color.OrangeRed);
                    return;
                }

                _logger.WriteLineAsync($"[{_modConfig.ModId}] Hooked SpawnCreatureEgg function at 0x{spawnCreatureEgg_address:X}.", Color.LightGreen);
            });

            //var getTableFunc = _hooks.CreateWrapper<GetTable>(0x1403CE588, out nint getTableWrapperAddress);

            var imGuiController = _modLoader.GetController<IImGui>();

            if (imGuiController?.TryGetTarget(out IImGui imGui) != true)
            {
                _logger.WriteLineAsync($"[{_modConfig.ModId}] ImGui not found.");
                return;
            }

            _imGui = imGui;

            var imGuiShellController = _modLoader.GetController<IImGuiShell>();

            if (imGuiShellController?.TryGetTarget(out IImGuiShell imGuiShell) != true)
            {
                _logger.WriteLineAsync($"[{_modConfig.ModId}] ImGuiShell not found.");
                return;
            }

            _imGuiShell = imGuiShell;

            _logger.WriteLineAsync($"[{_modConfig.ModId}] {imGui}.");
            _logger.WriteLineAsync($"[{_modConfig.ModId}] {imGuiShell}.");

            settingsMenu = new SettingsMenu(_configuration);
            settingsMenu.imGui = imGui;
            settingsMenu.PoachAction = () => PoachThing(0x5E);
            settingsMenu.MonsterIDToConfigMap = MonsterIDToConfigMap;
            settingsMenu.GetUnitCountAction = GetUnitCount;
            settingsMenu.GetUnitsAction = GetUnits;
            settingsMenu.CarcassCountAction = () => {
                Log($"Carcasses: 0x{CarcassCountsAddress:X}", Color.LightGreen);

                for (nuint i = 1; i <= 96; i++) 
                {
                    var poachItem = GetPoachItemData((int)i);
                    var poachItemNamePointer = (poachItem + 0x8) + Memory.Instance.Read<UInt32>((nuint)(poachItem + 0x8));
                    //var jobName = Memory.Instance.ReadWithMarshalling<string>((nuint)jobNamePointer));
                    var poachItemName = "blank";
                    poachItemName = Marshal.PtrToStringAnsi((nint)poachItemNamePointer);
                    var itemCount = Memory.Instance.Read<byte>((nuint)(CarcassCountsAddress + i));
                    if (itemCount > 0)
                    {
                        Log($"Carcass {poachItemName.Replace("<Icon=103>", "+")}: {itemCount}.", Color.LightGreen);
                    }
                }
            };
            imGuiShell.AddComponent(settingsMenu);

            _spoilsOverlay = new SpoilsOverlay(_configuration);
            _spoilsOverlay.imGui = imGui;
            _spoilsOverlay.PoachAction = () => PoachThing(0x5E); ;
            _spoilsOverlay.MonsterIDToConfigMap = MonsterIDToConfigMap;
            _spoilsOverlay.CarcassCountAction = () => { };
            imGuiShell.AddComponent(_spoilsOverlay);

            //startupScanner.AddMainModuleScan("48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 41 56 41 57 48 83 EC ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 44 24 ?? BE ?? ?? ?? ?? 8B CE E8 ?? ?? ?? ?? 41 BF ?? ?? ?? ?? 8D 7E ?? 8D 4E", result =>
            //{
            //    if (!result.Found)
            //    {
            //        Log($"Failed to find AoB pattern for ShowVictoryScreen function!", Color.OrangeRed);
            //        return;
            //    }

            //    var showVictoryScreen_address = Process.GetCurrentProcess().MainModule.BaseAddress + result.Offset;

            //    ShowVictoryScreen_Hook = _hooks!.CreateHook<ShowVictoryScreen>(ShowVictoryScreen_Replacement, showVictoryScreen_address).Activate();

            //    if (!ShowVictoryScreen_Hook.IsHookEnabled)
            //    {
            //        Log($"Failed to hook ShowVictoryScreen function at 0x{showVictoryScreen_address:X}.", Color.OrangeRed);
            //        return;
            //    }

            //    Log($"Hooked ShowVictoryScreen function at 0x{showVictoryScreen_address:X}.", Color.LightGreen);
            //});

            startupScanner.AddMainModuleScan("48 8B C4 48 89 58 ?? 48 89 68 ?? 48 89 70 ?? 48 89 78 ?? 41 57 48 83 EC ?? 83 25 ?? ?? ?? ?? ?? 48 8D 3D ?? ?? ?? ?? 48 8B CF E8", result =>
            {
                if (!result.Found)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to find AoB pattern for ProcessSpoils function!", Color.OrangeRed);
                    return;
                }

                var processSpoils_address = Process.GetCurrentProcess().MainModule.BaseAddress + result.Offset;

                ProcessSpoils_Hook = _hooks!.CreateHook<ProcessSpoils>(ProcessSpoils_Replacement, processSpoils_address).Activate();

                if (!ProcessSpoils_Hook.IsHookEnabled)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to hook ProcessSpoils function at 0x{processSpoils_address:X}.", Color.OrangeRed);
                    return;
                }

                _logger.WriteLineAsync($"[{_modConfig.ModId}] Hooked ProcessSpoils function at 0x{processSpoils_address:X}.", Color.LightGreen);
            });

            startupScanner.AddMainModuleScan("85 C9 7E ?? 48 89 5C 24 ?? 57 48 83 EC ?? 8B F9 33 DB", result =>
            {
                if (!result.Found)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to find AoB pattern for WaitForButtonPress function!", Color.OrangeRed);
                    return;
                }

                var WaitForButtonPress_address = Process.GetCurrentProcess().MainModule.BaseAddress + result.Offset;

                WaitForButtonPress_Hook = _hooks!.CreateHook<WaitForButtonPress>(WaitForButtonPress_Replacement, WaitForButtonPress_address).Activate();

                if (!WaitForButtonPress_Hook.IsHookEnabled)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to hook WaitForButtonPress function at 0x{WaitForButtonPress_address:X}.", Color.OrangeRed);
                    return;
                }

                _logger.WriteLineAsync($"[{_modConfig.ModId}] Hooked ProcessSpoils function at 0x{WaitForButtonPress_address:X}.", Color.LightGreen);
            });

            startupScanner.AddMainModuleScan("40 53 48 83 EC ?? 48 8B 05 ?? ?? ?? ?? 8B DA 48 8B 48 ?? 48 8B 89 ?? ?? ?? ?? 48 8B 01 FF 50 ?? 84 C0 74", result =>
            {
                if (!result.Found)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to find AoB pattern for ShowWarTrophies function!", Color.OrangeRed);
                    return;
                }

                var showWarTrophies_address = Process.GetCurrentProcess().MainModule.BaseAddress + result.Offset;

                ShowWarTrophies_Hook = _hooks!.CreateHook<ShowWarTrophies>(ShowWarTrophies_Replacement, showWarTrophies_address).Activate();

                if (!ShowWarTrophies_Hook.IsHookEnabled)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to hook ShowWarTrophies function at 0x{showWarTrophies_address:X}.", Color.OrangeRed);
                    return;
                }

                _logger.WriteLineAsync($"[{_modConfig.ModId}] Hooked ShowVictoryScreen function at 0x{showWarTrophies_address:X}.", Color.LightGreen);
            });

            startupScanner.AddMainModuleScan("45 33 C0 89 54 24 ?? 45 8B D0 4C 8B D9 49 B9 ?? ?? ?? ?? ?? ?? ?? ?? 42 0F B6 44 14 ?? 48 B9 ?? ?? ?? ?? ?? ?? ?? ?? 4C 33 C8 49 FF C2 4C 0F AF C9 49 83 FA ?? 72 ?? 49 8B 4B ?? 49 23 C9 4D 8B 4B", result =>
            {
                if (!result.Found)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to find AoB pattern for GetTable function!", Color.OrangeRed);
                    return;
                }

                GetTable_Func = _hooks.CreateWrapper<GetTable>(Process.GetCurrentProcess().MainModule.BaseAddress + result.Offset, out var _);
            });

            startupScanner.AddMainModuleScan("48 8B 41 ?? 48 85 C0 74 ?? 48 83 E8 ?? 74 ?? 48 83 F8 ?? 74 ?? 45 33 C9 45 33 C0 E9 ?? ?? ?? ?? 45 33 C0 E9 ?? ?? ?? ?? 80 79 ?? ?? 74 ?? 3B 51", result =>
            {
                if (!result.Found)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to find AoB pattern for NexSearchRow1K function!", Color.OrangeRed);
                    return;
                }

                NexSearchRow1K_Func = _hooks.CreateWrapper<NexSearchRow1K>(Process.GetCurrentProcess().MainModule.BaseAddress + result.Offset, out var _);
            });

            startupScanner.AddMainModuleScan("48 8B 01 48 BA ?? ?? ?? ?? ?? ?? ?? ?? 48 23 C2 74 ?? 48 8B 49 ?? 48 83 E8 ?? 74 ?? 48 83 F8 ?? 74 ?? 48 63 41 ?? EB ?? 48 63 41 ?? EB ?? 48 63 41 ?? 48 03 C1 C3", result =>
            {
                if (!result.Found)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to find AoB pattern for NexGetRowData function!", Color.OrangeRed);
                    return;
                }

                NexGetRowData_Func = _hooks.CreateWrapper<NexGetRowData>(Process.GetCurrentProcess().MainModule.BaseAddress + result.Offset, out var _);
            });

            startupScanner.AddMainModuleScan("40 53 48 83 EC ?? 8B D9 B9 ?? ?? ?? ?? E8", result =>
            {
                if (!result.Found)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to find AoB pattern for IncrementWarFunds function!", Color.OrangeRed);
                    return;
                }

                IncrementWarFunds_Func = _hooks.CreateWrapper<IncrementWarFunds>(Process.GetCurrentProcess().MainModule.BaseAddress + result.Offset, out var _);
            });

            startupScanner.AddMainModuleScan("48 8B 1D ?? ?? ?? ?? 48 8D 15 ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 0F 57 C0 0F 11 44 24", result =>
            {
                if (!result.Found)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to find AoB pattern for pNexModule!", Color.OrangeRed);
                    return;
                }

                var nexModuleAoBBase = (UInt64)(Process.GetCurrentProcess().MainModule.BaseAddress + result.Offset);
                var nexModuleOffset = Memory.Instance.Read<UInt32>((nuint)nexModuleAoBBase + 3);
                NexModulePointerAddress = (nuint)(nexModuleAoBBase + 7 + nexModuleOffset);

                _logger.WriteLine($"NexModule: AoB:0x{nexModuleAoBBase:X}, offset:0x{nexModuleOffset:X}, pointer:0x{NexModulePointerAddress:X}");
            });

            startupScanner.AddMainModuleScan("39 D7 0F 87 ?? ?? ?? ?? 48 63 C7 48 8D 15 ?? ?? ?? ?? 80 3C 10 ?? 0F 82 ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 85 C0 75 ?? 48 8B 0D ?? ?? ?? ?? 48 85 C9 74", result =>
            {
                if (!result.Found)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to find AoB pattern for CarcassCounts!", Color.OrangeRed);
                    return;
                }

                _logger.WriteLine($"CarcassCount time!", Color.CornflowerBlue);

                var carcassCountsAoBBase = (UInt64)(Process.GetCurrentProcess().MainModule.BaseAddress + result.Offset);
                LogHex((nuint)carcassCountsAoBBase);
                var carcassCountsOffset = Memory.Instance.Read<Int32>((nuint)carcassCountsAoBBase + 0xE);
                _logger.WriteLine($"offset: {carcassCountsOffset}: 0x{carcassCountsOffset:X}");
                CarcassCountsAddress = (nuint)((Int64)carcassCountsAoBBase + 0x12 + carcassCountsOffset);
                LogHex(CarcassCountsAddress);

                _logger.WriteLine($"Carcass Counts: AoB:0x{carcassCountsAoBBase:X}, offset:0x{carcassCountsOffset:X}, pointer:0x{CarcassCountsAddress:X}");
            });

            startupScanner.AddMainModuleScan("48 8D 05 ?? ?? ?? ?? 48 03 C8 74 ?? 8B 43 ?? F2 0F 10 43 ?? F2 0F 11 41 ?? 89 41 ?? 0F B7 43 ?? 66 89 41", result =>
            {
                if (!result.Found)
                {
                    _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to find AoB pattern for UnitDataAddress!", Color.OrangeRed);
                    return;
                }

                _logger.WriteLine($"UnitDataAddress time!", Color.CornflowerBlue);

                var UnitDataAddressAoBBase = (UInt64)(Process.GetCurrentProcess().MainModule.BaseAddress + result.Offset);
                LogHex((nuint)UnitDataAddressAoBBase);
                var UnitDataAddressOffset = Memory.Instance.Read<Int32>((nuint)UnitDataAddressAoBBase + 0x3);
                _logger.WriteLine($"offset: {UnitDataAddressOffset}: 0x{UnitDataAddressOffset:X}");
                UnitDataAddress = (nuint)((Int64)UnitDataAddressAoBBase + 0x7 + UnitDataAddressOffset);
                LogHex(UnitDataAddress);

                _logger.WriteLine($"UnitDataAddress: AoB:0x{UnitDataAddressAoBBase:X}, offset:0x{UnitDataAddressOffset:X}, pointer:0x{UnitDataAddress:X}");
            });

            //var poachItemTable = getTableFunc(0x143CD36F0, 0xEC);
            //var itemTable = getTableFunc(0x143CD36F0, 0x2D);

            //Log($"JobTable: {jobTable:X}.", Color.LightGreen);
            //Log($"PoachItemTable: {poachItemTable:X}.", Color.LightGreen);
            //Log($"ItemTable: {itemTable:X}.", Color.LightGreen);
        }

        void LogHex(nuint value, [CallerArgumentExpression("value")] string valueName = null)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] {valueName}: 0x{value:X}");
        }
        void LogHex(nint value, [CallerArgumentExpression("value")] string valueName = null)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] {valueName}: 0x{value:X}");
        }

        private Dictionary<int, int> GetUnitCount()
        {
            var unitCount = new Dictionary<int, int>();

            for (int i = 0; i <= 54; i++)
            {
                var unitDataBase = UnitDataAddress + (nuint)(i * 0x258);
                var index = Memory.Instance.Read<byte>(unitDataBase + 1);
                var job = Memory.Instance.Read<byte>(unitDataBase + 2);

                if (index == 0xFF)
                {
                    continue;
                }

                if (unitCount.ContainsKey(job))
                {
                    unitCount[job]++;
                }
                else
                {
                    unitCount[job] = 1;
                }
            }

            return unitCount;
        }

        private Dictionary<int, (int, int, int, int)> GetUnits()
        {
            var units = new Dictionary<int, (int, int, int, int)>();

            for (int i = 0; i <= 54; i++)
            {
                var unitDataBase = UnitDataAddress + (nuint)(i * 0x258);
                var spriteSet = Memory.Instance.Read<byte>(unitDataBase + 0);
                var unitIndex = Memory.Instance.Read<byte>(unitDataBase + 1);
                var job = Memory.Instance.Read<byte>(unitDataBase + 2);
                var index2 = Memory.Instance.Read<byte>(unitDataBase + 0x2C);

                units.Add(i, (spriteSet, unitIndex, job, index2));
            }

            return units;
        }

        void PoachFromUnits()
        {
            var jobTable = GetTablePointer(0x2E);
            var eggTable = GetTablePointer(0x136);

            _spoilsOverlay.LastCarcassList.Clear();
            _spoilsOverlay.LastPremiumCarcassList.Clear();
            _spoilsOverlay.LastSoldCarcassList.Clear();
            _spoilsOverlay.LastOverstockCarcassGil = 0;

            for (int i = 0; i <= 54; i++)
            {
                var unitDataBase = UnitDataAddress + (nuint)(i * 0x258);
                var job = Memory.Instance.Read<byte>(unitDataBase + 2);

                var jobRowData = GetRowPointer(jobTable, job);

                var jobCommonPoach = Memory.Instance.Read<int>((nuint)(jobRowData + 0x24));
                var jobRarePoach = Memory.Instance.Read<int>((nuint)(jobRowData + 0x28));
                var jobEggId = Memory.Instance.Read<int>((nuint)(jobRowData + 0x34));

                var jobNamePointer = (jobRowData + 0x8) + Memory.Instance.Read<UInt32>((nuint)(jobRowData + 0x8));
                var jobName = "blank";

                jobName = Marshal.PtrToStringAnsi((nint)jobNamePointer);

                if (job >= 94 && job <= 141)
                {
                    //Log($"Monster {i}: Job: {jobName}(0x{job:X})");
                    var rng = Random.Shared.Next(256);
                    var eggRarity = 0;
                    var generatedMonsterID = -1;

                    if (rng < _configuration.Rare_Egg_AutoPoach_Rate)        // Default: 6
                    {
                        eggRarity = 3;
                    }
                    else if (rng < _configuration.Uncommon_Egg_AutoPoach_Rate) // Default: 16
                    {
                        eggRarity = 2;
                    }
                    else if (rng < _configuration.Common_Egg_AutoPoach_Rate) // Default: 32
                    {
                        eggRarity = 1;
                    }

                    if (eggRarity > 0)
                    {
                        var eggRowData = GetRowPointer(eggTable, jobEggId);
                        var EggCommonMonster = Memory.Instance.Read<int>((nuint)(eggRowData + 0x8));
                        var EggUncommonMonster = Memory.Instance.Read<int>((nuint)(eggRowData + 0xC));
                        var EggRareMonster = Memory.Instance.Read<int>((nuint)(eggRowData + 0x10));
                        
                        if (eggRarity == 1)
                        {
                            generatedMonsterID = EggCommonMonster;
                        }
                        else if (eggRarity == 2)
                        {
                            generatedMonsterID = EggUncommonMonster;
                        }
                        else if (eggRarity == 3)
                        {
                            generatedMonsterID = EggRareMonster;
                        }
                    }

                    if (generatedMonsterID != -1)
                    {
                        var generatedMonsterJob = GetRowPointer(jobTable, job);

                        var generatedMonsterJobCommonPoach = Memory.Instance.Read<int>((nuint)(generatedMonsterJob + 0x24));
                        var generatedMonsterJobRarePoach = Memory.Instance.Read<int>((nuint)(generatedMonsterJob + 0x28));

                        var generatedMonsterJobNamePointer = (generatedMonsterJob + 0x8) + Memory.Instance.Read<UInt32>((nuint)(generatedMonsterJob + 0x8));
                        var generatedMonsterJobName = "blank";
                        generatedMonsterJobName = Marshal.PtrToStringAnsi((nint)generatedMonsterJobNamePointer);

                        Log($"Monster {i} ({jobName}) rolled rarity: {eggRarity}({rng}): Monster: {generatedMonsterJobName}(0x{generatedMonsterID:X})", Color.LawnGreen);

                        var rollPoachID = generatedMonsterJobCommonPoach;

                        var poachRarityRoll = Random.Shared.Next(256);

                        if (poachRarityRoll <= _configuration.Premium_Poach_Rate) // Default: 31
                        {
                            rollPoachID = generatedMonsterJobRarePoach;
                        }

                        var poachItem = GetPoachItemData(rollPoachID);
                        var carcassItemNamePointer = (poachItem + 0x8) + Memory.Instance.Read<UInt32>((nuint)(poachItem + 0x8));
                        var carcassItemName = "blank";
                        carcassItemName = Marshal.PtrToStringAnsi((nint)carcassItemNamePointer).Replace("<Icon=103>", "+");

                        var carcassAddress = (nuint)(CarcassCountsAddress + (nuint)rollPoachID);
                        var carcassCount = Memory.Instance.Read<byte>(carcassAddress);

                        if (carcassCount < 99)
                        {
                            if (carcassItemName.Contains("+"))
                            {
                                _spoilsOverlay.LastPremiumCarcassList.Add(carcassItemName);
                            }
                            else
                            {
                                _spoilsOverlay.LastCarcassList.Add(carcassItemName);
                            }

                            carcassCount++;
                            Memory.Instance.SafeWrite(carcassAddress, new Span<byte>([carcassCount]));
                            Log($"Obtained {carcassItemName}, total count: {carcassCount}");
                        }
                        else
                        {
                            var carcassValue = Memory.Instance.Read<int>((nuint)(poachItem + 0x28)) / 2;

                            _spoilsOverlay.LastOverstockCarcassGil += carcassValue;
                            _spoilsOverlay.LastSoldCarcassList.Add((carcassItemName, carcassValue));

                            var newWarFunds = IncrementWarFunds_Func(carcassValue);
                            var originalWarFunds = newWarFunds - carcassValue;
                            Log($"Already have 99 of {carcassItemName}, sold for {carcassValue}. {originalWarFunds} -> {newWarFunds}");
                        }
                    }
                    else
                    {
                        Log($"Monster {i} ({jobName}) failed egg roll: {rng}", Color.Salmon);
                    }
                }
            }
        }

        void PoachThing(int monsterJob)
        {
            ////_logger.WriteLine($"Poaching monster of job: 0x{monsterJob:X}");

            //var jobTable = GetTablePointer(0x2E);
            //var poachItemTable = GetTablePointer(0xEC);
            //var itemTable = GetTablePointer(0x2D);

            //var jobRowData = GetRowPointer(jobTable, monsterJob);

            //var jobCommonPoach = Memory.Instance.Read<int>((nuint)(jobRowData + 0x24));
            //var jobRarePoach = Memory.Instance.Read<int>((nuint)(jobRowData + 0x28));

            //var jobNamePointer = (jobRowData + 0x8) + Memory.Instance.Read<UInt32>((nuint)(jobRowData + 0x8));
            //var jobName = "blank";

            //jobName = Marshal.PtrToStringAnsi((nint)jobNamePointer);

            //var commonPoachItem = GetRowPointer(poachItemTable, jobCommonPoach);
            //var rarePoachItem = GetRowPointer(poachItemTable, jobRarePoach);

            //if (_configuration.Logging)
            //{
            //    Log($"Job Name: 0x{jobNamePointer:X} {jobName}: {jobCommonPoach}, {jobRarePoach}", Color.DeepSkyBlue);
            //}
            _spoilsOverlay.WindowOpen = !_spoilsOverlay.WindowOpen;

            _spoilsOverlay.LastCarcassList.Clear();
            _spoilsOverlay.LastPremiumCarcassList.Clear();
            _spoilsOverlay.LastSoldCarcassList.Clear();

            _spoilsOverlay.LastCarcassList.Add("Test Carcass A");
            _spoilsOverlay.LastCarcassList.Add("Test Carcass B");
            _spoilsOverlay.LastCarcassList.Add("Test Carcass C");
            _spoilsOverlay.LastPremiumCarcassList.Add("Test Carcass D+");
            _spoilsOverlay.LastPremiumCarcassList.Add("Test Carcass E+");
            _spoilsOverlay.LastPremiumCarcassList.Add("Test Carcass F+");
            _spoilsOverlay.LastSoldCarcassList.Add(("Test Sold Carcass G", 1000));
            _spoilsOverlay.LastSoldCarcassList.Add(("Test Sold Carcass H", 50000));
            _spoilsOverlay.LastSoldCarcassList.Add(("Test Sold Carcass I+", 120000));
            _spoilsOverlay.LastOverstockCarcassGil = 171000;
        }

        void Log(string text, Color color = default)
        {
            if (!_configuration.Logging_Enabled)
            {
                return;
            }

            if (color == default)
            {
                color = Color.White;
            }

            //_logger.WriteLine($"[{_modConfig.ModId}] {text}", color);
            _imGuiShell.LogWriteLine(_modConfig.ModId, text, color, LoggerOutputTargetFlags.All);
        }

        private ulong GetTablePointer(int tableID)
        {
            var nexModule = Memory.Instance.Read<UInt64>(NexModulePointerAddress);
            return GetTable_Func(nexModule, tableID);
        }

        private ulong GetRowPointer(ulong tablePointer, int key)
        {
            var row = NexSearchRow1K_Func(tablePointer, key);
            return NexGetRowData_Func(row);
        }

        private ulong GetJobData(int jobID)
        {
            var jobTable = GetTablePointer(0x2E);
            var jobRowData = GetRowPointer(jobTable, jobID);
            return jobRowData;
        }

        private ulong GetPoachItemData(int poachID)
        {
            var poachItemTable = GetTablePointer(0xEC);
            var poachItemData = GetRowPointer(poachItemTable, poachID);
            return poachItemData;
        }

        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            _configuration = configuration;
            if (settingsMenu != null)
            {
                settingsMenu._configuration = configuration;
            }

            _logger.WriteLine($"[{_modConfig.ModId}] Config Updated.");
        }
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}