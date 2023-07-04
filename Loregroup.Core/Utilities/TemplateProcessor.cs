using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using Loregroup.Core;

namespace Loregroup.Core.Utilities
{
    /// <summary>
    /// Summary description for TemplateProcessor.
    /// </summary>
    public class CompileException : Exception
    {
        private int Line;
        private string Error;
        public CompileException(int line, string error)
        {
            Line = line;
            Error = error;
        }
    }

    public class TemplateProcessor 
    {
        private enum CodeToken
        {
            Text,
            Variable,
            If,
            ElseIf,
            Else,
            EndIf,
            StartLoop,
            EndLoop,
            Include,
            IncludeDefault,
        };

        private class CodeBlock
        {
            public CodeToken Token;
            public int Start;
            public int Size;
            public string Field;
            public string Extra;
            public int Offset;
        }

        private enum ContextType
        {
            Init,
            If,
            Loop
        }

        private struct Context
        {
            public int Index;
            public ContextType Type;
        }

        private const int MaxLevels = 10;
        private string Text;
        private List<CodeBlock> Commands;
        private StringDictionary Variables;
        private StringDictionary IncludeFiles;
        private string IncludePath;

        public TemplateProcessor()
        {
            Commands = new List<CodeBlock>();
            Variables = new StringDictionary();
            IncludeFiles = new StringDictionary();
        }

        public bool LoadText(string text)
        {
            try
            {
                Text = text;
            }
            catch (Exception)
            {
                return false;
            }
            PreProcess();
            return true;
        }

        public bool LoadFile(string filename)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    Text = sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return false;
            }
            PreProcess();
            return true;
        }

        public bool LoadStream(Stream stream)
        {
            try
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    Text = sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return false;
            }
            PreProcess();
            return true;
        }

        public string Execute()
        {
            StringWriter writer = new StringWriter();
            Execute(writer);
            return writer.ToString();
        }

        public void Execute(TextWriter writer)
        {
            int level = -1;
            bool[] context = new bool[MaxLevels];
            int i = 0;
            while (i < Commands.Count)
            {
                int jump = -1;
                CodeBlock block = (CodeBlock)Commands[i];
                switch (block.Token)
                {
                    case CodeToken.Text:
                        writer.Write(Text.Substring(block.Start, block.Size));
                        break;
                    case CodeToken.Variable:
                        {
                            string var;
                            if (!GetVariable(out var, block.Field))
                            {
                                var = '@' + block.Field + '@';
                            }
                            writer.Write(var);
                            break;
                        }
                    case CodeToken.If:
                        level++;
                        if (!Evaluation(block.Field))
                        {
                            jump = block.Offset;
                            context[level] = false;
                        }
                        else
                        {
                            context[level] = true;
                        }
                        break;
                    case CodeToken.ElseIf:
                        if (context[level] || !Evaluation(block.Field))
                        {
                            jump = block.Offset;
                        }
                        else
                        {
                            context[level] = true;
                        }
                        break;
                    case CodeToken.Else:
                        if (context[level])
                        {
                            jump = block.Offset;
                        }
                        break;
                    case CodeToken.EndIf:
                        level--;
                        break;
                    case CodeToken.StartLoop:
                        LoopInit();
                        jump = block.Offset;
                        break;
                    case CodeToken.EndLoop:
                        if (LoopFunc())
                        {
                            jump = block.Offset + 1;
                        }
                        break;
                    case CodeToken.Include:
                        {
                            string includefile = (string)IncludeFiles[block.Field];
                            if (includefile != null)
                            {
                                writer.Write(includefile);
                            }
                            break;
                        }
                    case CodeToken.IncludeDefault:
                        {
                            string includefilename;
                            includefilename = block.Extra;
                            int slash = includefilename.LastIndexOf('\\');
                            int dot = includefilename.LastIndexOf('.');
                            if (slash != -1 && dot != -1)
                            {
                                includefilename = includefilename.Substring(0, slash + 1) + Value(block.Field) + includefilename.Substring(dot);
                            }
                            string includefile;
                            if (ALoadFile(out includefile, includefilename) || (includefile = (string)IncludeFiles[block.Extra]) != null)
                            {
                                writer.Write(includefile);
                            }
                            break;
                        }
                }
                if (jump != -1)
                {
                    i = jump;
                }
                else
                {
                    i++;
                }
            }
        }

        //variable input/output

        public void SetVariable(string name, string value)
        {
            Variables[name] = value;
        }

        public void SetVariable(string name, int value)
        {
            Variables[name] = value.ToString();
        }

        public void SetVariable(string name, DateTime value)
        {
            Variables[name] = value.ToString("D", DateTimeFormatInfo.InvariantInfo);
        }

        //looping 

        public virtual void LoopInit()
        {
        }

        public virtual bool LoopFunc()
        {
            return false;
        }

        //command block primatives

        private int AddText(int start, int stop)
        {
            CodeBlock cb = new CodeBlock();
            cb.Token = CodeToken.Text;
            cb.Start = start;
            cb.Size = (stop - start) + 1;
            Commands.Add(cb);
            return Commands.Count - 1;
        }

        int AddVariable(string variable)
        {
            CodeBlock cb = new CodeBlock();
            cb.Token = CodeToken.Variable;
            cb.Field = variable;
            Commands.Add(cb);
            return Commands.Count - 1;
        }

        private int AddIf(string expression)
        {
            CodeBlock cb = new CodeBlock();
            cb.Token = CodeToken.If;
            cb.Field = expression;
            Commands.Add(cb);
            return Commands.Count - 1;
        }

        private int AddElseIf(string expression)
        {
            CodeBlock cb = new CodeBlock();
            cb.Token = CodeToken.ElseIf;
            cb.Field = expression;
            Commands.Add(cb);
            return Commands.Count - 1;
        }

        private int AddElse()
        {
            CodeBlock cb = new CodeBlock();
            cb.Token = CodeToken.Else;
            Commands.Add(cb);
            return Commands.Count - 1;
        }

        private int AddEndIf()
        {
            CodeBlock cb = new CodeBlock();
            cb.Token = CodeToken.EndIf;
            Commands.Add(cb);
            return Commands.Count - 1;
        }

        private int AddLoopStart()
        {
            CodeBlock cb = new CodeBlock();
            cb.Token = CodeToken.StartLoop;
            cb.Offset = -1;
            Commands.Add(cb);
            return Commands.Count - 1;
        }

        private int AddLoopStop(int start)
        {
            CodeBlock cb = new CodeBlock();
            cb.Token = CodeToken.EndLoop;
            cb.Offset = start;
            Commands.Add(cb);
            return Commands.Count - 1;
        }

        private bool ALoadFile(out string s, string filename)
        {
            string fullfilename = BuildPath(filename);
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    s = sr.ReadToEnd();
                }
                return true;
            }
            catch (Exception)
            {
                s = null;
                return false;
            }
        }

        public void SetIncludePath(string includepath)
        {
            if (!IncludePath.EndsWith("\\"))
            {
                IncludePath = includepath + '\\';
            }
            else
            {
                IncludePath = includepath;
            }
        }

        private string BuildPath(string filename)
        {
            if (filename.Length > 0 && filename[0] == '\\')
            {
                return IncludePath + filename.Substring(1);
            }
            else
            {
                return IncludePath + filename;
            }
        }

        private int AddInclude(string filename)
        {
            string result = (string)IncludeFiles[filename];
            if (result == null)
            {
                string includefile;
                ALoadFile(out includefile, filename);
                IncludeFiles[filename] = includefile;
            }
            CodeBlock cb = new CodeBlock();
            cb.Token = CodeToken.Include;
            cb.Field = filename;
            Commands.Add(cb);
            return Commands.Count - 1;
        }

        private int AddIncludeDefault(string variable, string filename)
        {
            string result = (string)IncludeFiles[filename];
            if (result == null)
            {
                string includefile;
                ALoadFile(out includefile, filename);
                IncludeFiles[filename] = includefile;
            }
            CodeBlock cb = new CodeBlock();
            cb.Token = CodeToken.IncludeDefault;
            cb.Field = variable;
            cb.Extra = filename;
            Commands.Add(cb);
            return Commands.Count - 1;
        }

        //Variable evaluation

        private bool GetVariable(out string result, string name)
        {
            result = (string)Variables[name];
            return result != null;
        }

        private string Value(string s)
        {
            string result;
            s.Trim();
            int len = s.Length;
            if (len >= 1 && s[0] == '"')
            {
                if (s[len - 1] == '"')
                {
                    result = s.Substring(1, len - 2);
                }
                else
                {
                    result = s.Substring(1);
                }
            }
            else
            {
                s.ToLower();
                GetVariable(out result, s);
            }
            return result;
        }

        private bool Evaluation(string s)
        {
            int e = s.IndexOf('=');
            if (e != -1)
            {
                bool ne = e > 0 && s[e - 1] == '!';
                string left = s.Substring(0, ne ? e - 1 : e).ToLower();
                string right = s.Substring(e + 1).ToLower();
                if (ne)
                {
                    return Value(left) != Value(right);
                }
                else
                {
                    return Value(left) == Value(right);
                }
            }
            else
            {
                return Value(s) != "";
            }
        }

        //file processing

        private void PreProcessHelper(int start, int stop, ref int lasttoken)
        {
            if (lasttoken == -1)
            {
                if (start > 0)
                {
                    AddText(0, start - 1);
                }
            }
            else
            {
                if (start > lasttoken)
                {
                    AddText(lasttoken, start - 1);
                }
            }
            lasttoken = stop + 1;
        }

        private void PreProcess()
        {
            Context[] context = new Context[MaxLevels];
            int level = -1;

            int start = -1;
            int stop = -1;
            int lasttoken = -1;

            int line = 1;
#if DEBUG
            int col = 0;
#endif

            int i = 0;
            while (i < Text.Length)
            {
                switch (Text[i])
                {
                    case '\n':
                    case '\r':
                        if (Text[i] == '\n')
                        {
                            line++;
                        }
#if DEBUG
                        col = 1;
#endif
                        start = -1; //stop processing @ if we are
                        stop = -1;
                        break;
                    case '@':
                        if (start == -1)
                        {
                            start = i;
                        }
                        else
                        {
                            stop = i;
                            if (stop - start > 1)
                            {
                                string s = Text.Substring(start + 1, stop - start - 1);
                                if (s.StartsWith("include "))
                                {
                                    PreProcessHelper(start, stop, ref lasttoken);
                                    AddInclude(s.Substring(8));
                                }
                                else if (s.StartsWith("includedef "))
                                {
                                    PreProcessHelper(start, stop, ref lasttoken);
                                    string param = s.Substring(11);
                                    int comma = param.IndexOf(',');
                                    if (comma == -1)
                                    {
                                        throw new CompileException(line, "includedef expectsts two parameters");
                                    }
                                    AddIncludeDefault(param.Substring(0, comma), param.Substring(comma + 1));
                                }
                                else if (s.StartsWith("if "))
                                {
                                    PreProcessHelper(start, stop, ref lasttoken);
                                    level++;
                                    if (level >= MaxLevels)
                                    {
                                        throw new CompileException(line, "Too many levels of nesting");
                                    }
                                    context[level].Type = ContextType.If;
                                    context[level].Index = AddIf(s.Substring(3));
                                }
                                else if (s.StartsWith("elseif "))
                                {
                                    if (level != -1 && context[level].Type == ContextType.If)
                                    {
                                        PreProcessHelper(start, stop, ref lasttoken);
                                        int index = AddElseIf(s.Substring(7));
                                        CodeBlock block = (CodeBlock)Commands[context[level].Index];
                                        block.Offset = index;
                                        context[level].Index = index;
                                    }
                                    else
                                    {
                                        throw new CompileException(line, "if expected");
                                    }
                                }
                                else if (s == "else")
                                {
                                    if (level != -1 && context[level].Type == ContextType.If)
                                    {
                                        PreProcessHelper(start, stop, ref lasttoken);
                                        int index = AddElse();
                                        CodeBlock block = (CodeBlock)Commands[context[level].Index];
                                        block.Offset = index;
                                        context[level].Index = index;
                                    }
                                    else
                                    {
                                        throw new CompileException(line, "if expected");
                                    }
                                }
                                else if (s == "endif")
                                {
                                    if (level != -1 && context[level].Type == ContextType.If)
                                    {
                                        PreProcessHelper(start, stop, ref lasttoken);
                                        int index = AddEndIf();
                                        CodeBlock block = (CodeBlock)Commands[context[level].Index];
                                        block.Offset = index;
                                        level--;
                                    }
                                    else
                                    {
                                        throw new CompileException(line, "if expected");
                                    }
                                }
                                else if (s == "loop")
                                {
                                    PreProcessHelper(start, stop, ref lasttoken);
                                    level++;
                                    if (level >= MaxLevels)
                                    {
                                        throw new CompileException(line, "Too many levels of nesting");
                                    }
                                    context[level].Type = ContextType.Loop;
                                    context[level].Index = AddLoopStart();
                                }
                                else if (s == "endloop")
                                {
                                    if (level == -1 || context[level].Type == ContextType.Loop)
                                    {
                                        PreProcessHelper(start, stop, ref lasttoken);
                                        int index = AddLoopStop(context[level].Index);
                                        CodeBlock block = (CodeBlock)Commands[context[level].Index];
                                        block.Offset = index;
                                        level--;
                                    }
                                    else
                                    {
                                        throw new CompileException(line, "loop expected");
                                    }
                                }
                                else
                                {
                                    PreProcessHelper(start, stop, ref lasttoken);
                                    AddVariable(s);
                                }
                                start = -1;
                                stop = -1;
                            }
                            else
                            {
                                start = i;
                                stop = -1;
                            }
                        }
                        break;
                    default:
#if DEBUG
                        col++;
#endif
                        if (start != -1 && i - start > 22)
                        {
                            //if we are in an @ variable but have no end yet we stop
                            start = -1;
                        }
                        break;
                }
                i++;
            }
            if (i > lasttoken)
            {
                if (lasttoken == -1)
                {
                    AddText(0, i - 1);
                }
                else
                {
                    AddText(lasttoken, i - 1);
                }
            }
            if (level != -1)
            {
                switch (context[level].Type)
                {
                    case ContextType.Loop:
                        throw new CompileException(line, "endloop expected");
                    case ContextType.If:
                        throw new CompileException(line, "endif expected");
                }
            }
        }
    }
}