using ActionTimeline.Helpers;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using System.Numerics;

namespace ActionTimeline.Windows
{
    public class SettingsWindow : Window
    {
        private float _scale => ImGuiHelpers.GlobalScale;
        private Settings Settings => Plugin.Settings;

        public SettingsWindow(string name) : base(name)
        {
            Flags = ImGuiWindowFlags.NoScrollbar
                | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoResize
                | ImGuiWindowFlags.NoScrollWithMouse;

            Size = new Vector2(180, 84);
        }

        public override void Draw()
        {
            if (ImGui.Button("配置技能时间轴窗口"))
            {
                Plugin.ShowTimelineSettingsWindow();
            }

            if (ImGui.Button("配置技能队列窗口"))
            {
                Plugin.ShowRotationSettingsWindow();
            }
        }
    }
}
