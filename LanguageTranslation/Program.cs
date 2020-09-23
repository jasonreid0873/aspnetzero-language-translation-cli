using Google.Cloud.Translation.V2;
using System;
using System.IO;
using System.Xml;
using CommandLine;

namespace LanguageTranslation
{
    class Program
    {
        private static XmlDocument doc = new XmlDocument();

        public class Options
        {
            [Option('l', "targetLanguage", Required = true, HelpText = "Target language for translation (fr = french, etc.")]
            public string TargetLanguage { get; set; }

            [Option('t', "targetFilename", Required = false, HelpText = "Target filename (Default = <sourceFilename>-<targetLanguage> | example portal-fr.xml")]
            public string TargetFilename { get; set; }

            [Option('s', "sourceLanguage", Required = false, HelpText = "Source filename (Default = portal.xml")]
            public string SourceLanguage { get; set; } = "en";

            [Option('t', "sourceFilename", Required = false, HelpText = "Source language (Defaults to en = english)")]
            public string SourceFilename { get; set; } = "portal.xml";
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                string targetLanguage = o.TargetLanguage;
                string sourceFile = o.SourceFilename;
                string sourceLanguage = o.SourceLanguage;
                string targetFile = o.TargetFilename;
                if (targetFile == null) { targetFile = $"{sourceFile.Split(".")[0]}-{targetLanguage}.xml"; }

                Console.WriteLine($"Starting translation of {sourceFile} to language code {targetLanguage}...");
                XmlElement root = LoadXml(sourceFile);
                if (root != null)
                {
                    TranslateXml(root, sourceLanguage, targetLanguage, targetFile);
                }
                Console.WriteLine($"Translation completed and saved as {targetFile}");
            });
        }

        static XmlElement LoadXml(string sourceFile)
        {
            try
            {
                string path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "xml");
                string fileName = Path.Combine(path, sourceFile);
                doc.Load(fileName);
                return doc.DocumentElement;
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION!!!");
                Console.WriteLine(ex);
                return null;
            }
        }
        
        static void TranslateXml(XmlElement root, string sourceLanguage, string targetLanguage, string targetFile)
        {
            try
            {
                XmlNode texts = root.SelectSingleNode("texts");
                foreach (XmlElement text in texts.ChildNodes)
                {
                    string translation = GoogleTranslate(text.InnerText, sourceLanguage, targetLanguage);
                    if (translation != null)
                    {
                        Console.WriteLine($"{text.InnerText} = {translation}");
                        text.InnerText = translation;
                    }
                }

                SaveXml(targetFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION!!!");
                Console.WriteLine(ex);
            }
        }

        static void SaveXml(string targetFile)
        {
            try
            {
                string path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "xml");
                string fileName = Path.Combine(path, targetFile);
                doc.Save(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION!!!");
                Console.WriteLine(ex);
            }
        }

        static string GoogleTranslate(string text, string sourceLanguage, string targetLanguage)
        {
            // Ensure you have created a service account and enabled the Google Translation API
            // https://cloud.google.com/translate/docs/setup
            // Ensure you have set the environment variable GOOGLE_APPLICATION_CREDENTIALS to point to your download service account key JSON file
            // https://cloud.google.com/docs/authentication/getting-started
            try
            {
                TranslationClient client = TranslationClient.Create();
                TranslationResult result = client.TranslateText(text, targetLanguage, sourceLanguage);
                return result.TranslatedText;
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION!!!");
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
