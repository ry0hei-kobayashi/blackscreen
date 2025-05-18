// Blackout.exe : when you clicked exe, blackout all monitors. and when you double-clicked, close all blackouts.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

internal static class Program
{
    // ──────────────── Win32 API ────────────────
    [DllImport("user32.dll")]
    private static extern bool EnumDisplayMonitors(
        IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

    private delegate bool MonitorEnumProc(
        IntPtr hMonitor, IntPtr hdc, ref RECT rcMonitor, IntPtr dwData);

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT { public int Left, Top, Right, Bottom; }

    // ──────────────── Entry ────────────────
    [STAThread]
    private static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var covers = new List<Form>();

        // black screen at ea monitor
        EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
            (IntPtr hMon, IntPtr hdc, ref RECT r, IntPtr data) =>
            {
                var f = new Form
                {
                    FormBorderStyle = FormBorderStyle.None,
                    StartPosition = FormStartPosition.Manual,
                    BackColor = Color.Black,
                    ShowInTaskbar = false,
                    Bounds = new Rectangle(
                        r.Left, r.Top, r.Right - r.Left, r.Bottom - r.Top),
                    TopMost = true
                };

                // hide cursor
                f.Shown += static (_, __) => Cursor.Hide();

                // when double-clicked, close all covers
                f.MouseDown += (object? sender, MouseEventArgs e) =>
                {
                    if (e.Clicks >= 1 //you can change this to 2 for double-click
                    ) CloseAll(covers);
                };

                covers.Add(f);
                return true;
            }, IntPtr.Zero);

        foreach (var f in covers) f.Show();
        Application.Run();
    }

    private static void CloseAll(IEnumerable<Form> covers)
    {
        Cursor.Show(); //show cursor
        foreach (var f in covers)
            if (!f.IsDisposed) f.Close();
        Application.ExitThread();
    }
}
