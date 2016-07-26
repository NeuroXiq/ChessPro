using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    class UCIOptionParser
    {
        public UCIOptionParser()
        {

        }

        public UCIOption ParseOption(string rawOutputLine)
        {
            string optionType;
            int typeStartIndex = rawOutputLine.IndexOf("type") + 5;
            int typeEndIndex = rawOutputLine.IndexOf(' ', typeStartIndex);

            //type can be last parameter and 'type' is end of string.
            typeEndIndex = typeEndIndex == -1 ? rawOutputLine.Length - 1 : typeEndIndex;

            optionType = rawOutputLine.Substring(typeStartIndex, typeEndIndex - typeStartIndex + 1).Trim();

            UCIOption parsedOption;

            switch (optionType)
            {
                case "check":
                    parsedOption = ParseCheckOption(rawOutputLine);
                    break;
                case "spin":
                    parsedOption = ParseSpinOption(rawOutputLine);
                    break;
                case "combo":
                    parsedOption = ParseComboOption(rawOutputLine);
                    break;
                case "button":
                    parsedOption = ParseButtonOption(rawOutputLine);
                    break;
                case "string":
                    parsedOption = ParseStringOption(rawOutputLine);
                    break;
                default:
                    throw new Exception("Option type is unrecognizable");
            }

            return parsedOption;
        }

        private UCIOption ParseStringOption(string rawOutputLine)
        {
            string[] lineElements = rawOutputLine.Split(' ');
            string name = GetOptionName(rawOutputLine);
            string defaultValue = string.Empty;

            for (int i = 1; i < lineElements.Length; i++)
            {
               if (lineElements[i] == "default")
                    defaultValue = lineElements[i + 1];
            }

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("option name is empty.");

            UCIStringOption stringOption = new UCIStringOption(name, defaultValue, defaultValue);
            return stringOption;

        }

        private UCIOption ParseButtonOption(string rawOutputLine)
        {
            string[] lineElements = rawOutputLine.Split(' ');

            string name = GetOptionName(rawOutputLine);

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("name is empty");

            return new UCIButtonOption(name);
        }

        private UCIOption ParseComboOption(string rawOutputLine)
        {
            string[] lineElements = rawOutputLine.Split(' ');
            List<string> comboItems = new List<string>();

            string name = GetOptionName(rawOutputLine);

            string defaultValue = string.Empty;

            for (int i = 0; i < lineElements.Length; i++)
            {
                if (lineElements[i] == "default")
                    defaultValue = lineElements[i + 1];
                else if (lineElements[i] == "var")
                {
                    comboItems.Add(lineElements[i + 1]);
                    i++;
                }
            }

            UCIComboOption comboOption = new UCIComboOption(name, defaultValue, comboItems.ToArray(), defaultValue);
            return comboOption;
        }

        private UCIOption ParseSpinOption(string rawOutputLine)
        {
            string[] lineElements = rawOutputLine.Split(' ');

            string name = GetOptionName(rawOutputLine);

            string min=string.Empty;
            string max = string.Empty;
            string defaultValue = string.Empty;

            for (int i = 0; i < lineElements.Length; i++)
            {
                if (lineElements[i] == "min")
                    min = lineElements[i + 1];
                else if (lineElements[i] == "max")
                    max = lineElements[i + 1];
                else if (lineElements[i] == "default")
                    defaultValue = lineElements[i + 1];
            }

            int minV = int.Parse(min);
            int maxV = int.Parse(max);
            int defaultV = int.Parse(defaultValue);

            UCISpinOption spinOption = new UCISpinOption(name, minV, maxV, defaultV, defaultV);
            return spinOption;
        }

        private UCIOption ParseCheckOption(string rawOutputLine)
        {
            string[] lineElements = rawOutputLine.Split(' ');
            string name = GetOptionName(rawOutputLine);
            string defaultValue = string.Empty;

            for (int i = 0; i < lineElements.Length; i++)
            {
                if (lineElements[i] == "default")
                    defaultValue = lineElements[i + 1];
            }
            bool defaultV = bool.Parse(defaultValue);

            UCICheckOption checkOption = new UCICheckOption(name, defaultV, defaultV);
            return checkOption;
        }

        private string GetOptionName(string rawOutputLine)
        {
            int nameStart = rawOutputLine.IndexOf("name") + 5;
            int nameEnd = rawOutputLine.IndexOf("type");
            string name = rawOutputLine.Substring(nameStart, nameEnd - nameStart - 1);
            name = name.Trim();
            return name;
        }
    }
}
