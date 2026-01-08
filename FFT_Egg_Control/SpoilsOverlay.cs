using FFT_Egg_Control.Configuration;
using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;
using NenTools.ImGui.Interfaces.Shell.Textures;
using System.Drawing;
using System.Numerics;
using static FFT_Egg_Control.Constants.Constants;

namespace FFT_Egg_Control
{
    //[ImGuiMenu(Category = "Mods", Priority = 0, Owner = "Egg Control")]
    public class SpoilsOverlay : IImGuiComponent
    {
        public readonly Config _configuration;

        public SpoilsOverlay(Config configuration)
        {
            _configuration = configuration;
        }

        public IImGui imGui { get; set; }

        public bool IsOverlay => true;
        public bool WindowOpen = false;
        //public ImGuiWindowFlags WindowFlags = ImGuiWindowFlags.ImGuiWindowFlags_NoBackground | ImGuiWindowFlags.ImGuiWindowFlags_NoInputs | ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize;
        public ImGuiWindowFlags WindowFlags = ImGuiWindowFlags.ImGuiWindowFlags_NoTitleBar | ImGuiWindowFlags.ImGuiWindowFlags_NoResize | ImGuiWindowFlags.ImGuiWindowFlags_NoScrollbar | ImGuiWindowFlags.ImGuiWindowFlags_NoCollapse | ImGuiWindowFlags.ImGuiWindowFlags_NoInputs | ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize;
        //public ImGuiWindowFlags WindowFlags = ImGuiWindowFlags.ImGuiWindowFlags_NoResize | ImGuiWindowFlags.ImGuiWindowFlags_NoScrollbar | ImGuiWindowFlags.ImGuiWindowFlags_NoCollapse | ImGuiWindowFlags.ImGuiWindowFlags_NoBackground | ImGuiWindowFlags.ImGuiWindowFlags_NoInputs | ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize;
        //public ImGuiWindowFlags WindowFlags = ImGuiWindowFlags.ImGuiWindowFlags_NoDecoration | ImGuiWindowFlags.ImGuiWindowFlags_NoBackground | ImGuiWindowFlags.ImGuiWindowFlags_NoInputs | ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize;

        public IImGuiImage _chocoboImage { get; set; }

        public Action PoachAction { get; set; }
        public Action CarcassCountAction { get; set; }
        public Dictionary<MonsterID, Func<bool>> MonsterIDToConfigMap { get; set; }

        public List<string> LastCarcassList { get; set; } = new();
        public List<string> LastPremiumCarcassList { get; set; } = new();
        public List<(string, int)> LastSoldCarcassList { get; set; } = new();
        public int LastOverstockCarcassGil { get; set; }

        private Color NormalCarcassColor = Color.LightGray;
        private Color PremiumCarcassColor = Color.MediumPurple;

        public void RenderMenu(IImGuiShell imGuiShell)
        {
            if (imGui.MenuItem("Spoils Overlay"))
            {
                WindowOpen = true;
            }
        }

        public void Render(IImGuiShell imGuiShell)
        {
            if (WindowOpen)
            {
                //imGui.SetNextWindowSize(new Vector2(300, 600), ImGuiCond.ImGuiCond_Always);
                var size = imGui.GetMainViewport().Size;
                imGui.PushFontFloat(null, 28f);
                var maxTextSize = imGui.CalcTextSize("Overstock Dark Behemoth Carcass+ sold for 100000 gil");
                imGui.SetNextWindowPos(new Vector2(size.X - maxTextSize.X - 10, 10), ImGuiCond.ImGuiCond_Always);
                var result = imGui.Begin("AutoPoach", ref WindowOpen, WindowFlags);
                imGui.PushStyleColorImVec4(ImGuiCol.ImGuiCol_WindowBg, Color.FromArgb(150, 0, 0, 0).ToV4());
                //var result = imGui.Begin("AutoPoach", ref WindowOpen, WindowFlags);
                //imGui.SetCursorPos(new Vector2(10, 10));

                //if (_chocoboImage is null)
                //{
                //    var image = SixLabors.ImageSharp.Image.Load<Rgba32>(@"C:\Users\Thoth\Documents\FFT Stuff\Odin Main.png");
                //    var image2 = SixLabors.ImageSharp.Image.Load<Rgba32>(@"C:\Users\Thoth\Pictures\transparent-circle-with-line-through-it-4.jpg");
                //    //byte[] xByte = (byte[])_imageConverter.ConvertTo(a, typeof(byte[]));
                //    //_chocoboImage = imGuiShell.TextureManager.LoadImage(xByte, (uint)a.Width, (uint)a.Height);
                //    byte[] pixelBytes = new byte[image.Width * image.Height * Unsafe.SizeOf<Rgba32>()];
                //    byte[] pixelBytes2 = new byte[image2.Width * image2.Height * Unsafe.SizeOf<Rgba32>()];
                //    image.CopyPixelDataTo(pixelBytes);
                //    image2.CopyPixelDataTo(pixelBytes2);
                //    _chocoboImage = imGuiShell.TextureManager.LoadImage(pixelBytes, (uint)image.Width, (uint)image.Height);
                //    _chocoboImage = imGuiShell.TextureManager.LoadImage(pixelBytes, (uint)image.Width, (uint)image.Height);
                //}
                //else
                //{
                //    imGuiShell.TextureManager.UpdateImage(_chocoboImage, _normalizedScreenBuffer);
                //}

                //imGui.ImageWithBgEx(imGui.CreateTextureRef(_chocoboImage.TexId), new Vector2(116, 142), Vector2.Zero, Vector2.One, Color.FromArgb(0, 0, 0, 0).ToV4(), Color.FromArgb(255, 50, 255, 255).ToV4());
                //imGui.Image(imGui.CreateTextureRef(_chocoboImage.TexId), new Vector2(116, 142));

                //foreach (MonsterID monster in Constants.Constants.MonsterID.GetValues(typeof(Constants.Constants.MonsterID)))
                //{
                //    imGui.Text($"{monster}: ");
                //    imGui.SameLine();
                //    bool val = MonsterIDToConfigMap?[monster]() ?? false;
                //    imGui.Checkbox("Enabled", ref val);
                //    imGui.SameLine();
                //    imGui.TextColored(Color.Gold.ToV4(), "Rare");
                //}

                if (LastCarcassList.Count > 0 || LastPremiumCarcassList.Count > 0 || LastSoldCarcassList.Count > 0)
                {
                    imGui.PushFontFloat(null, 38f);
                    imGui.TextColored(Color.LawnGreen.ToV4(), "AutoPoach");
                    imGui.PopFont();
                }

                foreach (var carcass in LastCarcassList)
                {
                    imGui.TextColored(NormalCarcassColor.ToV4(), carcass);
                }

                foreach (var carcass in LastPremiumCarcassList)
                {
                    imGui.TextColored(PremiumCarcassColor.ToV4(), carcass);
                }

                foreach ((var carcassName, int value) in LastSoldCarcassList)
                {
                    imGui.Text("Overstock");
                    imGui.SameLine();
                    imGui.TextColored(carcassName.Contains("+") ? PremiumCarcassColor.ToV4() : NormalCarcassColor.ToV4(), carcassName);
                    imGui.SameLine();
                    imGui.Text("sold for");
                    imGui.SameLine();
                    imGui.TextColored(Color.Gold.ToV4(), value.ToString() + " gil");
                }

                if (LastOverstockCarcassGil > 0)
                {
                    imGui.Text($"Overstock Carcass Gil:");
                    imGui.SameLine();
                    imGui.TextColored(Color.Gold.ToV4(), LastOverstockCarcassGil.ToString());
                }

                //for (int i = 0; i < 4; i++)
                //{
                //    imGui.Text($"Item {i}: ");
                //    imGui.SameLine();
                //    imGui.TextColored(Color.LightGray.ToV4(), "Common");
                //    imGui.SameLine();
                //    imGui.TextColored(Color.Gold.ToV4(), "Rare");
                //}                

                imGui.PopFont();
                imGui.PopStyleColor();
                imGui.End();
            }
        }
    }
}
