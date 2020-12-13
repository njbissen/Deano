using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcumenRegistry.Models.Prototypes.Shared
{
    public class JsonMessage
    {
        public JsonMessage()
        {
            Type = JsonMessageType.Info;
            Text = string.Empty;
            SetDisplay();
        }

        public JsonMessage(string text)
        {
            Text = text;
            Type = JsonMessageType.Info;
            SetDisplay();
        }

        public JsonMessage(string text, JsonMessageType type)
        {
            Type = type;
            Text = text;
            SetDisplay();
        }

        private void SetDisplay()
        {
            if (this.Type == JsonMessageType.Info)
            {
                this.Display = true;
            }
            else
            {
                this.Display = true;
            }
        }

        public bool Display { get; set; }

        public JsonMessageType Type { get; set; }

        public string Text { get; set; }
    }

    public enum JsonMessageType
    {
        Info = 0,
        Debug = 2,
        Warning = 3,
        Error = 4
    }
}
