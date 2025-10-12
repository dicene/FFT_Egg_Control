using FFT_Egg_Control.Configuration;
using FFT_Egg_Control.Template;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Memory.Sigscan;
using Reloaded.Mod.Interfaces;

#if DEBUG
using System.Diagnostics;
using System.Drawing;
using static FFT_Egg_Control.Constants.Constants;
#endif

namespace FFT_Egg_Control
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase // <= Do not Remove.
    {
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader _modLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks? _hooks;

        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod _owner;

        /// <summary>
        /// Provides access to this mod's configuration.
        /// </summary>
        private Config _configuration;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig _modConfig;

        public Dictionary<MonsterID, Func<bool>> MonsterIDToConfigMap;

        [Function([FunctionAttribute.Register.rcx, FunctionAttribute.Register.rdx, FunctionAttribute.Register.r14], FunctionAttribute.Register.rax, false)]
        private delegate Int64 SpawnCreatureEgg(SpawnType spawnType, int eggID, MonsterID monsterID);

        private Reloaded.Hooks.Definitions.IHook<SpawnCreatureEgg> SpawnCreatureEgg_Hook;

        private Int64 SpawnCreatureEgg_Replacement(SpawnType spawnType, int eggID, MonsterID monsterID)
        {
            var blockSpawn = false;

            if (spawnType == SpawnType.Generic_Monster && _configuration.Enabled && MonsterIDToConfigMap.ContainsKey(monsterID) && !MonsterIDToConfigMap[monsterID]())
            {
                if (_configuration.Enabled)
                {
                    blockSpawn = true;
                }
            }

            if (blockSpawn)
            {
                if (_configuration.Logging)
                {
                    _logger.WriteLineAsync($"Discarding {monsterID} egg.");
                }

                return -1;
            }

            if (_configuration.Logging)
            {
                _logger.WriteLineAsync($"Allowing {monsterID} egg.");
            }

            return SpawnCreatureEgg_Hook.OriginalFunction(spawnType, eggID, monsterID);
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
            // Debugger.Launch();
#endif
            var thisProcess = Process.GetCurrentProcess();
            var scanner = new Scanner(thisProcess, thisProcess.MainModule);
            var scanResult = scanner.FindPattern("40 53 48 83 ec 20 41 89 c8 e8 ?? ?? ?? ?? 48 63 d8 85 c0 78 46"); // 0x14DE9E013 in Original release of FFTIC

            if (!scanResult.Found)
            {
                _logger.WriteLineAsync($"Failed to find AoB pattern for SpawnCreatureEgg function!", Color.OrangeRed);
                return;
            }

            MonsterIDToConfigMap = new()
            {
                [MonsterID.Chocobo] = () => { return _configuration.Allow_Chocobo; },
                [MonsterID.Black_Chocobo] = () => { return _configuration.Allow_Black_Chocobo; },
                [MonsterID.Red_Chocobo] = () => { return _configuration.Allow_Red_Chocobo; },
                [MonsterID.Goblin] = () => { return _configuration.Allow_Goblin; },
                [MonsterID.Black_Goblin] = () => { return _configuration.Allow_Black_Goblin; },
                [MonsterID.Gobbledygook] = () => { return _configuration.Allow_Gobbledygook; },
                [MonsterID.Bomb] = () => { return _configuration.Allow_Bomb; },
                [MonsterID.Grenade] = () => { return _configuration.Allow_Grenade; },
                [MonsterID.Exploder] = () => { return _configuration.Allow_Exploder; },
                [MonsterID.Red_Panther] = () => { return _configuration.Allow_Red_Panther; },
                [MonsterID.Coeurl] = () => { return _configuration.Allow_Coeurl; },
                [MonsterID.Vampire_Cat] = () => { return _configuration.Allow_Vampire_Cat; },
                [MonsterID.Piscodaemon] = () => { return _configuration.Allow_Piscodaemon; },
                [MonsterID.Squidraken] = () => { return _configuration.Allow_Squidraken; },
                [MonsterID.Mindflayer] = () => { return _configuration.Allow_Mindflayer; },
                [MonsterID.Skeleton] = () => { return _configuration.Allow_Skeleton; },
                [MonsterID.Bonesnatch] = () => { return _configuration.Allow_Bonesnatch; },
                [MonsterID.Skeletal_Fiend] = () => { return _configuration.Allow_Skeletal_Fiend; },
                [MonsterID.Ghoul] = () => { return _configuration.Allow_Ghoul; },
                [MonsterID.Ghast] = () => { return _configuration.Allow_Ghast; },
                [MonsterID.Revenant] = () => { return _configuration.Allow_Revenant; },
                [MonsterID.Floating_Eye] = () => { return _configuration.Allow_Floating_Eye; },
                [MonsterID.Ahriman] = () => { return _configuration.Allow_Ahriman; },
                [MonsterID.Plague_Horror] = () => { return _configuration.Allow_Plague_Horror; },
                [MonsterID.Jura_Aevis] = () => { return _configuration.Allow_Jura_Aevis; },
                [MonsterID.Steelhawk] = () => { return _configuration.Allow_Steelhawk; },
                [MonsterID.Cockatrice] = () => { return _configuration.Allow_Cockatrice; },
                [MonsterID.Pig] = () => { return _configuration.Allow_Pig; },
                [MonsterID.Swine] = () => { return _configuration.Allow_Swine; },
                [MonsterID.Wild_Boar] = () => { return _configuration.Allow_Wild_Boar; },
                [MonsterID.Dryad] = () => { return _configuration.Allow_Dryad; },
                [MonsterID.Treant] = () => { return _configuration.Allow_Treant; },
                [MonsterID.Elder_Treant] = () => { return _configuration.Allow_Elder_Treant; },
                [MonsterID.Wisenkin] = () => { return _configuration.Allow_Wisenkin; },
                [MonsterID.Minotaur] = () => { return _configuration.Allow_Minotaur; },
                [MonsterID.Sekhret] = () => { return _configuration.Allow_Sekhret; },
                [MonsterID.Malboro] = () => { return _configuration.Allow_Malboro; },
                [MonsterID.Ochu] = () => { return _configuration.Allow_Ochu; },
                [MonsterID.Greater_Malboro] = () => { return _configuration.Allow_Greater_Malboro; },
                [MonsterID.Behemoth] = () => { return _configuration.Allow_Behemoth; },
                [MonsterID.Behemoth_King] = () => { return _configuration.Allow_Behemoth_King; },
                [MonsterID.Dark_Behemoth] = () => { return _configuration.Allow_Dark_Behemoth; },
                [MonsterID.Dragon] = () => { return _configuration.Allow_Dragon; },
                [MonsterID.Blue_Dragon] = () => { return _configuration.Allow_Blue_Dragon; },
                [MonsterID.Red_Dragon] = () => { return _configuration.Allow_Red_Dragon; },
                [MonsterID.Hydra] = () => { return _configuration.Allow_Hydra; },
                [MonsterID.Greater_Hydra] = () => { return _configuration.Allow_Greater_Hydra; },
                [MonsterID.Tiamat] = () => { return _configuration.Allow_Tiamat; },
            };

            SpawnCreatureEgg_Hook = _hooks!.CreateHook<SpawnCreatureEgg>(SpawnCreatureEgg_Replacement, Process.GetCurrentProcess().MainModule.BaseAddress + scanResult.Offset).Activate();
        }

        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            // Apply settings from configuration.
            // ... your code here.
            _configuration = configuration;
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