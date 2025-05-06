using System;
using System.IO;
using System.Text;
using ReverseMarkdown;

namespace Html2Md
{
    class Program
    {
        static void Main(string[] args)
        {
            // 引数なしかつ標準入力がない場合、Usageを表示
            if (args.Length == 0 && !Console.IsInputRedirected)
            {
                ShowUsage();
                return;
            }

            // 標準入力のエンコーディングをUTF-8に設定
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            // 標準入力から文字列を逐次読み込み
            string input = null;
            try
            {
                var builder = new StringBuilder();
                string line;
                while ((line = Console.ReadLine()) != null)
                {
                    if (line.StartsWith("\"") && line.EndsWith("\""))
                    {
                        line = line.Substring(1, line.Length - 2);
                    }
                    builder.Append(line);
                }
                input = builder.ToString().Trim().Trim('"');
            }
            catch (Exception)
            {
                return;
            }

            // 空入力の場合、Usageを表示
            if (string.IsNullOrWhiteSpace(input))
            {
                ShowUsage();
                return;
            }

            // ReverseMarkdownでHTMLをGFMに変換
            var config = new Config
            {
                UnknownTags = Config.UnknownTagsOption.PassThrough,
                GithubFlavored = true
            };
            var converter = new Converter(config);
            string markdown = converter.Convert(input);

            // 標準出力にGFMを出力（UTF-8）
            Console.Write(markdown);
            Console.Out.Flush();
        }

        static void ShowUsage()
        {
            Console.WriteLine(@"Usage: html2md
Reads HTML from standard input and outputs Markdown (GFM) to standard output.
Input must be UTF-8 encoded HTML.
Example: echo ""<h1>Hello</h1>"" | html2md");
            Console.Out.Flush();
        }
    }
}