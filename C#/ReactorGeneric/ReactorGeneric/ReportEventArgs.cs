using System;

namespace ReactorGeneric
{
    public enum ReportType
    {
        Replace,
        Append,
        ReplaceLast
    }

    public class ReportEventArgs : EventArgs
    {
        public string ReportText { get; private set; }
        public ReportType Action { get; private set; }

        public ReportEventArgs (string text, ReportType action)
        {
            ReportText = text;
            Action = action;
        }
    }
}