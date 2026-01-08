using FFT_Egg_Control.Template.Configuration;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FFT_Egg_Control.Configuration
{
    public class Config : Configurable<Config>
    {
        [DisplayName("Egg Culling Enabled")]
        [Description("Toggle whether to prevent unwanted types of eggs from being spawned.")]
        [DefaultValue(true)]
        [Display(Order = 0)]
        public bool Egg_Culling_Enabled { get; set; } = true;

        [DisplayName("AutoPoach")]
        [Description("Toggle whether to occasionally generate carcasses from the types of monsters your units could generate eggs for. Note: This does NOT kill your units.")]
        [DefaultValue(true)]
        [Display(Order = 1)]
        //public bool AutoPoach_Enabled = true;
        public bool AutoPoach_Enabled { get; set; } = true;

        [DisplayName("Logging")]
        [Description("Whether to log egg creations.")]
        [DefaultValue(true)]
        [Display(Order = 2)]
        public bool Logging_Enabled { get; set; } = true;
        
        [DisplayName("Rare Egg AutoPoach Rate")]
        [Description("Likelihood of generating and auto-poaching the rare egg from a creature after battle. Formula for this calculation is <rare>/256. Defaults to 6 (2.3% chance).")]
        [DefaultValue(true)]
        [Display(Order = 3)]
        public int Rare_Egg_AutoPoach_Rate { get; set; } = 6;

        [DisplayName("Uncommon Egg AutoPoach Rate")]
        [Description("Likelihood of generating and auto-poaching the uncommon egg from a creature after battle. Formula for this calculation is <uncommon - rare>/256. Defaults to 16 (3.9% chance assuming default rare rate of 6).")]
        [DefaultValue(true)]
        [Display(Order = 4)]
        public int Uncommon_Egg_AutoPoach_Rate { get; set; } = 16;

        [DisplayName("Common Egg AutoPoach Rate")]
        [Description("Likelihood of generating and auto-poaching the common egg from a creature after battle. Formula for this calculation is <common - uncommon>/256. Defaults to 32 (6.25% chance assuming default uncommon rate of 16)")]
        [DefaultValue(true)]
        [Display(Order = 5)]
        public int Common_Egg_AutoPoach_Rate { get; set; } = 32;

        [DisplayName("Premium Poach Rate")]
        [Description("Likelihood of generating a premium carcass from an autopoach. Formula is <x>/256. Defaults to 31 (12.1% chance)")]
        [DefaultValue(true)]
        [Display(Order = 6)]
        public int Premium_Poach_Rate { get; set; } = 31;

        [DisplayName("Allow Chocobo Eggs")]
        [Description("Whether to allow new Chocobo eggs.")]
        [DefaultValue(true)]
        [Display(Order = 12)]
        public bool Allow_Chocobos_Eggs { get; set; } = true;

        [DisplayName("Allow Black Chocobo Eggs")]
        [Description("Whether to allow new Black Chocobo eggs.")]
        [DefaultValue(true)]
        [Display(Order = 13)]
        public bool Allow_Black_Chocobos_Eggs { get; set; } = true;

        [DisplayName("Allow Red Chocobo Eggs")]
        [Description("Whether to allow new Red Chocobo eggs.")]
        [DefaultValue(true)]
        [Display(Order = 14)]
        public bool Allow_Red_Chocobos_Eggs { get; set; } = true;

        [DisplayName("Allow Goblin Eggs")]
        [Description("Whether to allow new Goblin eggs.")]
        [DefaultValue(true)]
        [Display(Order = 15)]
        public bool Allow_Goblins_Eggs { get; set; } = true;

        [DisplayName("Allow Black Goblin Eggs")]
        [Description("Whether to allow new Black Goblin eggs.")]
        [DefaultValue(true)]
        [Display(Order = 16)]
        public bool Allow_Black_Goblins_Eggs { get; set; } = true;

        [DisplayName("Allow Gobbledygook Eggs")]
        [Description("Whether to allow new Gobbledygook eggs.")]
        [DefaultValue(true)]
        [Display(Order = 17)]
        public bool Allow_Gobbledygooks_Eggs { get; set; } = true;

        [DisplayName("Allow Bomb Eggs")]
        [Description("Whether to allow new Bomb eggs.")]
        [DefaultValue(true)]
        [Display(Order = 18)]
        public bool Allow_Bombs_Eggs { get; set; } = true;

        [DisplayName("Allow Grenade Eggs")]
        [Description("Whether to allow new Grenade eggs.")]
        [DefaultValue(true)]
        [Display(Order = 19)]
        public bool Allow_Grenades_Eggs { get; set; } = true;

        [DisplayName("Allow Exploder Eggs")]
        [Description("Whether to allow new Exploder eggs.")]
        [DefaultValue(true)]
        [Display(Order = 20)]
        public bool Allow_Exploders_Eggs { get; set; } = true;

        [DisplayName("Allow Red Panther Eggs")]
        [Description("Whether to allow new Red Panther eggs.")]
        [DefaultValue(true)]
        [Display(Order = 21)]
        public bool Allow_Red_Panthers_Eggs { get; set; } = true;

        [DisplayName("Allow Coeurl Eggs")]
        [Description("Whether to allow new Coeurl eggs.")]
        [DefaultValue(true)]
        [Display(Order = 22)]
        public bool Allow_Coeurls_Eggs { get; set; } = true;

        [DisplayName("Allow Vampire Cat Eggs")]
        [Description("Whether to allow new Vampire Cat eggs.")]
        [DefaultValue(true)]
        [Display(Order = 23)]
        public bool Allow_Vampire_Cats_Eggs { get; set; } = true;

        [DisplayName("Allow Piscodaemon Eggs")]
        [Description("Whether to allow new Piscodaemon eggs.")]
        [DefaultValue(true)]
        [Display(Order = 24)]
        public bool Allow_Piscodaemons_Eggs { get; set; } = true;

        [DisplayName("Allow Squidraken Eggs")]
        [Description("Whether to allow new Squidraken eggs.")]
        [DefaultValue(true)]
        [Display(Order = 25)]
        public bool Allow_Squidrakens_Eggs { get; set; } = true;

        [DisplayName("Allow Mindflayer Eggs")]
        [Description("Whether to allow new Mindflayer eggs.")]
        [DefaultValue(true)]
        [Display(Order = 26)]
        public bool Allow_Mindflayers_Eggs { get; set; } = true;

        [DisplayName("Allow Skeleton Eggs")]
        [Description("Whether to allow new Skeleton eggs.")]
        [DefaultValue(true)]
        [Display(Order = 27)]
        public bool Allow_Skeletons_Eggs { get; set; } = true;

        [DisplayName("Allow Bonesnatch Eggs")]
        [Description("Whether to allow new Bonesnatch eggs.")]
        [DefaultValue(true)]
        [Display(Order = 28)]
        public bool Allow_Bonesnatchs_Eggs { get; set; } = true;

        [DisplayName("Allow Skeletal Fiend Eggs")]
        [Description("Whether to allow new Skeletal Fiend eggs.")]
        [DefaultValue(true)]
        [Display(Order = 29)]
        public bool Allow_Skeletal_Fiends_Eggs { get; set; } = true;

        [DisplayName("Allow Ghoul Eggs")]
        [Description("Whether to allow new Ghoul eggs.")]
        [DefaultValue(true)]
        [Display(Order = 30)]
        public bool Allow_Ghouls_Eggs { get; set; } = true;

        [DisplayName("Allow Ghast Eggs")]
        [Description("Whether to allow new Ghast eggs.")]
        [DefaultValue(true)]
        [Display(Order = 31)]
        public bool Allow_Ghasts_Eggs { get; set; } = true;

        [DisplayName("Allow Revenant Eggs")]
        [Description("Whether to allow new Revenant eggs.")]
        [DefaultValue(true)]
        [Display(Order = 32)]
        public bool Allow_Revenants_Eggs { get; set; } = true;

        [DisplayName("Allow Floating Eye Eggs")]
        [Description("Whether to allow new Floating Eye eggs.")]
        [DefaultValue(true)]
        [Display(Order = 33)]
        public bool Allow_Floating_Eyes_Eggs { get; set; } = true;

        [DisplayName("Allow Ahriman Eggs")]
        [Description("Whether to allow new Ahriman eggs.")]
        [DefaultValue(true)]
        [Display(Order = 34)]
        public bool Allow_Ahrimans_Eggs { get; set; } = true;

        [DisplayName("Allow Plague Horror Eggs")]
        [Description("Whether to allow new Plague Horror eggs.")]
        [DefaultValue(true)]
        [Display(Order = 35)]
        public bool Allow_Plague_Horrors_Eggs { get; set; } = true;

        [DisplayName("Allow Jura Aevis Eggs")]
        [Description("Whether to allow new Jura Aevis eggs.")]
        [DefaultValue(true)]
        [Display(Order = 36)]
        public bool Allow_Jura_Aeviss_Eggs { get; set; } = true;

        [DisplayName("Allow Steelhawk Eggs")]
        [Description("Whether to allow new Steelhawk eggs.")]
        [DefaultValue(true)]
        [Display(Order = 37)]
        public bool Allow_Steelhawks_Eggs { get; set; } = true;

        [DisplayName("Allow Cockatrice Eggs")]
        [Description("Whether to allow new Cockatrice eggs.")]
        [DefaultValue(true)]
        [Display(Order = 38)]
        public bool Allow_Cockatrices_Eggs { get; set; } = true;

        [DisplayName("Allow Pig Eggs")]
        [Description("Whether to allow new Pig eggs.")]
        [DefaultValue(true)]
        [Display(Order = 39)]
        public bool Allow_Pigs_Eggs { get; set; } = true;

        [DisplayName("Allow Swine Eggs")]
        [Description("Whether to allow new Swine eggs.")]
        [DefaultValue(true)]
        [Display(Order = 40)]
        public bool Allow_Swines_Eggs { get; set; } = true;

        [DisplayName("Allow Wild Boar Eggs")]
        [Description("Whether to allow new Wild Boar eggs.")]
        [DefaultValue(true)]
        [Display(Order = 41)]
        public bool Allow_Wild_Boars_Eggs { get; set; } = true;

        [DisplayName("Allow Dryad Eggs")]
        [Description("Whether to allow new Dryad eggs.")]
        [DefaultValue(true)]
        [Display(Order = 42)]
        public bool Allow_Dryads_Eggs { get; set; } = true;

        [DisplayName("Allow Treant Eggs")]
        [Description("Whether to allow new Treant eggs.")]
        [DefaultValue(true)]
        [Display(Order = 43)]
        public bool Allow_Treants_Eggs { get; set; } = true;

        [DisplayName("Allow Elder Treant Eggs")]
        [Description("Whether to allow new Elder Treant eggs.")]
        [DefaultValue(true)]
        [Display(Order = 44)]
        public bool Allow_Elder_Treants_Eggs { get; set; } = true;

        [DisplayName("Allow Wisenkin Eggs")]
        [Description("Whether to allow new Wisenkin eggs.")]
        [DefaultValue(true)]
        [Display(Order = 45)]
        public bool Allow_Wisenkins_Eggs { get; set; } = true;

        [DisplayName("Allow Minotaur Eggs")]
        [Description("Whether to allow new Minotaur eggs.")]
        [DefaultValue(true)]
        [Display(Order = 46)]
        public bool Allow_Minotaurs_Eggs { get; set; } = true;

        [DisplayName("Allow Sekhret Eggs")]
        [Description("Whether to allow new Sekhret eggs.")]
        [DefaultValue(true)]
        [Display(Order = 47)]
        public bool Allow_Sekhrets_Eggs { get; set; } = true;

        [DisplayName("Allow Malboro Eggs")]
        [Description("Whether to allow new Malboro eggs.")]
        [DefaultValue(true)]
        [Display(Order = 48)]
        public bool Allow_Malboros_Eggs { get; set; } = true;

        [DisplayName("Allow Ochu Eggs")]
        [Description("Whether to allow new Ochu eggs.")]
        [DefaultValue(true)]
        [Display(Order = 49)]
        public bool Allow_Ochus_Eggs { get; set; } = true;

        [DisplayName("Allow Greater Malboro Eggs")]
        [Description("Whether to allow new Greater Malboro eggs.")]
        [DefaultValue(true)]
        [Display(Order = 50)]
        public bool Allow_Greater_Malboros_Eggs { get; set; } = true;

        [DisplayName("Allow Behemoth Eggs")]
        [Description("Whether to allow new Behemoth eggs.")]
        [DefaultValue(true)]
        [Display(Order = 51)]
        public bool Allow_Behemoths_Eggs { get; set; } = true;

        [DisplayName("Allow Behemoth King Eggs")]
        [Description("Whether to allow new Behemoth King eggs.")]
        [DefaultValue(true)]
        [Display(Order = 52)]
        public bool Allow_Behemoth_Kings_Eggs { get; set; } = true;

        [DisplayName("Allow Dark Behemoth Eggs")]
        [Description("Whether to allow new Dark Behemoth eggs.")]
        [DefaultValue(true)]
        [Display(Order = 53)]
        public bool Allow_Dark_Behemoths_Eggs { get; set; } = true;

        [DisplayName("Allow Dragon Eggs")]
        [Description("Whether to allow new Dragon eggs.")]
        [DefaultValue(true)]
        [Display(Order = 54)]
        public bool Allow_Dragons_Eggs { get; set; } = true;

        [DisplayName("Allow Blue Dragon Eggs")]
        [Description("Whether to allow new Blue Dragon eggs.")]
        [DefaultValue(true)]
        [Display(Order = 55)]
        public bool Allow_Blue_Dragons_Eggs { get; set; } = true;

        [DisplayName("Allow Red Dragon Eggs")]
        [Description("Whether to allow new Red Dragon eggs.")]
        [DefaultValue(true)]
        [Display(Order = 56)]
        public bool Allow_Red_Dragons_Eggs { get; set; } = true;

        [DisplayName("Allow Hydra Eggs")]
        [Description("Whether to allow new Hydra eggs.")]
        [DefaultValue(true)]
        [Display(Order = 57)]
        public bool Allow_Hydras_Eggs { get; set; } = true;

        [DisplayName("Allow Greater Hydra Eggs")]
        [Description("Whether to allow new Greater Hydra eggs.")]
        [DefaultValue(true)]
        [Display(Order = 58)]
        public bool Allow_Greater_Hydras_Eggs { get; set; } = true;

        [DisplayName("Allow Tiamat Eggs")]
        [Description("Whether to allow new Tiamat eggs.")]
        [DefaultValue(true)]
        [Display(Order = 59)]
        public bool Allow_Tiamats_Eggs { get; set; } = true;

        [DisplayName("AutoPoach Chocobo")]
        [Description("Whether to autopoach Chocobos.")]
        [DefaultValue(true)]
        [Display(Order = 62)]
        public bool AutoPoach_Chocobos { get; set; } = true;

        [DisplayName("AutoPoach Black Chocobo")]
        [Description("Whether to autopoach Black Chocobos.")]
        [DefaultValue(true)]
        [Display(Order = 63)]
        public bool AutoPoach_Black_Chocobos { get; set; } = true;

        [DisplayName("AutoPoach Red Chocobo")]
        [Description("Whether to autopoach Red Chocobos.")]
        [DefaultValue(true)]
        [Display(Order = 64)]
        public bool AutoPoach_Red_Chocobos { get; set; } = true;

        [DisplayName("AutoPoach Goblin")]
        [Description("Whether to autopoach Goblins.")]
        [DefaultValue(true)]
        [Display(Order = 65)]
        public bool AutoPoach_Goblins { get; set; } = true;

        [DisplayName("AutoPoach Black Goblin")]
        [Description("Whether to autopoach Black Goblins.")]
        [DefaultValue(true)]
        [Display(Order = 66)]
        public bool AutoPoach_Black_Goblins { get; set; } = true;

        [DisplayName("AutoPoach Gobbledygook")]
        [Description("Whether to autopoach Gobbledygooks.")]
        [DefaultValue(true)]
        [Display(Order = 67)]
        public bool AutoPoach_Gobbledygooks { get; set; } = true;

        [DisplayName("AutoPoach Bomb")]
        [Description("Whether to autopoach Bombs.")]
        [DefaultValue(true)]
        [Display(Order = 68)]
        public bool AutoPoach_Bombs { get; set; } = true;

        [DisplayName("AutoPoach Grenade")]
        [Description("Whether to autopoach Grenades.")]
        [DefaultValue(true)]
        [Display(Order = 69)]
        public bool AutoPoach_Grenades { get; set; } = true;

        [DisplayName("AutoPoach Exploder")]
        [Description("Whether to autopoach Exploders.")]
        [DefaultValue(true)]
        [Display(Order = 70)]
        public bool AutoPoach_Exploders { get; set; } = true;

        [DisplayName("AutoPoach Red Panther")]
        [Description("Whether to autopoach Red Panthers.")]
        [DefaultValue(true)]
        [Display(Order = 71)]
        public bool AutoPoach_Red_Panthers { get; set; } = true;

        [DisplayName("AutoPoach Coeurl")]
        [Description("Whether to autopoach Coeurls.")]
        [DefaultValue(true)]
        [Display(Order = 72)]
        public bool AutoPoach_Coeurls { get; set; } = true;

        [DisplayName("AutoPoach Vampire Cat")]
        [Description("Whether to autopoach Vampire Cats.")]
        [DefaultValue(true)]
        [Display(Order = 73)]
        public bool AutoPoach_Vampire_Cats { get; set; } = true;

        [DisplayName("AutoPoach Piscodaemon")]
        [Description("Whether to autopoach Piscodaemons.")]
        [DefaultValue(true)]
        [Display(Order = 74)]
        public bool AutoPoach_Piscodaemons { get; set; } = true;

        [DisplayName("AutoPoach Squidraken")]
        [Description("Whether to autopoach Squidrakens.")]
        [DefaultValue(true)]
        [Display(Order = 75)]
        public bool AutoPoach_Squidrakens { get; set; } = true;

        [DisplayName("AutoPoach Mindflayer")]
        [Description("Whether to autopoach Mindflayers.")]
        [DefaultValue(true)]
        [Display(Order = 76)]
        public bool AutoPoach_Mindflayers { get; set; } = true;

        [DisplayName("AutoPoach Skeleton")]
        [Description("Whether to autopoach Skeletons.")]
        [DefaultValue(true)]
        [Display(Order = 77)]
        public bool AutoPoach_Skeletons { get; set; } = true;

        [DisplayName("AutoPoach Bonesnatch")]
        [Description("Whether to autopoach Bonesnatchs.")]
        [DefaultValue(true)]
        [Display(Order = 78)]
        public bool AutoPoach_Bonesnatchs { get; set; } = true;

        [DisplayName("AutoPoach Skeletal Fiend")]
        [Description("Whether to autopoach Skeletal Fiends.")]
        [DefaultValue(true)]
        [Display(Order = 79)]
        public bool AutoPoach_Skeletal_Fiends { get; set; } = true;

        [DisplayName("AutoPoach Ghoul")]
        [Description("Whether to autopoach Ghouls.")]
        [DefaultValue(true)]
        [Display(Order = 80)]
        public bool AutoPoach_Ghouls { get; set; } = true;

        [DisplayName("AutoPoach Ghast")]
        [Description("Whether to autopoach Ghasts.")]
        [DefaultValue(true)]
        [Display(Order = 81)]
        public bool AutoPoach_Ghasts { get; set; } = true;

        [DisplayName("AutoPoach Revenant")]
        [Description("Whether to autopoach Revenants.")]
        [DefaultValue(true)]
        [Display(Order = 82)]
        public bool AutoPoach_Revenants { get; set; } = true;

        [DisplayName("AutoPoach Floating Eye")]
        [Description("Whether to autopoach Floating Eyes.")]
        [DefaultValue(true)]
        [Display(Order = 83)]
        public bool AutoPoach_Floating_Eyes { get; set; } = true;

        [DisplayName("AutoPoach Ahriman")]
        [Description("Whether to autopoach Ahrimans.")]
        [DefaultValue(true)]
        [Display(Order = 84)]
        public bool AutoPoach_Ahrimans { get; set; } = true;

        [DisplayName("AutoPoach Plague Horror")]
        [Description("Whether to autopoach Plague Horrors.")]
        [DefaultValue(true)]
        [Display(Order = 85)]
        public bool AutoPoach_Plague_Horrors { get; set; } = true;

        [DisplayName("AutoPoach Jura Aevis")]
        [Description("Whether to autopoach Jura Aeviss.")]
        [DefaultValue(true)]
        [Display(Order = 86)]
        public bool AutoPoach_Jura_Aeviss { get; set; } = true;

        [DisplayName("AutoPoach Steelhawk")]
        [Description("Whether to autopoach Steelhawks.")]
        [DefaultValue(true)]
        [Display(Order = 87)]
        public bool AutoPoach_Steelhawks { get; set; } = true;

        [DisplayName("AutoPoach Cockatrice")]
        [Description("Whether to autopoach Cockatrices.")]
        [DefaultValue(true)]
        [Display(Order = 88)]
        public bool AutoPoach_Cockatrices { get; set; } = true;

        [DisplayName("AutoPoach Pig")]
        [Description("Whether to autopoach Pigs.")]
        [DefaultValue(true)]
        [Display(Order = 89)]
        public bool AutoPoach_Pigs { get; set; } = true;

        [DisplayName("AutoPoach Swine")]
        [Description("Whether to autopoach Swines.")]
        [DefaultValue(true)]
        [Display(Order = 90)]
        public bool AutoPoach_Swines { get; set; } = true;

        [DisplayName("AutoPoach Wild Boar")]
        [Description("Whether to autopoach Wild Boars.")]
        [DefaultValue(true)]
        [Display(Order = 91)]
        public bool AutoPoach_Wild_Boars { get; set; } = true;

        [DisplayName("AutoPoach Dryad")]
        [Description("Whether to autopoach Dryads.")]
        [DefaultValue(true)]
        [Display(Order = 92)]
        public bool AutoPoach_Dryads { get; set; } = true;

        [DisplayName("AutoPoach Treant")]
        [Description("Whether to autopoach Treants.")]
        [DefaultValue(true)]
        [Display(Order = 93)]
        public bool AutoPoach_Treants { get; set; } = true;

        [DisplayName("AutoPoach Elder Treant")]
        [Description("Whether to autopoach Elder Treants.")]
        [DefaultValue(true)]
        [Display(Order = 94)]
        public bool AutoPoach_Elder_Treants { get; set; } = true;

        [DisplayName("AutoPoach Wisenkin")]
        [Description("Whether to autopoach Wisenkins.")]
        [DefaultValue(true)]
        [Display(Order = 95)]
        public bool AutoPoach_Wisenkins { get; set; } = true;

        [DisplayName("AutoPoach Minotaur")]
        [Description("Whether to autopoach Minotaurs.")]
        [DefaultValue(true)]
        [Display(Order = 96)]
        public bool AutoPoach_Minotaurs { get; set; } = true;

        [DisplayName("AutoPoach Sekhret")]
        [Description("Whether to autopoach Sekhrets.")]
        [DefaultValue(true)]
        [Display(Order = 97)]
        public bool AutoPoach_Sekhrets { get; set; } = true;

        [DisplayName("AutoPoach Malboro")]
        [Description("Whether to autopoach Malboros.")]
        [DefaultValue(true)]
        [Display(Order = 98)]
        public bool AutoPoach_Malboros { get; set; } = true;

        [DisplayName("AutoPoach Ochu")]
        [Description("Whether to autopoach Ochus.")]
        [DefaultValue(true)]
        [Display(Order = 99)]
        public bool AutoPoach_Ochus { get; set; } = true;

        [DisplayName("AutoPoach Greater Malboro")]
        [Description("Whether to autopoach Greater Malboros.")]
        [DefaultValue(true)]
        [Display(Order = 100)]
        public bool AutoPoach_Greater_Malboros { get; set; } = true;

        [DisplayName("AutoPoach Behemoth")]
        [Description("Whether to autopoach Behemoths.")]
        [DefaultValue(true)]
        [Display(Order = 101)]
        public bool AutoPoach_Behemoths { get; set; } = true;

        [DisplayName("AutoPoach Behemoth King")]
        [Description("Whether to autopoach Behemoth Kings.")]
        [DefaultValue(true)]
        [Display(Order = 102)]
        public bool AutoPoach_Behemoth_Kings { get; set; } = true;

        [DisplayName("AutoPoach Dark Behemoth")]
        [Description("Whether to autopoach Dark Behemoths.")]
        [DefaultValue(true)]
        [Display(Order = 103)]
        public bool AutoPoach_Dark_Behemoths { get; set; } = true;

        [DisplayName("AutoPoach Dragon")]
        [Description("Whether to autopoach Dragons.")]
        [DefaultValue(true)]
        [Display(Order = 104)]
        public bool AutoPoach_Dragons { get; set; } = true;

        [DisplayName("AutoPoach Blue Dragon")]
        [Description("Whether to autopoach Blue Dragons.")]
        [DefaultValue(true)]
        [Display(Order = 105)]
        public bool AutoPoach_Blue_Dragons { get; set; } = true;

        [DisplayName("AutoPoach Red Dragon")]
        [Description("Whether to autopoach Red Dragons.")]
        [DefaultValue(true)]
        [Display(Order = 106)]
        public bool AutoPoach_Red_Dragons { get; set; } = true;

        [DisplayName("AutoPoach Hydra")]
        [Description("Whether to autopoach Hydras.")]
        [DefaultValue(true)]
        [Display(Order = 107)]
        public bool AutoPoach_Hydras { get; set; } = true;

        [DisplayName("AutoPoach Greater Hydra")]
        [Description("Whether to autopoach Greater Hydras.")]
        [DefaultValue(true)]
        [Display(Order = 108)]
        public bool AutoPoach_Greater_Hydras { get; set; } = true;

        [DisplayName("AutoPoach Tiamat")]
        [Description("Whether to autopoach Tiamats.")]
        [DefaultValue(true)]
        [Display(Order = 109)]
        public bool AutoPoach_Tiamats { get; set; } = true;
    }

    public class ConfiguratorMixin : ConfiguratorMixinBase
    {

    }
}
