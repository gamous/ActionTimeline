using ActionTimeline.Helpers;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using System.Numerics;

namespace ActionTimeline.Windows
{
    public class TimelineSettingsWindow : Window
    {
        private float _scale => ImGuiHelpers.GlobalScale;
        private Settings Settings => Plugin.Settings;

        public TimelineSettingsWindow(string name) : base(name)
        {
            Flags = ImGuiWindowFlags.NoScrollbar
                | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoResize
                | ImGuiWindowFlags.NoScrollWithMouse;

            Size = new Vector2(300, 350);
        }

        public override void Draw()
        {
            if (!ImGui.BeginTabBar("##Timeline_Settings_TabBar"))
            {
                return;
            }

            ImGui.PushItemWidth(80 * _scale);

            // general
            if (ImGui.BeginTabItem("通用##Timeline_General"))
            {
                DrawGeneralTab();
                ImGui.EndTabItem();
            }

            // icons
            if (ImGui.BeginTabItem("图标##Timeline_Icons"))
            {
                DrawIconsTab();
                ImGui.EndTabItem();
            }

            // casts
            if (ImGui.BeginTabItem("咏唱##Timeline_Casts"))
            {
                DrawCastsTab();
                ImGui.EndTabItem();
            }

            // grid
            if (ImGui.BeginTabItem("条格##Timeline_Grid"))
            {
                DrawGridTab();
                ImGui.EndTabItem();
            }

            // gcd clipping
            if (ImGui.BeginTabItem("GCD空转##Timeline_GCD"))
            {
                DrawGCDClippingTab();
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }

        public void DrawGeneralTab()
        {
            ImGui.Checkbox("启用", ref Settings.ShowTimeline);

            if (!Settings.ShowTimeline) { return; }

            ImGui.DragInt("总时间 (秒)", ref Settings.TimelineTime, 0.1f, 1, 30);
            DrawHelper.SetTooltip("技能时间轴展示的时间总长度。");

            ImGui.NewLine();
            ImGui.Checkbox("锁定窗口", ref Settings.TimelineLocked);
            ImGui.ColorEdit4("锁定时的背景颜色", ref Settings.TimelineLockedBackgroundColor, ImGuiColorEditFlags.NoInputs);
            ImGui.ColorEdit4("解锁时的背景颜色", ref Settings.TimelineUnlockedBackgroundColor, ImGuiColorEditFlags.NoInputs);

            ImGui.NewLine();
            ImGui.DragInt("战斗结束清除记录时长 (秒)", ref Settings.OutOfCombatClearTime, 0.1f, 1, 30);
            DrawHelper.SetTooltip("在战斗结束后多少时间后会清除技能时间轴上的技能记录。(译者按：建议两倍于总时间)");

            ImGui.Checkbox("仅在任务中显示", ref Settings.ShowTimelineOnlyInDuty);
            ImGui.Checkbox("仅在战斗中显示", ref Settings.ShowTimelineOnlyInCombat);
         }

        public void DrawIconsTab()
        {
            ImGui.DragInt("GCD图标大小", ref Settings.TimelineIconSize);

            ImGui.NewLine();
            ImGui.DragInt("能力技图标大小", ref Settings.TimelineOffGCDIconSize);
            ImGui.DragInt("能力技图标竖直方向偏移", ref Settings.TimelineOffGCDOffset);

            ImGui.NewLine();
            ImGui.Checkbox("显示自动攻击图标", ref Settings.TimelineShowAutoAttacks);
            ImGui.DragInt("自动攻击图标大小", ref Settings.TimelineAutoAttackSize);
            ImGui.DragInt("自动攻击图标竖直方向偏移", ref Settings.TimelineAutoAttackOffset);
        }

        public void DrawCastsTab()
        {
            ImGui.ColorEdit4("咏唱时的颜色", ref Settings.CastInProgressColor, ImGuiColorEditFlags.NoInputs);
            ImGui.ColorEdit4("咏唱完成的颜色", ref Settings.CastFinishedColor, ImGuiColorEditFlags.NoInputs);
            ImGui.ColorEdit4("咏唱取消的颜色", ref Settings.CastCanceledColor, ImGuiColorEditFlags.NoInputs);
        }

        public void DrawGridTab()
        {
            ImGui.Checkbox("启用", ref Settings.ShowGrid);

            if (!Settings.ShowGrid) { return; }

            ImGui.DragInt("线的宽度", ref Settings.GridLineWidth, 0.5f, 1, 5);
            ImGui.ColorEdit4("线的颜色", ref Settings.GridLineColor, ImGuiColorEditFlags.NoInputs);

            ImGui.Checkbox("显示中心线", ref Settings.ShowGridCenterLine);

            ImGui.Checkbox("显示按秒分割线", ref Settings.GridDivideBySeconds);

            if (!Settings.GridDivideBySeconds) { return; }

            ImGui.Checkbox("显示秒文字", ref Settings.GridShowSecondsText);

            ImGui.NewLine();
            ImGui.Checkbox("显示按秒细分线", ref Settings.GridSubdivideSeconds);

            if (!Settings.GridSubdivideSeconds) { return; }

            ImGui.DragInt("细分线间段数", ref Settings.GridSubdivisionCount, 0.5f, 2, 8);
            ImGui.DragInt("细分线的宽度", ref Settings.GridSubdivisionLineWidth, 0.5f, 1, 5);
            ImGui.ColorEdit4("细分线的颜色", ref Settings.GridSubdivisionLineColor, ImGuiColorEditFlags.NoInputs);
        }

        public void DrawGCDClippingTab()
        {
            ImGui.Checkbox("启用", ref Settings.ShowGCDClipping);

            if (!Settings.ShowGCDClipping) { return; }

            int clippingThreshold = (int)(Settings.GCDClippingThreshold * 1000f);
            if (ImGui.DragInt("GCD阈值 (毫秒)", ref clippingThreshold, 0.1f, 0, 1000))
            {
                Settings.GCDClippingThreshold = (float)clippingThreshold / 1000f;
            }
            DrawHelper.SetTooltip("这可以用于过滤掉由于延迟或其他因素导致的“误报”。 检测到的任何比此值短的 GCD 空转都将被忽略。\n强烈建议您测试不同的值以找出最适合您的阈值。");

            int castClippingThresgold = (int)(Settings.GCDClippingCastsThreshold * 1000f);
            if (ImGui.DragInt("咏唱阈值 (毫秒)", ref castClippingThresgold, 0.1f, 0, 1000))
            {
                Settings.GCDClippingCastsThreshold = (float)castClippingThresgold / 1000f;
            }
            DrawHelper.SetTooltip("这可用于在咏唱后过滤掉“误报”，特别是对于长于 GCD 的咏唱。在咏唱后检测到的任何 GCD 空转都将被忽略。\\n强烈建议您测试不同的值以找出最适合您的阈值。(译者按：通常为GCD阈值的基础上加500毫秒)");

            ImGui.DragInt("最大时间 (秒)", ref Settings.GCDClippingMaxTime, 0.1f, 3, 60);
            DrawHelper.SetTooltip("任何超过最大时间的GCD空转都会被截停。");

            ImGui.ColorEdit4("GCD空转颜色", ref Settings.GCDClippingColor, ImGuiColorEditFlags.NoInputs);
        }
    }
}
