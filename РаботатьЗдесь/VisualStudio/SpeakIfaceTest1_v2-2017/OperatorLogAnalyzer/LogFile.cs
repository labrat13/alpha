using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OperatorLogAnalyzer
{
    public class LogFile
    {
        private StreamReader m_reader;
        private StreamWriter m_writer;
        private Encoding m_encoding;

        public LogFile()
        {
            this.m_reader = (StreamReader)null;
            this.m_writer = (StreamWriter)null;
            this.m_encoding = Encoding.GetEncoding(1251);
        }

        public bool Open(string filepath, bool forWriting)
        {
            bool flag = true;
            if (this.m_reader != null || this.m_writer != null)
                return false;
            try
            {
                if (!forWriting)
                    this.m_reader = new StreamReader(filepath, this.m_encoding);
                else
                    this.m_writer = new StreamWriter(filepath, true, this.m_encoding);
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }

        public bool Close()
        {
            bool flag = true;
            try
            {
                if (this.m_reader != null)
                {
                    this.m_reader.Close();
                    this.m_reader = (StreamReader)null;
                }
                if (this.m_writer != null)
                {
                    this.m_writer.Close();
                    this.m_writer = (StreamWriter)null;
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }

        public Dictionary<string, int> countCommand()
        {
            Dictionary<string, int> dictionary1 = new Dictionary<string, int>();
            string str1 = "QUERY";
            int length = str1.Length;
            while (!this.m_reader.EndOfStream)
            {
                string str2 = this.m_reader.ReadLine();
                if (str2.StartsWith(str1))
                {
                    string key = str2.Substring(length).Trim();
                    if (dictionary1.ContainsKey(key))
                    {
                        Dictionary<string, int> dictionary2;
                        string index;
                        (dictionary2 = dictionary1)[index = key] = dictionary2[index] + 1;
                    }
                    else
                        dictionary1.Add(key, 1);
                }
            }
            return dictionary1;
        }

        public static Dictionary<string, int> CommandCount(string filepath)
        {
            LogFile logFile = new LogFile();
            if (!logFile.Open(filepath, false))
                return (Dictionary<string, int>)null;
            Dictionary<string, int> dictionary = logFile.countCommand();
            if (!logFile.Close())
                return (Dictionary<string, int>)null;
            return dictionary;
        }
    }
}