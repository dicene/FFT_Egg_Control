using FFT_Egg_Control.Configuration;
using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;
using System.Drawing;
using System.Numerics;
using static FFT_Egg_Control.Constants.Constants;

namespace FFT_Egg_Control
{
    public static class ExtensionMethods
    {
        public static Vector4 ToV4(this Color color) => new(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
    }

    [ImGuiMenu(Category = "Mods", Priority = 0, Owner = "Egg Control")]
    public class SettingsMenu : IImGuiComponent
    {
        public Config _configuration { get; set; }

        public SettingsMenu(Config configuration)
        {
            _configuration = configuration;
            LoadConfig();
        }

        public IImGui imGui { get; set; }

        public bool IsOverlay => false;
        public bool WindowOpen = true;

        private bool ConfigChanged = false;

        private bool Egg_Culling_Enabled;
        private bool AutoPoach_Enabled;
        private bool Logging_Enabled;

        private int Rare_Egg_AutoPoach_Rate;
        private int Uncommon_Egg_AutoPoach_Rate;
        private int Common_Egg_AutoPoach_Rate;
        private int Premium_Poach_Rate;

        private bool Allow_Chocobos_Eggs;
        private bool Allow_Black_Chocobos_Eggs;
        private bool Allow_Red_Chocobos_Eggs;
        private bool Allow_Goblins_Eggs;
        private bool Allow_Black_Goblins_Eggs;
        private bool Allow_Gobbledygooks_Eggs;
        private bool Allow_Bombs_Eggs;
        private bool Allow_Grenades_Eggs;
        private bool Allow_Exploders_Eggs;
        private bool Allow_Red_Panthers_Eggs;
        private bool Allow_Coeurls_Eggs;
        private bool Allow_Vampire_Cats_Eggs;
        private bool Allow_Piscodaemons_Eggs;
        private bool Allow_Squidrakens_Eggs;
        private bool Allow_Mindflayers_Eggs;
        private bool Allow_Skeletons_Eggs;
        private bool Allow_Bonesnatchs_Eggs;
        private bool Allow_Skeletal_Fiends_Eggs;
        private bool Allow_Ghouls_Eggs;
        private bool Allow_Ghasts_Eggs;
        private bool Allow_Revenants_Eggs;
        private bool Allow_Floating_Eyes_Eggs;
        private bool Allow_Ahrimans_Eggs;
        private bool Allow_Plague_Horrors_Eggs;
        private bool Allow_Jura_Aeviss_Eggs;
        private bool Allow_Steelhawks_Eggs;
        private bool Allow_Cockatrices_Eggs;
        private bool Allow_Pigs_Eggs;
        private bool Allow_Swines_Eggs;
        private bool Allow_Wild_Boars_Eggs;
        private bool Allow_Dryads_Eggs;
        private bool Allow_Treants_Eggs;
        private bool Allow_Elder_Treants_Eggs;
        private bool Allow_Wisenkins_Eggs;
        private bool Allow_Minotaurs_Eggs;
        private bool Allow_Sekhrets_Eggs;
        private bool Allow_Malboros_Eggs;
        private bool Allow_Ochus_Eggs;
        private bool Allow_Greater_Malboros_Eggs;
        private bool Allow_Behemoths_Eggs;
        private bool Allow_Behemoth_Kings_Eggs;
        private bool Allow_Dark_Behemoths_Eggs;
        private bool Allow_Dragons_Eggs;
        private bool Allow_Blue_Dragons_Eggs;
        private bool Allow_Red_Dragons_Eggs;
        private bool Allow_Hydras_Eggs;
        private bool Allow_Greater_Hydras_Eggs;
        private bool Allow_Tiamats_Eggs;

        private bool AutoPoach_Chocobos;
        private bool AutoPoach_Black_Chocobos;
        private bool AutoPoach_Red_Chocobos;
        private bool AutoPoach_Goblins;
        private bool AutoPoach_Black_Goblins;
        private bool AutoPoach_Gobbledygooks;
        private bool AutoPoach_Bombs;
        private bool AutoPoach_Grenades;
        private bool AutoPoach_Exploders;
        private bool AutoPoach_Red_Panthers;
        private bool AutoPoach_Coeurls;
        private bool AutoPoach_Vampire_Cats;
        private bool AutoPoach_Piscodaemons;
        private bool AutoPoach_Squidrakens;
        private bool AutoPoach_Mindflayers;
        private bool AutoPoach_Skeletons;
        private bool AutoPoach_Bonesnatchs;
        private bool AutoPoach_Skeletal_Fiends;
        private bool AutoPoach_Ghouls;
        private bool AutoPoach_Ghasts;
        private bool AutoPoach_Revenants;
        private bool AutoPoach_Floating_Eyes;
        private bool AutoPoach_Ahrimans;
        private bool AutoPoach_Plague_Horrors;
        private bool AutoPoach_Jura_Aeviss;
        private bool AutoPoach_Steelhawks;
        private bool AutoPoach_Cockatrices;
        private bool AutoPoach_Pigs;
        private bool AutoPoach_Swines;
        private bool AutoPoach_Wild_Boars;
        private bool AutoPoach_Dryads;
        private bool AutoPoach_Treants;
        private bool AutoPoach_Elder_Treants;
        private bool AutoPoach_Wisenkins;
        private bool AutoPoach_Minotaurs;
        private bool AutoPoach_Sekhrets;
        private bool AutoPoach_Malboros;
        private bool AutoPoach_Ochus;
        private bool AutoPoach_Greater_Malboros;
        private bool AutoPoach_Behemoths;
        private bool AutoPoach_Behemoth_Kings;
        private bool AutoPoach_Dark_Behemoths;
        private bool AutoPoach_Dragons;
        private bool AutoPoach_Blue_Dragons;
        private bool AutoPoach_Red_Dragons;
        private bool AutoPoach_Hydras;
        private bool AutoPoach_Greater_Hydras;
        private bool AutoPoach_Tiamats;

        private Dictionary<MonsterID, (string, bool, bool)> MonsterInfoMap { get; set; }

        public Action PoachAction { get; set; }
        public Action CarcassCountAction { get; set; }
        public Func<Dictionary<int, int>> GetUnitCountAction { get; set; }
        public Func<Dictionary<int, (int, int, int, int)>> GetUnitsAction { get; set; }
        public Dictionary<MonsterID, Func<bool>> MonsterIDToConfigMap { get; set; }
        public Dictionary<int, int> MonsterUnitCount => GetUnitCountAction();
        public bool EnabledChanged = false;

        public void RenderMenu(IImGuiShell imGuiShell)
        {
            if (imGui.MenuItem("Egg Control"))
            {
                WindowOpen = true;
            }
        }

        public void Render(IImGuiShell imGuiShell)
        {
            if (WindowOpen)
            {
                var size = imGui.GetMainViewport().Size;
                var workSize = imGui.GetMainViewport().WorkSize;

                imGui.SetNextWindowSize(new Vector2(400, workSize.Y * 0.8f), ImGuiCond.ImGuiCond_Appearing);
                imGui.SetNextWindowPos(new Vector2(size.X - 400 - 100, workSize.Y * 0.2f / 2f), ImGuiCond.ImGuiCond_Appearing);
                var result = imGui.Begin("Egg Control Settings", ref WindowOpen, ImGuiWindowFlags.ImGuiWindowFlags_None);
                
                if (imGui.IsWindowCollapsed())
                {
                    imGui.End();
                    return;
                }

                //imGui.Text($"Size: {size.X}, {size.Y}");
                //imGui.Text($"WorkSize: {workSize.X}, {workSize.Y}");

                if (imGui.Button("Test overlay!"))
                {
                    PoachAction?.Invoke();
                }

                //var units = GetUnitsAction();
                //foreach ((int i, (int spriteSet, int unitIndex, int job, int index2)) in units)
                //{
                //    imGui.Text($"Unit {i}: SpriteSet=0x{spriteSet:X}, UnitIndex=0x{unitIndex:X}, Job=0x{job:X}, Index2=0x{index2:X}");
                //}

                LoadConfig();

                ConfigChanged = false;
                
                ConfigChanged = ConfigChanged || imGui.Checkbox("Cull Unwanted Eggs", ref Egg_Culling_Enabled);
                ConfigChanged = ConfigChanged || imGui.Checkbox("Log Debug Information", ref Logging_Enabled);
                ConfigChanged = ConfigChanged || imGui.Checkbox("Auto-Poach", ref AutoPoach_Enabled);

                var toggleTextMaxWidth = imGui.CalcTextSize("Uncommon Egg AutoPoach Rate:");

                imGui.Separator();
                //imGui.SetNextItemWidth(toggleTextMaxWidth.X);
                //imGui.Text("Rare Egg AutoPoach Rate:");
                //imGui.SameLine();
                imGui.SetNextItemWidth(150);
                ConfigChanged = ConfigChanged || imGui.SliderInt("Rare_Egg_AutoPoach_Rate", ref Rare_Egg_AutoPoach_Rate, 0, 255);

                //imGui.SetNextItemWidth(toggleTextMaxWidth.X);
                //imGui.Text("Uncommon Egg AutoPoach Rate:");
                //imGui.SameLine();
                imGui.SetNextItemWidth(150);
                ConfigChanged = ConfigChanged || imGui.SliderInt("Uncommon_Egg_AutoPoach_Rate", ref Uncommon_Egg_AutoPoach_Rate, 0, 255);
                
                //imGui.SetNextItemWidth(toggleTextMaxWidth.X);
                //imGui.Text("Common Egg AutoPoach Rate:");
                //imGui.SameLine();
                imGui.SetNextItemWidth(150);
                ConfigChanged = ConfigChanged || imGui.SliderInt("Common_Egg_AutoPoach_Rate", ref Common_Egg_AutoPoach_Rate, 0, 255);
                
                //imGui.SetNextItemWidth(toggleTextMaxWidth.X);
                //imGui.Text("Premium Poach Rate:");
                //imGui.SameLine();
                imGui.SetNextItemWidth(150);
                ConfigChanged = ConfigChanged || imGui.SliderInt("Premium_Poach_Rate", ref Premium_Poach_Rate, 0, 255);
                //ConfigChanged = ConfigChanged || imGui.SliderInt("Rare Egg Auto-Poach Rate", ref Rare_Egg_AutoPoach_Rate, 0, 255);
                //ConfigChanged = ConfigChanged || imGui.SliderInt("Uncommon Egg Auto-Poach Rate", ref Uncommon_Egg_AutoPoach_Rate, 0, 255);
                //ConfigChanged = ConfigChanged || imGui.SliderInt("Common Egg Auto-Poach Rate", ref Common_Egg_AutoPoach_Rate, 0, 255);
                //ConfigChanged = ConfigChanged || imGui.SliderInt("Premium Auto-Poach Rate", ref Premium_Poach_Rate, 0, 255);

                imGui.Separator();
                imGui.Text("Monster Auto-Poach Settings:");

                var monsterCounts = MonsterUnitCount;
                var maxNameSize = imGui.CalcTextSize("Greater Malboros_12345678");

                void DrawMonsterCheckbox(MonsterID monsterID, ref bool enabledValue, ref bool autoPoachValue)
                {
                    imGui.TableNextRow();
                    imGui.TableNextColumn();
                    var monsterName = monsterID.ToString().Replace("_", " ");
                    //imGui.PushItemWidth(maxNameSize.X);
                    imGui.Text(monsterName);
                    //imGui.PopItemWidth();
                    //imGui.SameLine();
                    imGui.TableNextColumn();
                    var monsterCount = monsterCounts.ContainsKey((int)monsterID) ? monsterCounts[(int)monsterID] : 0;
                    imGui.TextColored(monsterCount > 0 ? Color.White.ToV4() : Color.DarkGray.ToV4(), monsterCount.ToString());
                    //imGui.SameLine();
                    imGui.TableNextColumn();
                    ConfigChanged = ConfigChanged || imGui.Checkbox($"##Allow_{monsterName}", ref enabledValue);
                    //ConfigChanged = ConfigChanged || imGui.Checkbox($"Enabled##{monsterName}", ref enabledValue);
                    //imGui.SameLine();
                    imGui.TableNextColumn();
                    ConfigChanged = ConfigChanged || imGui.Checkbox($"##AutoPoach_{monsterName}", ref autoPoachValue);
                    //ConfigChanged = ConfigChanged || imGui.Checkbox($"Auto-Poach##{monsterName}", ref autoPoachValue);
                }

                void TableRowSeparator()
                {
                    //imGui.PushStyleColorImVec4(ImGuiCol.ImGuiCol_TableRowBgAlt, Color.FromArgb(125, 20, 120, 20).ToV4());
                    //imGui.PushStyleColorImVec4(ImGuiCol.ImGuiCol_TableRowBg, Color.FromArgb(125, 20, 120, 20).ToV4());
                    //imGui.TableSetBgColor(ImGuiTableBgTarget.ImGuiTableBgTarget_RowBg0, 0, -1);
                    imGui.TableNextRow();
                    //imGui.TableNextRowEx(ImGuiTableRowFlags.ImGuiTableRowFlags_None, 5f);
                    imGui.TableNextColumn();
                    //imGui.TableSetBgColor(ImGuiTableBgTarget.ImGuiTableBgTarget_RowBg0, 0, -1);
                    //imGui.Text("Separator1");
                    //imGui.TableSetBgColor(ImGuiTableBgTarget.ImGuiTableBgTarget_RowBg0, 0, -1);
                    //imGui.Text("Separator2");
                    imGui.TableSetBgColor(ImGuiTableBgTarget.ImGuiTableBgTarget_RowBg0, 0xff404040, -1);
                    //imGui.Separator();
                    //imGui.Dummy(new Vector2(100, 100));
                    //imGui.PopStyleColor();
                    //imGui.PopStyleColor();
                }

                imGui.BeginTable("MonsterOptions", 4, ImGuiTableFlags.ImGuiTableFlags_SizingFixedFit | ImGuiTableFlags.ImGuiTableFlags_Borders | ImGuiTableFlags.ImGuiTableFlags_NoHostExtendX);
                //imGui.BeginTable("MonsterOptions", 4, ImGuiTableFlags.ImGuiTableFlags_SizingStretchSame | ImGuiTableFlags.ImGuiTableFlags_Borders | ImGuiTableFlags.ImGuiTableFlags_Resizable);
                //imGui.BeginTable("MonsterOptions", 4, ImGuiTableFlags.ImGuiTableFlags_SizingStretchSame | ImGuiTableFlags.ImGuiTableFlags_BordersInnerH | ImGuiTableFlags.ImGuiTableFlags_Resizable | ImGuiTableFlags.ImGuiTableFlags_RowBg);

                imGui.PushStyleColorImVec4(ImGuiCol.ImGuiCol_TableRowBg, Color.FromArgb(255, 0, 0, 0).ToV4());
                imGui.PushStyleColorImVec4(ImGuiCol.ImGuiCol_TableRowBgAlt, Color.FromArgb(255, 0, 0, 0).ToV4());

                imGui.TableNextRow();
                imGui.TableNextColumn();
                imGui.Text("Type");
                imGui.TableNextColumn();
                imGui.Text("# in units");
                imGui.TableNextColumn();
                imGui.Text("Allow Eggs");
                imGui.TableNextColumn();
                imGui.Text("AutoPoach");

                DrawMonsterCheckbox(MonsterID.Chocobo, ref Allow_Chocobos_Eggs, ref AutoPoach_Chocobos);
                DrawMonsterCheckbox(MonsterID.Black_Chocobo, ref Allow_Black_Chocobos_Eggs, ref AutoPoach_Black_Chocobos);
                DrawMonsterCheckbox(MonsterID.Red_Chocobo, ref Allow_Red_Chocobos_Eggs, ref AutoPoach_Red_Chocobos);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Goblin, ref Allow_Goblins_Eggs, ref AutoPoach_Goblins);
                DrawMonsterCheckbox(MonsterID.Black_Goblin, ref Allow_Black_Goblins_Eggs, ref AutoPoach_Black_Goblins);
                DrawMonsterCheckbox(MonsterID.Gobbledygook, ref Allow_Gobbledygooks_Eggs, ref AutoPoach_Gobbledygooks);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Bomb, ref Allow_Bombs_Eggs, ref AutoPoach_Bombs);
                DrawMonsterCheckbox(MonsterID.Grenade, ref Allow_Grenades_Eggs, ref AutoPoach_Grenades);
                DrawMonsterCheckbox(MonsterID.Exploder, ref Allow_Exploders_Eggs, ref AutoPoach_Exploders);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Red_Panther, ref Allow_Red_Panthers_Eggs, ref AutoPoach_Red_Panthers);
                DrawMonsterCheckbox(MonsterID.Coeurl, ref Allow_Coeurls_Eggs, ref AutoPoach_Coeurls);
                DrawMonsterCheckbox(MonsterID.Vampire_Cat, ref Allow_Vampire_Cats_Eggs, ref AutoPoach_Vampire_Cats);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Piscodaemon, ref Allow_Piscodaemons_Eggs, ref AutoPoach_Piscodaemons);
                DrawMonsterCheckbox(MonsterID.Squidraken, ref Allow_Squidrakens_Eggs, ref AutoPoach_Squidrakens);
                DrawMonsterCheckbox(MonsterID.Mindflayer, ref Allow_Mindflayers_Eggs, ref AutoPoach_Mindflayers);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Skeleton, ref Allow_Skeletons_Eggs, ref AutoPoach_Skeletons);
                DrawMonsterCheckbox(MonsterID.Bonesnatch, ref Allow_Bonesnatchs_Eggs, ref AutoPoach_Bonesnatchs);
                DrawMonsterCheckbox(MonsterID.Skeletal_Fiend, ref Allow_Skeletal_Fiends_Eggs, ref AutoPoach_Skeletal_Fiends);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Ghoul, ref Allow_Ghouls_Eggs, ref AutoPoach_Ghouls);
                DrawMonsterCheckbox(MonsterID.Ghast, ref Allow_Ghasts_Eggs, ref AutoPoach_Ghasts);
                DrawMonsterCheckbox(MonsterID.Revenant, ref Allow_Revenants_Eggs, ref AutoPoach_Revenants);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Floating_Eye, ref Allow_Floating_Eyes_Eggs, ref AutoPoach_Floating_Eyes);
                DrawMonsterCheckbox(MonsterID.Ahriman, ref Allow_Ahrimans_Eggs, ref AutoPoach_Ahrimans);
                DrawMonsterCheckbox(MonsterID.Plague_Horror, ref Allow_Plague_Horrors_Eggs, ref AutoPoach_Plague_Horrors);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Jura_Aevis, ref Allow_Jura_Aeviss_Eggs, ref AutoPoach_Jura_Aeviss);
                DrawMonsterCheckbox(MonsterID.Steelhawk, ref Allow_Steelhawks_Eggs, ref AutoPoach_Steelhawks);
                DrawMonsterCheckbox(MonsterID.Cockatrice, ref Allow_Cockatrices_Eggs, ref AutoPoach_Cockatrices);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Pig, ref Allow_Pigs_Eggs, ref AutoPoach_Pigs);
                DrawMonsterCheckbox(MonsterID.Swine, ref Allow_Swines_Eggs, ref AutoPoach_Swines);
                DrawMonsterCheckbox(MonsterID.Wild_Boar, ref Allow_Wild_Boars_Eggs, ref AutoPoach_Wild_Boars);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Dryad, ref Allow_Dryads_Eggs, ref AutoPoach_Dryads);
                DrawMonsterCheckbox(MonsterID.Treant, ref Allow_Treants_Eggs, ref AutoPoach_Treants);
                DrawMonsterCheckbox(MonsterID.Elder_Treant, ref Allow_Elder_Treants_Eggs, ref AutoPoach_Elder_Treants);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Wisenkin, ref Allow_Wisenkins_Eggs, ref AutoPoach_Wisenkins);
                DrawMonsterCheckbox(MonsterID.Minotaur, ref Allow_Minotaurs_Eggs, ref AutoPoach_Minotaurs);
                DrawMonsterCheckbox(MonsterID.Sekhret, ref Allow_Sekhrets_Eggs, ref AutoPoach_Sekhrets);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Malboro, ref Allow_Malboros_Eggs, ref AutoPoach_Malboros);
                DrawMonsterCheckbox(MonsterID.Ochu, ref Allow_Ochus_Eggs, ref AutoPoach_Ochus);
                DrawMonsterCheckbox(MonsterID.Greater_Malboro, ref Allow_Greater_Malboros_Eggs, ref AutoPoach_Greater_Malboros);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Behemoth, ref Allow_Behemoths_Eggs, ref AutoPoach_Behemoths);
                DrawMonsterCheckbox(MonsterID.Behemoth_King, ref Allow_Behemoth_Kings_Eggs, ref AutoPoach_Behemoth_Kings);
                DrawMonsterCheckbox(MonsterID.Dark_Behemoth, ref Allow_Dark_Behemoths_Eggs, ref AutoPoach_Dark_Behemoths);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Dragon, ref Allow_Dragons_Eggs, ref AutoPoach_Dragons);
                DrawMonsterCheckbox(MonsterID.Blue_Dragon, ref Allow_Blue_Dragons_Eggs, ref AutoPoach_Blue_Dragons);
                DrawMonsterCheckbox(MonsterID.Red_Dragon, ref Allow_Red_Dragons_Eggs, ref AutoPoach_Red_Dragons);
                TableRowSeparator();
                DrawMonsterCheckbox(MonsterID.Hydra, ref Allow_Hydras_Eggs, ref AutoPoach_Hydras);
                DrawMonsterCheckbox(MonsterID.Greater_Hydra, ref Allow_Greater_Hydras_Eggs, ref AutoPoach_Greater_Hydras);
                DrawMonsterCheckbox(MonsterID.Tiamat, ref Allow_Tiamats_Eggs, ref AutoPoach_Tiamats);
                imGui.PopStyleColor();
                imGui.PopStyleColor();
                imGui.EndTable();

                //for (int i = 0; i < 4; i++)
                //{
                //    imGui.Text($"Item {i}: ");
                //    imGui.SameLine();
                //    imGui.TextColored(Color.LightGray.ToV4(), "Common");
                //    imGui.SameLine();
                //    imGui.TextColored(Color.Gold.ToV4(), "Rare");
                //}                

                imGui.End();

                if (ConfigChanged)
                {
                    SaveConfig();
                    ConfigChanged = false;
                }
            }
        }

        public void LoadConfig()
        {
            Egg_Culling_Enabled = _configuration.Egg_Culling_Enabled;
            AutoPoach_Enabled = _configuration.AutoPoach_Enabled;
            Logging_Enabled = _configuration.Logging_Enabled;

            Rare_Egg_AutoPoach_Rate = _configuration.Rare_Egg_AutoPoach_Rate;
            Uncommon_Egg_AutoPoach_Rate = _configuration.Uncommon_Egg_AutoPoach_Rate;
            Common_Egg_AutoPoach_Rate = _configuration.Common_Egg_AutoPoach_Rate;
            Premium_Poach_Rate = _configuration.Premium_Poach_Rate;

            Allow_Chocobos_Eggs = _configuration.Allow_Chocobos_Eggs;
            Allow_Black_Chocobos_Eggs = _configuration.Allow_Black_Chocobos_Eggs;
            Allow_Red_Chocobos_Eggs = _configuration.Allow_Red_Chocobos_Eggs;
            Allow_Goblins_Eggs = _configuration.Allow_Goblins_Eggs;
            Allow_Black_Goblins_Eggs = _configuration.Allow_Black_Goblins_Eggs;
            Allow_Gobbledygooks_Eggs = _configuration.Allow_Gobbledygooks_Eggs;
            Allow_Bombs_Eggs = _configuration.Allow_Bombs_Eggs;
            Allow_Grenades_Eggs = _configuration.Allow_Grenades_Eggs;
            Allow_Exploders_Eggs = _configuration.Allow_Exploders_Eggs;
            Allow_Red_Panthers_Eggs = _configuration.Allow_Red_Panthers_Eggs;
            Allow_Coeurls_Eggs = _configuration.Allow_Coeurls_Eggs;
            Allow_Vampire_Cats_Eggs = _configuration.Allow_Vampire_Cats_Eggs;
            Allow_Piscodaemons_Eggs = _configuration.Allow_Piscodaemons_Eggs;
            Allow_Squidrakens_Eggs = _configuration.Allow_Squidrakens_Eggs;
            Allow_Mindflayers_Eggs = _configuration.Allow_Mindflayers_Eggs;
            Allow_Skeletons_Eggs = _configuration.Allow_Skeletons_Eggs;
            Allow_Bonesnatchs_Eggs = _configuration.Allow_Bonesnatchs_Eggs;
            Allow_Skeletal_Fiends_Eggs = _configuration.Allow_Skeletal_Fiends_Eggs;
            Allow_Ghouls_Eggs = _configuration.Allow_Ghouls_Eggs;
            Allow_Ghasts_Eggs = _configuration.Allow_Ghasts_Eggs;
            Allow_Revenants_Eggs = _configuration.Allow_Revenants_Eggs;
            Allow_Floating_Eyes_Eggs = _configuration.Allow_Floating_Eyes_Eggs;
            Allow_Ahrimans_Eggs = _configuration.Allow_Ahrimans_Eggs;
            Allow_Plague_Horrors_Eggs = _configuration.Allow_Plague_Horrors_Eggs;
            Allow_Jura_Aeviss_Eggs = _configuration.Allow_Jura_Aeviss_Eggs;
            Allow_Steelhawks_Eggs = _configuration.Allow_Steelhawks_Eggs;
            Allow_Cockatrices_Eggs = _configuration.Allow_Cockatrices_Eggs;
            Allow_Pigs_Eggs = _configuration.Allow_Pigs_Eggs;
            Allow_Swines_Eggs = _configuration.Allow_Swines_Eggs;
            Allow_Wild_Boars_Eggs = _configuration.Allow_Wild_Boars_Eggs;
            Allow_Dryads_Eggs = _configuration.Allow_Dryads_Eggs;
            Allow_Treants_Eggs = _configuration.Allow_Treants_Eggs;
            Allow_Elder_Treants_Eggs = _configuration.Allow_Elder_Treants_Eggs;
            Allow_Wisenkins_Eggs = _configuration.Allow_Wisenkins_Eggs;
            Allow_Minotaurs_Eggs = _configuration.Allow_Minotaurs_Eggs;
            Allow_Sekhrets_Eggs = _configuration.Allow_Sekhrets_Eggs;
            Allow_Malboros_Eggs = _configuration.Allow_Malboros_Eggs;
            Allow_Ochus_Eggs = _configuration.Allow_Ochus_Eggs;
            Allow_Greater_Malboros_Eggs = _configuration.Allow_Greater_Malboros_Eggs;
            Allow_Behemoths_Eggs = _configuration.Allow_Behemoths_Eggs;
            Allow_Behemoth_Kings_Eggs = _configuration.Allow_Behemoth_Kings_Eggs;
            Allow_Dark_Behemoths_Eggs = _configuration.Allow_Dark_Behemoths_Eggs;
            Allow_Dragons_Eggs = _configuration.Allow_Dragons_Eggs;
            Allow_Blue_Dragons_Eggs = _configuration.Allow_Blue_Dragons_Eggs;
            Allow_Red_Dragons_Eggs = _configuration.Allow_Red_Dragons_Eggs;
            Allow_Hydras_Eggs = _configuration.Allow_Hydras_Eggs;
            Allow_Greater_Hydras_Eggs = _configuration.Allow_Greater_Hydras_Eggs;
            Allow_Tiamats_Eggs = _configuration.Allow_Tiamats_Eggs;

            AutoPoach_Chocobos = _configuration.AutoPoach_Chocobos;
            AutoPoach_Black_Chocobos = _configuration.AutoPoach_Black_Chocobos;
            AutoPoach_Red_Chocobos = _configuration.AutoPoach_Red_Chocobos;
            AutoPoach_Goblins = _configuration.AutoPoach_Goblins;
            AutoPoach_Black_Goblins = _configuration.AutoPoach_Black_Goblins;
            AutoPoach_Gobbledygooks = _configuration.AutoPoach_Gobbledygooks;
            AutoPoach_Bombs = _configuration.AutoPoach_Bombs;
            AutoPoach_Grenades = _configuration.AutoPoach_Grenades;
            AutoPoach_Exploders = _configuration.AutoPoach_Exploders;
            AutoPoach_Red_Panthers = _configuration.AutoPoach_Red_Panthers;
            AutoPoach_Coeurls = _configuration.AutoPoach_Coeurls;
            AutoPoach_Vampire_Cats = _configuration.AutoPoach_Vampire_Cats;
            AutoPoach_Piscodaemons = _configuration.AutoPoach_Piscodaemons;
            AutoPoach_Squidrakens = _configuration.AutoPoach_Squidrakens;
            AutoPoach_Mindflayers = _configuration.AutoPoach_Mindflayers;
            AutoPoach_Skeletons = _configuration.AutoPoach_Skeletons;
            AutoPoach_Bonesnatchs = _configuration.AutoPoach_Bonesnatchs;
            AutoPoach_Skeletal_Fiends = _configuration.AutoPoach_Skeletal_Fiends;
            AutoPoach_Ghouls = _configuration.AutoPoach_Ghouls;
            AutoPoach_Ghasts = _configuration.AutoPoach_Ghasts;
            AutoPoach_Revenants = _configuration.AutoPoach_Revenants;
            AutoPoach_Floating_Eyes = _configuration.AutoPoach_Floating_Eyes;
            AutoPoach_Ahrimans = _configuration.AutoPoach_Ahrimans;
            AutoPoach_Plague_Horrors = _configuration.AutoPoach_Plague_Horrors;
            AutoPoach_Jura_Aeviss = _configuration.AutoPoach_Jura_Aeviss;
            AutoPoach_Steelhawks = _configuration.AutoPoach_Steelhawks;
            AutoPoach_Cockatrices = _configuration.AutoPoach_Cockatrices;
            AutoPoach_Pigs = _configuration.AutoPoach_Pigs;
            AutoPoach_Swines = _configuration.AutoPoach_Swines;
            AutoPoach_Wild_Boars = _configuration.AutoPoach_Wild_Boars;
            AutoPoach_Dryads = _configuration.AutoPoach_Dryads;
            AutoPoach_Treants = _configuration.AutoPoach_Treants;
            AutoPoach_Elder_Treants = _configuration.AutoPoach_Elder_Treants;
            AutoPoach_Wisenkins = _configuration.AutoPoach_Wisenkins;
            AutoPoach_Minotaurs = _configuration.AutoPoach_Minotaurs;
            AutoPoach_Sekhrets = _configuration.AutoPoach_Sekhrets;
            AutoPoach_Malboros = _configuration.AutoPoach_Malboros;
            AutoPoach_Ochus = _configuration.AutoPoach_Ochus;
            AutoPoach_Greater_Malboros = _configuration.AutoPoach_Greater_Malboros;
            AutoPoach_Behemoths = _configuration.AutoPoach_Behemoths;
            AutoPoach_Behemoth_Kings = _configuration.AutoPoach_Behemoth_Kings;
            AutoPoach_Dark_Behemoths = _configuration.AutoPoach_Dark_Behemoths;
            AutoPoach_Dragons = _configuration.AutoPoach_Dragons;
            AutoPoach_Blue_Dragons = _configuration.AutoPoach_Blue_Dragons;
            AutoPoach_Red_Dragons = _configuration.AutoPoach_Red_Dragons;
            AutoPoach_Hydras = _configuration.AutoPoach_Hydras;
            AutoPoach_Greater_Hydras = _configuration.AutoPoach_Greater_Hydras;
            AutoPoach_Tiamats = _configuration.AutoPoach_Tiamats;
        }

        public void SaveConfig()
        {
            _configuration.Egg_Culling_Enabled = Egg_Culling_Enabled;
            _configuration.AutoPoach_Enabled = AutoPoach_Enabled;
            _configuration.Logging_Enabled = Logging_Enabled;

            _configuration.Rare_Egg_AutoPoach_Rate = Rare_Egg_AutoPoach_Rate;
            _configuration.Uncommon_Egg_AutoPoach_Rate = Uncommon_Egg_AutoPoach_Rate;
            _configuration.Common_Egg_AutoPoach_Rate = Common_Egg_AutoPoach_Rate;
            _configuration.Premium_Poach_Rate = Premium_Poach_Rate;

            _configuration.Allow_Chocobos_Eggs = Allow_Chocobos_Eggs;
            _configuration.Allow_Black_Chocobos_Eggs = Allow_Black_Chocobos_Eggs;
            _configuration.Allow_Red_Chocobos_Eggs = Allow_Red_Chocobos_Eggs;
            _configuration.Allow_Goblins_Eggs = Allow_Goblins_Eggs;
            _configuration.Allow_Black_Goblins_Eggs = Allow_Black_Goblins_Eggs;
            _configuration.Allow_Gobbledygooks_Eggs = Allow_Gobbledygooks_Eggs;
            _configuration.Allow_Bombs_Eggs = Allow_Bombs_Eggs;
            _configuration.Allow_Grenades_Eggs = Allow_Grenades_Eggs;
            _configuration.Allow_Exploders_Eggs = Allow_Exploders_Eggs;
            _configuration.Allow_Red_Panthers_Eggs = Allow_Red_Panthers_Eggs;
            _configuration.Allow_Coeurls_Eggs = Allow_Coeurls_Eggs;
            _configuration.Allow_Vampire_Cats_Eggs = Allow_Vampire_Cats_Eggs;
            _configuration.Allow_Piscodaemons_Eggs = Allow_Piscodaemons_Eggs;
            _configuration.Allow_Squidrakens_Eggs = Allow_Squidrakens_Eggs;
            _configuration.Allow_Mindflayers_Eggs = Allow_Mindflayers_Eggs;
            _configuration.Allow_Skeletons_Eggs = Allow_Skeletons_Eggs;
            _configuration.Allow_Bonesnatchs_Eggs = Allow_Bonesnatchs_Eggs;
            _configuration.Allow_Skeletal_Fiends_Eggs = Allow_Skeletal_Fiends_Eggs;
            _configuration.Allow_Ghouls_Eggs = Allow_Ghouls_Eggs;
            _configuration.Allow_Ghasts_Eggs = Allow_Ghasts_Eggs;
            _configuration.Allow_Revenants_Eggs = Allow_Revenants_Eggs;
            _configuration.Allow_Floating_Eyes_Eggs = Allow_Floating_Eyes_Eggs;
            _configuration.Allow_Ahrimans_Eggs = Allow_Ahrimans_Eggs;
            _configuration.Allow_Plague_Horrors_Eggs = Allow_Plague_Horrors_Eggs;
            _configuration.Allow_Jura_Aeviss_Eggs = Allow_Jura_Aeviss_Eggs;
            _configuration.Allow_Steelhawks_Eggs = Allow_Steelhawks_Eggs;
            _configuration.Allow_Cockatrices_Eggs = Allow_Cockatrices_Eggs;
            _configuration.Allow_Pigs_Eggs = Allow_Pigs_Eggs;
            _configuration.Allow_Swines_Eggs = Allow_Swines_Eggs;
            _configuration.Allow_Wild_Boars_Eggs = Allow_Wild_Boars_Eggs;
            _configuration.Allow_Dryads_Eggs = Allow_Dryads_Eggs;
            _configuration.Allow_Treants_Eggs = Allow_Treants_Eggs;
            _configuration.Allow_Elder_Treants_Eggs = Allow_Elder_Treants_Eggs;
            _configuration.Allow_Wisenkins_Eggs = Allow_Wisenkins_Eggs;
            _configuration.Allow_Minotaurs_Eggs = Allow_Minotaurs_Eggs;
            _configuration.Allow_Sekhrets_Eggs = Allow_Sekhrets_Eggs;
            _configuration.Allow_Malboros_Eggs = Allow_Malboros_Eggs;
            _configuration.Allow_Ochus_Eggs = Allow_Ochus_Eggs;
            _configuration.Allow_Greater_Malboros_Eggs = Allow_Greater_Malboros_Eggs;
            _configuration.Allow_Behemoths_Eggs = Allow_Behemoths_Eggs;
            _configuration.Allow_Behemoth_Kings_Eggs = Allow_Behemoth_Kings_Eggs;
            _configuration.Allow_Dark_Behemoths_Eggs = Allow_Dark_Behemoths_Eggs;
            _configuration.Allow_Dragons_Eggs = Allow_Dragons_Eggs;
            _configuration.Allow_Blue_Dragons_Eggs = Allow_Blue_Dragons_Eggs;
            _configuration.Allow_Red_Dragons_Eggs = Allow_Red_Dragons_Eggs;
            _configuration.Allow_Hydras_Eggs = Allow_Hydras_Eggs;
            _configuration.Allow_Greater_Hydras_Eggs = Allow_Greater_Hydras_Eggs;
            _configuration.Allow_Tiamats_Eggs = Allow_Tiamats_Eggs;

            _configuration.AutoPoach_Chocobos = AutoPoach_Chocobos;
            _configuration.AutoPoach_Black_Chocobos = AutoPoach_Black_Chocobos;
            _configuration.AutoPoach_Red_Chocobos = AutoPoach_Red_Chocobos;
            _configuration.AutoPoach_Goblins = AutoPoach_Goblins;
            _configuration.AutoPoach_Black_Goblins = AutoPoach_Black_Goblins;
            _configuration.AutoPoach_Gobbledygooks = AutoPoach_Gobbledygooks;
            _configuration.AutoPoach_Bombs = AutoPoach_Bombs;
            _configuration.AutoPoach_Grenades = AutoPoach_Grenades;
            _configuration.AutoPoach_Exploders = AutoPoach_Exploders;
            _configuration.AutoPoach_Red_Panthers = AutoPoach_Red_Panthers;
            _configuration.AutoPoach_Coeurls = AutoPoach_Coeurls;
            _configuration.AutoPoach_Vampire_Cats = AutoPoach_Vampire_Cats;
            _configuration.AutoPoach_Piscodaemons = AutoPoach_Piscodaemons;
            _configuration.AutoPoach_Squidrakens = AutoPoach_Squidrakens;
            _configuration.AutoPoach_Mindflayers = AutoPoach_Mindflayers;
            _configuration.AutoPoach_Skeletons = AutoPoach_Skeletons;
            _configuration.AutoPoach_Bonesnatchs = AutoPoach_Bonesnatchs;
            _configuration.AutoPoach_Skeletal_Fiends = AutoPoach_Skeletal_Fiends;
            _configuration.AutoPoach_Ghouls = AutoPoach_Ghouls;
            _configuration.AutoPoach_Ghasts = AutoPoach_Ghasts;
            _configuration.AutoPoach_Revenants = AutoPoach_Revenants;
            _configuration.AutoPoach_Floating_Eyes = AutoPoach_Floating_Eyes;
            _configuration.AutoPoach_Ahrimans = AutoPoach_Ahrimans;
            _configuration.AutoPoach_Plague_Horrors = AutoPoach_Plague_Horrors;
            _configuration.AutoPoach_Jura_Aeviss = AutoPoach_Jura_Aeviss;
            _configuration.AutoPoach_Steelhawks = AutoPoach_Steelhawks;
            _configuration.AutoPoach_Cockatrices = AutoPoach_Cockatrices;
            _configuration.AutoPoach_Pigs = AutoPoach_Pigs;
            _configuration.AutoPoach_Swines = AutoPoach_Swines;
            _configuration.AutoPoach_Wild_Boars = AutoPoach_Wild_Boars;
            _configuration.AutoPoach_Dryads = AutoPoach_Dryads;
            _configuration.AutoPoach_Treants = AutoPoach_Treants;
            _configuration.AutoPoach_Elder_Treants = AutoPoach_Elder_Treants;
            _configuration.AutoPoach_Wisenkins = AutoPoach_Wisenkins;
            _configuration.AutoPoach_Minotaurs = AutoPoach_Minotaurs;
            _configuration.AutoPoach_Sekhrets = AutoPoach_Sekhrets;
            _configuration.AutoPoach_Malboros = AutoPoach_Malboros;
            _configuration.AutoPoach_Ochus = AutoPoach_Ochus;
            _configuration.AutoPoach_Greater_Malboros = AutoPoach_Greater_Malboros;
            _configuration.AutoPoach_Behemoths = AutoPoach_Behemoths;
            _configuration.AutoPoach_Behemoth_Kings = AutoPoach_Behemoth_Kings;
            _configuration.AutoPoach_Dark_Behemoths = AutoPoach_Dark_Behemoths;
            _configuration.AutoPoach_Dragons = AutoPoach_Dragons;
            _configuration.AutoPoach_Blue_Dragons = AutoPoach_Blue_Dragons;
            _configuration.AutoPoach_Red_Dragons = AutoPoach_Red_Dragons;
            _configuration.AutoPoach_Hydras = AutoPoach_Hydras;
            _configuration.AutoPoach_Greater_Hydras = AutoPoach_Greater_Hydras;
            _configuration.AutoPoach_Tiamats = AutoPoach_Tiamats;
            _configuration.Save();
        }
    }
}
