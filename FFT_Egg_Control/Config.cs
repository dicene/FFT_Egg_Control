using FFT_Egg_Control.Template.Configuration;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FFT_Egg_Control.Configuration
{
    public class Config : Configurable<Config>
    {
        [DisplayName("Enabled")]
        [Description("Toggle whether the mod is active.")]
        [DefaultValue(true)]
        [Display(Order = 0)]
        public bool Enabled { get; set; } = true;

        [DisplayName("Logging")]
        [Description("Whether to log egg creations.")]
        [DefaultValue(true)]
        [Display(Order = 1)]
        public bool Logging { get; set; } = true;

        [DisplayName("Allow Chocobo")]
        [Description("Whether to allow new Chocobo eggs.")]
        [DefaultValue(true)]
        [Display(Order = 2)]
        public bool Allow_Chocobo { get; set; } = true;

        [DisplayName("Allow Black Chocobo")]
        [Description("Whether to allow new Black Chocobo eggs.")]
        [DefaultValue(true)]
        [Display(Order = 3)]
        public bool Allow_Black_Chocobo { get; set; } = true;

        [DisplayName("Allow Red Chocobo")]
        [Description("Whether to allow new Red Chocobo eggs.")]
        [DefaultValue(true)]
        [Display(Order = 4)]
        public bool Allow_Red_Chocobo { get; set; } = true;

        [DisplayName("Allow Goblin")]
        [Description("Whether to allow new Goblin eggs.")]
        [DefaultValue(true)]
        [Display(Order = 5)]
        public bool Allow_Goblin { get; set; } = true;

        [DisplayName("Allow Black Goblin")]
        [Description("Whether to allow new Black Goblin eggs.")]
        [DefaultValue(true)]
        [Display(Order = 6)]
        public bool Allow_Black_Goblin { get; set; } = true;

        [DisplayName("Allow Gobbledygook")]
        [Description("Whether to allow new Gobbledygook eggs.")]
        [DefaultValue(true)]
        [Display(Order = 7)]
        public bool Allow_Gobbledygook { get; set; } = true;

        [DisplayName("Allow Bomb")]
        [Description("Whether to allow new Bomb eggs.")]
        [DefaultValue(true)]
        [Display(Order = 8)]
        public bool Allow_Bomb { get; set; } = true;

        [DisplayName("Allow Grenade")]
        [Description("Whether to allow new Grenade eggs.")]
        [DefaultValue(true)]
        [Display(Order = 9)]
        public bool Allow_Grenade { get; set; } = true;

        [DisplayName("Allow Exploder")]
        [Description("Whether to allow new Exploder eggs.")]
        [DefaultValue(true)]
        [Display(Order = 10)]
        public bool Allow_Exploder { get; set; } = true;

        [DisplayName("Allow Red Panther")]
        [Description("Whether to allow new Red Panther eggs.")]
        [DefaultValue(true)]
        [Display(Order = 11)]
        public bool Allow_Red_Panther { get; set; } = true;

        [DisplayName("Allow Coeurl")]
        [Description("Whether to allow new Coeurl eggs.")]
        [DefaultValue(true)]
        [Display(Order = 12)]
        public bool Allow_Coeurl { get; set; } = true;

        [DisplayName("Allow Vampire Cat")]
        [Description("Whether to allow new Vampire Cat eggs.")]
        [DefaultValue(true)]
        [Display(Order = 13)]
        public bool Allow_Vampire_Cat { get; set; } = true;

        [DisplayName("Allow Piscodaemon")]
        [Description("Whether to allow new Piscodaemon eggs.")]
        [DefaultValue(true)]
        [Display(Order = 14)]
        public bool Allow_Piscodaemon { get; set; } = true;

        [DisplayName("Allow Squidraken")]
        [Description("Whether to allow new Squidraken eggs.")]
        [DefaultValue(true)]
        [Display(Order = 15)]
        public bool Allow_Squidraken { get; set; } = true;

        [DisplayName("Allow Mindflayer")]
        [Description("Whether to allow new Mindflayer eggs.")]
        [DefaultValue(true)]
        [Display(Order = 16)]
        public bool Allow_Mindflayer { get; set; } = true;

        [DisplayName("Allow Skeleton")]
        [Description("Whether to allow new Skeleton eggs.")]
        [DefaultValue(true)]
        [Display(Order = 17)]
        public bool Allow_Skeleton { get; set; } = true;

        [DisplayName("Allow Bonesnatch")]
        [Description("Whether to allow new Bonesnatch eggs.")]
        [DefaultValue(true)]
        [Display(Order = 18)]
        public bool Allow_Bonesnatch { get; set; } = true;

        [DisplayName("Allow Skeletal Fiend")]
        [Description("Whether to allow new Skeletal Fiend eggs.")]
        [DefaultValue(true)]
        [Display(Order = 19)]
        public bool Allow_Skeletal_Fiend { get; set; } = true;

        [DisplayName("Allow Ghoul")]
        [Description("Whether to allow new Ghoul eggs.")]
        [DefaultValue(true)]
        [Display(Order = 20)]
        public bool Allow_Ghoul { get; set; } = true;

        [DisplayName("Allow Ghast")]
        [Description("Whether to allow new Ghast eggs.")]
        [DefaultValue(true)]
        [Display(Order = 21)]
        public bool Allow_Ghast { get; set; } = true;

        [DisplayName("Allow Revenant")]
        [Description("Whether to allow new Revenant eggs.")]
        [DefaultValue(true)]
        [Display(Order = 22)]
        public bool Allow_Revenant { get; set; } = true;

        [DisplayName("Allow Floating Eye")]
        [Description("Whether to allow new Floating Eye eggs.")]
        [DefaultValue(true)]
        [Display(Order = 23)]
        public bool Allow_Floating_Eye { get; set; } = true;

        [DisplayName("Allow Ahriman")]
        [Description("Whether to allow new Ahriman eggs.")]
        [DefaultValue(true)]
        [Display(Order = 24)]
        public bool Allow_Ahriman { get; set; } = true;

        [DisplayName("Allow Plague Horror")]
        [Description("Whether to allow new Plague Horror eggs.")]
        [DefaultValue(true)]
        [Display(Order = 25)]
        public bool Allow_Plague_Horror { get; set; } = true;

        [DisplayName("Allow Jura Aevis")]
        [Description("Whether to allow new Jura Aevis eggs.")]
        [DefaultValue(true)]
        [Display(Order = 26)]
        public bool Allow_Jura_Aevis { get; set; } = true;

        [DisplayName("Allow Steelhawk")]
        [Description("Whether to allow new Steelhawk eggs.")]
        [DefaultValue(true)]
        [Display(Order = 27)]
        public bool Allow_Steelhawk { get; set; } = true;

        [DisplayName("Allow Cockatrice")]
        [Description("Whether to allow new Cockatrice eggs.")]
        [DefaultValue(true)]
        [Display(Order = 28)]
        public bool Allow_Cockatrice { get; set; } = true;

        [DisplayName("Allow Pig")]
        [Description("Whether to allow new Pig eggs.")]
        [DefaultValue(true)]
        [Display(Order = 29)]
        public bool Allow_Pig { get; set; } = true;

        [DisplayName("Allow Swine")]
        [Description("Whether to allow new Swine eggs.")]
        [DefaultValue(true)]
        [Display(Order = 30)]
        public bool Allow_Swine { get; set; } = true;

        [DisplayName("Allow Wild Boar")]
        [Description("Whether to allow new Wild Boar eggs.")]
        [DefaultValue(true)]
        [Display(Order = 31)]
        public bool Allow_Wild_Boar { get; set; } = true;

        [DisplayName("Allow Dryad")]
        [Description("Whether to allow new Dryad eggs.")]
        [DefaultValue(true)]
        [Display(Order = 32)]
        public bool Allow_Dryad { get; set; } = true;

        [DisplayName("Allow Treant")]
        [Description("Whether to allow new Treant eggs.")]
        [DefaultValue(true)]
        [Display(Order = 33)]
        public bool Allow_Treant { get; set; } = true;

        [DisplayName("Allow Elder Treant")]
        [Description("Whether to allow new Elder Treant eggs.")]
        [DefaultValue(true)]
        [Display(Order = 34)]
        public bool Allow_Elder_Treant { get; set; } = true;

        [DisplayName("Allow Wisenkin")]
        [Description("Whether to allow new Wisenkin eggs.")]
        [DefaultValue(true)]
        [Display(Order = 35)]
        public bool Allow_Wisenkin { get; set; } = true;

        [DisplayName("Allow Minotaur")]
        [Description("Whether to allow new Minotaur eggs.")]
        [DefaultValue(true)]
        [Display(Order = 36)]
        public bool Allow_Minotaur { get; set; } = true;

        [DisplayName("Allow Sekhret")]
        [Description("Whether to allow new Sekhret eggs.")]
        [DefaultValue(true)]
        [Display(Order = 37)]
        public bool Allow_Sekhret { get; set; } = true;

        [DisplayName("Allow Malboro")]
        [Description("Whether to allow new Malboro eggs.")]
        [DefaultValue(true)]
        [Display(Order = 38)]
        public bool Allow_Malboro { get; set; } = true;

        [DisplayName("Allow Ochu")]
        [Description("Whether to allow new Ochu eggs.")]
        [DefaultValue(true)]
        [Display(Order = 39)]
        public bool Allow_Ochu { get; set; } = true;

        [DisplayName("Allow Greater Malboro")]
        [Description("Whether to allow new Greater Malboro eggs.")]
        [DefaultValue(true)]
        [Display(Order = 40)]
        public bool Allow_Greater_Malboro { get; set; } = true;

        [DisplayName("Allow Behemoth")]
        [Description("Whether to allow new Behemoth eggs.")]
        [DefaultValue(true)]
        [Display(Order = 41)]
        public bool Allow_Behemoth { get; set; } = true;

        [DisplayName("Allow Behemoth King")]
        [Description("Whether to allow new Behemoth King eggs.")]
        [DefaultValue(true)]
        [Display(Order = 42)]
        public bool Allow_Behemoth_King { get; set; } = true;

        [DisplayName("Allow Dark Behemoth")]
        [Description("Whether to allow new Dark Behemoth eggs.")]
        [DefaultValue(true)]
        [Display(Order = 43)]
        public bool Allow_Dark_Behemoth { get; set; } = true;

        [DisplayName("Allow Dragon")]
        [Description("Whether to allow new Dragon eggs.")]
        [DefaultValue(true)]
        [Display(Order = 44)]
        public bool Allow_Dragon { get; set; } = true;

        [DisplayName("Allow Blue Dragon")]
        [Description("Whether to allow new Blue Dragon eggs.")]
        [DefaultValue(true)]
        [Display(Order = 45)]
        public bool Allow_Blue_Dragon { get; set; } = true;

        [DisplayName("Allow Red Dragon")]
        [Description("Whether to allow new Red Dragon eggs.")]
        [DefaultValue(true)]
        [Display(Order = 46)]
        public bool Allow_Red_Dragon { get; set; } = true;

        [DisplayName("Allow Hydra")]
        [Description("Whether to allow new Hydra eggs.")]
        [DefaultValue(true)]
        [Display(Order = 47)]
        public bool Allow_Hydra { get; set; } = true;

        [DisplayName("Allow Greater Hydra")]
        [Description("Whether to allow new Greater Hydra eggs.")]
        [DefaultValue(true)]
        [Display(Order = 48)]
        public bool Allow_Greater_Hydra { get; set; } = true;

        [DisplayName("Allow Tiamat")]
        [Description("Whether to allow new Tiamat eggs.")]
        [DefaultValue(true)]
        [Display(Order = 49)]
        public bool Allow_Tiamat { get; set; } = true;

    }

    public class ConfiguratorMixin : ConfiguratorMixinBase
    {

    }
}
