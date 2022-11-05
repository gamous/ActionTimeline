using ActionTimeline.Helpers;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using System.Numerics;

namespace ActionTimeline.Windows
{
    public class RotationSettingsWindow : Window
    {
        private float _scale => ImGuiHelpers.GlobalScale;
        private Settings Settings => Plugin.Settings;

        public RotationSettingsWindow(string name) : base(name)
        {
            Flags = ImGuiWindowFlags.NoScrollbar
                | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoResize
                | ImGuiWindowFlags.NoScrollWithMouse;

            Size = new Vector2(300, 350);
        }

        public override void Draw()
        {
            if (!ImGui.BeginTabBar("##Rotation_Settings_TabBar"))
            {
                return;
            }

            ImGui.PushItemWidth(80 * _scale);

            // general
            if (ImGui.BeginTabItem("通用##Rotation_General"))
            {
                DrawGeneralTab();
                ImGui.EndTabItem();
            }

            // icons
            if (ImGui.BeginTabItem("图标##Rotation_Icons"))
            {
                DrawIconsTab();
                ImGui.EndTabItem();
            }

            // separator
            if (ImGui.BeginTabItem("分割线##Rotation_Separator"))
            {
                DrawSeparatorTab();
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }

        public void DrawGeneralTab()
        {
            ImGui.Checkbox("启用", ref Settings.ShowRotation);

            if (!Settings.ShowRotation) { return; }

            ImGui.DragInt("GCD前间隔距离", ref Settings.RotationGCDSpacing);
            ImGui.DragInt("能力技前间隔距离", ref Settings.RotationOffGCDSpacing);

            ImGui.NewLine();
            ImGui.Checkbox("锁定窗口", ref Settings.RotationLocked);
            ImGui.ColorEdit4("锁定时的背景颜色", ref Settings.RotationLockedBackgroundColor, ImGuiColorEditFlags.NoInputs);
            ImGui.ColorEdit4("解锁时的背景颜色", ref Settings.RotationUnlockedBackgroundColor, ImGuiColorEditFlags.NoInputs);

            ImGui.NewLine();
            ImGui.DragInt("战斗结束清除记录时长 (秒)", ref Settings.OutOfCombatClearTime, 0.1f, 1, 30);
            DrawHelper.SetTooltip("战斗结束后多少时间后会清除技能队列的技能记录。");

            ImGui.Checkbox("仅在任务中显示", ref Settings.ShowRotationOnlyInDuty);
            ImGui.Checkbox("仅在战斗中显示", ref Settings.ShowRotationOnlyInCombat);
        }

        public void DrawIconsTab()
        {
            ImGui.DragInt("GCD图标大小", ref Settings.RotationIconSize);

            ImGui.NewLine();
            ImGui.DragInt("能力技图标大小", ref Settings.RotationOffGCDIconSize);
            ImGui.DragInt("能力技图标竖直方向偏移", ref Settings.RotationOffGCDOffset);
        }

        public void DrawSeparatorTab()
        {
            ImGui.Checkbox("启用", ref Settings.RotationSeparatorEnabled);
            DrawHelper.SetTooltip("如果两个技能之间的释放时间足够长，那么绘制分割线");

            if (!Settings.RotationSeparatorEnabled) { return; }

            ImGui.DragInt("技能时间间隔 (秒)", ref Settings.RotationSeparatorTime, 0.5f, 5, 60);
            ImGui.DragInt("分割线的宽度", ref Settings.RotationSeparatorWidth, 0.5f, 1, 10);
            ImGui.ColorEdit4("分割线的颜色", ref Settings.RotationSeparatorColor, ImGuiColorEditFlags.NoInputs);
        }
    }
}
